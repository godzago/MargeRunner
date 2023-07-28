using HyperCasualRunner.ScriptableObjects;
using UnityEngine;

namespace HyperCasualRunner
{
	/// <summary>
	/// ProjectileShooterModifiable uses this class for picking right Projectile for the active PopulatedEntity. Use this when you have multiple gameObjects (like playerLevel1, playerLevel2) and
	/// everyone of them shoots different projectile. You can see the example in Evolution Of Crowds demo scene. 
	/// </summary>
	public class ProjectileShooter : MonoBehaviour
	{
		[SerializeField] ProjectilePool _projectilePool;
		
		public Transform ProjectileSpawnPoint;

		public Projectile Get()
		{
			return _projectilePool.Get();
		}
	}
}
