using System;
using UnityEngine;

namespace HyperCasualRunner.ScriptableObjects
{
	/// <summary>
	/// Small class for distributing the input values that we collect from Joystick
	/// </summary>
	[CreateAssetMenu(menuName = "HyperCasualPack/Channels/Input Channel", fileName = "Input Channel", order = 0)]
	public class InputChannelSO : ScriptableObject
	{
		public event Action<Vector2> JoystickUpdated;
		public event Action PointerDown;
		public event Action PointerUp;
		public event Action Tapped;
		public event Action<bool> SetActiveInput;

		public void OnJoystickUpdated(Vector2 value)
		{
			JoystickUpdated?.Invoke(value);
		}

		public void OnPointerDown()
		{
			PointerDown?.Invoke();
		}

		public void OnPointerUp()
		{
			PointerUp?.Invoke();
		}

		public void OnTapped()
		{
			Tapped?.Invoke();
		}

		public void OnSetActiveInput(bool activeness)
		{
			SetActiveInput?.Invoke(activeness);
		}
	}
}
