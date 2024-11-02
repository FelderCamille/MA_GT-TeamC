using UI;
using UnityEngine;

namespace Controllers
{

    public class GridController : MonoBehaviour
    {
        
        private static readonly System.Random Random = new ();
        
        [Header("Content")]
        public Tile tilePrefab;
        public Tile landmineTilePrefab;

        [Header("Settings")]
        public int numberOfLandmines;

        /// <summary>
        /// The landmines emplacement. The index is the position in the grid, the value is whether or not it has a landmine
        /// </summary>
        private bool[] _landmines;
        
        private void Start()
        {
            ComputeLandminesEmplacement();
            GenerateGrid();
        }

        private void ComputeLandminesEmplacement()
        {
            // Initialize the array
            _landmines = new bool[Constants.GameSize.Width * Constants.GameSize.Height];
            // Compute the emplacements
            for (var i = 0; i < numberOfLandmines; i++)
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
        
        private void GenerateGrid()
        {
            for (var x = 0; x < Constants.GameSize.Width; x++)
            {
                for (var y = 0; y < Constants.GameSize.Height; y++)
                {
                    // Check if the emplacement must be a classic tile or a landmine
                    var index = x * Constants.GameSize.Height + y;
                    var prefab = _landmines[index] ? landmineTilePrefab : tilePrefab;
                    // Generate the tile
                    var tileObj = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity);
                    tileObj.transform.SetParent(transform, false);
                    tileObj.name = $"Tile {x} {y}" + (_landmines[index] ? " x" : "");
                }
            }
        }
        
    }
}