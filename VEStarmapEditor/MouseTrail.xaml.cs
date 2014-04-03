using System;
using System.Collections.Generic;
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
	using System.Threading;
	using System.Windows.Threading;

	/// <summary>
	/// Interaction logic for MouseTrail.xaml
	/// </summary>
	public partial class MouseTrail : UserControl
	{
		public delegate void TrailDeleter(MouseTrail obj);
		public event TrailDeleter DeleteMe;

		private Timer deletetimer;

		/// <summary>
		/// 
		/// </summary>
		public MouseTrail()
		{
			this.InitializeComponent();

			this.deletetimer = new Timer(this.GeneratorTimerCallback, this, 200, 1000);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="state"></param>
		private void GeneratorTimerCallback(object state)
		{
			this.Dispatcher.BeginInvoke(
				DispatcherPriority.Normal,
				new Action(() =>
				{
					//release timer
					deletetimer.Dispose();
					deletetimer = null;

					var handler = this.DeleteMe;
					if (handler != null) handler(this);
				}));
		}
	}
}