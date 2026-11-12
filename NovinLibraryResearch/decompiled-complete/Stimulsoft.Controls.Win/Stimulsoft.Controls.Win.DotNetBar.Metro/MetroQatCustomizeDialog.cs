using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

internal class MetroQatCustomizeDialog : MetroForm
{
	private QatCustomizePanel qatCustomizePanel1;

	internal ButtonX buttonOK;

	internal ButtonX buttonCancel;

	private IContainer components;

	public QatCustomizePanel QatCustomizePanel => qatCustomizePanel1;

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		qatCustomizePanel1 = new QatCustomizePanel();
		buttonOK = new ButtonX();
		buttonCancel = new ButtonX();
		((Control)this).SuspendLayout();
		((Control)qatCustomizePanel1).BackColor = Color.Transparent;
		((Control)qatCustomizePanel1).Location = new Point(0, 0);
		((Control)qatCustomizePanel1).Name = "qatCustomizePanel1";
		((Control)qatCustomizePanel1).Size = new Size(444, 298);
		((Control)qatCustomizePanel1).TabIndex = 0;
		buttonOK.ColorTable = eButtonColor.OrangeWithBackground;
		buttonOK.DialogResult = (DialogResult)1;
		((Control)buttonOK).Location = new Point(285, 297);
		((Control)buttonOK).Name = "buttonOK";
		((Control)buttonOK).Size = new Size(73, 21);
		((Control)buttonOK).TabIndex = 1;
		((Control)buttonOK).Text = "OK";
		buttonCancel.ColorTable = eButtonColor.OrangeWithBackground;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(364, 297);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(73, 21);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((Form)this).ClientSize = new Size(445, 324);
		((Control)this).Controls.Add((Control)(object)buttonCancel);
		((Control)this).Controls.Add((Control)(object)buttonOK);
		((Control)this).Controls.Add((Control)(object)qatCustomizePanel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Form)this).ShowInTaskbar = false;
		((Control)this).Name = "QatCustomizeDialog";
		((Control)this).Text = "Customize";
		((Control)this).ResumeLayout(false);
	}

	public MetroQatCustomizeDialog()
	{
		InitializeComponent();
	}

	public void LoadItems(MetroShell tab)
	{
		qatCustomizePanel1.LoadItems(tab);
	}
}
