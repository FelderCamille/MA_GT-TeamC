using System.Collections.Generic;
using Controllers;
using UI;
using UnityEngine;

namespace Core
{
    public class ResourcesManager : MonoBehaviour
    {
        
        private Ressources _resourcesPrefab;
        private BonusRow _bonusRowPrefab;
        private FeedbackPopup _feedbackPopup;
        
        // References
        private SoundManager _soundManager;
        private GameOverController _gameOver;
        
        // Robot properties
        private int _money = Constants.GameSettings.Money;
        private int _clearedMines = 0;
        private int _explodedMines = 0;
        private float _health = Constants.GameSettings.Health;
        private float _visionDistance = Constants.GameSettings.Vision;
        private readonly List<Objects.Bonus> _appliedBonuses = new ();
        
        private void Start()
        {
            _soundManager = FindFirstObjectByType<SoundManager>();
            _resourcesPrefab = FindFirstObjectByType<Ressources>();
            _bonusRowPrefab = FindFirstObjectByType<BonusRow>();
            _gameOver = FindFirstObjectByType<GameOverController>(FindObjectsInactive.Include);
            _feedbackPopup = GetComponentInChildren<FeedbackPopup>(includeInactive: true);
            _resourcesPrefab.SetMoney(_money);
            _resourcesPrefab.SetHealth(_health);
            _resourcesPrefab.SetMines(_clearedMines);
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
        public int ClearedMines => _clearedMines;
        
        /// <summary>
        /// Increase cleared mines counter
        /// </summary>
        public void IncreaseClearedMinesCounter()
        {
            _clearedMines += 1;
            _resourcesPrefab.SetMines(_clearedMines);
            _feedbackPopup.ShowMineAsCleared();
        }
        
        public int ExplodedMines => _explodedMines;
        
        public void IncreaseExplodedMinesCount()
        {
            _explodedMines += 1;
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
                var bonuses = new List<Objects.Bonus>(_appliedBonuses); // Copy the list to avoid concurrent modification
                foreach (var bonus in bonuses)
                {
                    bonus.RemoveBonus(this);
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
        
        public bool HasBonus(Objects.Bonus bonus)
        {
            return _appliedBonuses.Contains(bonus);
        }

        public void AddBonus(Objects.Bonus bonus)
        {
            // Add the bonus to the list
            _appliedBonuses.Add(bonus);
            // Show the bonus on the UI
            _bonusRowPrefab.AddBonus(bonus);
        }
        
        public void RemoveBonus(Objects.Bonus bonus)
        {
            // Remove the bonus from the list
            _appliedBonuses.Remove(bonus);
            // Remove the bonus from the UI
            _bonusRowPrefab.RemoveBonus(bonus);
        }
        
        // Score
        
        public int ClearedMinesScore => _clearedMines * Constants.Score.ClearMineSuccess;

        public int ExplodedMinesScore => _explodedMines * Constants.Score.MineExplosion; // MineExplosion is negative
        
        public int TotalScore => ClearedMinesScore + ExplodedMinesScore;

    }

}