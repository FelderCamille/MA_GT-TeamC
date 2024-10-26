using System;
using DefaultNamespace;
using UI;
using UnityEngine;

namespace Controllers
{
    public class RobotController : MonoBehaviour
    {
        
        [Header("Content")]
        public Ressources resourcesPrefab;
    
        [Header("Settings")]
        public int numberOfCase = 1;
        
        // Robot properties
        private int _money = 1000;
        private int _clearedMines = 0;
        private float _health = 100f;

        private void Start()
        {
            resourcesPrefab.SetMoney(_money);
            resourcesPrefab.SetHealth(_health);
            resourcesPrefab.SetMines(_clearedMines);
        }

        private void Update()
        {
            HandleMovements();
        }

        private void HandleMovements()
        {
            // Move to right
            if (Input.GetKeyDown(Globals.Actions.MoveRight))
            {
                transform.position += new Vector3(numberOfCase, 0f, 0f);
            }
            // Move to left
            if (Input.GetKeyDown(Globals.Actions.MoveLeft))
            {
                transform.position -= new Vector3(numberOfCase, 0f, 0f);
            }
            // Move up
            if (Input.GetKeyDown(Globals.Actions.MoveUp))
            {
                transform.position += new Vector3(0f, 0f, numberOfCase);
            }
            // Move down
            if (Input.GetKeyDown(Globals.Actions.MoveDown))
            {
                transform.position -= new Vector3(0f, 0f, numberOfCase);
            }
        }

        /**
         * Clear mine
         */
        public void ClearMine(bool success, GameObject mine)
        {
            if (success)
            {
                _clearedMines += 1;
                resourcesPrefab.SetMines(_clearedMines);
            }
            else
            {
                _health -= Globals.Values.HealthRemovedWhenExplosion;
                resourcesPrefab.SetHealth(_health);
            }
            mine.SetActive(false);
        }

        /**
         * Explode mine when entering in collision with it
         */
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<MineController>())
            {
                ClearMine(false, other.gameObject);
            }
        }
    }

}