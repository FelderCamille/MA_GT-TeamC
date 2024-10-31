using UI;
using UnityEngine;

namespace Core
{
    public class ResourcesManager : MonoBehaviour
    {
        
        private Ressources _resourcesPrefab;
        
        // Robot properties
        private int _money = 1000;
        private int _clearedMines = 0;
        private float _health = 100f;

        /// <summary>
        /// Money of the robot
        /// </summary>
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                _resourcesPrefab.SetMoney(_money);
            }
        }
        
        /// <summary>
        /// Cleared landmines of the robot
        /// </summary>
        public int ClearedLandmines
        {
            get => _clearedMines;
            set
            {
                _clearedMines = value;
                _resourcesPrefab.SetMoney(_clearedMines);
            }
        }

        /// <summary>
        /// Health of the robot
        /// </summary>
        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                _resourcesPrefab.SetHealth(_health);
            }
        }

        private void Start()
        {
            _resourcesPrefab = FindObjectOfType<Ressources>();
            _resourcesPrefab.SetMoney(_money);
            _resourcesPrefab.SetHealth(_health);
            _resourcesPrefab.SetMines(_clearedMines);
        }
    }

}