namespace Trivia.API.Common.Web
{
    public class SuccessEnvelope
    {
        public SuccessEnvelope(object data)
        {
            Data = data;
        }
        
        public object Data { get; }
    }
}