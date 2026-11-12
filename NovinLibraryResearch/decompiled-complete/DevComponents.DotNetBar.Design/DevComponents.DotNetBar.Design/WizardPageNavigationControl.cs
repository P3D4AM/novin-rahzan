using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design;

[ToolboxItem(false)]
internal class WizardPageNavigationControl : UserControl
{
	public LinkLabel LinkBack;

	public LinkLabel LinkNext;

	private Container components;

	public WizardPageNavigationControl()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		LinkBack = new LinkLabel();
		LinkNext = new LinkLabel();
		((Control)this).SuspendLayout();
		((Control)LinkBack).Dock = (DockStyle)3;
		((Control)LinkBack).Name = "LinkBack";
		((Control)LinkBack).Size = new Size(45, 16);
		((Control)LinkBack).TabIndex = 0;
		((Label)LinkBack).TabStop = true;
		((Control)LinkBack).Text = "< Back";
		((Control)LinkNext).Dock = (DockStyle)5;
		((Control)LinkNext).Location = new Point(40, 0);
		((Control)LinkNext).Name = "LinkNext";
		((Control)LinkNext).Size = new Size(45, 16);
		((Control)LinkNext).TabIndex = 1;
		((Label)LinkNext).TabStop = true;
		((Control)LinkNext).Text = "Next >";
		((Control)this).Controls.AddRange((Control[])(object)new Control[2]
		{
			(Control)LinkNext,
			(Control)LinkBack
		});
		((Control)this).Name = "WizardPageNavigationControl";
		((Control)this).Size = new Size(90, 16);
		((Control)this).ResumeLayout(false);
	}
}
