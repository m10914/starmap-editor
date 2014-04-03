namespace VEStarmapEditor.ScriptStuff
{
	#region

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;


	using Jurassic;
	using Jurassic.Library;
	using Jurassic.Utils;

	using VEStarmapEditor.Classes;
	using VEStarmapEditor.Primitives;

	#endregion

	/// <summary>
	///     Args to be passed to GenerateGalaxy script.
	/// </summary>
	public class GeneratorArgs : CBaseScope
	{
		#region Fields

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// </summary>
		/// <param name="cmd"></param>
		public GeneratorArgs()
		{
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// </summary>
		/// <param name="engine"></param>
		/// <returns></returns>
		public override CBaseScope Compile(ScriptEngine engine)
		{
			base.Compile(engine);

			return this;
		}

		#endregion
	}


	/// <summary>
	///     The scope generator - wrapper for some of worldManager functions,
	///     providing generation capabilities for scripts
	/// </summary>
	public class ScopeGenerator : CBaseScope
	{
		#region Public Methods and Operators


		/// <summary>
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="type"></param>
		/// <param name="scale"></param>
		/// <param name="resQ"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddAsteroid(
			int systemId,
			double coord_x,
			double coord_y,
			string type,
			double scale,
			double resQ,
			object rotation,
			object rotationSpeed)
		{
			return 1;

			throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="Type"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddBase(int systemId, double coord_x, double coord_y, string Type, string Name)
		{
			return 1;

			throw new NotImplementedException();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="system_id"></param>
		/// <param name="xml_id"></param>
		/// <param name="pos_x"></param>
		/// <param name="pos_y"></param>
		/// <param name="Depth"></param>
		/// <param name="rot_x"></param>
		/// <param name="rot_y"></param>
		/// <param name="rot_x_spd"></param>
		/// <param name="rot_y_spd"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddDecoration(int system_id, string xml_id,
			object ipos, double Depth,
			object irot, object irot_speed,
			double scale)
		{
			return 1;

			throw new NotImplementedException();
		}

		/// <summary>
		///     Adds container of specified type
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="typeID"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddContainer(int systemId, double coord_x, double coord_y, string typeID)
		{
			return 1;

			throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="Type"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddJumpgate(int systemId, double coord_x, double coord_y, string Type)
		{
			return 1;

			throw new NotImplementedException();
		}


		/// <summary>
		///     The add npc ship.
		/// </summary>
		/// <exception cref="NotImplementedException">
		/// </exception>
		[JSFunction]
		public int AddNPCShipToSystem(
			string npcId,
			string behavior,
			string xmlType,
			int systemId,
			double coord_x,
			double coord_y)
		{
			return 1;

			throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="seed"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddPlanet(int systemId, double coord_x, double coord_y, string name, int seed)
		{
			return 1;

			throw new NotImplementedException();
		}

		/// <summary>
		///     manual version
		/// </summary>
		[JSFunction]
		public int AddPlanet(
			int systemId,
			double coord_x,
			double coord_y,
			string name,
			string textureID,
			string cloudID,
			double atmosAlpha,
			double planetRotation,
			double atmosRotation,
			double atmosSize,
			double atmosFunction,
			double atmos_color_x,
			double atmos_color_y,
			double atmos_color_z,
			double planetSize)
		{
			return 1;

			throw new NotImplementedException();
		}

		/// <summary>
		///     Adds space object with specified typeID
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="seed"></param>
		/// <param name="typeID"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddSpaceObject(int systemId, double coord_x, double coord_y, string typeID)
		{
			return 1;

			throw new NotImplementedException();
		}

		/// <summary>
		///     Adds space object of random type
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="seed"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddSpaceObject(int systemId, double coord_x, double coord_y)
		{
			return 1;


			throw new NotImplementedException();
		}


		/// <summary>
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="seed"></param>
		/// <param name="heat"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddStar(int systemId, double coord_x, double coord_y, int seed, int heat)
		{
			return 1;

			throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="systemId"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="seed"></param>
		/// <param name="heat"></param>
		/// <param name="typeID"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddStar(int systemId, double coord_x, double coord_y, int seed, int heat, string typeID)
		{
			return 1;

			//throw new NotImplementedException();
		}

		/// <summary>
		///     Adds starsystem to the current universe
		/// </summary>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		[JSFunction]
		public int AddSystem(double coord_x, double coord_y, string name, int type)
		{
			var id = Generator.AddStar((float)coord_x, (float)coord_y);
			Generator.Stars[id].Name = name;

			//Debug.WriteLine("added system "+ id + " " + name);

			return (int)id;

			//throw new NotImplementedException();
		}

		/// <summary>
		///     creates a travel link between two systems
		/// </summary>
		/// <param name="sys1"></param>
		/// <param name="sys2"></param>
		[JSFunction]
		public void AddSystemsLink(int sys1, int sys2)
		{
			Generator.AddLink((uint)sys1, (uint)sys2);

			//Debug.WriteLine("added link " + sys1 + " " + sys2);
			//throw new NotImplementedException();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ship_id"></param>
		/// <param name="base_id"></param>
		[JSFunction]
		public void DockShipToBase(int ship_id, int base_id)
		{
			return;
		}

		/// <summary>
		/// 
		/// </summary>
		[JSFunction]
		public void FixConnectivity()
		{
			List<List<StarSystem>> clusters = Generator.GetStarClusters();

			//link two neighbor clusters
			for (int i = 0; i < clusters.Count-1; i++)
			{
				Generator.ClustersLink(clusters.ElementAt(i), clusters.ElementAt(i+1));
			}
		}


		/// <summary>
		/// 
		/// </summary>
		[JSFunction]
		public ObjectInstance GetSystemByID(int sys_id)
		{
			StarSystem sys;
			if (Generator.Stars.TryGetValue((uint)sys_id, out sys))
			{
				return new ScopeStarSystem(sys).Instantiate(this.context);
			}			

			return null;
		}


		/// <summary>
		/// </summary>
		/// <returns></returns>
		[JSFunction]
		public ArrayInstance GetAllSystems()
		{
			List<object> res =  new List<object>();
			foreach (var ss in Generator.Stars)
			{
				res.Add((int)ss.Value.ID);
			}
			return context.Array.New(res.ToArray());
		}

		/// <summary>
		/// </summary>
		/// <param name="sysID"></param>
		/// <returns></returns>
		[JSFunction]
		public ArrayInstance GetSystemsByDistanceTo(int sysID)
		{
			StarSystem curstar = Generator.Stars[(uint)sysID];

			StarSystem[] sys = Generator.Stars.Values.OrderBy(el => el.DistanceTo(curstar)).ToArray();
			return context.Array.New(sys.Select(el => (object)((int)el.ID)).ToArray()); //double unboxing
		}


		/// <summary>
		/// </summary>
		/// <param name="sysID"></param>
		/// <param name="sysID2"></param>
		/// <returns></returns>
		[JSFunction]
		public int GetJumpsBetweenSystems(int sysID, int sysID2, int maxJumps)
		{
			return Generator.GetJumpsNumberFromSystemToSystem(
				Generator.Stars[(uint)sysID],
				Generator.Stars[(uint)sysID2],
				maxJumps+1);
		}

		/// <summary>
		///     Returns all types of asteroids
		/// </summary>
		/// <returns></returns>
		[JSFunction]
		public ArrayInstance GetAsteroidTypes()
		{
			return context.Array.New();

			//throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="systemID"></param>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <returns></returns>
		[JSFunction]
		public string GetIntersection(int systemID, double coord_x, double coord_y)
		{
			StarSystem sys;
			if (Generator.Stars.TryGetValue((uint)systemID, out sys))
			{
				foreach (var lnk in Generator.Links)
				{
					if (lnk.Value.System1.ID == systemID || lnk.Value.System2.ID == systemID) continue;

					if (MathHelper.HasIntersection(
						sys.CoordX,
						sys.CoordY,
						(float)coord_x,
						(float)coord_y,
						lnk.Value.System1.CoordX,
						lnk.Value.System1.CoordY,
						lnk.Value.System2.CoordX,
						lnk.Value.System2.CoordY))
					{
						return lnk.Value.System1.ID + ";" + lnk.Value.System2.ID;
					}
				}
			}

			return String.Empty;
			//throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="systemID"></param>
		/// <param name="SystemID2"></param>
		/// <returns></returns>
		[JSFunction]
		public string GetIntersection(int systemID, int SystemID2)
		{
			StarSystem sys;
			if (Generator.Stars.TryGetValue((uint)systemID, out sys))
			{
				return GetIntersection(systemID, sys.CoordX, sys.CoordY);
			}

			return String.Empty;
		}

		/// <summary>
		/// </summary>
		/// <param name="sys1"></param>
		/// <returns></returns>
		[JSFunction]
		public ArrayInstance GetLinkedSystems(int sys1)
		{
			List<object> resarr = new List<object>();
 
			StarSystem sys;
			if (Generator.Stars.TryGetValue((uint)sys1, out sys))
			{
				foreach (var lnk in sys.Links)
				{
					var linked = lnk.System1 == sys ? lnk.System2 : lnk.System1;
					resarr.Add((int)linked.ID);
				}
			}

			return context.Array.New(resarr.ToArray());
		}

		/// <summary>
		///     gets distance to closest system
		/// </summary>
		/// <returns></returns>
		[JSFunction]
		public double GetMinDistanceToSystem(double coord_x, double coord_y)
		{
			double minDistance = double.MaxValue;

			foreach (var star in Generator.Stars)
			{
				var distx = star.Value.CoordX - coord_x;
				var disty = star.Value.CoordY - coord_y;

				var dist = distx * distx + disty * disty;
				if (dist < minDistance) minDistance = dist;
			}

			return Math.Sqrt(minDistance);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="coord_x"></param>
		/// <param name="coord_y"></param>
		/// <returns></returns>
		[JSFunction]
		public int GetClosestSystemToPoint(double coord_x, double coord_y)
		{
			double minDistance = double.MaxValue;
			StarSystem sys = null;

			foreach (var star in Generator.Stars)
			{
				var distx = star.Value.CoordX - coord_x;
				var disty = star.Value.CoordY - coord_y;

				var dist = distx * distx + disty * disty;
				if (dist < minDistance)
				{
					minDistance = dist;
					sys = star.Value;
				}
			}

			return (int)(sys == null ? 0 : sys.ID);
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="stars_array"></param>
		/// <returns></returns>
		[JSFunction]
		public ArrayInstance ExpandArea(object stars_array)
		{
			ArrayInstance arr = stars_array as ArrayInstance;
			List<int> indices = new List<int>();
			foreach(var ind in arr.ElementValues) indices.Add(Convert.ToInt32(ind));

			// try to expand every star
			foreach (var ind in indices.ToArray())
			{
				StarSystem starsys = Generator.Stars[(uint)ind];
				foreach (var lnk in starsys.Links)
				{
					var linkedSystem = lnk.System1 == starsys ? lnk.System2 : lnk.System1;
					if (indices.Contains((int)linkedSystem.ID)) continue;
					indices.Add((int)linkedSystem.ID);
				}
			}

			return context.Array.New(indices.Select(el => (object)el).ToArray());
		}


		/// <summary>
		///     Gets number of links
		/// </summary>
		[JSFunction]
		public int GetNumberOfLinks(int SystemID)
		{
			StarSystem sys;
			if (Generator.Stars.TryGetValue((uint)SystemID, out sys))
			{
				return sys.Links.Count;
			}

			return 0;
		}

		/// <summary>
		///     The remove asteroid.
		/// </summary>
		/// <param name="asterID">
		///     The aster id.
		/// </param>
		[JSFunction]
		public void RemoveAsteroid(int asterID)
		{
			return;

			//throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="baseId"></param>
		[JSFunction]
		public void RemoveBase(int baseId)
		{
			return;

			//throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="jagId"></param>
		[JSFunction]
		public void RemoveJumpgate(int jagId)
		{
			return;

			//throw new NotImplementedException();
		}

		/// <summary>
		///     The remove planet.
		/// </summary>
		/// <param name="planetID">
		///     The planet id.
		/// </param>
		[JSFunction]
		public void RemovePlanet(int planetID)
		{
			return;

			//throw new NotImplementedException();
		}




		[JSFunction]
		public void RemoveDecoration(int decorID)
		{
			return;

			//throw new NotImplementedException();
		}

		/// <summary>
		///     The remove star.
		/// </summary>
		/// <param name="starId">
		///     The star id.
		/// </param>
		[JSFunction]
		public void RemoveStar(int starId)
		{
			return;

			//throw new NotImplementedException();
		}

		/// <summary>
		///     removes link between two systems
		/// </summary>
		/// <param name="sys1"></param>
		/// <param name="sys2"></param>
		[JSFunction]
		public void RemoveSystemsLink(int sys1, int sys2)
		{
			foreach (var lnk in Generator.Links)
			{
				if ((lnk.Value.System1.ID == sys1 || lnk.Value.System2.ID == sys1)
				    && (lnk.Value.System1.ID == sys2 || lnk.Value.System2.ID == sys2))
				{
					Generator.RemoveLink(lnk.Value);
					break;
				}
			}

			return;
			//throw new NotImplementedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="NpcID"></param>
		/// <param name="?"></param>
		[JSFunction]
		public void SetNPCAvatar(int NpcShipID, object arr)
		{
			return;
			//throw new NotImplementedException();
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sys_id"></param>
		/// <param name="level"></param>
		[JSFunction]
		public void SetSystemTechLevel(int sys_id, int level)
		{
			StarSystem sys;
			if (Generator.Stars.TryGetValue((uint)sys_id, out sys))
			{
				sys.TechLevel = (uint)level;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sys_id"></param>
		/// <param name="level"></param>
		[JSFunction]
		public void SetSystemDangerLevel(int sys_id, int level)
		{
			StarSystem sys;
			if (Generator.Stars.TryGetValue((uint)sys_id, out sys))
			{
				sys.DangerLevel = (uint)level;
			}
		}


		#endregion

		#region Methods


		private static Vector2 GetVector2(object obj)
		{
			Dictionary<string, PlainObject> plainObjectsProps = new PlainObject(obj).Properties;
			float x = 0;
			float y = 0;

			if (plainObjectsProps.ContainsKey("X"))
				x = (float)Convert.ToDouble(plainObjectsProps["X"].Value);
			else if (plainObjectsProps.ContainsKey("x"))
				x = (float)Convert.ToDouble(plainObjectsProps["x"].Value);
			else
			{
				//Global.Logger.Write("Error: ScopeGenerator::GetVector2 recieved non-vector struct", LogSeverity.Error);
			}

			if (plainObjectsProps.ContainsKey("Y"))
				y = (float)Convert.ToDouble(plainObjectsProps["Y"].Value);
			else if (plainObjectsProps.ContainsKey("y"))
				y = (float)Convert.ToDouble(plainObjectsProps["y"].Value);
			else
			{
				//Global.Logger.Write("Error: ScopeGenerator::GetVector2 recieved non-vector struct", LogSeverity.Error);
			}

			Vector2 rotationVector = new Vector2(x, y);
			return rotationVector;
		}

		private static Vector3 GetVector3(object obj)
		{
			Dictionary<string, PlainObject> plainObjectsProps = new PlainObject(obj).Properties;
			float x = 0;
			float y = 0;
			float z = 0;

			if (plainObjectsProps.ContainsKey("X"))
				x = (float)Convert.ToDouble(plainObjectsProps["X"].Value);
			else if (plainObjectsProps.ContainsKey("x"))
				x = (float)Convert.ToDouble(plainObjectsProps["x"].Value);
			else
			{
				//Global.Logger.Write("Error: ScopeGenerator::GetVector3 recieved non-vector struct", LogSeverity.Error);
			}

			if (plainObjectsProps.ContainsKey("Y"))
				y = (float)Convert.ToDouble(plainObjectsProps["Y"].Value);
			else if (plainObjectsProps.ContainsKey("y"))
				y = (float)Convert.ToDouble(plainObjectsProps["y"].Value);
			else
			{
				//Global.Logger.Write("Error: ScopeGenerator::GetVector3 recieved non-vector struct", LogSeverity.Error);
			}


			if (plainObjectsProps.ContainsKey("Z"))
				y = (float)Convert.ToDouble(plainObjectsProps["Z"].Value);
			else if (plainObjectsProps.ContainsKey("z"))
				y = (float)Convert.ToDouble(plainObjectsProps["z"].Value);
			else
			{
				//Global.Logger.Write("Error: ScopeGenerator::GetVector3 recieved non-vector struct", LogSeverity.Error);
			}

			Vector3 rotationVector = new Vector3(x, y, z);
			return rotationVector;
		}

		#endregion
	}
	
}