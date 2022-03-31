using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoWeb.Data.Services;
using TodoWeb.Dtos;

namespace TodoWeb.Controllers
{
    public class StatusesController : ControllerWithErrors
    {
        private readonly IStatusService _statusService;
        public StatusesController(IStatusService statusService)
        {
            _statusService = statusService;
        }
        public async Task<IActionResult> CreateAjax(CreateStatusArgs args)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetModelErrorMessages(ModelState));
            }
            var commandResult = await _statusService.CreateAsync(args);
            if (!commandResult.IsValid)
            {
                return BadRequest(commandResult.Errors);
            }
            return Json(commandResult);
        }
    }
}
