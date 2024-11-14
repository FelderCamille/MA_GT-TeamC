namespace Objects
{
    public class GlassesBonus : Bonus
    {
        public GlassesBonus()
        {
            Name = "Glasses";
            BonusType = BonusType.Vision;
            Price = 100;
            Icon = "Icons/glasses";
            Multiplier = 1.5;
        }
    }
}