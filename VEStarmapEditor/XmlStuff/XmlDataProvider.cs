namespace VEStarmapEditor.XmlStuff
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Windows.Documents;
	using System.Xml.Linq;

	public static class XmlDataProvider
	{
		public static Dictionary<string,XmlFaction> Factions = new Dictionary<string, XmlFaction>();

		/// <summary>
		/// 
		/// </summary>
		static XmlDataProvider()
		{
			// load factions
			DirectoryInfo inf = new DirectoryInfo("factions");
			foreach (FileInfo finf in inf.GetFiles("*.xml"))
			{				
				try
				{
					var filename = finf.Name;
					XDocument doc = XDocument.Parse(File.ReadAllText("factions/" + filename));
					if (doc != null)
					{
						XmlFaction faction = new XmlFaction(doc.Root);
						Factions.Add(faction.ID, faction);
					}
				}
				catch (Exception exc)
				{
					Debug.WriteLine(exc.Message);
				}
			}

		}
	}
}