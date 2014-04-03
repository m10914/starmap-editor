namespace VEStarmapEditor.Classes
{
	using System.Windows.Media;

	public class GeneratorParams
	{
		public int NumOfStars;

		public double DistanceBetweenStars;

		public double DistanceBetweenStarAndLink;

		public int AverageJumpsBetweenSystems;

		public int LinkDensity;

		public FactionGenerationParams[] FactionParams;

		public int MinJumpsBetweenCapitals;

		/// <summary>
		/// Stores information for generation of factions' areas in galaxy.
		/// </summary>
		public class FactionGenerationParams
		{
			public string Name;
			public Color Color;

			public int SystemsNumber;
			public int SystemsNumberDiviation;

			public FactionGenerationParams(string name, Color color, int systemsNumber, int systemsNumberDiviation)
			{
				this.Name = name;
				this.Color = color;
				this.SystemsNumber = systemsNumber;
				this.SystemsNumberDiviation = systemsNumberDiviation;
			}
		}

	}
}