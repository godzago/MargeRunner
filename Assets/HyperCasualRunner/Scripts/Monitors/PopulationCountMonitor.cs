using DG.Tweening;
using HyperCasualRunner.PopulationManagers;
using TMPro;
using UnityEngine;

namespace HyperCasualRunner.Monitors
{
    /// <summary>
    /// It listens for the PopulationChanged event and shows updated value on the TextMeshProUGUI.
    /// </summary>
    public class PopulationCountMonitor : MonoBehaviour
    {
        [SerializeField] PopulationManagerBase _populationManagerBase;
        [SerializeField] TextMeshProUGUI _monitorText;

        Tween _valueSmoothingTween;
        int _previousCount;

        void OnEnable()
        {
            _populationManagerBase.PopulationChanged += PopulationManagerBase_PopulationChanged;
        }

        void OnDisable()
        {
            if (_populationManagerBase)
            {
                _populationManagerBase.PopulationChanged -= PopulationManagerBase_PopulationChanged;
            }
        }

        void PopulationManagerBase_PopulationChanged(int count)
        {
            UpdateText(count);
        }

        void UpdateText(int count)
        {
            int previousCount = _previousCount;
            _valueSmoothingTween.Kill();
            _valueSmoothingTween = DOVirtual.Int(previousCount, count, 0.5f, SetMonitorText);
        }

        void SetMonitorText(int count)
        {
            _previousCount = count;
            _monitorText.text = count.ToString();
        }
    }
}