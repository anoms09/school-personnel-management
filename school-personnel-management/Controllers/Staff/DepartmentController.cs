using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using School.Personnel.Management.Models.Miscellaneous;
using School.Personnel.Management.Models.Staff;
using School.Personnel.Management.Repositories.Miscellaneous;
using School.Personnel.Management.Repositories.Staff;

namespace School.Personnel.Management.Controllers.Staff
{
    [Route("staff/api/v1/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly DepartmentRepository _departmentRepository;
        private readonly FacultyRepository _facultyRepository;

        public DepartmentController(ILoggerFactory loggerFactory, DepartmentRepository departmentRepository, FacultyRepository facultyRepository)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _departmentRepository = departmentRepository;
            _facultyRepository = facultyRepository;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, int pageSize = 10, string searchKey = "")
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Parameters");

                var result = await _departmentRepository.GetDepartments(pageNumber, pageSize, searchKey);

                var totalCount = result != null && result.Any() ? result.FirstOrDefault().TotalCount : 0;

                return Ok(new
                {
                    ResponseCode = MyErrorCodes.Success,
                    totalCount,
                    pageSize,
                    pageNumber,
                    Faculties = result?.Select(x => new
                    {
                        x.Id,
                        x.DeptName,
                        x.DeptDescription,
                        x.DeptCode,
                        x.FacultyCode
                    })
                });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to get department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }


        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(long id)
        {
            try
            {
                if (id < 1)
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Id");

                var department = await _departmentRepository.GetDepartmentById(id);

                if (department == null)
                    return Ok(new { responsecode = MyErrorCodes.Success, responseDescription = "No department Found" });

                return Ok(new { responseCode = MyErrorCodes.Success, department });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to get department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }

        [HttpGet("code")]
        public async Task<IActionResult> GetDepartmentByCode([FromQuery] string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Code");

                var department = await _departmentRepository.GetDepartmentByCode(code);

                if (department == null)
                    return Ok(new { responsecode = MyErrorCodes.Success, responseDescription = "No Department Found" });

                return Ok(new { responseCode = MyErrorCodes.Success, department });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to get department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }

        // POST: api/Department
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Department request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = MyErrorCodes.PropertyValueNotValid,
                        ResponseDescription = HelpMe.StringifyValidationErrors(ModelState)
                    });

                var faculty = await _facultyRepository.GetFacultyByCode(request.FacultyCode);

                if (faculty == null)
                    return BadRequest(new { responseCode = MyErrorCodes.BadRequest, responseDescription = "Faculty does not exist." });

                var department = await _departmentRepository.GetDepartmentByCode(request.DeptCode);

                if (department != null)
                    return BadRequest(new { responseCode = MyErrorCodes.PropertyValueNotValid, responseDescription = "Department Code already exists" });         

                await _departmentRepository.CreateNewDepartment(request);

                return Created("", new { responseCode = MyErrorCodes.Success, responseDescription = "Department Created.", department = request });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to create department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to create department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }

        // PUT: api/Department/5
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] Department request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        ResponseCode = MyErrorCodes.PropertyValueNotValid,
                        ResponseDescription = HelpMe.StringifyValidationErrors(ModelState)
                    });

                if (request.Id < 1)
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Id");

                var faculty = await _facultyRepository.GetFacultyByCode(request.FacultyCode);

                if (faculty == null)
                    return BadRequest(new { responseCode = MyErrorCodes.BadRequest, responseDescription = "Faculty does not exist." });
                
                var department = await _departmentRepository.GetDepartmentByCode(request.DeptCode);

                if (department == null || department.Id != request.Id)
                    return BadRequest(new { responseCode = MyErrorCodes.PropertyValueNotValid, responseDescription = "Invalid department Code/id combination" });

                await _departmentRepository.UpdateDepartment(request);

                return Ok(new { responseCode = MyErrorCodes.Success, responseDescription = "Update Successful" });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to Update department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to update department {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
