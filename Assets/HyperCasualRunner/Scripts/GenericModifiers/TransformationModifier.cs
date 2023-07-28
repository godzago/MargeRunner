using System;
using HyperCasualRunner.Modifiables;
using HyperCasualRunner.PopulationManagers;
using HyperCasualRunner.ScriptableObjects;
using NaughtyAttributes;
using UnityEngine;

namespace HyperCasualRunner.GenericModifiers
{
    /// <summary>
    /// Modifies all TransformationModifiables. This is useful in situations where you want to transform an object into something else. Like bread to hamburger, old car to new one or bad guy to good guy.
    /// It uses common Modifier-Modifiable pattern, so it acts on top of every Modifiable object.
    /// </summary>
    public class TransformationModifier : GenericModifier<TransformationModifiable>
    {
        [SerializeField, InfoBox("When enabled, it doesn't use any threshold for Month/Year or Experience/Level conversion")] 
        bool _useDirectTransformation;
        [SerializeField, HideIf(nameof(_useDirectTransformation))] 
        IntThreshold _transformationThreshold;
        [SerializeField, Tooltip("Whether it should set the level of the currently picked one to the current level.")] 
        bool _shouldSetCollectedEntityToCurrentLevel;
        [SerializeField, Tooltip("Whether it should reset the levels of the objects we lose (when PopulatedEntity takes hit from obstacle).")] 
        bool _shouldResetLostEntityLevel;
        [SerializeField, Min(0f), Tooltip("Starting level of the modifier. This is useful when you create angel-evil game and the starting threshold is not starting at index 0. You can see the example in Run Of Destiny game.")] 
        int _defaultLevel;

        [ReadOnly, SerializeField, Tooltip("This value is grabbed from the PopulatedEntityPrefab's TransformationModifiable's MaxLevel field. This way we can the _currentLevel to never pass this.")] 
        int _maxLevel;
        [ReadOnly, SerializeField, Tooltip("Current level our modifier is in. This also indicates the level of all the object it controls.")] int _currentLevel;
        [ReadOnly, SerializeField, Tooltip("Current experience our modifier is in. If it pass transformationThreshold, modifier levels up.")] int _currentExperience;

        public event Action<int> ExperienceChanged;
        public event Action<int> LevelChanged;

        public override void Initialize(PopulationManagerBase populationManagerBase)
        {
            base.Initialize(populationManagerBase);
            populationManagerBase.PopulatedEntityEnabled += PopulationManagerBase_PopulatedEntityEnabled;
            populationManagerBase.PopulatedEntityDisabled += PopulationManagerBase_PopulatedEntityDisabled;
            _maxLevel = EffectReceivers[0].MaxLevel - 1;
        }

        void Start()
        {
            foreach (TransformationModifiable effectReceiver in EffectReceivers)
            {
                effectReceiver.DefaultLevel = _defaultLevel;
            }
            
            int experience = _transformationThreshold.GetValue(_defaultLevel);
            if (_useDirectTransformation)
            {
                TransformDirectly(_defaultLevel);
            }
            else
            {
                TransformIndirectly(experience);
            }
        }

        /// <summary>
        /// Like experience points from games, this method transforms entitites indirectly, when they hit certain thresholds
        /// </summary>
        /// <param name="amount"></param>
        public void TransformIndirectly(int amount)
        {
            _currentExperience += amount;
            ExperienceChanged?.Invoke(_currentExperience);
            int newLevel = Mathf.Clamp(_transformationThreshold.GetIndex(_currentExperience), 0, _maxLevel);
            AdjustEffectReceiverLevels(newLevel - _currentLevel);
        }

        /// <summary>
        /// Transforms entities directly by the level given
        /// </summary>
        /// <param name="level">Adds this on top of the current level</param>
        public void TransformDirectly(int level)
        {
            AdjustEffectReceiverLevels(level);
        }

        void PopulationManagerBase_PopulatedEntityEnabled(PopulatedEntity.PopulatedEntity obj)
        {
            if (_shouldSetCollectedEntityToCurrentLevel)
            {
                obj.GetComponent<TransformationModifiable>().SetLevel(_currentLevel);
            }
        }

        void PopulationManagerBase_PopulatedEntityDisabled(PopulatedEntity.PopulatedEntity obj)
        {
            if (_shouldResetLostEntityLevel)
            {
                obj.GetComponent<TransformationModifiable>().SetLevel(_defaultLevel);
            }
        }
        
        void AdjustEffectReceiverLevels(int level)
        {
            int previousLevel = _currentLevel;
            _currentLevel = Mathf.Clamp(level + _currentLevel, 0, _maxLevel);

            if (previousLevel == _currentLevel)
            {
                return;
            }
            
            foreach (TransformationModifiable item in EffectReceivers)
            {
                if (item.gameObject.activeInHierarchy)
                {
                    item.SetLevel(_currentLevel);
                }
            }
            LevelChanged?.Invoke(_currentLevel);
        }

        [Button]
        void IncreaseLevel()
        {
            TransformDirectly(1);
        }
        
        [Button]
        void DecreaseLevel()
        {
            TransformDirectly(-1);
        }
    }
}