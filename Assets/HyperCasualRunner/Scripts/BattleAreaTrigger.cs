using HyperCasualRunner.Interfaces;
using HyperCasualRunner.PopulationManagers;
using UnityEngine;

namespace HyperCasualRunner
{
    /// <summary>
    /// This listens for the trigger and if it interacts with CrowdManager, it spawns Battle Area and starts a battle.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class BattleAreaTrigger : MonoBehaviour
    {
        [SerializeField] BattleArea _battleAreaPrefab;
        [SerializeField] CrowdManager _crowdManager;
        [SerializeField] int _populationCountToAdd;

        BattleArea _battleArea;
        
        void Start()
        {
            _crowdManager.Initialize();
            _crowdManager.AddPopulation(_populationCountToAdd);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CrowdManager playerCrowdManager))
            {
                var interactors = other.GetComponents<IInteractor>();
                if (interactors != null)
                {
                    foreach (IInteractor interactor in interactors)
                    {
                        interactor.OnInteractionBegin();
                    }
                }
                
                _battleArea = Instantiate(_battleAreaPrefab, (transform.position + playerCrowdManager.CrowdOrganizingPoint.position) / 2f, Quaternion.identity);
                _battleArea.BattleFinished += OnBattleFinished;
                _battleArea.StartBattle(playerCrowdManager, _crowdManager, interactors);
            }
        }

        void OnBattleFinished(BattleResult battleResult)
        {
            if (battleResult == BattleResult.Won)
            {
                _battleArea.BattleFinished -= OnBattleFinished;
                Destroy(gameObject);
            }
        }
        
        void OnValidate()
        {
            _populationCountToAdd = Mathf.Clamp(_populationCountToAdd, 0, _crowdManager.MaxPopulationCount);
        }
    }
}