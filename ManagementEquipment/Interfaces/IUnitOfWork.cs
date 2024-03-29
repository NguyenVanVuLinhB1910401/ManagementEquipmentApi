﻿using ManagementEquipment.Models;
using Microsoft.AspNetCore.Identity;

namespace ManagementEquipment.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserManager<ApplicationUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        IEquipmentTypeRepository EquipmentTypes { get; }
        IEquipmentRepository Equipments { get; }
        IAssignmentEquipmentRepository AssignmentEquipments { get; }
        int Complete();
    }
}
