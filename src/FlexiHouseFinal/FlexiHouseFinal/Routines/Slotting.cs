using FlexiHouseFinal.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using System.Web.Script.Serialization;
using FlexiHouseFinal.Routines;

namespace FlexihouseRoutinesTest
{
   public class Slotting
    {
        // ArrayList shelfs = new ArrayList();
        // Shelf sf = new Shelf();
        List<ItemsSlotter> itemsA = new List<ItemsSlotter>();
        List<ItemsSlotter> itemsB = new List<ItemsSlotter>();
        List<ItemsSlotter> itemsC = new List<ItemsSlotter>();


        public List<Instructions> instructions = new List<Instructions>();
        public List<string> instructionsList = new List<string>();
        public List<string> shelfInserted = new List<string>();
        //TODO: Get no of shelfs

        //TODO: Write the general case of slotting(IF there is required space available then just slot)
        //public List<ItemsSlotter> getItems()
        //{
        //    //This method is only for testing, after integration this data will be acquired from item_warehouse table in the database
        //    List<ItemsSlotter> items = new List<ItemsSlotter>();
        //    items.Add(new ItemsSlotter("nestle", 5, 80));
        //    items.Add(new ItemsSlotter("milo", 2, 60));
        //    items.Add(new ItemsSlotter("pepsi", 20, 95));
        //    items.Add(new ItemsSlotter("siemens", 50, 50));
        //    items.Add(new ItemsSlotter("nesvita", 8, 92));

        //    return items;

        //}

        //public List<Shelf> getShelfs(int warehouseId)
        //{
        //    //this method is just for testing, when integrated it should get shelfs from database and sort them according to their zone
        //    int countA = 0;
        //    int countB = 0;
        //    List<Shelf> sflist = new List<Shelf>();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        if (countA < 1)
        //        {
        //            sflist.Add(new Shelf(i + 1, 'A'));
        //            countA++;
        //        }
        //        else if (countB < 2)
        //        {
        //            sflist.Add(new Shelf(i + 1, 'B'));
        //            countB++;
        //        }
        //        else
        //        {
        //            sflist.Add(new Shelf(i + 1, 'C'));
        //        }
        //    }

        //    return sflist;
        //}

        public void partitioningItems(List<ItemsSlotter> itemslotter)
        {
            ItemsSlotter pi;
            //This will get a list of items and their values
            List<ItemsSlotter> items = itemslotter;
            foreach (ItemsSlotter i in items)
            {

                if (i.orders >= 90)
                {
                    itemsA.Add(i);
                }
                else if (i.orders >= 70 && i.orders < 90)
                {
                    itemsB.Add(i);
                }
                else
                {
                    itemsC.Add(i);
                }
            }

        }

