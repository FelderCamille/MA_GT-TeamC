using System;
using System.Collections.Generic;
using Controllers;
using Core;
using UnityEngine;

namespace Objects
{
    public abstract class Bonus
    {
        public BonusName BonusName;
        public string Name;
        public string Icon;
        protected BonusType BonusType;
        public BonusLevel CurrentLevel;
        public Dictionary<BonusLevel, BonusLevelAttributes> Values;

        public void UpgradeLevel()
        {
            var nextLevel = Constants.Bonus.NextBonusLevel(CurrentLevel);
            if (nextLevel == null) return;
            CurrentLevel = (BonusLevel) nextLevel;
        }
        
        public void ApplyBonus(ResourcesManager resourcesManager)  
        {
            // Get price and value
            var price = Values[CurrentLevel].Price;
            var value = Values[CurrentLevel].Value;
            // Check if the player has enough money
            var hasEnoughMoney = resourcesManager.HasEnoughMoneyToBuy(price);
            if (!hasEnoughMoney) return;
            // Remove the money from the player
            resourcesManager.ReduceMoney(price);
            // Add the bonus to the player
            resourcesManager.AddBonus(this);
            // Apply the bonus
            var robot = resourcesManager.GetComponent<RobotController>();
            switch (BonusType)
            {
                case BonusType.Vision:
                    resourcesManager.SetVision(true);
                    robot.ShowMines();
                    break;
                default:
                    throw new NotImplementedException();
            }
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
                    resourcesManager.SetVision(false);
                    robot.HideMines();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}