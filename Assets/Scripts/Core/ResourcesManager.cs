using System.Collections.Generic;
using System.Linq;
using Controllers;
using Objects;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class ResourcesManager : NetworkBehaviour
    {
        
        private Ressources _resourcesPrefab;
        private InventoryRow _inventoryRowPrefab;
        private FeedbackPopup _feedbackPopup;
        
        // References
        private SoundManager _soundManager;
        private GameOverController _gameOver;
        
        // Robot properties
        private int _money = Constants.GameSettings.Money;
        private readonly NetworkVariable<int> _clearedMines = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private readonly NetworkVariable<int> _explodedMines = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private float _health = Constants.GameSettings.Health;
        private readonly Dictionary<LandmineDifficulty, int> _inventoryMines = new ()
        {
            { LandmineDifficulty.Easy, 0 },
            { LandmineDifficulty.Medium, 0 },
            { LandmineDifficulty.Hard, 0 }
        };
        private float _visionDistance = Constants.GameSettings.Vision;
        private readonly List<Bonus> _appliedBonuses = new ();
        private readonly Dictionary<LandmineDifficulty, InventoryLandmineIcon> _landminesInventoryIcons = new ();
        
        private void Start()
        {
            // Get references
            _soundManager = FindFirstObjectByType<SoundManager>();
            _resourcesPrefab = FindFirstObjectByType<Ressources>();
            _feedbackPopup = GetComponentInChildren<FeedbackPopup>(includeInactive: true);
            _gameOver = FindFirstObjectByType<GameOverController>(FindObjectsInactive.Include);
            // Get the inventory elements
            _inventoryRowPrefab = FindFirstObjectByType<InventoryRow>();
            var landmineIcons = _inventoryRowPrefab.GetComponentsInChildren<InventoryLandmineIcon>();
            foreach (var button in landmineIcons) _landminesInventoryIcons.Add(button.Difficulty, button);
            // Initialize the resources on the UI
            _resourcesPrefab.SetMoney(_money);
            _resourcesPrefab.SetHealth(_health);
            _resourcesPrefab.SetMines(_clearedMines.Value);
        }
        
        // Money
        
        /// <summary>
        /// Check if their is enough money to buy something
        /// </summary>
        /// <param name="value">The value of the thing to buy</param>
        /// <returns>True if their is enough money, false otherwise</returns>
        public bool HasEnoughMoneyToBuy(int value)
        {
            return _money >= value;
        }
        
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
            _soundManager.PlayBuySound();
            _money -= value;
            _resourcesPrefab.SetMoney(_money);
        }
        
        // Mines

        /// <summary>
        /// Cleared mines
        /// </summary>
        public int ClearedMines => _clearedMines.Value;
        
        /// <summary>
        /// Increase cleared mines counter
        /// </summary>
        public void IncreaseClearedMinesCounter()
        {
            _clearedMines.Value += 1;
            _resourcesPrefab.SetMines(_clearedMines.Value);
            _feedbackPopup.ShowMineAsCleared();
        }
        
        public int ExplodedMines => _explodedMines.Value;
        
        public void IncreaseExplodedMinesCount()
        {
            _explodedMines.Value += 1;
        }
        
        public void IncreaseInventoryMine(LandmineDifficulty difficulty)
        {
            _inventoryMines[difficulty] += 1;
            if (!_landminesInventoryIcons.ContainsKey(difficulty)) return;
            _landminesInventoryIcons[difficulty].SetNumber(_inventoryMines[difficulty]);
        }

        public bool CanPlaceMineOfSelectedDifficulty()
        {
            return _inventoryMines[_inventoryRowPrefab.SelectedLandmineDifficulty] > 0;
        }
        
        public void DecreaseInventoryMineOfSelectedDifficulty()
        {
            var difficulty = _inventoryRowPrefab.SelectedLandmineDifficulty;
            DecreaseInventoryMine(difficulty);
        }

        private void DecreaseInventoryMine(LandmineDifficulty difficulty)
        {
            if (_inventoryMines[difficulty] == 0) return;
            _inventoryMines[difficulty] -= 1;
            if (!_landminesInventoryIcons.ContainsKey(difficulty)) return;
            _landminesInventoryIcons[difficulty].SetNumber(_inventoryMines[difficulty]);
        }
        
        // Health

        /// <summary>
        /// Reduce the health of the robot
        /// </summary>
        public void ReduceHealth(float value)
        {
            // Reduce the health
            if (_health - value < 0) value = _health;
            _health -= value;
            // Update the health on the UI
            _resourcesPrefab.SetHealth(_health);
            _feedbackPopup.ShowHealthLost(value);
            // Manage game over
            if (_health <= 0)
            {
                var bonuses = new List<Bonus>(_appliedBonuses); // Copy the list to avoid concurrent modification
                foreach (var bonus in bonuses)
                {
                    bonus.RemoveBonus(this);
                }
                foreach (var difficulty in _inventoryMines.Keys.ToList())
                {
                    DecreaseInventoryMine(difficulty);
                }
                _gameOver.Show(this);
            }
        }

        /// <summary>
        /// Check whether it needs to be repaired
        /// </summary>
        /// <returns>True if the health is not at maximum, false otherwise</returns>
        public bool NeedRepair()
        {
            return _health < Constants.GameSettings.Health;
        }

        /// <summary>
        /// Increase the health of the robot to the maximum
        /// </summary>
        public void Repair(bool partial = false)
        {
            // If the health was at 0, hide the game over screen
            if (_health <= 0) _gameOver.Hide(this);
            // If total repair, set the health to the maximum. If partial, add a small value
            if (!partial)  _health = Constants.GameSettings.Health;
            else _health = Constants.Health.SmallRepair;
            // Update the health on the UI
            _resourcesPrefab.SetHealth(_health);
            // Play the repair sound
            _soundManager.PlayRepairSound();
        }
        
        // Bonus
        
        public float GetVisionDistance()
        {
            _soundManager.PlayVisionSound();
            return _visionDistance;
        }
        
        public void SetVision(double multiplier)
        {
            _soundManager.PlayVisionSound();
            _visionDistance = (float) multiplier;
        }
        
        public bool HasBonus(Bonus bonus)
        {
            return _appliedBonuses.Contains(bonus);
        }

        public void AddBonus(Bonus bonus)
        {
            // Add the bonus to the list
            _appliedBonuses.Add(bonus);
            // Show the bonus on the UI
            _inventoryRowPrefab.AddBonus(bonus);
        }
        
        public void RemoveBonus(Bonus bonus)
        {
            // Remove the bonus from the list
            _appliedBonuses.Remove(bonus);
            // Remove the bonus from the UI
            _inventoryRowPrefab.RemoveBonus(bonus);
        }

    }

}