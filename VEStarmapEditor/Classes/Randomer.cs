namespace VEStarmapEditor.Classes
{
	using System;

	public static class Randomer
	{
		private static Random rand = new Random();

		public static void SetSeed(int seed)
		{
			rand = new Random(seed);
		}

		public static int Int()
		{
			return rand.Next();
		}

		public static int Int(int max)
		{
			return rand.Next(max);
		}

		public static int Int(int min, int max)
		{
			return rand.Next(min, max);
		}

		public static double Double()
		{
			return rand.NextDouble();
		}

		public static double Double(double max)
		{
			return rand.NextDouble() * max;
		}

		public static double Double(double min, double max)
		{
			return rand.NextDouble() * (max - min) + min;
		}
	}
}