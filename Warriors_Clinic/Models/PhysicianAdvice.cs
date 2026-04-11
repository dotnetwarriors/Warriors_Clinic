using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

[Table("PhysicianAdvice")]
public partial class PhysicianAdvice
{
    [Key]
    public int PhysicianAdviceId { get; set; }

   
    [Column(TypeName = "text")]
    public string Advice { get; set; }

    
    public int PatientId {  get; set; }



    [InverseProperty("PhysicianAdvice")]
    public virtual ICollection<PhysicianPrescription> PhysicianPrescriptions { get; set; } = new List<PhysicianPrescription>();

 
}
