using UnityEngine;

namespace DefaultNamespace
{
    public static class Globals
    {
        public static class Scenes
        {
            public const string Title = "Title";
            public const string Game = "Game";
        }

        public static class GameSize
        {
            public const int Width = 50;
            public const int Height = 50;
        }

        public static class Actions
        {
            public const KeyCode MoveLeft = KeyCode.LeftArrow;
            public const KeyCode MoveRight = KeyCode.RightArrow;
            public const KeyCode MoveUp = KeyCode.UpArrow;
            public const KeyCode MoveDown = KeyCode.DownArrow;
            public const KeyCode ClearMine = KeyCode.D;
        }

        public static class Values
        {
            public const float HealthRemovedWhenExplosion = 10f;
        } 
    }
}