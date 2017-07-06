using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLayer
{
    public class WarehouseBL
    {

            public int warehouseId { get; set; }
            public string warehouseHtml { set; get; }
            
            public string warehouseWidth { get; set;}

            public string warehouseLength { get; set; }

            public string shelveWidth { get; set; }

            public string shelveHeight { get; set; }

            public string shelveLength { get; set; }

            public string shelveRows { get; set; }
            public int shelfSlots { get; set; }
            public int managerId { get; set; }

            public string  scaledWarehouseWidth { get; set; }
        public string scaledWarehouseLength { get; set; }

        public string scaledShelfLength { get; set; }

        public string scaledShelfWidth { get; set; }

        public int sections { get; set; }
    }
}