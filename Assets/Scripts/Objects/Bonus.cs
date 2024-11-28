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
        
        public void ApplyBonus(ResourcesManager resourcesManager, RobotController robot, Action action)
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
            switch (BonusType)
            {
                case BonusType.Vision:
                    resourcesManager.MultiplyVision(Multiplier);
                    robot.ShowMines();
                    break;
                default:
                    throw new NotImplementedException();
            }
            action();
        }
    }
}