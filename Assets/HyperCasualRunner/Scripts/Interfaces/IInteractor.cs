namespace HyperCasualRunner.Interfaces
{
	/// <summary>
	/// When you want certain systems to react certain events. For example, Player implements it and when he interacts with something,
	/// it disables RunnerMover until interaction ends. In the case of Masters of Count, it's the battling phase.
	/// </summary>
	public interface IInteractor
	{
		void OnInteractionBegin();
		void OnInteractionEnded();
	}
}
