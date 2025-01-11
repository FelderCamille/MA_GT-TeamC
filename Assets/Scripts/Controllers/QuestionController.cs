using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private QuestionButton buttonPrefab;
        [SerializeField] private Text difficulty;
        [SerializeField] private Text title;
        private readonly List<QuestionButton> _buttons = new ();

        private SoundManager _soundManager;
        
        private readonly Dictionary<LandmineDifficulty, int> _currentQuestionIndex = new ()
        {
            { LandmineDifficulty.Easy, -1 },
            { LandmineDifficulty.Medium, -1 },
            { LandmineDifficulty.Hard, -1 }
        };
        private Dictionary<LandmineDifficulty, Question[]> _questionsPerDifficulty;
        
        public LandmineController Mine {private get; set;}
        public bool IsAnswering {get; private set;}
        public RobotController Robot {private get; set;}
        
        private void Awake()
        {
            var questionsObj = JsonUtils<Questions>.Read("Json/questions");
            var questions = questionsObj.Shuffle();
            _questionsPerDifficulty = new Dictionary<LandmineDifficulty, Question[]>()
            {
                { LandmineDifficulty.Easy, questionsObj.QuestionPerDifficulty(questions, LandmineDifficulty.Easy) },
                { LandmineDifficulty.Medium, questionsObj.QuestionPerDifficulty(questions, LandmineDifficulty.Medium) },
                { LandmineDifficulty.Hard, questionsObj.QuestionPerDifficulty(questions, LandmineDifficulty.Hard) }
            };
            _soundManager = FindFirstObjectByType<SoundManager>();
        }

        private void OnEnable()
        {
            // Set difficulty text and color
            difficulty.text = Constants.Landmines.LandmineDifficultyName(Mine.Difficulty);
            difficulty.color = Constants.Landmines.LandmineDifficultyColor(Mine.Difficulty);
            // Get next question index
            _currentQuestionIndex[Mine.Difficulty]++;
            // Get a question
            if (CurrentQuestionIndexForDifficulty >= _questionsPerDifficulty[Mine.Difficulty].Length)
            {
                throw new Exception("All questions answered.");
            }
            var question = _questionsPerDifficulty[Mine.Difficulty][CurrentQuestionIndexForDifficulty];
            // Update _answering
            IsAnswering = true;
            // Remove old buttons
            _buttons.Clear();
            while (GetComponentInChildren<QuestionButton>() != null)
            {
                DestroyImmediate(GetComponentInChildren<QuestionButton>().gameObject);
            }
            // Update title
            title.text = question.query;
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
            var question = _questionsPerDifficulty[Mine.Difficulty][CurrentQuestionIndexForDifficulty];
            var isCorrect = question.IsCorrectResponse(questionButton.GetText());
            // Show feedback
            if (!isCorrect)
            {
                var correctIndex = question.correctIndex - 1;
                var correctQuestionButton = _buttons[correctIndex];
                StartCoroutine(correctQuestionButton.ShowResult(true));
            }
            yield return StartCoroutine(questionButton.ShowResult(isCorrect));
            // Manage mine
            Mine.OnLandmineCleared(Robot, isCorrect ? LandmineCleared.AnswerSuccess : LandmineCleared.AnswerFailure);
            // Not answering anymore
            IsAnswering = false;
            // Hide question overlay
            gameObject.SetActive(false);
        }
        
        private int CurrentQuestionIndexForDifficulty => _currentQuestionIndex[Mine.Difficulty];
    }
}
