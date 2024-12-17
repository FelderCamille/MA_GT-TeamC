using Objects;
using UnityEngine;

namespace UI
{
    public class InventoryLandmineIcon : InventoryIcon
    {
        public LandmineDifficulty Difficulty { get; private set; }
        
        public void Init(string sprite, LandmineDifficulty difficulty)
        {
            Difficulty = difficulty;
            // Choose color according to difficulty
            var color = Color.green;
            if (difficulty == LandmineDifficulty.Medium) color = Color.yellow;
            else if (difficulty == LandmineDifficulty.Hard) color = Color.red;
            number.color = color;
            // Set color and number
            InitWithNumber(sprite, null);
        }
        
        public void SetNumber(int value)
        {
            number.text = value.ToString();
        }
    }
}