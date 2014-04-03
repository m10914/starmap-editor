namespace VEStarmapEditor
{
	#region

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Threading;

	using VEStarmapEditor.Classes;
	using VEStarmapEditor.ScriptStuff;
	using VEStarmapEditor.XmlStuff;

	using Path = System.Windows.Shapes.Path;

	#endregion

	/// <summary>
	///     Interaction logic for GalaxyMapControl.xaml
	/// </summary>
	public partial class GalaxyMapControl : UserControl
	{
		#region Constants

		private const float maxScale = 2.72f;

		private const float minScale = 0.52f;

		#endregion

		#region Fields

		public DragMode CurrentDragMode = DragMode.None;

		public StarControl DraggingStar = null;

		public StarControl LinkStarEnd = null;

		public StarControl LinkStarStart = null;

		public StarLinkControl NewLink = null;

		private List<string> ConsoleMessages = new List<string>();

		private bool bGenerating = false;

		private Timer generateTimer;

		private Point lastPos;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Main init point
		/// </summary>
		public GalaxyMapControl()
		{
			this.InitializeComponent();

			this.Loaded += this.OnLoaded;
		}

		#endregion

		#region Enums

		public enum DragMode
		{
			None,

			DraggingCanvas,

			DraggingStar,

			SeverLinks,

			CreateLink,
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Thread-safe console messenger
		/// </summary>
		/// <param name="msg"></param>
		public void AddConsoleMessage(string msg)
		{
			this.Dispatcher.BeginInvoke(
				DispatcherPriority.Normal,
				new Action(
					() =>
					{
						//10 maximum
						this.ConsoleMessages.Add(msg);
						if (this.ConsoleMessages.Count > 10)
						{
							this.ConsoleMessages.RemoveAt(0);
						}

						//update controls
						this.CurrentStatus.Content = msg;

						this.ConsoleHistory.Children.Clear();
						foreach (string msgg in this.ConsoleMessages)
						{
							Label label = new Label();
							label.Style = (Style)Application.Current.FindResource("ConsoleLabelStyle");
							label.Content = msgg;

							this.ConsoleHistory.Children.Add(label);
						}
						this.ConsoleHistory.Children.RemoveAt(this.ConsoleMessages.Count - 1);
					}));
		}

		/// <summary>
		///     Zoom to point! Primary feature: mouse cursor will point on the same star on zooming
		/// </summary>
		/// <param name="newScale"></param>
		/// <param name="position"></param>
		public void ZoomToPoint(double newScale, Point position)
		{
			double currentScale = this.CanvasContentScaleTransform.ScaleX;

			double difX = this.LowCanvas.ActualWidth * newScale - this.LowCanvas.ActualWidth * currentScale;
			double frac = (position.X - this.CanvasContentTranslateTransform.X) / (this.LowCanvas.ActualWidth * currentScale);
			double newX = difX * frac;

			double difY = this.LowCanvas.ActualHeight * newScale - this.LowCanvas.ActualHeight * currentScale;
			frac = (position.Y - this.CanvasContentTranslateTransform.Y) / (this.LowCanvas.ActualHeight * currentScale);
			double newY = difY * frac;

			this.CanvasContentScaleTransform.ScaleX = this.CanvasContentScaleTransform.ScaleY = newScale;

			//this.UpdateBounds();

			//newX = this.Clamp(newX, this.minOffsetX, this.maxOffsetX);
			//newY = this.Clamp(newY, this.minOffsetY, this.maxOffsetY);

			this.CanvasContentTranslateTransform.X -= newX;
			this.CanvasContentTranslateTransform.Y -= newY;
		}

		#endregion

		#region Methods

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		private double Clamp(double value, double min, double max)
		{
			return Math.Max(min, Math.Min(max, value));
		}

		/// <summary>
		/// </summary>
		/// <param name="stateInfo"></param>
		private void GeneratorTimerCallback(Object stateInfo)
		{
			this.Dispatcher.BeginInvoke(
				DispatcherPriority.Normal,
				new Action(
					() =>
					{
						Generator.GenerateRandomSystemStep();

						if (Generator.IsGenerationFinished())
						{
							if (this.generateTimer != null)
							{
								//release timer
								this.generateTimer.Dispose();
								this.generateTimer = null;
								this.GenerateButton.IsEnabled = true;
								this.ScriptButton.IsEnabled = true;
								this.bGenerating = false;
							}
						}
					}));
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mouseButtonEventArgs"></param>
		private void OnBtnCenterMap(object sender, RoutedEventArgs e)
		{
			this.CanvasContentScaleTransform.ScaleX = this.CanvasContentScaleTransform.ScaleY = minScale;

			double centerX = this.LayoutRoot.ActualWidth / 2.0;
			double centerY = this.LayoutRoot.ActualHeight / 2.0;
			this.CanvasContentTranslateTransform.X = centerX - this.LowCanvas.ActualWidth * minScale / 2.0;
			this.CanvasContentTranslateTransform.Y = centerY - this.LowCanvas.ActualHeight * minScale / 2.0;
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mouseButtonEventArgs"></param>
		private void OnBtnGenerate(object sender, RoutedEventArgs e)
		{
			this.bGenerating = true;
			this.GenerateButton.IsEnabled = false;
			this.ScriptButton.IsEnabled = false;
			this.generateTimer = new Timer(this.GeneratorTimerCallback, this, 0, 1);

			//add generator params
			GeneratorParams param = new GeneratorParams()
			                        {
				                        NumOfStars = 200,
				                        DistanceBetweenStars = 3,
				                        DistanceBetweenStarAndLink = 2,
				                        AverageJumpsBetweenSystems = 4,
				                        LinkDensity = 5,
				                        FactionParams =
					                        new[]
					                        {
						                        new GeneratorParams.FactionGenerationParams(
							                        "Order",
							                        Colors.BlueViolet,
							                        10,
							                        2),
						                        new GeneratorParams.FactionGenerationParams(
							                        "Freedom",
							                        Colors.BurlyWood,
							                        10,
							                        2),
						                        new GeneratorParams.FactionGenerationParams(
							                        "Fanatics",
							                        Colors.ForestGreen,
							                        10,
							                        2),
						                        new GeneratorParams.FactionGenerationParams(
							                        "Pirates",
							                        Colors.LightSalmon,
							                        10,
							                        2),
						                        new GeneratorParams.FactionGenerationParams(
							                        "Aliens",
							                        Colors.Red,
							                        70,
							                        2),
					                        },
				                        MinJumpsBetweenCapitals = 4,
			                        };
			Generator.SetGeneratorParams(param);
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnBtnGenerateScript(object sender, RoutedEventArgs e)
		{
			this.bGenerating = true;
			this.GenerateButton.IsEnabled = false;
			this.ScriptButton.IsEnabled = false;


			//add generator params
			GeneratorParams param = new GeneratorParams()
			{
				NumOfStars = 200,
				DistanceBetweenStars = 3,
				DistanceBetweenStarAndLink = 2,
				AverageJumpsBetweenSystems = 4,
				LinkDensity = 5,
				FactionParams =
					new[]
					                        {
						                        new GeneratorParams.FactionGenerationParams(
							                        "Order",
							                        Colors.BlueViolet,
							                        10,
							                        2),
						                        new GeneratorParams.FactionGenerationParams(
							                        "Freedom",
							                        Colors.BurlyWood,
							                        10,
							                        2),
						                        new GeneratorParams.FactionGenerationParams(
							                        "Fanatics",
							                        Colors.ForestGreen,
							                        10,
							                        2),
						                        new GeneratorParams.FactionGenerationParams(
							                        "Pirates",
							                        Colors.LightSalmon,
							                        10,
							                        2),
						                        new GeneratorParams.FactionGenerationParams(
							                        "Aliens",
							                        Colors.Red,
							                        70,
							                        2),
					                        },
				MinJumpsBetweenCapitals = 4,
			};
			Generator.SetGeneratorParams(param);

			Script generatorScript = new Script("Generation", File.ReadAllText("scripts/GenerateGalaxy.js"), true);

			GeneratorArgs args = new GeneratorArgs();
			generatorScript.InvokeFunction("GenerateGalaxy", args);

			this.BuildFactionShapes();

			this.GenerateButton.IsEnabled = true;
			this.ScriptButton.IsEnabled = true;
			this.bGenerating = false;
		}


		/// <summary>
		/// 
		/// </summary>
		private void BuildFactionShapes()
		{
			Dictionary<string, List<StarSystem>> facs = new Dictionary<string, List<StarSystem>>();
			foreach (var fac in ScopeRelations.SystemsFactions)
			{
				if(!facs.ContainsKey(fac.Value))
					facs.Add(fac.Value, new List<StarSystem>{ Generator.Stars[(uint)fac.Key] });
				else facs[fac.Value].Add(Generator.Stars[(uint)fac.Key]);
			}

			foreach (var set in facs)
			{
				XmlFaction fact = XmlDataProvider.Factions[set.Key];
				this.OnBuildFactionShapes(set.Value, Color.FromRgb((byte)(fact.MapColor.R * 255), (byte)(fact.MapColor.G * 255), (byte)(fact.MapColor.B * 255)));
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="systems"></param>
		private void OnBuildFactionShapes(List<StarSystem> systems, Color color)
		{
			// create cool megashape over everything
			List<RectangleGeometry> ellipses = new List<RectangleGeometry>();
			foreach (StarSystem sys in systems)
			{
				foreach (StarLink lnk in sys.Links)
				{
					StarSystem linkedSystem = lnk.System1 != sys ? lnk.System1 : lnk.System2;
					if (!systems.Contains(linkedSystem))
					{
						continue;
					}

					Vector dif = new Vector(linkedSystem.CoordX - sys.CoordX, linkedSystem.CoordY - sys.CoordY);
					Vector center = new Vector(sys.CoordX, sys.CoordY) + dif;
					double ang = Math.Atan2(dif.Y, dif.X) / Math.PI * 180.0;
					double distance = dif.Length;

					Transform trans = new RotateTransform(ang, center.X * 10, center.Y * 10);
					RectangleGeometry ellipse =
						new RectangleGeometry(
							new Rect(center.X * 10 - distance * 10 - 10, center.Y * 10 - 10, distance * 10 + 20, 20),
							10,
							10,
							trans);
					ellipses.Add(ellipse);
				}
			}

			// add triangles
			List<Geometry> tris = new List<Geometry>();
			foreach (StarSystem sys in systems)
			{
				for (int i = 0; i < sys.Links.Count; i++)
				{
					StarSystem sys1 = sys.Links[i].System1 != sys ? sys.Links[i].System1 : sys.Links[i].System2;
					if (!systems.Contains(sys1))
					{
						continue;
					}
					for (int j = i + 1; j < sys.Links.Count; j++)
					{
						StarSystem sys2 = sys.Links[j].System1 != sys ? sys.Links[j].System1 : sys.Links[j].System2;
						if (!systems.Contains(sys2))
						{
							continue;
						}

						//check if intersected something
						bool bIntersect = false;
						foreach (StarLink link in Generator.Links.Values.ToArray())
						{
							if (link.System1 == sys.Links[i].System1 || link.System1 == sys.Links[j].System1
							    || link.System2 == sys.Links[i].System2 || link.System2 == sys.Links[j].System2
							    || link.System1 == sys.Links[i].System2 || link.System1 == sys.Links[j].System2
							    || link.System2 == sys.Links[i].System1 || link.System2 == sys.Links[j].System1)
							{
								continue;
							}

							if (MathHelper.HasIntersection(
								(float)sys1.CoordX,
								(float)sys1.CoordY,
								(float)sys2.CoordX,
								(float)sys2.CoordY,
								link.X1,
								link.Y1,
								link.X2,
								link.Y2))
							{
								bIntersect = true;
								Debug.WriteLine(
									"connection " + sys1.Name + " > " + sys2.Name + " interrupted with link " + link.System1.Name + ">"
									+ link.System2.Name);
								break;
							}
						}
						if (bIntersect)
						{
							continue;
						}

						// create poly
						string str = String.Format(
							"M {0},{1} L {2},{3} {4},{5}",
							sys.CoordX * 10,
							sys.CoordY * 10,
							sys1.CoordX * 10,
							sys1.CoordY * 10,
							sys2.CoordX * 10,
							sys2.CoordY * 10);
						Geometry geom = Geometry.Parse(str);
						tris.Add(geom);
					}
				}
			}

			// fill closed circuits
			/*List<Geometry> polys = new List<Geometry>();
	
			foreach (var list in lists)
			{
				var last = list.Last();
				string str = String.Format("M {0},{1} L ", last.CoordX*10, last.CoordY*10);
				foreach (var star in list) str += String.Format("{0},{1} ", star.CoordX*10, star.CoordY*10);
				Geometry geom = Geometry.Parse(str);
				polys.Add(geom);
			}*/

			// do not build anything
			if (tris.Count == 0) return;
			if (ellipses.Count == 0) return;

			// create nested combined geometry
			CombinedGeometry combgeombase = new CombinedGeometry();
			combgeombase.GeometryCombineMode = GeometryCombineMode.Union;
			CombinedGeometry current = combgeombase;
			current.Geometry1 = tris[0];
			for (int i = 1; i < tris.Count; i++)
			{
				CombinedGeometry curgeom = new CombinedGeometry();
				curgeom.GeometryCombineMode = GeometryCombineMode.Union;
				current.Geometry2 = curgeom;
				current = curgeom;
				current.Geometry1 = tris[i];
			}
			for (int i = 0; i < ellipses.Count; i++)
			{
				if (i == ellipses.Count - 1)
				{
					current.Geometry2 = ellipses[i];
				}
				else
				{
					CombinedGeometry curgeom = new CombinedGeometry();
					curgeom.GeometryCombineMode = GeometryCombineMode.Union;
					current.Geometry2 = curgeom;
					current = curgeom;
					current.Geometry1 = ellipses[i];
				}
			}

			// create shape
			Path path = new Path();
			path.IsHitTestVisible = false;
			path.Stroke = null;
			SolidColorBrush brushFill = new SolidColorBrush(color);
			brushFill.Opacity = 0.3;
			path.Fill = brushFill;
			path.Data = combgeombase;
			this.LowCanvas.Children.Add(path);
			Canvas.SetZIndex(path, 3);
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mouseButtonEventArgs"></param>
		private void OnConsoleOpenClose(object sender, MouseButtonEventArgs e)
		{
			if (this.ConsoleHistory.Visibility == Visibility.Collapsed)
			{
				this.ConsoleHistory.Visibility = Visibility.Visible;
			}
			else
			{
				this.ConsoleHistory.Visibility = Visibility.Collapsed;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCreateLinkStart(object sender, MouseButtonEventArgs e)
		{
			StarControl star = (StarControl)sender;
			double posx = Canvas.GetLeft(star);
			double posy = Canvas.GetTop(star);
			this.LinkStarStart = (StarControl)sender;
			VisualStateManager.GoToState(this.LinkStarStart, "Selected", true);

			this.NewLink = new StarLinkControl();
			this.NewLink.SetFromPoint((float)posx, (float)posy);
			this.NewLink.SetToPoint((float)posx, (float)posy);

			this.CurrentDragMode = DragMode.CreateLink;

			this.LowCanvas.Children.Add(this.NewLink);
		}

		/// <summary>
		/// </summary>
		/// <param name="linkControl"></param>
		private void OnLinkAdded(StarLink datalink)
		{
			StarLinkControl link = new StarLinkControl();
			link.SetDataObject(datalink);

			this.LowCanvas.Children.Add(link);
			Canvas.SetZIndex(link, 10);
		}

		/// <summary>
		/// </summary>
		/// <param name="linkControl"></param>
		private void OnLinkRemoved(StarLink link)
		{
			object obj = LogicalTreeHelper.FindLogicalNode(this.LowCanvas, "starlink_" + link.ID);
			StarLinkControl ctl = obj as StarLinkControl;
			if (ctl != null)
			{
				this.LowCanvas.Children.Remove(ctl);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.UpdateClippingRect();
			this.SizeChanged += this.OnSizeChanged;

			//setup events
			this.LayoutRoot.MouseLeftButtonDown += this.OnMouseKeyDown;
			this.LayoutRoot.MouseWheel += this.OnMouseWheel;
			this.LayoutRoot.MouseRightButtonDown += this.OnMouseRightClick;
			this.LayoutRoot.MouseRightButtonUp += this.OnMouseRightUp;
			this.LayoutRoot.PreviewMouseMove += this.OnMouseMove;
			this.LayoutRoot.MouseLeave += this.OnMouseLeave;

			this.CenterMap.Click += this.OnBtnCenterMap;
			this.GenerateButton.Click += this.OnBtnGenerate;
			this.ScriptButton.Click += this.OnBtnGenerateScript;
			this.ClearButton.Click += this.OnClear;

			this.CurrentStatus.MouseLeftButtonDown += this.OnConsoleOpenClose;
			this.ConsoleHistory.MouseLeftButtonDown += this.OnConsoleOpenClose;

			this.ConsoleHistory.Children.Clear();
			this.ConsoleHistory.Visibility = Visibility.Collapsed;

			this.AddConsoleMessage("-= VoidExpanse StarMap Editor: loaded and ready for work");
			Generator.OnLog += this.AddConsoleMessage;
			Generator.OnStarAdded += this.OnStarAdded;
			Generator.OnStarRemoved += this.OnStarRemoved;
			Generator.OnLinkAdded += this.OnLinkAdded;
			Generator.OnLinkRemoved += this.OnLinkRemoved;
			Generator.OnBuildFactionShapes += this.OnBuildFactionShapes;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClear(object sender, RoutedEventArgs e)
		{
			Generator.Clear();
			ScopeRelations.Clear();
			this.LowCanvas.Children.Clear();
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseEnterStar(object sender, MouseEventArgs e)
		{
			if (this.CurrentDragMode == DragMode.CreateLink)
			{
				StarControl targetStar = (StarControl)sender;
				if (targetStar.Equals(this.LinkStarStart))
				{
					return;
				}

				VisualStateManager.GoToState(targetStar, "Selected", true);

				this.LinkStarEnd = targetStar;
				double posx = Canvas.GetLeft(targetStar);
				double posy = Canvas.GetTop(targetStar);
				this.NewLink.SetToPoint((float)posx, (float)posy);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mouseButtonEventArgs"></param>
		private void OnMouseKeyDown(object sender, MouseButtonEventArgs e)
		{
			if (this.CurrentDragMode != DragMode.DraggingStar && this.CurrentDragMode != DragMode.SeverLinks)
			{
				this.CurrentDragMode = DragMode.DraggingCanvas;
			}

			this.lastPos = e.GetPosition(this.LayoutRoot);
			this.LayoutRoot.MouseLeftButtonUp += this.OnMouseLeftButtonUp;
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mouseButtonEventArgs"></param>
		private void OnMouseLeave(object sender, MouseEventArgs mouseButtonEventArgs)
		{
			this.ReleaseMouse();
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseLeaveStar(object sender, MouseEventArgs e)
		{
			if (this.CurrentDragMode == DragMode.CreateLink)
			{
				if (this.LinkStarEnd != null)
				{
					VisualStateManager.GoToState(this.LinkStarEnd, "Default", true);
					this.LinkStarEnd = null;
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mouseButtonEventArgs"></param>
		private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			this.ReleaseMouse();
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			//drag canvas
			if (this.CurrentDragMode == DragMode.DraggingCanvas)
			{
				Point pos = e.GetPosition(this.LayoutRoot);
				double newX = pos.X - this.lastPos.X;
				double newY = pos.Y - this.lastPos.Y;

				this.CanvasContentTranslateTransform.X += newX;
				this.CanvasContentTranslateTransform.Y += newY;
				this.lastPos = pos;
			}
				//drag stars
			else if (this.CurrentDragMode == DragMode.DraggingStar)
			{
				Point pos = e.GetPosition(this.LowCanvas);

				this.DraggingStar.UpdateDataObjectCoords((float)(pos.X / 10), (float)(pos.Y / 10));
			}
				//severe links
			else if (this.CurrentDragMode == DragMode.SeverLinks)
			{
				Point pos = e.GetPosition(this.LowCanvas);

				//leave mouse trail
				MouseTrail trail = new MouseTrail();
				trail.Line.X1 = this.lastPos.X;
				trail.Line.Y1 = this.lastPos.Y;
				trail.Line.X2 = pos.X;
				trail.Line.Y2 = pos.Y;
				this.LowCanvas.Children.Add(trail);
				trail.DeleteMe += this.TrailDeleter;

				//check if intersected something
				foreach (StarLink link in Generator.Links.Values.ToArray())
				{
					if (MathHelper.HasIntersection(
						(float)pos.X / 10,
						(float)pos.Y / 10,
						(float)this.lastPos.X / 10,
						(float)this.lastPos.Y / 10,
						link.X1,
						link.Y1,
						link.X2,
						link.Y2))
					{
						Generator.RemoveLink(link);
					}
				}

				this.lastPos = pos;
			}
				//create link
			else if (this.CurrentDragMode == DragMode.CreateLink)
			{
				if (this.LinkStarEnd == null)
				{
					Point pos = e.GetPosition(this.LowCanvas);
					this.NewLink.SetToPoint((float)pos.X, (float)pos.Y);
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseRightClick(object sender, MouseButtonEventArgs e)
		{
			if (this.CurrentDragMode != DragMode.DraggingCanvas && this.CurrentDragMode != DragMode.CreateLink)
			{
				this.CurrentDragMode = DragMode.SeverLinks;
				this.lastPos = e.GetPosition(this.LowCanvas);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseRightUp(object sender, MouseButtonEventArgs e)
		{
			if (this.CurrentDragMode == DragMode.SeverLinks)
			{
				this.CurrentDragMode = DragMode.None;
			}
			else if (this.CurrentDragMode == DragMode.CreateLink)
			{
				this.CurrentDragMode = DragMode.None;

				//invoke
				this.LowCanvas.Children.Remove(this.NewLink);

				if (this.LinkStarStart != null && this.LinkStarEnd != null)
				{
					Generator.AddLink(this.LinkStarEnd.SystemDataObject, this.LinkStarStart.SystemDataObject);
				}

				if (this.LinkStarEnd != null)
				{
					VisualStateManager.GoToState(this.LinkStarEnd, "Default", true);
					this.LinkStarEnd = null;
				}
				if (this.LinkStarStart != null)
				{
					VisualStateManager.GoToState(this.LinkStarStart, "Default", true);
					this.LinkStarStart = null;
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			Point point = e.GetPosition(this.LayoutRoot);
			double newScale = this.CanvasContentScaleTransform.ScaleX + (e.Delta * 0.001);

			newScale = this.Clamp(newScale, minScale, maxScale);
			this.ZoomToPoint(newScale, point);
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.UpdateClippingRect();
		}

		/// <summary>
		/// </summary>
		/// <param name="star"></param>
		private void OnStarAdded(StarSystem star)
		{
			StarControl starCtl = new StarControl(star);
			this.LowCanvas.Children.Add(starCtl);

			Canvas.SetLeft(starCtl, star.CoordX * 10);
			Canvas.SetTop(starCtl, star.CoordY * 10);
			Canvas.SetZIndex(starCtl, 15);

			starCtl.MouseLeftButtonDown += this.OnStarDragStart;
			starCtl.MouseRightButtonDown += this.OnCreateLinkStart;
			starCtl.MouseEnter += this.OnMouseEnterStar;
			starCtl.MouseLeave += this.OnMouseLeaveStar;
		}

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mouseButtonEventArgs"></param>
		private void OnStarDragStart(object sender, MouseButtonEventArgs e)
		{
			StarControl control = (StarControl)sender;

			this.CurrentDragMode = DragMode.DraggingStar;
			this.DraggingStar = control;
			VisualStateManager.GoToState(this.DraggingStar, "Selected", true);
			Canvas.SetZIndex(this.DraggingStar, 150);
		}

		/// <summary>
		/// </summary>
		/// <param name="star"></param>
		private void OnStarRemoved(StarSystem star)
		{
		}

		/// <summary>
		/// </summary>
		private void ReleaseMouse()
		{
			if (this.CurrentDragMode == DragMode.DraggingCanvas)
			{
				this.CurrentDragMode = DragMode.None;
			}
			else if (this.CurrentDragMode == DragMode.DraggingStar)
			{
				this.CurrentDragMode = DragMode.None;
				VisualStateManager.GoToState(this.DraggingStar, "Default", true);
				Canvas.SetZIndex(this.DraggingStar, 15);
				this.DraggingStar = null;
			}

			this.LayoutRoot.MouseLeftButtonUp -= this.OnMouseLeftButtonUp;
		}

		/// <summary>
		/// </summary>
		/// <param name="obj"></param>
		private void TrailDeleter(MouseTrail obj)
		{
			this.LowCanvas.Children.Remove(obj);
		}

		/// <summary>
		/// </summary>
		private void UpdateClippingRect()
		{
			this.ClippingRect.Rect = new Rect(0, 0, this.RootCanvas.ActualWidth, this.RootCanvas.ActualHeight);
		}

		#endregion
	}
}