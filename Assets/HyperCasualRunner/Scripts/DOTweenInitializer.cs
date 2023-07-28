using DG.Tweening;
using UnityEngine;

namespace HyperCasualRunner
{
	/// <summary>
	/// Use this when you plan to have huge amounts of PopulatedEntities. Numbers you need to provide depends on the game but as a rule of thumb, when you play the game, if you are seeing a log message
	/// from DOTween which says increase your tween capacity, you can increase them in here. You will need that especially when you want to create a Master Of Counts game.
	/// </summary>
	public class DOTweenInitializer : MonoBehaviour
	{
		[SerializeField] int _maxTweenerCapacity = 200;
		[SerializeField] int _maxSequenceCapacity = 50;
		
		void Awake()
		{
			DOTween.SetTweensCapacity(_maxTweenerCapacity, _maxSequenceCapacity);
		}
	}
}