        public int[,] getEmptySlots(List<Shelf> shelflist)
        {
            int[,] slotslist = new int[100, 4]; //will contain shelfid, zone , start count and end count
            int startindex = -1;
            int endindex = -1;
            int i = 0;
            bool startcounted = false;
            foreach (Shelf shelf in shelflist)     //this loop creates a list of successive empty slots on each shelf
            {
                startcounted = false;
                List<ShelfItems> sitems = covert_to_object(shelf.shelfItems);
                //TODO: Get shelf items and generate a list of empty slots
                for(int x=0; x<sitems.Count; x++ )
                {
                    if (sitems[x].item_id == -1 && startcounted == false)
                    {
                        startindex = sitems[x].slot_id;
                        startcounted = true;
                    }
                    else if ((sitems[x].item_id != -1 && startcounted == true) || sitems[x].slot_id == sitems.Count)
                    {
                        endindex = sitems[x-1].slot_id;
                        slotslist[i, 0] = shelf.id;
                        try
                        {
                            if (shelf.zone.Equals("A"))
                            {
                                slotslist[i, 1] = 65;
                            }
                            else if (shelf.zone.Equals("B"))
                            {
                                slotslist[i, 1] = 66;

                            }
                            else if (shelf.zone.Equals("C"))
                            {
                                slotslist[i, 1] = 67;

                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        slotslist[i, 2] = startindex;
                        slotslist[i, 3] = endindex;
                        // Console.WriteLine("id: " + slotslist[i, 0] + "\nzone: " + (char)slotslist[i, 1] + "\nstart:" + slotslist[i, 2] + "\nend: " + slotslist[i, 3]);
                        i = i + 1;
                        startcounted = false;
                    }
                    else
                    {
                       
                        continue;
                        
                    }
                }

            }
            return slotslist;
        }
        public List<ShelfItems> covert_to_object(string json)
        {

            List<ShelfItems> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ShelfItems>>(json);
           


            return items;
        }

        public List<String> string_to_object(string json)
        {

            List<String> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<String>>(json);



            return items;
        }
        public bool slotting(List<Shelf> shelflist, List<ItemsSlotter> itemslotter)
        {
            bool errorA;
            bool errorB;
            bool errorC;
            partitioningItems(itemslotter);
            //List<Shelf> shelflist = getShelfs();
            int[,] slotslist = getEmptySlots(shelflist);
            errorA = slottingProcedure(slotslist, itemsA, 65);
            errorB = slottingProcedure(slotslist, itemsB, 66);
            errorC = slottingProcedure(slotslist, itemsC, 67);
            if (errorA | errorB | errorC)
            {
                return true;
            }
            else
            {
                generateInstructions(shelflist);
                return false;
            }
            return false;
        }
        // List<ItemsSlotter> itemcopy = new List<ItemsSlotter>();
        //List<ItemsSlotter> itemscopy2 = new List<ItemsSlotter>();
        //int totalweight = 0;
        //int slotspace;
        //int start = -1;
        //int end = -1;
        //bool error = false;
        //int j = -1;
        ////for (int x = 0; x < slotslist.GetLength(0); x++)
        ////{
        ////    if (slotslist[x, 1] == 65)
        ////    {
        ////        j = x;
        ////        break;
        ////    }
        ////}

        ////foreach (var item in itemsA)
        ////{
        ////    itemcopy.Add(item);
        ////    totalweight = totalweight + item.weight;
        ////}
        ////for (int i = j; i < slotslist.GetLength(0); i++)
        ////{
        ////    slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
        ////    start = slotslist[i, 2];
        ////    foreach (var item in itemsA)
        ////    {

        ////        if (totalweight <= slotspace && slotslist[i, 1] == 65 && itemcopy.Count > 0)
        ////        {
        ////            end = start + item.weight - 1;
        ////            instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        ////            start = end + 1;
        ////            itemcopy.Remove(item);
        ////            slotslist[i, 2] = end + 1;
        ////        }

        ////        //else if(totalweight <= slotspace && itemcopy.Count > 0)
        ////        //{
        ////        //    end = start + item.weight - 1;
        ////        //    instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        ////        //    start = end + 1;
        ////        //    itemcopy.Remove(item);
        ////        //    slotslist[i, 2] = end + 1;
        ////        //}

        ////        else if (slotslist[i, 1] != 65)
        ////        {
        ////            break;
        ////        }
        ////        else if (itemcopy.Count == 0)
        ////        {
        ////            break;
        ////        }
        ////    }

        ////}


        ////itemscopy2.AddRange(itemcopy);
        ////if (itemcopy.Count != 0)
        ////{
        ////    totalweight = 0;
        ////    List<ItemsSlotter> itemsremaining = new List<ItemsSlotter>();
        ////    int shelfid = -1;
        ////    int maxslotspace = 0;
        ////    bool remaining = true;
        ////    int ind = -1;
        ////    while (remaining == true)
        ////    {
        ////        foreach (var item in itemscopy2)
        ////        {

        ////            totalweight = totalweight + item.weight;
        ////        }

        ////        for (int i = j; i < slotslist.GetLength(0); i++)
        ////        {
        ////            slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
        ////            if (slotslist[i, 1] == 65 && slotspace > maxslotspace)
        ////            {
        ////                maxslotspace = slotspace;
        ////                start = slotslist[i, 2];
        ////                shelfid = slotslist[i, 0];
        ////                ind = i;
        ////            }
        ////        }
        ////        if (maxslotspace > 0)
        ////        {
        ////            itemsremaining.Clear();
        ////            itemsremaining.AddRange(knapsack(maxslotspace, ref start, shelfid, itemscopy2));//send items list    
        ////        }
        ////        else
        ////        {
        ////            break;
        ////        }
        ////        if (itemsremaining.Count == 0)
        ////        {
        ////            remaining = false;

        ////        }
        ////        else
        ////        {
        ////            itemscopy2.Clear();
        ////            itemscopy2.AddRange(itemsremaining);
        ////            maxslotspace = 0;
        ////            slotslist[ind, 2] = start;
        ////            shelfid = -1;
        ////        }
        ////    }
        ////    itemcopy.Clear();
        ////    itemcopy.AddRange(itemsremaining);
        ////}


        ////    itemscopy2.Clear();
        ////    itemscopy2.AddRange(itemcopy);
        ////    if (itemcopy.Count != 0)
        ////    {
        ////        totalweight = 0;
        ////        for (int i = j; i < slotslist.GetLength(0); i++)
        ////        {
        ////            slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
        ////            start = slotslist[i, 2];
        ////            if(slotspace == 0)
        ////            {
        ////                continue;
        ////            }
        ////            else if (totalweight <= slotspace)
        ////            {
        ////                foreach (var item in itemscopy2)
        ////                {
        ////                    end = start + item.weight - 1;
        ////                    instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        ////                    start = end + 1;
        ////                    itemcopy.Remove(item);
        ////                    slotslist[i, 2] = end + 1;
        ////                }

        ////            }
        ////            if (itemcopy.Count == 0)
        ////            {
        ////                break;
        ////            }


        ////        }

        ////    }
        ////    else if (itemcopy.Count != 0 && error == false)
        ////    {
        ////        Console.Write("No space in warehouse");
        ////        error = true;
        ////    }
        ////    itemcopy.Clear();
        //    //uptill here logic is correct
        //    ///////////////////////////////////////////////////////////////////////////////

        //    totalweight = 0;
        //    start = -1;
        //    end = -1;
        //    for (int x = 0; x < slotslist.GetLength(0); x++)
        //    {
        //        if (slotslist[x, 1] == 66)
        //        {
        //            j = x;
        //            break;
        //        }
        //    }

        //    foreach (var item in itemsB)
        //    {
        //        itemcopy.Add(item);
        //        totalweight = totalweight + item.weight;
        //        //Console.WriteLine(totalweight);
        //    }

        //    for (int i = j; i < slotslist.GetLength(0); i++)
        //    {
        //        slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
        //        start = slotslist[i, 2];
        //        foreach (var item in itemsB)
        //        {

        //            if (totalweight <= slotspace && slotslist[i, 1] == 66 && itemcopy.Count > 0)
        //            {

        //                end = start + item.weight - 1;
        //                instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        //                start = end + 1;
        //                itemcopy.Remove(item);
        //                slotslist[i, 2] = end + 1;
        //            }
        //            else if (totalweight >= slotspace && slotslist[i, 1] == 66 && itemcopy.Count > 0)
        //            {
        //                //Call knapsack
        //            }
        //            else if (totalweight <= slotspace && itemcopy.Count > 0)
        //            {
        //                end = start + item.weight - 1;
        //                instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        //                start = end + 1;
        //                itemcopy.Remove(item);
        //                slotslist[i, 2] = end + 1;
        //            }
        //        }
        //        if (itemcopy.Count != 0 && error == false)
        //        {

        //            Console.WriteLine("No space in warehouse");
        //            error = true;
        //        }
        //        else if (itemcopy.Count == 0)
        //        {

        //            break;
        //        }
        //    }

        //    itemcopy.Clear();


        //    totalweight = 0;
        //    start = -1;
        //    end = -1;
        //    for (int x = 0; x < slotslist.GetLength(0); x++)
        //    {
        //        if (slotslist[x, 1] == 67)
        //        {
        //            j = x;

        //            break;
        //        }
        //    }

        //    foreach (var item in itemsC)
        //    {
        //        itemcopy.Add(item);
        //        totalweight = totalweight + item.weight;
        //        //Console.WriteLine(totalweight);
        //    }

        //    for (int i = j; i < slotslist.GetLength(0); i++)
        //    {
        //        slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
        //        start = slotslist[i, 2];
        //        foreach (var item in itemsC)
        //        {

        //            if (totalweight <= slotspace && slotslist[i, 1] == 67 && itemcopy.Count > 0)
        //            {

        //                end = start + item.weight - 1;
        //                instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        //                start = end + 1;
        //                itemcopy.Remove(item);
        //                slotslist[i, 2] = end + 1;
        //            }
        //            else if (totalweight >= slotspace && slotslist[i, 1] == 67 && itemcopy.Count > 0)
        //            {
        //                //Call knapsack
        //            }

        //        }
        //        if (itemcopy.Count != 0 && error == false)
        //        {

        //            Console.WriteLine("No space in warehouse");
        //            error = true;
        //        }
        //        else if (itemcopy.Count == 0)
        //        {
        //            break;
        //        }
        //    }

        //    //slottingA(slotslist, itemsA);
        //    //slottingB(slotslist, itemsB);
        //    //slottingC(slotslist, itemsC);
        //    printInstructions();
        //    //TODO: Make sure to eliminate any conflicting instructions, either by checking when they are added or by updating slotslist
        //}


        public bool slottingProcedure(int[,] slotslist, List<ItemsSlotter> items, int zone)
        {
            List<ItemsSlotter> itemcopy = new List<ItemsSlotter>();
            List<ItemsSlotter> itemscopy2 = new List<ItemsSlotter>();
            int totalweight = 0;
            int slotspace;
            int start = -1;
            int end = -1;
            bool error = false;
            int j = -1;

            for (int x = 0; x < slotslist.GetLength(0); x++)
            {
                if (slotslist[x, 1] == zone)
                {
                    j = x;
                    break;
                }
            }

            foreach (var item in items)
            {
                itemcopy.Add(item);
                totalweight = totalweight + (int)item.quantity;
            }
            for (int i = j; i < slotslist.GetLength(0); i++)
            {
                slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
                start = slotslist[i, 2];
                foreach (var item in items)
                {

                    if (totalweight <= slotspace && slotslist[i, 1] == zone && itemcopy.Count > 0)
                    {
                        end = start + (int)item.quantity - 1;
                        instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name,item.item_id));
                        start = end + 1;
                        itemcopy.Remove(item);
                        slotslist[i, 2] = end + 1;
                    }

                    else if (slotslist[i, 1] != zone)
                    {
                        break;
                    }
                    else if (itemcopy.Count == 0)
                    {
                        break;
                    }
                }

            }


            itemscopy2.AddRange(itemcopy);
            if (itemcopy.Count != 0)
            {
                totalweight = 0;
                List<ItemsSlotter> itemsremaining = new List<ItemsSlotter>();
                int shelfid = -1;
                int maxslotspace = 0;
                bool remaining = true;
                int ind = -1;
                while (remaining == true)
                {
                    foreach (var item in itemscopy2)
                    {

                        totalweight = totalweight + (int)item.quantity;
                    }

                    for (int i = j; i < slotslist.GetLength(0); i++)
                    {
                        slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
                        if (slotslist[i, 1] == zone && slotspace > maxslotspace)
                        {
                            maxslotspace = slotspace;
                            start = slotslist[i, 2];
                            shelfid = slotslist[i, 0];
                            ind = i;
                        }
                    }
                    if (maxslotspace > 0)
                    {
                        itemsremaining.Clear();
                        itemsremaining.AddRange(knapsack(maxslotspace, ref start, shelfid, itemscopy2));//send items list    
                    }
                    else
                    {
                        break;
                    }
                    if (itemsremaining.Count == 0)
                    {
                        remaining = false;

                    }
                    else
                    {
                        itemscopy2.Clear();
                        itemscopy2.AddRange(itemsremaining);
                        maxslotspace = 0;
                        slotslist[ind, 2] = start;
                        shelfid = -1;
                    }
                }
                itemcopy.Clear();
                itemcopy.AddRange(itemsremaining);
            }


            itemscopy2.Clear();
            itemscopy2.AddRange(itemcopy);
            if (itemcopy.Count != 0)
            {
                totalweight = 0;
                foreach (var item in itemscopy2)
                {
                    itemcopy.Add(item);
                    totalweight = totalweight + (int)item.quantity;
                }
                for (int i = j; i < slotslist.GetLength(0); i++)
                {
                    slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
                    start = slotslist[i, 2];
                    if (slotspace == 0)
                    {
                        if (i == slotslist.GetLength(0) - 1)
                        {
                            Console.Write("No space in warehouse");
                            return true;

                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (totalweight <= slotspace)
                    {
                        foreach (var item in itemscopy2)
                        {
                            end = start + (int)item.quantity - 1;
                            instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name,item.item_id));
                            start = end + 1;
                            itemcopy.Remove(item);
                            slotslist[i, 2] = end + 1;
                        }

                    }
                    else if (totalweight > slotspace && i == slotslist.GetLength(0) - 1)
                    {
                        Console.Write("No space in warehouse");
                        return true;
                    }
                    if (itemcopy.Count == 0)
                    {
                        break;
                    }
                    //else
                    //{
                    //    error = true;

                    //}


                }

            }
            else if (itemcopy.Count != 0 && error == false)
            {
                Console.Write("No space in warehouse");
                return true;
            }
            itemcopy.Clear();
            return false;
        }








