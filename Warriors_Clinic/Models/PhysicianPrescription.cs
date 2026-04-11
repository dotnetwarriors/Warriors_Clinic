using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

[Table("PhysicianPrescription")]
public class PhysicianPrescription
{
    public int PrescriptionId { get; set; }

    public int? PhysicianAdviceId { get; set; }   // FK

    public string? Prescription { get; set; }

    public int? DrugId { get; set; }

    public virtual Drug? Drug { get; set; }

    public PhysicianAdvice? PhysicianAdvice { get; set; }

    public string? Dosage { get; set; }

    public string? Timing { get; set; }

    public string? Duration { get; set; }

}
