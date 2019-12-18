using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Controllers
{
    public class va
    {
        [Required]
        public int? MyProperty { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AuthenticateAttribute]
    [ValidateModel]

    public class ValuesController : ControllerBase
    {

        // GET api/values
        [HttpGet]
        [Route("getAll")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [AuthAccessToOrganization(Role = @short.OWNER, ParamType = ParamType.QUERY_PARAM)]
        // GET api/values/5
        [HttpGet]
        public IActionResult Get([FromBody]va vv)
        {
            var user = (User)HttpContext.Items.Single(i => i.Key as string == "current-user").Value;
            return Ok(vv);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
