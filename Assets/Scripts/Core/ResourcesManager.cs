using UI;
using UnityEngine;

namespace Core
{
    public class ResourcesManager : MonoBehaviour
    {
        
        private Ressources _resourcesPrefab;
        
        // Robot properties
        private int _money = Constants.GameSettings.Money;
        private int _clearedMines = 0;
        private float _health = Constants.GameSettings.Health;
        
        private void Start()
        {
            _resourcesPrefab = FindObjectOfType<Ressources>();
            _resourcesPrefab.SetMoney(_money);
            _resourcesPrefab.SetHealth(_health);
            _resourcesPrefab.SetMines(_clearedMines);
        }

        /// <summary>
        /// Money of the robot
        /// </summary>
        public int Money => _money;
        
        /// <summary>
        /// Increase the money of the robot
        /// </summary>
        public void IncreaseMoney(int value)
        {
            _money += value;
            _resourcesPrefab.SetMoney(_money);
        }
        
        /// <summary>
        /// Reduce the money of the robot
        /// </summary>
        public void ReduceMoney(int value)
        {
            _money -= value;
            _resourcesPrefab.SetMoney(_money);
        }

        /// <summary>
        /// Cleared mines
        /// </summary>
        public int ClearedMines => _clearedMines;

        /// <summary>
        /// Increase cleared mines counter
        /// </summary>
        public void IncreaseClearedMinesCounter()
        {
            _clearedMines += 1;
            _resourcesPrefab.SetMines(_clearedMines);
        }

        /// <summary>
        /// Health of the robot
        /// </summary>
        public float Health => _health;

        /// <summary>
        /// Reduce the health of the robot
        /// </summary>
        public void ReduceHealth(float value)
        {
            _health -= value;
            _resourcesPrefab.SetHealth(_health);
        }
    }

}