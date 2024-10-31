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

    }
}
