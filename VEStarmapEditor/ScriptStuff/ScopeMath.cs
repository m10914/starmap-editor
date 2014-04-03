namespace VEStarmapEditor.ScriptStuff
{

		#region

		using System;

		using Jurassic.Library;

		using Random = System.Random;

		#endregion

		/// <summary>
		///     The ScopeMath implements some complex math functions, which is better to execute by c#, or
		///     impossible to implement on JavaScript without certain restrictions.
		/// </summary>
		public class ScopeMath : CBaseScope
		{
			#region Fields

			/// <summary>
			///     Private object to store c# random instance
			/// </summary>
			private static Random rnd = new Random();

			#endregion

			#region Constructors and Destructors

			/// <summary>
			///     Initializes a new instance of the <see cref="ScopeMath" /> class.
			/// </summary>
			public ScopeMath()
			{
			}

			#endregion

			#region Public Methods and Operators


			public static Random GetGlobalRandom()
			{
				return rnd;
			}

			/// <summary>
			///     Generates random double seed-based value
			/// </summary>
			/// <returns>
			///     The <see cref="double" />.
			/// </returns>
			[JSFunction]
			public double RandDouble()
			{
				return rnd.NextDouble();
			}

			/// <summary>
			///     Generates random int seed-based value
			/// </summary>
			/// <returns>
			///     The <see cref="int" />.
			/// </returns>
			[JSFunction]
			public int RandInt()
			{
				return rnd.Next();
			}

			[JSFunction]
			public int RandRange(int start, int end)
			{
				return rnd.Next(start, end);
			}

			[JSFunction]
			public double RandRangeDouble(double start, double end)
			{
				return rnd.NextDouble() * (end - start) + start;
			}

			/// <summary>
			///     Delete seed value for RandInt RandDouble methods
			/// </summary>
			[JSFunction]
			public void RandSeed()
			{
				rnd = new Random();
			}

			/// <summary>
			///     Initialize seed value for RandInt RandDouble methods
			/// </summary>
			/// <param name="seed">
			///     The seed.
			/// </param>
			[JSFunction]
			public void RandSeed(int seed)
			{
				rnd = new Random(seed);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="val"></param>
			/// <param name="deg"></param>
			/// <returns></returns>
			[JSFunction]
			public double pow(object val, object deg)
			{
				return Math.Pow((double)val, (double)deg);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="vec1"></param>
			/// <param name="vec2"></param>
			/// <returns></returns>
			[JSFunction]
			public double Vector2Length(ObjectInstance vec1, ObjectInstance vec2)
			{
				try
				{
					double x = double.Parse(vec1["x"].ToString());
					double y = double.Parse(vec1["y"].ToString());
					double x2 = double.Parse(vec2["x"].ToString());
					double y2 = double.Parse(vec2["y"].ToString());

					return Math.Sqrt((x - x2) * (x - x2) + (y - y2) * (y - y2));
				}
				catch (Exception)
				{
					//Global.Logger.Write("MathExt.Vector2Length error", LogSeverity.Error);
				}

				return 0;
			}

			#endregion
		}
	
}