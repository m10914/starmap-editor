namespace VEStarmapEditor.XmlStuff
{
	using System;
	using System.Globalization;

	using VEStarmapEditor.Primitives;

	public class XmlColor
	{
		#region Static Fields

		public static readonly XmlColor Black = new XmlColor(0, 0, 0);

		public static readonly XmlColor White = new XmlColor(1, 1, 1);

		#endregion

		#region Fields

		public readonly float A;

		public readonly float B;

		public readonly float G;

		public readonly float R;

		#endregion

		#region Constructors and Destructors

		public XmlColor(float r, float g, float b, float a = 1f)
		{
			this.R = r;
			this.G = g;
			this.B = b;
			this.A = a;
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
		public static XmlColor Parse(string value)
		{
			value = value.ToLower();
			string delimiter;
			if (value.Contains(","))
			{
				delimiter = ",";
			}
			else if (value.Contains(";"))
			{
				delimiter = ";";
			}
			else
			{
				delimiter = ".";
			}

			string[] split = value.Split(delimiter.ToCharArray());
			if (split.Length < 3)
			{
				throw new Exception("Incorrect Color data form");
			}

			float r = float.Parse(split[0], CultureInfo.InvariantCulture);
			float g = float.Parse(split[1], CultureInfo.InvariantCulture);
			float b = float.Parse(split[2], CultureInfo.InvariantCulture);
			float a = split.Length < 4 ? 1f : float.Parse(split[3], CultureInfo.InvariantCulture);

			return new XmlColor(r, g, b, a);
		}

		public static XmlColor operator +(XmlColor a, XmlColor b)
		{
			return new XmlColor(a.R + b.R, a.G + b.G, a.B + b.B);
		}

		public static XmlColor operator /(XmlColor a, XmlColor b)
		{
			return new XmlColor(a.R / b.R, a.G / b.G, a.B / b.B);
		}

		public static XmlColor operator *(XmlColor a, XmlColor b)
		{
			return new XmlColor(a.R * b.R, a.G * b.G, a.B * b.B);
		}

		public static XmlColor operator *(double op1, XmlColor op2)
		{
			return new XmlColor((float)(op2.R * op1), (float)(op2.G * op1), (float)(op2.B * op1));
		}

		public static XmlColor operator *(float op1, XmlColor op2)
		{
			return new XmlColor(op2.R * op1, op2.G * op1, op2.B * op1);
		}

		public static XmlColor operator -(XmlColor a, XmlColor b)
		{
			return new XmlColor(a.R - b.R, a.G - b.G, a.B - b.B);
		}

		public override string ToString()
		{
			return string.Format("{0};{1};{2};{3}", this.R, this.G, this.B, this.A);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(this.R, this.G, this.B);
		}

		#endregion
	}
}