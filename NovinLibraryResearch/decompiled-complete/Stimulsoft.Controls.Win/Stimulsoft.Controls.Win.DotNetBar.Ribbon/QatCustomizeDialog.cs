using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro;

namespace Stimulsoft.Controls.Win.DotNetBar.Ribbon;

public class QatCustomizeDialog : Form
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
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		buttonCancel = new ButtonX();
		buttonOK = new ButtonX();
		qatCustomizePanel1 = new QatCustomizePanel();
		((Control)this).SuspendLayout();
		((Control)buttonCancel).AccessibleRole = (AccessibleRole)43;
		buttonCancel.ColorTable = eButtonColor.OrangeWithBackground;
		buttonCancel.DialogResult = (DialogResult)2;
		buttonCancel.Is20Button = false;
		((Control)buttonCancel).Location = new Point(360, 291);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(73, 25);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((Control)buttonOK).AccessibleRole = (AccessibleRole)43;
		buttonOK.ColorTable = eButtonColor.OrangeWithBackground;
		buttonOK.DialogResult = (DialogResult)1;
		buttonOK.Is20Button = false;
		((Control)buttonOK).Location = new Point(281, 291);
		((Control)buttonOK).Name = "buttonOK";
		((Control)buttonOK).Size = new Size(73, 25);
		((Control)buttonOK).TabIndex = 1;
		((Control)buttonOK).Text = "OK";
		((Control)qatCustomizePanel1).BackColor = Color.Transparent;
		((Control)qatCustomizePanel1).Location = new Point(0, 0);
		((Control)qatCustomizePanel1).Name = "qatCustomizePanel1";
		((Control)qatCustomizePanel1).Size = new Size(444, 298);
		((Control)qatCustomizePanel1).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(445, 324);
		((Control)this).Controls.Add((Control)(object)buttonCancel);
		((Control)this).Controls.Add((Control)(object)buttonOK);
		((Control)this).Controls.Add((Control)(object)qatCustomizePanel1);
		((Control)this).DoubleBuffered = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "QatCustomizeDialog";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "Customize";
		((Control)this).ResumeLayout(false);
	}

	public QatCustomizeDialog()
	{
		InitializeComponent();
	}

	public void LoadItems(RibbonControl rc)
	{
		qatCustomizePanel1.LoadItems(rc);
	}

	public void LoadItems(MetroShell tab)
	{
		qatCustomizePanel1.LoadItems(tab);
	}
}
