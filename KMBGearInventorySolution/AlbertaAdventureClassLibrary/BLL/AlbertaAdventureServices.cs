using AlbertaAdventureClassLibrary.DAL;
using AlbertaAdventureClassLibrary.Entities;
using AlbertaAdventureClassLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertaAdventureClassLibrary.BLL
{
    public class AlbertaAdventureServices
    {
        private readonly aamcgeardbContext _context;

        public AlbertaAdventureServices(aamcgeardbContext context)
        {
            _context = context;
        }

        //Method to View All Categories in a list ordered by CategoryName

        public async Task<List<CategoryViewModel>> GetAllCategories()
        {
            List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            categoryViewModels = await _context.Categories
                .Select(c => new CategoryViewModel
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName
                })
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
            return categoryViewModels;
        }

        //Method to View All GearInventory in a list ordered TagNumber
        public async Task<List<GearInventoryViewModel>> GetAllGearInventory(DateTime startDate, DateTime endDate)
        {
            List<GearInventoryViewModel> gearInventoryViewModels = new List<GearInventoryViewModel>();
            gearInventoryViewModels = await _context.GearInventories
                .Select(g => new GearInventoryViewModel
                {
                    GearID = g.GearID,
                    TagNumber = g.TagNumber,
                    Picture = g.Picture,
                    CategoryID = g.CategoryID,
                    CategoryName = g.Category.CategoryName,
                    Description = g.Description,
                    Brand = g.Brand,
                    Size = g.Size,
                    Condition = g.Condition,
                    HoursUsed = g.HoursUsed,
                    HoursLimit = g.HoursLimit,
                })
                .OrderBy(g => g.TagNumber)
                .ToListAsync();



            List<int> reservedGearIDs = new();
            reservedGearIDs = await DetermineGearInventoryReservations(startDate, endDate);
            foreach (GearInventoryViewModel g in gearInventoryViewModels)
            {
                if (reservedGearIDs.Contains(g.GearID))
                {
                    g.IsAvailable = false;
                }
                else
                {
                    g.IsAvailable = true;
                    //g.CategoryName = "Hello";
                }
            }
            return gearInventoryViewModels;
        }

        


        //Method to determine if gear is reserved during the inputted period by checking it against its attached reservations
        public async Task<List<int>> DetermineGearInventoryReservations(DateTime startDate, DateTime endDate)
        {
            List<int> reservedGear = new List<int>();
            reservedGear = await _context.GearInventories
                .Join(
                _context.ReservationGears,
                GearInventory => GearInventory.GearID,
                ReservationGear => ReservationGear.GearID,
                (GearInventory, ReservationGear) => new { GearInventory, ReservationGear }
                )
                .Join(
                _context.Reservations,
                gearGearReservation => gearGearReservation.ReservationGear.ReservationID,
                Reservation => Reservation.ReservationID,
                (gearGearReservation, Reservation) => new { gearGearReservation.GearInventory, gearGearReservation.ReservationGear, Reservation }
                )
                .Where(joined =>
                    joined.Reservation.StartDate < endDate && joined.Reservation.EndDate > startDate

                )
                .Select(joined => joined.GearInventory.GearID)
                .ToListAsync();
            return reservedGear;
        }

        //Method to Get All Submitted Gear Reservations Waiting Approval
        public async Task<List<ReservationViewModel>> GetAllSubmittedGearReservationsWaitingApproval()
        {
            List<ReservationViewModel> reservationViewModels = new List<ReservationViewModel>();
            reservationViewModels = await _context.Reservations
                .Where(r => r.Status == "Pending")
                .Select(r => new ReservationViewModel
                {
                    ReservationID = r.ReservationID,
                    UserID = r.UserID,
                    RequestDate = r.RequestDate,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    EstimatedUseHours = r.EstimatedUseHours,
                    ReservationInstructions = r.ReservationInstructions,
                })
                .OrderBy(r => r.RequestDate)
                .ToListAsync();
            return reservationViewModels;
        }

        //Get GearInventory by GearID
        public async Task<GearInventoryViewModel> GetGearInventory(int gearID)
        {
            GearInventoryViewModel gearInventoryViewModel = new GearInventoryViewModel();
            gearInventoryViewModel = await _context.GearInventories
                .Where(g => g.GearID == gearID)
                .Select(g => new GearInventoryViewModel
                {
                    GearID = g.GearID,
                    TagNumber = g.TagNumber,
                    Picture = g.Picture,
                    CategoryID = g.CategoryID,
                    CategoryName = g.Category.CategoryName,
                    Description = g.Description,
                    Brand = g.Brand,
                    Size = g.Size,
                    Condition = g.Condition,
                    HoursUsed = g.HoursUsed,
                    HoursLimit = g.HoursLimit,
                })
                .FirstOrDefaultAsync();

            //gets a list of gear currently reserved at some point during the inputted span of time,
            //and makes the gear unavaliable if it intersects
            //List<int> reservedGearIDs = new();
            //reservedGearIDs = await DetermineGearInventoryReservations(startDate, endDate);

            //        if (reservedGearIDs.Contains(gearInventoryViewModel.GearID))
            //        {
            //            gearInventoryViewModel.IsAvailable = false;
            //        }
            //        else
            //        {
            //            gearInventoryViewModel.IsAvailable = true;
            //        }

            return gearInventoryViewModel;
        }

        //Get Users Reserved Gear by UserID
        public async Task<List<ReservationViewModel>> GetUsersReservedGear(int userID)
        {
            //this should be catched so that we dont have make a call to the database if the user has no reservations
            List<ReservationViewModel> reservationViewModels = new List<ReservationViewModel>();
            reservationViewModels = await _context.Reservations
                .Where(r => r.UserID == userID)
                .Select(r => new ReservationViewModel
                {
                    ReservationID = r.ReservationID,
                    UserID = r.UserID,


                })
                .ToListAsync();
            return reservationViewModels;
        }

        // Add a reservation async method
        public async Task<int> AddReservationAsync(ReservationViewModel reservationModel)
        {
            int hardcodedUserId = 30; // Hardcoded UserID

            // Create a new reservation object
            var reservation = new Reservation
            {
                UserID = hardcodedUserId,
                Status = reservationModel.Status,
                StartDate = (DateTime)reservationModel.StartDate,
                EndDate = (DateTime)reservationModel.EndDate,
                ReservationInstructions = reservationModel.ReservationInstructions,
                EstimatedUseHours = reservationModel.EstimatedUseHours,
                RequestDate = DateTime.UtcNow
            };

            // Add reservation to the database
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Return the newly created ReservationID
            return reservation.ReservationID;
        }

        // Call the AddReservationGearAsync method to create the relation between the reservation and the gear because of the many-to-many relationship
        public async Task AddReservationGearAsync(int reservationID, List<int> gearIDs)
        {
            foreach (var gearID in gearIDs)
            {
                var reservationGear = new ReservationGear
                {
                    ReservationID = reservationID,
                    GearID = gearID
                };
                _context.ReservationGears.Add(reservationGear);
            }
            await _context.SaveChangesAsync();
        }

        //CalL a method to update the gear status

        public async Task UpdateGearStatusAsync(int gearID, bool isAvailable)
        {
            var gear = await _context.GearInventories.FindAsync(gearID);
            gear.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
        }



        public async Task AddReservation(List<GearInventory> reservedGearlist, Reservation reservation)
        {
            _context.Reservations.Add(reservation);

            await _context.SaveChangesAsync();

            int reservationID;
            reservationID = await _context.Reservations
            .DefaultIfEmpty() // In case the table is empty, use a default value
            .MaxAsync(r => r.ReservationID); //pulls the largest (therefore most recent) ID


            foreach (GearInventory g in reservedGearlist)
            {
                ReservationGear reservationGear = new ReservationGear();
                reservationGear.ReservationID = reservationID;
                reservationGear.GearID = g.GearID;
                AddReservationGear(reservationGear);
            }

            await _context.SaveChangesAsync();
        }

        //Method to populate ReservationGear when a reservation is placed
        public void AddReservationGear(ReservationGear reservationGear)
        {
            _context.ReservationGears.Add(reservationGear);

        }

        public async Task<GearInventory?> GetGearInventoryById(int gearID)
        {
            return await _context.GearInventories.FirstOrDefaultAsync(g => g.GearID == gearID);
        }


        public async Task DeleteResevervation(Reservation reservation)
        {
            if (await _context.Reservations.AnyAsync(r => r.ReservationID == reservation.ReservationID))
            {
                List<ReservationGear> reservationGearsToDelete = new();
                reservationGearsToDelete = _context.ReservationGears.Where(r => r.ReservationID == reservation.ReservationID).ToList();

                foreach (ReservationGear rg in reservationGearsToDelete)
                {
                    DeleteReservationGear(rg);
                }

                await _context.SaveChangesAsync();

                _context.Reservations.Remove(reservation);

                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Resevervation not found");
            }


        }

        public void DeleteReservationGear(ReservationGear reservationGear)
        {
            _context.ReservationGears.Remove(reservationGear);
        }

        //Method to update GearInventory.
        public async Task<bool> UpdateGearInventory(GearInventoryViewModel gearData)
        {
            var existingGear = await _context.GearInventories.FindAsync(gearData.GearID);
            List<Exception> errorlist = new List<Exception>();

            if (gearData.GearID == 0)
                errorlist.Add(new ArgumentException("Gear ID must be given."));

            if (string.IsNullOrEmpty(gearData.TagNumber))
                errorlist.Add(new ArgumentException("Tag number must be given."));

            if (gearData.CategoryID == 0)
                errorlist.Add(new ArgumentException("Category ID must be given."));

            if (string.IsNullOrEmpty(gearData.Description))
                errorlist.Add(new ArgumentException("Description must be given."));

            if (string.IsNullOrEmpty(gearData.Brand))
                errorlist.Add(new ArgumentException("Brand must be given."));

            if (gearData.HoursUsed < 0)
                errorlist.Add(new ArgumentException("Hours used cannot be negative."));

            if (errorlist.Count > 0)
            {
                //_context.ChangeTracker.Clear();
                throw new AggregateException("Unable to update the piece of gear. Check concerns.", errorlist);
            }

            if (existingGear != null)
            {
                existingGear.TagNumber = gearData.TagNumber;
                existingGear.CategoryID = gearData.CategoryID;
                existingGear.Description = gearData.Description;
                existingGear.Brand = gearData.Brand;
                existingGear.Size = gearData.Size;
                existingGear.Condition = gearData.Condition;
                existingGear.HoursUsed = gearData.HoursUsed;
                // Keep existing value if null
                existingGear.HoursLimit = gearData.HoursLimit ?? existingGear.HoursLimit;
                existingGear.IsAvailable = gearData.IsAvailable;

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteGearInventory(int gearId)
        {
            var gear = await _context.GearInventories.FindAsync(gearId);

            if (gearId <= 0)
            {
                throw new ArgumentException("Invalid Gear ID provided.", nameof(gearId));
            }
            if (gear == null)
            {
                return false;
            }

            _context.GearInventories.Remove(gear);
            await _context.SaveChangesAsync();

            return true;
        }

        //My Reservations. Methods to get all reservations for a user, order by start date and display all fields in the view model
        public async Task<List<ReservationViewModel>> GetAllReservationsByUserId(int userId)
        {
            List<ReservationViewModel> reservationViewModels = new List<ReservationViewModel>();
            reservationViewModels = await _context.Reservations
                .Where(r => r.UserID == userId)
                .Select(r => new ReservationViewModel
                {
                    ReservationID = r.ReservationID,
                    UserID = r.UserID,
                    Status = r.Status,
                    RequestDate = r.RequestDate,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    EstimatedUseHours = r.EstimatedUseHours,
                    ReservationInstructions = r.ReservationInstructions,
                })
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
            return reservationViewModels;
        }

        // Method to get all gear for a specific reservation
        public async Task<List<GearInventoryViewModel>> GetGearByReservationId(int reservationId)
        {
            var gearList = await _context.ReservationGears
                .Where(rg => rg.ReservationID == reservationId)
                .Join(_context.GearInventories,
                      rg => rg.GearID,
                      g => g.GearID,
                      (rg, g) => g)
                .Include(g => g.Category) // assuming navigation property Category is configured
                .Select(g => new GearInventoryViewModel
                {
                    GearID = g.GearID,
                    TagNumber = g.TagNumber,
                    Brand = g.Brand,
                    CategoryID = g.CategoryID,
                    CategoryName = g.Category.CategoryName,
                    Size = g.Size,
                    Condition = g.Condition,
                    Description = g.Description,
                    HoursLimit = g.HoursLimit,
                    
                })
                .ToListAsync();

            return gearList;
        }


    }
}
