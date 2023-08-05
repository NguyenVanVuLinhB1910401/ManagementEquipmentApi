using System.ComponentModel.DataAnnotations;

namespace ManagementEquipment.Models
{
    public class ReturnEquipmentModel
    {
        [Required]
        public IList<Guid> ListEquipmentId { get; set; }
    }
}
