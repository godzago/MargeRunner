using HyperCasualRunner.Interfaces;
using HyperCasualRunner.Modifiables;
using UnityEngine;

namespace HyperCasualRunner.GenericModifiers
{
	/// <summary>
	/// Modifies all IntervalTriggerModifiables. This triggers their Trigger method on every specified interval.
	/// It uses common Modifier-Modifiable pattern, so it acts on top of every Modifiable object.
	/// </summary>
	public class IntervalTriggerModifier : GenericModifier<IntervalTriggerModifiable>, ITickable
	{
		[SerializeField, Tooltip("Every x second, modifier triggers Trigger method on IntervalTriggerModifiables.")] float _triggerInterval;
		
		float _timer;
		
		public void Tick()
		{
			_timer += Time.deltaTime;

			if (_timer >= _triggerInterval)
			{
				_timer -= _triggerInterval;
				Trigger();
			}
		}
		
		void Trigger()
		{
			foreach (IntervalTriggerModifiable intervalTrigger in EffectReceivers)
			{
				if (intervalTrigger.gameObject.activeInHierarchy)
				{
					intervalTrigger.Trigger();
				}
			}
		}
	}
}