namespace VEStarmapEditor.Classes
{
	/// <summary>
	/// </summary>
	public class StarLink
	{
		#region Fields

		public uint ID;

		public StarSystem System1;

		public StarSystem System2;

		public float X1;

		public float X2;

		public float Y1;

		public float Y2;

		#endregion

		#region Constructors and Destructors

		public StarLink(StarSystem system1, StarSystem system2)
		{
			this.ID = GlobalIndexer.New();
			this.System1 = system1;
			this.System2 = system2;

			this.System1.Links.Add(this);
			this.System2.Links.Add(this);

			this.System1.OnChanged += this.SysChanged;
			this.System2.OnChanged += this.SysChanged;
			this.System1.OnDeleted += this.OnDeleted;
			this.System2.OnDeleted += this.OnDeleted;
			this.SysChanged(this.System1);
		}

		#endregion

		#region Delegates

		public delegate void StarLinkChangedDelegate();

		#endregion

		#region Public Events

		public event StarLinkChangedDelegate OnChanged;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// </summary>
		public void Dispose()
		{
			this.System1.OnChanged -= this.SysChanged;
			this.System2.OnChanged -= this.SysChanged;
			this.System1.OnDeleted -= this.OnDeleted;
			this.System2.OnDeleted -= this.OnDeleted;

			this.System1.Links.Remove(this);
			this.System2.Links.Remove(this);

			this.System1 = null;
			this.System2 = null;
		}

		#endregion

		#region Methods

		private void OnDeleted(StarSystem sys)
		{
			this.System1.OnChanged -= this.SysChanged;
			this.System2.OnChanged -= this.SysChanged;
			this.System1.OnDeleted -= this.OnDeleted;
			this.System2.OnDeleted -= this.OnDeleted;
		}

		/// <summary>
		/// </summary>
		/// <param name="sys"></param>
		private void SysChanged(StarSystem sys)
		{
			this.X1 = this.System1.CoordX;
			this.Y1 = this.System1.CoordY;

			this.X2 = this.System2.CoordX;
			this.Y2 = this.System2.CoordY;

			// raise event
			StarLinkChangedDelegate handler = this.OnChanged;
			if (handler != null)
			{
				handler();
			}
		}

		#endregion
	}
}