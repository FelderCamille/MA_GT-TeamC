using System.Collections;
using Utils;
using UnityEngine;

namespace Core
{
    public class TimeManager : MonoBehaviour
    {
        private SceneLoader _sceneLoader;
        private UI.Time _timePrefab;
        
        private float _time = Constants.GameSettings.Timer;
        private bool _isRunning = false;

        private void Start()
        {
            // Get objects
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            _timePrefab = FindFirstObjectByType<UI.Time>();
            // Set time
            _timePrefab.SetTime(_time);
        }

        private void OnEnable()
        {
            // Add events
            EventManager.TimerStart += EventManagerOnTimerStart;
            EventManager.TimerStop += EventManagerOnTimerStop;
            // Start timer
            EventManager.OnTimerStart();
        }

        private void OnDisable()
        {
            // Remove events
            EventManager.TimerStart -= EventManagerOnTimerStart;
            EventManager.TimerStop -= EventManagerOnTimerStop;
            // Stop timer
            EventManager.OnTimerStop();
        }
        
        private void SetTime(float value)
        {
            _time = value;
            _timePrefab.SetTime(_time);
        }

        private void EventManagerOnTimerStart() => _isRunning = true;

        private void EventManagerOnTimerStop() => _isRunning = false;
        
        private void Update()
        {
            // If the timer is stopped, do not run the code
            if (!_isRunning) return;
            // Manage when the timer is done
            if (_time <= 0.0f)
            {
                StartCoroutine(ManageEndTimer());
                return;
            }
            // Update timer
            SetTime(_time - Time.deltaTime);
        }

        private IEnumerator ManageEndTimer()
        {
            // Stop timer
            EventManager.OnTimerStop();
            // Wait to have a smoother transition
            yield return new WaitForSeconds(0.5f);
            // Show result scene
            _sceneLoader.ShowScene(Constants.Scenes.Result);
        }
    }

}