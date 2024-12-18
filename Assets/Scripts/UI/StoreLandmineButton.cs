using System;
using Objects;

namespace UI
{
    public class StoreLandmineButton : StoreButton
    {
        public void Init(Action onClickCallback, Landmine landmine)
        {
            Init(onClickCallback, landmine.Name, landmine.Price, landmine.Icon);
        }
    }
}