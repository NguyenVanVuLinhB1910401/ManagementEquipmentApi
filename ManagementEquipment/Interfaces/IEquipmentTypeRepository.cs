using ManagementEquipment.Models;

namespace ManagementEquipment.Interfaces
{
    public interface IEquipmentTypeRepository : IGenericRepository<EquipmentType>
    {
        Task<EquipmentType> GetByName(string name);
    }
}
