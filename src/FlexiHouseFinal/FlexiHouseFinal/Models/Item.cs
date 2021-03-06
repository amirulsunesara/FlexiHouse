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
    
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.item_Order = new HashSet<item_Order>();
            this.Item_Consignment = new HashSet<Item_Consignment>();
            this.Item_Warehouse = new HashSet<Item_Warehouse>();
        }
    
        public int id { get; set; }
        public string itemName { get; set; }
        public string Manufacturer { get; set; }
        public string Country { get; set; }
        public string itemCode { get; set; }
        public string Category { get; set; }
        public Nullable<int> itemDetails { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<item_Order> item_Order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item_Consignment> Item_Consignment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item_Warehouse> Item_Warehouse { get; set; }
        public virtual itemDetail itemDetail { get; set; }
    }
}