        //public void slottingA(int[,] slotslist, List<ItemsSlotter> itemsA)
        //{
        //    // partitioningItems();
        //    // List<Shelf> shelflist = getShelfs();
        //    // int[,] slotslist = getEmptySlots(shelflist);
        //    int slotspace;
        //    int totalweight = 0;
        //    int start;
        //    int end = -1;
        //    foreach (ItemsSlotter item in itemsA)
        //    {
        //        totalweight = totalweight + item.weight;

        //    }
        //    for (int i = 0; i < slotslist.Length / 4; i++)
        //    {
        //        slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
        //        if (totalweight <= slotspace && slotslist[i, 1] == 65)
        //        {

        //            start = slotslist[i, 2];
        //            foreach (ItemsSlotter item in itemsA)
        //            {
        //                Console.Write("First");
        //                end = start + item.weight - 1;
        //                instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        //                start = end + 1;

        //            }






        //        }

        //        else if (totalweight > slotspace && slotslist[i, 1] == 65)
        //        {
        //            //Call knapsack here
        //        }





        //    }
        //}
        //public void slottingB(int[,] slotslist, List<ItemsSlotter> itemsB)
        //{
        //    //partitioningItems();
        //    //List<Shelf> shelflist = getShelfs();
        //    //int[,] slotslist = getEmptySlots(shelflist);
        //    int slotspace;
        //    int totalweight = 0;
        //    int start;
        //    int end = -1;
        //    foreach (ItemsSlotter item in itemsB)
        //    {
        //        totalweight = totalweight + item.weight;
        //    }
        //    for (int i = 0; i < slotslist.Length / 4; i++)
        //    {
        //        slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
        //        if (totalweight <= slotspace && slotslist[i, 1] == 66)
        //        {

