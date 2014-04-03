namespace VEStarmapEditor.XmlStuff
{
	using System.Linq;
	using System.Xml.Linq;

	/// <summary>
	/// 
	/// </summary>
	public class XmlFaction : BaseXmlObject
	{
		public class XmlRelation
		{
			public string Faction;
			public float Relation;

			public XmlRelation(string faction, float relation)
			{
				this.Faction = faction;
				this.Relation = relation;
			}
		}


		public XmlColor MapColor;

		public string PathToIcon;
		public string PathToSmallIcon;

		public float DistanceFromOriginMin;
		public float DistanceFromOriginMax;

		public int CapitalTechLevel;
		public int CapitalDangerLevel;

		public int StarsCountValue;
		public int StarsCountPercent;


		public float CapitalAreaOfInfluence;

		public XmlRelation[] Relations;


		public XmlFaction(XElement xEl)
			: base(xEl)
		{
			var data = xEl.Element("data");

			MapColor = data.Element("map_color").ToXmlColor();
			PathToIcon = data.Element("icon_big").Value;
			PathToSmallIcon = data.Element("icon_small").Value;

			Relations =
				data.Element("relations")
					.Elements()
					.Select(el => new XmlRelation(el.Element("id").Value, el.Element("value").ToFloat()))
					.ToArray();

			{
				var gen = data.Element("generation");

				StarsCountValue = gen.Element("stars_count_value").ToInt32();
				StarsCountPercent = gen.Element("stars_count_percent").ToInt32();

				DistanceFromOriginMin = gen.Element("distance_from_origin_min").ToFloat();
				DistanceFromOriginMax = gen.Element("distance_from_origin_max").ToFloat();

				CapitalTechLevel = gen.Element("capital_tech_level").ToInt32();
				CapitalDangerLevel = gen.Element("capital_danger_level").ToInt32();
				CapitalAreaOfInfluence = gen.Element("capital_area_of_influence").ToFloat();
			}
		}
	}
}