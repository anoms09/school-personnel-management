using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using School.Personnel.Management.Models.Miscellaneous;
using School.Personnel.Management.Repositories.AppAdmin;
using School.Personnel.Management.Repositories.Miscellaneous;

namespace School.Personnel.Management.Controllers.AppAdmin
{
    [Route("appadmin/api/v1/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly PermissionRepository _permissionRepository;

        public PermissionsController(ILoggerFactory loggerFactory, PermissionRepository permissionRepository)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _permissionRepository = permissionRepository;
        }


        // GET: api/Permissions
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber= 1, int pageSize =10, string searchKey = "")
        {
            try
            {
                if (pageNumber < 1 || pageSize <1)
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Parameters");

                var result = await _permissionRepository.GetPermissions(pageNumber, pageSize, searchKey);
                
                var totalCount = result != null && result.Any() ? result.FirstOrDefault().TotalCount : 0;

                return Ok(new
                {
                    ResponseCode = MyErrorCodes.Success,
                    totalCount,
                    pageSize,
                    pageNumber,
                    Permissions = result?.Select(x => new
                    {
                        x.Id,
                        x.PermissionName,
                        x.PermissionDescription,
                        x.IsAppAdminRole
                    })
                });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to get permission {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get permission {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }

        // GET: api/Permissions/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetPermissionById(long id)
        {
            try
            {
                if (id < 1)
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Id");

                var permission = await _permissionRepository.GetPermissionById(id);

                if (permission == null)
                    return Ok(new { responsecode = MyErrorCodes.Success, responseDescription = "No Permission Found" });

                return Ok(new { responseCode = MyErrorCodes.Success, permission });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to get permission {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get permission {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }

        // POST: api/Permissions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Permission request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = MyErrorCodes.PropertyValueNotValid,
                        ResponseDescription = HelpMe.StringifyValidationErrors(ModelState)
                    });
                

                await _permissionRepository.CreateNewPermission(request);
                
                return Created("", new { responseCode = MyErrorCodes.Success, responseDescription = "Permission Created.", permission = request });
                    
            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to create permission {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to create permission {ex.Message} | {ex.StackTrace}");
                return BadRequest(new {responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }

        // PUT: api/Permissions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
