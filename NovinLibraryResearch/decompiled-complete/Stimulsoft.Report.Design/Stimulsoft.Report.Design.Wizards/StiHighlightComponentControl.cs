using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Design.Properties;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiHighlightComponentControl : UserControl
{
	private IContainer components;

	private GroupPanel groupPanel1;

	internal ButtonX btCancel;

	internal ButtonX btAddContentRight;

	internal ButtonX btAddContentLeft;

	internal ButtonX btReplaceContent;

	internal ButtonX btAddContentNewLine;

	public StiHighlightComponentControl()
	{
		InitializeComponent();
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Close");
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
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiHighlightComponentControl));
		groupPanel1 = new GroupPanel();
		btAddContentNewLine = new ButtonX();
		btCancel = new ButtonX();
		btAddContentRight = new ButtonX();
		btAddContentLeft = new ButtonX();
		btReplaceContent = new ButtonX();
		((Control)groupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		groupPanel1.CanvasColor = SystemColors.Control;
		groupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)groupPanel1).Controls.Add((Control)(object)btAddContentNewLine);
		((Control)groupPanel1).Controls.Add((Control)(object)btCancel);
		((Control)groupPanel1).Controls.Add((Control)(object)btAddContentRight);
		((Control)groupPanel1).Controls.Add((Control)(object)btAddContentLeft);
		((Control)groupPanel1).Controls.Add((Control)(object)btReplaceContent);
		((Control)groupPanel1).Dock = (DockStyle)5;
		((Control)groupPanel1).Location = new Point(0, 0);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(182, 79);
		groupPanel1.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		groupPanel1.Style.BackColorGradientAngle = 90;
		groupPanel1.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		groupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		groupPanel1.Style.BorderBottomWidth = 1;
		groupPanel1.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		groupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		groupPanel1.Style.BorderLeftWidth = 1;
		groupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		groupPanel1.Style.BorderRightWidth = 1;
		groupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		groupPanel1.Style.BorderTopWidth = 1;
		groupPanel1.Style.CornerDiameter = 4;
		groupPanel1.Style.CornerType = eCornerType.Rounded;
		groupPanel1.Style.TextAlignment = eStyleTextAlignment.Center;
		groupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		groupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		groupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		groupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)groupPanel1).TabIndex = 0;
		((Control)btAddContentNewLine).AccessibleRole = (AccessibleRole)43;
		btAddContentNewLine.ColorTable = eButtonColor.OrangeWithBackground;
		btAddContentNewLine.Image = (Image)(object)Resources.AddContentNewLine;
		btAddContentNewLine.Is20Button = false;
		((Control)btAddContentNewLine).Location = new Point(135, 3);
		((Control)btAddContentNewLine).Name = "btAddContentNewLine";
		((Control)btAddContentNewLine).Size = new Size(38, 38);
		btAddContentNewLine.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btAddContentNewLine).TabIndex = 5;
		((Control)btCancel).AccessibleRole = (AccessibleRole)43;
		btCancel.ColorTable = eButtonColor.OrangeWithBackground;
		btCancel.Is20Button = false;
		((Control)btCancel).Location = new Point(3, 48);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(170, 23);
		btCancel.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btCancel).TabIndex = 4;
		((Control)btCancel).Text = "Close";
		((Control)btAddContentRight).AccessibleRole = (AccessibleRole)43;
		btAddContentRight.ColorTable = eButtonColor.OrangeWithBackground;
		btAddContentRight.Image = (Image)(object)Resources.AddContentRight;
		btAddContentRight.Is20Button = false;
		((Control)btAddContentRight).Location = new Point(91, 3);
		((Control)btAddContentRight).Name = "btAddContentRight";
		((Control)btAddContentRight).Size = new Size(38, 38);
		btAddContentRight.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btAddContentRight).TabIndex = 3;
		((Control)btAddContentLeft).AccessibleRole = (AccessibleRole)43;
		btAddContentLeft.ColorTable = eButtonColor.OrangeWithBackground;
		btAddContentLeft.Image = (Image)(object)Resources.AddContentLeft;
		btAddContentLeft.Is20Button = false;
		((Control)btAddContentLeft).Location = new Point(47, 3);
		((Control)btAddContentLeft).Name = "btAddContentLeft";
		((Control)btAddContentLeft).Size = new Size(38, 38);
		btAddContentLeft.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btAddContentLeft).TabIndex = 2;
		((Control)btReplaceContent).AccessibleRole = (AccessibleRole)43;
		btReplaceContent.ColorTable = eButtonColor.OrangeWithBackground;
		btReplaceContent.Image = (Image)componentResourceManager.GetObject("btReplaceContent.Image");
		btReplaceContent.Is20Button = false;
		((Control)btReplaceContent).Location = new Point(3, 3);
		((Control)btReplaceContent).Name = "btReplaceContent";
		((Control)btReplaceContent).Size = new Size(38, 38);
		btReplaceContent.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btReplaceContent).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)groupPanel1);
		((Control)this).Name = "StiHighlightComponentControl";
		((Control)this).Size = new Size(182, 79);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
