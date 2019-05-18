using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Trivia.API.Common;

namespace Trivia.API.Controllers
{
    [Route("v1/answers")]
    public class AnswersController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public AnswersController(IDbConnection connection)
        {
            _connection = connection;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            const string sql = @"SELECT * 
                FROM answers 
	                INNER JOIN questions ON answers.question_id = questions.id 
                WHERE answers.id = @id";

            var answer = (await _connection.QueryAsync<Answer, Question, Answer>(sql, (a, q) =>
                {
                    a.Question = q;
                    return a;
                },

                new {id})).FirstOrDefault();

            return Ok(Mapper.MapAnswerToResourceModel(answer));
        }
    }
}