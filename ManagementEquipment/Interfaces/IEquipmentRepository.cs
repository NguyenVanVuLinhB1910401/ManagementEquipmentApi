using ManagementEquipment.Models;

namespace ManagementEquipment.Interfaces
{
    public interface IEquipmentRepository : IGenericRepository<Equipment>
    {
        Task<IEnumerable<Equipment>> GetAllByStatus(string status);
        Task<IEnumerable<Equipment>> GetAllByIdType(Guid id);
   
    }
}
