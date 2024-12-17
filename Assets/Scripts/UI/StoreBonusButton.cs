using System;
using Objects;

namespace UI
{
    public class StoreBonusButton : StoreButton
    {
        public Bonus Bonus { get; private set; }
        public void Init(Action onClickCallback, Bonus bonus)
        {
            Bonus = bonus;
            var bonusName = bonus.Name + " (" + bonus.Multiplier + ")";
            Init(onClickCallback, bonusName, bonus.Price, bonus.Icon);
        }
    }
}