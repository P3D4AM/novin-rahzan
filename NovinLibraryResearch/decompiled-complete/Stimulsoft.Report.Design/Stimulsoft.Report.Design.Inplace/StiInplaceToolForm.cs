using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Inplace;

public class StiInplaceToolForm : StiInplaceEditorForm
{
	private IContainer components;

	public StiInplaceToolForm()
	{
		InitializeComponent();
	}

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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(800, 450);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Control)this).Name = "StiInplaceToolForm";
		((Control)this).Text = "StiInplaceToolForm";
		((Control)this).ResumeLayout(false);
	}
}
