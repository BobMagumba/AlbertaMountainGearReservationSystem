﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AlbertaAdventureClassLibrary.Entities;

public partial class Reservation
{
    [Key]
    public int ReservationID { get; set; }

    public int UserID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequestDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ReservationInstructions { get; set; }

    public int EstimatedUseHours { get; set; }

    [InverseProperty("Reservation")]
    public virtual ICollection<ReservationGear> ReservationGears { get; set; } = new List<ReservationGear>();

    [ForeignKey("UserID")]
    [InverseProperty("Reservations")]
    public virtual User User { get; set; }
}