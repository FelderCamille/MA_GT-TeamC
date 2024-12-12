using System;
using Objects;

namespace UI
{
    public class BonusButton : StoreButton
    {

        public Bonus bonus;
        
        public void InitBonusButton(Action onClickCallback, Objects.Bonus bonus)
        {
            this.bonus = bonus;
            var bonusName = bonus.Name + " (" + bonus.Multiplier + ")";
            Init(onClickCallback, bonusName, bonus.Price, bonus.Icon);
        }
    }
}
