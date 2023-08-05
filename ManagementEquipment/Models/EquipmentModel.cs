using System.ComponentModel.DataAnnotations;

namespace ManagementEquipment.Models
{
    public class EquipmentModel
    {
        public Guid EquipmentId { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        [Required]
        public Guid EquipmentTypeId { get; set; }
    }
}
