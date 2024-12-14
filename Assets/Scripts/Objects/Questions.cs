using System;
using System.Linq;

namespace Objects
{
    [Serializable]
    public class Questions
    {
        /// <summary>
        /// The questions to ask.
        /// </summary>
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
        /// <summary>
        /// Difficulty of the question. From 1 = easy to 3 = hard.
        /// </summary>
        public int difficulty;
        
        /// <summary>
        /// The question to ask.
        /// </summary>
        public string query;
        
        /// <summary>
        /// The possible responses to the question.
        /// </summary>
        public string[] responses;
        
        /// <summary>
        ///  The index of the correct response in the responses array. Starting from 1.
        /// </summary>
        public int correctIndex;
        
        public bool IsCorrectResponse(string response) => responses[correctIndex-1].Equals(response);
    }
}