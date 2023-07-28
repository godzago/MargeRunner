using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualRunner
{
	/// <summary>
	/// Damage sources like Projectiles attacks this. It contains health and if it falls below 0, it dies.
	/// Class itself is extensible by allowing you to bind other methods using UnityEvents. 
	/// </summary>
	public class Damageable : MonoBehaviour
	{
		[SerializeField] int _hitPoint;
		public UnityEvent<HitDamageInfo> OnTakingHit;
		[SerializeField] UnityEvent _onDied;

		public int HitPoint => _hitPoint;
		
		public void TakeHit(int hitDamage)
		{
			if (_hitPoint <= 0)
			{
				return;
			}

			HitDamageInfo hitDamageInfo = new HitDamageInfo(hitDamage, _hitPoint - hitDamage);
			OnTakingHit.Invoke(hitDamageInfo);
			_hitPoint -= hitDamage;
			if (_hitPoint <= 0)
			{
				_onDied.Invoke();
			}
		}
	}

}
