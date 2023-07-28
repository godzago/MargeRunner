using System;
using DG.Tweening;
using HyperCasualRunner.PopulationManagers;
using UnityEngine;

namespace HyperCasualRunner.Locomotion
{
    /// <summary>
    /// Use this when you want to constrain the movement based on the entities you have. It's useful in games like Master Of Counts.
    /// </summary>
    [CreateAssetMenu(menuName = "HyperCasualPack/Movement Constrainers/CrowdMovementConstrainer", fileName = "CrowdMovementConstrainer", order = 0)]
    public class CrowdMovementConstrainer : MovementConstrainerBase
    {
        [SerializeField] float _xLimit;
        
        CrowdManager _crowdManager;
        Tween _delayedCall;
        Bounds _movableBounds;

        public override void Initialize(GameObject runnerGameObject)
        {
            _crowdManager = runnerGameObject.GetComponent<CrowdManager>();
            _crowdManager.PopulationChanged += CrowdManager_PopulationChanged;
        }

        void OnDisable()
        {
            if (_crowdManager)
            {
                _crowdManager.PopulationChanged -= CrowdManager_PopulationChanged;
            }
        }

        public override void OnDestroying()
        {
            _delayedCall.Kill();
        }

        void CrowdManager_PopulationChanged(int amount)
        {
            _delayedCall.Kill();
            _delayedCall = DOVirtual.DelayedCall(0.3f, CalculateBoundaries, false);
        }
        
        void CalculateBoundaries()
        {
            Bounds bounds = new Bounds(_crowdManager.transform.position, Vector3.zero);
            foreach (PopulatedEntity.PopulatedEntity populatedEntity in _crowdManager.ShownPopulatedEntities)
            {
                bounds.Encapsulate(populatedEntity.transform.position);
            }
            _movableBounds = bounds;
        }

        public override Vector3 GetConstrainedMotion(Vector3 motionVector, Vector3 position)
        {
            if (position.x > _xLimit - _movableBounds.extents.x)
            {
                if (motionVector.x > 0)
                {
                    return new Vector3(0f, motionVector.y, motionVector.z);
                }
            }
            else if (position.x < -_xLimit + _movableBounds.extents.x)
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