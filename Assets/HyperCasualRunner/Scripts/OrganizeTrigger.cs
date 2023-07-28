using HyperCasualRunner.PopulationManagers;
using UnityEngine;

namespace HyperCasualRunner
{
	/// <summary>
	/// Use this to trigger organizing of CrowdManager manually. It might come handy in situations that I don't know yet!
	/// </summary>
	public class OrganizeTrigger : MonoBehaviour
	{
		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out CrowdManager crowdManager))
			{
				crowdManager.StartOrganizing();
			}
		}
	}
}
