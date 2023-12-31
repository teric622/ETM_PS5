﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Controllers.Base;
using OCTOBER.Shared.DTO;
using System.Diagnostics;
//using Telerik.Blazor.Components;
//using Telerik.DataSource.Extensions;
//using Telerik.SvgIcons;

namespace OCTOBER.Server.Controllers.UD
{
    //    public class SchoolController : BaseController, GenericRestController<SchoolDTO>
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : BaseController, GenericRestController<SectionDTO>
    {
        public SectionController(OCTOBEROracleContext context,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        : base(context, httpContextAccessor)
        {
        }

        //section has composite primary key
        // sectionid, schoolid
        [HttpDelete]
        [Route("Delete/{SectionId}/{SchoolId}")]
        public async Task<IActionResult> Delete(int SectionId, int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == SectionId && x.SchoolId == SchoolId).FirstOrDefaultAsync();

                if (itm != null)
                {
                    _context.Sections.Remove(itm);
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

                var result = await _context.Sections.Select(sp => new SectionDTO
                {
                    SectionId = sp.SectionId,
                    CourseNo = sp.CourseNo,
                    StartDateTime = sp.StartDateTime,
                    Location = sp.Location,
                    InstructorId = sp.InstructorId,
                    Capacity = sp.Capacity,
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


        //section has composite primary key
        // sectionid, schoolid
        [HttpGet]
        [Route("Get/{SectionId}/{SchoolId}")]
        public async Task<IActionResult> Get(int SectionId, int SchoolId)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                SectionDTO? result = await _context
                    .Sections
                    .Where(x => x.SectionId == SectionId && x.SchoolId == SchoolId)
                     .Select(sp => new SectionDTO
                     {
                         SectionId = sp.SectionId,
                         CourseNo = sp.CourseNo,
                         StartDateTime = sp.StartDateTime,
                         Location = sp.Location,
                         InstructorId = sp.InstructorId,
                         Capacity = sp.Capacity,
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
                                                SectionDTO _SectionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SectionId && x.SchoolId == _SectionDTO.SchoolId).FirstOrDefaultAsync();
                if (itm == null)
                {
                    Section s = new Section
                    {
                        //sectionid and schoolid needed for new instance
                        // would also make sense for a course to come with
                        // the number, time itlle start, location, instructorid and capacity
                        SectionId = _SectionDTO.SectionId,
                        CourseNo = _SectionDTO.CourseNo,
                        StartDateTime = _SectionDTO.StartDateTime,
                        Location = _SectionDTO.Location,
                        InstructorId = _SectionDTO.InstructorId,
                        Capacity = _SectionDTO.Capacity,
                        SchoolId = _SectionDTO.SchoolId,


                       // SectionId = _SectionDTO.SectionId,
                       // CourseNo = _SectionDTO.CourseNo,
                       // SectionNo = _SectionDTO.SectionNo,
                      //  InstructorId = _SectionDTO.InstructorId,
                      //  SchoolId = _SectionDTO.SchoolId,
                    };
                    _context.Sections.Add(s);
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
                                                SectionDTO _SectionDTO)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var itm = await _context.Sections.Where(x => x.SectionId == _SectionDTO.SchoolId && x.SchoolId == _SectionDTO.SchoolId).FirstOrDefaultAsync();

                if (itm != null)
                {

                    //user shouldnt be able to modify any columns related to the primary key,
                    //all others are good
                    itm.CourseNo = _SectionDTO.CourseNo;
                    itm.StartDateTime = _SectionDTO.StartDateTime;
                    itm.Location = _SectionDTO.Location;
                    itm.InstructorId = _SectionDTO.InstructorId;
                    itm.Capacity = _SectionDTO.Capacity;
                    _context.Sections.Update(itm);
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
