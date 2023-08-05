using ManagementEquipment.Interfaces;
using ManagementEquipment.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementEquipment.Repositories
{
    public class EquipmentTypeRepository : GenericRepository<EquipmentType>, IEquipmentTypeRepository
    {
        public EquipmentTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<EquipmentType> GetByName(string name)
        {
            return await _context.EquipmentTypes.Where(et => et.Name == name).FirstOrDefaultAsync();
        }
    }
}
