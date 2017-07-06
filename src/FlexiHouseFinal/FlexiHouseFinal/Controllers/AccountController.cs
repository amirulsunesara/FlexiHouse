using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexiHouseFinal.Models;
using BusinessLayer;
using System.Data;
using System.Data.Entity;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;

using System.Web.Services;


namespace FlexiHouseFinal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account


   
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateWorker()
        {

            return View();
        }
        [HttpGet]
        public ActionResult EditWorker(int id)
        {
            WarehouseDBEntities wde = new WarehouseDBEntities();
            UserAccount useracc = wde.UserAccounts.Find(id);
            return View(useracc);
        }
        public ActionResult EditProfile(int id)
        {
            WarehouseDBEntities wde = new WarehouseDBEntities();
            UserAccount useracc = wde.UserAccounts.Find(id);
            return View(useracc);
        }
        [HttpPost]
        public RedirectToRouteResult EditProfile(string userid, string fullname, string email, string number, string username, string password)
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

            return RedirectToAction("LoggedIn");
        }
        [HttpPost]
        public RedirectToRouteResult SaveEditWorker(string userid, string fullname, string email, string number, string username, string password,string[] shelfs)
        {
            WarehouseDBEntities wde = new WarehouseDBEntities();
            UserAccount useracc = wde.UserAccounts.Find(Convert.ToInt32(userid));
            
        
            useracc.Name = fullname;
            useracc.Email = email;
            useracc.UserName = username;
            useracc.Contact = number;
            useracc.Password = password;
            useracc.ConfirmPassword = password;
            useracc.Worker.assignedShelfs = JsonConvert.SerializeObject(shelfs);

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

            return RedirectToAction("ViewWorkers");
        }
        [HttpPost]
        public RedirectToRouteResult CreateWorker(string fullname, string email, string number, string username, string password,string[] shelfs)
        {
            WarehouseDBEntities wde = new WarehouseDBEntities();
            UserAccount acc = new UserAccount();
            acc.Email = email;
            acc.Name = fullname;
            acc.UserName = username;
            acc.Password = password;
            acc.Contact = number;
            acc.Role = "Worker";
            acc.Registered = DateTime.Now.ToShortDateString();
            wde.Workers.Add(new Worker
            {
                warehouseId = Convert.ToInt32(Session["UserId"]),
                assignedShelfs = JsonConvert.SerializeObject(shelfs)


            });
            if (wde.UserAccounts.Where(a => a.UserName == acc.UserName).ToList().Count < 1)
            {
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

                // acc.workerId = Convert.ToInt32(Session["UserId"]);
                acc.workerId = wde.Workers.Max(a => a.Id);
                ConsignmentBusinessLayer cbl = new ConsignmentBusinessLayer();
                cbl.addAccount(acc);

                return RedirectToAction("ViewWorkers");
            }
            else {

                return null;
            }
            }
        public ActionResult ViewWorkers()
        {
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            int warehouseID = Convert.ToInt32(Session["UserId"]);
            List<UserAccount> useracc = wdb.UserAccounts.Where(a => a.Worker.warehouseId== warehouseID && a.Role == "Worker").ToList();


             return View(useracc);
        }
        [HttpGet]
        public RedirectToRouteResult Delete(int id)
        {
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            var user = (from d in wdb.UserAccounts
                          where d.UserID == id
                          select d).Single();
            wdb.UserAccounts.Remove(user);
            wdb.SaveChanges();
          return RedirectToAction("ViewWorkers");
         }
        public ActionResult LogOff()
        {
            if (Session["WorkerID"] != null)
            {
                WarehouseDBEntities wde = new WarehouseDBEntities();
                WorkerLogs wl = new WorkerLogs();
                Worker worker = wde.Workers.Find(Convert.ToInt32(Session["WorkerID"]));
                JavaScriptSerializer jss = new JavaScriptSerializer();
                if (worker.workerLogs != null)
                {
                    List<WorkerLogs> workerlogs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkerLogs>>(worker.workerLogs);
                    wl.date = DateTime.Now.ToShortDateString();
                    wl.time = DateTime.Now.ToShortTimeString();
                    wl.description = "Logout";
                    workerlogs.Add(wl);
                    string jsObj = jss.Serialize(workerlogs);
                    worker.workerLogs = jsObj;
                    wde.SaveChanges();
                }

            }

            Session.Abandon();
            return RedirectToAction("Register", "Account");
        }
        public ActionResult Settings()
        {
            WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
            DataSet ds = new DataSet();
            ds = wbl.getWarehouseDetails(Convert.ToInt32(Session["UserID"]));
     
            return View(ds);
        }
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Register()
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
        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            bool success = false;

            ConsignmentBusinessLayer db = new ConsignmentBusinessLayer();    
                if (db.getUsers(account.signUp.UserName,account.signUp.Email).Tables[0].Rows.Count > 0)
                {


                    ModelState.AddModelError("keyName2", "Username or Email Already Taken.");


                }
                else
                {

                    UserAccount useracc = new UserAccount();
                    useracc.Name = account.signUp.Name;
                    useracc.UserName = account.signUp.UserName;

                    useracc.Email = account.signUp.Email;
                    useracc.Role = account.signUp.Role;
                    success = true;

                    TempData["SignupStepOne"] = useracc;


/**

                    ConsignmentBusinessLayer cbl = new ConsignmentBusinessLayer();
                    cbl.addAccount(useracc);
                      


               
                        ViewBag.Message = account.signUp.UserName + " " + "Successfully Registered, Please LogIn To Proceed";
                    WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
                    if (account.signUp.Role == "Manager")
                        {
                           
                            int Managerid = wbl.getRecentlyId();
                            wbl.createWarehouse(Managerid);

                        }
                    Session["UserID"] = wbl.getRecentlyId();
                        Session["Username"] = account.signUp.UserName.ToString();
                        success = true;
                    
    **/

                }



            



            if (success)
            {
                return RedirectToAction("StepTwoManager");



            }
            else
            {
                return View();
            }
        }
     
        public ActionResult StepTwoManager()
        {
            if (TempData["SignupStepOne"] != null)
            {
                UserAccount useracc = (UserAccount)TempData["SignupStepOne"];
                return View(useracc);
            }
            else
            {
                return RedirectToAction("Register");

            }

        }
      


        public ActionResult StepTwoCustomer()
        {
            if (TempData["SignupStepOne"] != null)
            {
                UserAccount useracc = (UserAccount)TempData["SignupStepOne"];
                WarehouseDBEntities wdb = new WarehouseDBEntities();
                List<Warehouse> warehouses = wdb.Warehouses.ToList();
                ViewBag.Warehouses = warehouses;
                return View(useracc);
            }
            else
            {
                return RedirectToAction("Register");

            }

        }
        [HttpPost]
        public ActionResult RegisterManager(string fullname,string email,string number,string warehousename,string warehouseaddress,string username,string password,string password2, HttpPostedFileBase warehouselogo,string countries)
        {
            string thePictureDataAsString="";
            if (warehouselogo != null)
            {
                string theFileName = Path.GetFileName(warehouselogo.FileName);
                byte[] thePictureAsBytes = new byte[warehouselogo.ContentLength];
                using (BinaryReader theReader = new BinaryReader(warehouselogo.InputStream))
                {
                    thePictureAsBytes = theReader.ReadBytes(warehouselogo.ContentLength);
                }
                thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
            }
            WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();


       

            UserAccount useracc = new UserAccount();
            useracc.Name = fullname;
            useracc.Email = email;
            useracc.Contact = number;
            useracc.Password = password;
            useracc.ConfirmPassword = password;
            useracc.Registered = DateTime.Now.ToShortDateString();
            useracc.Role = "Manager";
            useracc.UserName = username;

            //   ConsignmentBusinessLayer cbl = new ConsignmentBusinessLayer();
            // cbl.addAccount(useracc);
            WarehouseDBEntities wde = new WarehouseDBEntities();
            wde.UserAccounts.Add(useracc);
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


            int Managerid = wbl.getRecentlyId();
            wbl.createWarehouse(Managerid, warehousename, warehouseaddress, thePictureDataAsString,countries);

            Session["UserID"] = wbl.getRecentlyId();
            Session["Username"] = username;
        
            return RedirectToAction("LoggedIn");

        }
        [HttpPost]
        public ActionResult RegisterCustomer(string fullname, string email, string number, string organizationname, string organizationaddress, string username, string password, string password2, string[] warehouses)
        {
          
            WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();




           Customer useracc = new Customer();
            useracc.fullName = fullname;
            useracc.email = email;
            useracc.contact = number;
            useracc.password = password;
            useracc.userName = username;
            useracc.organizationAddress = organizationaddress;
            useracc.organizationName = organizationname;
            useracc.selectedWarehouse = JsonConvert.SerializeObject(warehouses);

            WarehouseDBEntities wdb = new WarehouseDBEntities();
            wdb.Customers.Add(useracc);
            wdb.SaveChanges();
       

            Session["UserID"] = wbl.getRecentlyId();
            Session["Username"] = username;

            return RedirectToAction("LoggedIn");

        }
        public Image LoadImage(string myimage)
        {
            byte[] bytes = Convert.FromBase64String(myimage);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }


        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Login()
        {
            if (Session["Username"] != null)
            {
                return RedirectToAction("LoggedIn", "Account");
            }
            else
            {
                return RedirectToAction("Register", "Account");
            }
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
           
            using (WarehouseDBEntities db = new WarehouseDBEntities())
            {

                try
                {
                    var usr = db.UserAccounts.Single(u => u.UserName == user.login.UserName && u.Password == user.login.Password);


                    if (usr != null && usr.Role == "Manager")
                    {
                        Session["UserID"] = usr.UserID.ToString();
                        Session["Username"] = usr.Name.ToString();
                        return RedirectToAction("LoggedIn");
                    }
                    else if (usr != null && usr.Role == "Worker")
                    {
                        Session["WorkerID"] = usr.workerId;
                        Session["WorkerName"] = usr.Name.ToString();

                        WorkerLogs wl = new WorkerLogs();
                        Worker worker = db.Workers.Find(usr.workerId);
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        if (worker.workerLogs != null)
                        {
                            List<WorkerLogs> workerlogs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkerLogs>>(worker.workerLogs);
                            wl.date = DateTime.Now.ToShortDateString();
                            wl.time = DateTime.Now.ToShortTimeString();
                            wl.description = "User Logged In";
                            workerlogs.Add(wl);
                            string jsObj = jss.Serialize(workerlogs);
                            worker.workerLogs = jsObj;
                            db.SaveChanges();
                        }
                    else
                        {
                            List<WorkerLogs> workerlogs = new List<WorkerLogs>();
                            wl.date = DateTime.Now.ToShortDateString();
                            wl.time = DateTime.Now.ToShortTimeString();
                            wl.description = "User Logged In";
                            workerlogs.Add(wl);
                            string jsObj = jss.Serialize(workerlogs);
                            worker.workerLogs = jsObj;
                            db.SaveChanges();
                        }



                        return RedirectToAction("Index","Worker");

                    }
                    else
                    {
                        ModelState.AddModelError("keyName", "Username or Password Incorrect.");
                    }


                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("keyName", "Username or Password Incorrect.");
                  
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetAllUser()
        {
            try
            {
                List<WarehouseBL> allUser = new List<WarehouseBL>();

                WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
                DataSet ds = wbl.getWarehouseDetails(Convert.ToInt32(Session["UserID"]));
                DataRow dr = ds.Tables[0].Rows[0];
                WarehouseBL w = new WarehouseBL();
                w.scaledShelfLength = dr["scaledShelfLength"].ToString();
                w.scaledShelfWidth = dr["scaledShelfWidth"].ToString();

                allUser.Add(w);

                return new JsonResult { Data = allUser, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch(Exception ex)
            {
                return null; 
            }
            }
        [HttpGet]
        public JsonResult GetWorkerLogs(string id)
        {
            try
            {
              
                WarehouseDBEntities wde = new WarehouseDBEntities();
                Worker worker = wde.Workers.Find(Convert.ToInt32(id));

                List<WorkerLogs> logs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkerLogs>>(worker.workerLogs);

                return new JsonResult { Data = logs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public ActionResult LoggedIn()
        {


            if (Session["UserID"] != null)
            {
                int id = Convert.ToInt32(Session["UserID"]);
                String ss;
                WarehouseBusinessLayer warehouseBL = new WarehouseBusinessLayer();
                Byte[] dataInBytes = warehouseBL.getWarehouse(id);
                Boolean value = warehouseBL.getWarehouseAttr(id);
                WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();

                DataSet ds = wbl.getWarehouseDetails(id);
                DataRow dr = ds.Tables[0].Rows[0];
                string imageDataURL = "";
                if (dr["warehouseLogo"].ToString() != "")
                {
                     imageDataURL = string.Format("data:image;base64,{0}", dr["warehouseLogo"].ToString());
                }
                else
                {
                    imageDataURL = "/images/logo.png";



                }
                
                    Session["logo"] = imageDataURL;


                try
                {
                    ss = System.Text.Encoding.UTF8.GetString(dataInBytes);
                }
                catch (Exception ex)
                {
                    if (value)
                    {
                        ss = "";
                    }
                    else
                    {
                        ss = null;
                    }
                }
                ViewBag.HtmlStr = ss;

                return View();

            }
            else
            {
                return RedirectToAction("Login");

            }

        }
    }
}