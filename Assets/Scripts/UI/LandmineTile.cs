using UnityEngine;

namespace UI
{
    public class LandmineTile : Tile
    {

        public GameObject landmine;

        public void Show()
        {
            if (!landmine.activeSelf) landmine.SetActive(true);
        }

        public void Hide()
        {
            if (landmine.activeSelf) landmine.SetActive(false);
        }
    }
}