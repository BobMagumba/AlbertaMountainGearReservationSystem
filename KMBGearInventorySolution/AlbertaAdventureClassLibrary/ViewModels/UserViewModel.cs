using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertaAdventureClassLibrary.ViewModels
{
    public class UserViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; } = string.Empty;
    }

}
