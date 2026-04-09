using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [ForeignKey("ReceiverId")]
    [InverseProperty("MessageReceivers")]
    public virtual User? Receiver { get; set; }

    [ForeignKey("SenderId")]
    [InverseProperty("MessageSenders")]
    public virtual User? Sender { get; set; }
}
