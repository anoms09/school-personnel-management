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
    public class FacultyController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly FacultyRepository _facultyRepository;

        public FacultyController(ILoggerFactory loggerFactory, FacultyRepository facultyRepository)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _facultyRepository = facultyRepository;
        }
        // GET: api/Faculty
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, int pageSize = 10, string searchKey = "")
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Parameters");

                var result = await _facultyRepository.GetFaculties(pageNumber, pageSize, searchKey);

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
                        x.FacultyName,
                        x.FacultyDescription,
                        x.FacultyCode
                    })
                });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to get faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }


        // GET: api/Faculty/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacultyById(long id)
        {
            try
            {
                if (id < 1)
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Id");

                var faculty = await _facultyRepository.GetFacultyById(id);

                if (faculty == null)
                    return Ok(new { responsecode = MyErrorCodes.Success, responseDescription = "No faculty Found" });

                return Ok(new { responseCode = MyErrorCodes.Success, faculty });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to get faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }

        [HttpGet("code")]
        public async Task<IActionResult> GetFacultyByCode([FromQuery] string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    throw new CustomException(MyErrorCodes.BadRequest, "Invalid Code");

                var faculty = await _facultyRepository.GetFacultyByCode(code);

                if (faculty == null)
                    return Ok(new { responsecode = MyErrorCodes.Success, responseDescription = "No faculty Found" });

                return Ok(new { responseCode = MyErrorCodes.Success, faculty });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to get faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to get faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }


        // POST: api/Faculty
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Faculty request)
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

                if (faculty != null)
                    return BadRequest(new { responseCode = MyErrorCodes.PropertyValueNotValid, responseDescription = "Faculty Code already exists" });

                await _facultyRepository.CreateNewFaculty(request);

                return Created("", new { responseCode = MyErrorCodes.Success, responseDescription = "Faculty Created.", faculty = request });

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to create faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to create faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = MyErrorCodes.UnexpectedError, responseDescription = "An unexpected error occured. Please try again later." });
            }
        }


        // PUT: api/Faculty/5
        [HttpPut()]
        public async Task<IActionResult> Put( [FromBody] Faculty request)
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
                
                 await _facultyRepository.UpdateFaculty(request);

                return Ok(new { responseCode = MyErrorCodes.Success, responseDescription = "Update Successful"});

            }
            catch (CustomException ex)
            {
                _logger.LogError($"Unable to Update faculty {ex.Message} | {ex.StackTrace}");
                return BadRequest(new { responseCode = ex.ErrorCode, responseDescription = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to update faculty {ex.Message} | {ex.StackTrace}");
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
