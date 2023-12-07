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
        //STUDENT_ID, SECTION_ID, 
        // public async Task<IActionResult> Delete(int SectionID, int StudentID, int SchoolID)
        //{
        //  try
        //{
        //  await _context.Database.BeginTransactionAsync();

        //var itm = await _context.Enrollments.Where(x => x.SectionId == SectionID && x.StudentId == StudentID && x.SchoolId == SchoolID).FirstOrDefaultAsync();

        //                if (itm != null)
        //              {
        //                _context.Enrollments.Remove(itm);
        //          }
        //        await _context.SaveChangesAsync();
        //      await _context.Database.CommitTransactionAsync();

        //    return Ok();
        //}
        //catch (Exception Dex)
        // {
        //   await _context.Database.RollbackTransactionAsync();
        //List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
        // return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
        //}






        // }
        //kept eroring whenever i tried to pass in multiple arguments to Get (int x, int, y, int z)
        // kept saying interface didnt support more than 1 arg
        // otherwise I would have doen the delete based on the 3 fields that make the composite primary key

        [HttpDelete]
        [Route("Delete/{StudentID}")]
        public async Task<IActionResult> Delete(int StudentID)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x =>  x.StudentId == StudentID ).FirstOrDefaultAsync();

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

        [HttpGet]
        [Route("Get")]


        public async Task<IActionResult> Get()
        {

            try
            {
                await _context.Database.BeginTransactionAsync();

                var result = await _context.Enrollments.Select(sp => new EnrollmentDTO
                {
                     FinalGrade = sp.FinalGrade,
                     CreatedBy = sp.CreatedBy,
                     CreatedDate = sp.CreatedDate,
                     SchoolId = sp.SchoolId,
                     EnrollDate = sp.EnrollDate,
                     ModifiedBy = sp.ModifiedBy,
                     ModifiedDate = sp.ModifiedDate,
                     SectionId = sp.SectionId,
                     StudentId = sp.StudentId,

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

        [HttpGet]
        [Route("Get/{StudentID}")]

        //kept eroring whenever i tried to pass in multiple arguments to Get (int x, int, y, int z)
        // otherwise I would have doen the get based on the 3 fields that make the composite primary key
        public async Task<IActionResult> Get(int StudentID)
        {


            try
            {
                await _context.Database.BeginTransactionAsync();

                EnrollmentDTO? result = await _context.Enrollments
                    .Where(x => x.StudentId == StudentID)
                    .Select(sp => new EnrollmentDTO
                    {
                        FinalGrade = sp.FinalGrade,
                        CreatedBy = sp.CreatedBy,
                        CreatedDate = sp.CreatedDate,
                        SchoolId = sp.SchoolId,
                        EnrollDate = sp.EnrollDate,
                        ModifiedBy = sp.ModifiedBy,
                        ModifiedDate = sp.ModifiedDate,
                        SectionId = sp.SectionId,
                        StudentId = sp.StudentId,
                       
                       
                    })
                .SingleAsync();
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

        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] EnrollmentDTO _EnrollmentDTO)
        {


            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId).FirstOrDefaultAsync();

                if (itm == null)
                {
                    Enrollment e = new Enrollment
                    {
                        SectionId = _EnrollmentDTO.SectionId,
                        SchoolId = _EnrollmentDTO.SchoolId,
                        StudentId = _EnrollmentDTO.StudentId


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

        public async Task<IActionResult> Put([FromBody] EnrollmentDTO _EnrollmentDTO)
        {

            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId).FirstOrDefaultAsync();

                itm.FinalGrade = _EnrollmentDTO.FinalGrade;

                _context.Enrollments.Update(itm);
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
