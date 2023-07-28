using UnityEngine;

namespace HyperCasualRunner
{
	/// <summary>
	/// Use this to trigger jump for PopulatedEntities.
	/// </summary>
	public class JumpTrigger : MonoBehaviour
	{
		[SerializeField] float _jumpPower = 4f;
		[SerializeField] float _duration = 1.5f;
		
		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out PopulatedEntity.PopulatedEntity populatedEntity))
			{
				populatedEntity.Jump(_jumpPower, _duration);
			}
		}
	}
}
