using System;
using UnityEngine;

namespace HyperCasualRunner.Interfaces
{
	/// <summary>
	/// It's needed when you want to control bunch of gameObjects under one PopulatedEntity. When active child gameObject changes, with this interface, other classes can know
	/// the active gameObject, so they can change their targets. For example AnimationModifiable can react to this and changes it's target gameObject for animating.
	/// </summary>
	public interface ITransformator
	{
		Action<GameObject> Transformed { get; set; }
	}
}
