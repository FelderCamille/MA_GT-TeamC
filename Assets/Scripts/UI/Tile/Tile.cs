using UnityEngine;

namespace UI.Tile
{
    public class Tile : MonoBehaviour
    {
        [Header("Settings")]
        public int width = 1;
        public int depth = 1;
        
        [Header("Floor")]
        [SerializeField] private BaseTile[] baseTiles;
        
        public void InitAsOnGrid()
        {
            foreach (var baseTile in baseTiles)
            {
                baseTile.InitAsOnGrid();
            }
        }
    }
}