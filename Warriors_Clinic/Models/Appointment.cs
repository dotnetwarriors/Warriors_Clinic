using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

    public partial class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int? PatientId { get; set; }

        public int? PhysicianId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? AppointmentDateTime { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? Criticality { get; set; }

        [StringLength(255)]
        [Unicode(false)]
        public string? Reason { get; set; }

        [StringLength(255)]
        [Unicode(false)]
        public string? Note { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? ScheduleStatus { get; set; }

        [ForeignKey("PatientId")]
        [InverseProperty("Appointments")]
        public virtual Patient? Patient { get; set; }

        [ForeignKey("PhysicianId")]
        [InverseProperty("Appointments")]
        public virtual Physician? Physician { get; set; }

        public bool IsVisibleToDoctor { get; set; } = true;


        [InverseProperty("Appointment")]
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }


