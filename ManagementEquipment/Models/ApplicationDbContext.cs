using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ManagementEquipment.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<AssignmentEquipment> AssignmentEquipments { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ResetPasswordToken).IsRequired(false);
            });

            builder.Entity<EquipmentType>(entity =>
            {
                entity.HasKey(e => e.EquipmentTypeId);
                entity.Property(e => e.EquipmentTypeId).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.EquipmentId);
                entity.Property(e => e.EquipmentId).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.HasOne<EquipmentType>(e => e.EquipmentType)
                        .WithMany(et => et.Equipments)
                        .HasForeignKey(e => e.EquipmentTypeId);
                
            });

            builder.Entity<AssignmentEquipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne<Equipment>(ass => ass.Equipment)
                        .WithMany(e => e.AssignmentEquipments)
                        .HasForeignKey(ass => ass.EquipmentId);
                entity.HasOne<ApplicationUser>(ass => ass.UserHandle)
                        .WithMany(u => u.HistoryHandles)
                        .HasForeignKey(ass => ass.UserIdOfHandle)
                        .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<ApplicationUser>(ass => ass.Employee)
                        .WithMany(u => u.HistoryEquipments)
                        .HasForeignKey(ass => ass.EmployeeId)
                        .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
