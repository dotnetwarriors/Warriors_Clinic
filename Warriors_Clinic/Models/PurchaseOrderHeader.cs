using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warriors_Clinic.Models
{
    [Table("PurchaseOrderHeader")]
    public class PurchaseOrderHeader
    {
        [Key]
        public int Poid { get; set; }

        public DateTime? Podate { get; set; }

        public int? SupplierId { get; set; }
        public string? SupplierNote { get; set; }
        public string? Status { get; set; } = "Pending";

        // Navigation
        public virtual Supplier? Supplier { get; set; }

        public bool IsVisible { get; set; } = true;

       

        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
            = new List<PurchaseOrderLine>();
    }
}
