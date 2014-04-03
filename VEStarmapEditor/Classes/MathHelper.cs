namespace VEStarmapEditor.Classes
{
	using System;

	public static class MathHelper
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="x3"></param>
		/// <param name="y3"></param>
		/// <param name="x4"></param>
		/// <param name="y4"></param>
		/// <returns></returns>
		public static bool HasIntersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
			//(x1, y1), (x2, y2) - first segment
			//(x3, y3), (x4, y4) - second segment

			//Quick test
			if (!(Math.Max(x1, x2) >= Math.Min(x3, x4) && Math.Max(x3, x4) >= Math.Min(x1, x2) &&
			   Math.Max(y1, y2) >= Math.Min(y3, y4) && Math.Max(y3, y4) >= Math.Min(y1, y2)))
				return false;//Rectangles has not intersected

			//Full test
			//Vector product of vectors 1_2 and 1_3
			float vector_prod1 = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);
			//Vector product of vectors 1_2 and 1_4
			float vector_prod2 = (x2 - x1) * (y4 - y1) - (x4 - x1) * (y2 - y1);
			if (vector_prod1 * vector_prod2 > 0)
				return false;

			//Vector product of vectors 3_4 and 3_1
			float vector_prod3 = (x4 - x3) * (y1 - y3) - (x1 - x3) * (y4 - y3);
			//Vector product of vectors 3_4 and 3_2
			float vector_prod4 = (x4 - x3) * (y2 - y3) - (x2 - x3) * (y4 - y3);
			if (vector_prod3 * vector_prod4 > 0)
				return false;

			return true;
		}

	}
}