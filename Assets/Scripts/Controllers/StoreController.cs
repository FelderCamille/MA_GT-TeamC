using System;
using System.Linq;
using Core;
using Objects;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controllers
{
    public class StoreController : MonoBehaviour
    {
        public bool IsShopping { get; private set; }
        public bool JustOpened { get; set; }
        
        [SerializeField] private CloseButton closeButton;
        [SerializeField] private StoreRepairButton repairButton;
        [SerializeField] private VerticalLayoutGroup bonusSectionsEmplacement;
        [SerializeField] private StoreSection storeSectionPrefab;
        
        private RobotController _robot;

        // Audio
        private SoundManager _soundManager;

        private void Awake()
        {
            // Initialize variables
            JustOpened = true;
            // Retrieve sound manager
            _soundManager = FindFirstObjectByType<SoundManager>();
            // Retrieve robot
            _robot = FindFirstObjectByType<RobotController>();
            // Init close button
            closeButton.Init(CloseStore);
            // Init repair button
            repairButton.Init(RepairRobot);
            // Add mines
            var minesSectionObj = Instantiate(storeSectionPrefab, bonusSectionsEmplacement.transform);
            minesSectionObj.name = "Section mines";
            minesSectionObj.InitLandmineSection(CheckIfCanBuy);
            // Add bonus sections
            foreach (var bonusType in Enum.GetValues(typeof(BonusType)).Cast<BonusType>())
            {
                var bonusSectionObj = Instantiate(storeSectionPrefab, bonusSectionsEmplacement.transform);
                bonusSectionObj.name = "Section " + bonusType;
                bonusSectionObj.InitBonusSection(bonusType, CheckIfCanBuy);
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
            var bonusButtons = GetComponentsInChildren<StoreBonusButton>();
            foreach (var bonusButton in bonusButtons)
            {
                if (_robot.CanBuyBonus(bonusButton.Bonus)) bonusButton.Enabled();
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
