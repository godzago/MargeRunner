using System;
using HyperCasualRunner.CollectableEffects;
using HyperCasualRunner.PopulationManagers;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner
{
    /// <summary>
    /// It gathers all possible CollectableEffects and applies them when PopulationManagerBase contacts with Collectable's collider. You can use multiple effects
    /// and all of them would be applied to the PopulationManagerBase.
    /// </summary>
    public class Collectable : MonoBehaviour
    {
        [InfoBox("This component centralizes collectableEffects by binding all the effects on the same gameObject")]
        [SerializeField] CollectableEffectBase[] _collectableEffects;
        [SerializeField] GameObject _visuals;
        [SerializeField] bool _particleFeedbackEnabled;
        [ShowIf(nameof(_particleFeedbackEnabled)), Required("you need to assign a particle if you enable Particle Feedback Enabled")]
        [SerializeField] ParticleSystem _collectingParticleFeedback;
        
        [ReadOnly] public bool IsCollected;

        public event Action<Collectable> Collected;

        void OnTriggerEnter(Collider other)
        {
            if (IsCollected) return;

            if (other.TryGetComponent(out PopulationManagerBase manager))
            {
                ApplyCollectEffects(manager);
            }
            else if (other.TryGetComponent(out PopulatedEntity.PopulatedEntity entity))
            {
                ApplyCollectEffects(entity.PopulationManagerBase);
            }
            else
            {
                return;
            }

            IsCollected = true;
            Collected?.Invoke(this);
        }

        void ApplyCollectEffects(PopulationManagerBase populationManager)
        {
            foreach (CollectableEffectBase collectableEffectBase in _collectableEffects)
            {
                collectableEffectBase.ApplyEffect(populationManager);
            }

            if (_particleFeedbackEnabled)
            {
                _collectingParticleFeedback.Play();
            }
            
            _visuals.SetActive(false);
            Destroy(gameObject, 5f);
        }

        [Button("Setup Collectable", EButtonEnableMode.Editor)]
        void Reset()
        {
            _collectableEffects = GetComponents<CollectableEffectBase>();
            Collider[] colls = GetComponents<Collider>();
            foreach (var item in colls)
            {
                if (item.isTrigger)
                {
                    return;
                }
            }

            Debug.LogError("You need to have at least one collider with isTrigger enabled!");
        }
    }
}
