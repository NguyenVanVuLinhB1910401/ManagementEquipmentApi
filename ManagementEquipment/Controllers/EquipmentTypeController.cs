using ManagementEquipment.Interfaces;
using ManagementEquipment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagementEquipment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EquipmentTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Employee)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _unitOfWork.EquipmentTypes.GetAll();
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var equipmentType = await _unitOfWork.EquipmentTypes.Get(id);
                if (equipmentType == null)
                {
                    return NotFound(new { Status = "Failure", Message = "EquipmentType does not exists." });
                }
                return Ok(equipmentType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddEquipmentType([FromBody] EquipmentTypeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var equipmentType = new EquipmentType()
                {
                    Name = model.Name,
                };
                var isEquipmentTypeExist = await _unitOfWork.EquipmentTypes.GetByName(model.Name);
                if (isEquipmentTypeExist != null)
                {
                    return BadRequest(new { Status = "Failure", Message = "EquipmentType already exists" });
                }

                await _unitOfWork.EquipmentTypes.Add(equipmentType);
                _unitOfWork.Complete();
                return Ok(new { Status = "Success", Message = "EquipmentType is created", EquipmentType = equipmentType });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateEquipmentType(Guid id, [FromBody] EquipmentTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != model.EquipmentTypeId)
            {
                return BadRequest(new { Status = "Failure", Message = "The Id is not the same" });
            }
            try
            {
                var updateEquipmentType = new EquipmentType()
                {
                    EquipmentTypeId = model.EquipmentTypeId,
                    Name = model.Name,
                };

                _unitOfWork.EquipmentTypes.Update(updateEquipmentType);
                _unitOfWork.Complete();
                return Ok(new { Status = "Success", Message = "EquipmentType is updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var equipmentType = await _unitOfWork.EquipmentTypes.Get(id);
                if (equipmentType != null)
                {
                    _unitOfWork.EquipmentTypes.Delete(equipmentType);
                    _unitOfWork.Complete();
                    return Ok(new { Status = "Succces", Message = "EquipmentType is deleted." });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
