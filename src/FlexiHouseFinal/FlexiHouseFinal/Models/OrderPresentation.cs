using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlexiHouseFinal.Models
{
    public partial class OrderPresentation
    {
      public  List<Order> orderNew { get; set; }
      public  List<Customer> customerNew { get; set; }
      public  List<item_Order> itemOrder { get; set; }
      public  List<Item> itemList { get; set; }


    }
}