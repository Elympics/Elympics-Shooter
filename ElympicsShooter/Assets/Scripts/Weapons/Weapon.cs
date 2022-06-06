using Elympics;
using UnityEngine;

public abstract class Weapon : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
	[SerializeField] protected float damage = 0.0f;
	[SerializeField] [Tooltip("Shots per minute")] protected float fireRate = 60.0f;

	protected ElympicsFloat currentTimeBetweenShoots = new ElympicsFloat();

	protected float timeBetweenShoots = 0.0f;
	public float TimeBetweenShoots => timeBetweenShoots;

	private bool IsReadyToShot => currentTimeBetweenShoots >= timeBetweenShoots;

	public void Initialize()
	{
		CalculateTimeBetweenShoots();
	}

	public void CalculateTimeBetweenShoots()
	{
		timeBetweenShoots = 60.0f / fireRate;
	}

	public void ExecutePrimaryAction()
	{
		Shot();
	}

	private void Shot()
	{
		if (IsReadyToShot)
		{
			ProcessBulletSpawn();

			currentTimeBetweenShoots.Value = 0.0f;
		}
	}

	protected abstract void ProcessBulletSpawn();

	public void ElympicsUpdate()
	{
		if (!IsReadyToShot)
		{
			currentTimeBetweenShoots.Value += Elympics.TickDuration;
		}
	}
}
