using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertaAdventureClassLibrary.ViewModels
{
    public class ReservationViewModel
    {
        public int ReservationID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }

        public string FullName { get; set; } = string.Empty; // For display

        [Required(ErrorMessage = "Reservation status is required.")]
        public string Status { get; set; } = "Pending";

        public DateTime? RequestDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime? EndDate { get; set; }

        public string? ReservationInstructions { get; set; }

        [Required(ErrorMessage = "Estimated use hours must be provided.")]
        public int EstimatedUseHours { get; set; }
    }

}
