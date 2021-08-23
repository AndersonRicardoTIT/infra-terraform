using Microsoft.AspNetCore.Mvc;

namespace Usadosbr.Contas.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}