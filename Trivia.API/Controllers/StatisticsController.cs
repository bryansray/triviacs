using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Trivia.API.Common.Web;

namespace Trivia.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IDbConnection _connection;

        public StatisticsController(IDbConnection connection)
        {
            _connection = connection;
        }
        
        // GET v1/statistics
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var queries =
                "SELECT COUNT(*) FROM questions; SELECT COUNT(*) FROM answers; SELECT SUM(count) FROM answers;";
            var results = await _connection.QueryMultipleAsync(queries);

            var totalQuestions =  await results.ReadFirstAsync<long>();
            var totalAnswers = await results.ReadFirstAsync<long>();
            var surveyed = (int)(await results.ReadFirstAsync<decimal>());

            return Ok(
                new SuccessEnvelope( new { totalQuestions, totalAnswers, surveyed }));
        }
    }
}