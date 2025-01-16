using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Objects;
using UI.Tile;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{

    public class GridController : NetworkBehaviour, IGrid
    {
        private const int RobotSpawnDistance = 2;
        private const int GridXYStartIndex = Constants.GameSettings.GridPadding;
        private const int GridXEndIndex = Constants.GameSettings.GridPadding + Constants.GameSettings.GridWidth;
        private const int GridYEndIndex = Constants.GameSettings.GridPadding + Constants.GameSettings.GridHeight;
        private const int MapWidth = Constants.GameSettings.GridWidth + Constants.GameSettings.GridPadding * 2;
        private const int MapHeight = Constants.GameSettings.GridHeight + Constants.GameSettings.GridPadding * 2;
        private const int GridHalfWidthIndex = (Constants.GameSettings.GridWidth / 2) + Constants.GameSettings.GridPadding;
        private static readonly System.Random Random = new();

        [Header("Grid tiles")]
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private LandmineTile landmineTilePrefab;
        [SerializeField] private Tile safeAreaTilePrefab;

        [Header("Padding tiles")]
        [SerializeField] private Tile paddingTilePrefab;
        [SerializeField] private Tile[] treeTilePrefabs;
        [SerializeField] private Tile spruceTilePrefab;
        [SerializeField] private Tile[] rockTilePrefabs;
        [SerializeField] private Tile bushTilePrefab;
        [SerializeField] private Tile logTilePrefab;
        [SerializeField] private Tile rootTilePrefab;
        [SerializeField] private Tile deadTreeTilePrefab;
        [SerializeField] private Tile deadSpruceTilePrefab;

        [Header("Content")]
        [SerializeField] private RobotController playerPrefab;
        [SerializeField] private TentController tentTilePrefab;
        
        [Header("BoxColliders")]
        [SerializeField] private BoxCollider boxColliderBottom;
        [SerializeField] private BoxCollider boxColliderLeft;
        [SerializeField] private BoxCollider boxColliderRight;
        [SerializeField] private BoxCollider boxColliderUp;

        /// <summary>
        /// The landmines emplacement. The index is the position in the grid, the value is whether or not it has a landmine
        /// </summary>
        private readonly NetworkVariable<LandmineEmplacementData> _landmines = new ();
        
        private bool[] LandminesEmplacement => _landmines.Value.emplacements;

        /// <summary>
        /// The decor tiles of the padding according to the theme
        /// </summary>
        private readonly Dictionary<DecorTileType, Tile[]> _decorPaddingTiles = new();
        
        /// <summary>
        /// The decor tiles of the grid according to the theme
        /// </summary>
        private readonly Dictionary<DecorTileType, Tile[]> _decorGridTiles = new();

        /// <summary>
        /// The padding tiles of the grid. Permit to know if an emplacement (x,y) is occupied or not. False if occupied, true otherwise.
        /// </summary>
        private bool[][] _paddingTiles;
        
        /// <summary>
        /// Safe area tiles. Permit to know if an emplacement (x,y) is safe area or not. True if safe area, false otherwise.
        /// </summary>
        private bool[] _safeAreaGridTiles;
        
        /// <summary>
        /// Obstacles and safe area tiles. It's emplacements where no landmine can be placed. Permit to know if an
        /// emplacement (x,y) is occupied or not. True if occupied, false otherwise.
        /// </summary>
        private bool[] _obstacleGridTiles;

        private void Start()
        {
            SetupPaddingColliders();   
            // Compute emplacements
            InitPaddingTilesArray();
            ChooseDecorPrefabs();
            // Generate the tents (needs to be here to avoid the tent to be generated on a decor)
            _safeAreaGridTiles = new bool[Constants.GameSettings.GridWidth * Constants.GameSettings.GridHeight];
            _obstacleGridTiles = new bool[Constants.GameSettings.GridWidth * Constants.GameSettings.GridHeight];
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SpawnTent(clientId);
            }
            // Compute landmines emplacement
            ComputeLandminesEmplacement();
            // Generate the map
            GenerateMap();
            // Spawn robot
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SpawnRobot(clientId);
            }
        }
        
        private void SetupPaddingColliders()
        {
            // Compute dimensions and positions for the colliders
            const float paddingSize = Constants.GameSettings.GridPadding;
            const float halfTile = 0.5f;
            const float mapWidth = MapWidth - halfTile; // Handle the half tile
            const float mapHeight = MapHeight - halfTile; // Handle the half tile
            const float halfPadding = paddingSize / 2f - halfTile;
            // Top Padding Collider
            if (boxColliderUp != null)
            {
                boxColliderUp.size = new Vector3(MapWidth, 1, paddingSize);
                boxColliderUp.center = new Vector3(mapWidth / 2f, 0.5f, mapHeight - paddingSize / 2f);
            }
            // Bottom Padding Collider
            if (boxColliderBottom != null)
            {
                boxColliderBottom.size = new Vector3(MapWidth, 1, paddingSize);
                boxColliderBottom.center = new Vector3(MapWidth / 2f, 0.5f, halfPadding);
            }
            // Left Padding Collider
            if (boxColliderLeft != null)
            {
                boxColliderLeft.size = new Vector3(paddingSize, 1, MapHeight);
                boxColliderLeft.center = new Vector3(halfPadding, 0.5f, MapHeight / 2f);
            }
            // Right Padding Collider
            if (boxColliderRight != null)
            {
                boxColliderRight.size = new Vector3(paddingSize, 1, MapHeight);
                boxColliderRight.center = new Vector3(mapWidth - paddingSize / 2f, 0.5f, mapHeight / 2f);
            }
        }

        private void ChooseDecorPrefabs()
        {
            // Add tiles to the dictionary according to the theme
            var mapTheme = GameParametersManager.Instance.MapTheme;
            switch (mapTheme)
            {
                case MapTheme.Nature:
                    _decorPaddingTiles.Add(DecorTileType.Tree, treeTilePrefabs);
                    _decorPaddingTiles.Add(DecorTileType.Spruce, new[] { spruceTilePrefab });
                    _decorPaddingTiles.Add(DecorTileType.Bush, new[] { bushTilePrefab });
                    _decorGridTiles.Add(DecorTileType.Bush, new[] { bushTilePrefab });
                    break;
                case MapTheme.War:
                    _decorPaddingTiles.Add(DecorTileType.DeadTree, new[] { deadTreeTilePrefab });
                    _decorPaddingTiles.Add(DecorTileType.DeadSpruce, new[] { deadSpruceTilePrefab });
                    break;
                default:
                    throw new NotImplementedException("The theme is not implemented");
            }
            // Add common tiles to the dictionary
            _decorPaddingTiles.Add(DecorTileType.Root, new[] { rootTilePrefab });
            _decorGridTiles.Add(DecorTileType.Root, new[] { rootTilePrefab });
            _decorPaddingTiles.Add(DecorTileType.Rock, rockTilePrefabs);
            _decorGridTiles.Add(DecorTileType.Rock, rockTilePrefabs);
            _decorPaddingTiles.Add(DecorTileType.Log, new[] { logTilePrefab });
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
                        _paddingTiles[x][y] = false; // Cannot place something on the grid
                    }
                    else
                    {
                        _paddingTiles[x][y] = true; // Can place something
                    }
                }
            }
        }

        private void SpawnTent(ulong clientId)
        {
            // Compute emplacement
            var isLeft = clientId == 0;
            var xIndex = isLeft ? (GridXYStartIndex - TentController.TentLength) : GridXEndIndex;
            var yIndex = MapHeight / 2;
            const int padding = TentController.TentLength / 2;
            var rotationY = isLeft ? 90f : 270f;
            // Set the emplacements as occupied
            for (var x = xIndex; x < xIndex + TentController.TentLength; x++)
            {
                for (var y = yIndex - padding; y < yIndex + padding; y++)
                {
                    _paddingTiles[x][y] = false;
                }
            }
            // Fix the right tent position because of the prefab orientation
            if (!isLeft)
            {
                xIndex += TentController.TentLength - 1;
                yIndex -= 1;
            }
            // Compute safe area of the tent
            int safeAreaXMin, safeAreaXMax;
            if (isLeft)
            {
                safeAreaXMin = 0;
                safeAreaXMax = Constants.GameSettings.SafeAreaWidth;
            }
            else
            {
                safeAreaXMax = Constants.GameSettings.GridWidth;
                safeAreaXMin = safeAreaXMax - Constants.GameSettings.SafeAreaWidth;
            }
            const int safeAreaYMin = Constants.GameSettings.GridHeight / 2 - padding;
            const int safeAreaYMax = Constants.GameSettings.GridHeight / 2 + padding;
            for (var x = safeAreaXMin; x < safeAreaXMax; x++)
            {
                for (var y = safeAreaYMin; y < safeAreaYMax; y++)
                {
                    var index = GetMapIndex(x, y);
                    _safeAreaGridTiles[index] = true;
                    _obstacleGridTiles[index] = true;
                }
            }
            // Place the tent
            if (!NetworkManager.Singleton.IsHost) return;
            var tentObj = Instantiate(
                tentTilePrefab,
                new Vector3(xIndex, 0, yIndex),
                Quaternion.Euler(0, rotationY, 0)
                );
            tentObj.name = $"Tent {clientId}";
            tentObj.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        }
        
        private void ComputeLandminesEmplacement()
        {
            // Only host can compute the landmines emplacement
            if (!NetworkManager.Singleton.IsHost) return;
            // Initialize the array
            _landmines.Value = new LandmineEmplacementData{emplacements = new bool[Constants.GameSettings.GridWidth * Constants.GameSettings.GridHeight]};
            // Compute the emplacements in the first half of the grid (left side)
            ComputeLandmineEmplacement(GridXYStartIndex, GridHalfWidthIndex);
            // Compute the emplacements in the second half of the grid (right side)
            if (!Constants.DebugAllowOnlyOneConnection) ComputeLandmineEmplacement(GridHalfWidthIndex, GridXEndIndex);
        }

        private void ComputeLandmineEmplacement(int xStart, int xEnd)
        {
            for (var i = 0; i < Constants.GameSettings.NumberOfLandminesPerSide; i++)
            {
                // Compute the emplacement
                int landmineIndex;
                do
                {
                    var x = Random.Next(xStart, xEnd);
                    var y = Random.Next(GridXYStartIndex, GridYEndIndex);
                    landmineIndex = GetGridIndex(x, y);
                } while (LandminesEmplacement[landmineIndex] || _obstacleGridTiles[landmineIndex]);
                // Set a landmine at this emplacement
                LandminesEmplacement[landmineIndex] = true;
            }
        }
        
        public int NotClearedMineCount(ulong clientId)
        {
            var notClearedMines = 0;
            for (var i = 0; i < LandminesEmplacement.Length; i++)
            {
                if (!LandminesEmplacement[i]) continue;
                // Transform the index to x and y
                var x = i / Constants.GameSettings.GridHeight + Constants.GameSettings.GridPadding;
                // Check if the emplacement is in the client area
                var isClientLeft = clientId == 0;
                var isEmplacementLeft = x < GridHalfWidthIndex;
                if ((isClientLeft && isEmplacementLeft) || (!isClientLeft && !isEmplacementLeft)) notClearedMines++;
            }
            return notClearedMines;
        }
        
        private void SpawnRobot(ulong clientId)
        {
            // Only host can spawn the robot
            if (!NetworkManager.Singleton.IsHost) return;
            // Compute emplacement
            var (position, rotation) = ComputeRobotEmplacement(clientId);
            // Spawn the robot
            var robot = Instantiate(playerPrefab, position, rotation);
            robot.name = $"Robot {clientId}";
            robot.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
        
        public void ResetRobotSpawn(RobotController robot)
        {
            // Compute emplacement
            var (position, rotation) = ComputeRobotEmplacement(robot.OwnerClientId);
            // Reset the robot spawn (automatically updated by the ClientTransform)
            robot.transform.position = position;
            robot.transform.rotation = rotation;
        }

        private static (Vector3, Quaternion) ComputeRobotEmplacement(ulong clientId)
        {
            var isLeft = clientId == 0;
            // Compute emplacement
            var xIndex = isLeft ? (GridXYStartIndex + RobotSpawnDistance - 1) : (GridXEndIndex - RobotSpawnDistance);
            const int yIndex = MapHeight / 2;
            var rotationY = isLeft ? 90f : 270f;
            // Return position and rotation
            var position = new Vector3(xIndex, 0.1f, yIndex);
            var rotation = Quaternion.Euler(0, rotationY, 0);
            return (position, rotation);
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
            if (_paddingTiles[x][y] == false) return;
            // Randomly pick a tile type
            var tileTypeIndex = Random.Next(0, _decorPaddingTiles.Keys.Count);
            var decorTileType = _decorPaddingTiles.Keys.ElementAt(tileTypeIndex);
            // Compute the probability of spawn
            var spawnProbability = Random.Next(0, 100);
            var objectSpawnProbability = decorTileType switch
            {
                DecorTileType.Tree => Constants.PaddingSpawnProbabilities.Tree,
                DecorTileType.Spruce => Constants.PaddingSpawnProbabilities.Spruce,
                DecorTileType.Bush => Constants.PaddingSpawnProbabilities.Bush,
                DecorTileType.Log => Constants.PaddingSpawnProbabilities.Log,
                DecorTileType.Rock => Constants.PaddingSpawnProbabilities.Rock,
                DecorTileType.Root => Constants.PaddingSpawnProbabilities.Root,
                DecorTileType.DeadSpruce => Constants.PaddingSpawnProbabilities.DeadSpruce,
                DecorTileType.DeadTree => Constants.PaddingSpawnProbabilities.DeadTree,
                _ => 0
            };
            // Take the prefab according to the probability
            var isDecorTile = spawnProbability < objectSpawnProbability;
            var decorPrefab = isDecorTile
                ? _decorPaddingTiles[decorTileType][Random.Next(0, _decorPaddingTiles[decorTileType].Length)]
                : paddingTilePrefab;
            // Check if the prefab can be placed, otherwise place a padding
            if (isDecorTile)
            {
                var canPlace = CanPlaceDecorPrefab(x, y, decorPrefab, true);
                if (!canPlace) decorPrefab = paddingTilePrefab;
            }
            // Set the emplacements as occupied
            ReserveDecorEmplacement(x, y, decorPrefab, true);
            // Generate the tile
            var tileObj = Instantiate(decorPrefab, new Vector3(x, 0, y), Quaternion.identity);
            // tileObj.transform.SetParent(transform, false); TODO: set the parent
            // Set the name
            tileObj.name = $"Tile {x} {y} padding" + (isDecorTile ? $" {decorTileType.ToString()}" : "");
        }

        private bool CanPlaceDecorPrefab(int x, int y, Tile decorPrefab, bool onPadding)
        {
            for (var x1 = x; x1 < x + decorPrefab.width; x1++)
            {
                for (var y1 = y; y1 < y + decorPrefab.depth; y1++)
                {
                    if (onPadding)
                    {
                        if (x1 < MapWidth && y1 < MapHeight && _paddingTiles[x1][y1]) continue;
                    }
                    else
                    {
                        if (x1 < MapWidth && y1 < MapHeight && !_obstacleGridTiles[GetGridIndex(x1, y1)]) continue;
                    }
                    return false;
                }
            }
            return true;
        }

        private void ReserveDecorEmplacement(int x, int y, Tile decorPrefab, bool onPadding)
        {
            for (var x1 = x; x1 < x + decorPrefab.width && x1 < MapWidth; x1++)
            {
                for (var y1 = y; y1 < y + decorPrefab.depth && y1 < MapHeight; y1++)
                {
                    if (onPadding)
                    {
                        _paddingTiles[x1][y1] = false;
                    }
                    else
                    {
                        _obstacleGridTiles[GetGridIndex(x1, y1)] = true;
                    }
                }
            }
        }

        private void GenerateGridTile(int x, int y)
        {
            if (!NetworkManager.Singleton.IsHost) return;
            // Check if the emplacement must be a classic tile, a landmine, a safe area, or an obstacle
            var index = GetGridIndex(x, y);
            var tileName = $"Tile {x} {y}";
            Tile prefab;
            if (LandminesEmplacement[index]) // Landmine
            {
                prefab = landmineTilePrefab;
                tileName += " x";
            }
            else if (_safeAreaGridTiles[index]) // Safe area
            {
                prefab = safeAreaTilePrefab;
                tileName += " safe";
            }
            else // Try to place some decor
            {
                // Randomly pick a tile type
                var tileTypeIndex = Random.Next(0, _decorGridTiles.Keys.Count);
                var decorTileType = _decorGridTiles.Keys.ElementAt(tileTypeIndex);
                // Compute the probability of spawn
                var spawnProbability = Random.Next(0, 100);
                var objectSpawnProbability = decorTileType switch
                {
                    DecorTileType.Tree => Constants.GridSpawnProbabilities.Tree,
                    DecorTileType.Spruce => Constants.GridSpawnProbabilities.Spruce,
                    DecorTileType.Bush => Constants.GridSpawnProbabilities.Bush,
                    DecorTileType.Log => Constants.GridSpawnProbabilities.Log,
                    DecorTileType.Rock => Constants.GridSpawnProbabilities.Rock,
                    DecorTileType.Root => Constants.GridSpawnProbabilities.Root,
                    DecorTileType.DeadSpruce => Constants.GridSpawnProbabilities.DeadSpruce,
                    DecorTileType.DeadTree => Constants.GridSpawnProbabilities.DeadTree,
                    _ => 0
                };
                // Take the prefab according to the probability
                var isDecorTile = spawnProbability < objectSpawnProbability;
                if (isDecorTile)
                {
                    prefab = _decorGridTiles[decorTileType][Random.Next(0, _decorGridTiles[decorTileType].Length)];
                    if (CanPlaceDecorPrefab(x, y, prefab, false))
                    {
                        name += decorTileType.ToString();
                        ReserveDecorEmplacement(x, y, prefab, false);
                    }
                    else
                    {
                        prefab = tilePrefab;
                    }
                }
                else
                {
                    prefab = tilePrefab;
                }
            }
            // Generate the tile
            var tileObj = Instantiate(prefab, new Vector3(x, 0, y), Quaternion.identity);
            tileObj.name = tileName;
            tileObj.GetComponent<NetworkObject>().Spawn();
            // TODO: set the tile as a grid tile
        }
        
        public void ReplaceMineByTile(LandmineTile landmineTile)
        {
            // Only host can replace the landmine by a classic tile
            if (!NetworkManager.Singleton.IsHost) return;
            // Get the position of the landmine tile
            var x = (int) landmineTile.transform.position.x;
            var y = (int) landmineTile.transform.position.z;
            // Despawn the landmine tile
            landmineTile.GetComponent<NetworkObject>().Despawn();
            var index = GetGridIndex(x, y);
            LandminesEmplacement[index] = false;
            // Replace the landmine tile by a classic tile
            var tileObj = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
            tileObj.name = $"Tile {x} {y}";
            tileObj.GetComponent<NetworkObject>().Spawn();
        }

        
        public bool CanPlaceMine(int x, int y, ulong clientId)
        {
            // Check if the emplacement is in the grid area
            if (x is < GridXYStartIndex or >= GridXEndIndex || y is < GridXYStartIndex or >= GridYEndIndex) return false;
            // Check if the emplacement is in the other client area
            var isOnOwnSide = IsOnOwnSide(x, clientId);
            if (isOnOwnSide) return false;
            // Check whether the tile is not already a landmine or on a safe area
            var index = GetGridIndex(x, y);
            // Check if the tile is not already a landmine or a decor
            return !LandminesEmplacement[index] && !_obstacleGridTiles[index];
        }

        public static bool IsOnOwnSide(int x, ulong clientId)
        {
            var isClientLeft = clientId == 0;
            var isEmplacementLeft = x < GridHalfWidthIndex;
            return (isClientLeft && isEmplacementLeft) || (!isClientLeft && !isEmplacementLeft);
        }
        
        public void ReplaceTileByMine(int x, int y, LandmineDifficulty difficulty, ulong clientId)
        {
            // Only host can replace the tile by a landmine
            if (!NetworkManager.Singleton.IsHost) return;
            // Check if the emplacement is valid
            if (!CanPlaceMine(x, y, clientId)) return;
            // Get the tile
            var tile = FindObjectsByType<Tile>(FindObjectsSortMode.None).First(t => t.name == $"Tile {x} {y}");
            // Despawn the tile
            tile.GetComponent<NetworkObject>().Despawn();
            // Indicate that the tile is now a landmine
            LandminesEmplacement[GetGridIndex(x, y)] = true;
            // Replace the classic tile by a landmine tile
            var landmineTileObj = Instantiate(landmineTilePrefab, new Vector3(x, 0, y), Quaternion.identity);
            landmineTileObj.name = $"Tile {x} {y} x";
            landmineTileObj.GetComponentInChildren<LandmineController>().Difficulty = difficulty;
            landmineTileObj.GetComponent<NetworkObject>().Spawn();
            // Indicate that the client has placed a landmine
            var robot = FindObjectsByType<RobotController>(FindObjectsSortMode.None)
                .First(r => r.OwnerClientId == clientId);
            robot.GetComponent<ResourcesManager>().IncreasePlacedMinesCounterRpc();
        }

        public bool CanMoveRight(float newX) => CanGoToNewX(newX);

        public bool CanMoveLeft(float newX) => CanGoToNewX(newX);

        private static bool CanGoToNewX(float newX) => newX is >= GridXYStartIndex and < GridXEndIndex;

        public bool CanMoveUp(float newY) => CanGoToNewY(newY);

        public bool CanMoveDown(float newY) => CanGoToNewY(newY);

        private static bool CanGoToNewY(float newY) => newY is >= GridXYStartIndex and < GridYEndIndex;

        private static int GetGridIndex(int x, int y) =>
            (x - Constants.GameSettings.GridPadding) * Constants.GameSettings.GridHeight
            + (y - Constants.GameSettings.GridPadding);

        private static int GetMapIndex(int x, int y) => x * Constants.GameSettings.GridHeight + y;
    }
}