using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualRunner.Pool
{
	public abstract class ObjectPoolerSO<T> : ScriptableObject where T : MonoBehaviour
	{
		[SerializeField] protected int _maxSize;
		[SerializeField] protected T _behaviour;
		protected Queue<T> PooledObjectQueue = new Queue<T>();

		public virtual T TakeFromPool()
		{
			T obj;
			if (PooledObjectQueue.Count > 0)
			{
				obj = PooledObjectQueue.Dequeue();
				obj.gameObject.SetActive(true);
			}
			else
			{
				obj = Instantiate(_behaviour);
			}

			return obj;
		}

		public void PutBackToPool(T t)
		{
			if (PooledObjectQueue.Count > _maxSize)
			{
				Destroy(t.gameObject);
			}
			else
			{
				t.gameObject.SetActive(false);
				PooledObjectQueue.Enqueue(t);
			}
		}
	}
}
