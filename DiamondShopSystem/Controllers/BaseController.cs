using Service.Commons;
using Microsoft.AspNetCore.Mvc;
using DiamondShopSystem.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiamondShopSystem.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleErrorResponse(List<Error> errors)
        {
            if (errors.Any(e => e.StatusCode == (int)Service.Commons.StatusCode.UnAuthorize))
            {
                var error = errors.FirstOrDefault(e => e.StatusCode == (int)Service.Commons.StatusCode.UnAuthorize);
                return Unauthorized(new ErrorResponse(401, "UnAuthorize", true, error!.Message, DateTime.Now));
            }
            if (errors.Any(e => e.StatusCode == (int)Service.Commons.StatusCode.NotFound))
            {
                var error = errors.FirstOrDefault(e => e.StatusCode == (int)Service.Commons.StatusCode.NotFound);
                return NotFound(new ErrorResponse(404, "Not Found", true, error!.Message, DateTime.Now));
            }
            if (errors.Any(e => e.StatusCode == (int)Service.Commons.StatusCode.ServerError))
            {
                var error = errors.FirstOrDefault(e => e.StatusCode == (int)Service.Commons.StatusCode.ServerError);
                return StatusCode(500, new ErrorResponse(500, error?.Message == null ? "Server Error" : "Server Error", true, error!.Message, DateTime.Now));
            }
            return BadRequest(new ErrorResponse(400, errors.FirstOrDefault()?.Message == null ? "Bad Request" : "Bad Request", true, errors, DateTime.Now));
        }
    }
}
