using System;
using UnityEngine;
using UnityEngine.Pool;

namespace HyperCasualRunner.ScriptableObjects
{
	/// <summary>
	/// Simple pool which uses new Unity Pooling library. It's particularly useful when you spawn a lot of things.
	/// </summary>
	[CreateAssetMenu(fileName = "Projectile Pool", menuName = "HyperCasualPack/Projectile Pool", order = 0)]
	public class ProjectilePool : ScriptableObject
	{
		[SerializeField] Projectile _projectilePrefab;
		[SerializeField] bool _collectionCheck;
		[SerializeField] int _defaultCapacity = 50;
		[SerializeField] int _maxSize = 100;

		[NonSerialized] ObjectPool<Projectile> _projectilePool;

		void OnEnable()
		{
			_projectilePool = new ObjectPool<Projectile>(() =>
			{
				Projectile instantiated = Instantiate(_projectilePrefab);
				instantiated.Pool = this;
				return instantiated;
			}, shape =>
			{
				shape.gameObject.SetActive(true);
			}, shape => 
			{
				shape.gameObject.SetActive(false);
			}, shape =>
			{
				Destroy(shape.gameObject);
			}, _collectionCheck, _defaultCapacity, _maxSize);
		}

		public Projectile Get()
		{
			return _projectilePool.Get();
		}
		
		public void Release(Projectile projectile)
		{
			_projectilePool.Release(projectile);
		}
	}
}
