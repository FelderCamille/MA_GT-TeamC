using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

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

        public static Question[] Shuffle(Question[] questions)
        {
            questions = questions.OrderBy(_ => _random.Next()).ToArray();
            return questions;
        }
        
        public Question[] QuestionPerDifficulty(Question[] questionsShuffled, LandmineDifficulty difficulty)
        {
            // Filter questions (+1 because difficulty starts at 0 and question.difficulty at 1)
            return questionsShuffled.Where(question => question.difficulty == (int) difficulty + 1).ToArray();
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
        [SerializeField] private int correctIndex;
        
        public int CorrectIndex => correctIndex - 1;
        
        public bool IsCorrectResponse(int responsesIndex) => responsesIndex == CorrectIndex;
    }
}