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
        private SoundManager _soundManager;

        // Movements
        [Header("Movements")]
        [SerializeField] private float moveSpeed = 5f; // Movement speed
        [SerializeField] private float rotationSpeed = 180f; // Rotation speed
        private Vector3 _moveDirection; // Current movement direction

        private void Start()
         {
             // Do not destroy the gameObject when the scene is changed
             DontDestroyOnLoad(gameObject);
             // Attach camera
             if (IsOwner) FindFirstObjectByType<FollowPlayerCameraController>().Init(this);
             // Initialize references
             _grid = FindFirstObjectByType<GridController>();
             _questionOverlay = FindFirstObjectByType<QuestionController>(FindObjectsInactive.Include);
             _storeOverlay = FindFirstObjectByType<StoreController>(FindObjectsInactive.Include);
             _soundManager = FindFirstObjectByType<SoundManager>();
             _resourcesManager = GetComponent<ResourcesManager>();
             // TODO (TEMP): Set robot color for debugging
             if (IsOwner)
             {
                 GetComponentInChildren<MeshRenderer>().materials[0].color = Color.green;
             }
             // Play single wave effect at the start
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
            }
            else if (Input.GetKey(Constants.Actions.MoveDown))
            {
                _moveDirection = -transform.forward; // Move backward
                PlaySoundIfNeeded(_soundManager.moveSoundSource, () => _soundManager.PlayTankGoSound());
            }
            else
            {
                _moveDirection = Vector3.zero; // No movement
                _soundManager.moveSoundSource.Stop();
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

        public void IndicateClearedMine()
        {
            _resourcesManager.IncreaseClearedMinesCounter();
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
        }

        public void Show()
        {
            robotObject.SetActive(true); 
            directionalArrowObject.SetActive(true);
        }

    }
}
