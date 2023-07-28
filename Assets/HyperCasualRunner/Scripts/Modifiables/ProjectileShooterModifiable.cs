using HyperCasualRunner.Interfaces;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.Modifiables
{
	/// <summary>
	/// Controls projectile shooting. You can specify particle effect when projectiles has been spawned.
	/// </summary>
	public class ProjectileShooterModifiable : MonoBehaviour
	{
		[SerializeField, Required] ProjectileShooter _defaultShooter;
		[SerializeField] bool _useParticle;
		[SerializeField, ShowIf(nameof(_useParticle))] ParticleSystem _shootingParticle;

		ITransformator _transformator;
		ProjectileShooter _activeProjectileShooter;

		public void Initialize()
		{
			_activeProjectileShooter = _defaultShooter;
			_transformator = GetComponent<ITransformator>();
			if (_transformator != null)
			{
				_transformator.Transformed += Transformator_Transformed;
			}
		}
		void Transformator_Transformed(GameObject obj)
		{
			_activeProjectileShooter = obj.GetComponent<ProjectileShooter>();
		}

		/// <summary>
		/// ProjectileShooterModifier calls this for shooting on PopulatedEntities. This trigger shooting on the activeProjectileShooter.
		/// </summary>
		public void Shoot()
		{
			Projectile projectile = _activeProjectileShooter.Get();
			projectile.transform.SetPositionAndRotation(_activeProjectileShooter.ProjectileSpawnPoint.position, transform.rotation);
			projectile.Fire();

			if (_useParticle)
			{
				_shootingParticle.Play();
			}
		}
	}
}
