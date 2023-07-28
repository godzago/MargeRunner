using UnityEngine;

namespace HyperCasualRunner.Locomotion
{
    /// <summary>
    /// Use this when you want a simple movement constrain that constrains on X Limit. If you set it to 3, it means object can move in between -3 to 3 on X axis.
    /// </summary>
	[CreateAssetMenu(menuName = "HyperCasualPack/Movement Constrainers/PlainMovementConstrainer", fileName = "PlainMovementConstrainer", order = 0)]
    public class PlainMovementConstrainer : MovementConstrainerBase
	{
        [SerializeField] float _xLimit;

        public override Vector3 GetConstrainedMotion(Vector3 motionVector, Vector3 position)
        {
            if (position.x > _xLimit)
            {
                if (motionVector.x > 0)
                {
                    return new Vector3(0f, motionVector.y, motionVector.z);
                }
            }
            else if (position.x < -_xLimit)
            {
                if (motionVector.x < 0)
                {
                    return new Vector3(0f, motionVector.y, motionVector.z);
                }
            }

            return motionVector;
        }
	}
}
