using TMPro;
using UnityEngine;

namespace HyperCasualRunner.Monitors
{
	/// <summary>
	/// It listens for the OnTakingHit event from Damageable and shows updated value on the TextMeshProUGUI.
	/// </summary>
	public class DamageableMonitor : MonoBehaviour
	{
		[SerializeField] Damageable _damageable;
		[SerializeField] TextMeshPro _hitPointText;

		void OnEnable()
		{
			_damageable.OnTakingHit.AddListener(Damageable_OnTakingHit);
			SetText(_damageable.HitPoint.ToString());
			
		}

		void Damageable_OnTakingHit(HitDamageInfo hitDamageInfo)
		{
			SetText(hitDamageInfo.RemainingHitPoint.ToString());
		}

		void SetText(string text)
		{
			_hitPointText.text = text;
		}
	}
}
