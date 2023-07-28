using UnityEngine;

namespace HyperCasualRunner
{
    /// <summary>
    /// If you want to have multiple collectables but one of them can be chosen. Others are disabled.
    /// </summary>
    public class CollectableChoice : MonoBehaviour
    {
        Collectable[] _collectables;

        void Awake()
        {
            _collectables = GetComponentsInChildren<Collectable>();
        }

        void OnEnable()
        {
            foreach (Collectable collectable in _collectables)
            {
                collectable.Collected += Collectable_Collected;
            }
        }

        void OnDisable()
        {
            foreach (Collectable collectable in _collectables)
            {
                collectable.Collected -= Collectable_Collected;
            }
        }
        
        void Collectable_Collected(Collectable obj)
        {
            foreach (Collectable collectable in _collectables)
            {
                if (collectable == obj)
                {
                    collectable.gameObject.SetActive(false);
                }

                collectable.IsCollected = true;
            }
        }
    }
}