namespace VEStarmapEditor.Primitives
{

	#region

	using System;
	using System.Globalization;

	#endregion

	/// <summary>
	///     The vector 2d.
	/// </summary>
	[Serializable]
	public class Vector3
	{
		#region Static Fields

		/// <summary>
		///     The one.
		/// </summary>
		public static readonly Vector3 One = new Vector3(1, 1, 1);

		/// <summary>
		///     The zero.
		/// </summary>
		public static readonly Vector3 Zero = new Vector3(0, 0, 0);

		#endregion

		#region Fields

		public readonly float X;

		public readonly float Y;

		public readonly float Z;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the <see cref="Vector2" /> class.
		/// </summary>
		/// <param name="x">
		///     The x.
		/// </param>
		/// <param name="y">
		///     The y.
		/// </param>
		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		#endregion

		#region Public Properties

		/// <summary>
		///     Gets the length.
		/// </summary>
		public float Length
		{
			get
			{
				return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
		}

		#endregion

		#region Public Methods and Operators

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Vector3 operator /(Vector3 a, double value)
		{
			return new Vector3((float)(a.X / value), (float)(a.Y / value), (float)(a.Z / value));
		}

		public static Vector3 operator *(Vector3 a, double value)
		{
			return new Vector3((float)(a.X * value), (float)(a.Y * value), (float)(a.Z * value));
		}

		public static Vector3 operator *(double op1, Vector3 op2)
		{
			return new Vector3((float)(op2.X * op1), (float)(op2.Y * op1), (float)(op2.Z * op1));
		}

		public static Vector3 operator *(float op1, Vector3 op2)
		{
			return new Vector3(op2.X * op1, op2.Y * op1, op2.Z * op1);
		}

		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return this.Equals((Vector3)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = this.X.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
				return hashCode;
			}
		}

		/// <summary>
		///     The to string.
		/// </summary>
		/// <returns>
		///     The <see cref="string" />.
		/// </returns>
		public override string ToString()
		{
			return this.X.ToString(CultureInfo.InvariantCulture) + ";" + this.Y.ToString(CultureInfo.InvariantCulture) + ";"
					+ this.Z.ToString(CultureInfo.InvariantCulture);
		}

		#endregion

		#region Methods

		/// <summary>
		///     The equals.
		/// </summary>
		/// <param name="other">
		///     The other.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		protected bool Equals(Vector3 other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z);
		}

		#endregion
	}
	
}