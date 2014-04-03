namespace VEStarmapEditor.XmlStuff
{
	#region

	using System.Xml.Linq;

	#endregion

	/// <summary>
	///     The base xml object.
	/// </summary>
	public class BaseXmlObject
	{
		#region Fields

		public readonly string Description;

		public readonly string GameVersion;

		public readonly string ID;

		public readonly bool IsEnabled;

		public readonly string Title;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the <see cref="BaseXmlObject" /> class.
		/// </summary>
		/// <param name="xEl">
		///     The el.
		/// </param>
		public BaseXmlObject(XElement xEl)
		{
			XElement xHeader = xEl.Element("header");
			this.GameVersion = xHeader.Element("game_version").Value;
			this.ID = xHeader.Element("id").Value;
			this.Title = xHeader.Element("title").ToStringWithDefault(string.Empty);
			this.Description = xHeader.Element("description").ToStringWithDefault(string.Empty);
			this.IsEnabled = xHeader.Element("isEnabled").ToBoolWithDefault(true);
		}

		#endregion
	}
	
}