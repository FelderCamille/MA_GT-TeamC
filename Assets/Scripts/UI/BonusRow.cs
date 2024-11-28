using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class BonusRow : MonoBehaviour
    {

        [FormerlySerializedAs("bonusPrefab")] public BonusIcon bonusIconPrefab;
        
        public void AddBonus(Objects.Bonus bonus)
        {
            var bonusObj = Instantiate(bonusIconPrefab, transform);
            bonusObj.name = bonus.Name;
            bonusObj.Init(bonus.Icon);
        }
        
        public void RemoveBonus(Objects.Bonus bonus)
        {
            var bonusObj = transform.Find(bonus.Name);
            if (bonusObj != null)
            {
                Destroy(bonusObj.gameObject);
            }
        }
    }
}