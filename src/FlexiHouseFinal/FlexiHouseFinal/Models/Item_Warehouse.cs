//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlexiHouseFinal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Item_Warehouse
    {
        public int itemId { get; set; }
        public int warehouseId { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<int> orders { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
