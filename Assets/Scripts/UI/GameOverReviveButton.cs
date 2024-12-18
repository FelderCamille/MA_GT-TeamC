using System;

namespace UI
{
    public class GameOverReviveButton : StoreButton
    {
        public void Init(Action onClickCallback)
        {
            Init(onClickCallback, "Réapparaître", Constants.Prices.Revive, "Icons/repair");
        }
    }
}