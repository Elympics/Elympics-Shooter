using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour
{
	private Vector2 movement = Vector2.zero;
	public Vector2 Movement => movement;

	public bool Jump { get; private set; }

	private void Update()
	{
		movement.x = Input.GetAxis("Horizontal");
		movement.y = Input.GetAxis("Vertical");

		Jump = Input.GetButton("Jump");
	}
}
