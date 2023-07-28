using UnityEngine;

namespace HyperCasualRunner.ScriptableObjects
{
	/// <summary>
	/// Useful for things like Day/Week/Month/Year or Experience/Level conversions.
	/// </summary>
	[CreateAssetMenu(fileName = "Int Threshold", menuName = "HyperCasualPack/Int Threshold", order = 0)]
	public class IntThreshold : ScriptableObject
	{
		[SerializeField] int[] _value;

		/// <summary>
		/// Get the threshold level by passing threshold value. Think like convert 500 experience to level.
		/// </summary>
		/// <param name="value">Value to pass, like experience point.</param>
		/// <returns></returns>
		public int GetIndex(int value)
		{
			for (int i = 0; i < _value.Length; i++)
			{
				if (value <= _value[i])
				{
					return i;
				}
			}

			return _value.Length;
		}

		/// <summary>
		/// Pass threshold level and it returns threshold value. In other words, it gives experience points when you pass level.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public int GetValue(int index)
		{
			return _value[index];
		}
	}
}
