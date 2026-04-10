using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

[Table("PurchaseOrderHeader")]
public partial class PurchaseOrderHeader
{
    [Key]
    [Column("POId")]
    public int Poid { get; set; }

    [Column("PODate", TypeName = "datetime")]
    public DateTime? Podate { get; set; }

    public int? SupplierId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }  

    [InverseProperty("Po")]
    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();

    [ForeignKey("SupplierId")]
    [InverseProperty("PurchaseOrderHeaders")]
    public virtual Supplier? Supplier { get; set; }
}