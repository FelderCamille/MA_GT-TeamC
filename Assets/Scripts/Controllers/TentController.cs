using UI;
using UnityEngine;

namespace Controllers
{
    public class TentController : MonoBehaviour
    {
        private const float StoreDistance = Constants.GameSettings.NumberOfTileOpenStore + .5f; // On z axis
        public const int TentLength = 4;
        
        private Tile _tile;
        private RobotController _robot;
        private StoreController _store;
        private float _tentXPosition;
        private float _tentZ1Position;
        private float _tentZ2Position;

        private void Start()
        {
            _tile = GetComponent<Tile>();
            _robot = FindObjectOfType<RobotController>();
            _tentXPosition = transform.position.x + _tile.width / 2;
            _tentZ1Position = transform.position.z - _tile.depth / 2;
            _tentZ2Position = transform.position.z + _tile.depth / 2;
            _store = FindObjectOfType<StoreController>(includeInactive: true);
        }

        private void Update()
        {
            // Check if the user is close enough to the tent, if not return
            var isCloseEnoughOnX = _robot.transform.position.x - _tentXPosition <= StoreDistance;
            var isCloseEnoughOnZ = _robot.transform.position.z >= _tentZ1Position && _robot.transform.position.z <= _tentZ2Position;
            if (!isCloseEnoughOnX || !isCloseEnoughOnZ)
            {
                _store.JustOpened = false;
                return;
            }
            // Open the store
            if (!_store.IsShopping && !_store.JustOpened) _store.OpenStore();
        }
    }
}
