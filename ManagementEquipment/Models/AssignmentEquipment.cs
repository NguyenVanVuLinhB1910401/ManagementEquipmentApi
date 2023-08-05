namespace ManagementEquipment.Models
{
    public class AssignmentEquipment
    {
        public int Id { get; set; }
        public string UserIdOfHandle { get; set; }
        public ApplicationUser? UserHandle { get; set; }
        public Guid EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUser? Employee { get; set; }
        public string Status { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
