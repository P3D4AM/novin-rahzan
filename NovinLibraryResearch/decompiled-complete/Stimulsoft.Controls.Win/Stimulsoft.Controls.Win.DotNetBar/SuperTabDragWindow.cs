using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabDragWindow : Form
{
	private IContainer components;

	protected override bool ShowWithoutActivation => true;

	public SuperTabDragWindow()
	{
		InitializeComponent();
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).CausesValidation = false;
		((Form)this).ClientSize = new Size(63, 24);
		((Control)this).DoubleBuffered = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "SuperTabDragWindow";
		((Form)this).Opacity = 0.7;
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Control)this).ResumeLayout(false);
	}
}
