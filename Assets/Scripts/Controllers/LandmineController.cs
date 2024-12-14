using Core;
using System;
using System.Collections;
using Objects;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class LandmineController : MonoBehaviour, ILandmine
    {
        
        [Header("Settings")]
        public float collidingDistance = Constants.GameSettings.NumberOfTileClearLandmine + .5f; // One tile of distance, no diagonal
        private SoundManager _soundManager;
        private QuestionController _questionOverlay;
        private RobotController _robot;
        public LandmineTile landmine;
        public ParticleSystem explosionEffect;
        
        private void Start()
        {
            _questionOverlay = FindObjectOfType<QuestionController>(true);
            _robot = FindObjectOfType<RobotController>();
            _soundManager = FindObjectOfType<SoundManager>();
        }

        private void Update()
        {
            DetectRobotApproach();
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

        public void DetectRobotApproach()
        {
            // Check if the user wants to clear the mine, if not return
            if (!Input.GetKeyDown(Constants.Actions.ClearMine) || _questionOverlay.IsAnswering) return;
            // Check if the distance between the robot and the landmine permits to answer the question, if not return
            if (!(Vector3.Distance(transform.position, _robot.gameObject.transform.position) < collidingDistance)) return;
            // Check if the robot faces the landmine
            if (_robot.Direction == RobotDirection.FacingRight && transform.position.x > _robot.transform.position.x ||
                _robot.Direction == RobotDirection.FacingLeft && transform.position.x < _robot.transform.position.x ||
                _robot.Direction == RobotDirection.FacingUp && transform.position.z > _robot.transform.position.z ||
                _robot.Direction == RobotDirection.FacingDown && transform.position.z < _robot.transform.position.z)
            {
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
