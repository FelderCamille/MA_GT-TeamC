using Controllers;
using Core;
using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StoreBonusSection : MonoBehaviour
    {
        public Text title;
        public HorizontalLayoutGroup bonusesEmplacement;
        public StoreButton bonusButtonPrefab;
        
        public void Init(BonusType bonusType)
        {
            // Retrieve bonuses of the given type
            var bonuses = Constants.Bonus.BonusesPerType(bonusType);
            // Set the title
            title.text = Constants.Bonus.BonusTypeName(bonusType);
            // Retrieve resource manager
            var resourcesManager = FindObjectOfType<RobotController>().GetComponentInChildren<ResourcesManager>();
            // Add bonus buttons
            foreach (var bonus in bonuses)
            {
                var bonusButtonObj = Instantiate(bonusButtonPrefab, bonusesEmplacement.transform);
                bonusButtonObj.name = bonus.Name;
                bonusButtonObj.InitBonusButton(() => bonus.ApplyBonus(resourcesManager), bonus);
            }
        }
    }
}
