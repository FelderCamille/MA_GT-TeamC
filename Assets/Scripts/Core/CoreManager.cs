using Controllers;
using UnityEngine;

namespace Core
{
    
    public class CoreManager : MonoBehaviour
    {
        public static CoreManager Instance { get; private set; }
        
        public RobotController HostRobot { private get; set; }
        public RobotController ClientRobot { private get; set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }
        
    }
}