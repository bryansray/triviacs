using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trivia.API.Common;

namespace Trivia.API.Controllers
{
    [Route("v1/answers")]
    public class AnswersController : ControllerBase
    {
        private readonly TriviaDbContext _context;

        public AnswersController(TriviaDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var answer = await _context.Answers.Include(x => x.Question)
                .SingleOrDefaultAsync(x => x.Id == id);

            return Ok(Mapper.MapAnswerToResourceModel(answer));
        }
    }
}