using Objects;
using UI;
using UnityEngine;

namespace Controllers
{

    public class GridController : MonoBehaviour, IGrid
    {
        // Padding indexes
        private const int GridXYStartIndex = Constants.GameSettings.GridPadding;
        private const int GridXEndIndex = Constants.GameSettings.GridPadding + Constants.GameSettings.GridWidth;
        private const int GridYEndIndex = Constants.GameSettings.GridPadding + Constants.GameSettings.GridHeight;
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
            _landmines = new bool[Constants.GameSettings.GridWidth * Constants.GameSettings.GridHeight];
            // Compute the emplacements
            for (var i = 0; i < Constants.GameSettings.NumberOfLandmines; i++)
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
            const int mapWidth = Constants.GameSettings.GridWidth + Constants.GameSettings.GridPadding * 2;
            const int mapHeight =  Constants.GameSettings.GridHeight + Constants.GameSettings.GridPadding * 2;
            // Generate the map
            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    // Check if it's the emplacement of a grid tile
                    if (x is >= GridXYStartIndex and < GridXEndIndex 
                        && y is >= GridXYStartIndex and < GridYEndIndex)
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
            var index = (x - Constants.GameSettings.GridPadding) * Constants.GameSettings.GridHeight + (y - Constants.GameSettings.GridPadding);
            var prefab = _landmines[index] ? landmineTilePrefab : tilePrefab;
            // Generate the tile
            var tileObj = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity);
            tileObj.transform.SetParent(transform, false);
            tileObj.name = $"Tile {x} {y}" + (_landmines[index] ? " x" : "");
        }

        public bool CanMoveRight(float newX) => CanGoToNewX(newX);

        public bool CanMoveLeft(float newX) => CanGoToNewX(newX);
        
        private static bool CanGoToNewX(float newX) => newX is >= GridXYStartIndex and < GridXEndIndex;
        
        public bool CanMoveUp(float newY) => CanGoToNewY(newY);

        public bool CanMoveDown(float newY) => CanGoToNewY(newY);
        
        private static bool CanGoToNewY(float newY) => newY is >= GridXYStartIndex and < GridYEndIndex;
    }
}