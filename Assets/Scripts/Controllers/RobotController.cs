using System;
using Core;
using Objects;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{
    public class RobotController : NetworkBehaviour, IRobot
    {
        // Parts
        [Header("Parts")]
        [SerializeField] private GameObject robotObject;
        [SerializeField] private ParticleSystem singleWaveEffect;
        [SerializeField] private ParticleSystem repeatedWaveEffect;
        [SerializeField] private ParticleSystem mudParticules;
        [SerializeField] private Animator animator;

        // Controllers
        private ResourcesManager _resourcesManager;
        private QuestionController _questionOverlay;
        private StoreController _storeOverlay;
        private GridController _grid;

        // Audio
        private SoundManager _soundManager;

        // Movements
        private const int NumberOfTile = Constants.GameSettings.NumberOfTileMovement;
        private const float RotationSpeed = 180f; // Rotation speed
        private float _moveSpeed; // Movement speed
        private Vector3 _moveDirection; // Current movement direction
        
        public Vector3 GridPosition => new (transform.position.x, 0, transform.position.z);
        
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
             if (!IsOwner && !Constants.DebugShowOtherPlayer)
             {
                 Hide();
                 return;
             }
             // Play single wave effect at the start and forever repeated wave effect after
             singleWaveEffect.Play();
             SetSpeed();
             ShowMines();
         }

        private void Update()
        {
            // Only control the robot that we own
            if (!IsOwner) return;
            // Do nothing if dead
            if (_resourcesManager.IsDead) return;
            // Do nothing if the robot is answering a question or in the store
            if (_questionOverlay.IsAnswering || _storeOverlay.IsShopping) return;
            // Handle mining
            HandleMining();
            // Handle movements
            HandleMovements();
        }

        private void HandleMining()
        {
            // Do nothing if the robot doesn't want to place a mine
            if (!Input.GetKeyDown(Constants.Actions.PlaceMine)) return;
            // Check if the robot can place a mine of the selected difficulty
            if (_resourcesManager.CanPlaceMineOfSelectedDifficulty())
            {
                var (x, y) = ComputeLandminePlacement();
                if(_grid.CanPlaceMine(x, y, OwnerClientId))
                {
                    _resourcesManager.DecreaseInventoryMineOfSelectedDifficulty();
                    PlaceLandmineRpc(x, y, _resourcesManager.SelectedLandmineDifficulty, OwnerClientId);
                    _soundManager.PlaySetMineSound();
                    return;
                }
            }
            // Play denied sound if the mine cannot be placed
            _soundManager.PlayDeniedSound();
        }

        private void HandleMovements()
        {
            if (_moveSpeed == 0) return;
            // Handle rotation
            if (Input.GetKey(Constants.Actions.MoveRight))
            {
                transform.Rotate(0f, RotationSpeed * Time.deltaTime, 0f); // Turn right
                PlaySoundIfNeeded(_soundManager.turnSoundSource, () => _soundManager.PlayTankTurnSound());
            }
            else if (Input.GetKey(Constants.Actions.MoveLeft))
            {
                transform.Rotate(0f, -RotationSpeed * Time.deltaTime, 0f); // Turn left
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
                RunMudAnimation(true);
            }
            else if (Input.GetKey(Constants.Actions.MoveDown))
            {
                _moveDirection = -transform.forward; // Move backward
                PlaySoundIfNeeded(_soundManager.moveSoundSource, () => _soundManager.PlayTankGoSound());
                RunMudAnimation(false);
            }
            else
            {
                _moveDirection = Vector3.zero; // No movement
                _soundManager.moveSoundSource.Stop();
                mudParticules.Stop();
            }
            
            // Apply movement respecting grid boundaries
            ApplyMovement();
        }
        
        private void RunMudAnimation(bool moveForward)
        {
            if (mudParticules.isPlaying) return;
            if (moveForward)
            {
                // animator.SetTrigger("MoveForward"); // Trigger animation TODO: does not exist
                var shape = mudParticules.shape;
                shape.position = new Vector3(2.5f, -1.2f, -0.5f);
                shape.rotation = new Vector3(0, 180, 0);
                mudParticules.Play();
            }
            else
            {
                // animator.SetTrigger("MoveBackward"); // Trigger animation TODO: does not exist ?
                var shape = mudParticules.shape;
                shape.position = new Vector3(-2.5f, -1.2f, 0.5f);
                shape.rotation = new Vector3(0, 0, 0);
                mudParticules.Play();
            }
        }

        private void ApplyMovement()
        {
            var newPosition = transform.position + _moveDirection * _moveSpeed * Time.deltaTime;

            // Ensure the robot stays within the grid boundaries
            if (newPosition.x >= _grid.MinX && newPosition.x <= _grid.MaxX - NumberOfTile &&
                newPosition.z >= _grid.MinZ && newPosition.z <= _grid.MaxZ - NumberOfTile)
            {
                transform.position = newPosition; // Move directly via transform (ClientNetworkTransform will sync)
            }
        }

        private static void PlaySoundIfNeeded(AudioSource soundSource, Action playSoundAction)
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
        
        public void IndicateExplodedMine() => IndicateExplodedMine(Constants.Damages.RemovedWhenExplosion);
        
        public void IndicateExplodedMine(LandmineDifficulty difficulty)
        {
            var removedHealth = difficulty switch
            {
                LandmineDifficulty.Easy => Constants.Damages.RemovedWhenFailureEasy,
                LandmineDifficulty.Medium => Constants.Damages.RemovedWhenFailureMedium,
                LandmineDifficulty.Hard => Constants.Damages.RemovedWhenFailureHard,
                _ => throw new NotImplementedException("Unknown landmine difficulty")
            };
            IndicateExplodedMine(removedHealth);
        }
        
        private void IndicateExplodedMine(int healthToRemove)
        {
            _resourcesManager.ReduceHealth(healthToRemove);
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
            var price = bonus.Values[bonus.CurrentLevel].Price;
            return _resourcesManager.HasEnoughMoneyToBuy(price) && !_resourcesManager.HasBonus(bonus);
        }

        public void SetSpeed()
        {
            var speed = _resourcesManager.GetBonusValue(BonusName.Speed) ?? SpeedBonus.BaseSpeed;
            _moveSpeed = speed;
        }

        public void ShowMines()
        {
            repeatedWaveEffect.Stop();
            var level = _resourcesManager.GetBonusLevel(BonusName.Detection) ?? BonusLevel.Zero;
            var value = Constants.Bonus.RepeatedWaveEffectStartSize(level);
            var pMain = repeatedWaveEffect.main;
            pMain.startSize = new ParticleSystem.MinMaxCurve(value, value);
            repeatedWaveEffect.Play();
        }
        
        public void HideMines()
        {
            repeatedWaveEffect.Stop();
        }

        public void Hide()
        {
            robotObject.SetActive(false);
            singleWaveEffect.Stop();
            repeatedWaveEffect.Stop();
            mudParticules.Stop();
        }

        public void Show(bool force = false)
        {
            if (!force && !(IsOwner || Constants.DebugShowOtherPlayer)) return;
            robotObject.SetActive(true);
            if (!force)
            {
                singleWaveEffect.Stop();
                ShowMines();
            }
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
            Debug.Log("Mine will be placed at X=" + mineX + ", Z=" + mineZ + " if possible");
            // Return the emplacement
            return (mineX, mineZ);
        }
        
        [Rpc(SendTo.Server)]
        private void PlaceLandmineRpc(int x, int y, LandmineDifficulty difficulty, ulong clientId)
        {
            // Place the mine
            _grid.ReplaceTileByMine(x, y, difficulty, clientId);
        }

    }
}
