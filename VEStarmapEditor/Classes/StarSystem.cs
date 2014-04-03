namespace VEStarmapEditor.Classes
{
	#region

	using System.Collections.Generic;
	using System.Windows;

	#endregion

	public class StarSystem
	{
		#region Fields

		public uint ID;

		public List<StarLink> Links = new List<StarLink>();

		private float coordX;

		private float coordY;

		private uint dangerLevel = 0;

		private uint enemyLevel;

		private string faction;

		private bool hasStar;

		private string name;

		private uint techLevel = 0;

		#endregion

		#region Constructors and Destructors

		public StarSystem(string name, float coordX, float coordY)
		{
			this.ID = GlobalIndexer.New();

			this.Name = name;
			this.CoordX = coordX;
			this.CoordY = coordY;
		}

		#endregion

		#region Delegates

		public delegate void StarSystemChangedDelegate(StarSystem sys);

		public delegate void StarSystemDeleted(StarSystem sys);

		#endregion

		#region Public Events

		public event StarSystemChangedDelegate OnChanged;
		public event StarSystemDeleted OnDeleted;

		#endregion

		#region Public Properties

		public float CoordX
		{
			get
			{
				return this.coordX;
			}
			set
			{
				this.coordX = value;
				StarSystemChangedDelegate handler = this.OnChanged;
				if (handler != null)
				{
					this.OnChanged(this);
				}
			}
		}

		public float CoordY
		{
			get
			{
				return this.coordY;
			}
			set
			{
				this.coordY = value;
				StarSystemChangedDelegate handler = this.OnChanged;
				if (handler != null)
				{
					this.OnChanged(this);
				}
			}
		}

		public uint DangerLevel
		{
			get
			{
				return this.dangerLevel;
			}
			set
			{
				this.dangerLevel = value;
				StarSystemChangedDelegate handler = this.OnChanged;
				if (handler != null)
				{
					this.OnChanged(this);
				}
			}
		}

		public uint EnemyLevel
		{
			get
			{
				return this.enemyLevel;
			}
			set
			{
				this.enemyLevel = value;
				StarSystemChangedDelegate handler = this.OnChanged;
				if (handler != null)
				{
					this.OnChanged(this);
				}
			}
		}

		public string Faction
		{
			get
			{
				return this.faction;
			}
			set
			{
				this.faction = value;
				StarSystemChangedDelegate handler = this.OnChanged;
				if (handler != null)
				{
					this.OnChanged(this);
				}
			}
		}

		public bool HasStar
		{
			get
			{
				return this.hasStar;
			}
			set
			{
				this.hasStar = value;
				StarSystemChangedDelegate handler = this.OnChanged;
				if (handler != null)
				{
					this.OnChanged(this);
				}
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				StarSystemChangedDelegate handler = this.OnChanged;
				if (handler != null)
				{
					this.OnChanged(this);
				}
			}
		}

		public uint TechLevel
		{
			get
			{
				return this.techLevel;
			}
			set
			{
				this.techLevel = value;
				StarSystemChangedDelegate handler = this.OnChanged;
				if (handler != null)
				{
					this.OnChanged(this);
				}
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// </summary>
		public void Dispose()
		{
			StarSystemDeleted handler = this.OnDeleted;
			if (handler != null)
			{
				handler(this);
			}

			// clear all links
			foreach (StarLink link in this.Links.ToArray())
			{
				Generator.RemoveLink(link);
			}
			this.Links.Clear();

			// clear events
		}

		public void UpdateDataObject()
		{
			StarSystemChangedDelegate handler = this.OnChanged;
			if (handler != null)
			{
				this.OnChanged(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <returns></returns>
		public double DistanceToLink(double x1, double y1, double x2, double y2)
		{
			Vector source = new Vector(this.CoordX, this.CoordY);
			Vector p0 = new Vector(x1,y1);
			Vector p1 = new Vector(x2,y2);
			Vector zero = new Vector(0,0);

			double l2 = (p1 - p0).LengthSquared;
			if (l2 == 0) return (source - p0).Length;

			double t = (source - p0) * (p1 - p0) / l2;
			if (t < 0) return (source - p0).Length;
			else if (t > 1) return (source - p1).Length;

			var proj = p0 + t * (p1-p0);
			return (source - proj).Length;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sys"></param>
		/// <returns></returns>
		public double DistanceTo(StarSystem sys)
		{
			return (new Vector(sys.CoordX, sys.CoordY) - new Vector(this.CoordX, this.CoordY)).Length;
		}

		#endregion
	}
}