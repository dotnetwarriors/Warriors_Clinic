using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warriors_Clinic.Models;

public partial class Message
{
    [Key]
    public int MessageId { get; set; }

    public int? SenderId { get; set; }
    public int? ReceiverId { get; set; }

    [Column(TypeName = "text")]
    public string? MessageText { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SentDate { get; set; }

    // ✅ REQUIRED (DO NOT REMOVE)
    public virtual User? Sender { get; set; }
    public virtual User? Receiver { get; set; }
}