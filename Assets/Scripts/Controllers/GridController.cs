using System;
using Classes;
using UnityEngine;

namespace UI
{
    public class GridController : MonoBehaviour
    {
        
        [Header("Content")]
        public Tile TilePrefab;

        [Header("Settings")]
        public int Width;
        public int Height;

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var tileObj = Instantiate(TilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                    tileObj.transform.SetParent(this.transform, false);
                    tileObj.name = $"Tile {x} {y}";
                }
            }
        }
        
    }
}