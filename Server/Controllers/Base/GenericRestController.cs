using Microsoft.AspNetCore.Mvc;
using OCTOBER.Shared.DTO;

namespace OCTOBER.Server.Controllers.Base
{
    public interface GenericRestController<T>
    {
        Task<IActionResult> Delete(int KeyVal);
        Task<IActionResult> Get();
        Task<IActionResult> Get(int KeyVal);
        Task<IActionResult> Post([FromBody] T _T);
        Task<IActionResult> Put([FromBody] T _T);
    }
}