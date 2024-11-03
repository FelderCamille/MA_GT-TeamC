using UnityEngine;

public static class Constants
{
    public static class Scenes
    {
        public const string Title = "Title";
        public const string Game = "Game";
    }

    public static class GameSize
    {
        public const int GridWidth = 30;
        public const int GridHeight = 30;
        public const int Padding = 20;
        // Width and height of the map
        public const int MapWidth = GridWidth + Padding * 2;
        public const int MapHeight =  GridHeight + Padding * 2;
        // Padding indexes
        public const int GridXYStartIndex = Padding;
        public const int GridXEndIndex = Padding + GridWidth;
        public const int GridYEndIndex = Padding + GridHeight;
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
        public const int NumberOfLandmines = 20;
        public const float HealthRemovedWhenExplosion = 10f;
    } 
}