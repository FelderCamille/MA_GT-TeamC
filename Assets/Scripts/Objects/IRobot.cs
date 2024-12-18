namespace Objects
{
    public interface IRobot
    {

        /// <summary>
        /// Method <c>IncreaseClearedMineCounter</c> increase the cleared landmine counter
        /// </summary>
        public void IndicateClearedMine(LandmineDifficulty difficulty);
        
        /// <summary>
        /// Method <c>IndicateExplodedMine</c> reduces the health because of a wrong answer to a question
        /// </summary>
        /// <param name="difficulty">The difficulty of the question</param>
        public void IndicateExplodedMine(LandmineDifficulty difficulty);
        
        /// <summary>
        /// Method <c>IndicateExplodedMine</c> reduces the health because of a collision with a landmine
        /// </summary>
        public void IndicateExplodedMine();

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
