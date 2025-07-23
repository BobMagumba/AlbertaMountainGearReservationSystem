using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertaAdventureClassLibrary.Entities
{
    //a singleton class designed to facilitate storing the users selected items for reservation, according to the date range selected
    public class ReservationCart
    {

        private static ReservationCart _instance;
        private static readonly object _lock = new object();

        //[Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }

        // [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string ReservationInstructions { get; set; } 

        public int EstimatedUseHours { get; set; }

        public List<GearInventory> ReservedGearCart { get; set; }

        public static ReservationCart Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ReservationCart();
                    }
                    return _instance;
                }
            }
        }

        private ReservationCart()
        {
            ReservedGearCart = new List<GearInventory>();
            StartDate = new();
            EndDate = new();
        }
    }
}
