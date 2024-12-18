using Controllers;

namespace Objects
{
    public interface ILandmine
    {
        
        /// <summary>
        /// Method <c>OnLandmineCleared</c> is called when the landmine has been cleared.
        /// </summary>
        public void OnLandmineCleared(RobotController robot, LandmineCleared state);
        
        /// <summary>
        /// Method <c>OnRobotCollided</c> is called when the robot enters in collision with the landmine.
        /// </summary>
        public void OnRobotCollided(RobotController robot);

    }
}
