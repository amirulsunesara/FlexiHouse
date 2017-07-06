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
    public class WorkerController : Controller
    {
        // GET: Worker
        WarehouseDBEntities wde = new WarehouseDBEntities();
        [HttpGet]
        public ActionResult EditProfile(int id)
        {
            WarehouseDBEntities wde = new WarehouseDBEntities();
            UserAccount useracc = wde.UserAccounts.Find(id);
            return View(useracc);
        
        }
        [HttpPost]
        public RedirectToRouteResult SaveEditWorker(string userid, string fullname, string email, string number, string username, string password)
        {
            WarehouseDBEntities wde = new WarehouseDBEntities();
            UserAccount useracc = wde.UserAccounts.Find(Convert.ToInt32(userid));


            useracc.Name = fullname;
            useracc.Email = email;
            useracc.UserName = username;
            useracc.Contact = number;
            useracc.Password = password;
            useracc.ConfirmPassword = password;
         

            wde.Entry(useracc).State = EntityState.Modified;

            try
            {
                wde.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
           // string imageDataURL = "/images/logo.png";
            int workerId = Convert.ToInt32(Session["WorkerID"]);
            ViewBag.Worker = wde.Workers.Find(workerId);
            UserAccount useracc = wde.UserAccounts.Where(a=>a.workerId==workerId).Single();
            int managerId = Convert.ToInt32(wde.Workers.Where(a => a.Id == workerId).Single().warehouseId);
            string warehouseLogo = wde.Warehouses.Where(z => z.managerId == managerId).FirstOrDefault().warehouseLogo;

            if (warehouseLogo != "")
            {
                warehouseLogo = string.Format("data:image;base64,{0}", warehouseLogo);
            }
            else
            {
               warehouseLogo = "/images/logo.png";

            }




            Session["logo"] = warehouseLogo;

            return View(useracc);
      
        }
        public ActionResult Consignments()
        {
            int workerId = Convert.ToInt32(Session["WorkerID"]);
            int managerId = Convert.ToInt32(wde.Workers.Where(a => a.Id == workerId).Single().warehouseId);
            int warehouseIdd = Convert.ToInt32(wde.Warehouses.Where(z => z.managerId == managerId).FirstOrDefault().id);
            string strAssignedShelfs = wde.Workers.Where(a => a.Id == workerId).Single().assignedShelfs;

            List<string> assignedShelfs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(strAssignedShelfs);

          
                //  count = wde.Consignments.Count(a => a.consignmentStatus == "Added" && a.warehouseId == warehouseIdd);
                List<Consignment> conShelfs = wde.Consignments.Where(a=>a.warehouseId == warehouseIdd).ToList();
            List<Consignment> hasResponsible = new List<Consignment>();
                foreach (var conShelf in conShelfs)
                {
                    List<string> shelfInConsignment = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(conShelf.shelfInserted);

                    foreach (string shelf in assignedShelfs)
                    {
                        if (shelfInConsignment.Contains(shelf))
                        {
                        hasResponsible.Add(conShelf);
                            break;
                        }

                    }
                }

            
           
        




            return View(hasResponsible);
        }
        public RedirectToRouteResult CompleteConsignment(int? id)
        {
            Consignment consi = wde.Consignments.Find(id);


            WorkerLogs wl = new WorkerLogs();
            Worker worker = wde.Workers.Find(Convert.ToInt32(Session["WorkerID"]));
            JavaScriptSerializer jss = new JavaScriptSerializer();
            if (worker.workerLogs != null)
            {
                List<WorkerLogs> workerlogs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkerLogs>>(worker.workerLogs);
                wl.date = DateTime.Now.ToShortDateString();
                wl.time = DateTime.Now.ToShortTimeString();
                wl.description = "Completed Consignment of "+consi.supplier;
                workerlogs.Add(wl);
                string jsObj = jss.Serialize(workerlogs);
                worker.workerLogs = jsObj;
                wde.SaveChanges();
            }
            





            consi.consignmentStatus = "Completed";
            wde.Entry(consi).State = EntityState.Modified;
            wde.SaveChanges();


            return RedirectToAction("ConsignmentsSlotted");
        }
        public ActionResult ConsignmentDetails(int? id)
        {
           
            Consignment consi = wde.Consignments.Find(id);
            if (consi.consignmentStatus != "Completed")
            {
                consi.consignmentStatus = "Seen";
                wde.Entry(consi).State = EntityState.Modified;
                wde.SaveChanges();
            }
            //ViewBag.PageName = "Consignments";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consignment consignment = wde.Consignments.Find(id);
            List<Item_Consignment> ic = wde.Item_Consignment.Where(u => u.consignmentId == consignment.id).ToList();
            List<Item_Consignment> newIc = new List<Item_Consignment>();

            foreach (Item_Consignment i in ic)
            {
                i.Consignment = consignment;

                Item item = wde.Items.Find(i.itemId);
                i.Item = item;
                newIc.Add(i);
            }

            ViewBag.ItemDetails = newIc;


                    List<string> instructionList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(consignment.instruction);
            ViewBag.Instructions = instructionList;


            if (consignment == null)
            {
                return HttpNotFound();
            }
            return View(consignment);
        }
        public ActionResult Orders()
        {

            int workerId = Convert.ToInt32(Session["WorkerID"]);
            int managerId = Convert.ToInt32(wde.Workers.Where(a => a.Id == workerId).Single().warehouseId);
            int warehouseIdd = Convert.ToInt32(wde.Warehouses.Where(z => z.managerId == managerId).FirstOrDefault().id);
            string strAssignedShelfs = wde.Workers.Where(a => a.Id == workerId).Single().assignedShelfs;

            List<string> assignedShelfs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(strAssignedShelfs);
            List<Order> orderShelfs = wde.Orders.Where(a => a.orderStatus == "Dispatched" && a.warehouseId == warehouseIdd).ToList();
            List<Order> hasResponsible = new List<Order>();

            foreach (var orderShelf in orderShelfs)
            {
                List<string> shelfInOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(orderShelf.shelfRetrieval);
                foreach (string shelf in assignedShelfs)
                {
                    if (shelfInOrder.Contains(shelf))
                    {
                        hasResponsible.Add(orderShelf);
                        break;
                    }

                }

            }
            return View(hasResponsible);
        }
        public RedirectToRouteResult CompletedOrder(int? id)
        {
          Order Order = wde.Orders.Find(id);
            Customer cust = wde.Customers.Find(Order.customerId);
        
            WorkerLogs wl = new WorkerLogs();
            Worker worker = wde.Workers.Find(Convert.ToInt32(Session["WorkerID"]));
            JavaScriptSerializer jss = new JavaScriptSerializer();
            if (worker.workerLogs != null)
            {
                List<WorkerLogs> workerlogs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkerLogs>>(worker.workerLogs);
                wl.date = DateTime.Now.ToShortDateString();
                wl.time = DateTime.Now.ToShortTimeString();
                wl.description = "Completed Order of " + cust.fullName + "("+cust.organizationName+")";
                workerlogs.Add(wl);
                string jsObj = jss.Serialize(workerlogs);
                worker.workerLogs = jsObj;
                wde.SaveChanges();
            }



            if (Order.orderStatus == "Dispatched")
            {
                Order.orderStatus = "Completed";
                wde.Entry(Order).State = EntityState.Modified;
                wde.SaveChanges();
            }

            return RedirectToAction("OrdersDelivered");
        }
        public ActionResult ConsignmentsSlotted()
        {

            int workerId = Convert.ToInt32(Session["WorkerID"]);
            int managerId = Convert.ToInt32(wde.Workers.Where(a => a.Id == workerId).Single().warehouseId);
            int warehouseIdd = Convert.ToInt32(wde.Warehouses.Where(z => z.managerId == managerId).FirstOrDefault().id);
            string strAssignedShelfs = wde.Workers.Where(a => a.Id == workerId).Single().assignedShelfs;

            List<string> assignedShelfs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(strAssignedShelfs);


            //  count = wde.Consignments.Count(a => a.consignmentStatus == "Added" && a.warehouseId == warehouseIdd);
            List<Consignment> conShelfs = wde.Consignments.Where(a => a.warehouseId == warehouseIdd && a.consignmentStatus=="Completed").ToList();
            List<Consignment> hasResponsible = new List<Consignment>();
            foreach (var conShelf in conShelfs)
            {
                List<string> shelfInConsignment = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(conShelf.shelfInserted);

                foreach (string shelf in assignedShelfs)
                {
                    if (shelfInConsignment.Contains(shelf))
                    {
                        hasResponsible.Add(conShelf);
                        break;
                    }

                }
            }








            return View(hasResponsible);
        }
        public ActionResult OrdersDelivered()
        {
            int workerId = Convert.ToInt32(Session["WorkerID"]);
            int managerId = Convert.ToInt32(wde.Workers.Where(a => a.Id == workerId).Single().warehouseId);
            int warehouseIdd = Convert.ToInt32(wde.Warehouses.Where(z => z.managerId == managerId).FirstOrDefault().id);
            string strAssignedShelfs = wde.Workers.Where(a => a.Id == workerId).Single().assignedShelfs;

            List<string> assignedShelfs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(strAssignedShelfs);

            List<Order> orderShelfs = wde.Orders.Where(a => a.orderStatus == "Completed" && a.warehouseId == warehouseIdd).ToList();
            List<Order> hasResponsible = new List<Order>();

            foreach (var orderShelf in orderShelfs)
            {
                List<string> shelfInOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(orderShelf.shelfRetrieval);
                foreach (string shelf in assignedShelfs)
                {
                    if (shelfInOrder.Contains(shelf))
                    {
                        hasResponsible.Add(orderShelf);
                        break;
                    }

                }

            }
            return View(hasResponsible);

           
        }

    }
}