namespace VEStarmapEditor.ScriptStuff
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Jurassic.Library;

	using VEStarmapEditor.Classes;
	using VEStarmapEditor.XmlStuff;

	/// <summary>
	///     Governs factions, relations, etc
	/// </summary>
	public class ScopeRelations : CBaseScope
	{
		public static Dictionary<int, string> SystemsFactions = new Dictionary<int, string>();
		public static Dictionary<string, int> FactionCapitals = new Dictionary<string, int>();



		public static void Clear()
		{
			SystemsFactions.Clear();
			FactionCapitals.Clear();
		}

			#region Public Methods and Operators

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[JSFunction]
		public ArrayInstance GetFactions()
		{
			return this.context.Array.New(XmlDataProvider.Factions.Select(el => (object)el.Value.ID).ToArray());
		}


		/// <summary>
		/// 
		/// </summary>
		[JSFunction]
		public void ClearAllInfoOnSystems()
		{
			SystemsFactions.Clear();

			foreach (var star in Generator.Stars) star.Value.Faction = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[JSFunction]
		public ObjectInstance GetFactionInfoByID(string id)
		{
			return new ScopeFaction(XmlDataProvider.Factions[id]).Instantiate(this.context);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ship_id"></param>
		/// <returns></returns>
		
		[JSFunction]
		public string GetShipFaction(int ship_id)
		{
			return String.Empty;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="faction"></param>
		/// <returns></returns>
		[JSFunction]
		public int GetFactionCapital(string faction)
		{
			int id;
			if (FactionCapitals.TryGetValue(faction, out id))
			{
				return id;
			}

			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="capital"></param>
		[JSFunction]
		public void SetFactionCapital(string faction, int capital)
		{
			if (FactionCapitals.ContainsKey(faction)) FactionCapitals[faction] = capital;
			else FactionCapitals.Add(faction, capital);
		}



		[JSFunction]
		public void SetBaseFaction(int base_id, string faction)
		{
			return;
		}


		[JSFunction]
		public void SetShipFaction(int ship_id, string faction)
		{
			return;
		}

		/// <summary>
		/// Sets system faction
		/// </summary>
		/// <param name="system_id"></param>
		/// <param name="faction"></param>
		[JSFunction]
		public void SetSystemFaction(int system_id, string faction)
		{
			if (SystemsFactions.ContainsKey(system_id)) SystemsFactions[system_id] = faction;
			else SystemsFactions.Add(system_id,faction);

			Generator.Stars[(uint)system_id].Faction = faction;
		}


		/// <summary>
		/// Gets system faction
		/// </summary>
		/// <param name="system_id"></param>
		/// <param name="faction"></param>
		/// <returns></returns>
		[JSFunction]
		public string GetSystemFaction(int system_id)
		{
			string outsr;
			if (SystemsFactions.TryGetValue(system_id, out outsr))
			{
				return outsr;
			}
			return string.Empty;
		}

		#endregion
	}
}