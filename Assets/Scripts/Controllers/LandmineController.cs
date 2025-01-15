using System;
using Core;
using System.Collections;
using System.Linq;
using Objects;
using UI;
using UI.Tile;
using UnityEngine;
using Unity.Netcode;

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

        private RobotController _robot;
        private GridController _grid;

        // Animation
        private Animator _animator;

        private ulong _robotOnItAtSpawn = ulong.MaxValue;
        private Vector3 GridPosition => new (transform.position.x, 0, transform.position.z);
        
        private readonly NetworkVariable<int> _landmineDifficultyData = new(Constants.Landmines.LandmineDifficultyToNumber(LandmineDifficulty.Easy));
        public LandmineDifficulty Difficulty
        {
            get => Constants.Landmines.NumberToLandmineDifficulty(_landmineDifficultyData.Value);
            set => _landmineDifficultyData.Value = Constants.Landmines.LandmineDifficultyToNumber(value);
        }
        
        private void Start()
        {
            _questionOverlay = FindFirstObjectByType<QuestionController>(FindObjectsInactive.Include);
            _soundManager = SoundManager.Instance;
            _grid = FindFirstObjectByType<GridController>();
            var robots = FindObjectsByType<RobotController>(FindObjectsSortMode.None);
            _robot = robots.First(robot => robot.IsOwner);
            _animator = _robot.GetComponent<Animator>();
            // Handle when the landmine is placed below the enemy robot
            foreach (var robot in robots)
            {
                // Indicate robot owner
                if (!(Vector3.Distance(robot.GridPosition, GridPosition) < 0.5f)) continue;
                _robotOnItAtSpawn = robot.OwnerClientId;
                break;
            }
        }
        
        private void Update()
        {
            DetectRobotsApproach();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            // Get robot
            var robot = other.gameObject.GetComponent<RobotController>();
            // Manage when collision on the robot object and not the prefab
            if (robot == null) robot = other.gameObject.GetComponentInParent<RobotController>();
            if (robot.OwnerClientId == _robotOnItAtSpawn || !_robot.IsOwner) return; // This robot is on the landmine at spawn, no explosion
            // Handle collision response
            OnRobotCollided(robot);
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Player") && _robotOnItAtSpawn != ulong.MaxValue)
            {
                _robotOnItAtSpawn = ulong.MaxValue;
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Effects"))
            {
                landmine.Show();
            }
        }

        private void DetectRobotsApproach()
        {
            // Check if the user wants to clear the mine, if not return
            if (!Input.GetKeyDown(Constants.Actions.ClearMine) || _questionOverlay.IsAnswering) return;
            // Check if the distance between the robots and the landmine permits to answer the question, if not return
            if (!(Vector3.Distance(transform.position, _robot.gameObject.transform.position) < collidingDistance)) return;
            // Check if the user is in its side
            var onOwnSide = GridController.IsOnOwnSide(_robot.GridX, _robot.OwnerClientId);
            if (!onOwnSide)
            {
                _soundManager.PlayDeniedSound();
                return;
            }
            // Play arm animation
            _animator.SetTrigger("ArmOut");
            // Show question overlay
            _soundManager.PlayOpenMineSound();
            StartCoroutine(DelayedShowQuestionOverlay(_robot));

        }
        
        public void OnLandmineCleared(RobotController robot, LandmineCleared state)
        {
            _animator.SetTrigger("ArmIn");

            // Manage result on robot
            if (robot.IsOwner)
            {
                switch (state)
                {
                    case LandmineCleared.AnswerSuccess:
                        _soundManager.PlayBeepSound();
                        robot.IndicateClearedMine(Difficulty);
                        break;
                    case LandmineCleared.AnswerFailure:
                        robot.IndicateExplodedMine(Difficulty);
                        break;
                    case LandmineCleared.Explosion:
                        robot.IndicateExplodedMine();
                        break;
                    default:
                        throw new Exception("Unknown landmine cleared state");
                }
            }
            // Remove landmine for each client
            if (state == LandmineCleared.AnswerSuccess)
            {
                ReplaceLandmineRpc();
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
            ReplaceLandmineRpc();
        }

        [Rpc(SendTo.Server)]
        private void ReplaceLandmineRpc()
        {
            _grid.ReplaceMineByTile(GetComponentInParent<LandmineTile>());
        }

        public void OnRobotCollided(RobotController robot)
        {
            OnLandmineCleared(robot, LandmineCleared.Explosion);
        }

        private IEnumerator DelayedShowQuestionOverlay(RobotController robot)
        {
            yield return new WaitForSeconds(1.0f);

            // Open question overlay
            ShowQuestionOverlay(robot);
        }
        
        private void ShowQuestionOverlay(RobotController robot)
        {
            // Define mine in the question overlay
            _questionOverlay.Mine = this;
            _questionOverlay.Robot = robot;
            // Show question overlay
            _questionOverlay.gameObject.SetActive(true);
        }
        
    }
}
