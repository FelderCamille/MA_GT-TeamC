using UnityEngine;

namespace Net
{
	public class CampController : MonoBehaviour
	{
		[Header("The base of the camp")]
		public BaseController baseController;

		[Header("The \"owner\" field of this camp (where the player demine)")]
		public FieldController field;

		[Header("The spawn position")]
		public Transform spawnPosition;

		void Start() { }

		void Update() { }
	}
}
