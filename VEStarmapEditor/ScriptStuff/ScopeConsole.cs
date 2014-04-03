namespace VEStarmapEditor.ScriptStuff
{

	#region

	using System;
	using System.Diagnostics;

	using Jurassic.Library;

	#endregion

	/// <summary>
	///     The console scope.
	/// </summary>
	public class ScopeConsole : CBaseScope
	{
		#region Fields

		private Script _script;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// </summary>
		/// <param name="script"></param>
		public ScopeConsole(Script script)
		{
			this._script = script;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// </summary>
		/// <param name="str"></param>
		[JSFunction]
		public void Print(string str)
		{
			Debug.WriteLine(this._script.ScriptName + ": " + str);
		}

		#endregion
	}
	
}