        //            start = slotslist[i, 2];
        //            foreach (ItemsSlotter item in itemsB)
        //            {
        //                if (!instructions.Exists(ins => ins.itemname.Equals(item.item_name)))
        //                {
        //                    Console.Write("First B");
        //                    end = start + item.weight - 1;
        //                    instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        //                    start = end + 1;

        //                }
        //                else
        //                {

        //                }
        //            }
        //            slotslist[i, 2] = end + 1;


        //        }
        //        else if (totalweight > slotspace && slotslist[i, 1] == 66)
        //        {
        //            //Call knapsack here
        //        }
        //        //else if (totalweight <= slotspace)
        //        //{

        //        //    start = slotslist[i, 2];
        //        //    foreach (ItemsSlotter item in itemsB)
        //        //    {
        //        //        if (instructions.Exists(ins => ins.itemname == item.item_name))
        //        //        {


        //        //        }
        //        //        else
        //        //        {
        //        //            end = start + item.weight - 1;
        //        //            instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        //        //            start = end + 1;
        //        //        }
        //        //    }
        //        //    slotslist[i, 2] = end + 1;

        //        //}
        //        else
        //        {
        //            // Console.WriteLine("No space in warehouse, items cannot be slotted");
        //        }

        //    }







        //}
        //public void slottingC(int[,] slotslist, List<ItemsSlotter> itemsC)
        //{
        //    //partitioningItems();
        //    //List<Shelf> shelflist = getShelfs();
        //    //int[,] slotslist = getEmptySlots(shelflist);
        //    int slotspace;
        //    int totalweight = 0;
        //    int start;
        //    int end = -1;
        //    foreach (ItemsSlotter item in itemsC)
        //    {
        //        totalweight = totalweight + item.weight;
        //    }
        //    for (int i = 0; i < slotslist.Length / 4; i++)
        //    {
        //        slotspace = slotslist[i, 3] - slotslist[i, 2] + 1;
        //        if (totalweight <= slotspace && (char)slotslist[i, 1] == 'C')
        //        {
        //            start = slotslist[i, 2];
        //            foreach (ItemsSlotter item in itemsC)
        //            {
        //                if (instructions.Exists(ins => ins.itemname == item.item_name))
        //                {


