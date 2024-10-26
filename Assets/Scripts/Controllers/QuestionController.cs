using Objects;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class QuestionController : MonoBehaviour
    {

        [Header("Content")]
        public GameObject questionOverlay;
        public QuestionButton buttonPrefab;
    
        [Header("Settings")]
        public int numberOfResponse;

        private RobotController _robot;
        private Question _question;
        private bool _answering;

        private GameObject _mine;

        private void Start()
        {
            _robot = FindObjectOfType<RobotController>();
        }
        
        public void SetMine(MineController mine)
        {
            _mine = mine.gameObject;
        }

        private void OnEnable()
        {
            _answering = true;
            // Set question
            // TODO: fetch question
            _question = new Question("The title of the question", new []{"first", "second", "third", "fourth", "fifth"}, 1);
            // Update title
            questionOverlay.GetComponentInChildren<Text>().text = _question.Query;
            // Place responses buttons
            for (int i = 0; i < _question.Responses.Count; i++)
            {
                if (i >= numberOfResponse) break; // Stop if the number of responses is reached
                var buttonObj = Instantiate(buttonPrefab, new Vector3(0, -i * 70, 0), Quaternion.identity);
                buttonObj.transform.SetParent(questionOverlay.GetComponentInChildren<Canvas>().transform, false); // To avoid the Transform component to be at (0,0,0)
                buttonObj.Init(_question.Responses[i]);
                buttonObj.button.onClick.AddListener(() => OnResponseClicked(buttonObj.buttonText));
            }
        }

        private void OnResponseClicked(Text buttonText)
        {
            // Manage response
            _robot.ClearMine(_question.IsCorrectResponse(buttonText.text), _mine);
            // Not answering anymore
            _answering = false;
            // Hide question overlay
            questionOverlay.SetActive(false);
        }
        
        public bool IsAnswering()
        {
            return _answering;
        }
    }
}
