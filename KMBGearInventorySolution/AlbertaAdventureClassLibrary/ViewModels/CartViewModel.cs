//using AlbertaAdventureClassLibrary.ViewModels;


public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new();

    // NEW: Store reservation-wide date range
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public void RemoveItem(int gearId)
    {
        var item = Items.FirstOrDefault(i => i.GearID == gearId);
        if (item != null)
        {
            Items.Remove(item);
        }
    }

    public void Clear()
    {
        Items.Clear();
        StartDate = null;
        EndDate = null;
    }

    public class CartItem
    {
        public int GearID { get; set; }
        public string TagNumber { get; set; }
        public string CategoryName { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string Condition { get; set; }
        public string Description { get; set; }
        public int EstimatedUseHours { get; set; }
        public string Instructions { get; set; }
    }
}
