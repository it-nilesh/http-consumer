using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HttpConsumer.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet("v")]
        public User Get()
        {
            return  new User { Name1 = "V_value1" };
        }

        [HttpGet("v/down/{id}")]
        public IActionResult Down(string id)
        {
            var file = System.IO.File.ReadAllBytes(@"D:\Development\MyDev\http-consumer\Http.Consumer\HttpConsumer.Sample\appsettings.Development.json");
            return File(file, "application/json");
        }

        [HttpGet("d")]
        public User GetD()
        {
            //Dictionary<string, object> va = new Dictionary<string, object>();
            //va.Add("Name", new string[] { "Nil1", "Nil" });
            //va.Add("Name1", "Nilesh");
            //va.Add("Name2", 1);
            //va.Add("Name3", 10.2);
            //va.Add("Name4", DateTime.Now);

            return new User { Name1 = "D_value1" };
        }

        // GET api/values/5
        [HttpGet("v/{id}")]
        public ActionResult<string> Get(int id, string name, string lastName)
        {
            HttpContext.Response.ContentType = "application/json";
            return "value Nilesh";
        }

        [HttpPost("v")]
        public IActionResult Post1(FileStream fileStream)
        {
            var s = System.IO.File.Create("hh1.txt");
            fileStream.CopyTo(s);
            s.Flush();
            s.Dispose();
            //,  IFormFile file
            //var filrname = HttpContext.Request.Form.Files[0].FileName;
            //var name = HttpContext.Request.Form.Files[0].Name;
            return Ok(new User { Name1 = "value1" });
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            // throw new Exception("Manual Ex");
            //,  IFormFile file
            //var filrname = HttpContext.Request.Form.Files[0].FileName;
            //var name = HttpContext.Request.Form.Files[0].Name;
            return BadRequest(new User { Name1 = "value1" });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public Task Put(int id, [FromBody] User value, string name, string lastname)
        {
            return Task.CompletedTask;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class User
    {
        public string[] Name { get; set; }

        public string Name1 { get; set; }
        public int Name2 { get; set; }

        public decimal Name3 { get; set; }
        public DateTime Name4 { get; set; }
    }
}
