namespace ManagementEquipment.Models
{
    public class EquipmentType
    {
        public Guid EquipmentTypeId { get; set; }
        public string Name { get; set; }
        public IList<Equipment>? Equipments { get; set; }
    }
}
