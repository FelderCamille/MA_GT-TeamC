using Events;
using UnityEngine;

namespace Core
{
    public class TimeManager : MonoBehaviour
    {
        
        private UI.Time _timePrefab;
        
        private float _time = 600f; // 600s = 10m
        private bool _isRunning = false;

        private void Start()
        {
            // Set time on UI
            _timePrefab = FindObjectOfType<UI.Time>();
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
                ManageEndTimer();
                return;
            }
            // Update timer
            SetTime(_time - Time.deltaTime);
        }

        private void ManageEndTimer()
        {
            EventManager.OnTimerStop();
        }
    }

}