using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Trivia.API.Common;
using Trivia.API.Common.Web;

namespace Trivia.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IDbConnection _connection;
        private readonly IEmbeddedResourceProvider _resourceProvider;

        public QuestionsController(IDbConnection connection, IEmbeddedResourceProvider resourceProvider)
        {
            _connection = connection;
            _resourceProvider = resourceProvider;
        }

        [HttpGet("random")]
        public async Task<ActionResult> Random()
        {
            var total = await _connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM questions");
            var random = new Random();
            var id = random.Next(1, total);
            
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
                "SELECT * FROM questions INNER JOIN answers ON questions.id = answers.question_id WHERE questions.id = @id",
                MapResults, new {id})).Distinct().SingleOrDefault();
            
            return Ok(new SuccessEnvelope(Mapper.MapQuestionToResourceModel(question)));
        }

        // GET /v1/questions
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            int.TryParse(Request.Query["page"], out var page);
            int.TryParse(Request.Query["pageSize"], out var pageSize);

            if (page <= 0) page = 1;
            if (pageSize <= 10 || pageSize > 100) pageSize = 100;

            var total = await _connection.ExecuteScalarAsync<int>(_resourceProvider.Get("GetQuestionsCount.sql"));
            var questions = (await _connection.QueryAsync<Question>(_resourceProvider.Get("GetQuestions.sql"), new { Page = (page - 1) * pageSize, PageSize = pageSize})).Distinct();
            var totalPages = (int)Math.Ceiling(total / (decimal)pageSize);
            var resourceModels = questions.Select(Mapper.MapQuestionToResourceModel);
            
            return Ok(new PagedSuccessEnvelope(resourceModels, page, pageSize, total) { Meta = { TotalPages = totalPages } });
        }

        // GET /v1/questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
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

            var sql = _resourceProvider.Get("GetQuestionById.sql");
            var question = (await _connection.QueryAsync<Question, Answer, Question>(sql, MapResults, new {id})).Distinct().SingleOrDefault();
            return Ok(new SuccessEnvelope(Mapper.MapQuestionToResourceModel(question)));
        }
    }
}