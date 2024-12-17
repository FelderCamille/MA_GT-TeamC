using Core;
using Objects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class RobotController : MonoBehaviour, IRobot
    {
        private const int NumberOfTile = Constants.GameSettings.NumberOfTileMovement;
        public RobotDirection Direction { get; private set; }

        private ResourcesManager _resourcesManager;
        public ParticleSystem singleWaveEffect;
        public GameObject repeatedWaveEffect;
        private QuestionController _questionOverlay;
        private StoreController _storeOverlay;
        private GridController _grid;
        private LandmineController _currentLandmine;

        // robot
        private Animator _animator;

        // Audio
        private SoundManager _soundManager;

        [SerializeField] private float moveSpeed = 5f; // Vitesse de d�placement (modifiable dans l'�diteur)
        [SerializeField] private float rotationSpeed = 180f; // Vitesse de rotation (en degr�s/seconde)
        private Vector3 moveDirection; // Direction actuelle du d�placement

        private void Start()
        {
            _grid = FindObjectOfType<GridController>();
            _questionOverlay = FindObjectOfType<QuestionController>(true);
            _storeOverlay = FindObjectOfType<StoreController>(true);
            _resourcesManager = gameObject.AddComponent<ResourcesManager>();
            singleWaveEffect.Play();
            _soundManager = FindObjectOfType<SoundManager>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // Do nothing if the robot is answering a question or in the store
            if (_questionOverlay.IsAnswering || _storeOverlay.IsShopping) return;
            // Handle movements
            //HandleRotation();
            HandleMovements();
        }
        
        /*
        private void HandleRotation()
        {
            // Rotation to right
            if (Input.GetKey(Constants.Actions.Rotation) && Input.GetKeyDown(Constants.Actions.MoveRight))
            {
                RotateRight();
            }
            // Rotation to left
            if (Input.GetKey(Constants.Actions.Rotation) && Input.GetKeyDown(Constants.Actions.MoveLeft))
            {
                RotateLeft();
            }
            // Rotation up
            if (Input.GetKey(Constants.Actions.Rotation) && Input.GetKeyDown(Constants.Actions.MoveUp))
            {
                RotateUp();
            }
            // Rotation down
            if (Input.GetKey(Constants.Actions.Rotation) && Input.GetKeyDown(Constants.Actions.MoveDown))
            {
                RotateDown();
            }
        }*/

        private void HandleMovements()
        {
            if (Input.GetKey(Constants.Actions.MoveRight))
            {
                transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f); // Rotation � droite
                if (!_soundManager.turnSoundSource.isPlaying)
                {
                    _soundManager.PlayTankTurnSound();
                }
            }
            else if (Input.GetKey(Constants.Actions.MoveLeft))
            {
                transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f); // Rotation � gauche
                if (!_soundManager.turnSoundSource.isPlaying)
                {
                    _soundManager.PlayTankTurnSound();
                }
            }
            else
            {
                _soundManager.turnSoundSource.Stop(); // Arr�te le son si la rotation cesse
            }

            // D�placement : Toujours avancer/reculer dans la direction actuelle (orientation)
            if (Input.GetKey(Constants.Actions.MoveUp))
            {
                moveDirection = transform.forward; // Avance dans la direction du regard
                _animator.SetTrigger("MoveForward"); // Trigger animation
                if (!_soundManager.moveSoundSource.isPlaying) // Emp�che les r�p�titions si le son est d�j� en cours
                {
                    _soundManager.PlayTankGoSound();
                }
            }
            else if (Input.GetKey(Constants.Actions.MoveDown))
            {
                moveDirection = -transform.forward; // Recule dans la direction oppos�e
                _animator.SetTrigger("MoveBackward"); // Trigger animation
                if (!_soundManager.moveSoundSource.isPlaying)
                {
                    _soundManager.PlayTankGoSound();
                }
            }
            else
            {
                moveDirection = Vector3.zero; // Pas de mouvement
                _soundManager.moveSoundSource.Stop(); // Arr�te le son si le robot s'arr�te
            }

            // Appliquer le d�placement
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;

            Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
            var gridController = FindObjectOfType<GridController>();
            if (newPosition.x >= gridController.MinX && newPosition.x <= gridController.MaxX - 1.0f &&
                newPosition.z >= gridController.MinZ && newPosition.z <= gridController.MaxZ- 1.0f )
            {
                transform.position = newPosition;
            }


            /*
            // Do nothing if the user is rotating
            if (Input.GetKey(Constants.Actions.Rotation)) return;
            // Move to right
            if (Input.GetKeyDown(Constants.Actions.MoveRight) && _grid.CanMoveRight(transform.position.x + NumberOfTile))
            {
                transform.position += new Vector3(NumberOfTile, 0f, 0f);
                RotateRight();
            }
            // Move to left
            if (Input.GetKeyDown(Constants.Actions.MoveLeft) && _grid.CanMoveLeft(transform.position.x - NumberOfTile))
            {
                transform.position -= new Vector3(NumberOfTile, 0f, 0f);
                RotateLeft();
            }
            // Move up
            if (Input.GetKeyDown(Constants.Actions.MoveUp) && _grid.CanMoveUp(transform.position.z + NumberOfTile))
            {
                transform.position += new Vector3(0f, 0f, NumberOfTile);
                RotateUp();
            }
            // Move down
            if (Input.GetKeyDown(Constants.Actions.MoveDown) && _grid.CanMoveDown(transform.position.z - NumberOfTile))
            {
                transform.position -= new Vector3(0f, 0f, NumberOfTile);
                RotateDown();
            }*/
        }

        private void RotateRight()
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Direction = RobotDirection.FacingRight;
        }

        private void RotateLeft()
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            Direction = RobotDirection.FacingLeft;
        }

        private void RotateUp()
        {
            transform.eulerAngles = new Vector3(0f, -90f, 0f);
            Direction = RobotDirection.FacingUp;
        }
        
        private void RotateDown()
        {
            transform.eulerAngles = new Vector3(0f, 90f, 0f);
            Direction = RobotDirection.FacingDown;
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