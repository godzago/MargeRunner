using HyperCasualRunner.Interfaces;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.Modifiables
{
	/// <summary>
	/// Use when you plan to use Animation instead of Animator.
	/// </summary>
	public class AnimationModifiable : MonoBehaviour
	{
		[SerializeField, Required, Tooltip("Animation component that is used on start.")] Animation _defaultAnimation;

		ITransformator _transformator;
		Animation _activeAnimation;
		string _playingAnimationName;

		public void Initialize()
		{
			_activeAnimation = _defaultAnimation;
			_transformator = GetComponent<ITransformator>();
			if (_transformator != null)
				_transformator.Transformed += Transformator_Transformed;
		}

		void OnEnable()
		{
			if (_playingAnimationName != null)
			{
				Play(_playingAnimationName);
			}
		}

		void OnDestroy()
		{
			if (_transformator != null)
				_transformator.Transformed -= Transformator_Transformed;
		}

		/// <summary>
		/// Pass the name of the animation and it plays those. 
		/// </summary>
		/// <param name="animationName"></param>
		public void Play(string animationName)
		{
			_activeAnimation.Play(animationName);
			_playingAnimationName = animationName;
		}
		
		void Transformator_Transformed(GameObject obj)
		{
			_activeAnimation = obj.GetComponent<Animation>();
			if (_playingAnimationName != null)
			{
				Play(_playingAnimationName);
			}
		}
	}
}
