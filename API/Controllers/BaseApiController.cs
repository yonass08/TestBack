using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Responses;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediatr;

        protected IMediator Mediator => _mediatr ??= HttpContext.RequestServices.GetService<IMediator>();



        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
                return NotFound(Result<Unit>.Failure("Item not found."));

            if (result.IsSuccess && result.Value != null)
                return Ok(result);

            if (result.IsSuccess && result.Value == null)
                return NotFound(Result<Unit>.Failure("Item not found."));

            return BadRequest(result);

        }

    }


}