using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VEStarmapEditor
{
	using VEStarmapEditor.Classes;

	/// <summary>
	/// Interaction logic for StarControl.xaml
	/// </summary>
	public partial class StarControl : UserControl
	{
		public StarSystem SystemDataObject;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sys"></param>
		public StarControl(StarSystem sys)
		{
			this.InitializeComponent();
			this.Loaded += this.OnLoaded;

			this.SystemDataObject = sys;
			this.Name = "starsystem_"+sys.ID;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="args"></param>
		private void OnLoaded(Object obj, RoutedEventArgs args)
		{
			this.SystemDataObject.OnChanged += this.OnDataObjectChanged;
			this.SystemDataObject.OnDeleted += this.OnDataObjectDeleted;

			this.SystemDataObject.UpdateDataObject();
		}

		private void OnDataObjectDeleted(StarSystem sys)
		{
			this.SystemDataObject.OnChanged -= this.OnDataObjectChanged;
			this.SystemDataObject.OnDeleted -= this.OnDataObjectDeleted;
		}

		public void OnDataObjectChanged(StarSystem sys)
		{
			//update all data
			Canvas.SetLeft(this, SystemDataObject.CoordX * 10);
			Canvas.SetTop(this, SystemDataObject.CoordY * 10);

			this.StarName.Content = SystemDataObject.Name;

			this.TechLevel.Content = SystemDataObject.TechLevel;
			this.DangerLevel.Content = SystemDataObject.DangerLevel;
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateDataObjectCoords(float nCoordX, float nCoordY)
		{
			this.SystemDataObject.CoordX = nCoordX;
			this.SystemDataObject.CoordY = nCoordY;
		}
	}
}