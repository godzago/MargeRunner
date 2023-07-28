using HyperCasualRunner.Modifiables;
using HyperCasualRunner.PopulationManagers;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.GenericModifiers
{
    /// <summary>
    /// Modifies all the AnimatorModifiables. It uses common Modifier-Modifiable pattern, so it acts on top of every Modifiable object.
    /// </summary>
    public class AnimatorModifier : GenericModifier<AnimatorModifiable>
    {
        [SerializeField, Tooltip("Name of the parameter in Animator.")] string _moveParameterName;
        [SerializeField, Tooltip("Name of the parameter in Animator.")] string _jumpParameterName;

        int _moveHash;
        int _jumpHash;

        public override void Initialize(PopulationManagerBase populationManagerBase)
        {
            base.Initialize(populationManagerBase);
            
            _moveHash = Animator.StringToHash(_moveParameterName);
            _jumpHash = Animator.StringToHash(_jumpParameterName);

            foreach (AnimatorModifiable animatorController in EffectReceivers)
            {
                animatorController.Initialize();
            }
        }

        public void PlayLocomotion(float movement)
        {
            foreach (AnimatorModifiable animatorController in EffectReceivers)
            {
                animatorController.SetMove(_moveHash, movement);
            }
        }

        [Button]
        public void PlayJump()
        {
            foreach (AnimatorModifiable animatorController in EffectReceivers)
            {
                animatorController.SetTrigger(_jumpHash);
            }
        }
    }
}