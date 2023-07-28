using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner
{
	/// <summary>
	/// Simple blink feedback when hitting damageable things like enemies. It requires renderer to work.
	/// </summary>
	public class BlinkFeedback : MonoBehaviour
	{
		[SerializeField, Required] Renderer _rendererToBlink;
		[SerializeField] Color _blinkColor = Color.white;
		[SerializeField] float _duration = 0.1f;
		[SerializeField] int _blinkCount = 4;
		
		int _propertyId;
		bool _canBlink = true;
		
		void Awake()
		{
			_propertyId = Shader.PropertyToID("_EmissionColor");
		}
		
		public void Blink()
		{
			if (!_canBlink)
			{
				return;
			}
			_canBlink = false;
			_rendererToBlink.material.DOColor(_blinkColor, _propertyId, _duration).SetLoops(_blinkCount, LoopType.Yoyo).SetEase(Ease.InOutFlash).OnComplete(() => _canBlink = true);
		}
	}
}
