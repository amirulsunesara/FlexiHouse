using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlexiHouseFinal.Models;

namespace FlexiHouseFinal.Controllers
{
    public class Item_ConsignmentController : Controller
    {
        private ItemsConsigmentConStr db = new ItemsConsigmentConStr();

        // GET: Item_Consignment
        public ActionResult Index()
        {
            var item_Consignment = db.Item_Consignment.Include(i => i.Consignment).Include(i => i.Item);
            return View(item_Consignment.ToList());
        }

        // GET: Item_Consignment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_Consignment item_Consignment = db.Item_Consignment.Find(id);
            if (item_Consignment == null)
            {
                return HttpNotFound();
            }
            return View(item_Consignment);
        }

        // GET: Item_Consignment/Create
        public ActionResult Create()
        {
            ViewBag.consignmentId = new SelectList(db.Consignments, "id", "consignmentName");
            ViewBag.itemId = new SelectList(db.Items, "id", "itemName");
            return View();
        }

        // POST: Item_Consignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "itemId,consignmentId,quantity,expiry")] Item_Consignment item_Consignment)
        {
            if (ModelState.IsValid)
            {
                db.Item_Consignment.Add(item_Consignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.consignmentId = new SelectList(db.Consignments, "id", "supplier", item_Consignment.consignmentId);
            ViewBag.itemId = new SelectList(db.Items, "id", "itemName", item_Consignment.itemId);
            return View(item_Consignment);
        }

        // GET: Item_Consignment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_Consignment item_Consignment = db.Item_Consignment.Find(id);
            if (item_Consignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.consignmentId = new SelectList(db.Consignments, "id", "supplier", item_Consignment.consignmentId);
            ViewBag.itemId = new SelectList(db.Items, "id", "itemName", item_Consignment.itemId);
            return View(item_Consignment);
        }

        // POST: Item_Consignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "itemId,consignmentId,quantity,expiry")] Item_Consignment item_Consignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item_Consignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.consignmentId = new SelectList(db.Consignments, "id", "supplier", item_Consignment.consignmentId);
            ViewBag.itemId = new SelectList(db.Items, "id", "itemName", item_Consignment.itemId);
            return View(item_Consignment);
        }

        // GET: Item_Consignment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_Consignment item_Consignment = db.Item_Consignment.Find(id);
            if (item_Consignment == null)
            {
                return HttpNotFound();
            }
            return View(item_Consignment);
        }

        // POST: Item_Consignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item_Consignment item_Consignment = db.Item_Consignment.Find(id);
            db.Item_Consignment.Remove(item_Consignment);
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
