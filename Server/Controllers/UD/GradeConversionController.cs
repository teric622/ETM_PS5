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
    public class GradeConversionController : BaseController, GenericRestController<GradeConversionDTO>

    {

        //constructor 

        public GradeConversionController(OCTOBEROracleContext context,
        IHttpContextAccessor httpContextAccessor,
        IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }


        [HttpDelete]
        [Route("Delete/{SchoolID}")]

        public async Task<IActionResult> Delete(int SchoolID)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeConversions.Where(x => x.SchoolId == SchoolID).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.GradeConversions.Remove(itm);
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

                var result = await _context.GradeConversions.Select(sp => new GradeConversionDTO
                {
                     CreatedBy = sp.CreatedBy,
                     CreatedDate = sp.CreatedDate,
                     LetterGrade  = sp.LetterGrade,
                     GradePoint = sp.GradePoint,
                     ModifiedDate = sp.ModifiedDate,
                     ModifiedBy = sp.ModifiedBy,
                     MinGrade = sp.MinGrade,
                     MaxGrade = sp.MaxGrade,
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



        [HttpGet]
        [Route("Get/{SchoolID}")]
        public async Task<IActionResult> Get(int SchoolID)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeConversionDTO? result = await _context.GradeConversions
                    .Where(x => x.SchoolId == SchoolID)
                    .Select(sp => new GradeConversionDTO
                    {
                        CreatedBy = sp.CreatedBy,
                        CreatedDate = sp.CreatedDate,
                        LetterGrade = sp.LetterGrade,
                        GradePoint = sp.GradePoint,
                        ModifiedDate = sp.ModifiedDate,
                        ModifiedBy = sp.ModifiedBy,
                        MinGrade = sp.MinGrade,
                        MaxGrade = sp.MaxGrade,
                        SchoolId = sp.SchoolId,


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
        public async Task<IActionResult> Post([FromBody] GradeConversionDTO _GradeConversionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversionDTO.SchoolId).FirstOrDefaultAsync();
                if (itm == null)
                {
                    GradeConversion gc = new GradeConversion
                    {
                         LetterGrade = _GradeConversionDTO.LetterGrade,
                         SchoolId = _GradeConversionDTO.SchoolId,
                    };
                    _context.GradeConversions.Add(gc);
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

        public async Task<IActionResult> Put([FromBody] GradeConversionDTO _GradeConversionDTO)
        {

            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversionDTO.SchoolId).FirstOrDefaultAsync();

                itm.SchoolId = _GradeConversionDTO.SchoolId;
                itm.LetterGrade = _GradeConversionDTO.LetterGrade;
                _context.GradeConversions.Update(itm);
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
