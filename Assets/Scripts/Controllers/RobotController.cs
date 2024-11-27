using Core;
using Objects;
using UnityEngine;

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

        private void Start()
        {
            _grid = FindObjectOfType<GridController>();
            _questionOverlay = FindObjectOfType<QuestionController>(true);
            _storeOverlay = FindObjectOfType<StoreController>(true);
            _resourcesManager = gameObject.AddComponent<ResourcesManager>();
            singleWaveEffect.Play();
        }

        private void Update()
        {
            // Do nothing if the robot is answering a question or in the store
            if (_questionOverlay.IsAnswering || _storeOverlay.IsShopping) return;
            // Handle movements
            HandleRotation();
            HandleMovements();
        }
        
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
        }

        private void HandleMovements()
        {
            // Do nothing if the user is rotating
            if(Input.GetKey(Constants.Actions.Rotation)) return;
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
            }
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