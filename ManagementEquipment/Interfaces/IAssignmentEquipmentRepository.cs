using ManagementEquipment.Models;

namespace ManagementEquipment.Interfaces
{
    public interface IAssignmentEquipmentRepository : IGenericRepository<AssignmentEquipment>
    {

        //Task<AssignmentEquipment> GetLatestAssignByEquipmentId(Guid equipmentId);
        Task<IEnumerable<AssignmentEquipment>> HistoryHandleByUserId(string userId);
        Task<IEnumerable<AssignmentEquipment>> HistoryByEquipmentId(Guid equipmentId);
        Task<AssignmentEquipment> GetEquipmentAssignedById(Guid equipmentId);
        Task<IEnumerable<AssignmentEquipment>> GetAllEquipmentAssignedForEmployee(string employeeId);
    }
}
