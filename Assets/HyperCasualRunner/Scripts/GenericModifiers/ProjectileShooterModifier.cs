using HyperCasualRunner.Interfaces;
using HyperCasualRunner.Modifiables;
using HyperCasualRunner.PopulationManagers;
using UnityEngine;

namespace HyperCasualRunner.GenericModifiers
{
	/// <summary>
	/// Modifies all ProjectileShooterModifiables. They spawn projectile on an specified interval. It uses common Modifier-Modifiable pattern, so it acts on top of every Modifiable object.
	/// </summary>
	public class ProjectileShooterModifier : GenericModifier<ProjectileShooterModifiable>, ITickable
	{
		[SerializeField, Tooltip("Shooting interval in seconds. Every x second, it shoots.")] float _shootInterval;
		
		float _timer;

		public override void Initialize(PopulationManagerBase populationManagerBase)
		{
			base.Initialize(populationManagerBase);

			foreach (ProjectileShooterModifiable projectileShooterController in EffectReceivers)
			{
				projectileShooterController.Initialize();
			}
		}

		public void Tick()
		{
			_timer += Time.deltaTime;

			if (_timer >= _shootInterval)
			{
				_timer -= _shootInterval;
				Shoot();
			}
		}
		
		void Shoot()
		{
			foreach (ProjectileShooterModifiable projectileShooter in EffectReceivers)
			{
				if (projectileShooter.gameObject.activeInHierarchy)
				{
					projectileShooter.Shoot();
				}
			}
		}
	}
}