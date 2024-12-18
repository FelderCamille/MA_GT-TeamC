using System;
using Controllers;
using Core;

namespace Objects
{
    public abstract class Bonus
    {
        public string Name;
        public int Price;
        public string Icon;
        public double Multiplier;
        protected BonusType BonusType;
        
        public void ApplyBonus(ResourcesManager resourcesManager, Action action)  
        {
            // Check if the bonus is already applied
            var hasBonus = resourcesManager.HasBonus(this);
            if (hasBonus) return;
            // Check if the player has enough money
            var hasEnoughMoney = resourcesManager.HasEnoughMoneyToBuy(Price);
            if (!hasEnoughMoney) return;
            // Remove the money from the player
            resourcesManager.ReduceMoney(Price);
            // Add the bonus to the player
            resourcesManager.AddBonus(this);
            // Apply the bonus
            var robot = resourcesManager.GetComponent<RobotController>();
            switch (BonusType)
            {
                case BonusType.Vision:
                    resourcesManager.SetVision(Multiplier);
                    robot.ShowMines();
                    break;
                default:
                    throw new NotImplementedException();
            }
            action();
        }

        public void RemoveBonus(ResourcesManager resourcesManager)
        {
            // Check if the bonus has the bonus
            var hasBonus = resourcesManager.HasBonus(this);
            if (!hasBonus) return;
            // Remove the bonus from the player
            resourcesManager.RemoveBonus(this);
            // Apply the bonus
            var robot = resourcesManager.GetComponent<RobotController>();
            switch (BonusType)
            {
                case BonusType.Vision:
                    resourcesManager.SetVision(Constants.GameSettings.Vision);
                    robot.HideMines();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}