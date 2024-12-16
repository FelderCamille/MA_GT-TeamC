namespace Objects
{
    public interface IRobot
    {

        /// <summary>
        /// Method <c>IncreaseClearedMineCounter</c> increase the cleared landmine counter
        /// </summary>
        public void IndicateClearedMine();
        
        /// <summary>
        /// Method <c>IndicateExplodedMine</c> reduces the health
        /// </summary>
        /// <param name="failure">If the mine exploded because the question was answered wrongly</param>
        public void IndicateExplodedMine(bool failure = false);

        /// <summary>
        /// Method <c>Repair</c> repairs the robot
        /// </summary>
        /// <returns>True if the repair has been done, false otherwise.</returns>
        public bool Repair();

        /// <summary>
        /// Check if the robot has enough money to be repair
        /// </summary>
        public bool CanRepair();

    }
}
