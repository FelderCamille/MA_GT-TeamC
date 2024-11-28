using UnityEngine;

namespace Objects
{
    public interface IGrid
    {

        /// <summary>
        ///  If the object can move to the right
        /// </summary>
        /// <param name="newX">The new x position of the object</param>
        /// <returns>True if the object can move to the right, false otherwise.</returns>
        public bool CanMoveRight(float newX);
        
        /// <summary>
        ///  If the object can move to the left
        /// </summary>
        /// <param name="newX">The new x position of the object</param>
        /// <returns>True if the object can move to the left, false otherwise.</returns>
        public bool CanMoveLeft(float newX);
        
        /// <summary>
        ///  If the object can move to the top
        /// </summary>
        /// <param name="newY">The new y position of the object</param>
        /// <returns>True if the object can move to the top, false otherwise.</returns>
        public bool CanMoveUp(float newY);
        
        /// <summary>
        ///  If the object can move to the bottom
        /// </summary>
        /// <param name="newY">The new y position of the object</param>
        /// <returns>True if the object can move to the bottom, false otherwise.</returns>
        public bool CanMoveDown(float newY);

    }
}
