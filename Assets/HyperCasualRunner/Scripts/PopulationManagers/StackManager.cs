using HyperCasualRunner.CollectableEffects;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualRunner.PopulationManagers
{
    /// <summary>
    /// It's responsible for managing stacked entities. Example usage can be found in Stack Of Coffees.
    /// </summary>
    public class StackManager : PopulationManagerBase
    {
        [SerializeField] bool _shouldThrowLostCollectable;
        [SerializeField, ShowIf(nameof(_shouldThrowLostCollectable)), Tooltip("Collectable to spawn when populated entity has been lost.")] 
        CollectablePopulationEffect _collectableEntityPrefab;
        
        [HorizontalLine]
        [SerializeField, Tooltip("Distance between each populated entities in the stack.")] 
        float _distanceBetweenCollectables = 1f;
        [SerializeField, Range(0f, 1f), Tooltip("1 is instant, 0 is slow")] 
        float _movementSmoothingSpeed = 0.5f;
        [SerializeField] float _rotationSpeed = 45f;
        [SerializeField] float _firstPickableRotationDecreaser;
        
        [HorizontalLine]
        [SerializeField] bool _shouldPlayParticleOnCollecting;
        [SerializeField] bool _shouldPlayParticleOnLosing;
        [SerializeField, ShowIf(nameof(_shouldPlayParticleOnCollecting))] 
        ParticleSystem _collectablePickingParticleSystem;
        [SerializeField, ShowIf(nameof(_shouldPlayParticleOnLosing))] 
        ParticleSystem _collectableLosingParticleSystem;
        
        Vector3? _lastRelativeXPosition;
        Vector3 _currentVelocity = Vector3.zero;

        void Update()
        {
            MoveAndRotateStackItems();
        }
        
        public override void Depopulate(PopulatedEntity.PopulatedEntity entity)
        {
            int hitIndex = ShownPopulatedEntities.IndexOf(entity);
            HiddenPopulatedEntities.Add(entity);
            ShownPopulatedEntities.RemoveAt(hitIndex);
            entity.Disappear();
            PopulatedEntityDisabled?.Invoke(entity);

            for (int i = hitIndex, count = ShownPopulatedEntities.Count; i < count; i++)
            {
                PopulatedEntity.PopulatedEntity populatedEntity = ShownPopulatedEntities[hitIndex];
                HiddenPopulatedEntities.Add(populatedEntity);
                ShownPopulatedEntities.RemoveAt(hitIndex);
                populatedEntity.Disappear();
                PopulatedEntityDisabled?.Invoke(populatedEntity);
                if (_shouldThrowLostCollectable)
                {
                    ThrowCollectable(populatedEntity.transform.position);
                }
            }
        }
        
        protected override void Populate()
        {
            int hiddenListCount = HiddenPopulatedEntities.Count;
            if (hiddenListCount == 0)
            {
                return;
            }

            PopulatedEntity.PopulatedEntity entity = HiddenPopulatedEntities[hiddenListCount - 1];
            HiddenPopulatedEntities.RemoveAt(hiddenListCount - 1);
            ShownPopulatedEntities.Add(entity);
            entity.Appear();
            entity.transform.position = transform.position;
            if (_shouldPlayParticleOnCollecting)
            {
                _collectablePickingParticleSystem.Play();
            }
        }

        protected override void Depopulate()
        {
            if (ShownPopulatedEntities.Count == 0)
            {
                return;
            }

            int lastIndex = ShownPopulatedEntities.Count - 1;
            PopulatedEntity.PopulatedEntity entity = ShownPopulatedEntities[lastIndex];
            entity.TakeHit();

            if (_shouldPlayParticleOnLosing)
            {
                _collectableLosingParticleSystem.Play();
            }
        }

        void ThrowCollectable(Vector3 position)
        {
            float rnd = UnityEngine.Random.Range(-2f, 2f);
            Transform trans = transform;
            Vector3 rndVector = trans.right * rnd + trans.forward * 8f;
            CollectablePopulationEffect collectablePopulationEffect = Instantiate(_collectableEntityPrefab, position, Quaternion.identity);
            collectablePopulationEffect.Throw(rndVector);
        }

        void MoveAndRotateStackItems()
        {
            if (ShownPopulatedEntities.Count == 0)
            {
                return;
            }

            if (_lastRelativeXPosition.HasValue)
            {
                Transform trans = transform;
                Vector3 position = trans.position;
                Vector3 forward = trans.forward;
                ShownPopulatedEntities[0].transform.position = position + forward * _distanceBetweenCollectables;
                ShownPopulatedEntities[0].transform
                    .LookAt(
                        position + forward *
                        (_distanceBetweenCollectables * _firstPickableRotationDecreaser), trans.up);
            }

            for (int i = 1; i < ShownPopulatedEntities.Count; i++)
            {
                Vector3 currentPos = ShownPopulatedEntities[i].transform.position;
                Vector3 prevPos = ShownPopulatedEntities[i - 1].transform.position +
                                  transform.forward * _distanceBetweenCollectables;
                currentPos = Vector3.SmoothDamp(currentPos, prevPos, ref _currentVelocity,  0.05f - Mathf.Lerp(0.01f, 0.05f, _movementSmoothingSpeed));
                Quaternion rot = Quaternion.RotateTowards(ShownPopulatedEntities[i].transform.rotation,
                    ShownPopulatedEntities[i - 1].transform.rotation, _rotationSpeed * Time.deltaTime);
                ShownPopulatedEntities[i].transform.position = currentPos;
                ShownPopulatedEntities[i].transform.rotation = rot;
            }

            _lastRelativeXPosition = transform.position;
        }
    }
}