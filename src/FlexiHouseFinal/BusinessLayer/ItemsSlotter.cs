using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ItemsSlotter
    {
        public ItemsSlotter() { }
        public ItemsSlotter(int item_id, int quantity, string item_name, int orders)
        {
            this.item_id = item_id;
            this.quantity = quantity;
            this.item_name = item_name;
            this.orders = orders;
        }

        public string expiry_date { get; set; }
        public int item_id { get; set; }

        public int quantity { get; set; }
        public string item_name { get; set; }

        public int orders { get; set; }

    }
}
