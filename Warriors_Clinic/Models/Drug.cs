using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

public partial class Drug
{
    [Key]
    [Column("DrugID")]
    public int DrugId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Tittle { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    public DateOnly Expiry { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Dosage { get; set; } = null!;

    [InverseProperty("Drug")]
    public virtual ICollection<PhysicianPrescription> PhysicianPrescriptions { get; set; } = new List<PhysicianPrescription>();

    [InverseProperty("Drug")]
    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();
}
