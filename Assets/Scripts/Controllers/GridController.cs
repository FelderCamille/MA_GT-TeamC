using System.Collections.Generic;
using System.Linq;
using Objects;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{

    public class GridController : MonoBehaviour, IGrid
    {
        private const int GridXYStartIndex = Constants.GameSettings.GridPadding;
        private const int GridXEndIndex = Constants.GameSettings.GridPadding + Constants.GameSettings.GridWidth;
        private const int GridYEndIndex = Constants.GameSettings.GridPadding + Constants.GameSettings.GridHeight;
        private const int MapWidth = Constants.GameSettings.GridWidth + Constants.GameSettings.GridPadding * 2;
        private const int MapHeight =  Constants.GameSettings.GridHeight + Constants.GameSettings.GridPadding * 2;
        private static readonly System.Random Random = new ();
        
        [Header("Grid tiles")]
        public Tile tilePrefab;
        public LandmineTile landmineTilePrefab;
        
        [Header("Padding tiles")]
        public Tile paddingTilePrefab;
        public Tile[] treeTilePrefabs;
        public Tile spruceTilePrefab;
        public Tile[] rockTilePrefabs;
        public Tile bushTilePrefab;
        public Tile logTilePrefab;
        public Tile rootTilePrefab;
        public Tile deadTreeTilePrefab;
        public Tile deadSpruceTilePrefab;
        
        [Header("Content")]
        public TentController tentTilePrefab;

        /// <summary>
        /// The landmines emplacement. The index is the position in the grid, the value is whether or not it has a landmine
        /// </summary>
        private bool[] _landmines;
        
        /// <summary>
        /// The decor tiles according to the theme
        /// </summary>
        private readonly Dictionary<DecorTileType, Tile[]> _decorTiles = new ();

        /// <summary>
        /// The tiles of the grid. Permit to know if an emplacement (x,y) is occupied or not
        /// </summary>
        private bool[][] _paddingTiles;

        public float MinX => GridXYStartIndex;
        public float MaxX => GridXEndIndex;
        public float MinZ => GridXYStartIndex;
        public float MaxZ => GridYEndIndex;

        private void Awake()
        {
            // Compute emplacements
            InitPaddingTilesArray();
            ChooseDecorPrefabs();
            ComputeLandminesEmplacement();
            // Generate the map and spawn the robot and tent
            // SpawnTent(); // TODO - TEMP
            GenerateMap();
        }
        
        private void ChooseDecorPrefabs()
        {
            // Add tiles to the dictionary according to the theme
            switch (Constants.GameSettings.GameMapTheme)
            {
                case MapTheme.Nature:
                    _decorTiles.Add(DecorTileType.Tree, treeTilePrefabs);
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
        
        private void InitPaddingTilesArray()
        {
            // Init the padding tiles array
            _paddingTiles = new bool[MapWidth][];
            for (var x = 0; x < _paddingTiles.Length; x++)
            {
                _paddingTiles[x] = new bool[MapHeight];
                for (var y = 0; y < _paddingTiles[x].Length; y++)
                {
                    if (x is >= GridXYStartIndex and < GridXEndIndex
                        && y is >= GridXYStartIndex and < GridYEndIndex)
                    {
                        _paddingTiles[x][y] = false; // Cannot place something
                    }
                    else
                    {
                        _paddingTiles[x][y] = true; // Can place something
                    }
                }
            }
        }
        
        private void SpawnTent()
        {
            // Compute emplacement
            const int xIndex = GridXYStartIndex - TentController.TentLength;
            const int yIndex = MapHeight / 2;
            const int padding = TentController.TentLength / 2;
            // Set the emplacements as occupied
            for (var x = xIndex; x < xIndex + TentController.TentLength; x++)
            {
                for (var y = yIndex - padding; y < yIndex + padding; y++)
                {
                    _paddingTiles[x][y] = false;
                }
            }
            // Place the tent
            var tentObj = Instantiate(tentTilePrefab, new Vector3(xIndex, 0, yIndex), Quaternion.Euler(0, 90f, 0));
            tentObj.name = "Tent";
        }

        private void GenerateMap()
        {
            // Generate the map
            for (var x = 0; x < MapWidth; x++)
            {
                for (var y = 0; y < MapHeight; y++)
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
            // Check if the emplacement is already occupied
            if (_paddingTiles[x][y] == false)
            {
                return;
            }
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
            // Check if the prefab can be placed, otherwise place a padding
            if (isDecorTile)
            {
                for (var x1 = x; x1 < x + decorPrefab.width; x1++)
                {
                    for (var y1 = y; y1 < y + decorPrefab.depth; y1++)
                    {
                        if (x1 < MapWidth && y1 < MapHeight && _paddingTiles[x1][y1]) continue;
                        decorPrefab = paddingTilePrefab;
                        break;
                    }
                } 
            }
            // Set the emplacements as occupied
            for (var x1 = x; x1 < x + decorPrefab.width && x1 < MapWidth; x1++)
            {
                for (var y1 = y; y1 < y + decorPrefab.depth && y1 < MapHeight; y1++)
                {
                    _paddingTiles[x1][y1] = false;
                }
            }
            // Generate the tile
            var tileObj = Instantiate(decorPrefab, new Vector3(x, 0, y), Quaternion.identity);
            tileObj.transform.SetParent(transform, false);
            // Set the name
            tileObj.name = $"Tile {x} {y} padding" + (isDecorTile ? $" {decorTileType.ToString()}" : "");
        }

        private void GenerateGridTile(int x, int y)
        {
            // Check if the emplacement must be a classic tile or a landmine
            var index = (x - Constants.GameSettings.GridPadding) * Constants.GameSettings.GridHeight 
                        + (y - Constants.GameSettings.GridPadding);
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