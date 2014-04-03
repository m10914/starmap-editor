namespace VEStarmapEditor.ScriptStuff
{
	using VEStarmapEditor.Classes;

	public class ScopeStarSystem : CSmallScope
	{
		#region Fields

		public double coord_x;

		public double coord_y;

		public string name;

		public string faction;

		#endregion

		#region Constructors and Destructors

		public ScopeStarSystem(StarSystem parentSystem)
		{
			this.coord_x = parentSystem.CoordX;
			this.coord_y = parentSystem.CoordY;
			this.name = parentSystem.Name;
			this.faction = parentSystem.Faction;
		}

		#endregion
	}
}