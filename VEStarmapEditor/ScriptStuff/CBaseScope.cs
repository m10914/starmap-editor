namespace VEStarmapEditor.ScriptStuff
{

	#region

	using Jurassic;
	using Jurassic.Library;

	#endregion

	/// <summary>
	///     Base class for all big scopes (with methods)
	///     for small scopes (with just a few parameters) CSmallScope is using
	/// </summary>
	public class CBaseScope : ObjectInstance
	{
		#region Fields

		public ScriptEngine context;

		#endregion

		#region Constructors and Destructors

		public CBaseScope()
		{
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     function prepares scope for use in scrips. In overrides we must initialize all
		///     complex public variables!
		/// </summary>
		/// <param name="engine"></param>
		public virtual CBaseScope Compile(ScriptEngine engine)
		{
			this.context = engine;

			this.InitializeObjectInstance(engine); //instantiate object

			this.PopulateFields(); //add fields
			this.PopulateFunctions(); //add functions

			return this;
		}

		public void Decompile()
		{
			this.context = null;
		}

		#endregion
	}
	
}