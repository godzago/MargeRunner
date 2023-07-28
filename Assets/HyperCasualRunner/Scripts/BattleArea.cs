using System;
using DG.Tweening;
using HyperCasualRunner.Interfaces;
using HyperCasualRunner.PopulationManagers;
using HyperCasualRunner.Tweening;
using UnityEngine;

namespace HyperCasualRunner
{
    /// <summary>
    /// Battle Area that is spawned when battle begins. It's been used in Master Of Counts.
    /// </summary>
    public class BattleArea : MonoBehaviour
    {
        [SerializeField] GameObject _visual;
        [SerializeField] float _battlingEntityMoveSpeed = 1.5f;

        CrowdManager _allyCrowdManager;
        CrowdManager _enemyCrowdManager;
        IInteractor[] _interactors;

        public event Action<BattleResult> BattleFinished;
        
        public void StartBattle(CrowdManager allyCrowdManager, CrowdManager enemyCrowdManager, IInteractor[] interactors)
        {
            _allyCrowdManager = allyCrowdManager;
            _enemyCrowdManager = enemyCrowdManager;
            _interactors = interactors;

            RegisterEvents();

            // TODO: change here for more versatile customization solution
            // rotate and play move animation for our characters
            _visual.transform.localPosition = Vector3.up * 10f;
            _visual.transform.localScale = new Vector3(0.1f, 1.5f, 0.1f);
            _visual.transform.MoveLocal(Vector3.zero, 0.4f, Ease.OutBounce).OnComplete(MoveCrowds);
            _visual.transform.Scale(Vector3.one, 0.4f, Ease.OutBounce);
        }

        void MoveCrowds()
        {
            _allyCrowdManager.MoveRotated(transform, _battlingEntityMoveSpeed);
            _enemyCrowdManager.MoveRotated(transform, _battlingEntityMoveSpeed);
        }

        void RegisterEvents()
        {
            _allyCrowdManager.PopulationChanged += AllyCrowdManager_PopulationChanged;
            _enemyCrowdManager.PopulationChanged += EnemyCrowdManager_PopulationChanged;
        }
        
        void UnregisterPopulationChangedEvents()
        {
            _allyCrowdManager.PopulationChanged -= AllyCrowdManager_PopulationChanged;
            _enemyCrowdManager.PopulationChanged -= EnemyCrowdManager_PopulationChanged;
        }

        void AllyCrowdManager_PopulationChanged(int amount)
        {
            if (amount <= 0)
            {
                OnBattleLost();
            }
        }

        void EnemyCrowdManager_PopulationChanged(int amount)
        {
            if (amount <= 0)
            {
                OnBattleWon();
            }
        }

        void OnBattleWon()
        {
            UnregisterPopulationChangedEvents();
            FinishInteractors();
            BattleFinished?.Invoke(BattleResult.Won);
            _allyCrowdManager.StartOrganizing();
            print("battle won");
            Destroy(gameObject);
        }

        void OnBattleLost()
        {
            UnregisterPopulationChangedEvents();
            FinishInteractors();
            BattleFinished?.Invoke(BattleResult.Lost);
            _enemyCrowdManager.StartOrganizing();
            print("battle lost");
            Destroy(gameObject);
        }

        void FinishInteractors()
        {
            if (_interactors != null)
            {
                foreach (IInteractor interactor in _interactors)
                {
                    interactor.OnInteractionEnded();
                }
            }
        }
    }
}