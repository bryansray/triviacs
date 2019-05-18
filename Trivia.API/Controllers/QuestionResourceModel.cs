using System.Collections.Generic;
using Trivia.API.ResourceModels;

namespace Trivia.API.Controllers
{
    public class QuestionResourceModel
    {
        public int Id { get; set; }
        
        public string Text { get; set; }
        
        public IEnumerable<AnswerResourceModel> Answers { get; set; }
    }
}