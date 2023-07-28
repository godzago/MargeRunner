using HyperCasualRunner.Interfaces;
using HyperCasualRunner.PopulationManagers;
using UnityEngine;

namespace HyperCasualRunner.GenericModifiers
{
    /// <summary>
    /// It stores array of T in the beginning of the game by looping every PopulatedEntity and use GetComponent on them.
    /// So derived classes can use it for custom effects. You can see examples in AnimationModifier or TransformationModifier.
    /// </summary>
    /// <typeparam name="T">Custom type</typeparam>
    public abstract class GenericModifier<T> : MonoBehaviour, IInitializable where T : Behaviour
    {
        protected T[] EffectReceivers;

        public virtual void Initialize(PopulationManagerBase populationManagerBase)
        {
            enabled = true;

            var result = new T[populationManagerBase.MaxPopulationCount];
            int resultIndex = 0;
            for (int i = 0; i < populationManagerBase.ShownPopulatedEntities.Count; i++)
            {
                PopulatedEntity.PopulatedEntity shownPopulatedEntity = populationManagerBase.ShownPopulatedEntities[i];
                result[resultIndex] = shownPopulatedEntity.GetComponent<T>();
                resultIndex++;
            }

            for (int i = 0; i < populationManagerBase.HiddenPopulatedEntities.Count; i++)
            {
                PopulatedEntity.PopulatedEntity hiddenPopulatedEntity = populationManagerBase.HiddenPopulatedEntities[i];
                result[resultIndex] = hiddenPopulatedEntity.GetComponent<T>();
                resultIndex++;
            }

            EffectReceivers = result;
        }
    }
}