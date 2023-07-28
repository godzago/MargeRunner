using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualRunner.Modifiables
{
	/// <summary>
	/// Extensible class when you want to trigger things on an interval based.
	/// </summary>
	public class IntervalTriggerModifiable : MonoBehaviour
	{
		public UnityEvent OnTrigger;
		
		public void Trigger()
		{
			OnTrigger.Invoke();
		}
	}
}
