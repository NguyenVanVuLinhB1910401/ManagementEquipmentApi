using System.ComponentModel.DataAnnotations;

namespace ManagementEquipment.Models
{
    public class AssignmentEquipmentModel
    {
        [Required(ErrorMessage = "EquipmentId is required")]
        public IList<Guid> ListEquipment { get; set; }

        [Required(ErrorMessage = "EmployeeId is required")]
        public string EmployeeId { get; set; }
    }
}
