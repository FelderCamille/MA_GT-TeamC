namespace Objects
{
    public interface IRobot
    {

        /// <summary>
        /// Method <c>IncreaseClearedMineCounter</c> increase the cleared landmine counter
        /// </summary>
        public void IncreaseClearedMineCounter();
        
        /// <summary>
        /// Method <c>ReduceHealth</c> reduces the health
        /// </summary>
        /// <param name="value">The health to reduce</param>
        public void ReduceHealth(float value);

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
