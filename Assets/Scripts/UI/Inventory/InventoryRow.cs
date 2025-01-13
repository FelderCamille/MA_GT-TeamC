using System;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Inventory
{
    public class InventoryRow : MonoBehaviour
    {

        
        [FormerlySerializedAs("landmineIconPrefab")]
        [SerializeField] private InventoryLandmineIcon inventoryLandmineIconPrefab;
        [FormerlySerializedAs("bonusIconPrefab")]
        [SerializeField] private InventoryBonusIcon inventoryBonusIconPrefab;

        private readonly InventoryLandmineIcon[] _landminesButtons = new InventoryLandmineIcon[3];
        public LandmineDifficulty SelectedLandmineDifficulty { get; private set; }

        private void Start()
        {
            // Set default value for selected landmine difficulty
            SelectedLandmineDifficulty = LandmineDifficulty.Easy;
            // Add all the mines
            foreach (LandmineDifficulty difficulty in Enum.GetValues(typeof(LandmineDifficulty)))
            {
                var mineIconObj = Instantiate(inventoryLandmineIconPrefab, transform);
                mineIconObj.name = "Mine " + difficulty;
                var isDefault = difficulty == LandmineDifficulty.Easy;
                mineIconObj.Init(() => SelectLandmineDifficulty(difficulty), "Icons/bomb", difficulty, isDefault);
                _landminesButtons[(int) difficulty] = mineIconObj;
            }
        }
        
        private void SelectLandmineDifficulty(LandmineDifficulty difficulty)
        {
            if (SelectedLandmineDifficulty == difficulty) return;
            // Deselect previous landmine difficulty
            _landminesButtons[(int) SelectedLandmineDifficulty].ToggleSelected();
            // Update selected landmine difficulty
            SelectedLandmineDifficulty = difficulty;
            _landminesButtons[(int) difficulty].ToggleSelected();
        }

        public void AddBonus(Bonus bonus)
        {
            var bonusObj = Instantiate(inventoryBonusIconPrefab, transform);
            bonusObj.name = bonus.Name;
            bonusObj.Init(bonus);
        }

        public void UpdateBonus(Bonus bonus)
        {
            var bonusObj = transform.Find(bonus.Name);
            if (bonusObj != null)
            {
                bonusObj.GetComponent<InventoryBonusIcon>().Init(bonus);
            }
            else
            {
                throw new Exception("Bonus not found");
            }
        }
        
        public void RemoveBonus(Bonus bonus)
        {
            var bonusObj = transform.Find(bonus.Name);
            if (bonusObj != null)
            {
                Destroy(bonusObj.gameObject);
            }
        }
        
        public float? GetBonusValue(Bonus bonus)
        {
            var bonusObj = transform.Find(bonus.Name);
            return bonusObj != null ? bonusObj.GetComponent<InventoryBonusIcon>().GetValue() : null;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}