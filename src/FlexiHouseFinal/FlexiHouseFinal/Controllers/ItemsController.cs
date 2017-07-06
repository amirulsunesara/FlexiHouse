using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlexiHouseFinal.Models;
using System.Globalization;
using BusinessLayer;
using System.IO;
namespace FlexiHouseFinal.Controllers
{
    public class ItemsController : Controller
    {
        private WarehouseDBEntities db = new WarehouseDBEntities();
        WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
        // GET: Items
        public ActionResult Index()
        {
            int warehouseIdd = wbl.getWarehouseId(Convert.ToInt32(Session["UserID"]));
            ViewBag.PageName = "Consignments";
            return View(db.Item_Warehouse.Where(a=>a.warehouseId==warehouseIdd).ToList());
        }

        public IEnumerable<SelectListItem> GetCountries()
        {
            RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
            List<SelectListItem> countryNames = new List<SelectListItem>();

            //To get the Country Names from the CultureInfo installed in windows
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                countryNames.Add(new SelectListItem() { Text = country.DisplayName, Value = country.DisplayName });
            }

            //Assigning all Country names to IEnumerable
            IEnumerable<SelectListItem> nameAdded = countryNames.GroupBy(x => x.Text).Select(x => x.FirstOrDefault()).ToList<SelectListItem>().OrderBy(x => x.Text);
            return nameAdded;
        }

        // GET: Items/Details/5
        public ActionResult ItemDetails(int? id)
        {
            Item item = db.Items.Find(id);
            try
            {
                if (item.itemDetails == null)
                {
                    item.itemDetail = new itemDetail();
                    item.itemDetail.Id = db.itemDetails.Max(a => a.Id) + 1;
                    item.itemDetails = item.itemDetail.Id;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                item.itemDetail = new itemDetail();
                item.itemDetail.Id = 1;
                item.itemDetails = item.itemDetail.Id;
                db.SaveChanges();
            }
        
                               
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.CountryList = GetCountries();
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,itemName,Manufacturer,Country,itemCode,Category")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,itemName,Manufacturer,Country,itemCode,Category")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }
        [HttpPost]
        public RedirectToRouteResult SaveItem(string itemId, string length, string width, string height, string weight, HttpPostedFileBase imgItem)
        {
          
            string thePictureDataAsString = "";
            if (imgItem != null)
            {
                string theFileName = Path.GetFileName(imgItem.FileName);
                byte[] thePictureAsBytes = new byte[imgItem.ContentLength];
                using (BinaryReader theReader = new BinaryReader(imgItem.InputStream))
                {
                    thePictureAsBytes = theReader.ReadBytes(imgItem.ContentLength);
                }
                thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
            }

            Item item = db.Items.Find(Convert.ToInt32(itemId));
            itemDetail itemd = db.itemDetails.Find(item.itemDetails);
            if (imgItem == null)
            {


            }
            else
            {
                itemd.picture = thePictureDataAsString;

            }
            itemd.weight = weight;
            itemd.dimensions = length + "X" + width + "X" + height;
            
            db.SaveChanges();


            return  RedirectToAction("index"); 
        }
        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
