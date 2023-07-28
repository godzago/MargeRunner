using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.PopulatedEntity
{
	/// <summary>
	/// This reacts to the PopulatedEntity when they are in the trigger area of this. Based on the type it uses, it does different thing, but in the end, this kills the PopulatedEntities.
	/// </summary>
    public class PopulatedEntityObstacle : MonoBehaviour
	{
		enum ObstacleType
		{
			Kill, Abyss
		}
		
		[SerializeField, Tooltip("Kill directly kills it, Abyss creates a falling down effect while killing")] ObstacleType _obstacleType;
		[SerializeField, ShowIf(nameof(_obstacleType), ObstacleType.Kill), Tooltip("Whether it has limited life when killing PopulatedEntities")] 
		bool _hasLimitedLife;
		[SerializeField, ShowIf(nameof(_obstacleType), ObstacleType.Kill), EnableIf(nameof(_hasLimitedLife)), Tooltip("On kill type, it destroys itself after killing this number of PopulatedEntities")] 
		int _hitCountBeforeDestroyed = 5;

		void OnTriggerEnter(Collider coll)
		{
			if (coll.TryGetComponent(out PopulatedEntity entity))
			{
				if (_obstacleType == ObstacleType.Kill)
				{
					FeedKill(entity);
				}
				else if (_obstacleType == ObstacleType.Abyss)
				{
					FeedAbyss(entity);
				}
			}
		}

		void FeedKill(PopulatedEntity entity)
		{
			entity.TakeHit();

			if (_hasLimitedLife)
			{
				_hitCountBeforeDestroyed--;
				if (_hitCountBeforeDestroyed <= 0)
				{
					Destroy(gameObject);
				}	
			}
		}

		void FeedAbyss(PopulatedEntity entity)
		{
			var trans = entity.transform;
			var instantiated = Instantiate(entity.Visuals, trans.position, trans.rotation);
			var gameObj = instantiated.gameObject;
			gameObj.AddComponent<Rigidbody>();
			DOVirtual.DelayedCall(3f, () => Destroy(gameObj), false);

			entity.TakeHit();
		}
	}
}	