        //                }
        //                else
        //                {
        //                    end = start + item.weight - 1;
        //                    instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        //                    start = end + 1;
        //                }
        //            }
        //            slotslist[i, 2] = end + 1;


        //        }
        //        else if (totalweight > slotspace && (char)slotslist[i, 1] == 'C')
        //        {
        //            //Call knapsack here
        //        }
        //        //else if (totalweight <= slotspace)
        //        //{
        //        //    start = slotslist[i, 2];
        //        //    foreach (ItemsSlotter item in itemsC)
        //        //    {
        //        //        if (instructions.Exists(ins => ins.itemname == item.item_name))
        //        //        {


        //        //        }
        //        //        else
        //        //        {
        //        //            end = start + item.weight - 1;
        //        //            instructions.Add(new Instructions(slotslist[i, 0], start, end, item.item_name));
        //        //            start = end + 1;
        //        //        }
        //        //    }
        //        //    slotslist[i, 2] = end + 1;

        //        //}
        //        else
        //        {
        //            //  Console.WriteLine("No space in warehouse, items cannot be slotted");
        //        }

        //    }






        //}



        //public void knapsack(int capacity, int[] weight, double[] value, String[] item)
        //{
        //    int count = 0;//count for item no.
        //    int remaining = capacity; //space in knapsack remaining
        //    int current_weight = 0; //selected weight
        //    double current_value = 0.0; //selected value
        //    String current_item; //selected item
        //    List<ItemsSlotter> selected_items = new List<ItemsSlotter>();
        //    ItemsSlotter items = new ItemsSlotter();
        //    //sorting weight and value according to weight and value ratio
        //    double[] x = Enumerable.Repeat(0.0, weight.Length).ToArray();
        //    for (int i = 0; i <= weight.Length - 1; i++)
        //    {

