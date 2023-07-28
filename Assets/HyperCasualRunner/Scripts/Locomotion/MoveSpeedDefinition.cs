using UnityEngine;

namespace HyperCasualRunner.Locomotion
{
	/// <summary>
	/// This is a small script that alters the RunnerMover object when enabled. Used in the Evolution Of Car game. 
	/// </summary>
	public class MoveSpeedDefinition : MonoBehaviour
	{
		public float MoveSpeed = 5f;

		void OnEnable()
		{
			GetComponentInParent<RunnerMover>().ForwardMoveSpeed = MoveSpeed;
		}
	}
}
