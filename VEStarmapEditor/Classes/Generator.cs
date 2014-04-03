namespace VEStarmapEditor.Classes
{
	#region

	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Windows;
	using System.Windows.Media;

	#endregion

	public static class Generator
	{
		#region Static Fields

		public static GenerationStep CurrentStep = GenerationStep.NotReady;

		public static Dictionary<uint, StarLink> Links = new Dictionary<uint, StarLink>();

		public static Dictionary<uint, StarSystem> Stars = new Dictionary<uint, StarSystem>();

		private static int CurrentStarLinking = 0;

		private static int CurrentStarLinkingStage = 0;

		/// <summary>
		///     dictionary capital system -> all other systems
		/// </summary>
		private static Dictionary<GeneratorParams.FactionGenerationParams, List<StarSystem>> Factions =
			new Dictionary<GeneratorParams.FactionGenerationParams, List<StarSystem>>();

		private static FactionsGenerationStage factionsGenStage = FactionsGenerationStage.Ready;

		private static GeneratorParams genParams;

		private static LinksGenerationStage linksGenStage = LinksGenerationStage.Ready;

		#endregion

		#region Delegates

		public delegate void BuildFactionShapes(List<StarSystem> systems, Color color);

		public delegate void GeneratorLinkAddedDelegate(StarLink link);

		public delegate void GeneratorLinkRemovedDelegate(StarLink link);

		public delegate void GeneratorStarAddedDelegate(StarSystem sys);

		public delegate void GeneratorStarRemovedDelegate(StarSystem sys);

		public delegate void LoggerDelegate(string message);

		#endregion

		#region Public Events

		public static event BuildFactionShapes OnBuildFactionShapes;

		public static event GeneratorLinkAddedDelegate OnLinkAdded;
		public static event GeneratorLinkRemovedDelegate OnLinkRemoved;
		public static event LoggerDelegate OnLog;
		public static event GeneratorStarAddedDelegate OnStarAdded;
		public static event GeneratorStarRemovedDelegate OnStarRemoved;

		#endregion

		#region Enums

		public enum FactionsGenerationStage
		{
			Ready,

			PickCapital,

			GrowAreas,

			BuildShapes,

			Finished
		}

		public enum LinksGenerationStage
		{
			Ready,

			CloseFour,

			ClastersUnite,

			FinalCheckups,

			Finished
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// </summary>
		public static void AddLink(uint systemID1, uint systemID2)
		{
			AddLink(Stars[systemID1], Stars[systemID2]);
		}

		/// <summary>
		/// </summary>
		public static void AddLink(StarSystem sys1, StarSystem sys2, bool bProximityCheck = true)
		{
			// first check - duplication
			foreach (StarLink lnk in Links.Values)
			{
				if ((lnk.System1 == sys1 && lnk.System2 == sys2) || (lnk.System1 == sys2 && lnk.System2 == sys1))
				{
					//Log("Error: duplicated link. Removing...");
					return;
				}
			}

			// second check - proximity to another star
			if (bProximityCheck)
			{
				foreach (StarSystem star in Stars.Values)
				{
					if (star == sys1 || star == sys2)
					{
						continue;
					}
					double dist = star.DistanceToLink(sys1.CoordX, sys1.CoordY, sys2.CoordX, sys2.CoordY);
					if (dist <= genParams.DistanceBetweenStarAndLink)
					{
						Log(
							"Error: link is too close to star " + star.Name + "(" + dist + " of " + genParams.DistanceBetweenStarAndLink
							+ "). Removing...");
						return;
					}
				}
			}

			// third check - intersections
			foreach (StarLink lnk in Links.Values)
			{
				if (lnk.System1 == sys1 || lnk.System2 == sys2 || lnk.System1 == sys2 || lnk.System2 == sys1)
				{
					continue;
				}
				if (MathHelper.HasIntersection(sys1.CoordX, sys1.CoordY, sys2.CoordX, sys2.CoordY, lnk.X1, lnk.Y1, lnk.X2, lnk.Y2))
				{
					Log("Error: intersection link. Removing...");
					return;
				}
			}

			// forth check - excessive links
			{
				if (sys1.Links.Count > 4 || sys2.Links.Count > 4)
				{
					return;
				}
			}

			StarLink link = new StarLink(sys1, sys2);
			Links.Add(link.ID, link);

			GeneratorLinkAddedDelegate handler = OnLinkAdded;
			if (handler != null)
			{
				handler(link);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static uint AddStar(float x, float y)
		{
			StarSystem sys = new StarSystem(RandomName(), x, y);

			if (!Stars.ContainsKey(sys.ID))
			{
				Stars.Add(sys.ID, sys);
			}
			else
			{
				Debug.WriteLine("HUGE ERROR: Star with this id already present!");
				if (Debugger.IsAttached)
				{
					Debugger.Break();
				}
			}

			//event
			GeneratorStarAddedDelegate handler = OnStarAdded;
			if (handler != null)
			{
				handler(sys);
			}

			return sys.ID;
		}

		/// <summary>
		/// </summary>
		public static void Clear()
		{
			foreach (StarLink lnk in Links.Values.ToArray())
			{
				RemoveLink(lnk);
			}
			foreach (StarSystem star in Stars.Values.ToArray())
			{
				RemoveStar(star);
			}

			Links.Clear();
			Stars.Clear();

			ResetGenerationStages();
		}

		/// <summary>
		/// </summary>
		public static void ClustersLink(List<StarSystem> clust1, List<StarSystem> clust2)
		{
			StarSystem candidate1 = null;
			StarSystem candidate2 = null;
			double minDist = double.MaxValue;

			foreach (StarSystem star in clust1)
			{
				foreach (StarSystem star2 in clust2)
				{
					double ndist = star.DistanceTo(star2);
					if (ndist < minDist)
					{
						candidate1 = star;
						candidate2 = star2;
						minDist = ndist;
					}
				}
			}

			AddLink(candidate1, candidate2, false);
		}

		/// <summary>
		/// </summary>
		public static void GenerateRandomSystemStep()
		{
			if (CurrentStep == GenerationStep.NotReady)
			{
				Log("Error: GenerateRandomSystemStep: Cannot generate, generationg params arre not specified");
				return;
			}

			//first stars
			if (CurrentStep == GenerationStep.Ready)
			{
				CurrentStep = GenerationStep.GeneratingStars;
				Log("Generating stars");
			}
			else if (CurrentStep == GenerationStep.GeneratingStars)
			{
				GenerationStepGeneratingStars();
			}
				//then links
			else if (CurrentStep == GenerationStep.GeneratingLinks)
			{
				GenerationStepGeneratingLinks();
			}
				//then factions
			else if (CurrentStep == GenerationStep.GeneratingFactions)
			{
				GenerationStepGenerateFactions();
			}
				//then misc
			else if (CurrentStep == GenerationStep.GenerationMisc)
			{
				GenerationStepGenerateMisc();
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="systems"></param>
		/// <returns></returns>
		public static List<List<StarSystem>> GetClosedCircuitsForSystems(List<StarSystem> systems)
		{
			// first check - remove from systems array all dead ends
			List<StarSystem> sms = systems.ToList();
			for (int i = 0; i < sms.Count; i++)
			{
				int linkscnt = 0;
				sms[i].Links.ForEach(
					el =>
					{
						StarSystem linkedSys = el.System1 == sms[i] ? el.System2 : el.System1;
						if (sms.Contains(linkedSys))
						{
							linkscnt++;
						}
					});
				if (linkscnt < 2)
				{
					Debug.WriteLine(sms[i].Name + ": " + linkscnt + " links");
					sms.RemoveAt(i);
					i = 0;
				}
			}

			Debug.WriteLine("was " + systems.Count + ", now " + sms.Count);
			foreach (StarSystem ss in systems)
			{
				if (!sms.Contains(ss))
				{
					Debug.WriteLine("deleted " + ss.Name);
				}
			}
			//Debugger.Break();

			// now for each
			Dictionary<uint, uint> circuitsFor = new Dictionary<uint, uint>();
			List<List<StarSystem>> res = new List<List<StarSystem>>();
			List<StarSystem> curpath = new List<StarSystem>();
			foreach (StarSystem sys in sms)
			{
				curpath.Clear();
				curpath.Add(sys);
				GetClosedCircuitsForSystemsRecursive(sys, sms, res, curpath);
			}
			return res;
		}

		/// <summary>
		/// </summary>
		/// <param name="curStar"></param>
		/// <param name="elementAt"></param>
		/// <returns></returns>
		public static int GetJumpsNumberFromSystemToSystem(
			StarSystem curStar,
			StarSystem destStar,
			int maxJumps = int.MaxValue)
		{
			int curindex = int.MaxValue;
			List<StarSystem> respath = new List<StarSystem>();
			List<StarSystem> curpath = new List<StarSystem>();

			curpath.Add(curStar);

			foreach (StarLink lnk in curStar.Links)
			{
				if (lnk.System1 != curStar)
				{
					GetJumpsNumberFromSystemToSystemRecursive(lnk.System1, destStar, ref curindex, curpath, respath, maxJumps);
				}
				if (lnk.System2 != curStar)
				{
					GetJumpsNumberFromSystemToSystemRecursive(lnk.System2, destStar, ref curindex, curpath, respath, maxJumps);
				}
			}

			return curindex;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public static List<List<StarSystem>> GetStarClusters()
		{
			List<List<StarSystem>> res = new List<List<StarSystem>>();
			foreach (StarSystem star in Stars.Values)
			{
				// check if this star is already in a cluster
				bool bFound = false;
				foreach (List<StarSystem> subres in res)
				{
					if (subres.Contains(star))
					{
						bFound = true;
						break;
					}
				}
				if (bFound)
				{
					continue;
				}

				// if not, create new one
				res.Add(CreateClusterForStar(star));
			}

			return res;
		}

		/// <summary>
		///     Returns whether generation is in progress
		/// </summary>
		/// <returns></returns>
		public static bool IsGenerationFinished()
		{
			return (CurrentStep == GenerationStep.Finished || CurrentStep == GenerationStep.Ready
			        || CurrentStep == GenerationStep.Ready);
		}

		/// <summary>
		/// </summary>
		/// <param name="message"></param>
		public static void Log(string message)
		{
			LoggerDelegate handler = OnLog;
			if (handler != null)
			{
				handler(message);
			}
		}

		/// <summary>
		///     generate random name for system
		/// </summary>
		/// <returns></returns>
		public static string RandomName()
		{
			string[] Beginnings = new[] { "", "", "", "", "", "", "", "", "N'" };
			string[] Parts1 = new[] { "For", "Ter", "Mer", "Gat", "Or", "In", "Tes", "Nor", "Bra", "Car", "Ton", "No", "Stro" };
			string[] Parts2 = new[] { "", "", "", "mat", "ra", "cot", "th", "sima", "bata", "der", "on", "port", "garr", "den" };
			string[] Parts3 = new[] { "ma", "ed", "ar", "enn", "car", "den", "", "", "" };
			string[] Endings = new[]
			                   {
				                   " Alpha", " Beta", " Gamma", " Rissa", " Tequa", " Thai", " Cluster", " Then", " Theta",
				                   " Comma", "", "", "", ""
			                   };

			string res = "" + Beginnings[Randomer.Int(0, Beginnings.Length)];
			res += Parts1[Randomer.Int(0, Parts1.Length)];
			res += Parts2[Randomer.Int(0, Parts2.Length)];
			res += Parts3[Randomer.Int(0, Parts3.Length)];
			res += Endings[Randomer.Int(0, Endings.Length)];

			return res;
		}

		/// <summary>
		/// </summary>
		public static void RemoveLink(uint id)
		{
			RemoveLink(Links[id]);
		}

		/// <summary>
		/// </summary>
		/// <param name="link"></param>
		public static void RemoveLink(StarLink link)
		{
			link.Dispose();
			Links.Remove(link.ID);

			// raise event
			GeneratorLinkRemovedDelegate handler = OnLinkRemoved;
			if (handler != null)
			{
				handler(link);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="id"></param>
		public static void RemoveStar(uint id)
		{
			RemoveStar(Stars[id]);
		}

		/// <summary>
		/// </summary>
		/// <param name="sys"></param>
		public static void RemoveStar(StarSystem sys)
		{
			//remove star itself
			sys.Dispose();
			Stars.Remove(sys.ID);

			//event
			GeneratorStarRemovedDelegate handler = OnStarRemoved;
			if (handler != null)
			{
				handler(sys);
			}
		}

		/// <summary>
		/// </summary>
		public static void ResetGenerationStages()
		{
			CurrentStep = GenerationStep.Ready;
			CurrentStarLinking = 0;
			CurrentStarLinkingStage = 0;
			factionsGenStage = FactionsGenerationStage.Ready;
			linksGenStage = LinksGenerationStage.Ready;
		}

		/// <summary>
		/// </summary>
		/// <param name="parms"></param>
		public static void SetGeneratorParams(GeneratorParams parms)
		{
			if (parms != null)
			{
				genParams = parms;
				CurrentStep = GenerationStep.Ready;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// </summary>
		/// <param name="star"></param>
		/// <returns></returns>
		private static List<StarSystem> CreateClusterForStar(StarSystem star)
		{
			List<StarSystem> res = new List<StarSystem>();

			res.Add(star);
			foreach (StarLink lnk in star.Links)
			{
				if (lnk.System1 != star)
				{
					CreateClusterForStarRecursive(lnk.System1, res);
				}
				if (lnk.System2 != star)
				{
					CreateClusterForStarRecursive(lnk.System2, res);
				}
			}

			return res;
		}

		private static void CreateClusterForStarRecursive(StarSystem star, List<StarSystem> res)
		{
			if (res.Contains(star))
			{
				return;
			}
			res.Add(star);

			foreach (StarLink lnk in star.Links)
			{
				if (lnk.System1 != star)
				{
					CreateClusterForStarRecursive(lnk.System1, res);
				}
				if (lnk.System2 != star)
				{
					CreateClusterForStarRecursive(lnk.System2, res);
				}
			}
		}

		/// <summary>
		/// </summary>
		private static void GenerationStepGenerateFactions()
		{
			if (factionsGenStage == FactionsGenerationStage.Ready)
			{
				factionsGenStage = FactionsGenerationStage.PickCapital;
			}
			else if (factionsGenStage == FactionsGenerationStage.PickCapital)
			{
				Factions.Clear();

				//pick capitals
				foreach (GeneratorParams.FactionGenerationParams faction in genParams.FactionParams)
				{
					//find first unfactioned system far away from any other faction
					StarSystem factionMainSystem;
					while (true)
					{
						bool bFailed = false;
						factionMainSystem = Stars.ElementAt(Randomer.Int(Stars.Count)).Value;

						// check if already has faction
						if (factionMainSystem.Faction != null)
						{
							continue;
						}

						// check if far away from closest distance
						foreach (KeyValuePair<GeneratorParams.FactionGenerationParams, List<StarSystem>> capitals in Factions)
						{
							StarSystem capital = capitals.Value.FirstOrDefault();
							int jumps = GetJumpsNumberFromSystemToSystem(factionMainSystem, capital, genParams.MinJumpsBetweenCapitals + 1);
							if (jumps < genParams.MinJumpsBetweenCapitals)
							{
								bFailed = true;
								break;
							}
						}

						// if any fails, go on
						if (!bFailed)
						{
							break;
						}
					}

					//presume we've found candidate for capital
					factionMainSystem.Faction = faction.Name;
					Factions.Add(faction, new List<StarSystem>() { factionMainSystem });
					Log("Captial of " + faction.Name + " is " + factionMainSystem.Name);
				}

				factionsGenStage = FactionsGenerationStage.GrowAreas;
			}
			else if (factionsGenStage == FactionsGenerationStage.GrowAreas)
			{
				bool bGenerated = false;
				foreach (KeyValuePair<GeneratorParams.FactionGenerationParams, List<StarSystem>> fact in Factions)
				{
					if (fact.Value.Count < fact.Key.SystemsNumber)
					{
						bGenerated = true;

						//try to generate more systems
						foreach (StarSystem sys in fact.Value.ToArray())
						{
							if (fact.Value.Count >= fact.Key.SystemsNumber)
							{
								break;
							}
							foreach (StarLink link in sys.Links)
							{
								if (link.System1.Faction == null)
								{
									link.System1.Faction = fact.Key.Name;
									fact.Value.Add(link.System1);
								}
								if (link.System2.Faction == null)
								{
									link.System2.Faction = fact.Key.Name;
									fact.Value.Add(link.System2);
								}
							}
						}
					}
				}

				//goto next stage
				if (!bGenerated)
				{
					factionsGenStage = FactionsGenerationStage.BuildShapes;
				}
			}
			else if (factionsGenStage == FactionsGenerationStage.BuildShapes)
			{
				//raise event on building
				BuildFactionShapes handler = OnBuildFactionShapes;
				if (handler != null)
				{
					foreach (KeyValuePair<GeneratorParams.FactionGenerationParams, List<StarSystem>> sys in Factions)
					{
						handler(sys.Value, sys.Key.Color);
					}
				}

				//goto next stage
				factionsGenStage = FactionsGenerationStage.Finished;
			}
			else //if finished
			{
				//goto next step
				Log("Generating misc");
				CurrentStep = GenerationStep.GenerationMisc;
			}
		}

		/// <summary>
		/// </summary>
		private static void GenerationStepGenerateMisc()
		{
			//goto next step
			Log("-= Generation completed!");
			CurrentStep = GenerationStep.Finished;
		}

		/// <summary>
		/// </summary>
		private static void GenerationStepGeneratingLinks()
		{
			if (linksGenStage != LinksGenerationStage.Finished)
			{
				// start
				if (linksGenStage == LinksGenerationStage.Ready)
				{
					linksGenStage = LinksGenerationStage.CloseFour;
					CurrentStarLinking = 0;
				}
					// generate links
				else if (linksGenStage == LinksGenerationStage.CloseFour)
				{
					while (true)
					{
						//get four closest stars, make links
						Dictionary<double, StarSystem> sys = new Dictionary<double, StarSystem>();
						KeyValuePair<uint, StarSystem> curStar = Stars.ElementAt(CurrentStarLinking);
						foreach (KeyValuePair<uint, StarSystem> tarStar in Stars)
						{
							double dist = tarStar.Value.DistanceTo(curStar.Value);
							sys.Add(dist, tarStar.Value);
						}
						IOrderedEnumerable<KeyValuePair<double, StarSystem>> res = sys.OrderBy(el => el.Key);

						int numLinks = Links.Count;
						if (GetJumpsNumberFromSystemToSystem(
							curStar.Value,
							res.ElementAt(CurrentStarLinkingStage + 1).Value,
							genParams.AverageJumpsBetweenSystems + 1) > genParams.AverageJumpsBetweenSystems)
						{
							AddLink(curStar.Value, res.ElementAt(CurrentStarLinkingStage + 1).Value);
						}

						CurrentStarLinking++;

						//goto next step
						if (CurrentStarLinking == Stars.Count - 1)
						{
							if (CurrentStarLinkingStage < genParams.LinkDensity)
							{
								CurrentStarLinking = 0;
								CurrentStarLinkingStage++;
							}
							else
							{
								linksGenStage = LinksGenerationStage.ClastersUnite;
							}
							break;
						}
						//if was added, postpone  for next frame
						if (numLinks < Links.Count)
						{
							break;
						}
					}
				}
					// unite clasters
				else if (linksGenStage == LinksGenerationStage.ClastersUnite)
				{
					List<List<StarSystem>> clusters = GetStarClusters();

					//link first two clusters
					if (clusters.Count > 1)
					{
						ClustersLink(clusters.ElementAt(0), clusters.ElementAt(1));
					}
					else
					{
						linksGenStage = LinksGenerationStage.Finished;
					}
				}
			}
			else
			{
				//goto next step
				Log("Generating factions...");
				CurrentStep = GenerationStep.GeneratingFactions;
			}
		}

		/// <summary>
		/// </summary>
		private static void GenerationStepGeneratingStars()
		{
			if (Stars.Count <= genParams.NumOfStars)
			{
				//looking for coordinate
				const int tries = 100;
				int i = 0;
				for (i = 0; i < tries; i++)
				{
					Vector coord = new Vector(Randomer.Double(5, 95), Randomer.Double(5, 95));
					if ((coord - new Vector(50, 50)).LengthSquared > 2500)
					{
						i--;
						continue;
					}

					bool bFound = false;
					foreach (KeyValuePair<uint, StarSystem> star in Stars)
					{
						if ((coord - new Vector(star.Value.CoordX, star.Value.CoordY)).Length < genParams.DistanceBetweenStars)
						{
							bFound = true;
							break;
						}
					}
					if (!bFound)
					{
						AddStar((float)coord.X, (float)coord.Y);
						break;
					}
				}
				if (i == tries)
				{
					Log("Couldn't find a place for a star in 100 tries. Generating links...");
					CurrentStep = GenerationStep.GeneratingLinks;
				}
			}
			else
			{
				//goto next step
				Log("Generating links...");
				CurrentStep = GenerationStep.GeneratingLinks;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="system"></param>
		/// <param name="systems"></param>
		/// <param name="res"></param>
		/// <param name="curpath"></param>
		/// <returns></returns>
		private static bool GetClosedCircuitsForSystemsRecursive(
			StarSystem system,
			List<StarSystem> systems,
			List<List<StarSystem>> res,
			List<StarSystem> curpath)
		{
			//Debug.WriteLine("entered "+system.Name);
			if (curpath.Count > 10)
			{
				string str = "circuit too long: ";
				foreach (StarSystem ss in curpath)
				{
					str += ss.Name + " > ";
				}
				//Debug.WriteLine(str);
				return true;
			}

			foreach (StarLink link in system.Links)
			{
				StarSystem linkedSystem = link.System1 == system ? link.System2 : link.System1;

				// check if system is available
				if (!systems.Contains(linkedSystem))
				{
					continue;
				}

				// don't go back!
				if (curpath.Count > 1 && linkedSystem == curpath.ElementAt(curpath.Count - 2))
				{
					continue;
				}

				if (curpath.Contains(linkedSystem)) //we found a circuit! congrats!
				{
					List<StarSystem> circuit = new List<StarSystem>();
					int i = 0;
					for (i = 0; i < curpath.Count; i++)
					{
						if (curpath[i] == linkedSystem)
						{
							break;
						}
					}
					for (; i < curpath.Count; i++)
					{
						circuit.Add(curpath[i]);
					}

					bool bBest = true;
					foreach (List<StarSystem> rss in res)
					{
						//if()
					}
					if (bBest)
					{
						//hashes.Add(str);
						res.Add(circuit);

						//TODO: remove
						string debugres = "found circuit: ";
						foreach (StarSystem ss in circuit)
						{
							debugres += ss.Name + " > ";
						}
						Debug.WriteLine(debugres);
					}

					return true;
				}
				else
				{
					//go on searching
					curpath.Add(linkedSystem);

					if (!GetClosedCircuitsForSystemsRecursive(linkedSystem, systems, res, curpath))
					{
						return false;
					}

					//Debug.WriteLine("goin out from " + linkedSystem.Name + " to " + system.Name);
					curpath.Remove(linkedSystem);
				}
			}
			return true;
		}

		/// <summary>
		/// </summary>
		/// <param name="curStar"></param>
		/// <param name="destStar"></param>
		/// <param name="curindex"></param>
		/// <param name="curpath"></param>
		/// <param name="respath"></param>
		private static void GetJumpsNumberFromSystemToSystemRecursive(
			StarSystem curStar,
			StarSystem destStar,
			ref int curindex,
			List<StarSystem> curpath,
			List<StarSystem> respath,
			int maxJumps)
		{
			// already have in this path
			if (curpath.Contains(curStar))
			{
				return;
			}

			// already have in respath, and with better results
			if (respath.Contains(curStar) && respath.IndexOf(curStar) < curpath.Count)
			{
				return;
			}

			curpath.Add(curStar);

			if (curStar == destStar)
			{
				//reached!
				if (curindex > curpath.Count)
				{
					curindex = curpath.Count;
					respath = curpath.ToList();
					curpath.Remove(curStar);
					return;
				}
			}

			if (curpath.Count <= maxJumps + 1)
			{
				foreach (StarLink lnk in curStar.Links)
				{
					if (lnk.System1 != curStar)
					{
						GetJumpsNumberFromSystemToSystemRecursive(lnk.System1, destStar, ref curindex, curpath, respath, maxJumps);
					}
					if (lnk.System2 != curStar)
					{
						GetJumpsNumberFromSystemToSystemRecursive(lnk.System2, destStar, ref curindex, curpath, respath, maxJumps);
					}
				}
			}

			curpath.Remove(curStar);
			return;
		}

		#endregion
	}
}