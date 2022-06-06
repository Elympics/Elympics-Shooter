using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	private void Awake()
	{
		SetDefaultCursorState();
	}

	private void SetDefaultCursorState()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = true;
	}
}
