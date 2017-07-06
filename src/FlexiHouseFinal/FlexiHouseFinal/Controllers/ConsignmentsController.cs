using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlexiHouseFinal.Models;
using BusinessLayer;
using System.Web.Services;
using FlexihouseRoutinesTest;
using System.Web.Script.Serialization;
using FlexiHouseFinal.Routines;

namespace FlexiHouseFinal.Controllers
{
    public class ConsignmentsController : Controller
    {
        private WarehouseDBEntities db = new WarehouseDBEntities();
        ConsignmentBusinessLayer cbl = new ConsignmentBusinessLayer();

        // GET: Consignments
        public ActionResult Index()

        {
            ViewBag.PageName = "Consignments";
            WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();

            int warehouseIdd = wbl.getWarehouseId(Convert.ToInt32(Session["UserID"]));
            return View(db.Consignments.Where(u => u.warehouseId == warehouseIdd));

        }
  
        // GET: Consignments/Details/5
        public ActionResult Details(int? id)
            {
            ViewBag.PageName = "Consignments";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consignment consignment = db.Consignments.Find(id);
            List<Item_Consignment> ic = db.Item_Consignment.Where(u => u.consignmentId == consignment.id).ToList();
            List<Item_Consignment> newIc = new List<Item_Consignment>();

            foreach (Item_Consignment i in ic)
            {
                i.Consignment = consignment;

                Item item = db.Items.Find(i.itemId);
                i.Item = item;
                newIc.Add(i);
            }
           
            ViewBag.ItemDetails = newIc;


            //Yaha se instruction ja rhi ha
            List<string> instructionList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(consignment.instruction);
            ViewBag.Instructions = instructionList;


            if (consignment == null)
            {
                return HttpNotFound();
            }
            return View(consignment);
        }

