
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
    public class GradeController : BaseController, GenericRestController<GradeDTO>
    {
        public GradeController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }



        //Grade has composite primary key
        //schoolid, studentid, sectionid, gradetypecode, gradecodeoccurence
        [HttpDelete]
        [Route("Delete/{SchoolId}/{StudentId}/{SectionId}/{GradeTypeCode}/{GradeCodeOccurrence}")]
        public async Task<IActionResult> Delete(int SchoolId, int StudentId, int SectionId, string GradeTypeCode, int GradeCodeOccurrence)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                //Grade has composite primary key
                //schoolid, studentid, sectionid, gradetypecode, gradecodeoccurence 
                // these fields need to match something in db in order for it to be removed
                var itm = await _context.Grades.Where(x => x.SectionId == SectionId && x.StudentId == StudentId
                                                            && x.SchoolId == SchoolId && x.GradeTypeCode == GradeTypeCode
                                                            && x.GradeCodeOccurrence == GradeCodeOccurrence).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Grades.Remove(itm);
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

                var result = await _context.Grades.Select(sp => new GradeDTO
                {
                    SchoolId = sp.SchoolId,
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    GradeCodeOccurrence = sp.GradeCodeOccurrence,
                    NumericGrade = sp.NumericGrade,
                    Comments = sp.Comments,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,

                })
                    //tolistasync, since more than 1 hting being returned, fixed error
                .ToListAsync();
                await _context.Database.RollbackTransactionAsync();
                return Ok(result);
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
            
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }


        //Grade has composite primary key
        //schoolid, studentid, sectionid, gradetypecode, gradecodeoccurence

        [HttpGet]
        [Route("Get/{SchoolId}/{StudentId}/{SectionId}/{GradeTypeCode}/{GradeCodeOccurrence}")]
        public async Task<IActionResult> Get(int SchoolId, int StudentId, int SectionId, string GradeTypeCode, int GradeCodeOccurrence)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                //Grade has composite primary key
                //schoolid, studentid, sectionid, gradetypecode, gradecodeoccurence need to match for get request
                GradeDTO? result = await _context
                    .Grades
                    .Where(x => x.SectionId == SectionId && x.StudentId == StudentId
                                                            && x.SchoolId == SchoolId && x.GradeTypeCode == GradeTypeCode
                                                            && x.GradeCodeOccurrence == GradeCodeOccurrence)
                     .Select(sp => new GradeDTO
                     {
                         SchoolId = sp.SchoolId,
                         StudentId = sp.StudentId,
                         SectionId = sp.SectionId,
                         GradeTypeCode = sp.GradeTypeCode,
                         GradeCodeOccurrence = sp.GradeCodeOccurrence,
                         NumericGrade = sp.NumericGrade,
                         Comments = sp.Comments,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate,
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

        //Grade has composite primary key
        //schoolid, studentid, sectionid, gradetypecode, gradecodeoccurence
        public async Task<IActionResult> Post([FromBody]
                                                GradeDTO _GradeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Grades.Where(x => x.SectionId == _GradeDTO.SectionId && x.StudentId == _GradeDTO.StudentId
                                                            && x.SchoolId == _GradeDTO.SchoolId && x.GradeTypeCode == _GradeDTO.GradeTypeCode
                                                            && x.GradeCodeOccurrence == _GradeDTO.GradeCodeOccurrence).FirstOrDefaultAsync();
                if (itm == null)
                {
                    //schoolid, studentid, sectionid, gradetypecode, gradecodeoccurence are a must for new occurence
                    // would make sense for numericGrade to be needed, comments arnt requried
                    Grade g = new Grade
                    {
                        SchoolId = _GradeDTO.SchoolId,
                        StudentId = _GradeDTO.StudentId,
                        SectionId = _GradeDTO.SectionId,
                        GradeTypeCode = _GradeDTO.GradeTypeCode,
                        GradeCodeOccurrence = _GradeDTO.GradeCodeOccurrence,
                        NumericGrade = _GradeDTO.NumericGrade,
 
                    };
                    _context.Grades.Add(g);
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
                                                GradeDTO _GradeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Grades.Where(x => x.SectionId == _GradeDTO.SectionId && x.StudentId == _GradeDTO.StudentId && x.SchoolId == _GradeDTO.SchoolId).FirstOrDefaultAsync();
                //user should only be able to modify the numergrafe or comments,
                // shouldnt be able to modify any of the primary key objs
                if (itm != null)
                {
                    itm.NumericGrade = _GradeDTO.NumericGrade;
                    itm.Comments = _GradeDTO.Comments;
                    _context.Grades.Update(itm);
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
