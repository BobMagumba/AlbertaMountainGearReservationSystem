using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertaAdventureClassLibrary.ViewModels
{
    public class ReservationGearViewModel
    {
        public int ReservationGearID { get; set; }

        [Required(ErrorMessage = "Reservation ID is required.")]
        public int ReservationID { get; set; }

        [Required(ErrorMessage = "Gear ID is required.")]
        public int GearID { get; set; }

        public string TagNumber { get; set; } = string.Empty; // For display
        public string GearDescription { get; set; } = string.Empty; // For display
    }

}
