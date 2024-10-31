namespace Objects
{
    public interface ILandmine
    {

        /// <summary>
        /// Method <c>DetectRobotApproach</c> detects when the robot approach the landmine.
        /// If the robot is at a certain distance, it manages the question answering.
        /// </summary>
        public void DetectRobotApproach();
        
        /// <summary>
        /// Method <c>OnLandmineCleared</c> is called when the landmine has been cleared.
        /// </summary>
        public void OnLandmineCleared(bool success);
        
        /// <summary>
        /// Method <c>OnRobotCollided</c> is called when the robot enters in collision with the landmine.
        /// </summary>
        public void OnRobotCollided();

    }
}
