using UnityEngine;

namespace HyperCasualRunner.PopulatedEntity
{
    /// <summary>
    /// If gameObject contacts with a PopulatedEntity, this kills PopulatedEntity by calling it's TakeHit method.
    /// </summary>
    public class PopulatedEntityKiller : MonoBehaviour
	{
        PopulatedEntity _populatedEntity;

        void Awake()
        {
            _populatedEntity = GetComponent<PopulatedEntity>();
        }

        void OnCollisionEnter(Collision other)
        {
            if (!other.collider.TryGetComponent(out PopulatedEntityKiller _) && other.collider.TryGetComponent(out PopulatedEntity entity))
            {
                entity.TakeHit();
                _populatedEntity.TakeHit();
            }
        }
	}
}
