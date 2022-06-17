using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	[SerializeField] private PlayerScoresManager playerScoresManager = null;

	private void Awake()
	{
		SetDefaultCursorState();

		playerScoresManager.GameEnded.ValueChanged += UnlockCursor;
	}

	private void UnlockCursor(bool lastValue, bool newValue)
	{
		if (newValue)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	private void SetDefaultCursorState()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
