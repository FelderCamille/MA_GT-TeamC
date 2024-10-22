using System.Collections.Generic;
using Classes;
using DefaultNamespace;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{

    [Header("Content")]
    public GameObject QuestionOverlay;
    public QuestionButton ButtonPrefab;
    
    [Header("Settings")]
    public int NumberOfResponse;

    private Question _question;

    private void OnEnable()
    {
        // Set question
        // TODO: fetch question
        _question = new Question("The title of the question", new []{"first", "second", "third", "fourth", "fifth"}, 1);
        // Update title
        QuestionOverlay.GetComponentInChildren<Text>().text = _question.Query;
        // Place responses buttons
        for (int i = 0; i < _question.Responses.Count; i++)
        {
            if (i >= NumberOfResponse) break; // Stop if the number of responses is reached
            var buttonObj = Instantiate(ButtonPrefab, new Vector3(0, -i * 70, 0), Quaternion.identity);
            buttonObj.transform.SetParent(QuestionOverlay.GetComponentInChildren<Canvas>().transform, false); // To avoid the Transform component to be at (0,0,0)
            buttonObj.Init(_question.Responses[i]);
            buttonObj.Button.onClick.AddListener(() => OnResponseClicked(buttonObj.ButtonText));
        }
    }

    private void OnResponseClicked(Text buttonText)
    {
        // Manage response
        if (_question.IsCorrectResponse(buttonText.text))
        {
            print("Correct response :)"); // TODO
        }
        else
        {
            print("Wrong response :("); // TODO
        }
        // Hide question overlay
        QuestionOverlay.SetActive(false);
    }
    
}
