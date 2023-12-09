using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;
//using Telerik.Blazor.Components;
//using Telerik.DataSource.Extensions;
//using Telerik.SvgIcons;

namespace OCTOBER.Server.Controllers.UD
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BaseController, GenericRestController<EnrollmentDTO>
    {
        public EnrollmentController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }
        //Enrollment has composite primary key
        //sectionId, StudentID, schoolid

        [HttpDelete]
        [Route("Delete/{SectionId}/{StudentId}/{SchoolId}")]
        public async Task<IActionResult> Delete(int SectionId, int SchoolId, int StudentId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x => x.SectionId == SectionId && x.StudentId == StudentId && x.SchoolId == SchoolId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Enrollments.Remove(itm);
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

                var result = await _context.Enrollments.Select(sp => new EnrollmentDTO
                {
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    EnrollDate = sp.EnrollDate,
                    FinalGrade = sp.FinalGrade,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    SchoolId = sp.SchoolId
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
        //Enrollment has composite primary key
        //sectionId, StudentID, schoolid

        [HttpGet]
        [Route("Get/{SectionId}/{StudentId}/{SchoolId}")]
        public async Task<IActionResult> Get(int SectionId, int StudentId, int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                EnrollmentDTO? result = await _context
                    .Enrollments
                    .Where(x => x.SectionId == SectionId && x.StudentId == StudentId && x.SchoolId == SchoolId)
                     .Select(sp => new EnrollmentDTO
                     {
                         StudentId = sp.StudentId,
                         SectionId = sp.SectionId,
                         EnrollDate = sp.EnrollDate,
                         FinalGrade = sp.FinalGrade,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate,
                         SchoolId = sp.SchoolId
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
                                                EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x => x.SectionId == _EnrollmentDTO.SectionId && x.StudentId == _EnrollmentDTO.StudentId && x.SchoolId == _EnrollmentDTO.SchoolId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    Enrollment e = new Enrollment
                    {
                        //Enrollment has composite primary key
                        //sectionId, StudentID, schoolid are requried for new Enrollment instance
                        StudentId = _EnrollmentDTO.StudentId,
                        SectionId = _EnrollmentDTO.SectionId,
                        EnrollDate = _EnrollmentDTO.EnrollDate,
                        SchoolId = _EnrollmentDTO.SchoolId
                    };
                    _context.Enrollments.Add(e);
                    await _context.SaveChangesAsync();
                    await _context.Database.CommitTransactionAsync();
                }
                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
                //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody]
                                                Enrollment _EnrollmentDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                //Enrollment has composite primary key
                //sectionId, StudentID, schoolid are requried for lookup,
                // user should only be able to modify enroll date and final grade
                var itm = await _context.Enrollments.Where(x => x.SectionId == _EnrollmentDTO.SectionId && x.StudentId == _EnrollmentDTO.StudentId && x.SchoolId == _EnrollmentDTO.SchoolId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    itm.EnrollDate = _EnrollmentDTO.EnrollDate;
                    itm.FinalGrade = _EnrollmentDTO.FinalGrade;
                    _context.Enrollments.Update(itm);
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

        public Task<IActionResult> Put([FromBody] EnrollmentDTO _T)
        {
            throw new NotImplementedException();
        }
    }
}
