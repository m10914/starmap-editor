namespace VEStarmapEditor.ScriptStuff
{

	#region

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Text.RegularExpressions;

	using Jurassic;

	#endregion

	/// <summary>
	///     Base wrapper for scripting context based on Jint
	/// </summary>
	public class Script
	{
		#region Fields

		/// <summary>
		///     Scripting engine context
		/// </summary>
		public ScriptEngine Context;

		public string CurrentFunction;

		private List<string> libsInited = new List<string>();

		private Dictionary<string, FunctionUsageRestriction> restrictions =
			new Dictionary<string, FunctionUsageRestriction>(StringComparer.Ordinal);

		private List<string> scopesInited = new List<string>();

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// </summary>
		/// <param name="scriptName">scriptname (not necessarily unique)</param>
		/// <param name="scriptContent">body of a script</param>
		/// <param name="bDoNotCallOnInit">not to launch automatically OnInit function, or not</param>
		public Script(string scriptName, string scriptContent, bool bDoNotCallOnInit = false)
		{
			this.ScriptName = scriptName;
			this.Context = new ScriptEngine();

			string contents = scriptContent;

			//-------------------
			// STD API EXTENSIONS

			this.Context.SetGlobalValue("MathExt", new ScopeMath().Compile(this.Context));

			//--------------
			// SCOPES & LIBS

			// scopes
			contents = this.InitScopes(contents);

			// libs
			contents = this.InitLibs(contents);

			//remove some bugs
			contents = contents.Replace('\0', ' ');

			// load script itself
			try
			{
				this.Context.Execute(contents);

				// run initizliation
				if (!bDoNotCallOnInit)
				{
					this.Context.CallGlobalFunction("OnInit");
				}
			}
			catch (JavaScriptException exc)
			{
				Debug.WriteLine(
					String.Format(
						"Script error! {0}:{1} : function {2} :: {3}",
						this.ScriptName,
						exc.LineNumber,
						exc.FunctionName,
						exc.Message));
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		///     Gets the script name.
		/// </summary>
		public string ScriptName { get; private set; }

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// </summary>
		/// <param name="ivar"></param>
		/// <param name="scope"></param>
		public void AddGlobalObject(string ivar, object scope)
		{
			this.Context.SetGlobalValue(ivar, scope);
		}

		/// <summary>
		/// </summary>
		public double GetSecondsMultiplierForFunction(string name)
		{
			if (this.restrictions.ContainsKey(name))
			{
				return this.restrictions[name].ElapsedTime;
			}
			else
			{
				return 1.0 / 60.0; //WorldManager.Instance.ElapsedMs / 1000.0;
			}
		}

		/// <summary>
		///     inits libraries by processing include() directive
		/// </summary>
		/// <param name="source"></param>
		public string InitLibs(string source)
		{
			string[] libs;
			string modified = ParseIncludeDirective(source, out libs);

			foreach (string l in libs)
			{
				//to prevent recursive calls
				if (this.libsInited.Contains(l))
				{
					continue;
				}
				this.libsInited.Add(l);

				string str = File.ReadAllText("scripts/lib/"+l.ToLowerInvariant());
				if (str.Length > 0)
				{
					//analyzie this lib for more libs =)
					str = this.InitLibs(str);
					str = this.InitScopes(str);

					this.Context.Execute(str);
				}		
			}

			return modified;
		}

		/// <summary>
		///     inits scopes by processing using() directive
		/// </summary>
		/// <param name="source"></param>
		public string InitScopes(string source)
		{
			string[] usings;
			string modified = ParseUsingDirective(source, out usings);

			foreach (string us in usings)
			{
				if (this.scopesInited.Contains(us))
				{
					continue;
				}
				this.scopesInited.Add(us);

				switch (us)
				{
					case "console":
						this.Context.SetGlobalValue("console", new ScopeConsole(this).Compile(this.Context));
						break;
					case "generator":
						this.Context.SetGlobalValue("generator", new ScopeGenerator().Compile(this.Context));
						break;
					case "relations":
						this.Context.SetGlobalValue("relations", new ScopeRelations().Compile(this.Context));
						break;
					case "spawn":
						this.Context.SetGlobalValue("spawn", new ScopeSpawn().Compile(this.Context));
						break;
					default:
						Debug.WriteLine(this.ScriptName + ": unknown using scope " + us);
						break;
				}
			}

			return modified;
		}

		/// <summary>
		/// </summary>
		/// <param name="name"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public object InvokeFunction(string name, object args)
		{

			bool bResetElapsedTime = false;

			object retobj;

			if (this.Context.HasGlobalValue(name))
			{
				this.CurrentFunction = name;

				try
				{
					if (args == null)
					{
						retobj = this.Context.CallGlobalFunction(name);
					}
					else if (args is CSmallScope)
					{
						retobj = this.Context.CallGlobalFunction(name, (args as CSmallScope).Instantiate(this.Context));
					}
					else if (args is CBaseScope)
					{
						retobj = this.Context.CallGlobalFunction(name, (args as CBaseScope).Compile(this.Context));
					}
					else
					{
						retobj = this.Context.CallGlobalFunction(name, args);
					}
				}
				catch (JavaScriptException exc)
				{
					Debug.WriteLine(
						String.Format(
							"Script error! {0}:{1} : function {2} :: {3}",
							this.ScriptName,
							exc.LineNumber,
							exc.FunctionName,
							exc.Message));
					retobj = null;
				}

				this.CurrentFunction = null;
				if (bResetElapsedTime)
				{
					this.restrictions[name].ElapsedTime = 0;
				}
				return retobj;
			}
			return null;
				
		}

		/// <summary>
		///     The release.
		/// </summary>
		public void Release()
		{
			//release context?
			this.Context = null;
		}

		/// <summary>
		/// </summary>
		/// <param name="FPS"></param>
		public void RestrictFunctionUsageByTime(string function, double sec)
		{
			if (!this.restrictions.ContainsKey(function))
			{
				this.restrictions.Add(function, new FunctionUsageRestriction() { ElapsedTime = 0, UpdateTime = sec });
			}
			else
			{
				this.restrictions[function].UpdateTime = sec;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="function"></param>
		/// <param name="FPS"></param>
		public void RestrictFunctionUsageByTime(string function, int FPS)
		{
			this.RestrictFunctionUsageByTime(function, 1.0 / (double)FPS);
		}

		/// <summary>
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public object Run(string code)
		{
			return this.Context.Evaluate(code);
		}

		#endregion

		#region Methods

		/// <summary>
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		private static string ParseIncludeDirective(string source, out string[] includes)
		{
			List<string> res = new List<string>();

			//cut all following first function declaration
			string search;
			int indexOfFirstFunction = source.IndexOf("function");
			if (indexOfFirstFunction == -1)
			{
				search = String.Copy(source);
			}
			else
			{
				search = source.Substring(0, indexOfFirstFunction);
			}

			MatchCollection col = Regex.Matches(search, @"include\([^\)]*\)", RegexOptions.Multiline);
			foreach (object match in col)
			{
				//try to include stuff
				string libname = match.ToString().Substring(8, match.ToString().Length - 9);
				res.Add(libname);
			}
			includes = res.ToArray();

			return Regex.Replace(source, @"include\([^\)]*\)[;]?", "", RegexOptions.Multiline);
		}

		/// <summary>
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		private static string ParseUsingDirective(string source, out string[] usings)
		{
			List<string> res = new List<string>();

			//cut all following first function declaration
			string search = StripComments(source);
			int indexOfFirstFunction = search.IndexOf("function");
			if (indexOfFirstFunction != -1)
			{
				search = search.Substring(0, indexOfFirstFunction);
			}

			MatchCollection col = Regex.Matches(search, @"using\([^\)]*\)", RegexOptions.Multiline);
			foreach (object match in col)
			{
				//try to include stuff
				string libname = match.ToString().Substring(6, match.ToString().Length - 7);
				libname = libname.Trim(new[] { '\t', '\r', '\n', ' ', '"' });
				res.Add(libname);
			}
			usings = res.ToArray();

			return Regex.Replace(source, @"using\([^\)]*\)[;]?", "", RegexOptions.Multiline);
		}

		private static string StripComments(string code)
		{
			string re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
		}

		#endregion

		public class FunctionUsageRestriction
		{
			#region Fields

			public int CurrentFrame;

			public double ElapsedTime;

			public double UpdateTime;

			#endregion
		}
	}
	
}