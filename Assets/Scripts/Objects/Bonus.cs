using System;
using Controllers;
using Core;
using UnityEngine;

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

        public void RemoveBonus(ResourcesManager resourcesManager, RobotController robot)
        {
            // Check if the bonus has the bonus
            var hasBonus = resourcesManager.HasBonus(this);
            if (!hasBonus) return;
            // Remove the bonus from the player
            resourcesManager.RemoveBonus(this);
            // Apply the bonus
            switch (BonusType)
            {
                case BonusType.Vision:
                    resourcesManager.MultiplyVision(1 / Multiplier);
                    robot.HideMines();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}