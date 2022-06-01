using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	[Header("Parameters:")]
	[SerializeField] private int playerId = 0;

	public int PlayerId => playerId;
}
