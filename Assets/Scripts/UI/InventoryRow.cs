using System;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class InventoryRow : MonoBehaviour
    {

        [FormerlySerializedAs("landmineIconPrefab")] [SerializeField] private InventoryLandmineIcon inventoryLandmineIconPrefab;
        [FormerlySerializedAs("bonusIconPrefab")] [SerializeField] private InventoryBonusIcon inventoryBonusIconPrefab;

        private void Start()
        {
            // Add all the mines
            foreach (LandmineDifficulty difficulty in Enum.GetValues(typeof(LandmineDifficulty)))
            {
                var mineIconObj = Instantiate(inventoryLandmineIconPrefab, transform);
                mineIconObj.name = "Mine " + difficulty;
                mineIconObj.Init("Icons/bomb", difficulty);
            }
        }

        public void AddBonus(Bonus bonus)
        {
            var bonusObj = Instantiate(inventoryBonusIconPrefab, transform);
            bonusObj.name = bonus.Name;
            bonusObj.Init(bonus.Icon);
        }
        
        public void RemoveBonus(Bonus bonus)
        {
            var bonusObj = transform.Find(bonus.Name);
            if (bonusObj != null)
            {
                Destroy(bonusObj.gameObject);
            }
        }
    }
}