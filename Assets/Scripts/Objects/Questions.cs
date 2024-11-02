using System;
using System.Linq;

namespace Objects
{
    [Serializable]
    public class Questions
    {
        public Question[] questions;
        
        private static Random _random = new ();

        public Question[] Shuffle()
        {
            questions = questions.OrderBy(_ => _random.Next()).ToArray();
            return questions;
        }
    }
    
    [Serializable]
    public class Question
    {
        public string query;
        public string[] responses;
        public int correctIndex;
        
        public bool IsCorrectResponse(string response) => responses[correctIndex].Equals(response);
    }
}