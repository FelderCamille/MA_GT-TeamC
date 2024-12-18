using System.Linq;
using Core;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{
    public class TentController : NetworkBehaviour
    {
        private const float StoreDistance = Constants.GameSettings.NumberOfTileOpenStore + .5f; // On z axis
        public const int TentLength = 4;
        
        private StoreController _store;
        private float _tentXPosition;
        private float _tentZ1Position;
        private float _tentZ2Position;

        private RobotController _robot;
        
        private void Start()
        {
            // Run only on the owner client
            if (!IsOwner) return;
            // Get robot that owns the tent
            _robot = FindObjectsByType<RobotController>(FindObjectsSortMode.None).First(robot => robot.IsOwner);
            // Compute tent position
            ComputeTentPosition();
            // Get the store controller
            _store = FindFirstObjectByType<StoreController>(FindObjectsInactive.Include);
        }

        private void ComputeTentPosition()
        {
            if (_robot.OwnerClientId == 0)
            {
                _tentXPosition = transform.position.x + TentLength - 1;
                _tentZ1Position = transform.position.z - TentLength / 2;
                _tentZ2Position = transform.position.z + TentLength / 2 - 1;
            }
            else
            {
                _tentXPosition = transform.position.x - TentLength;
                _tentZ1Position = transform.position.z - TentLength / 2 + 1;
                _tentZ2Position = transform.position.z + TentLength / 2;
            }
        }

        private void Update()
        {
            // Run only on the owner client
            if (!IsOwner) return;
            // Check if the user is close enough to the tent, if not return
            bool isCloseEnoughOnX;
            if (_robot.OwnerClientId == 0) isCloseEnoughOnX = _robot.transform.position.x - _tentXPosition <= StoreDistance;
            else isCloseEnoughOnX = _tentXPosition - _robot.transform.position.x <= StoreDistance;
            var isCloseEnoughOnZ = _robot.transform.position.z >= _tentZ1Position && _robot.transform.position.z <= _tentZ2Position;
            if (!isCloseEnoughOnX || !isCloseEnoughOnZ)
            {
                _store.JustOpened = false;
                return;
            }
            // Open the store
            if (!_store.IsShopping && !_store.JustOpened)
            {
                _store.OpenStore();
            }
        }
    }
}
