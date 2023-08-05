using ManagementEquipment.Interfaces;
using ManagementEquipment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ManagementEquipment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetAll()
        {
            var userWithRoles = new List<UserModel>();
            try
            {
                var result = await _unitOfWork.UserManager.Users.ToListAsync();
                foreach (var user in result)
                {
                    var roles = await _unitOfWork.UserManager.GetRolesAsync(user);
                    userWithRoles.Add(new UserModel() { 
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        UserName = user.UserName,
                        Roles = roles
                    });

                }
                if (result.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(userWithRoles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("equipmentasssigned/{employeeId}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetAllEquipmentAssignedForEmployee(string employeeId)
        {
            try
            {
                var result = await _unitOfWork.AssignmentEquipments.GetAllEquipmentAssignedForEmployee(employeeId);
                if (result.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
