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
        
        private Dictionary<LandmineDifficulty, int> _currentQuestionIndex;
        private Dictionary<LandmineDifficulty, Question[]> _questionsPerDifficulty;
        
        public LandmineController Mine {private get; set;}
        public bool IsAnswering {get; private set;}
        public RobotController Robot {private get; set;}
        
        private LandmineDifficulty Difficulty => Mine.Difficulty;
        private int CurrentQuestionIndexForDifficulty => _currentQuestionIndex[Difficulty];
        
        private void Awake()
        {
            InitQuestions();
            _soundManager = SoundManager.instance;
        }

        private void InitQuestions()
        {
            var questionsObj = JsonUtils<Questions>.Read("Json/questions");
            var questions = Questions.Shuffle(questionsObj.questions);
            _questionsPerDifficulty = new Dictionary<LandmineDifficulty, Question[]>
            {
                { LandmineDifficulty.Easy, questionsObj.QuestionPerDifficulty(questions, LandmineDifficulty.Easy) },
                { LandmineDifficulty.Medium, questionsObj.QuestionPerDifficulty(questions, LandmineDifficulty.Medium) },
                { LandmineDifficulty.Hard, questionsObj.QuestionPerDifficulty(questions, LandmineDifficulty.Hard) }
            };
            _currentQuestionIndex = new Dictionary<LandmineDifficulty, int>
            {
                { LandmineDifficulty.Easy, -1 },
                { LandmineDifficulty.Medium, -1 },
                { LandmineDifficulty.Hard, -1 }
            };
        }

        private void ResetQuestionsForCurrentDifficulty()
        {
            _questionsPerDifficulty[Difficulty] = Questions.Shuffle(_questionsPerDifficulty[Difficulty]);
            _currentQuestionIndex[Difficulty] = -1;
        }

        private void OnEnable()
        {
            // Set difficulty text and color
            difficulty.text = Constants.Landmines.LandmineDifficultyName(Difficulty);
            difficulty.color = Constants.Landmines.LandmineDifficultyColor(Difficulty);
            // Check if their is more questions
            if (CurrentQuestionIndexForDifficulty + 1 >= _questionsPerDifficulty[Difficulty].Length)
            {
                Debug.Log("No more questions for this difficulty. Resetting questions.");
                ResetQuestionsForCurrentDifficulty();
                Debug.Log("Questions reset.");
            }
            // Get next question
            _currentQuestionIndex[Difficulty]++;
            var question = _questionsPerDifficulty[Difficulty][CurrentQuestionIndexForDifficulty];
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
                buttonObj.name = "Response n°" + i + (question.IsCorrectResponse(i) ? " x" : "");
                buttonObj.Init(question.responses[i], OnResponseClicked(buttonObj), i);
                _buttons.Add(buttonObj);
            }
        }

        private IEnumerator OnResponseClicked(QuestionButton questionButton)
        {
            // Play sound
            _soundManager.PlayCutSound();
            // Manage response
            var question = _questionsPerDifficulty[Difficulty][CurrentQuestionIndexForDifficulty];
            var isCorrect = question.IsCorrectResponse(questionButton.Index);
            // Show feedback
            if (!isCorrect)
            {
                var correctQuestionButton = _buttons[question.CorrectIndex];
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
        
    }
}
