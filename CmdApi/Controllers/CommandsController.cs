using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CmdApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetString()
        {
            return new string[] { "this", "that"};
        }
    }
}