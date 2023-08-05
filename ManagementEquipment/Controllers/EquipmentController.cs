using ManagementEquipment.Interfaces;
using ManagementEquipment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace ManagementEquipment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EquipmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Authorize( Roles = UserRoles.Employee )]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _unitOfWork.Equipments.GetAll();
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


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var equipment = await _unitOfWork.Equipments.Get(id);
                if (equipment == null)
                {
                    return NotFound(new { Status = "Failure", Message = "Equipment does not exists." });
                }
                return Ok(equipment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("equipmentType/{id}")]
        public async Task<IActionResult> GetByType(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _unitOfWork.Equipments.GetAllByIdType(id);
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

        [HttpGet]
        [Route("history/{id}")]
        public async Task<IActionResult> HistoryEquipment(Guid id)
        {
            try
            {
                var result = await _unitOfWork.AssignmentEquipments.HistoryByEquipmentId(id);
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



        [HttpPost]
        //[Authorize( Roles = UserRoles.Admin )]
        public async Task<IActionResult> AddEquipment([FromBody] EquipmentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var isEquipmentType = await _unitOfWork.EquipmentTypes.Get(model.EquipmentTypeId);
                if (isEquipmentType == null)
                {
                    return NotFound(new { Status = "Failure", Message = "EquipmentType does not exist!" });
                }
                Equipment equipment = new Equipment()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Status = EquipmentStatus.Available.ToString(),
                    EquipmentTypeId = model.EquipmentTypeId,
                };
                await _unitOfWork.Equipments.Add(equipment);
                _unitOfWork.Complete();
                return Ok(new { Status = "Success", Message = "Equipment is created.", Equipment = equipment });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userNameOfHandler = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            return await _unitOfWork.UserManager.FindByNameAsync(userNameOfHandler);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEquipment(Guid id, [FromBody] EquipmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != model.EquipmentId)
            {
                return BadRequest(new { Status = "Failure", Message = "The Id is not the same" });
            }
            try
            {
                var equipment = await _unitOfWork.Equipments.Get(model.EquipmentId);
                if(equipment == null)
                {
                    return BadRequest(new { Status = "Failure", Message = "Equipment doesn't exist." });
                }
                equipment.Name = model.Name;
                equipment.EquipmentTypeId = model.EquipmentTypeId;
                equipment.Description = model.Description;
                _unitOfWork.Equipments.Update(equipment);
                _unitOfWork.Complete();
                return Ok(new { Status = "Success", Message = "Equipment is updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete]
        [Route("{id}")]
        //[Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteEquipment(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var equipment = await _unitOfWork.Equipments.Get(id);
                if (equipment != null)
                {
                    _unitOfWork.Equipments.Delete(equipment);
                    _unitOfWork.Complete();
                    return Ok(new { Status = "Success", Message = "Equipment is deleted." });
                }
                else
                {
                    return NotFound(new { Status = "Failure", Message = "Equipment does not exist!" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("assign")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AssignmentEquipment([FromBody] AssignmentEquipmentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _unitOfWork.UserManager.FindByIdAsync(model.EmployeeId);
                if (user == null)
                {
                    return NotFound(new { Status = "Failure", Message = "Employee does not exists." });
                }
                var handler = await GetCurrentUser();
                for(int i = 0; i < model.ListEquipment.Count(); i++)
                {
                    var equipment = await _unitOfWork.Equipments.Get(model.ListEquipment[i]);
                    if (equipment == null)
                    {
                        return NotFound(new { Status = "Failure", Message = $"{model.ListEquipment[i]} does not exists." });
                    }
                    if (equipment.Status != "AVAILABLE")
                    {
                        return BadRequest(new { Status = "Failure", Message = $"{model.ListEquipment[i]} can not assign. Because Status = {equipment.Status}" });
                    }
                    equipment.Status = EquipmentStatus.InUse.ToString();
                    var assignEquipment = new AssignmentEquipment
                    {
                        EquipmentId = model.ListEquipment[i],
                        UserIdOfHandle = handler.Id,
                        EmployeeId = model.EmployeeId,
                        AssignmentDate = DateTime.Now,
                        Status = "Assigned"
                    };
                    await _unitOfWork.AssignmentEquipments.Add(assignEquipment);
                }    
                _unitOfWork.Complete();

                return Ok(new Response { Status = "Success", Message = "Equipment successfully assigned" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }



        [HttpPost]
        [Route("return")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> ReturnEquipment([FromBody] ReturnEquipmentModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                for(int i = 0; i < model.ListEquipmentId.Count(); i++)
                {
                    var equipment = await _unitOfWork.Equipments.Get(model.ListEquipmentId[i]);
                    if (equipment == null)
                    {
                        return NotFound(new { Status = "Failure", Message = $"{model.ListEquipmentId[i]} does not exists." });
                    }
                    if (equipment.Status != EquipmentStatus.InUse)
                    {
                        return BadRequest(new { Status = "Failure", Message = $"{model.ListEquipmentId[i]} EquipmentStatus is not IN-USE" });
                    }
                    var assigned = await _unitOfWork.AssignmentEquipments.GetEquipmentAssignedById(model.ListEquipmentId[i]);
                    if (assigned == null) return NotFound(new { Status = "Failure", Message = $"{model.ListEquipmentId[i]} Equipment does not be assigned." });
                    equipment.Status = EquipmentStatus.Available.ToString();
                    assigned.ReturnDate = DateTime.Now;
                    assigned.Status = "Returned";
                }
                _unitOfWork.Complete();

                return Ok(new Response { Status = "Success", Message = "Equipment successfully returned" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


       
    }
}
