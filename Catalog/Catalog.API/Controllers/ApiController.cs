using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiVersion("1")]   //Package: Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
    }
}
