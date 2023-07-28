using HyperCasualRunner.PopulationManagers;

namespace HyperCasualRunner.Interfaces
{
	/// <summary>
	/// Every GenericModifier uses this for getting the required T type. For example TransformationModifier get TransformationModifiable via this interface. Player script initializes all the IInitializables.
	/// </summary>
	public interface IInitializable
	{
		void Initialize(PopulationManagerBase populationManagerBase);
	}
}
