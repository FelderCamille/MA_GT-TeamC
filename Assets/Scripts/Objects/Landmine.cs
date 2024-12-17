using System;
using Core;

namespace Objects
{
    public class Landmine
    {
        public string Name;
        public int Price;
        public string Icon;
        public LandmineDifficulty Difficulty;

        public void Buy(ResourcesManager resourcesManager, Action action)
        {
            // Check if the player has enough money
            if (!resourcesManager.HasEnoughMoneyToBuy(Price)) return;
            // Remove the money from the player
            resourcesManager.ReduceMoney(Price);
            // Add the landmine to the player
            resourcesManager.IncreaseInventoryMine(Difficulty);
            // Run action
            action();
        }
    }
}