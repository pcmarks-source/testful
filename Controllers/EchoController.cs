using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TESTful.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EchoController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> PostAsync() {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) {
                string message = await reader.ReadToEndAsync();
                return Ok("POST method was sent: " + "\n" + "        " + message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id) {
            return Ok("This simple API is intended to be used as a test for HTTP method requests.");
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get() {
            return Ok(new string[] { "This simple API is intended to be used as a test for HTTP method requests.", "It should echo back the body content sent through POST/PUT/PATCH/DELETE requests." });
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync() {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) {
                string message = await reader.ReadToEndAsync();
                return Ok("PUT method was sent: " + "\n" + "        " + message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult> PatchAsync() {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) {
                string message = await reader.ReadToEndAsync();
                return Ok("PATCH method was sent: " + "\n" + "        " + message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync() {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8)) {
                string message = await reader.ReadToEndAsync();
                return Ok("DELETE method was sent: " + "\n" + "        " + message);
            }
        }
    }
}
