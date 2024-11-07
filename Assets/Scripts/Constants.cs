using UnityEngine;

public static class Constants
{
    public static class Scenes
    {
        public const string Title = "Title";
        public const string Game = "Game";
        public const string Result = "Result";
    }

    public static class GameSettings
    {
        // Grid
        public const int GridWidth = 30;
        public const int GridHeight = 30;
        public const int GridPadding = 20;
        // Timer
        public const float Timer = 10 * 60f; // 10 min
        // Landmines
        public const int NumberOfLandmines = 20;
        public const float Health = 100f;
        public const int Money = 1000;
    }

    public static class Actions
    {
        public const KeyCode MoveLeft = KeyCode.LeftArrow;
        public const KeyCode MoveRight = KeyCode.RightArrow;
        public const KeyCode MoveUp = KeyCode.UpArrow;
        public const KeyCode MoveDown = KeyCode.DownArrow;
        public const KeyCode ClearMine = KeyCode.D;
        public const KeyCode OpenCloseEncyclopedia = KeyCode.E;
    }

    public static class Values
    {
        public const float HealthRemovedWhenFailure = 10f;
        public const float HealthRemovedWhenExplosion = 20f;
    } 
}