using System.Linq;
using Trivia.API.Controllers;
using Trivia.API.ResourceModels;

namespace Trivia.API.Common
{
    public static class Mapper
    {
        public static AnswerResourceModel MapAnswerToResourceModel(Answer answer)
        {
            return new AnswerResourceModel
            {
                Id = answer.Id,
                Text = answer.Text,
                Count = answer.Count
            };
        }
        
        public static QuestionResourceModel MapQuestionToResourceModel(Question question)
        {
            return new QuestionResourceModel {Id = question.Id, Text = question.Text, Answers = question?.Answers.Count() ?? 0};
        }
    }
}