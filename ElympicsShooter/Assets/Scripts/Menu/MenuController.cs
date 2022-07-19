using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	[SerializeField] private Button playButton = null;
	[SerializeField] private GameObject matchmakingStatus = null;
	[SerializeField] private string matchmakingQueueBase = "Default";
	[SerializeField] private string matchmakingQueueFree = "Free";

	private void Start()
	{
		ElympicsLobbyClient.Instance.Authenticated += HandleAuthenticated;
		playButton.interactable = ElympicsLobbyClient.Instance.IsAuthenticated;
	}

	private void HandleAuthenticated(bool success, string userId, string jwtToken, string error)
	{
		if (success)
			playButton.interactable = success;
		else
			Debug.Log(error);
	}

	public void OnPlayClicked()
	{
		//ElympicsLobbyClient.Instance.PlayOnline();
		ElympicsLobbyClient.Instance.PlayOnline(null, null, $"{matchmakingQueueBase}:{matchmakingQueueFree}");
		playButton.interactable = false;
		matchmakingStatus.SetActive(true);
	}
}
