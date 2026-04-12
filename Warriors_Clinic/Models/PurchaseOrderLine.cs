using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warriors_Clinic.Models
{
    [Table("PurchaseOrderLine")]
    public class PurchaseOrderLine
    {
        [Key]
        public int PolineId { get; set; }

        public int? Poid { get; set; }

        public int? DrugId { get; set; }

        public int? Quantity { get; set; }

        public string? Note { get; set; }

        // Navigation
        [ForeignKey("Poid")]
        public virtual PurchaseOrderHeader? PurchaseOrderHeader { get; set; }

        [ForeignKey("DrugId")]
        public virtual Drug? Drug { get; set; }
    }
}
