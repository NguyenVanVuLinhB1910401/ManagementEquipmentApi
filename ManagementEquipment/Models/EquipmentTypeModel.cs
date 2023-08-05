using System.ComponentModel.DataAnnotations;

namespace ManagementEquipment.Models
{
    public class EquipmentTypeModel
    {
        public Guid EquipmentTypeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
