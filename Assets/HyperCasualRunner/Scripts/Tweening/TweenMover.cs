using DG.Tweening;
using UnityEngine;

namespace HyperCasualRunner.Tweening
{
	/// <summary>
	/// Simple class for using DOTween to move stuff around.
	/// </summary>
	public class TweenMover : MonoBehaviour
	{
		[SerializeField] Ease _ease = Ease.InOutQuad;
		[SerializeField] LoopType _loopType = LoopType.Yoyo;
		[SerializeField] Vector3 _relativeTargetPoint = new Vector3(-6f, 0f, 0f);
		[SerializeField] float _targetPointArriveDuration = 2f;
		[SerializeField] bool _useDelayedStart = true;
		
		void Start()
		{
			if (_useDelayedStart)
			{
				float rnd = UnityEngine.Random.Range(0f, 2f);
				transform.DOMove(_relativeTargetPoint, _targetPointArriveDuration).SetRelative().SetEase(_ease).SetDelay(rnd).SetLoops(-1, _loopType);
			}
			else
			{
				transform.DOMove(_relativeTargetPoint, _targetPointArriveDuration).SetRelative().SetEase(_ease).SetLoops(-1, _loopType);
			}
		}

		void OnDestroy()
		{
			transform.Kill();
		}

		void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position + _relativeTargetPoint, 0.2f);
		}
	}
}
