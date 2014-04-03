namespace VEStarmapEditor.XmlStuff
{
	using System;
	using System.Globalization;


	[Serializable]
	public class XmlVector2
	{
		#region Static Fields

		public static readonly XmlVector2 Identity = new XmlVector2(1, 1);

		public static readonly XmlVector2 Random = new XmlVector2(null, null);

		public static readonly XmlVector2 Zero = new XmlVector2(0, 0);

		#endregion

		#region Fields

		public readonly float? X;

		public readonly float? Y;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public XmlVector2(float? x, float? y)
		{
			this.X = x;
			this.Y = y;
		}

		#endregion

		#region Public Methods and Operators

		public static XmlVector2 FromAngle(float directionAngle, float distance)
		{
			return new XmlVector2((float)(Math.Cos(directionAngle) * distance), (float)Math.Sin(directionAngle) * distance);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static XmlVector2 Parse(string value)
		{
			float? x, y;

			value = value.ToLower();

			if (!value.Contains(";"))
			{
				x = ParseValue(value);
				return new XmlVector2(x, x);
			}

			string[] split = value.Split(';');
			if (split.Length != 2)
			{
				throw new Exception("Incorrect XmlVector2 data form");
			}

			x = ParseValue(split[0]);
			y = ParseValue(split[1]);

			return new XmlVector2(x, y);
		}

		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
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