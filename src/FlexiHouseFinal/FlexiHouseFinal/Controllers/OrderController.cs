using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlexiHouseFinal.Models;
using System.Web.Services;
using BusinessLayer;
using FlexiHouseFinal.Routines;
using FlexihouseRoutinesTest;
using System.Web.Script.Serialization;
namespace FlexiHouseFinal.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderInstruction()
        {
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
            int id = wbl.getWarehouseId(Convert.ToInt32(Session["UserID"]));
            List<Order> orders = wdb.Orders.Where(a=>a.warehouseId==id && a.orderStatus=="Dispatched").ToList();


            return View(orders);
        }
        public ActionResult GetOrders() {

            WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            int warehouseIdd = wbl.getWarehouseId(Convert.ToInt32(Session["UserID"]));

           List<Order> orderNew = wdb.Orders.Where(a => a.warehouseId == warehouseIdd).ToList();
     

            return View(orderNew);
        }
        [HttpPost]
        public void UpdateOrderStatus(string status) {
            int orderId = Convert.ToInt32(status);
            WarehouseDBEntities wdm = new WarehouseDBEntities();
            Order Oldorder = wdm.Orders.FirstOrDefault(a => a.orderId == orderId);
            Order orderCopy = Oldorder;
            orderCopy.orderStatus = "Seen";

            wdm.Entry(Oldorder).CurrentValues.SetValues(orderCopy);
            wdm.SaveChanges();
            wdm.Dispose();

        }
        [HttpPost]
        public void UpdateOrderCancel(string status)
        {
            int orderId = Convert.ToInt32(status);
            WarehouseDBEntities wdm = new WarehouseDBEntities();
            Order Oldorder = wdm.Orders.FirstOrDefault(a => a.orderId == orderId);
            Order orderCopy = Oldorder;
            orderCopy.orderStatus = "Canceled";

            wdm.Entry(Oldorder).CurrentValues.SetValues(orderCopy);
            wdm.SaveChanges();
            wdm.Dispose();


        }
        [HttpGet]
        public void DispatchOrders(string id)
        {
            WarehouseBusinessLayer wbl = new WarehouseBusinessLayer();
            int warehouseId = wbl.getWarehouseId(Convert.ToInt32(Session["UserID"]));
            int oid = Convert.ToInt32(id);
            WarehouseDBEntities wde = new WarehouseDBEntities();
            List<Order> orders = wde.Orders.Where(a => a.warehouseId == warehouseId && a.orderId==oid && (a.orderStatus == "Unseen" || a.orderStatus == "Seen")).OrderBy(a=>a.totalOrderQuanitity).ToList();
            int i = 0;
            //Sorting items according to quantity
            foreach (var order in orders)
            {
                var neworder = order.item_Order;
                neworder = neworder.OrderBy(a => a.quantity).ToList();
                orders.ElementAt(i).item_Order = neworder;
                i++;


            }
            Slotting slotting = new Slotting();
            List<Item_Warehouse> itemWarehouse = wde.Item_Warehouse.Where(a => a.warehouseId == warehouseId).ToList();

            List<Shelf> shelfs = wde.Shelves.Where(a => a.warehouse_id == warehouseId).ToList();
            Dictionary<int, List<ShelfItems>> shelfItems = new Dictionary<int, List<ShelfItems>>();

            foreach(Shelf shelf in shelfs)
            {
                List<ShelfItems> shelves = slotting.covert_to_object(shelf.shelfItems);
                shelfItems.Add(shelf.id,shelves);


            }
            
                List<Item> items = new List<Item>();
            foreach(Item_Warehouse item_Warehouse in itemWarehouse)
            {
                items.Add(item_Warehouse.Item); 
            }

            OrderPicking orderPick = new OrderPicking();
            orderPick.getFirstOrder(orders, shelfItems);
        


        }
        public ActionResult CompleteOrder() {

            if (Session["customerRecord"] != null && Session["orderRecord"] != null)
            {
                WarehouseDBEntities wdb = new WarehouseDBEntities();
                Customer customer = (Customer)Session["customerRecord"];
                List<Item> items = (List<Item>)Session["orderRecord"];
                int customerId;

                try { customerId = wdb.Customers.Max(i => i.id) + 1; }
                catch (Exception ex) { customerId = 1; }
                customer.id = customerId;

                int warehouseId = Convert.ToInt32(customer.selectedWarehouse);


                int orderId;

                try { orderId = wdb.Orders.Max(i => i.orderId) + 1; }
                catch (Exception ex) { orderId = 1; }
                wdb.Customers.Add(customer);
                wdb.SaveChanges();
                wdb.Dispose();

                wdb = new WarehouseDBEntities();
                Order order = new Order();
                order.orderId = orderId;
                order.customerId = customerId;
                order.orderDate = System.DateTime.Now.Date;
                order.orderStatus = "Unseen";
                order.warehouseId = warehouseId;
                order.totalOrderQuanitity = 0;
                foreach (Item item in items)
                {
                    order.totalOrderQuanitity = order.totalOrderQuanitity+Convert.ToInt32(item.Manufacturer);

                } 


                wdb.Orders.Add(order);
                wdb.SaveChanges();
                wdb.Dispose();


                wdb = new WarehouseDBEntities();
                List<item_Order> item_OrderList = new List<item_Order>();
                foreach (Item item in items)
                {

                    item_Order itemOrder = new item_Order();
                    itemOrder.itemId = item.id;
                    itemOrder.orderId = orderId;
                    itemOrder.quantity = Convert.ToInt32(item.Manufacturer);
                    item_OrderList.Add(itemOrder);

                    Item_Warehouse original = wdb.Item_Warehouse.FirstOrDefault(a => a.itemId == item.id && a.warehouseId == warehouseId);
                    Item_Warehouse copy = original;
                    copy.orders += 1;
                    copy.quantity = copy.quantity - itemOrder.quantity;
                    if (original != null)
                    {
                        wdb.Entry(original).CurrentValues.SetValues(copy);
                        wdb.SaveChanges();
                        wdb.Dispose();
                        wdb = new WarehouseDBEntities();
                    }

                }
                wdb.item_Order.AddRange(item_OrderList);
                wdb.SaveChanges();
                wdb.Dispose();


                Session.Clear();
                Session.Abandon();
            }
            else
            {
                RedirectToAction("Register", "Account");
            }

            return View();
        }
        public ActionResult PlaceOrder()
        {
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            List<Warehouse> warehouses = wdb.Warehouses.ToList();
            ViewBag.Warehouses = warehouses;

            return View();
        }

        public ActionResult OrderReview()
        {

            if (Session["customerRecord"] != null && Session["orderRecord"] != null)
            {
                return View();
            }
            else
            {
                RedirectToAction("Register", "Account");
            }
            RedirectToAction("Register", "Account");
            return View();
        }
        [HttpPost]
        public ActionResult PlaceOrderRegister(string fullname, string email, string number, string organizationname, string organizationaddress, string orderCountries, string orderWarehouse)
        {
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            Customer customer = new Customer();
            customer.fullName = fullname;
            customer.email = email;
            customer.contact = number;
            customer.organizationName = organizationname;
            customer.organizationAddress = organizationaddress;
            customer.selectedWarehouse = orderWarehouse;
            Session["warehouseKey"] = orderWarehouse;
            int warehouseID = Convert.ToInt32(orderWarehouse);
            Warehouse w = wdb.Warehouses.FirstOrDefault(a => a.id == warehouseID);
           Session["warehouseName"] = w.warehouseName;
            Session["customerRecord"] = customer;


            return RedirectToAction("PlaceOrderComplete");
        }
        public ActionResult PlaceOrderComplete()
        {
            if (Session["customerRecord"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Register", "Account");


            }
        }
        [HttpPost]
        public ActionResult PlaceOrderDoIt(List<Item> ware)
        {

            Session["orderRecord"] = ware;

            return RedirectToAction("OrderReview");


        }
        [HttpGet]
        [WebMethod]
        public JsonResult GetWarehouses(string id)
        {
            try
            {

                ConsignmentBusinessLayer cbl = new ConsignmentBusinessLayer();
                List<Warehouse> warehouse = cbl.getNameAddress(id);


                return new JsonResult { Data = warehouse, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        [WebMethod]
        public JsonResult GetItems(int warehouseId)
        {
            List<Item> items = new List<Item>();
            WarehouseDBEntities wdb = new WarehouseDBEntities();
            List<Item_Warehouse> iw = wdb.Item_Warehouse.Where(i => i.warehouseId == warehouseId).ToList();
            ConsignmentBusinessLayer cbl = new ConsignmentBusinessLayer();
            foreach (Item_Warehouse i in iw)
            {
                //Item it = wdb.Items.First(k=>k.id==i.itemId);
                items.Add(cbl.getItem(i.itemId, (int)i.quantity));

            }

            wdb.Dispose();


            return new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
      



    }
}