        //        for (int j = 0; j <= weight.Length - 2; j++)
        //        {
        //            if ((weight[i] / value[i]) < (weight[j] / value[j]))
        //            {
        //                int temp1 = weight[i];
        //                weight[i] = weight[j];
        //                weight[j] = temp1;
        //                double temp2 = value[i];
        //                value[i] = value[j];
        //                value[j] = temp2;
        //            }
        //        }
        //    }

        //    for (count = 0; count < item.Length; count++)
        //    {
        //        current_weight = weight[count];
        //        current_value = value[count];
        //        current_item = item[count];
        //        if (current_weight > remaining)
        //        {
        //            break;
        //        }
        //        else
        //        {

        //            x[count] = 1.0;
        //            items.weight = current_weight;
        //            items.value = current_value;
        //            items.item_name = current_item;
        //            items.factor = x[count];
        //            selected_items.Add(items);
        //            remaining = remaining - current_weight;
        //        }

        //    }

        //    if (count < item.Length)
        //    {
        //        x[count] = remaining / weight[count];
        //        items.weight = weight[count];
        //        items.value = value[count];
        //        items.item_name = item[count];
        //        items.factor = x[count];
        //        selected_items.Add(items);
        //    }




        //}


        public List<ItemsSlotter> knapsack(int slotspace, ref int start, int shelfid, List<ItemsSlotter> items)
        {
            int count = 0;//count for item no.
            int remaining = slotspace; //space in knapsack remaining
            int current_weight = 0; //selected weight
            double current_value = 0.0; //selected value
            String current_item; //selected item
            List<ItemsSlotter> itemscopy = new List<ItemsSlotter>();

            List<ItemsSlotter> itemsremaining = new List<ItemsSlotter>();
            //ItemsSlotter items = new ItemsSlotter();
            //sorting weight and value according to weight and value ratio
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 1; j <= items.Count - 2; j++)
                {
                    if (items[i].orders / items[i].quantity < items[j].orders / items[j].quantity)
                    {
                        ItemsSlotter tmp = items[j];
                        items[j] = items[i];
                        items[i] = items[j];
                    }
                }
            }
            itemscopy.AddRange(items);
            foreach (var item in items)
            {

                current_weight = item.quantity;
                current_value = item.orders;
                current_item = item.item_name;
                if (current_weight > remaining)
                {
                    break;

                }
                else
                {
                    //selected_items.Add(new ItemsSlotter(current_item,current_weight,current_value,1));
                    instructions.Add(new Instructions(shelfid, start, start + current_weight - 1, current_item,item.item_id));
                    itemscopy.Remove(item);
                    start = start + current_weight;
                    remaining = remaining - current_weight;
                }
            }

