using System.Collections.Generic;
using System.Linq;
using Objects;
using UI;
using UnityEngine;

namespace Controllers
{

    public class GridController : MonoBehaviour, IGrid
    {
        private const int GridXYStartIndex = Constants.GameSettings.GridPadding;
        private const int GridXEndIndex = Constants.GameSettings.GridPadding + Constants.GameSettings.GridWidth;
        private const int GridYEndIndex = Constants.GameSettings.GridPadding + Constants.GameSettings.GridHeight;
        private static readonly System.Random Random = new ();
        private Dictionary<DecorTileType, Tile[]> _decorTiles = new ();
        
        [Header("Grid tiles")]
        public Tile tilePrefab;
        public LandmineTile landmineTilePrefab;
        
        [Header("Padding tiles")]
        public Tile paddingTilePrefab;
        public Tile treeTilePrefab;
        public Tile spruceTilePrefab;
        public Tile[] rockTilePrefabs;
        public Tile bushTilePrefab;
        public Tile logTilePrefab;
        public Tile rootTilePrefab;
        public Tile deadTreeTilePrefab;
        public Tile deadSpruceTilePrefab;
        
        [Header("Content")]
        public RobotController robotPrefab;
        public TentController tentPrefab;

        /// <summary>
        /// The landmines emplacement. The index is the position in the grid, the value is whether or not it has a landmine
        /// </summary>
        private bool[] _landmines;
        
        private void Awake()
        {
            ComputeLandminesEmplacement();
            ChooseDecorPrefabs();
            GenerateMap();
            SpawnRobot();
            SpawnTent();
        }

        private void ComputeLandminesEmplacement()
        {
            // Initialize the array
            _landmines = new bool[Constants.GameSettings.GridWidth * Constants.GameSettings.GridHeight];
            // Compute the emplacements
            for (var i = 0; i < Constants.GameSettings.NumberOfLandmines; i++)
            {
                // Get an index
                var landmineIndex = Random.Next(0, _landmines.Length);
                // If their is already a landmine, look for another index
                while (_landmines[landmineIndex])
                {
                    landmineIndex = Random.Next(0, _landmines.Length);
                }
                // Set a landmine at this emplacement
                _landmines[landmineIndex] = true;
            }
        }
        
        private void ChooseDecorPrefabs()
        {
            // Add tiles to the dictionary according to the theme
            switch (Constants.GameSettings.GameMapTheme)
            {
                case MapTheme.Nature:
                    _decorTiles.Add(DecorTileType.Tree, new []{treeTilePrefab});
                    _decorTiles.Add(DecorTileType.Spruce, new []{spruceTilePrefab});
                    _decorTiles.Add(DecorTileType.Bush, new []{bushTilePrefab});
                    break;
                case MapTheme.War:
                    _decorTiles.Add(DecorTileType.DeadTree, new []{deadTreeTilePrefab});
                    _decorTiles.Add(DecorTileType.DeadSpruce, new []{deadSpruceTilePrefab});
                    _decorTiles.Add(DecorTileType.Root, new []{rootTilePrefab});
                    break;
                default:
                    break;
            }
            // Add common tiles to the dictionary
            _decorTiles.Add(DecorTileType.Rock, rockTilePrefabs);
            _decorTiles.Add(DecorTileType.Log, new []{logTilePrefab});
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
            // Randomly pick a tile type
            var tileTypeIndex = Random.Next(0, _decorTiles.Keys.Count);
            var decorTileType = _decorTiles.Keys.ElementAt(tileTypeIndex);
            // Compute the probability of spawn
            var spawnProbability = Random.Next(0, 100);
            var objectSpawnProbability = decorTileType switch
            {
                DecorTileType.Tree => Constants.SpawnProbabilities.Tree,
                DecorTileType.Spruce => Constants.SpawnProbabilities.Spruce,
                DecorTileType.Bush => Constants.SpawnProbabilities.Bush,
                DecorTileType.Log => Constants.SpawnProbabilities.Log,
                DecorTileType.Rock => Constants.SpawnProbabilities.Rock,
                DecorTileType.Root => Constants.SpawnProbabilities.Root,
                DecorTileType.DeadSpruce => Constants.SpawnProbabilities.DeadSpruce,
                DecorTileType.DeadTree => Constants.SpawnProbabilities.DeadTree,
                _ => 0
            };
            // Take the prefab according to the probability
            var isDecorTile = spawnProbability < objectSpawnProbability;
            var decorPrefab = isDecorTile 
                ? _decorTiles[decorTileType][Random.Next(0, _decorTiles[decorTileType].Length)] 
                : paddingTilePrefab;
            // Generate the tile
            var tileObj = Instantiate(decorPrefab, new Vector3(x, 0, y), Quaternion.identity);
            tileObj.transform.SetParent(transform, false);
            // Set the name
            tileObj.name = $"Tile {x} {y} padding" + (isDecorTile ? $" {decorTileType.ToString()}" : "");
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
        
        private void SpawnRobot()
        {
            const int xIndex = GridXYStartIndex + 2;
            const int yIndex = (GridYEndIndex - GridXYStartIndex) / 2 + Constants.GameSettings.GridPadding;
            var robotObj = Instantiate(robotPrefab, new Vector3(xIndex, 1, yIndex), Quaternion.identity);
            robotObj.name = "Robot";
        }

        private void SpawnTent()
        {
            const int xIndex = GridXYStartIndex - TentController.TentLength / 2;
            const int yIndex = (GridYEndIndex - GridXYStartIndex) / 2 + Constants.GameSettings.GridPadding;
            var tentObj = Instantiate(tentPrefab, new Vector3(xIndex, 1, yIndex), Quaternion.Euler(0, 90f, 0));
            tentObj.name = "Tent";
        }

        public bool CanMoveRight(float newX) => CanGoToNewX(newX);

        public bool CanMoveLeft(float newX) => CanGoToNewX(newX);
        
        private static bool CanGoToNewX(float newX) => newX is >= GridXYStartIndex and < GridXEndIndex;
        
        public bool CanMoveUp(float newY) => CanGoToNewY(newY);

        public bool CanMoveDown(float newY) => CanGoToNewY(newY);
        
        private static bool CanGoToNewY(float newY) => newY is >= GridXYStartIndex and < GridYEndIndex;
    }
}