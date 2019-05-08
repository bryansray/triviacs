using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trivia.API.Common;
using Trivia.API.Common.Web;

namespace Trivia.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly TriviaDbContext _context;

        public QuestionsController(TriviaDbContext context)
        {
            _context = context;
        }
        
        // GET v1/questions
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            int.TryParse(Request.Query["page"], out var page);
            int.TryParse(Request.Query["pageSize"], out var pageSize);
            if (page <= 0) page = 1;
            if (pageSize <= 10 || pageSize > 100) pageSize = 100;

            var total = await _context.Questions.CountAsync();
            var questions = await _context.Questions.Skip((page - 1) * pageSize).Take(pageSize).Include(x => x.Answers).ToListAsync();
            var totalPages = (int)Math.Ceiling(total / (decimal)pageSize);
            
            return Ok(
                new PagedSuccessEnvelope(
                        questions.Select(Mapper.MapQuestionToResourceModel), 
                        page, 
                        pageSize, 
                        total)
                {Meta = { TotalPages = totalPages } });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var question = await _context.Questions.Include(x => x.Answers).SingleOrDefaultAsync(x => x.Id == id);
            return Ok(new SuccessEnvelope(Mapper.MapQuestionToResourceModel(question)));
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
    
    [Route("v1/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly TriviaDbContext _context;

        public StatisticsController(TriviaDbContext context)
        {
            _context = context;
        }
        
        // GET v1/statistics
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var totalQuestions = await _context.Questions.CountAsync();
            var totalAnswers = await _context.Answers.CountAsync();
            var totalPeopleSurveyed = await _context.Answers.SumAsync(x => x.Count);
            
            return Ok(
                new SuccessEnvelope( new { totalQuestions, totalAnswers, totalPeopleSurveyed }));
        }
    }
}