            if (itemscopy.Count != 0 && remaining > 0)
            {
                foreach (var item in itemscopy)
                {
                    if (remaining > 0)
                    {
                        current_weight = remaining;
                        current_item = item.item_name;
                        instructions.Add(new Instructions(shelfid, start, start + current_weight - 1, current_item,item.item_id));
                        itemsremaining.Add(new ItemsSlotter(item.item_id, item.quantity - remaining, item.item_name,  item.orders));
                        start = start + current_weight;
                        remaining = remaining - current_weight;
                    }

                }
            }

            return itemsremaining;






        }


        public void generateInstructions(List<Shelf> shelfList)
        {
            bool shelffound = false;
            string shelfName="-1";
            List<Shelfandshelfitems> shelfandshelfitems = new List<Shelfandshelfitems>();
            List<ShelfItems> sitemsList = new List<ShelfItems>();
            Shelfandshelfitems ssi = new Shelfandshelfitems();
            ArrayList al = new ArrayList();
            foreach (var shelf in shelfList)
            {
                for(int i = 0; i<instructions.Count; i++)
                {
                    if (instructions[i].shelfid == shelf.id)
                    {
                        if (shelffound == false)
                        {
                            shelfName = shelf.shelfName;
                            sitemsList = covert_to_object(shelf.shelfItems);
                            shelffound = true;
                        }
                        for (int a = instructions[i].startslot - 1; a < instructions[i].endslot; a++)
                        {
                            sitemsList[a].item_id = instructions[i].itemid;
                            sitemsList[a].item_name = instructions[i].itemname;



                        }
                        try
                        {
                            if (instructions[i + 1].shelfid != instructions[i].shelfid)
                            {

                                ssi.shelfitemslist = sitemsList;
                                ssi.id = shelf.id;
                                shelfandshelfitems.Add(ssi);
                                shelffound = false;
                            }
                        }
                        catch(Exception e)
                        {
                            ssi.shelfitemslist = sitemsList;
                            ssi.id = shelf.id;
                            shelfandshelfitems.Add(ssi);
                            shelffound = false;
                            continue;
                        }
                        
                    }
                }
         
            }
            SlottingBusinessLayer sbl = new SlottingBusinessLayer();
            foreach (var z in shelfandshelfitems)
            {
                int shelfid = z.id;
                List<ShelfItems> si = z.shelfitemslist;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string output = jss.Serialize(si);
                sbl.updateShelfSlotting(shelfid,output);    


            }
            string oneI;
            foreach (var i in instructions)
            {
                WarehouseDBEntities wdb = new WarehouseDBEntities();
                string sName = wdb.Shelves.FirstOrDefault(s=>s.id==i.shelfid).shelfName;
                string[] shelveName = sName.Split('s');
                string shelve = "s" + shelveName[1];
                oneI = "Place " + i.itemname + " on shelf " + shelve + ", from slot " + i.startslot + " to slot " + i.endslot;

                instructionsList.Add(oneI);
                shelfInserted.Add(shelve);


            }
            JavaScriptSerializer jsss = new JavaScriptSerializer();
            string output2 = jsss.Serialize(instructionsList);

            
        }

    }
}


