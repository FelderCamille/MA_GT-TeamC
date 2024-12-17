using System;
using Core;
using Objects;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{
    public class RobotController : NetworkBehaviour, IRobot
    {
        private const int NumberOfTile = Constants.GameSettings.NumberOfTileMovement;

        // Effects
        [Header("Parts")]
        [SerializeField] private GameObject robotObject;
        [SerializeField] private GameObject directionalArrowObject;
        [SerializeField] private ParticleSystem singleWaveEffect;
        [SerializeField] private GameObject repeatedWaveEffect;

        // Controllers
        private ResourcesManager _resourcesManager;
        private QuestionController _questionOverlay;
        private StoreController _storeOverlay;
        private GridController _grid;
        // robot
        private Animator _animator;
        [SerializeField] private ParticleSystem _mudParticules;

        // Audio
        private SoundManager _soundManager;

        // Movements
        [Header("Movements")]
        [SerializeField] private float moveSpeed = 5f; // Movement speed
        [SerializeField] private float rotationSpeed = 180f; // Rotation speed
        private Vector3 _moveDirection; // Current movement direction

        private void Start()
         {
             // Attach camera
             if (IsOwner) FindFirstObjectByType<FollowPlayerCameraController>().Init(this);
             // Initialize references
             _grid = FindFirstObjectByType<GridController>();
             _questionOverlay = FindFirstObjectByType<QuestionController>(FindObjectsInactive.Include);
             _storeOverlay = FindFirstObjectByType<StoreController>(FindObjectsInactive.Include);
             _soundManager = FindFirstObjectByType<SoundManager>();
             _resourcesManager = GetComponent<ResourcesManager>();
             // Hide the robot for the enemy
             if (!IsOwner && !Constants.DebugShowOtherPlayer) Hide();
             // Play single wave effect at the start
             singleWaveEffect.Play();
            _animator = GetComponent<Animator>();
            _mudParticules = GetComponentInChildren<ParticleSystem>();
            _mudParticules.Stop();
         }

        private void Update()
        {
            // Only control the robot that we own
            if (!IsOwner) return;
            // Do nothing if the robot is answering a question or in the store
            if (_questionOverlay.IsAnswering || _storeOverlay.IsShopping) return;
            // Handle mining
            HandleMining();
            // Handle movements
            HandleMovements();
        }

        private void HandleMining()
        {
            if (Input.GetKeyDown(Constants.Actions.PlaceMine))
            {
                if (_resourcesManager.CanPlaceMineOfSelectedDifficulty())
                {
                    var (x, y) = ComputeLandminePlacement();
                    if(_grid.CanPlaceMine(x, y))
                    {
                        _resourcesManager.DecreaseInventoryMineOfSelectedDifficulty();
                        PlaceLandmineRpc(x, y, _resourcesManager.SelectedLandmineDifficulty);
                        _soundManager.PlaySetMineSound();
                    } // TODO: add other feedback otherwise
                } // TODO: add feedback if not enough mines
            }
        }

        private void HandleMovements()
        {
            // Handle rotation
            if (Input.GetKey(Constants.Actions.MoveRight))
            {
                transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f); // Turn right
                PlaySoundIfNeeded(_soundManager.turnSoundSource, () => _soundManager.PlayTankTurnSound());
            }
            else if (Input.GetKey(Constants.Actions.MoveLeft))
            {
                transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f); // Turn left
                PlaySoundIfNeeded(_soundManager.turnSoundSource, () => _soundManager.PlayTankTurnSound());
            }
            else
            {
                _soundManager.turnSoundSource.Stop(); // Stop rotation sound if no rotation
            }

            // Handle forward and backward movement
            if (Input.GetKey(Constants.Actions.MoveUp))
            {
                _moveDirection = transform.forward; // Move forward
                PlaySoundIfNeeded(_soundManager.moveSoundSource, () => _soundManager.PlayTankGoSound());
                _animator.SetTrigger("MoveForward"); // Trigger animation
                if (!_mudParticules.isPlaying)
                {
                    var shape = _mudParticules.shape;
                    shape.position = new Vector3(2.5f, -1.2f, -0.5f);
                    shape.rotation = new Vector3(0, 180, 0);
                    _mudParticules.Play();
                }
            }
            else if (Input.GetKey(Constants.Actions.MoveDown))
            {
                _moveDirection = -transform.forward; // Move backward
                PlaySoundIfNeeded(_soundManager.moveSoundSource, () => _soundManager.PlayTankGoSound());
                _animator.SetTrigger("MoveBackward"); // Trigger animation
                if (!_mudParticules.isPlaying)
                {
                    var shape = _mudParticules.shape;
                    shape.position = new Vector3(-2.5f, -1.2f, 0.5f);
                    shape.rotation = new Vector3(0, 0, 0);
                    _mudParticules.Play();
                }
            }
            else
            {
                _moveDirection = Vector3.zero; // No movement
                _soundManager.moveSoundSource.Stop();
                _mudParticules.Stop();
            }

            // Apply movement respecting grid boundaries
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            var newPosition = transform.position + _moveDirection * moveSpeed * Time.deltaTime;

            // Ensure the robot stays within the grid boundaries
            if (newPosition.x >= _grid.MinX && newPosition.x <= _grid.MaxX - NumberOfTile &&
                newPosition.z >= _grid.MinZ && newPosition.z <= _grid.MaxZ - NumberOfTile)
            {
                transform.position = newPosition; // Move directly via transform (ClientNetworkTransform will sync)
            }
        }

        private void PlaySoundIfNeeded(AudioSource soundSource, System.Action playSoundAction)
        {
            if (!soundSource.isPlaying)
            {
                playSoundAction();
            }
        }

        public void IndicateClearedMine(LandmineDifficulty difficulty)
        {
            _resourcesManager.IncreaseClearedMinesCounter(difficulty);
            _resourcesManager.IncreaseMoney(Constants.Prices.ClearMineSuccess);
        }
        
        public void IndicateExplodedMine(bool failure = false)
        {
            var value = failure ? Constants.Health.RemovedWhenFailure : Constants.Health.RemovedWhenExplosion;
            _resourcesManager.ReduceHealth(value);
            _resourcesManager.IncreaseExplodedMinesCount();
        }

        public bool Repair()
        {
            if (!_resourcesManager.HasEnoughMoneyToBuy(Constants.Prices.Repair)) return false;
            _resourcesManager.ReduceMoney(Constants.Prices.Repair);
            _resourcesManager.Repair();
            return true;
        }

        public bool CanRepair()
        {
            return _resourcesManager.HasEnoughMoneyToBuy(Constants.Prices.Repair) && _resourcesManager.NeedRepair();
        }

        public bool CanBuyBonus(Bonus bonus)
        {
            return _resourcesManager.HasEnoughMoneyToBuy(bonus.Price) && !_resourcesManager.HasBonus(bonus);
        }

        public void ShowMines()
        {
            repeatedWaveEffect.SetActive(true);
            repeatedWaveEffect.GetComponentInChildren<ParticleSystem>().Play();
        }
        
        public void HideMines()
        {
            repeatedWaveEffect.GetComponentInChildren<ParticleSystem>().Stop();
            repeatedWaveEffect.SetActive(false);
        }

        public void Hide()
        {
            robotObject.SetActive(false); 
            directionalArrowObject.SetActive(false);
            repeatedWaveEffect.SetActive(false);
            singleWaveEffect.gameObject.SetActive(false);
        }

        public void Show()
        {
            if (!(IsOwner || Constants.DebugShowOtherPlayer)) return;
            robotObject.SetActive(true);
            directionalArrowObject.SetActive(true);
            repeatedWaveEffect.SetActive(true);
            singleWaveEffect.gameObject.SetActive(true);
        }

        private (int, int) ComputeLandminePlacement()
        {
            Debug.Log($"Robot {OwnerClientId} wants to place a mine");
            // Get the robot's position and direction
            var robotPosition = transform.position;
            var forwardDirection = transform.forward;
            Debug.Log("Robot position: " + robotPosition + ", forward direction: " + forwardDirection);
            // Compute direction
            var facingLeft = Math.Round(forwardDirection.x) < 0;
            var facingRight = Math.Round(forwardDirection.x) > 0;
            var facingUp = Math.Round(forwardDirection.z) > 0;
            var facingDown = Math.Round(forwardDirection.z) < 0;
            Debug.Log("Facing left: " + facingLeft + ", facing right: "+ facingRight +", facing up: " + facingUp + ", facing down: " + facingDown);
            // If the robot is over another tile, we need to place the mine on the next tile
            var robotPositionX = (int) (facingRight ? Math.Ceiling(robotPosition.x) : (facingLeft ? Math.Floor(robotPosition.x) : Math.Round(robotPosition.x)));
            var robotPositionZ = (int) (facingUp ? Math.Ceiling(robotPosition.z) : (facingDown ? Math.Floor(robotPosition.z) : Math.Round(robotPosition.z)));
            Debug.Log($"Robot position X=" + robotPositionX + ", Z=" + robotPositionZ);
            // Compute offsets
            var offsetX = facingLeft ? -1 : facingRight ? 1 : 0;
            var offsetZ = facingDown ? -1 : facingUp ? 1 : 0;
            // Determine the direction the robot is facing
            var mineX = robotPositionX + offsetX;
            var mineZ = robotPositionZ + offsetZ;
            Debug.Log("Mine will be placed at X=" + mineX + ", Z=" + mineZ);
            // Return the emplacement
            return (mineX, mineZ);
        }
        
        [Rpc(SendTo.Everyone)]
        private void PlaceLandmineRpc(int x, int y, LandmineDifficulty difficulty)
        {
            // Place the mine
            _grid.ReplaceTileByMine(x, y, difficulty);
        }

    }
}
