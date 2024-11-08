using UI;
using UnityEngine;

namespace Controllers
{
    public class StoreController : MonoBehaviour
    {
        public bool IsShopping { get; private set; }

        public CloseButton closeButton;
        public StoreButton repairButton;

        private RobotController _robot;
        
        private void Awake()
        {
            _robot = FindObjectOfType<RobotController>();
            closeButton.Init(CloseStore);
            repairButton.InitRepairButton(RepairRobot);
        }
        
        private void OnEnable()
        {
            IsShopping = true;
            // Enable/Disable content
            CheckIfCanBuy();
        }

        private void RepairRobot()
        {
            _robot.Repair();
            CheckIfCanBuy();
        }

        private void CheckIfCanBuy()
        {
            // Enable/Disable repair button
            if (_robot.CanRepair()) repairButton.Enabled();
            else repairButton.Disable();
        }

        private void CloseStore()
        {
            IsShopping = false;
            gameObject.SetActive(false);
        }
    }
}
