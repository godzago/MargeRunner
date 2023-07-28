using HyperCasualRunner.PopulationManagers;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.CollectableEffects
{
    /// <summary>
    /// Use when you want regular collectables. Like gates, or collectable things on the ground.
    /// </summary>
    public class PopulationEffect : CollectableEffectBase
    {
        [InfoBox("This component helps you to create a powerup that increases/decreases population count when interacted")]
        [SerializeField] bool _useMultiply;
        [SerializeField, HideIf(nameof(_useMultiply))] int _increaseAmount;
        [SerializeField, ShowIf(nameof(_useMultiply)), Min(0f)] float _multiplyRatio = 2f;
		
		public override void ApplyEffect(PopulationManagerBase manager)
		{
            if (_useMultiply)
            {
                manager.MultiplyPopulation(_multiplyRatio);
            }
            else
            {
                manager.AddPopulation(_increaseAmount);
            }
        }
	}
}