namespace VirtualDean.Models
{
    public class Office
    {
        public int Id { get; set; }
        public int BrotherId { get; set; }
        public int WeekOfOffices { get; set; }
        public string CantorOffice { get; set; }
        public string LiturgistOffice { get; set; }
        public string DeanOffice { get; set; }
    }
}
