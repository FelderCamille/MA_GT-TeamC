using System;
using System.Collections.Generic;
using System.Linq;
using Objects;
using UnityEngine;

public static class Constants
{
    
    // Warning, the following constants could break some part of the game if set to "true"
    public static readonly bool DebugShowMines = false;
    public static readonly bool DebugAllowOnlyOneConnection = true;
    public static readonly bool DebugFillIPAddressOnClient = false;
    public static readonly bool DebugShowOtherPlayer = false;
    
    public static class Scenes
    {
        public const string Title = "Title";
        public const string Settings = "Settings";
        public const string Base = "Base";
        public const string Game = "Game";
        public const string Result = "Result";
    }
    
    public static class GameSettings
    {
        // Grid
        public const int GridWidth = 50; // Should be even, so each player has the same number of tiles
        public const int GridHeight = 20;
        public const int GridPadding = 30;
        // Timer
        public const float Timer = 10f * 60f; // 10 min
        // Landmines
        public const int NumberOfLandminesPerSide = 5;
        public const int NumberOfTileClearLandmine = 1;
        public const int SafeAreaWidth = 2;
        // Robot
        public const float Health = 100f;
        public const int MinMoney = 1000;
        public const int MaxMoney = 10000;
        public const int DefaultMoney = 5000;
        // Tent
        public const int NumberOfTileOpenStore = 1;
        // Player
        public const string DefaultPlayer1Name = "Joueur 1";
        public const string DefaultPlayer2Name = "Joueur 2";
    }

    public static class PaddingSpawnProbabilities
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
    
    public static class GridSpawnProbabilities
    {
        public const int Spruce = 0; // We don't want to spawn spruce on the grid
        public const int DeadSpruce = 0; // We don't want to spawn dead spruce on the grid
        public const int Tree = 0; // We don't want to spawn tree on the grid
        public const int DeadTree = 0; // We don't want to spawn dead tree on the grid
        public const int Log = 0; // We don't want to spawn log on the grid
        public const int Root = 5;
        public const int Rock = 5;
        public const int Bush = 5;
    }

    public static class Actions
    {
        public const KeyCode MoveLeft = KeyCode.LeftArrow;
        public const KeyCode MoveRight = KeyCode.RightArrow;
        public const KeyCode MoveUp = KeyCode.UpArrow;
        public const KeyCode MoveDown = KeyCode.DownArrow;
        public const KeyCode ClearMine = KeyCode.D;
        public const KeyCode PlaceMine = KeyCode.M;
        public const KeyCode OpenCloseEncyclopedia = KeyCode.E;
    }

    public static class Prices
    {
        // Health
        public const int Revive = 1500;
        public const int Repair = 500;
        // Rewards
        public const int ClearMineSuccess = 200;
    }

    public static class Damages
    {
        // Explosion
        public const int RemovedWhenFailureEasy = 20;
        public const int RemovedWhenFailureMedium = 40;
        public const int RemovedWhenFailureHard = 60;
        public const int RemovedWhenExplosion = 30;
        // Repair
        public const int SmallRepair = 25;
    }
    
    public static class Score
    {
        // Clear mine
        public const int ClearMineEasySuccess = 100;
        public const int ClearMineMediumSuccess = 250;
        public const int ClearMineHardSuccess = 400;
        public const int MineExplosion = -200;
        // Final score
        public const int MineNotCleared = -100;
        public const int MinePlaced = 50;
    }

    public static class Landmines
    {
        public static readonly float[] GiveLandmineTimes = { 0f, 2f, 4f, 6f, 8f };
        
        public static readonly LandmineDifficulty[] GiveLandmineDifficulties =
        {
            LandmineDifficulty.Easy, // 1
            LandmineDifficulty.Medium, // 2
            LandmineDifficulty.Medium, // 4
            LandmineDifficulty.Medium, // 6
            LandmineDifficulty.Hard, // 8
        };
        
