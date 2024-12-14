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
        [SerializeField] private ParticleSystem singleWaveEffect;
        [SerializeField] private GameObject repeatedWaveEffect;
        
        // Controllers
        private ResourcesManager _resourcesManager;
        private QuestionController _questionOverlay;
        private StoreController _storeOverlay;
        private GridController _grid;
        private LandmineController _currentLandmine;
        private SoundManager _soundManager;

        // Movements
        [SerializeField] private float moveSpeed = 5f; // Movement speed
        [SerializeField] private float rotationSpeed = 180f; // Rotation speed
        private Vector3 _moveDirection; // Current movement direction
        
        private void Start()
        {
            // Get objects of scene
            _grid = FindObjectOfType<GridController>();
            _questionOverlay = FindObjectOfType<QuestionController>(true);
            _storeOverlay = FindObjectOfType<StoreController>(true);
            _resourcesManager = gameObject.AddComponent<ResourcesManager>();
            _soundManager = FindObjectOfType<SoundManager>();
            // Change color if the robot is the owner (for debugging)
            if (IsOwner) FindObjectOfType<MeshRenderer>().materials[0].color = Color.green;
            // Play single wave
            singleWaveEffect.Play();
        }

        private void Update()
        {
            // Only control the robot that we own
            if (!IsOwner) return;
            // Do nothing if the robot is answering a question or in the store
            if (_questionOverlay.IsAnswering || _storeOverlay.IsShopping) return;
            // Handle movements
            HandleMovements();
        }
        
        private void HandleMovements()
        {
            if (Input.GetKey(Constants.Actions.MoveRight))
            {
                transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f); // Turn right
                if (!_soundManager.turnSoundSource.isPlaying)
                {
                    _soundManager.PlayTankTurnSound();
                }
            }
            else if (Input.GetKey(Constants.Actions.MoveLeft))
            {
                transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f); // Turn left
                if (!_soundManager.turnSoundSource.isPlaying)
                {
                    _soundManager.PlayTankTurnSound();
                }
            }
            else
            {
                _soundManager.turnSoundSource.Stop(); // Stop sound if the robot is not turning
            }

            // Movement always forward or backward
            if (Input.GetKey(Constants.Actions.MoveUp))
            {
                _moveDirection = transform.forward; // Move forward
                if (!_soundManager.moveSoundSource.isPlaying)
                {
                    _soundManager.PlayTankGoSound();
                }
            }
            else if (Input.GetKey(Constants.Actions.MoveDown))
            {
                _moveDirection = -transform.forward; // Move backward
                if (!_soundManager.moveSoundSource.isPlaying)
                {
                    _soundManager.PlayTankGoSound();
                }
            }
            else
            {
                _moveDirection = Vector3.zero; // No movement // TODO - necessary ?
                _soundManager.moveSoundSource.Stop();
            }

            // Apply movement
            var newPosition = transform.position + _moveDirection * moveSpeed * Time.deltaTime;
            if (newPosition.x >= _grid.MinX && newPosition.x <= _grid.MaxX - NumberOfTile &&
                newPosition.z >= _grid.MinZ && newPosition.z <= _grid.MaxZ - NumberOfTile )
            {
                transform.position = newPosition;
            }
        }

        public void IncreaseClearedMineCounter()
        {
            _resourcesManager.IncreaseClearedMinesCounter();
        }

        public void ReduceHealth(float value)
        {
            _resourcesManager.ReduceHealth(value);
        }

        public bool Repair()
        {
            if (!_resourcesManager.HasEnoughMoneyToBuy(Constants.Values.RepairPrice)) return false;
            _resourcesManager.ReduceMoney(Constants.Values.RepairPrice);
            _resourcesManager.Repair();
            return true;
        }

        public bool CanRepair()
        {
            return _resourcesManager.HasEnoughMoneyToBuy(Constants.Values.RepairPrice) && _resourcesManager.NeedRepair();
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
    }

}