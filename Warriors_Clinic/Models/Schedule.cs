using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

[Table("Schedule")]
public partial class Schedule
{
    [Key]
    public int ScheduleId { get; set; }

    public int? AppointmentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ScheduleDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ScheduleStatus { get; set; }

    [ForeignKey("AppointmentId")]
    [InverseProperty("Schedules")]
    public virtual Appointment? Appointment { get; set; }

    [InverseProperty("Schedule")]
    public virtual ICollection<PhysicianAdvice> PhysicianAdvices { get; set; } = new List<PhysicianAdvice>();
}
