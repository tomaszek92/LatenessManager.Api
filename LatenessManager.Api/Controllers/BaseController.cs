using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LatenessManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController
    {
        protected ISender Sender;

        protected BaseController(ISender sender)
        {
            Sender = sender;
        }
    }
}