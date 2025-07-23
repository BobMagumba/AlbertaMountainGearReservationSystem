using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertaAdventureClassLibrary.ViewModels
{
    public class GearInventoryViewModel
    {
        public int GearID { get; set; }

        [Required(ErrorMessage = "Tag number is required.")]
        public string TagNumber { get; set; } = string.Empty;

        public string? Picture { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = string.Empty; // For display

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required.")]
        public string Brand { get; set; } = string.Empty;

        public string? Size { get; set; }

        [Required(ErrorMessage = "Condition is required.")]
        public string Condition { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hours used must be provided.")]
        public int HoursUsed { get; set; }

        public int? HoursLimit { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

}
