using Objects;

namespace UI
{
    public class InventoryBonusIcon : InventoryIcon
    {
        public void Init(Bonus bonus)
        {
            Init(bonus.Icon, bonus.Values[bonus.CurrentLevel].Value, null);;
        }

        public int? GetValue()
        {
            return number.text == "" ? null : int.Parse(number.text);
        }
    }
}