using System.Collections;
using Unity.Netcode;
using Utils;
using UnityEngine;

namespace Core
{
    public class TimeManager : NetworkBehaviour
    {
        private SceneLoader _sceneLoader;
        private UI.Time _timePrefab;
        
        private readonly NetworkVariable<float> _time = new (Constants.GameSettings.Timer);
        private bool _isRunning; // false by default

        private void Start()
        {
            // Get objects
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            _timePrefab = FindFirstObjectByType<UI.Time>();
            // Set time
            _timePrefab.SetTime(_time.Value);
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
            if (NetworkManager.Singleton.IsHost) _time.Value = value;
            _timePrefab.SetTime(_time.Value);
        }

        private void EventManagerOnTimerStart() => _isRunning = true;

        private void EventManagerOnTimerStop() => _isRunning = false;
        
        private void Update()
        {
            // If the timer is stopped, do not run the code
            if (!_isRunning) return;
            // Manage when the timer is done
            if (_time.Value <= 0.0f)
            {
                StopRpc();
                return;
            }
            // Update timer
            SetTime(_time.Value - Time.deltaTime);
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

        [Rpc(SendTo.Server)]
        public void StopRpc()
        {
            StartCoroutine(ManageEndTimer());
        }
    }

}