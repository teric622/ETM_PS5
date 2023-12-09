using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using static Duende.IdentityServer.Models.IdentityResources;

namespace OCTOBER.Server.Controllers.UD
{



        [Route("api/[controller]")]
        [ApiController]
        public class InstructorController : BaseController, GenericRestController<InstructorDTO>
        {
            public InstructorController(OCTOBEROracleContext context,
                IHttpContextAccessor httpContextAccessor,
                IMemoryCache memoryCache)
            : base(context, httpContextAccessor)
            {
            }

            //Instructor has composite primary key
            //schoolid, InstructorId

            [HttpDelete]
            [Route("Delete/{SchoolId}/{InstructorId}")]
            public async Task<IActionResult> Delete(int SchoolId, int InstructorId)
            {
                try
                {
                    await _context.Database.BeginTransactionAsync();

                    var itm = await _context.Instructors.Where(x => x.SchoolId == SchoolId && x.InstructorId == InstructorId).FirstOrDefaultAsync();

                    if (itm != null)
                    {
                        _context.Instructors.Remove(itm);
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

                    var result = await _context.Instructors.Select(sp => new InstructorDTO
                    {
                        FirstName = sp.FirstName,
                        LastName = sp.LastName,
                        Phone = sp.Phone,
                        Salutation = sp.Salutation,
                        StreetAddress = sp.StreetAddress,
                        Zip = sp.Zip,
                        SchoolId = sp.SchoolId,
                        InstructorId = sp.InstructorId,
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
            //Instructor has composite primary key
            //schoolid, InstructorId
            [HttpGet]
            [Route("Get/{SchoolId}/{InstructorId}")]
            public async Task<IActionResult> Get(int SchoolId, int InstructorId)
            {
                try
                {
                    await _context.Database.BeginTransactionAsync();

                    InstructorDTO? result = await _context
                        .Instructors
                        .Where(x => x.SchoolId == SchoolId && x.InstructorId == InstructorId)
                         .Select(sp => new InstructorDTO
                         {
                             FirstName = sp.FirstName,
                             LastName = sp.LastName,
                             Phone = sp.Phone,
                             Salutation = sp.Salutation,
                             StreetAddress = sp.StreetAddress,
                             Zip = sp.Zip,
                             SchoolId = sp.SchoolId,
                             InstructorId = sp.InstructorId,
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
                                                InstructorDTO _InstructorDTO)
            {
                try
                {
                    await _context.Database.BeginTransactionAsync();

                    var itm = await _context.Instructors.Where(x => x.SchoolId == _InstructorDTO.SchoolId
                                                                        && x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();
                    if (itm == null)
                    {
                        //schoolid and InstructorId mandatory for new instance, due to the composite primary key
                        //wouldnt hurt to add the all fields not made by triggers as a required field
                        Instructor i = new Instructor
                        {
                            SchoolId = _InstructorDTO.SchoolId,
                            InstructorId = _InstructorDTO.InstructorId,
                            FirstName = _InstructorDTO.FirstName,
                            LastName = _InstructorDTO.LastName,
                            Phone = _InstructorDTO.Phone,
                            Salutation = _InstructorDTO.Salutation,
                            StreetAddress = _InstructorDTO.StreetAddress,
                            Zip = _InstructorDTO.Zip,

                        };
                        _context.Instructors.Add(i);
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
                                                InstructorDTO _InstructorDTO)
            {
                try
                {
                    await _context.Database.BeginTransactionAsync();

                    var itm = await _context.Instructors.Where(x => x.SchoolId == _InstructorDTO.SchoolId && x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();
                    //reallisticLLY, USER SHOULD ONLY BE ALLOWED TO EDIT most fields not mad eby triggers and not correlated to primary composite key
                    if (itm != null)
                    {
                    itm.FirstName = _InstructorDTO.FirstName;
                    itm.LastName = _InstructorDTO.LastName;
                    itm.Phone = _InstructorDTO.Phone;
                    itm.Salutation = _InstructorDTO.Salutation;
                    itm.StreetAddress = _InstructorDTO.StreetAddress;
                    itm.Zip = _InstructorDTO.Zip;
                     _context.Instructors.Update(itm);
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

