using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trivia.API.Common;
using Trivia.API.Common.Web;

namespace Trivia.API.Controllers.Questions
{
    [Route("v1/questions/{questionId}/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public AnswersController(IDbConnection connection)
        {
            _connection = connection;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index([FromRoute] int questionId)
        {
            var dictionary = new Dictionary<int, Question>();
            
            Question MapResults(Question q, Answer a)
            {
                if (!dictionary.TryGetValue(q.Id, out var entry))
                {
                    entry = q;
                    entry.Answers = new List<Answer>();
                    dictionary.Add(entry.Id, entry);
                }

                entry.Answers.Add(a);
                return entry;
            }
            
            var question = (await _connection.QueryAsync<Question, Answer, Question>(
                "SELECT * FROM questions INNER JOIN answers ON questions.id = answers.question_id WHERE questions.id = @questionId",
                MapResults, new {questionId})).Distinct().SingleOrDefault();
            if (question == null) return NotFound();
            return Ok(new SuccessEnvelope(Mapper.MapQuestionToResourceModel(question)));
        }
    }
}