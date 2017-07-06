using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ShelfItems
    {

    public int slot_id;
    public int item_id;
    public string item_name;
    public string expiry_date;
    public int status; //0 for empty, 1 for filled, 2 for pending
        public int section_id;

    }
    public class nameList
    {
        public ShelfItems name { get; set; }
    }
}
