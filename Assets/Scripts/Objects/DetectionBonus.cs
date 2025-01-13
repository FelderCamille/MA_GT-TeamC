using System.Collections.Generic;

namespace Objects
{
    public class DetectionBonus : Bonus
    {
        public DetectionBonus()
        {
            BonusName = BonusName.Detection;
            Name = "Détection";
            BonusType = BonusType.Vision;
            Icon = "Icons/glasses";
            CurrentLevel = BonusLevel.Zero;
            Values = new Dictionary<BonusLevel, BonusLevelAttributes>()
            {
                {BonusLevel.Zero, new BonusLevelAttributes {Price = 0, Value = 1}},
                {BonusLevel.One, new BonusLevelAttributes {Price = 200, Value = 2}},
                {BonusLevel.Two, new BonusLevelAttributes {Price = 400, Value = 3}},
                {BonusLevel.Three, new BonusLevelAttributes {Price = 600, Value = 4}},
                {BonusLevel.Four, new BonusLevelAttributes {Price = 800, Value = 5}},
                {BonusLevel.Five, new BonusLevelAttributes {Price = 1000, Value = 7}},
            };
        }
    }
}