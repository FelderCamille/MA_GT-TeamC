using System;
using System.Linq;
using Controllers;
using Core;
using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StoreSection : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private HorizontalLayoutGroup bonusesEmplacement;
        [SerializeField] private StoreBonusButton storeBonusButtonPrefab;
        [SerializeField] private StoreLandmineButton storeLandmineButtonPrefab;
        
        public void InitBonusSection(BonusType bonusType, Action action)
        {
            // Retrieve bonuses of the given type
            var bonuses = Constants.Bonus.BonusesPerType(bonusType);
            // Set the title
            title.text = Constants.Bonus.BonusTypeName(bonusType);
            // Retrieve resource manager
            var currentRobot = FindObjectsByType<RobotController>(FindObjectsSortMode.None).First(robot => robot.IsOwner);
            var resourcesManager = currentRobot.GetComponentInChildren<ResourcesManager>();
            // Add bonus buttons
            foreach (var bonus in bonuses)
            {
                var bonusButtonObj = Instantiate(storeBonusButtonPrefab, bonusesEmplacement.transform);
                bonusButtonObj.name = bonus.Name;
                bonusButtonObj.Init(
                    () => bonus.ApplyBonus(resourcesManager, action), 
                    bonus
                );
            }
        }
        
        public void InitLandmineSection(Action action)
        {
            // Set the title
            title.text = "Mines";
            // Retrieve bonuses of the given type
            var landmines = Constants.Landmines.LandminesObjects();
            // Retrieve resource manager
            var currentRobot = FindObjectsByType<RobotController>(FindObjectsSortMode.None).First(robot => robot.IsOwner);
            var resourcesManager = currentRobot.GetComponentInChildren<ResourcesManager>();
            // Add bonus buttons
            foreach (var landmine in landmines)
            {
                var landmineButtonObj = Instantiate(storeLandmineButtonPrefab, bonusesEmplacement.transform);
                landmineButtonObj.name = landmine.Name;
                landmineButtonObj.Init(
                    () => landmine.Buy(resourcesManager, action), 
                    landmine
                );
            }
        }
    }
}
