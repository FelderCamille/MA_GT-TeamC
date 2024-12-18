namespace Objects
{
    public class GlassesBonus : Bonus
    {
        public GlassesBonus()
        {
            Name = "Glasses";
            BonusType = BonusType.Vision;
            Price = Constants.Prices.Vision;
            Icon = "Icons/glasses";
            Multiplier = 2;
        }
    }
}