        // GET: Consignments/Create
        public ActionResult Create()
        {
            ViewBag.PageName = "Consignments";
            return View();
        }
        [HttpGet]
        public JsonResult GetConsignmentId()
        {
            List<Consignment> allUser = new List<Consignment>();

            Consignment w = new Consignment();
            try
            {
                w.id = db.Consignments.Max(u => u.id);

                allUser.Add(w);
            }
            catch(Exception ex)
            {
                w.id = 0;
                allUser.Add(w);
            }

            return new JsonResult { Data = allUser, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
   public JsonResult AutoComplete(string prefix)
        {
            
            var item= (from items in db.Items
                        where items.itemName.StartsWith(prefix)
                        select new
                        {
                           
                            label = items.itemName,
                            val = items.id,
                            itemCode = items.itemCode,
                            manufacturer = items.Manufacturer,
                            country = items.Country

                        }).ToList();

            return Json(item);
        }


        // POST: Consignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,warehouseId,supplier,totalItems,arrivalDate,consignmentName")] Consignment consignment)
        {
            //Yaha pe Manager Id add Kardena
            //Select id from Warehouse where managerId = Session[id]
            //Increase Session Timeout
            WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();

            consignment.warehouseId = wbl.getWarehouseId(Convert.ToInt32(Session["UserID"]));


            //if (ModelState.IsValid)
            //{
            //    //db.Consignments.Add(consignment);
            //    // db.SaveChanges();
            //    TempData["ConsignmentDetails"] = consignment;
            //    return RedirectToAction("De", "Consignments");
            //}

            List<String> instructionsList = new List<string>();

            ViewBag.Instructions = instructionsList;

            return RedirectToAction("Details", new { id=Convert.ToInt32(Session["ConsignmentID"])});
        }

        [HttpPost]
     
        public ActionResult Update(List<Item> ware, List<Item_Consignment> itemcon, List<Consignment> con)
        {

            foreach (Consignment conn in con)
            {

                WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
                int warehouseIdd = wbl.getWarehouseId(Convert.ToInt32(Session["UserID"]));
                conn.warehouseId = warehouseIdd;
               

                try
                {
                   
                    cbl.UpdateConsignment(conn);



                    int index = 0;
                    foreach (Item_Consignment ic in itemcon)
                    {

                        Item items = ware.ElementAt(index);
                        ic.itemId = getNewOrOldId(items);
                        ic.consignmentId = conn.id;
                        List<Item_Consignment> itc = db.Item_Consignment.Where(i => i.itemId == ic.itemId && i.consignmentId == ic.consignmentId ).ToList();
                        if (itc.Count > 0)
                        {
                            cbl.UpdateItem_Consignment(ic);
                        }
                        else
                        {
                           cbl.addItem_Consignment(ic);
                            
                        }
                        index++;
              
                    }
                 
                }
                catch (Exception ex)
                {
                   
                }

               
                int count = db.Item_Consignment.Where(u => u.consignmentId == conn.id).Count();
                cbl.updateItemsCount(count,conn.id);

                break;
            }

           
            return RedirectToAction("Create", "Consignments");
        }


        [HttpPost]
        public ActionResult DeleteItem(List<Item_Consignment> itemcon)
            {
     
        
                Item_Consignment it = itemcon.ElementAt(0);
               Item_Consignment ic = db.Item_Consignment.FirstOrDefault(i => i.consignmentId == it.consignmentId && i.itemId == it.itemId);
                db.Item_Consignment.Remove(ic);
                db.SaveChanges();
           
            
              int count = db.Item_Consignment.Where(u => u.consignmentId == ic.consignmentId).Count();
            ConsignmentBusinessLayer cbl = new ConsignmentBusinessLayer();
            cbl.updateItemsCount(count,ic.consignmentId);
            
             //Write code to update item count
            return RedirectToAction("Index", "Consignments");
        }











        [HttpPost]
        [WebMethod]
        public ActionResult Save(List<Item> ware,List<Item_Consignment> itemcon, List<Consignment> con)
        {
            int conid = -1;
            List<ItemsSlotter> isl = new List<ItemsSlotter>();
          foreach(Consignment conn in con)
            {
               
                WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
                int warehouseIdd = wbl.getWarehouseId(Convert.ToInt32(Session["UserID"]));
                conn.warehouseId = warehouseIdd;
                conn.consignmentStatus = "Added";
                try
                {
                    conid = db.Consignments.Max(u => u.id) + 1;
                }
                catch (Exception ex) { conid=1;}
                    conn.id = conid;
                    cbl.addConsignment(conn);


                    int index = 0;
                    foreach (Item_Consignment ic in itemcon)
                    {

                        Item items = ware.ElementAt(index);
                        ic.itemId = getNewOrOldId(items);

                        ItemsSlotter itemslot = new ItemsSlotter();
                    
                        itemslot.item_id= ic.itemId;
                        itemslot.quantity = (int)ic.quantity;
                     //   itemslot.expiry_date = ic.expiry.Value.ToShortDateString();

                    Item it= db.Items.FirstOrDefault(a => a.id == itemslot.item_id);
                        itemslot.item_name = it.itemName;
                       


                        bool val = db.Item_Warehouse.Any(o => o.itemId == ic.itemId && o.warehouseId==warehouseIdd);
                        if (val==true)
                        {
                            Item_Warehouse iw = db.Item_Warehouse.FirstOrDefault(a => a.itemId ==ic.itemId && a.warehouseId==warehouseIdd );
                            iw.quantity += ic.quantity;
                            cbl.updateItemWarehouse(iw);
                            itemslot.quantity = (int)iw.quantity;
                            // found = true;
                            //TODO: ADO.NET CODE
                        }
                        else
                        {
                            Item_Warehouse iw = new Item_Warehouse();
                            iw.warehouseId = warehouseIdd;
                            iw.itemId = ic.itemId;
                            iw.quantity = ic.quantity;
                            iw.orders = 0;
                            cbl.addItemWarehouse(iw);
                        itemslot.quantity = (int)iw.quantity;
                            
                        }

                        ic.consignmentId = conid;
                        cbl.addItem_Consignment(ic);

                    isl.Add(itemslot);
                    index++;

                    }
             

                //Yaha pe SLotting Horae ha
                List<Shelf> newShelf = db.Shelves.Where(a => a.warehouse_id == warehouseIdd).ToList();
                Slotting slotting = new Slotting();
              bool isError = slotting.slotting(newShelf, isl);
                if(isError)
                {

                    ViewBag.throwError = "Unfortunately, there is no space in warehouse for the new items";

                }
                List<String> instructionList = slotting.instructionsList;
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string inst = jss.Serialize(instructionList);
                SlottingBusinessLayer sbl = new SlottingBusinessLayer();
                sbl.UpdateConsignmentInstruction(conid, inst);
                List<String> shelfInserted = slotting.shelfInserted.Distinct().ToList();
                string insertedShelfJSON = jss.Serialize(shelfInserted);

                WarehouseDBEntities wdb = new WarehouseDBEntities();
                Consignment consignment = wdb.Consignments.Find(conid);
                consignment.consignmentStatus = "Added";
                consignment.shelfInserted = insertedShelfJSON;
                wdb.Entry(consignment).State = EntityState.Modified;
                wdb.SaveChanges();

                Session["consignmentID"] = conid;

                break;
            }
       

            return RedirectToAction("LoggedIn", "Account");
        }
        public int getNewOrOldId(Item it)
        {

            int newid;
            try
            {
                newid = db.Items.Max(u => u.id) + 1;
            }
            catch(Exception ex)
            {
                newid = 1;
            }

            var item = (from items in db.Items
                        where items.itemName == it.itemName
                        && items.Manufacturer == it.Manufacturer
                        && items.Country == it.Country
                        && items.itemCode == it.itemCode
                        select items
                        ).ToList();
            if (item.Count > 0)
            {
                 int oldid = 0;
                foreach (Item itt in item)
                {
                    oldid = itt.id;

                }
                return oldid;
            }
            else
            {
               

                it.id = newid;

                cbl.addItems(it);
                return newid;
            }



        }


        // GET: Consignments/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consignment consignment = db.Consignments.Find(id);
        
           List<Item_Consignment> ic = db.Item_Consignment.Where(u => u.consignmentId == consignment.id).ToList();
            List<Item_Consignment> newIc = new List<Item_Consignment>();
            
            foreach (Item_Consignment i in ic)
            {
                i.Consignment = consignment;
                
                Item item = db.Items.Find(i.itemId);
                i.Item = item;
                newIc.Add(i);
            }
            ViewBag.ItemDetails = newIc;
         if (consignment == null)
            {
                return HttpNotFound();
            }
            return View(consignment);
        }

        // POST: Consignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: Consignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consignment consignment = db.Consignments.Find(id);
            if (consignment == null)
            {
                return HttpNotFound();
            }
            return View(consignment);
        }

        // POST: Consignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Consignment consignment = db.Consignments.Find(id);
            db.Consignments.Remove(consignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
