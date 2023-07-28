using System;
using DG.Tweening;
using HyperCasualRunner.ScriptableObjects;
using UnityEngine;

namespace HyperCasualRunner
{
	/// <summary>
	/// Projectile Pool spawns this, ProjectileShooterModifier uses this to shoot Projectile. It contains damage and speed of the projectiles. It damages Damageable types.
	/// </summary>
	public class Projectile : MonoBehaviour
	{
		[SerializeField] Rigidbody _rigidbody;
		[SerializeField] int _hitDamage;
		[SerializeField] float _speed;

		Tween _delayedCall;
		
		public ProjectilePool Pool { get; set; }
		
		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Damageable damageable))
			{
				damageable.TakeHit(_hitDamage);
				Release();
			}
		}

		void OnDestroy()
		{
			_delayedCall.Kill();
		}

		public void Fire()
		{
			_rigidbody.velocity = transform.forward * _speed;
			_delayedCall = DOVirtual.DelayedCall(1.5f, Release, false);
		}

		void Release()
		{
			_delayedCall.Kill();
			Pool.Release(this);
		}
	}
}
