using System.Collections.Generic;
using Objects;
using UnityEngine;

public static class Constants
{
    
    public static readonly bool DebugShowMines = true;
    
    public static class Scenes
    {
        public const string Title = "Title";
        public const string Game = "Game";
        public const string Result = "Result";
    }
    
    public static class GameSettings
    {
        // Context
        public const MapTheme GameMapTheme = MapTheme.Nature;
        // Grid
        public const int GridWidth = 50;
        public const int GridHeight = 20; // Should be odd
        public const int GridPadding = 20;
        // Timer
        public const float Timer = 10 * 60f; // 10 min
        // Landmines
        public const int NumberOfLandmines = 20;
        public const int NumberOfTileClearLandmine = 1;
        // Robot
        public const int NumberOfTileMovement = 1;
        public const float Health = 100f;
        public const int Vision = 0;
        public const int Money = 1000;
        // Tent
        public const int NumberOfTileOpenStore = 1;
    }

    public static class SpawnProbabilities
    {
        public const int Spruce = 10;
        public const int DeadSpruce = 10;
        public const int Tree = 5;
        public const int DeadTree = 2;
        public const int Log = 2;
        public const int Root = 1;
        public const int Rock = 10;
        public const int Bush = 5;
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

    public static class Bonus
    {
        public static string BonusTypeName(BonusType bonusType)
        {
            return bonusType switch
            {
                BonusType.Vision => "Vision",
                _ => "Unknown"
            };
        }
        
        public static List<Objects.Bonus> BonusesPerType(BonusType bonusType)
        {
            return bonusType switch
            {
                BonusType.Vision => new List<Objects.Bonus>{ new GlassesBonus() },
                _ => new List<Objects.Bonus>()
            };
        }
    }
}