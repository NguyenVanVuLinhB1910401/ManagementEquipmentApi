using Microsoft.AspNetCore.Identity;

namespace ManagementEquipment.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? CCCD { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public string? MobilePhone { get; set; }

        public string? Location { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExprityTime { get; set; }
        public string? ResetPasswordToken { get; set; }
        public IEnumerable<AssignmentEquipment> HistoryEquipments { get; set; }
        public IEnumerable<AssignmentEquipment> HistoryHandles { get; set; }
    }
}
