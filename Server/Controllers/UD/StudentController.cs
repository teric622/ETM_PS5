using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;



namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentController : BaseController, GenericRestController<StudentDTO>
    {
        public StudentController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }


        //student tbale has composite primary key
        //studentid, schoolid
        [HttpDelete]
        [Route("Delete/{StudentId}/{SchoolId}")]
        public async Task<IActionResult> Delete(int StudentId, int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Students.Where(x => x.StudentId == StudentId && x.SchoolId == SchoolId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Students.Remove(itm);
                }

                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        public Task<IActionResult> Delete(int KeyVal)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Students.Select(sp => new StudentDTO
                {
                    StudentId = sp.StudentId,
                    Salutation = sp.Salutation,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    Employer = sp.Employer,
                    RegistrationDate = sp.RegistrationDate,
                    StreetAddress = sp.StreetAddress,
                    Zip = sp.Zip,
                    Phone = sp.Phone,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    SchoolId = sp.SchoolId,
                })
                .ToListAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }


        //student tbale has composite primary key
        //studentid, schoolid
        [HttpGet]
        [Route("Get/{StudentId}/{SchoolId}")]
        public async Task<IActionResult> Get(int StudentId, int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                StudentDTO? result = await _context
                    .Students
                    .Where(x => x.StudentId == StudentId && x.SchoolId == SchoolId)
                     .Select(sp => new StudentDTO
                     {
                         StudentId = sp.StudentId,
                         Salutation = sp.Salutation,
                         FirstName = sp.FirstName,
                         LastName = sp.LastName,
                         StreetAddress = sp.StreetAddress,
                         Zip = sp.Zip,
                         Phone = sp.Phone,
                         Employer = sp.Employer,
                         RegistrationDate = sp.RegistrationDate,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate,
                         SchoolId = sp.SchoolId,
                     })
                .SingleOrDefaultAsync();

                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        public Task<IActionResult> Get(int KeyVal)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody]
                                                StudentDTO _StudentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId && x.SchoolId == _StudentDTO.SchoolId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    
                    //studentid, schoolid requried for composite primary key
                    // fields would make sense all others not made by triggers to be needed
                    // when new instance made
                    Student s = new Student
                    {
                        StudentId = _StudentDTO.StudentId,
                        Salutation = _StudentDTO.Salutation,
                        FirstName = _StudentDTO.FirstName,
                        Phone = _StudentDTO.Phone,
                        Employer = _StudentDTO.Employer,
                        LastName = _StudentDTO.LastName,
                        StreetAddress = _StudentDTO.StreetAddress,
                        Zip = _StudentDTO.Zip,
                        RegistrationDate = _StudentDTO.RegistrationDate,
                        SchoolId = _StudentDTO.SchoolId,
                    };
                    _context.Students.Add(s);
                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();

                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody]
                                                StudentDTO _StudentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId && x.SchoolId == _StudentDTO.SchoolId).FirstOrDefaultAsync();


                //Only fields user shouldnt be able to edit are those correlated to the primary composit key
                // and the ones made by triggers, all others are okay to edit or modify
                if (itm != null)
                {
                    itm.Salutation = _StudentDTO.Salutation;
                    itm.FirstName = _StudentDTO.FirstName;
                    itm.LastName = _StudentDTO.LastName;
                    itm.StreetAddress = _StudentDTO.StreetAddress;
                    itm.Zip = _StudentDTO.Zip;
                    itm.Phone = _StudentDTO.Phone;
                    itm.Employer = _StudentDTO.Employer;
                    itm.RegistrationDate = _StudentDTO.RegistrationDate;
                    _context.Students.Update(itm);
                }

                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
    }
}
