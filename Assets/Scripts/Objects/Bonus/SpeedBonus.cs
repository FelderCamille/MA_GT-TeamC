using System.Collections.Generic;

namespace Objects
{
    public class SpeedBonus : Bonus
    {
        
        public const float BaseSpeed = 1.5f;
        
        public SpeedBonus()
        {
            BonusName = BonusName.Speed;
            Name = "Vitesse";
            BonusType = BonusType.Speed;
            Icon = "Icons/speed";
            CurrentLevel = BonusLevel.Zero;
            Values = new Dictionary<BonusLevel, BonusLevelAttributes>
            {
                {BonusLevel.Zero, new BonusLevelAttributes {Price = 0, Value = BaseSpeed}},
                {BonusLevel.One, new BonusLevelAttributes {Price = 200, Value = 1.3f * BaseSpeed}},
                {BonusLevel.Two, new BonusLevelAttributes {Price = 400, Value = 1.6f * BaseSpeed}},
                {BonusLevel.Three, new BonusLevelAttributes {Price = 600, Value = 1.9f * BaseSpeed}},
                {BonusLevel.Four, new BonusLevelAttributes {Price = 800, Value = 2.2f * BaseSpeed}},
                {BonusLevel.Five, new BonusLevelAttributes {Price = 1000, Value = 2.5f * BaseSpeed}}
            };
        }
    }
}