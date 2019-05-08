using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trivia.API.Common;

namespace Trivia.API.Controllers.Questions
{
    [Route("v1/questions/{questionId}/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly TriviaDbContext _context;

        public AnswersController(TriviaDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index([FromRoute] int questionId)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x => x.Id == questionId);
            var answers = await _context.Answers
                .Include(x => x.Question)
                .Where(x => x.Question.Id == questionId)
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            var result = new
            {
                Question = Mapper.MapQuestionToResourceModel(question),
                Answers = answers.Select(Mapper.MapAnswerToResourceModel)
            };
            return Ok(result);
        }
    }
}