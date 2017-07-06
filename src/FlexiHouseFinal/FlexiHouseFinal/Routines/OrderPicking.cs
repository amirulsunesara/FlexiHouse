using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlexiHouseFinal.Models;
using FlexihouseRoutinesTest;
using FlexiHouseFinal.Routines;
using BusinessLayer;
using System.Web.Script.Serialization;
namespace FlexiHouseFinal.Routines
{
    public class OrderPicking
    {
        List<item_Order> io = new List<item_Order>();
        List<item_Order> firstTrip = new List<item_Order>();
        List<item_Order> CopyfirstTrip = new List<item_Order>();
        List<item_Order> CopyfirstTrip2 = new List<item_Order>();
        List<Instructions> refinedList = new List<Instructions>();
        Instructions instructionMax = new Instructions(0, 0, 0, "", 0);
        List<Instructions> dispatchInstructions = new List<Instructions>();
        public List<string> shelfRetrieval = new List<string>();
        public void generateOrderIntruction(Order order)
        {
            List<String> instruction = new List<string>();
            JavaScriptSerializer jsonSerialiser = new JavaScriptSerializer();
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            OrderBusinessLayer wbl = new OrderBusinessLayer();
            string instructionString = "";
            /**
             * 
             * Go to Shelf "shelf id" and pick "item name" from slots "start slot" to "end slot" and send them to depot
             **/
            foreach (var inst in dispatchInstructions.Distinct())
            {
                Shelf shelf = wdb.Shelves.Find(inst.shelfid);

                string[] shelveName = shelf.shelfName.Split('s');
                string shelves = "s" + shelveName[1];
                shelfRetrieval.Add(shelves);

                instructionString = "Go to shelf " + shelves + " and pick " + inst.itemname + " from slots " + inst.startslot + " to " + inst.endslot + " and send them to depot";
                instruction.Add(instructionString);

               
           

                Slotting s = new Slotting();
              
                List<ShelfItems> shelfItems = s.covert_to_object(shelf.shelfItems);
                List<ShelfItems> copyShelfItems = new List<ShelfItems>();
                copyShelfItems.AddRange(shelfItems);

                bool with = false;
                int i = 0;
                foreach (ShelfItems shelve in shelfItems)
                {
                    if (shelve.slot_id == inst.startslot || with==true )
                    {
                        with = true;
                        copyShelfItems[i].item_id = -1;
                        copyShelfItems[i].status = 0;
                        copyShelfItems[i].item_name = "";
                        copyShelfItems[i].expiry_date = "";


                        if (shelve.slot_id == inst.endslot)
                        {
                            with = false;
                        }

                    }
                    i++;
                }


                string updatedString = jsonSerialiser.Serialize(copyShelfItems);
                wbl.updateShelf(inst.shelfid, updatedString);
            }
       
         
           
            string json = jsonSerialiser.Serialize(instruction);
            
            int maxDispatchId = 1;
            try
            {
                maxDispatchId = wdb.Orders.Max(a => a.dispatchNo).Value+1;

            }
            catch(Exception ex)
            {

                maxDispatchId = 1;
            }

         
            wbl.updateOrder(order.orderId,json,maxDispatchId);
            WarehouseDBEntities wde = new WarehouseDBEntities();
            Order orders = wde.Orders.Find(order.orderId);
            orders.shelfRetrieval = jsonSerialiser.Serialize(shelfRetrieval.Distinct().ToList());
            wde.Entry(orders).State = System.Data.Entity.EntityState.Modified;
            wde.SaveChanges(); 



        }


