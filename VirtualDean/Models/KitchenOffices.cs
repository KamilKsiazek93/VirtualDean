namespace VirtualDean.Models
{
    public class KitchenOffices
    {
        public int Id { get; set; }
        public int BrotherId { get; set; }
        public int WeekOfOffices { get; set; }
        public string SaturdayOffices { get; set; }
        public string SundayOffices { get; set; }
    }
}
