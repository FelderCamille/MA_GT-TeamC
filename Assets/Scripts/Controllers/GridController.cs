using DefaultNamespace;
using UI;
using UnityEngine;

namespace Controllers
{
    public class GridController : MonoBehaviour
    {
        
        [Header("Content")]
        public Tile tilePrefab;

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int x = 0; x < Globals.GameSize.Width; x++)
            {
                for (int y = 0; y < Globals.GameSize.Height; y++)
                {
                    var tileObj = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                    tileObj.transform.SetParent(this.transform, false);
                    tileObj.name = $"Tile {x} {y}";
                }
            }
        }
        
    }
}