using Trivia.API.Controllers;

namespace Trivia.API.ResourceModels
{
    public class AnswerResourceModel
    {
        public int Id { get; set; }
        
        public string Text { get; set; }
        
        public int Count { get; set; }
    }
}