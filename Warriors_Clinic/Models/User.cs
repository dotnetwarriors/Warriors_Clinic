using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Warriors_Clinic.Models;

public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Role { get; set; } = "Patient"; // ✅ default role

    [Column("ReferenceToId")]
    public int? ReferenceToId { get; set; } // ✅ nullable + fixed spelling

    public bool IsApproved { get; set; } = false;


    [StringLength(100)]
    public string? Email { get; set; }

    // Messaging System
    [InverseProperty("Receiver")]
    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    [InverseProperty("Sender")]
    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();
}