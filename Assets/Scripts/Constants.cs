using System.Collections.Generic;
using Objects;
using UnityEngine;

public static class Constants
{
    
    public static readonly bool DebugShowMines = true;
    public static readonly bool DebugAllowOnlyOneConnection = false;
    public static readonly bool DebugFillIPAddressOnClient = true;
    
    public static class Scenes
    {
        public const string Title = "Title";
        public const string Base = "Base";
        public const string Game = "Game";
        public const string Result = "Result";
    }
    
    public static class GameSettings
    {
        // Context
        public const MapTheme GameMapTheme = MapTheme.Nature;
        // Grid
        public const int GridWidth = 10;
        public const int GridHeight = 10; // Should be odd
        public const int GridPadding = 10;
        // Timer
        public const float Timer = 10 * 60f; // 10 min
        // Landmines
        public const int NumberOfLandmines = 5;
        public const int NumberOfTileClearLandmine = 1;
        // Robot
        public const int NumberOfTileMovement = 1;
        public const float Health = 100f;
        public const int Vision = 0;
        public const int Money = 5000;
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
        public const KeyCode ClearMine = KeyCode.D;
        public const KeyCode OpenCloseEncyclopedia = KeyCode.E;
    }

    public static class Prices
    {
        public const int Revive = 1500;
        public const int Repair = 500;
        public const int Vision = 100;
    }

    public static class Health
    {
        public const int RemovedWhenFailureMin = 10;
        public const int RemovedWhenFailureMax = 15;
        public const int RemovedWhenExplosionMin = 20;
        public const int RemovedWhenExplosionMax = 30;
        public const int SmallRepair = 25;
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