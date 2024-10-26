using System.Collections.Generic;

namespace Objects
{
    public class Question
    {
        public string Query { get; }
        public List<string> Responses { get; }
        
        private readonly int _correctIndex;
        public Question(string query, string[] responses, int correctIndex)
        {
            Query = query;
            Responses = new List<string>(responses);
            _correctIndex = correctIndex;
        }
        
        public bool IsCorrectResponse(string response) => Responses[_correctIndex].Equals(response);
    }
}