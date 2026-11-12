using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class StiColorZoneControl : StiZoneControl
{
	private IContainer components;

	public StiColorBoxXF colorBoxColor;

	public Color SelectedColor => colorBoxColor.SelectedColor;

	public StiColorZoneControl(Color color)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			colorBoxColor.SelectedColor = color;
			base.DrawBorder = false;
		}
	}

	private void ColorBoxColor_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).Focus();
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
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		colorBoxColor = new StiColorBoxXF();
		((Control)this).SuspendLayout();
		((Control)colorBoxColor).Anchor = (AnchorStyles)15;
		((Control)colorBoxColor).BackColor = SystemColors.Window;
		((Control)colorBoxColor).Location = new Point(24, 10);
		((Control)colorBoxColor).Name = "colorBoxColor";
		colorBoxColor.SelectedColor = Color.Transparent;
		colorBoxColor.ShowColorName = false;
		((Control)colorBoxColor).Size = new Size(270, 26);
		((Control)colorBoxColor).TabIndex = 7;
		((Control)colorBoxColor).MouseDown += new MouseEventHandler(ColorBoxColor_MouseDown);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)colorBoxColor);
		((Control)this).Margin = new Padding(3, 3, 3, 3);
		((Control)this).Name = "StiColorZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(321, 45);
		((Control)this).ResumeLayout(false);
	}
}
