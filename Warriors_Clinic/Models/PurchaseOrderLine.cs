using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

[Table("PurchaseOrderLine")]
public partial class PurchaseOrderLine
{
    [Key]
    [Column("POLineId")]
    public int PolineId { get; set; }

    [Column("POId")]
    public int? Poid { get; set; }

    public int? DrugId { get; set; }

    public int? Quantity { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Note { get; set; }

    [ForeignKey("DrugId")]
    [InverseProperty("PurchaseOrderLines")]
    public virtual Drug? Drug { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PurchaseOrderLines")]
    public virtual PurchaseOrderHeader? Po { get; set; }
}
