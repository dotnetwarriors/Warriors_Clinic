using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

[Table("PhysicianPrescription")]
public partial class PhysicianPrescription
{
    [Key]
    public int PrescriptionId { get; set; }

    public int? PhysicianAdviceId { get; set; }

    public int? DrugId { get; set; }

    [Column(TypeName = "text")]
    public string? Prescription { get; set; }

    [ForeignKey("DrugId")]
    [InverseProperty("PhysicianPrescriptions")]
    public virtual Drug? Drug { get; set; }

    [ForeignKey("PhysicianAdviceId")]
    [InverseProperty("PhysicianPrescriptions")]
    public virtual PhysicianAdvice? PhysicianAdvice { get; set; }
}
