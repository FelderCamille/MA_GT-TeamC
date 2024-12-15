using System;
using System.Linq;
using Core;
using Objects;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class StoreController : MonoBehaviour
    {
        public bool IsShopping { get; private set; }
        public bool JustOpened { get; set; }
        
        public CloseButton closeButton;
        public StoreButton repairButton;
        public VerticalLayoutGroup bonusSectionsEmplacement;
        public StoreBonusSection storeBonusSectionPrefab;
        
        private RobotController _robot;

        // Audio
        private SoundManager _soundManager;

        private void Awake()
        {
            _soundManager = FindFirstObjectByType<SoundManager>();
            // Retrieve robot
            _robot = FindFirstObjectByType<RobotController>();
            // Init close button
            closeButton.Init(CloseStore);
            // Init repair button
            repairButton.InitRepairButton(RepairRobot);
            // Add bonus sections
            foreach (var bonusType in Enum.GetValues(typeof(BonusType)).Cast<BonusType>())
            {
                var bonusSectionObj = Instantiate(storeBonusSectionPrefab, bonusSectionsEmplacement.transform);
                bonusSectionObj.name = "Section " + bonusType;
                bonusSectionObj.Init(bonusType, _robot, CheckIfCanBuy);
            }
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
            // Enable/Disable bonuses
            var bonusButtons = GetComponentsInChildren<BonusButton>();
            foreach (var bonusButton in bonusButtons)
            {
                if (_robot.CanBuyBonus(bonusButton.bonus)) bonusButton.Enabled();
                else bonusButton.Disable();
            }
        }

        public void OpenStore()
        {
            gameObject.SetActive(true);
            IsShopping = true;
            JustOpened = true;
            _soundManager.playOpenTentSound();
        }

        private void CloseStore()
        {
            IsShopping = false;
            gameObject.SetActive(false);
            _soundManager.playCloseTentSound();
        }
    }
}
