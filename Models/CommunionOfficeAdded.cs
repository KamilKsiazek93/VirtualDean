namespace VirtualDean.Models
{
    public class CommunionOfficeAdded
    {
        public int Id { get; set; }
        public int BrotherId { get; set; }
        public int WeekOfOffices { get; set; }
        public string CommunionHour { get; set; }
    }
}
