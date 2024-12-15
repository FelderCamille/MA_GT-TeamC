using System;
using Core;
using System.Collections;
using System.Linq;
using Objects;
using UI;
using UnityEngine;
using Unity.Netcode;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class LandmineController : NetworkBehaviour, ILandmine
    {
        
        [Header("Settings")]
        [SerializeField] private float collidingDistance = Constants.GameSettings.NumberOfTileClearLandmine; // One tile of distance, no diagonal
        private SoundManager _soundManager;
        private QuestionController _questionOverlay;
        [SerializeField] private LandmineTile landmine;
        [SerializeField] private ParticleSystem explosionEffect;

        private RobotController[] _robots = new RobotController[2];
        private RobotController _robot;
        
        private void Start()
        { 
            _questionOverlay = FindFirstObjectByType<QuestionController>(FindObjectsInactive.Include);
            _soundManager = FindFirstObjectByType<SoundManager>();
            _robots = FindObjectsByType<RobotController>(FindObjectsSortMode.None);
            _robot = _robots.First(r => r.IsOwner);
        }

        private void Update()
        {
            DetectRobotsApproach();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnRobotCollided();
            }
        }
        
        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Effects"))
            {
                landmine.Show();
            }
        }

        public void DetectRobotsApproach()
        {
            // Check if the user wants to clear the mine, if not return
            if (!Input.GetKeyDown(Constants.Actions.ClearMine) || _questionOverlay.IsAnswering) return;
            // Check if the distance between the robots and the landmine permits to answer the question, if not return
            foreach (var robot in _robots)
            {
                if (!robot.IsOwner) return;
                if (!(Vector3.Distance(transform.position, robot.gameObject.transform.position) < collidingDistance)) return;
                // Show question overlay
                _soundManager.playOpenMineSound();
                ShowQuestionOverlay();  
            }
        }

        
        public void OnLandmineCleared(LandmineCleared state)
        {
            // Manage robot
            switch (state)
            {
                case LandmineCleared.AnswerSuccess:
                    _soundManager.PlayBeepSound();
                    _robot.IncreaseClearedMineCounter();
                    break;
                case LandmineCleared.AnswerFailure:
                    var hTRFailure = Random.Range(Constants.Values.HealthRemovedWhenFailureMin, Constants.Values.HealthRemovedWhenFailureMax);
                    _robot.ReduceHealth(hTRFailure);
                    break;
                case LandmineCleared.Explosion:
                    var hTRExplosion = Random.Range(Constants.Values.HealthRemovedWhenExplosionMin, Constants.Values.HealthRemovedWhenExplosionMax);
                    _robot.ReduceHealth(hTRExplosion);
                    break;
                default:
                    throw new Exception("Unknown landmine cleared state");
            }
            // Remove landmine
            if (state == LandmineCleared.AnswerSuccess)
            {
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(ExplodeLandmine());
            }
        }

        private IEnumerator ExplodeLandmine()
        {
            // Play sound
            _soundManager.PlayExplosionSound();
            // Play explosion effect
            landmine.Hide();
            explosionEffect.Play();
            // Wait for the explosion to play
            yield return new WaitForSeconds(explosionEffect.main.duration - 1.5f);
            // Remove the landmine
            gameObject.SetActive(false);
        }

        public void OnRobotCollided()
        {
            OnLandmineCleared(LandmineCleared.Explosion);
        }
        
        private void ShowQuestionOverlay()
        {
            // Define mine in the question overlay
            _questionOverlay.Mine = this;
            // Show question overlay
            _questionOverlay.gameObject.SetActive(true);
        }
        
    }
}
