using HyperCasualRunner.PopulationManagers;
using UnityEngine;

namespace HyperCasualRunner.CollectableEffects
{
    /// <summary>
    /// Create derived type from this class when you want to create new effect when collecting things. PopulationManagerBase has been used for the method parameter so you can do
    /// bunch of different things on it. If the PopulationManagerBase can't satisfy your custom behaviour needs, you can use GetComponent<T> to grab the custom component and modify that.
    /// </summary>
    [RequireComponent(typeof(Collectable))]
    public abstract class CollectableEffectBase : MonoBehaviour
    {
        public abstract void ApplyEffect(PopulationManagerBase populationManager);
    }
}
