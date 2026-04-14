using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

public partial class Patient
{
    [Key]
    [Column("PatientID")]
    public int PatientId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("DOB")]
    public DateTime DOB { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Summary { get; set; } = null!;

    

    [InverseProperty("Patient")]
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
