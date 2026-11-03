using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Ribbon;

public class QatCustomizeDialog : OfficeForm
{
	private QatCustomizePanel _23DC;

	internal ButtonX ẗ;

	internal ButtonX ẘ;

	private IContainer Պ;

	public QatCustomizePanel QatCustomizePanel => _23DC;

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		base.Dispose(disposing);
	}

	private void ـ()
	{
		_23DC = new QatCustomizePanel();
		ẗ = new ButtonX();
		ẘ = new ButtonX();
		((Control)this).SuspendLayout();
		((Control)_23DC).BackColor = Color.Transparent;
		((Control)_23DC).Location = new Point(0, 0);
		((Control)_23DC).Name = "qatCustomizePanel1";
		((Control)_23DC).Size = new Size(444, 298);
		((Control)_23DC).TabIndex = 0;
		ẗ.ColorTable = eButtonColor.OrangeWithBackground;
		ẗ.DialogResult = (DialogResult)1;
		((Control)ẗ).Location = new Point(285, 297);
		((Control)ẗ).Name = "buttonOK";
		((Control)ẗ).Size = new Size(73, 21);
		((Control)ẗ).TabIndex = 1;
		((Control)ẗ).Text = "OK";
		ẘ.ColorTable = eButtonColor.OrangeWithBackground;
		ẘ.DialogResult = (DialogResult)2;
		((Control)ẘ).Location = new Point(364, 297);
		((Control)ẘ).Name = "buttonCancel";
		((Control)ẘ).Size = new Size(73, 21);
		((Control)ẘ).TabIndex = 2;
		((Control)ẘ).Text = "Cancel";
		((Form)this).ClientSize = new Size(445, 324);
		((Control)this).Controls.Add((Control)(object)ẘ);
		((Control)this).Controls.Add((Control)(object)ẗ);
		((Control)this).Controls.Add((Control)(object)_23DC);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Form)this).ShowInTaskbar = false;
		((Control)this).Name = "QatCustomizeDialog";
		((Control)this).Text = "Customize";
		((Control)this).ResumeLayout(false);
	}

	public QatCustomizeDialog()
	{
		ـ();
		EnableGlass = false;
	}

	public void LoadItems(RibbonControl rc)
	{
		_23DC.LoadItems(rc);
	}

	public void LoadItems(MetroShell tab)
	{
		_23DC.LoadItems(tab);
	}
}
