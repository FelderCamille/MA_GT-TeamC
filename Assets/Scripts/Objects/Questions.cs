using System;
using System.Linq;

namespace Objects
{
    [Serializable]
    public class Questions
    {
        public Question[] questions;
        
        private static Random _random = new Random();

        public Question[] Shuffle()
        {
            questions = questions.OrderBy(_ => _random.Next()).ToArray();
            return questions;
        }
    }
}