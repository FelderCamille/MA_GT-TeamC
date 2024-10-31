using System;
using Objects;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Controllers
{
    public class QuestionController : MonoBehaviour
    {

        [Header("Content")]
        public QuestionButton buttonPrefab;
    
        [Header("Settings")]
        public int numberOfResponse;

        private int _currentQuestionIndex = -1; // To start at index 0
        private Question[] _questions;
        private bool _answering;

        private LandmineController _mine;

        public LandmineController Mine
        {
            set => _mine = value;
        }
        
        private void Awake()
        {
            var questionsObj = JsonUtils<Questions>.Read("Json/questions");
            _questions = questionsObj.Shuffle();
        }

        private void OnEnable()
        {
            // Get a question
            _currentQuestionIndex += 1;
            if (_currentQuestionIndex >= _questions.Length)
            {
                throw new Exception("All questions answered.");
            }
            var question = _questions[_currentQuestionIndex];
            // Update _answering
            _answering = true;
            // Update title
            GetComponentInChildren<Text>().text = question.query;
            // Place responses buttons
            for (var i = 0; i < question.responses.Length; i++)
            {
                if (i >= numberOfResponse) break; // Stop if the number of responses is reached
                var buttonObj = Instantiate(buttonPrefab, new Vector3(-300, -i * 70, 0), Quaternion.identity);
                buttonObj.transform.SetParent(GetComponentInChildren<Canvas>().transform, false); // To avoid the Transform component to be at (0,0,0)
                buttonObj.Init(question.responses[i]);
                buttonObj.button.onClick.AddListener(() => OnResponseClicked(buttonObj.buttonText));
            }
        }

        private void OnResponseClicked(Text buttonText)
        {
            // Manage response
            var isCorrect = _questions[_currentQuestionIndex].IsCorrectResponse(buttonText.text);
            _mine.OnLandmineCleared(isCorrect);
            // Not answering anymore
            _answering = false;
            // Hide question overlay
            gameObject.SetActive(false);
        }
        
        public bool IsAnswering()
        {
            return _answering;
        }
    }
}
