namespace VEStarmapEditor.ScriptStuff
{
	using System.Diagnostics;

	using Jurassic.Library;

	using VEStarmapEditor.Classes;
	using VEStarmapEditor.Primitives;

	/// <summary>
	/// </summary>
	public class ScopeSpawn : CBaseScope
	{
		#region Public Methods and Operators

		/// <summary>
		///     sets starting coordinates id
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[JSFunction]
		public void SetCoordinates(double x, double y)
		{
			//SpawnData.Instance.Coord = new Vector2((float)x, (float)y);
		}

		/// <summary>
		///     sets starting system id
		/// </summary>
		/// <param name="id"></param>
		[JSFunction]
		public void SetSystemID(int id)
		{
			StarSystem sys = Generator.Stars[(uint)id];
			Debug.WriteLine("Starting system was picked: " + sys.Name);
		}

		#endregion
	}
}