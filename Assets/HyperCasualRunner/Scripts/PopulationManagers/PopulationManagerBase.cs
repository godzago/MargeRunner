using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace HyperCasualRunner.PopulationManagers
{
    /// <summary>
    /// Base class for creating custom PopulationManagers.
    /// </summary>
    public abstract class PopulationManagerBase : MonoBehaviour
    {
        [SerializeField, Range(1, 500), Tooltip("This limit can't be exceedable later on, so give it a higher number")] 
        protected int maxPopulationCount = 20;
        [SerializeField, Tooltip("Prefab to use for populated entities")] 
        protected PopulatedEntity.PopulatedEntity populatedEntityPrefab;
        [SerializeField] bool _shouldSpawnEntitiesAsChild = true;
        [SerializeField, Required(), ShowIf(nameof(_shouldSpawnEntitiesAsChild)), Tooltip("Populated entities will be spawned as a child of this game object")] 
        protected Transform instantiateRoot;

        [SerializeField] protected UnityEvent OnLastEntityDied;

        List<PopulatedEntity.PopulatedEntity> _shownPopulatedEntities = new List<PopulatedEntity.PopulatedEntity>();
        List<PopulatedEntity.PopulatedEntity> _hiddenPopulatedEntities = new List<PopulatedEntity.PopulatedEntity>();

        public int MaxPopulationCount => maxPopulationCount;

        public List<PopulatedEntity.PopulatedEntity> ShownPopulatedEntities 
        { 
            get => _shownPopulatedEntities; 
            protected set => _shownPopulatedEntities = value;
        }
        public List<PopulatedEntity.PopulatedEntity> HiddenPopulatedEntities 
        { 
            get => _hiddenPopulatedEntities;
            protected set => _hiddenPopulatedEntities = value;
        }
        
        public Action<int> PopulationChanged;
        public Action<PopulatedEntity.PopulatedEntity> PopulatedEntityEnabled;
        public Action<PopulatedEntity.PopulatedEntity> PopulatedEntityDisabled;

        public virtual void Initialize()
        {
            if (_shouldSpawnEntitiesAsChild)
            {
                for (int i = 0; i < maxPopulationCount; i++)
                {
                    PopulatedEntity.PopulatedEntity instantiated = Instantiate(populatedEntityPrefab, instantiateRoot);
                    instantiated.Initialize(this);
                    _hiddenPopulatedEntities.Add(instantiated);
                }        
            }
            else
            {
                for (int i = 0; i < maxPopulationCount; i++)
                {
                    PopulatedEntity.PopulatedEntity instantiated = Instantiate(populatedEntityPrefab);
                    instantiated.Initialize(this);
                    _hiddenPopulatedEntities.Add(instantiated);
                }
            }
        }

        /// <summary>
        /// Create new populated entities by adding specified amount.
        /// </summary>
        /// <param name="amount">How many populated entities should be added.</param>
        public void AddPopulation(int amount)
        {
            int absoluteAmount = Mathf.Abs(amount);
            if (Mathf.Sign(amount) > 0)
            {
                PopulateMultiple(absoluteAmount);
            }
            else
            {
                DepopulateMultiple(absoluteAmount);
            }
        }
        
        /// <summary>
        /// Create new populated entities by multiplying shown populated entities.
        /// </summary>
        /// <param name="ratio">Ratio to multiply.</param>
        public void MultiplyPopulation(float ratio)
        {
            int multipliedCount = Mathf.RoundToInt(_shownPopulatedEntities.Count * ratio);
            int deltaAbsolute = Mathf.Abs(multipliedCount - _shownPopulatedEntities.Count);
            if (ratio > 1)
            {
                PopulateMultiple(deltaAbsolute);
            }
            else
            {
                DepopulateMultiple(deltaAbsolute);
            }
        }

        /// <summary>
        /// Depopulates specific PopulatedEntity instead of random one. Useful when one takes a hit and you want to kill it.
        /// </summary>
        /// <param name="populatedEntity"></param>
        public abstract void Depopulate(PopulatedEntity.PopulatedEntity populatedEntity);

        /// <summary>
        /// Populates random PopulatedEntity.
        /// </summary>
        protected abstract void Populate();
        
        /// <summary>
        /// Depopulates random PopulatedEntity.
        /// </summary>
        protected abstract void Depopulate();
        
        protected virtual void OnPopulationChanged()
        {
            PopulationChanged?.Invoke(_shownPopulatedEntities.Count);
            if (_shownPopulatedEntities.Count == 0)
            {
                OnLastEntityDied.Invoke();
            }
        }

        void PopulateMultiple(int amount)
        {
            if (_shownPopulatedEntities.Count >= maxPopulationCount)
            {
                return;
            }
            
            if (_shownPopulatedEntities.Count + amount > maxPopulationCount)
            {
                amount = maxPopulationCount - _shownPopulatedEntities.Count;
            }

            for (int i = 0; i < amount; i++)
            {
                Populate();
            }
            OnPopulationChanged();
        }

        void DepopulateMultiple(int amount)
        {
            if (_shownPopulatedEntities.Count == 0)
            {
                return;
            }

            if (_shownPopulatedEntities.Count - amount < 0)
            {
                amount = _shownPopulatedEntities.Count;
            }

            for (int i = 0; i < amount; i++)
            {
                Depopulate();
            }
            OnPopulationChanged();
        }

        [Button()]
        void PopulateOne()
        {
            AddPopulation(1);
        }
        
        [Button()]
        void DepopulateOne()
        {
            AddPopulation(-1);
        }
        
        [Button()]
        void PopulateTen()
        {
            AddPopulation(10);
        }
        
        [Button()]
        void DepopulateTen()
        {
            AddPopulation(-10);
        }
    }
}