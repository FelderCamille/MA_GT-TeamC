using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
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
        
        private SoundManager _soundManager;
        private int _currentQuestionIndex = -1; // To start at index 0
        private Question[] _questions;
        private readonly List<QuestionButton> _buttons = new ();
        
        public LandmineController Mine {private get; set;}
        public bool IsAnswering {get; private set;}
        public RobotController Robot {private get; set;}
        
        private void Awake()
        {
            var questionsObj = JsonUtils<Questions>.Read("Json/questions");
            _questions = questionsObj.Shuffle();
            _soundManager = FindFirstObjectByType<SoundManager>();
        }

        private void OnEnable()
        {
            // Get a question
            _currentQuestionIndex++;
            if (_currentQuestionIndex >= _questions.Length)
            {
                throw new Exception("All questions answered.");
            }
            var question = _questions[_currentQuestionIndex];
            // Update _answering
            IsAnswering = true;
            // Remove old buttons
            _buttons.Clear();
            while (GetComponentInChildren<QuestionButton>() != null)
            {
                DestroyImmediate(GetComponentInChildren<QuestionButton>().gameObject);
            }
            // Update title
            GetComponentInChildren<Text>().text = question.query;
            // Place responses buttons
            for (var i = 0; i < question.responses.Length; i++)
            {
                var buttonObj = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
                buttonObj.transform.SetParent(GetComponentInChildren<VerticalLayoutGroup>().transform, false); // To avoid the Transform component to be at (0,0,0)
                buttonObj.name = "Response n°" + i + (question.IsCorrectResponse(question.responses[i]) ? " x" : "");
                buttonObj.Init(question.responses[i], OnResponseClicked(buttonObj));
                _buttons.Add(buttonObj);
            }
        }

        private IEnumerator OnResponseClicked(QuestionButton questionButton)
        {
            // Play sound
            _soundManager.PlayCutSound();
            // Manage response
            var question = _questions[_currentQuestionIndex];
            var isCorrect = question.IsCorrectResponse(questionButton.buttonText.text);
            // Show feedback
            if (!isCorrect)
            {
                var correctIndex = question.correctIndex - 1;
                var correctQuestionButton = _buttons[correctIndex];
                StartCoroutine(correctQuestionButton.ShowResult(true));
            }
            yield return StartCoroutine(questionButton.ShowResult(isCorrect));
            // Manage mine
            print("2: " + Robot.OwnerClientId);
            Mine.OnLandmineCleared(isCorrect ? LandmineCleared.AnswerSuccess : LandmineCleared.AnswerFailure);
            // Not answering anymore
            IsAnswering = false;
            // Hide question overlay
            gameObject.SetActive(false);
        }
    }
}
