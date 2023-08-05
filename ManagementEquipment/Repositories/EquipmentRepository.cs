using ManagementEquipment.Interfaces;
using ManagementEquipment.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementEquipment.Repositories
{
    public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Equipment>> GetAll()
        {
            return await _context.Equipments.Include(e => e.EquipmentType).ToListAsync();
        }
        public async Task<IEnumerable<Equipment>> GetAllByIdType(Guid id)
        {
            return await _context.Equipments.Where(e => e.EquipmentTypeId == id).ToListAsync();
        }

        public async Task<IEnumerable<Equipment>> GetAllByStatus(string status)
        {
            return await _context.Equipments.Where(e => e.Status == status).ToListAsync();
        }
       
    }
}
