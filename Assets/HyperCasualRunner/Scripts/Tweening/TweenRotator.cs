using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.Tweening
{
	public class TweenRotator : MonoBehaviour
	{
		[SerializeField] LoopType _loopType;
		[SerializeField] Ease _ease;
		[SerializeField] Vector3 _relativeTargetRotation = new Vector3(0f, 180f, 0f);
		[SerializeField] float _targetPointArriveDuration = 1f;
		[SerializeField] bool _useDelayedStart;
		
		void Start()
		{
			if (_useDelayedStart)
			{
				float rnd = Random.Range(0f, 2f);
				transform.DORotate(_relativeTargetRotation, _targetPointArriveDuration).SetRelative().SetEase(_ease).SetDelay(rnd).SetLoops(-1, _loopType);
			}
			else
			{
				transform.DORotate(_relativeTargetRotation, _targetPointArriveDuration).SetRelative().SetEase(_ease).SetLoops(-1, _loopType);
			}
		}
		
		void OnDestroy()
		{
			transform.Kill();
		}

		[Button()]
		void SetupRoller()
		{
			_ease = Ease.Linear;
			_loopType = LoopType.Incremental;
			_relativeTargetRotation = new Vector3(0f, 180f, 0f);
		}

		[Button()]
		void SetupSmasher()
		{
			_ease = Ease.InCubic;
			_loopType = LoopType.Yoyo;
			_relativeTargetRotation = new Vector3(0f, 0f, 90f);
		}
	}
}
