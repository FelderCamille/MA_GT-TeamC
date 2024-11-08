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
        public const int NumberOfTileClearLandmine = 1;
        // Robot
        public const int NumberOfTileMovement = 1;
        public const float Health = 100f;
        public const int Money = 1000;
        // Tent
        public const int NumberOfTileOpenStore = 1;
    }

    public static class Actions
    {
        public const KeyCode MoveLeft = KeyCode.LeftArrow;
        public const KeyCode MoveRight = KeyCode.RightArrow;
        public const KeyCode MoveUp = KeyCode.UpArrow;
        public const KeyCode MoveDown = KeyCode.DownArrow;
        public const KeyCode Rotation = KeyCode.LeftShift;
        public const KeyCode ClearMine = KeyCode.D;
        public const KeyCode OpenCloseEncyclopedia = KeyCode.E;
    }

    public static class Values
    {
        public const int RepairPrice = 500;
        public const int HealthRemovedWhenFailureMin = 10;
        public const int HealthRemovedWhenFailureMax = 15;
        public const int HealthRemovedWhenExplosionMin = 20;
        public const int HealthRemovedWhenExplosionMax = 30;
    } 
}