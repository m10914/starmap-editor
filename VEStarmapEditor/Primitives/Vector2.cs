namespace VEStarmapEditor.Primitives
{

	#region

	using System;


	#endregion

	/// <summary>
	///     The vector 2d.
	/// </summary>
	[Serializable]
	public class Vector2
	{
		#region Static Fields

		/// <summary>
		///     The one.
		/// </summary>
		public static readonly Vector2 One = new Vector2(1, 1);

		/// <summary>
		///     The zero.
		/// </summary>
		public static readonly Vector2 Zero = new Vector2(0, 0);

		#endregion

		#region Fields

		/// <summary>
		///     The x.
		/// </summary>
		public readonly float X;

		/// <summary>
		///     The y.
		/// </summary>
		public readonly float Y;

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
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
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
				return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y);
			}
		}

		#endregion

		#region Public Methods and Operators


		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X + b.X, a.Y + b.Y);
		}

		public static Vector2 operator /(Vector2 a, double value)
		{
			return new Vector2((float)(a.X / value), (float)(a.Y / value));
		}


		public static Vector2 operator *(Vector2 a, double value)
		{
			return new Vector2((float)(a.X * value), (float)(a.Y * value));
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.X - b.X, a.Y - b.Y);
		}

		/// <summary>
		///     The equals.
		/// </summary>
		/// <param name="obj">
		///     The obj.
		/// </param>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
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

			return this.Equals((Vector2)obj);
		}

		/// <summary>
		///     The get hash code.
		/// </summary>
		/// <returns>
		///     The <see cref="int" />.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (this.X.GetHashCode() * 397) ^ this.Y.GetHashCode();
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
			return this.X.ToString("F") + ";" + this.Y.ToString("F");
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
		protected bool Equals(Vector2 other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y);
		}

		#endregion
	}
	
}