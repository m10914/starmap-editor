namespace VEStarmapEditor.Classes
{
	/// <summary>
	/// global indexer (simplified version)
	/// </summary>
	public static class GlobalIndexer
	{
		private static uint index = 1;

		public static uint New()
		{
			return index++;
		}
	}
}