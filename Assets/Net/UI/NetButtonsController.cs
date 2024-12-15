using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace Net
{
	public class NetButtons : MonoBehaviour
	{
		public Button btnServer,
			btnHost,
			btnJoin,
			btnQuit;

		public String NET_HOST = "127.0.0.1";
		public ushort NET_PORT = 7777;

		void Start()
		{
			NetworkManager
				.Singleton.GetComponent<UnityTransport>()
				.SetConnectionData(NET_HOST, NET_PORT);

			btnServer.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
			btnHost.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
			btnJoin.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
			btnQuit.onClick.AddListener(() => NetworkManager.Singleton.Shutdown());
		}

		void LateUpdate()
		{
			var net = NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient;

			btnServer.gameObject.SetActive(!net);
			btnHost.gameObject.SetActive(!net);
			btnJoin.gameObject.SetActive(!net);
			btnQuit.gameObject.SetActive(net);
		}
	}
}
