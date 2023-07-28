using HyperCasualRunner.GenericModifiers;
using HyperCasualRunner.PopulationManagers;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.CollectableEffects
{
	/// <summary>
	/// Use when you want to transform collector using TransformationModifier. It gets the TransformationModifier by GetComponent<T>.
	/// </summary>
	public class TransformationEffect : CollectableEffectBase
	{
		[SerializeField, Tooltip("If enabled, it directly alters the transformation level of the collector, so it will change immediately. If disabled, then it may or may not be change, which depends on it's experience point.")] 
		bool _useDirectTransformation;
		[SerializeField, ShowIf(nameof(_useDirectTransformation))] int _levelAddAmount;
		[SerializeField, HideIf(nameof(_useDirectTransformation)), InfoBox("Experience is just a generic term for Day/Week or anything else")] int _experienceToAdd;
		
		public override void ApplyEffect(PopulationManagerBase populationManager)
		{
			if (_useDirectTransformation)
			{
				populationManager.GetComponent<TransformationModifier>().TransformDirectly(_levelAddAmount);
			}
			else
			{
				populationManager.GetComponent<TransformationModifier>().TransformIndirectly(_experienceToAdd);
			}
		}
	}
}
