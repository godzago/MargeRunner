using HyperCasualRunner.Modifiables;
using HyperCasualRunner.PopulationManagers;
using UnityEngine;

namespace HyperCasualRunner.GenericModifiers
{
	/// <summary>
	/// Modifies all the AnimationModifiables. It uses common Modifier-Modifiable pattern, so it acts on top of every Modifiable object.
	/// </summary>
	public class AnimationModifier : GenericModifier<AnimationModifiable>
	{
		[SerializeField, Tooltip("Name of the parameter in Animator.")] string _idleAnimationName = "IdleLegacy";
		[SerializeField, Tooltip("Name of the parameter in Animator.")] string _runAnimationName = "RunLegacy";
		[SerializeField, Tooltip("Name of the parameter in Animator.")] string _jumpAnimationName = "JumpLegacy";

		public override void Initialize(PopulationManagerBase populationManagerBase)
		{
			base.Initialize(populationManagerBase);

			foreach (AnimationModifiable animationController in EffectReceivers)
			{
				animationController.Initialize();
			}
		}

		public void PlayLocomotion(float motion)
		{
			if (motion > 0.2f)
			{
				foreach (AnimationModifiable effectReceiver in EffectReceivers)
				{
					effectReceiver.Play(_runAnimationName);
				}
			}
			else
			{
				foreach (AnimationModifiable effectReceiver in EffectReceivers)
				{
					effectReceiver.Play(_idleAnimationName);
				}
			}
		}

		public void Jump()
		{
			foreach (AnimationModifiable effectReceiver in EffectReceivers)
			{
				if (effectReceiver.gameObject.activeInHierarchy)
				{
					effectReceiver.Play(_jumpAnimationName);
				}
			}
		}
	}
}
