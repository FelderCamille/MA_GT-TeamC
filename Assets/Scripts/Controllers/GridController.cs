using Objects;
using UI;
using UnityEngine;

namespace Controllers
{

    public class GridController : MonoBehaviour, IGrid
    {
        
        private static readonly System.Random Random = new ();
        
        [Header("Content")]
        public Tile tilePrefab;
        public Tile landmineTilePrefab;
        public Tile paddingTile;

        /// <summary>
        /// The landmines emplacement. The index is the position in the grid, the value is whether or not it has a landmine
        /// </summary>
        private bool[] _landmines;
        
        private void Start()
        {
            ComputeLandminesEmplacement();
            GenerateMap();
        }

        private void ComputeLandminesEmplacement()
        {
            // Initialize the array
            _landmines = new bool[Constants.GameSize.GridWidth * Constants.GameSize.GridHeight];
            // Compute the emplacements
            for (var i = 0; i < Constants.Values.NumberOfLandmines; i++)
            {
                // Get an index
                var landmineIndex = Random.Next(0, _landmines.Length); // [0, _landmines.Length[
                // If their is already a landmine, look for another index
                while (_landmines[landmineIndex])
                {
                    landmineIndex = Random.Next(0, _landmines.Length); // [0, _landmines.Length[
                }
                // Set a landmine at this emplacement
                _landmines[landmineIndex] = true;
            }
        }

        private void GenerateMap()
        {
            // Generate the map
            for (var x = 0; x < Constants.GameSize.MapWidth; x++)
            {
                for (var y = 0; y < Constants.GameSize.MapHeight; y++)
                {
                    // Check if it's the emplacement of a grid tile
                    if (x is >= Constants.GameSize.GridXYStartIndex and < Constants.GameSize.GridXEndIndex 
                        && y is >= Constants.GameSize.GridXYStartIndex and < Constants.GameSize.GridYEndIndex)
                    {
                        GenerateGridTile(x, y);
                    }
                    else
                    {
                        GeneratePaddingTile(x, y);
                    }
                }
            }
        }

        private void GeneratePaddingTile(int x, int y)
        {
            var tileObj = Instantiate(paddingTile, new Vector3(x, 0, y), Quaternion.identity);
            tileObj.transform.SetParent(transform, false);
            tileObj.name = $"Tile {x} {y} padding";
        }

        private void GenerateGridTile(int x, int y)
        {
            // Check if the emplacement must be a classic tile or a landmine
            var index = (x - Constants.GameSize.Padding) * Constants.GameSize.GridHeight + (y - Constants.GameSize.Padding);
            var prefab = _landmines[index] ? landmineTilePrefab : tilePrefab;
            // Generate the tile
            var tileObj = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity);
            tileObj.transform.SetParent(transform, false);
            tileObj.name = $"Tile {x} {y}" + (_landmines[index] ? " x" : "");
        }

        public bool CanMoveRight(float newX) => CanGoToNewX(newX);

        public bool CanMoveLeft(float newX) => CanGoToNewX(newX);
        
        private static bool CanGoToNewX(float newX) =>
            newX is >= Constants.GameSize.GridXYStartIndex and < Constants.GameSize.GridXEndIndex;
        
        public bool CanMoveUp(float newY) => CanGoToNewY(newY);

        public bool CanMoveDown(float newY) => CanGoToNewY(newY);
        
        private static bool CanGoToNewY(float newY) =>
            newY is >= Constants.GameSize.GridXYStartIndex and < Constants.GameSize.GridYEndIndex;
    }
}