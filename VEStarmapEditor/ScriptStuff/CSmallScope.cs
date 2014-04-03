namespace VEStarmapEditor.ScriptStuff
{

	#region

	using System.Reflection;

	using Jurassic;
	using Jurassic.Library;

	#endregion

	public class CSmallScope
	{
		#region Public Methods and Operators

		/// <summary>
		///     get all fields and push it to new object
		/// </summary>
		/// <param name="engine"></param>
		/// <returns></returns>
		public ObjectInstance Instantiate(ScriptEngine engine)
		{
			ObjectInstance newObj = engine.Object.Construct();

			foreach (FieldInfo fld in this.GetType().GetFields())
			{
				newObj[fld.Name] = fld.GetValue(this);
			}

			return newObj;
		}

		#endregion
	}
	
}