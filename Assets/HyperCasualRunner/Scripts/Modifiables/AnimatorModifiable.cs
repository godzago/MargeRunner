using HyperCasualRunner.Interfaces;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.Modifiables
{
	/// <summary>
	/// Use when you plan to use Animator instead of Animation.
	/// </summary>
	public class AnimatorModifiable : MonoBehaviour
	{
		[SerializeField, Required] Animator _defaultAnimator;

		ITransformator _transformator;
		Animator _activeAnimator;
		int _moveHash;
		float _moveValue;
		
		public void Initialize()
		{
			_activeAnimator = _defaultAnimator;
			_transformator = GetComponent<ITransformator>();
			if (_transformator != null)
			{
				_transformator.Transformed += Transformator_Transformed;
			}
		}

		void OnEnable()
		{
			if (_moveHash != 0)
			{
				SetMove(_moveHash, _moveValue);
			}
		}

		void OnDestroy()
		{
			if (_transformator != null)
			{
				_transformator.Transformed -= Transformator_Transformed;
			}
		}

		public void SetMove(int id, float value)
		{
			SetFloat(id, value);
			_moveValue = value;
			_moveHash = id;
		}
		
		public void SetFloat(int id, float value)
		{
			_activeAnimator.SetFloat(id, value);
		}

		public void SetTrigger(int id)
		{
			_activeAnimator.SetTrigger(id);
		}

		void Transformator_Transformed(GameObject animatorObject)
		{
			_activeAnimator = animatorObject.GetComponent<Animator>();
			_activeAnimator.SetFloat(_moveHash, _moveValue);
		}
	}
}