        public static string LandmineDifficultyName(LandmineDifficulty difficulty)
        {
            return difficulty switch
            {
                LandmineDifficulty.Easy => "Facile",
                LandmineDifficulty.Medium => "Moyen",
                LandmineDifficulty.Hard => "Difficile",
                _ => "Unknown"
            };
        }
        
        public static int LandmineDifficultyToNumber(LandmineDifficulty difficulty)
        {
            return difficulty switch
            {
                LandmineDifficulty.Easy => 0,
                LandmineDifficulty.Medium => 1,
                LandmineDifficulty.Hard => 2,
                _ => 0
            };
        }
        
        public static LandmineDifficulty NumberToLandmineDifficulty(int difficulty)
        {
            return difficulty switch
            {
                0 => LandmineDifficulty.Easy,
                1 => LandmineDifficulty.Medium,
                2 => LandmineDifficulty.Hard,
                _ => throw new Exception("Unknown difficulty")
            };
        }
        
        public static Color LandmineDifficultyColor(LandmineDifficulty difficulty)
        {
            return difficulty switch
            {
                LandmineDifficulty.Easy => Color.green,
                LandmineDifficulty.Medium => Color.yellow,
                LandmineDifficulty.Hard => Color.red,
                _ => Color.white
            };
        }
        
        public static List<Landmine> LandminesObjects()
        {
            return new List<Landmine>
            {
                new()
                {
                    Name = LandmineDifficultyName(LandmineDifficulty.Easy),
                    Price = 100,
                    Icon = "Icons/bomb",
                    Difficulty = LandmineDifficulty.Easy
                },
                new()
                {
                    Name = LandmineDifficultyName(LandmineDifficulty.Medium),
                    Price = 200,
                    Icon = "Icons/bomb",
                    Difficulty = LandmineDifficulty.Medium
                },
                new()
                {
                    Name = LandmineDifficultyName(LandmineDifficulty.Hard),
                    Price = 300,
                    Icon = "Icons/bomb",
                    Difficulty = LandmineDifficulty.Hard
                }
            };
        }
    }

    public static class Bonus
    {
        
        public static string BonusTypeName(BonusType bonusType)
        {
            return bonusType switch
            {
                BonusType.Vision => "Bonus visuel",
                BonusType.Speed => "Bonus de vitesse",
                _ => "Unknown"
            };
        }
        
        public static List<Objects.Bonus> BonusesPerType(BonusType bonusType)
        {
            return bonusType switch
            {
                BonusType.Vision => new List<Objects.Bonus>{ new DetectionBonus() },
                BonusType.Speed => new List<Objects.Bonus>{ new SpeedBonus() },
                _ => new List<Objects.Bonus>()
            };
        }

        public static List<Objects.Bonus> BonusesAtStart()
        {
            var visionBonus = BonusesPerType(BonusType.Vision).First();
            var speedBonus = BonusesPerType(BonusType.Speed).First();
            return new List<Objects.Bonus>
            {
                visionBonus,
                speedBonus
            };
        }
        
        public static float RepeatedWaveEffectStartSize(BonusLevel level)
        {
            return level switch
            {
                BonusLevel.Zero => 3, // One case
                BonusLevel.One => 5, // Two cases
                BonusLevel.Two => 7, // Three cases
                BonusLevel.Three => 9, // Four cases
                BonusLevel.Four => 11.2f, // Five cases
                BonusLevel.Five => 16, // Seven cases
                _ => throw new Exception("Unknown level")
            };
        }
        
        public static BonusLevel? NextBonusLevel(BonusLevel level)
        {
            return level switch
            {
                BonusLevel.Zero => BonusLevel.One,
                BonusLevel.One => BonusLevel.Two,
                BonusLevel.Two => BonusLevel.Three,
                BonusLevel.Three => BonusLevel.Four,
                BonusLevel.Four => BonusLevel.Five,
                _ => null
            };
        }
    }
}