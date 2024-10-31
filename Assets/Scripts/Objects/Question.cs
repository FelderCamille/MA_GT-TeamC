using System;

namespace Objects
{
    [Serializable]
    public class Question
    {
        public string query;
        public string[] responses;
        public int correctIndex;
        
        public bool IsCorrectResponse(string response) => responses[correctIndex].Equals(response);
    }
}