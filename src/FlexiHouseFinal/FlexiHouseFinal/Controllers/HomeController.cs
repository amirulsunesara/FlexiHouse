using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexiHouseFinal.Models;
using BusinessLayer;
using System.Web.Services;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace FlexiHouseFinal.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        [WebMethod]
        public ActionResult Start(WarehouseBL warehouse)
        {
            WarehouseBusinessLayer warehouseBL = new WarehouseBusinessLayer();
            warehouse.managerId = Convert.ToInt32(Session["UserID"]);
            warehouseBL.UpdateStartWareHouse(warehouse);

            return RedirectToAction("LoggedIn", "Account");
        }

        public ActionResult Index()
        {
            if (Session["Username"] != null)
            {
                return RedirectToAction("LoggedIn", "Account");
            }
            else
            {
                return View();
            }
        }
        public ActionResult viewWarehouse()
        {

            ViewBag.PageName = "View Warehouse";
            String ss;
            WarehouseBusinessLayer warehouseBL = new WarehouseBusinessLayer();
            Byte[] dataInBytes = warehouseBL.getWarehouse(Convert.ToInt32(Session["UserID"]));
            try
            {
                ss = System.Text.Encoding.UTF8.GetString(dataInBytes);
            }
            catch (Exception ex)
            {
                ss = null;

            }
            ViewBag.HtmlStr = ss;
            return View();
        }
        [HttpGet]
        public JsonResult GetAllItems(string id)
        {
            try
            {
                WarehouseDBEntities wdb = new WarehouseDBEntities();
                WarehouseBusinessLayer ware = new WarehouseBusinessLayer();
                int warehouseID = ware.getWarehouseId(Convert.ToInt32(Session["UserID"]));
                Warehouse warehouse = wdb.Warehouses.FirstOrDefault(k => k.id == warehouseID);
                int palletsInOneRow = ((int)warehouse.shelfSlots / (int)warehouse.sections)/(int)warehouse.shelfRows;

                string sname = warehouseID.ToString() + id;
                Shelf shelf = wdb.Shelves.FirstOrDefault(a => a.shelfName == sname);
                shelf.shelfName = id;
                shelf.warehouse_id = palletsInOneRow;
                shelf.slotsRemaining = warehouse.sections;
                List<Shelf> sitems = new List<Shelf>();
               
                sitems.Add(shelf);

                return new JsonResult { Data = sitems, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public JsonResult GetSpecificItem(string id)
        {
            try
            {
                WarehouseDBEntities wdb = new WarehouseDBEntities();
                WarehouseBusinessLayer ware = new WarehouseBusinessLayer();
                int warehouseID = ware.getWarehouseId(Convert.ToInt32(Session["UserID"]));
                
             //   Shelf shelf = wdb.Shelves.FirstOrDefault(a => a.shelfName == sname);
                Item item = wdb.Items.Find(Convert.ToInt32(id));


                List<String> sitems = new List<String>();

                sitems.Add(item.Manufacturer);
                sitems.Add(item.itemCode);
                sitems.Add(item.itemDetail.dimensions);
                sitems.Add(item.itemDetail.weight);
                sitems.Add(item.itemDetail.picture);
                sitems.Add(item.Country);


                return new JsonResult { Data = sitems, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        [HttpPost]
        [WebMethod]
        public ActionResult Save(WarehouseBL ware , List<Shelves> shel , List<Depot> depot)
        {
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            Shelves s = new Shelves();
            WarehouseBusinessLayer warehouseBL = new WarehouseBusinessLayer();
            ware.managerId = Convert.ToInt32(Session["UserID"]);
            warehouseBL.UpdateWareHouse(ware);
            int shelfCount = shel.Count;
            int zoneA = s.ZoneAshelf(shelfCount);
            int zoneB = s.ZoneBshelf(shelfCount);
            int zoneC = s.ZoneCshelf(shelfCount, zoneA, zoneB);
            shel = s.ShelvesWithDistance(shel,depot);
            shel = s.SortShelves(shel);
            shel = s.assignZones(shel,zoneA,zoneB,zoneC);
         
            foreach(Shelves shelve in shel)
            {
                shelve.warehouse_id = warehouseBL.getWarehouseId(Convert.ToInt32(Session["UserID"]));
                int myid = warehouseBL.getWarehouseId(Convert.ToInt32(Session["UserID"]));
              bool isExist = wdb.Shelves.Any(u => u.shelfName == myid.ToString()+shelve.shelveID);
                if (isExist)
                {
                    shelve.shelveID = shelve.warehouse_id + shelve.shelveID;
                    warehouseBL.UpdateShelf(shelve);
                }
                else
                {
                   
                    shelve.shelveID = shelve.warehouse_id + shelve.shelveID;
                  
                    
                    int warehouseID = warehouseBL.getWarehouseId(Convert.ToInt32(Session["UserID"]));
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    Warehouse warehouse = wdb.Warehouses.Find(warehouseID);
                    List<ShelfItems> shelfItems = new List<ShelfItems>();
                    int z = 1;
                    for (int i = 1; i <= warehouse.shelfSlots; i++)
                    {
                        ShelfItems si = new ShelfItems();
                        si.slot_id = i;
                        si.item_id = -1;
                        si.item_name = "";
                        si.expiry_date = "";
                        si.status = 0;


                        if (i - warehouse.sections<= 0)
                	    {
                            si.section_id = i;

                        }
	                else if (i - warehouse.sections > 0 && i - warehouse.sections *z <= warehouse.sections)
	                    {
                        si.section_id = i - (int)warehouse.sections * z;
			
	                    }
	                else
	                {
                        z += 1;
                            si.section_id = i - ((int)warehouse.sections*z);
	                 }






                    shelfItems.Add(si);
                    }
                    string output = JsonConvert.SerializeObject(shelfItems);
                    
                    shelve.slotsRemaining = warehouse.shelfSlots;
                    shelve.shelfItems = output;
                 



                    warehouseBL.AddShelf(shelve);
                }
               
            }
           
            return RedirectToAction("LoggedIn", "Account");
        }

  


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}