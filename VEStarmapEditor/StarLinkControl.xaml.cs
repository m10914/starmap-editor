using System.Windows.Controls;

namespace VEStarmapEditor
{
	using VEStarmapEditor.Classes;

	/// <summary>
	/// Interaction logic for StarLink.xaml
	/// </summary>
	public partial class StarLinkControl : UserControl
	{

		public StarLink DataLinkControl;

		/// <summary>
		/// 
		/// </summary>
		public StarLinkControl()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="link"></param>
		public void SetDataObject(StarLink link)
		{
			this.DataLinkControl = link;
			this.Name = "starlink_" + link.ID;

			link.OnChanged += this.OnDataObjectChanged;
			this.OnDataObjectChanged();
		}

		/// <summary>
		/// 
		/// </summary>
		private void OnDataObjectChanged()
		{
			this.SetFromPoint(DataLinkControl.X1 * 10, DataLinkControl.Y1 * 10);
			this.SetToPoint(DataLinkControl.X2 * 10, DataLinkControl.Y2 * 10);
		}

		/// <summary>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetFromPoint(float x, float y)
		{
			this.Line.X1 = x;
			this.Line.Y1 = y;
		}

		/// <summary>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetToPoint(float x, float y)
		{
			this.Line.X2 = x;
			this.Line.Y2 = y;
		}


	}
}