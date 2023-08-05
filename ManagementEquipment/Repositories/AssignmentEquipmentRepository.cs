using ManagementEquipment.Interfaces;
using ManagementEquipment.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagementEquipment.Repositories
{
    public class AssignmentEquipmentRepository : GenericRepository<AssignmentEquipment>, IAssignmentEquipmentRepository
    {
        public AssignmentEquipmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AssignmentEquipment>> GetAllEquipmentAssignedForEmployee(string employeeId)
        {
            return await _context.AssignmentEquipments.Where(ass => ass.EmployeeId == employeeId && ass.Status == "Assigned").ToListAsync();
        }

        public async Task<AssignmentEquipment> GetEquipmentAssignedById(Guid equipmentId)
        {
            return await _context.AssignmentEquipments.FirstOrDefaultAsync(ass => ass.EquipmentId == equipmentId && ass.Status == "Assigned");
        }

        public async Task<IEnumerable<AssignmentEquipment>> HistoryByEquipmentId(Guid equipmentId)
        {
            return await _context.AssignmentEquipments
                                    .Where(ass => ass.EquipmentId == equipmentId)
                                    .OrderByDescending(ass => ass.Id)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<AssignmentEquipment>> HistoryHandleByUserId(string userId)
        {
            return await _context.AssignmentEquipments
                .Where(ass => ass.UserIdOfHandle == userId)
                .OrderByDescending(ass => ass.Id).ToListAsync();
        }

    }
}
