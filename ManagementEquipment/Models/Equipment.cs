namespace ManagementEquipment.Models
{
    public class Equipment
    {
        public Guid EquipmentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public EquipmentType? EquipmentType { get; set; }
        public IList<AssignmentEquipment>? AssignmentEquipments { get; set; }
    }
}
