using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlexiHouseFinal.Models
{
    public class Shelfandshelfitems
    {
        public int id { get; set; }
        public List<ShelfItems> shelfitemslist { get; set; }

        public Shelfandshelfitems(){}
    }

}