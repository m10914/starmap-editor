namespace VEStarmapEditor.Classes
{
	public enum GenerationStep : byte
	{
		NotReady,
		Ready,
		GeneratingStars,
		GeneratingLinks,
		GeneratingFactions,
		GenerationMisc,
		Finished
	}
}