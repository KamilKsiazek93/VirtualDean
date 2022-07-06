namespace VirtualDean.Models
{
    public class PasswordUpdate
    {
        public int BrotherId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
