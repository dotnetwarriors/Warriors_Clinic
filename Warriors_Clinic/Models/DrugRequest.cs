using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

public partial class DrugRequest
{
    [Key]
    public int DrugRequestId { get; set; }

    public int? PhysicianId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? DrugInfoText { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequestDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? RequestStatus { get; set; }

    public bool IsDeletedByPhysician { get; set; } = false;

    public string? ChemistNote { get; set; }
    public int? ChemistId {  get; set; }
    public Chemist? Chemist { get; set; }

    [ForeignKey("PhysicianId")]
    public virtual Physician? Physician { get; set; }
}
