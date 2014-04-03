namespace VEStarmapEditor.XmlStuff
{
	using System;
	using System.Globalization;

	[Serializable]
	public class XmlVector3
	{
		#region Static Fields

		/// <summary>
		///     The identity.
		/// </summary>
		public static readonly XmlVector3 Identity = new XmlVector3(1, 1, 1);

		/// <summary>
		///     The random.
		/// </summary>
		public static readonly XmlVector3 Random = new XmlVector3(null, null, null);

		/// <summary>
		///     The zero.
		/// </summary>
		public static readonly XmlVector3 Zero = new XmlVector3(0, 0, 0);

		#endregion

		#region Fields

		/// <summary>
		///     The x.
		/// </summary>
		public readonly float? X;

		/// <summary>
		///     The y.
		/// </summary>
		public readonly float? Y;

		/// <summary>
		///     The z.
		/// </summary>
		public readonly float? Z;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the <see cref="XmlVector3" /> class.
		/// </summary>
		/// <param name="x">
		///     The x.
		/// </param>
		/// <param name="y">
		///     The y.
		/// </param>
		/// <param name="z">
		///     The z.
		/// </param>
		public XmlVector3(float? x, float? y, float? z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     The parse.
		/// </summary>
		/// <param name="value">
		///     The value.
		/// </param>
		/// <returns>
		///     The <see cref="XmlVector3" />.
		/// </returns>
		/// <exception cref="Exception">
		/// </exception>
		public static XmlVector3 Parse(string value)
		{
			float? x, y, z;

			value = value.ToLower();

			if (!value.Contains(";"))
			{
				x = ParseValue(value);
				return new XmlVector3(x, x, x);
			}

			string[] split = value.Split(';');
			if (split.Length != 3)
			{
				throw new Exception("Incorrect XmlVector3 data form");
			}

			x = ParseValue(split[0]);
			y = ParseValue(split[1]);
			z = ParseValue(split[2]);

			return new XmlVector3(x, y, z);
		}

		#endregion

		#region Methods

		/// <summary>
		///     The parse value.
		/// </summary>
		/// <param name="value">
		///     The value.
		/// </param>
		/// <returns>
		///     The <see cref="float?" />.
		/// </returns>
		private static float? ParseValue(string value)
		{
			if (value == "random")
			{
				return null;
			}

			if (value.Length == 0)
			{
				return 0;
			}

			return float.Parse(value, CultureInfo.InvariantCulture);
		}

		#endregion
	}
}