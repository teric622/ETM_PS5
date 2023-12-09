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
    public class ZipcodeController : BaseController, GenericRestController<ZipcodeDTO>
    {
        public ZipcodeController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }

        //zipcode only has one primary key, no composite key 
        // field zip

        [HttpDelete]
        [Route("Delete/{Zip}")]
        public async Task<IActionResult> Delete(string Zip)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Zipcodes.Where(x => x.Zip == Zip).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Zipcodes.Remove(itm);
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

                var result = await _context.Zipcodes.Select(sp => new ZipcodeDTO
                {
                    Zip = sp.Zip,
                    City = sp.City,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    State = sp.State,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate
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
        //zip is the primary key
        [HttpGet]
        [Route("Get/{Zip}")]
        public async Task<IActionResult> Get(string Zip)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                ZipcodeDTO? result = await _context
                    .Zipcodes
                    .Where(x => x.Zip == Zip)
                     .Select(sp => new ZipcodeDTO
                     {
                         Zip = sp.Zip,
                         City = sp.City,
                         CreatedBy = sp.CreatedBy,
                         CreatedDate = sp.CreatedDate,
                         State = sp.State,
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
                                                ZipcodeDTO _ZipcodeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Zipcodes.Where(x => x.Zip == _ZipcodeDTO.Zip).FirstOrDefaultAsync();
                if (itm == null)
                {
                    //zip requried for new instance 
                    Zipcode z = new Zipcode
                    {
                        Zip = _ZipcodeDTO.Zip,
                        City = _ZipcodeDTO.City,
                        State = _ZipcodeDTO.State,
                    };
                    _context.Zipcodes.Add(z);
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
                                                ZipcodeDTO _ZipcodeDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Zipcodes.Where(x => x.Zip == _ZipcodeDTO.Zip).FirstOrDefaultAsync();

                if (itm != null)
                    //user shouldne be able to edit primary key field (zip)
                    //nor the ones made by triggers
                    // city and state only possible ones they can edit
                {
                    itm.City = _ZipcodeDTO.City;
                    itm.State = _ZipcodeDTO.State;
                    _context.Zipcodes.Update(itm);
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
