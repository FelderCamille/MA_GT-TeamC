using System;
using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryLandmineIcon : InventoryIcon
    {
        
        [SerializeField] private Color selectedColor = new (58, 58, 58, 200); // gray with some opacity
        [SerializeField] private Color defaultColor = new (0, 0, 0, 0); // transparent

        public LandmineDifficulty Difficulty { get; private set; }
        private bool _isPressed;
        
        [SerializeField] private Button button;
        
        public void Init(Action action, string sprite, LandmineDifficulty difficulty, bool isDefault)
        {
            Difficulty = difficulty;
            // Choose color according to difficulty
            number.color = Constants.Landmines.LandmineDifficultyColor(difficulty);
            // Set color and number
            Init(sprite, 0, null);
            // Init button
            button.onClick.AddListener(() => action());
            if (isDefault) _isPressed = true;
            SetButtonColor();
        }
        
        public void SetNumber(int value)
        {
            number.text = value.ToString();
        }

        public void ToggleSelected()
        {
            _isPressed = !_isPressed;
            SetButtonColor(); // Change button color based on the selected state
        }

        private void SetButtonColor()
        {
            button.GetComponent<Image>().color = _isPressed ? selectedColor : defaultColor;
        }
    }
}