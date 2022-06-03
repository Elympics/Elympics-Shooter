using Elympics;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class CameraInitializer : ElympicsMonoBehaviour, IInitializable
{
	[SerializeField] private bool isDefaultCamera = false;

	public void Initialize()
	{
		var playerData = GetComponent<PlayerData>();

		InitializeCameras(playerData);
	}

	private void InitializeCameras(PlayerData playerData)
	{
		var camerasInChildren = GetComponentsInChildren<Camera>();

		bool enableCamera = ((int)Elympics.Player == playerData.PlayerId) || isDefaultCamera;

		foreach (Camera camera in camerasInChildren)
			camera.gameObject.SetActive(enableCamera);
	}
}
