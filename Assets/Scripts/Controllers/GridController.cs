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
            for (var x = 0; x < Constants.GameSize.Width; x++)
            {
                for (var y = 0; y < Constants.GameSize.Height; y++)
                {
                    var tileObj = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                    tileObj.transform.SetParent(this.transform, false);
                    tileObj.name = $"Tile {x} {y}";
                }
            }
        }
        
    }
}