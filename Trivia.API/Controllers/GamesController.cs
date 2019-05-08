using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Trivia.API.Controllers
{
    public class QuestionResourceModel
    {
        public int Id { get; set; }
        
        public string Text { get; set; }
        
        public int Answers { get; set; }
    }


    [Route("v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
