using DG.Tweening;
using HyperCasualRunner.PopulationManagers;
using HyperCasualRunner.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.PopulatedEntity
{
    /// <summary>
    /// Used by PopulationManagers. It represents a thing that is populatable so we can create new one or lose one while playing game.
    /// But in examples like Car Evolution, you can use one for the rest of the game and use other stuff like transformation.
    /// </summary>
    [DisallowMultipleComponent]
    public class PopulatedEntity : MonoBehaviour
    {
        [SerializeField, Required] Rigidbody _rigidbody;
        [SerializeField, Required, Tooltip("Collider the populated entity uses.")] Collider _collider;
        [SerializeField, Required, Tooltip("GameObject that contains the visuals of the gameObject.")] Transform _visuals;
        [SerializeField] float _visibilityChangeDuration = 0.5f;
        
        [Space(15)]
        [SerializeField] bool _disappearParticleEnabled;
        [ShowIf(nameof(_disappearParticleEnabled)), Required]
        [SerializeField] ParticleSystem _disappearParticle;

        [Space(10)]
        [SerializeField] bool _appearParticleEnabled;
        [ShowIf(nameof(_appearParticleEnabled)), Required]
        [SerializeField] ParticleSystem _appearParticle;

        Tween _scaleTween;
        Tween _jumpTween;
        
        public PopulationManagerBase PopulationManagerBase { get; set; }
        public Transform Visuals => _visuals;

        public void Initialize(PopulationManagerBase manager)
        {
            PopulationManagerBase = manager;
            DisablePhysicsInteraction();
            gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            _scaleTween.Kill();
            _jumpTween.Kill();
        }

        public void Appear()
        {
            _scaleTween.Kill();
            _scaleTween = transform.ShowSmoothly(_visibilityChangeDuration);
            _collider.enabled = true;
            
            if (_appearParticleEnabled)
            {
                _appearParticle.Play();
            }
        }

        /// <summary>
        /// It informs the PopulationManagerBase and if it's stack based manager, it throws collectable populated entities. If it's crowd based, then it disappears.
        /// </summary>
        public void TakeHit()
        {
            PopulationManagerBase.Depopulate(this);
        }

        /// <summary>
        /// Makes this disappear smoothly.
        /// </summary>
        public void Disappear()
        {
            _scaleTween.Kill();
            _scaleTween = transform.HideSmoothly(_visibilityChangeDuration);
            _collider.enabled = false;

            if (_disappearParticleEnabled)
            {
                _disappearParticle.Play();
            }
        }
        
        /// <summary>
        /// Moves this to the target point. It's useful in crowd battling games. 
        /// </summary>
        /// <param name="target">Target point to reach.</param>
        /// <param name="moveSpeed">Move speed to use for reaching that point.</param>
        /// <param name="shouldRotate">Whether this should rotate when moving to target point.</param>
        public void Move(Transform target, float moveSpeed, bool shouldRotate)
        {
            EnablePhysicsInteraction();
            var position = _rigidbody.position;
            Vector3 distance = target.position - position;
            Vector3 force;
            if (distance.sqrMagnitude < 0.04f)
            {
                force = Vector3.zero;
            }
            else
            {
                force = distance.normalized * moveSpeed;
            }
            _rigidbody.velocity = force;
            if (shouldRotate && force.magnitude > 0.2f)
            {
                _visuals.LookAt(position + force, Vector3.up);
            }
        }

        public void DisablePhysicsInteraction()
        {
            _rigidbody.isKinematic = true;
        }

        public void EnablePhysicsInteraction()
        {
            _rigidbody.isKinematic = false;
        }

        public void ResetVisualRotation()
        {
            _visuals.rotation = Quaternion.identity;
        }
        
        /// <summary>
        /// It's useful if you want to create jumping effect.
        /// </summary>
        /// <param name="jumpPower"></param>
        /// <param name="duration"></param>
        public void Jump(float jumpPower, float duration)
        {
            if (_jumpTween?.IsActive() ?? false)
            {
                _jumpTween.Complete();
            }
            _jumpTween = transform.DOLocalJump(transform.localPosition, jumpPower, 1, duration).SetEase(Ease.Linear);
        }
    }
}