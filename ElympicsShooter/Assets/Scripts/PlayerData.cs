using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
	[Header("Parameters:")]
	[SerializeField] private int playerId = 0;

	[Header("References:")]
	[SerializeField] private StatsController statsController = null;

	public int PlayerId => playerId;
	public StatsController StatsController => statsController;
}
