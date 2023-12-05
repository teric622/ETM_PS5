using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OCTOBER.EF.Data;
using OCTOBER.EF.Models;
using OCTOBER.Server.Models;
//using Telerik.DataSource.Extensions;



namespace OCTOBER.Server.Controllers.Base
{
    public class BaseController : Controller
    {
        protected readonly OCTOBEROracleContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;


        public BaseController(OCTOBEROracleContext context,
                                 IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }
    }
}