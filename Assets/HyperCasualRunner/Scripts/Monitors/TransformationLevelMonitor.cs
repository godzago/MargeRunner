using DG.Tweening;
using HyperCasualRunner.GenericModifiers;
using TMPro;
using UnityEngine;

namespace HyperCasualRunner.Monitors
{
	/// <summary>
	/// It listens for the events ExperienceChanged and LevelChanged from TransformationModifier and shows updated value on the TextMeshProUGUI.
	/// </summary>
	public class TransformationLevelMonitor : MonoBehaviour
	{
		[SerializeField] TransformationModifier _transformationModifier;
		[SerializeField] TextMeshProUGUI _monitorText;
		[SerializeField] bool _monitorExperience;

		Tween _valueSmoothingTween;
		int _previousCount;

		void OnEnable()
		{
			if (_monitorExperience)
			{
				_transformationModifier.ExperienceChanged += TransformationModifier_ExperienceChanged;
			}
			else
			{
				_transformationModifier.LevelChanged += TransformationModifier_LevelChanged;
			}
		}

		void OnDisable()
		{
			if (_monitorExperience)
			{
				_transformationModifier.ExperienceChanged -= TransformationModifier_ExperienceChanged;
			}
			else
			{
				_transformationModifier.LevelChanged -= TransformationModifier_LevelChanged;
			}
		}

		void TransformationModifier_ExperienceChanged(int count)
		{
			UpdateText(count);
		}

		void TransformationModifier_LevelChanged(int count)
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
