using Elympics;
using UnityEngine;

public abstract class Weapon : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
	[SerializeField] protected float damage = 0.0f;
	[SerializeField] [Tooltip("Shots per minute")] protected float fireRate = 60.0f;

	protected ElympicsFloat currentTimeBetweenShoots = new ElympicsFloat();

	protected float timeBetweenShoots = 0.0f;
	public float TimeBetweenShoots => timeBetweenShoots;

	private bool IsReadyToShoot => currentTimeBetweenShoots >= timeBetweenShoots;

	public GameObject Owner => this.transform.root.gameObject;

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
		if (IsReadyToShoot)
		{
			ProcessBulletSpawn();

			currentTimeBetweenShoots.Value = 0.0f;
		}
	}

	protected abstract void ProcessBulletSpawn();

	public virtual void ElympicsUpdate()
	{
		if (!IsReadyToShoot)
		{
			currentTimeBetweenShoots.Value += Elympics.TickDuration;
		}
	}
}
