using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Base.Drawing.Design;

[ToolboxItem(false)]
internal class StiBorderSideViewForm : Form
{
	public StiBorderSides BorderSide;

	public Panel panel1;

	private ButtonX buttonBottom;

	private ButtonX buttonTop;

	private ButtonX buttonRight;

	private ButtonX buttonLeft;

	private ButtonX buttonOk;

	private ButtonX buttonNone;

	private ToolTip toolTip1;

	private IContainer components;

	public StiBorderSideViewForm(StiBorderSides borderSide)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			BorderSide = borderSide;
			Localize();
		}
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		components = new Container();
		panel1 = new Panel();
		buttonNone = new ButtonX();
		buttonOk = new ButtonX();
		buttonBottom = new ButtonX();
		buttonTop = new ButtonX();
		buttonRight = new ButtonX();
		buttonLeft = new ButtonX();
		toolTip1 = new ToolTip(components);
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).BackColor = Color.White;
		((Control)panel1).Controls.Add((Control)(object)buttonNone);
		((Control)panel1).Controls.Add((Control)(object)buttonOk);
		((Control)panel1).Controls.Add((Control)(object)buttonBottom);
		((Control)panel1).Controls.Add((Control)(object)buttonTop);
		((Control)panel1).Controls.Add((Control)(object)buttonRight);
		((Control)panel1).Controls.Add((Control)(object)buttonLeft);
		((Control)panel1).Location = new Point(136, 66);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(334, 366);
		((Control)panel1).TabIndex = 6;
		((Control)buttonNone).AccessibleRole = (AccessibleRole)43;
		buttonNone.ColorTable = eButtonColor.OrangeWithBackground;
		buttonNone.Is20Button = false;
		((Control)buttonNone).Location = new Point(179, 236);
		((Control)buttonNone).Name = "buttonNone";
		((Control)buttonNone).Size = new Size(128, 104);
		((Control)buttonNone).TabIndex = 11;
		((Control)buttonNone).Text = "None";
		((Control)buttonNone).Click += buttonNone_Click;
		((Control)buttonOk).AccessibleRole = (AccessibleRole)43;
		buttonOk.ColorTable = eButtonColor.OrangeWithBackground;
		buttonOk.Is20Button = false;
		((Control)buttonOk).Location = new Point(29, 236);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(128, 104);
		((Control)buttonOk).TabIndex = 10;
		((Control)buttonOk).Text = "All";
		((Control)buttonOk).Click += buttonOk_Click;
		((Control)buttonBottom).AccessibleRole = (AccessibleRole)43;
		buttonBottom.AutoCheckOnClick = true;
		buttonBottom.ColorTable = eButtonColor.OrangeWithBackground;
		buttonBottom.Is20Button = false;
		((Control)buttonBottom).Location = new Point(104, 179);
		((Control)buttonBottom).Name = "buttonBottom";
		((Control)buttonBottom).Size = new Size(128, 37);
		((Control)buttonBottom).TabIndex = 9;
		((Control)buttonBottom).Click += buttonBottom_Click;
		((Control)buttonTop).AccessibleRole = (AccessibleRole)43;
		buttonTop.AutoCheckOnClick = true;
		buttonTop.ColorTable = eButtonColor.OrangeWithBackground;
		buttonTop.Is20Button = false;
		((Control)buttonTop).Location = new Point(104, 24);
		((Control)buttonTop).Name = "buttonTop";
		((Control)buttonTop).Size = new Size(128, 37);
		((Control)buttonTop).TabIndex = 8;
		((Control)buttonTop).Click += buttonTop_Click;
		((Control)buttonRight).AccessibleRole = (AccessibleRole)43;
		buttonRight.AutoCheckOnClick = true;
		buttonRight.ColorTable = eButtonColor.OrangeWithBackground;
		buttonRight.Is20Button = false;
		((Control)buttonRight).Location = new Point(232, 61);
		((Control)buttonRight).Name = "buttonRight";
		((Control)buttonRight).Size = new Size(40, 118);
		((Control)buttonRight).TabIndex = 7;
		((Control)buttonRight).Click += buttonRight_Click;
		((Control)buttonLeft).AccessibleRole = (AccessibleRole)43;
		buttonLeft.AutoCheckOnClick = true;
		buttonLeft.ColorTable = eButtonColor.OrangeWithBackground;
		buttonLeft.Is20Button = false;
		((Control)buttonLeft).Location = new Point(64, 61);
		((Control)buttonLeft).Name = "buttonLeft";
		((Control)buttonLeft).Size = new Size(40, 118);
		((Control)buttonLeft).TabIndex = 6;
		((Control)buttonLeft).Click += buttonLeft_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Form)this).ClientSize = new Size(693, 500);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiBorderSideViewForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "StiBorderSideView";
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	public void SetBorder()
	{
		buttonTop.Checked = (BorderSide & StiBorderSides.Top) != 0;
		buttonBottom.Checked = (BorderSide & StiBorderSides.Bottom) != 0;
		buttonLeft.Checked = (BorderSide & StiBorderSides.Left) != 0;
		buttonRight.Checked = (BorderSide & StiBorderSides.Right) != 0;
	}

	private void Localize()
	{
		((Control)buttonOk).Text = StiLocalization.Get("PropertyEnum", "StiBorderSidesAll");
		((Control)buttonNone).Text = StiLocalization.Get("PropertyEnum", "StiBorderSidesNone");
		toolTip1.SetToolTip((Control)(object)buttonOk, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesAll")));
		toolTip1.SetToolTip((Control)(object)buttonNone, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesNone")));
		toolTip1.SetToolTip((Control)(object)buttonLeft, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesLeft")));
		toolTip1.SetToolTip((Control)(object)buttonTop, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesTop")));
		toolTip1.SetToolTip((Control)(object)buttonRight, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesRight")));
		toolTip1.SetToolTip((Control)(object)buttonBottom, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesBottom")));
	}

	private void buttonTop_Click(object sender, EventArgs e)
	{
		BorderSide ^= StiBorderSides.Top;
		SetBorder();
	}

	private void buttonBottom_Click(object sender, EventArgs e)
	{
		BorderSide ^= StiBorderSides.Bottom;
		SetBorder();
	}

	private void buttonLeft_Click(object sender, EventArgs e)
	{
		BorderSide ^= StiBorderSides.Left;
		SetBorder();
	}

	private void buttonRight_Click(object sender, EventArgs e)
	{
		BorderSide ^= StiBorderSides.Right;
		SetBorder();
	}

	private void buttonOk_Click(object sender, EventArgs e)
	{
		BorderSide = StiBorderSides.All;
		SetBorder();
	}

	private void buttonNone_Click(object sender, EventArgs e)
	{
		BorderSide = StiBorderSides.None;
		SetBorder();
	}
}
