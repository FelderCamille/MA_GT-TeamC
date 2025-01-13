using System;
using Objects;

namespace UI
{
    public class StoreBonusButton : StoreButton
    {
        public Bonus Bonus { get; private set; }
        private Action _onClickCallback;
        
        public void Init(Action onClickCallback, Bonus bonus)
        {
            Bonus = bonus;
            _onClickCallback = onClickCallback;
            InitButton();
        }

        public void UpdateButton(Bonus bonus)
        {
            Bonus = bonus;
            InitButton();
        }

        private void InitButton()
        {
            var values = Bonus.Values[Bonus.CurrentLevel];
            var bonusName = Bonus.Name + " (+" + values.Value + ")";
            Init(_onClickCallback, bonusName, values.Price, Bonus.Icon); 
        }
    }
}