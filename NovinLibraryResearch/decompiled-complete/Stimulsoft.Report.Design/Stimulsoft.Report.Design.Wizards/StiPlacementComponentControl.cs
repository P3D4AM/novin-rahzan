using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Design.Properties;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiPlacementComponentControl : UserControl
{
	private IContainer components;

	private GroupPanel groupPanel1;

	private ButtonX btRight;

	private ButtonX btLeft;

	internal ButtonX btClose;

	public StiPlacementComponentDockStyle DockStyle { get; internal set; }

	public event EventHandler DockStateChanged;

	public event StiPreChangePlacementComponentStateEventHandler PreChangeDockState;

	public void Localize()
	{
		btLeft.Tooltip = SplitString(StiLocalization.Get("PlacementComponent", "MoveLeftFreeSpace"));
		btRight.Tooltip = SplitString(StiLocalization.Get("PlacementComponent", "MoveRightFreeSpace"));
		((Control)btClose).Text = StiLocalization.Get("Buttons", "Close");
	}

	private string SplitString(string text)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] array = text.Split(new char[1] { ' ' });
		int num = 0;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			num += text2.Length;
			stringBuilder.Append(text2);
			if (num >= 40)
			{
				num = 0;
				stringBuilder.Append("\n");
			}
			else
			{
				stringBuilder.Append(" ");
			}
		}
		return stringBuilder.ToString();
	}

	private void DoScaling()
	{
		btLeft.Image = StiImageResizer.GetImageWithSampling(btLeft.Image);
		btRight.Image = StiImageResizer.GetImageWithSampling(btRight.Image);
	}

	private void Button_Click(object sender, EventArgs e)
	{
		if (sender == btLeft)
		{
			DockStyle = StiPlacementComponentDockStyle.MoveLeftFreeSpace;
		}
		else if (sender == btRight)
		{
			DockStyle = StiPlacementComponentDockStyle.MoveRightFreeSpace;
		}
		InvokeDockStateChanged();
	}

	private void Button_MouseEnter(object sender, EventArgs e)
	{
		if (sender == btLeft)
		{
			InvokePreChangeDockState(StiPlacementComponentDockStyle.MoveLeftFreeSpace);
		}
		else if (sender == btRight)
		{
			InvokePreChangeDockState(StiPlacementComponentDockStyle.MoveRightFreeSpace);
		}
	}

	private void Button_MouseLeave(object sender, EventArgs e)
	{
		InvokePreChangeDockState(StiPlacementComponentDockStyle.None);
	}

	private void InvokeDockStateChanged()
	{
		this.DockStateChanged?.Invoke(this, EventArgs.Empty);
	}

	private void InvokePreChangeDockState(StiPlacementComponentDockStyle dockStyle)
	{
		this.PreChangeDockState?.Invoke(this, new StiPreChangePlacementComponentStateEventArgs(dockStyle));
	}

	public StiPlacementComponentControl()
	{
		InitializeComponent();
		Localize();
		DoScaling();
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
		groupPanel1 = new GroupPanel();
		btClose = new ButtonX();
		btRight = new ButtonX();
		btLeft = new ButtonX();
		((Control)groupPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupPanel1).BackColor = Color.Transparent;
		groupPanel1.CanvasColor = SystemColors.Control;
		groupPanel1.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)groupPanel1).Controls.Add((Control)(object)btClose);
		((Control)groupPanel1).Controls.Add((Control)(object)btRight);
		((Control)groupPanel1).Controls.Add((Control)(object)btLeft);
		((Control)groupPanel1).Dock = (DockStyle)5;
		((Control)groupPanel1).Location = new Point(0, 0);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Size = new Size(204, 39);
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
		groupPanel1.Style.Class = "";
		groupPanel1.Style.CornerDiameter = 4;
		groupPanel1.Style.CornerType = eCornerType.Rounded;
		groupPanel1.Style.TextAlignment = eStyleTextAlignment.Center;
		groupPanel1.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		groupPanel1.Style.TextLineAlignment = eStyleTextAlignment.Near;
		groupPanel1.StyleMouseDown.Class = "";
		groupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		groupPanel1.StyleMouseOver.Class = "";
		groupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)groupPanel1).TabIndex = 2;
		((Control)btClose).AccessibleRole = (AccessibleRole)43;
		btClose.ColorTable = eButtonColor.OrangeWithBackground;
		btClose.FocusCuesEnabled = false;
		((Control)btClose).Location = new Point(95, 3);
		((Control)btClose).Name = "btClose";
		((Control)btClose).Size = new Size(100, 27);
		btClose.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btClose).TabIndex = 6;
		((Control)btClose).Text = "Close";
		((Control)btRight).AccessibleRole = (AccessibleRole)43;
		btRight.ColorTable = eButtonColor.OrangeWithBackground;
		btRight.FocusCuesEnabled = false;
		btRight.Image = (Image)(object)Resources.PlacementRight;
		((Control)btRight).Location = new Point(49, 3);
		((Control)btRight).Name = "btRight";
		((Control)btRight).Size = new Size(40, 27);
		btRight.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btRight).TabIndex = 2;
		((Control)btRight).MouseLeave += Button_MouseLeave;
		((Control)btRight).Click += Button_Click;
		((Control)btRight).MouseEnter += Button_MouseEnter;
		((Control)btLeft).AccessibleRole = (AccessibleRole)43;
		btLeft.ColorTable = eButtonColor.OrangeWithBackground;
		btLeft.FocusCuesEnabled = false;
		btLeft.Image = (Image)(object)Resources.PlacementLeft;
		((Control)btLeft).Location = new Point(3, 3);
		((Control)btLeft).Name = "btLeft";
		((Control)btLeft).Size = new Size(40, 27);
		btLeft.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)btLeft).TabIndex = 0;
		((Control)btLeft).MouseLeave += Button_MouseLeave;
		((Control)btLeft).Click += Button_Click;
		((Control)btLeft).MouseEnter += Button_MouseEnter;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)groupPanel1);
		((Control)this).Name = "StiPlacementComponentControl";
		((Control)this).Size = new Size(204, 39);
		((Control)groupPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