        public void performAction()
        {

            CopyfirstTrip.AddRange(firstTrip);

            foreach (var ins in refinedList)
            {

                foreach (var trip in firstTrip)
                {
                    if (ins.itemid == trip.itemId)
                    {
                        if ((ins.endslot - ins.startslot + 1) == trip.quantity)
                        {
                            Instructions instructions = new Instructions();
                            instructions.shelfid = ins.shelfid;
                            instructions.itemid = ins.itemid;
                            instructions.itemname = ins.itemname;
                            instructions.startslot = ins.startslot;
                            instructions.endslot = ins.startslot + (int)trip.quantity - 1;
                            dispatchInstructions.Add(instructions);
                            CopyfirstTrip.Remove(trip);
                        }

                    }


                }



            }
            CopyfirstTrip2.AddRange(CopyfirstTrip);
            foreach (var ins in refinedList)
            {

                foreach (var trip in CopyfirstTrip)
                {
                    if (ins.itemid == trip.itemId)
                    {
                        if ((ins.endslot - ins.startslot + 1) > trip.quantity + 5)
                        {
                            Instructions instructions = new Instructions();
                            instructions.shelfid = ins.shelfid;
                            instructions.itemid = ins.itemid;
                            instructions.itemname = ins.itemname;
                            instructions.startslot = ins.startslot;
                            instructions.endslot = ins.startslot + (int)trip.quantity - 1;
                            dispatchInstructions.Add(instructions);
                            CopyfirstTrip2.Remove(trip);


                        }

                    }


                }


            }





            foreach (var ins in refinedList)
            {

                foreach (var trip in CopyfirstTrip2)
                {
                    if (ins.itemid == trip.itemId)
                    {
                        if ((ins.endslot - ins.startslot + 1) >= trip.quantity)
                        {
                            Instructions instructions = new Instructions();
                            instructions.shelfid = ins.shelfid;
                            instructions.itemid = ins.itemid;
                            instructions.itemname = ins.itemname;
                            instructions.startslot = ins.startslot;
                            instructions.endslot = ins.startslot + (int)trip.quantity - 1;
                            dispatchInstructions.Add(instructions);
                            CopyfirstTrip.Remove(trip);
                        }

                    }

                }
            }
        }
                


            
        public void getFirstOrder(List<Order> orders, Dictionary<int, List<ShelfItems>> shelfItems)
        {

            //if (sitems[x].item_id == -1 && startcounted == false)
            //{
            //    startindex = sitems[x].slot_id;
            //    startcounted = true;
            //}

            Order od = new Order();
            Instructions shelfPick = new Instructions();
            List<Instructions> listShelfPick = new List<Instructions>();
            //list with only items that are ordered
            List<ShelfItems> si = new List<ShelfItems>();
            bool startcounted = false;

            int currentId;
            string currentItemName;
            int currentStartSlot;
            int currentEndSlot;
            foreach (var shelf in shelfItems)
            {
                shelfPick = new Instructions();
                si.Clear();
                si.AddRange(shelf.Value);
                for (int y = 0; y < si.Count; y++)
                {
                    if (si[y].item_id != -1 && startcounted == false)
                    {
                        //currentId = x.item_id;
                        shelfPick = new Instructions();
                        shelfPick.shelfid = shelf.Key;
                        shelfPick.itemid = si[y].item_id;
                        shelfPick.itemname = si[y].item_name;
                        shelfPick.startslot = si[y].slot_id;
                        if (listShelfPick.Where(a => a.itemid == shelfPick.itemid && a.startslot == shelfPick.startslot && a.shelfid == shelf.Key).ToList().Count == 0)
                        {
                            listShelfPick.Add(shelfPick);
                            startcounted = true;
                        }
                        //if(listShelfPick.Contains(shelfPick  
                        //TODO: Insert Condition so that no duplication happens


                    }
                    if (si[y].item_id != shelfPick.itemid && startcounted == true)
                    {
                        shelfPick.endslot = si[y].slot_id - 1;
                        startcounted = false;
                        listShelfPick[listShelfPick.Count - 1].endslot = shelfPick.endslot;
                        y = y - 1;
                        //end slot - start slot + = quantity
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            List<item_Order> itemOrders = new List<item_Order>();
            foreach (var order in orders)
            {
                itemOrders.AddRange(order.item_Order);
                foreach (var items in itemOrders)
                {
                    List<Instructions> instr = listShelfPick.Where(a => a.itemid == items.itemId).Distinct().ToList();
                    if (instr.Count > 0)
                    {
                        refinedList.AddRange(instr);

                    }

                }
            }


            int tripQuantity = 0;
            foreach (var order in orders)
            {
                if (order.totalOrderQuanitity > 24)
                {
                    io.AddRange(order.item_Order);
                    foreach (var a in io)
                    {
                        tripQuantity += (int)a.quantity;
                        while (tripQuantity <= 24)
                        {
                            firstTrip.Add(a);
                        }

                    }

                    performAction();



                }
                else if (order.totalOrderQuanitity <=24)
                {
                    io.AddRange(order.item_Order);
                    firstTrip.AddRange(io);
                    performAction();
                    generateOrderIntruction(order);



                }

            }
        }





        }







    }



