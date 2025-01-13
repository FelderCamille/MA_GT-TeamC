using Objects;

namespace UI
{
    public class InventoryBonusIcon : InventoryIcon
    {
        public void Init(Bonus bonus)
        {
            Init(bonus.Icon, bonus.Values[bonus.CurrentLevel].Value, null);;
        }

        public float? GetValue()
        {
            return number.text == "" ? null : float.Parse(number.text);
        }
    }
}