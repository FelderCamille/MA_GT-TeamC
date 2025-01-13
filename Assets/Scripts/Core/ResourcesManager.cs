using System.Collections;
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
        private readonly NetworkVariable<ClearedMinesData> _clearedMines = new ( 
            new ClearedMinesData {
                clearedMinesEasy = 0,
                clearedMinesNormal = 0,
                clearedMinesHard = 0
             }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private readonly NetworkVariable<int> _explodedMines = new (0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private float _health = Constants.GameSettings.Health;
        private readonly Dictionary<LandmineDifficulty, int> _inventoryMines = new ()
        {
            { LandmineDifficulty.Easy, 0 },
            { LandmineDifficulty.Medium, 0 },
            { LandmineDifficulty.Hard, 0 }
        };
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
            _resourcesPrefab.SetMines(ClearedMines);
            // Add level zero bonuses
            foreach (var bonus in Constants.Bonus.BonusesAtStart())
            {
                AddBonus(bonus);
            }
            // Give some landmines to set
            StartCoroutine(ScheduleGivingMines());
        }
        
        /// <summary>
        /// Generic Coroutine to trigger a method at specified times.
        /// </summary>
        private IEnumerator ScheduleGivingMines()
        {
            var times = Constants.Landmines.GiveLandmineTimes;
            for (var i = 0; i < times.Length; i++)
            {
                yield return new WaitForSeconds(times[i] * 60f);
                var difficulty = Constants.Landmines.GiveLandmineDifficulties[i];
                Debug.Log("Giving " + difficulty + " landmine at " + times[i] + " minutes");;
                IncreaseInventoryMine(difficulty);
            }
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
        private int ClearedMines => _clearedMines.Value.clearedMinesEasy
                                   + _clearedMines.Value.clearedMinesNormal
                                   + _clearedMines.Value.clearedMinesHard;
        
        /// <summary>
        /// Cleared mines easy
        /// </summary>
        public int ClearedMinesEasy => _clearedMines.Value.clearedMinesEasy;
        
        /// <summary>
        /// Cleared mines normal (medium)
        /// </summary>
        public int ClearedMinesMedium => _clearedMines.Value.clearedMinesNormal;
        
        /// <summary>
        /// Cleared mines hard
        /// </summary>
        public int ClearedMinesHard => _clearedMines.Value.clearedMinesHard;
        
        /// <summary>
        /// Increase cleared mines counter
        /// </summary>
        public void IncreaseClearedMinesCounter(LandmineDifficulty difficulty)
        {
            // Increment cleared mines count according to difficulty
            var clearedMinesData = _clearedMines.Value;
            switch (difficulty)
            {
                case LandmineDifficulty.Easy:
                    clearedMinesData.clearedMinesEasy += 1;
                    break;
                case LandmineDifficulty.Medium:
                    clearedMinesData.clearedMinesNormal += 1;
                    break;
                case LandmineDifficulty.Hard:
                    clearedMinesData.clearedMinesHard += 1;
                    break;
            }
            _clearedMines.Value = clearedMinesData;
            // Update the UI
            _resourcesPrefab.SetMines(ClearedMines);
            // Show feedback
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
        
        public LandmineDifficulty SelectedLandmineDifficulty => _inventoryRowPrefab.SelectedLandmineDifficulty;

        public bool CanPlaceMineOfSelectedDifficulty()
        {
            return _inventoryMines[SelectedLandmineDifficulty] > 0;
        }
        
        public void DecreaseInventoryMineOfSelectedDifficulty()
        {
            DecreaseInventoryMine(SelectedLandmineDifficulty);
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
                // Remove bonuses
                var bonuses = new List<Bonus>(_appliedBonuses); // Copy the list to avoid concurrent modification
                foreach (var bonus in bonuses)
                {
                    bonus.RemoveBonus(this);
                }
                // Remove landmines
                foreach (var difficulty in _inventoryMines.Keys.ToList())
                {
                    for (var i = 0; i < _inventoryMines[difficulty]; i++)
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
            else _health = Constants.Damages.SmallRepair;
            // Update the health on the UI
            _resourcesPrefab.SetHealth(_health);
            // Play the repair sound
            _soundManager.PlayRepairSound();
        }
        
        // Bonus

        public BonusLevel? GetBonusLevel(BonusName bonusName) =>
            _appliedBonuses.FirstOrDefault(b => b.BonusName == bonusName)?.CurrentLevel;
        
        public void SetVision(bool playSound)
        {
            if (playSound) _soundManager.PlayVisionSound();
            else _soundManager.StopVisionSound();
        }
        
        public bool HasBonus(Bonus bonus)
        {
            var foundBonus = _appliedBonuses.FirstOrDefault(b => bonus.BonusName == b.BonusName);
            var bonusValue = _inventoryRowPrefab.GetBonusValue(bonus);
            return foundBonus != null && bonusValue == bonus.Values[bonus.CurrentLevel].Value;
        }

        public void AddBonus(Bonus bonus)
        {
            // Upgrade old bonus if already applied
            var oldBonus = _appliedBonuses.FirstOrDefault(b => b.BonusName == bonus.BonusName);
            if (oldBonus != null)
            {
                UpdateBonus(bonus);
            }
            else
            {
                // Add the bonus to the list
                _appliedBonuses.Add(bonus);
                // Show the bonus on the UI
                if (bonus.CurrentLevel != BonusLevel.Zero) _inventoryRowPrefab.AddBonus(bonus);
            }
        }

        private void UpdateBonus(Bonus bonus)
        {
            // Remove the bonus from the list
            var bonusIndex = _appliedBonuses.FindIndex(b => b.BonusName == bonus.BonusName);
            var oldBonus = _appliedBonuses[bonusIndex];
            _appliedBonuses.RemoveAt(bonusIndex);
            _appliedBonuses.Insert(bonusIndex, bonus);
            // Remove the bonus from the UI
            if (oldBonus.CurrentLevel != BonusLevel.Zero) _inventoryRowPrefab.UpdateBonus(bonus);
            else _inventoryRowPrefab.AddBonus(bonus);
        }
        
        public void RemoveBonus(Bonus bonus)
        {
            // Remove the bonus from the list
            var bonusIndex = _appliedBonuses.FindIndex(b => b.BonusName == bonus.BonusName);
            _appliedBonuses.RemoveAt(bonusIndex);
            // Remove the bonus from the UI
            _inventoryRowPrefab.RemoveBonus(bonus);
        }
        
    }

}