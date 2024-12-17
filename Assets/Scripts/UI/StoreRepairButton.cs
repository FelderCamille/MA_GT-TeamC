using System;

namespace UI
{
    public class StoreRepairButton : StoreButton
    {
        public void Init(Action onClickCallback)
        {
            Init(onClickCallback, "Réparer", Constants.Prices.Repair, "Icons/repair");
        }
    }
}