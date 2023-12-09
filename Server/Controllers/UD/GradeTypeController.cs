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
    public class GradeTypeController : BaseController, GenericRestController<GradeTypeDTO>
    {
        public GradeTypeController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }

        //GradeType has composite primary key
        //schoolid, gradetypecode

        [HttpDelete]
        [Route("Delete/{SchoolId}/{GradeTypeCode}")]
        public async Task<IActionResult> Delete(int SchoolId, string GradeTypeCode)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeTypes.Where(x => x.SchoolId == SchoolId && x.GradeTypeCode == GradeTypeCode).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.GradeTypes.Remove(itm);
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

                var result = await _context.GradeTypes.Select(sp => new GradeTypeDTO
                {
                    SchoolId = sp.SchoolId,
                    GradeTypeCode = sp.GradeTypeCode,
                    Description = sp.Description,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
                })
                    //more than 1 thing ret, tolistasync needed
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
        //GradeType has composite primary key
        //schoolid, gradetypecode
        [HttpGet]
        [Route("Get/{SchoolId}/{GradeTypeCode}")]
        public async Task<IActionResult> Get(int SchoolId, string GradeTypeCode)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                GradeTypeDTO? result = await _context
                    .GradeTypes
                    .Where(x => x.SchoolId == SchoolId && x.GradeTypeCode == GradeTypeCode)
                     .Select(sp => new GradeTypeDTO
                     {
                         SchoolId = sp.SchoolId,
                         GradeTypeCode = sp.GradeTypeCode,
                         Description = sp.Description,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         ModifiedBy = sp.ModifiedBy,
                         ModifiedDate = sp.ModifiedDate
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
                                                GradeTypeDTO _GradeTypeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeTypes.Where(x => x.SchoolId == _GradeTypeDTO.SchoolId
                                                                    && x.GradeTypeCode == _GradeTypeDTO.GradeTypeCode).FirstOrDefaultAsync();
                if (itm == null)
                {
                    //schoolid and gradetypecode mandatory for new instance, due to the composite primary key
                    //wouldnt hurt to add the description as a required field
                    GradeType g = new GradeType
                    {
                        SchoolId = _GradeTypeDTO.SchoolId,
                        GradeTypeCode = _GradeTypeDTO.GradeTypeCode,
                        Description = _GradeTypeDTO.Description,
                    };
                    _context.GradeTypes.Add(g);
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
                                                GradeTypeDTO _GradeTypeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.GradeTypes.Where(x => x.SchoolId == _GradeTypeDTO.SchoolId && x.GradeTypeCode == _GradeTypeDTO.GradeTypeCode).FirstOrDefaultAsync();
                //reallisticLLY, USER SHOULD ONLY BE ALLOWED TO EDIT THE DESCRIPTION
                if (itm != null)
                {
                    itm.Description = _GradeTypeDTO.Description;
                    _context.GradeTypes.Update(itm);
                }

                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return Ok();
            }
            catch (Exception Dex)
            {
                await _context.Database.RollbackTransactionAsync();
               
                return StatusCode(StatusCodes.Status417ExpectationFailed, "An Error has occurred");
            }
        }
    }
}
