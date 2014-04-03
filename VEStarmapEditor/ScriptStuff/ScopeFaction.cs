namespace VEStarmapEditor.ScriptStuff
{
	#region

	using VEStarmapEditor.XmlStuff;

	#endregion

	/// <summary>
	/// </summary>
	public class ScopeFaction : CSmallScope
	{
		#region Fields

		public double capital_area_of_influence;

		public int capital_danger_level;

		public int capital_tech_level;

		public double distance_from_origin_max;

		public double distance_from_origin_min;

		public int stars_count_percent;

		public int stars_count_value;

		#endregion

		#region Constructors and Destructors

		public ScopeFaction(XmlFaction xmlFaction)
		{
			this.stars_count_value = xmlFaction.StarsCountValue;
			this.stars_count_percent = xmlFaction.StarsCountPercent;

			this.distance_from_origin_min = xmlFaction.DistanceFromOriginMin;
			this.distance_from_origin_max = xmlFaction.DistanceFromOriginMax;

			this.capital_tech_level = xmlFaction.CapitalTechLevel;
			this.capital_danger_level = xmlFaction.CapitalDangerLevel;
			this.capital_area_of_influence = xmlFaction.CapitalAreaOfInfluence;
		}

		#endregion
	}
}