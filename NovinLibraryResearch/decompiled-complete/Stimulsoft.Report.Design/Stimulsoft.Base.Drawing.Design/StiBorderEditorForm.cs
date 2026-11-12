using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Base.Drawing.Design;

[ToolboxItem(false)]
public class StiBorderEditorForm : StiFormUX
{
	private double topSize;

	private double bottomSize;

	private double leftSize;

	private double rightSize;

	private bool locked;

	private IContainer components;

	private ToolTip toolTip1;

	private ItemPanel itemPanel1;

	private ButtonItem biNone;

	private ButtonItem biSolid;

	private ButtonItem biDash;

	private ButtonItem biDashDot;

	private ButtonItem biDashDotDot;

	private ButtonItem biDot;

	private ButtonItem biDouble;

	private ColorPickerButton colorPickerShadowColor;

	private ButtonX buttonNone;

	private ButtonX buttonAll;

	private ButtonX biShadowSize;

	private ButtonX biDropShadow;

	private ButtonItem biShadowSize1;

	private ButtonItem biShadowSize2;

	private ButtonItem biShadowSize3;

	private ButtonItem biShadowSize4;

	private ButtonItem biShadowSize5;

	private Label labelDropShadow;

	private Label labelShadowColor;

	private Label labelShadowSize;

	private Label labelColor;

	private Label labelSize;

	private ColorPickerButton colorPickerColor;

	private ButtonX biSize;

	private ButtonItem biSize1;

	private ButtonItem biSize2;

	private ButtonItem biSize3;

	private ButtonItem biSize4;

	private ButtonItem biSize5;

	private ImageList imageListPenStyle;

	private ImageList imageListPenStyleRotated;

	private ButtonX buttonLeft;

	private ButtonX buttonRight;

	private ButtonX buttonBottom;

	private ButtonX buttonTop;

	private Panel panelSample;

	private ImageList imageListShadowSizes;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	private StiTabulator TabControl;

	private StiTabulatorPage pageSimple;

	private StiTabulatorPage pageAdvanced;

	private StiOffice2013GroupBox groupSides;

	private StiOffice2013GroupBox groupPenStyle;

	private StiOffice2013GroupBox groupShadow;

	private Panel panel_Advanced;

	private StiOffice2013GroupBox groupSides_A;

	private StiOffice2013GroupBox groupShadow_A;

	private Label labelShadowColor_A;

	private Label labelShadowSize_A;

	private Label labelDropShadow_A;

	private ButtonX biDropShadow_A;

	private ButtonX biShadowSize_A;

	private ButtonItem biShadowSize1_A;

	private ButtonItem biShadowSize2_A;

	private ButtonItem biShadowSize3_A;

	private ButtonItem biShadowSize4_A;

	private ButtonItem biShadowSize5_A;

	private ColorPickerButton colorPickerShadowColor_A;

	private ButtonX buttonLeftSelector;

	private ButtonItem buttonLeftSolid;

	private ButtonItem buttonLeftDash;

	private ButtonItem buttonLeftDashDot;

	private ButtonItem buttonLeftDashDotDot;

	private ButtonItem buttonLeftDot;

	private ButtonItem buttonLeftDouble;

	private ButtonItem buttonLeftNone;

	private ColorPickerDropDown colorPickerLeft;

	private ButtonItem buttonLeftSize1;

	private ButtonItem buttonLeftSize2;

	private ButtonItem buttonLeftSize3;

	private ButtonItem buttonLeftSize4;

	private ButtonItem buttonLeftSize5;

	private ButtonX buttonTopSelector;

	private ButtonItem buttonTopSolid;

	private ButtonItem buttonTopDash;

	private ButtonItem buttonTopDashDot;

	private ButtonItem buttonTopDashDotDot;

	private ButtonItem buttonTopDot;

	private ButtonItem buttonTopDouble;

	private ButtonItem buttonTopNone;

	private ColorPickerDropDown colorPickerTop;

	private ButtonItem buttonTopSize1;

	private ButtonItem buttonTopSize2;

	private ButtonItem buttonTopSize3;

	private ButtonItem buttonTopSize4;

	private ButtonItem buttonTopSize5;

	private ButtonX buttonRightSelector;

	private ButtonItem buttonRightSolid;

	private ButtonItem buttonRightDash;

	private ButtonItem buttonRightDashDot;

	private ButtonItem buttonRightDashDotDot;

	private ButtonItem buttonRightDot;

	private ButtonItem buttonRightDouble;

	private ButtonItem buttonRightNone;

	private ColorPickerDropDown colorPickerRight;

	private ButtonItem buttonRightSize1;

	private ButtonItem buttonRightSize2;

	private ButtonItem buttonRightSize3;

	private ButtonItem buttonRightSize4;

	private ButtonItem buttonRightSize5;

	private ButtonX buttonBottomSelector;

	private ButtonItem buttonBottomSolid;

	private ButtonItem buttonBottomDash;

	private ButtonItem buttonBottomDashDot;

	private ButtonItem buttonBottomDashDotDot;

	private ButtonItem buttonBottomDot;

	private ButtonItem buttonBottomDouble;

	private ButtonItem buttonBottomNone;

	private ColorPickerDropDown colorPickerBottom;

	private ButtonItem buttonBottomSize1;

	private ButtonItem buttonBottomSize2;

	private ButtonItem buttonBottomSize3;

	private ButtonItem buttonBottomSize4;

	private ButtonItem buttonBottomSize5;

	private ButtonX buttonRight_A;

	private ButtonX buttonBottom_A;

	private ButtonX buttonTop_A;

	private ButtonX buttonLeft_A;

	private StiButtonPanelUX stiGroupLine1;

	private StiOffice2013GroupBox gbBorderColor;

	public StiBorder Border { get; private set; }

	public StiBorderEditorForm(StiBorder border)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Border = border;
			Localize();
			DoTheming();
			DoScaling();
			buttonTopSize1.Tag = (buttonBottomSize1.Tag = (buttonLeftSize1.Tag = (buttonRightSize1.Tag = 1.0)));
			buttonTopSize2.Tag = (buttonBottomSize2.Tag = (buttonLeftSize2.Tag = (buttonRightSize2.Tag = 2.0)));
			buttonTopSize3.Tag = (buttonBottomSize3.Tag = (buttonLeftSize3.Tag = (buttonRightSize3.Tag = 3.0)));
			buttonTopSize4.Tag = (buttonBottomSize4.Tag = (buttonLeftSize4.Tag = (buttonRightSize4.Tag = 4.0)));
			buttonTopSize5.Tag = (buttonBottomSize5.Tag = (buttonLeftSize5.Tag = (buttonRightSize5.Tag = 5.0)));
			biSize1.Tag = (biShadowSize1.Tag = (biShadowSize1_A.Tag = 1.0));
			biSize2.Tag = (biShadowSize2.Tag = (biShadowSize2_A.Tag = 2.0));
			biSize3.Tag = (biShadowSize3.Tag = (biShadowSize3_A.Tag = 3.0));
			biSize4.Tag = (biShadowSize4.Tag = (biShadowSize4_A.Tag = 4.0));
			biSize5.Tag = (biShadowSize5.Tag = (biShadowSize5_A.Tag = 5.0));
			buttonLeftSelector.IsCenteredButton = true;
			buttonRightSelector.IsCenteredButton = true;
			buttonTopSelector.IsCenteredButton = true;
			buttonBottomSelector.IsCenteredButton = true;
			TabControl.SelectedIndex = 0;
			SetBorder();
		}
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			itemPanel1.BackgroundStyle.BackColor = StiUX.Background;
			itemPanel1.BackgroundStyle.TextColor = StiUX.ItemForeground;
			ButtonItem buttonItem = biSolid;
			ButtonItem buttonItem2 = biDash;
			ButtonItem buttonItem3 = biDashDot;
			ButtonItem buttonItem4 = biDashDotDot;
			ButtonItem buttonItem5 = biDot;
			ButtonItem buttonItem6 = biDouble;
			Color color = (biNone.ForeColor = StiUX.ItemForeground);
			Color color3 = (buttonItem6.ForeColor = color);
			Color color5 = (buttonItem5.ForeColor = color3);
			Color color7 = (buttonItem4.ForeColor = color5);
			Color color9 = (buttonItem3.ForeColor = color7);
			Color foreColor = (buttonItem2.ForeColor = color9);
			buttonItem.ForeColor = foreColor;
			ButtonItem buttonItem7 = biSolid;
			ButtonItem buttonItem8 = biDash;
			ButtonItem buttonItem9 = biDashDot;
			ButtonItem buttonItem10 = biDashDotDot;
			ButtonItem buttonItem11 = biDot;
			ButtonItem buttonItem12 = biDouble;
			color = (biNone.HotForeColor = StiUX.ItemForegroundHover);
			color3 = (buttonItem12.HotForeColor = color);
			color5 = (buttonItem11.HotForeColor = color3);
			color7 = (buttonItem10.HotForeColor = color5);
			color9 = (buttonItem9.HotForeColor = color7);
			foreColor = (buttonItem8.HotForeColor = color9);
			buttonItem7.HotForeColor = foreColor;
		}
	}

	private void DoScaling()
	{
		buttonAll.Image = StiImageResizer.GetImage(buttonAll.Image);
		buttonNone.Image = StiImageResizer.GetImage(buttonNone.Image);
		ButtonItem buttonItem = biSolid;
		ButtonItem buttonItem2 = buttonLeftSolid;
		ButtonItem buttonItem3 = buttonRightSolid;
		ButtonItem buttonItem4 = buttonTopSolid;
		Image val = (buttonBottomSolid.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Solid.png"], allowDispose: false));
		Image val3 = (buttonItem4.Image = val);
		Image val5 = (buttonItem3.Image = val3);
		Image image2 = (buttonItem2.Image = val5);
		buttonItem.Image = image2;
		ButtonItem buttonItem5 = biDash;
		ButtonItem buttonItem6 = buttonLeftDash;
		ButtonItem buttonItem7 = buttonRightDash;
		ButtonItem buttonItem8 = buttonTopDash;
		val = (buttonBottomDash.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dash.png"], allowDispose: false));
		val3 = (buttonItem8.Image = val);
		val5 = (buttonItem7.Image = val3);
		image2 = (buttonItem6.Image = val5);
		buttonItem5.Image = image2;
		ButtonItem buttonItem9 = biDashDot;
		ButtonItem buttonItem10 = buttonLeftDashDot;
		ButtonItem buttonItem11 = buttonRightDashDot;
		ButtonItem buttonItem12 = buttonTopDashDot;
		val = (buttonBottomDashDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDot.png"], allowDispose: false));
		val3 = (buttonItem12.Image = val);
		val5 = (buttonItem11.Image = val3);
		image2 = (buttonItem10.Image = val5);
		buttonItem9.Image = image2;
		ButtonItem buttonItem13 = biDashDotDot;
		ButtonItem buttonItem14 = buttonLeftDashDotDot;
		ButtonItem buttonItem15 = buttonRightDashDotDot;
		ButtonItem buttonItem16 = buttonTopDashDotDot;
		val = (buttonBottomDashDotDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDotDot.png"], allowDispose: false));
		val3 = (buttonItem16.Image = val);
		val5 = (buttonItem15.Image = val3);
		image2 = (buttonItem14.Image = val5);
		buttonItem13.Image = image2;
		ButtonItem buttonItem17 = biDot;
		ButtonItem buttonItem18 = buttonLeftDot;
		ButtonItem buttonItem19 = buttonRightDot;
		ButtonItem buttonItem20 = buttonTopDot;
		val = (buttonBottomDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dot.png"], allowDispose: false));
		val3 = (buttonItem20.Image = val);
		val5 = (buttonItem19.Image = val3);
		image2 = (buttonItem18.Image = val5);
		buttonItem17.Image = image2;
		ButtonItem buttonItem21 = biDouble;
		ButtonItem buttonItem22 = buttonLeftDouble;
		ButtonItem buttonItem23 = buttonRightDouble;
		ButtonItem buttonItem24 = buttonTopDouble;
		val = (buttonBottomDouble.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Double.png"], allowDispose: false));
		val3 = (buttonItem24.Image = val);
		val5 = (buttonItem23.Image = val3);
		image2 = (buttonItem22.Image = val5);
		buttonItem21.Image = image2;
		ButtonItem buttonItem25 = biNone;
		ButtonItem buttonItem26 = buttonLeftNone;
		ButtonItem buttonItem27 = buttonRightNone;
		ButtonItem buttonItem28 = buttonTopNone;
		val = (buttonBottomNone.Image = StiImageResizer.GetImage(imageListPenStyle.Images["None.png"], allowDispose: false));
		val3 = (buttonItem28.Image = val);
		val5 = (buttonItem27.Image = val3);
		image2 = (buttonItem26.Image = val5);
		buttonItem25.Image = image2;
		biSize1.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		biShadowSize1.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		biShadowSize1_A.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		biSize2.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		biShadowSize2.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		biShadowSize2_A.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		biSize3.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		biShadowSize3.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		biShadowSize3_A.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		biSize4.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		biShadowSize4.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		biShadowSize4_A.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		biSize5.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
		biShadowSize5.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
		biShadowSize5_A.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
		buttonTopSize1.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		buttonBottomSize1.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		buttonLeftSize1.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		buttonRightSize1.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		buttonTopSize2.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		buttonBottomSize2.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		buttonLeftSize2.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		buttonRightSize2.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		buttonTopSize3.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		buttonBottomSize3.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		buttonLeftSize3.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		buttonRightSize3.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		buttonTopSize4.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		buttonBottomSize4.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		buttonLeftSize4.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		buttonRightSize4.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		buttonTopSize5.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
		buttonBottomSize5.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
		buttonLeftSize5.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
		buttonRightSize5.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
		biDropShadow.Image = StiImageResizer.GetImage(biDropShadow.Image);
		biDropShadow_A.Image = StiImageResizer.GetImage(biDropShadow_A.Image);
	}

	private void Localize()
	{
		biSolid.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
		biDash.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
		biDashDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
		biDashDotDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
		biDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
		biDouble.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDouble");
		biNone.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
		toolTip1.SetToolTip((Control)(object)buttonAll, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesAll")));
		toolTip1.SetToolTip((Control)(object)buttonNone, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesNone")));
		toolTip1.SetToolTip((Control)(object)buttonLeft, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesLeft")));
		toolTip1.SetToolTip((Control)(object)buttonTop, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesTop")));
		toolTip1.SetToolTip((Control)(object)buttonRight, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesRight")));
		toolTip1.SetToolTip((Control)(object)buttonBottom, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesBottom")));
		ButtonItem buttonItem = buttonLeftSolid;
		ButtonItem buttonItem2 = buttonRightSolid;
		ButtonItem buttonItem3 = buttonTopSolid;
		string text2 = (buttonBottomSolid.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid"));
		string text4 = (buttonItem3.Text = text2);
		string text6 = (buttonItem2.Text = text4);
		buttonItem.Text = text6;
		ButtonItem buttonItem4 = buttonLeftDash;
		ButtonItem buttonItem5 = buttonRightDash;
		ButtonItem buttonItem6 = buttonTopDash;
		text2 = (buttonBottomDash.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash"));
		text4 = (buttonItem6.Text = text2);
		text6 = (buttonItem5.Text = text4);
		buttonItem4.Text = text6;
		ButtonItem buttonItem7 = buttonLeftDashDot;
		ButtonItem buttonItem8 = buttonRightDashDot;
		ButtonItem buttonItem9 = buttonTopDashDot;
		text2 = (buttonBottomDashDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot"));
		text4 = (buttonItem9.Text = text2);
		text6 = (buttonItem8.Text = text4);
		buttonItem7.Text = text6;
		ButtonItem buttonItem10 = buttonLeftDashDotDot;
		ButtonItem buttonItem11 = buttonRightDashDotDot;
		ButtonItem buttonItem12 = buttonTopDashDotDot;
		text2 = (buttonBottomDashDotDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot"));
		text4 = (buttonItem12.Text = text2);
		text6 = (buttonItem11.Text = text4);
		buttonItem10.Text = text6;
		ButtonItem buttonItem13 = buttonLeftDot;
		ButtonItem buttonItem14 = buttonRightDot;
		ButtonItem buttonItem15 = buttonTopDot;
		text2 = (buttonBottomDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot"));
		text4 = (buttonItem15.Text = text2);
		text6 = (buttonItem14.Text = text4);
		buttonItem13.Text = text6;
		ButtonItem buttonItem16 = buttonLeftDouble;
		ButtonItem buttonItem17 = buttonRightDouble;
		ButtonItem buttonItem18 = buttonTopDouble;
		text2 = (buttonBottomDouble.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDouble"));
		text4 = (buttonItem18.Text = text2);
		text6 = (buttonItem17.Text = text4);
		buttonItem16.Text = text6;
		ButtonItem buttonItem19 = buttonLeftNone;
		ButtonItem buttonItem20 = buttonRightNone;
		ButtonItem buttonItem21 = buttonTopNone;
		text2 = (buttonBottomNone.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone"));
		text4 = (buttonItem21.Text = text2);
		text6 = (buttonItem20.Text = text4);
		buttonItem19.Text = text6;
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)gbBorderColor).Text = StiLocalization.Get("PropertyMain", "Border");
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Border");
		((Control)groupSides).Text = StiLocalization.Get("PropertyMain", "Sides");
		((Control)groupSides_A).Text = StiLocalization.Get("PropertyMain", "Sides");
		((Control)groupPenStyle).Text = StiLocalization.Get("PropertyMain", "Style");
		((Control)groupShadow).Text = StiLocalization.Get("PropertyMain", "Shadow");
		((Control)groupShadow_A).Text = StiLocalization.Get("PropertyMain", "Shadow");
		((Control)labelDropShadow).Text = StiLocalization.Get("PropertyMain", "DropShadow");
		((Control)labelShadowSize).Text = StiLocalization.Get("PropertyMain", "ShadowSize");
		((Control)labelShadowColor).Text = StiLocalization.Get("PropertyMain", "Color");
		((Control)labelDropShadow_A).Text = StiLocalization.Get("PropertyMain", "DropShadow");
		((Control)labelShadowSize_A).Text = StiLocalization.Get("PropertyMain", "ShadowSize");
		((Control)labelShadowColor_A).Text = StiLocalization.Get("PropertyMain", "Color");
		((Control)labelSize).Text = StiLocalization.Get("PropertyMain", "Size");
		((Control)labelColor).Text = StiLocalization.Get("PropertyMain", "Color");
		((Control)pageSimple).Text = StiLocalization.Get("PropertyMain", "Simple");
		((Control)pageAdvanced).Text = StiLocalization.Get("PropertyMain", "Advanced");
		colorPickerTop.Text = StiLocalization.Get("PropertyMain", "Color");
		colorPickerBottom.Text = StiLocalization.Get("PropertyMain", "Color");
		colorPickerLeft.Text = StiLocalization.Get("PropertyMain", "Color");
		colorPickerRight.Text = StiLocalization.Get("PropertyMain", "Color");
	}

	private static int GetSizeIndex(double size)
	{
		if (size == 1.0)
		{
			return 0;
		}
		if (size == 2.0)
		{
			return 1;
		}
		if (size == 3.0)
		{
			return 2;
		}
		if (size == 4.0)
		{
			return 3;
		}
		if (size == 5.0)
		{
			return 4;
		}
		return -1;
	}

	private StiBorder GetBorder()
	{
		StiBorder stiBorder = ((TabControl.SelectedIndex == 1) ? new StiAdvancedBorder() : new StiBorder());
		stiBorder.DropShadow = biDropShadow.Checked;
		stiBorder.ShadowBrush = new StiSolidBrush(colorPickerShadowColor.SelectedColor);
		if (((Control)biShadowSize).Tag is double)
		{
			stiBorder.ShadowSize = (double)((Control)biShadowSize).Tag;
		}
		stiBorder.Color = colorPickerColor.SelectedColor;
		if (((Control)biSize).Tag is double)
		{
			stiBorder.Size = (double)((Control)biSize).Tag;
		}
		if (stiBorder is StiAdvancedBorder)
		{
			StiAdvancedBorder stiAdvancedBorder = stiBorder as StiAdvancedBorder;
			stiAdvancedBorder.TopSide.Color = colorPickerTop.SelectedColor;
			stiAdvancedBorder.BottomSide.Color = colorPickerBottom.SelectedColor;
			stiAdvancedBorder.LeftSide.Color = colorPickerLeft.SelectedColor;
			stiAdvancedBorder.RightSide.Color = colorPickerRight.SelectedColor;
			stiAdvancedBorder.TopSide.Size = topSize;
			stiAdvancedBorder.BottomSide.Size = bottomSize;
			stiAdvancedBorder.LeftSide.Size = leftSize;
			stiAdvancedBorder.RightSide.Size = rightSize;
			stiAdvancedBorder.TopSide.Style = (StiPenStyle)((Control)buttonTop_A).Tag;
			stiAdvancedBorder.BottomSide.Style = (StiPenStyle)((Control)buttonBottom_A).Tag;
			stiAdvancedBorder.LeftSide.Style = (StiPenStyle)((Control)buttonLeft_A).Tag;
			stiAdvancedBorder.RightSide.Style = (StiPenStyle)((Control)buttonRight_A).Tag;
		}
		else
		{
			if (biSolid.Checked)
			{
				stiBorder.Style = StiPenStyle.Solid;
			}
			if (biDash.Checked)
			{
				stiBorder.Style = StiPenStyle.Dash;
			}
			if (biDashDot.Checked)
			{
				stiBorder.Style = StiPenStyle.DashDot;
			}
			if (biDashDotDot.Checked)
			{
				stiBorder.Style = StiPenStyle.DashDotDot;
			}
			if (biDot.Checked)
			{
				stiBorder.Style = StiPenStyle.Dot;
			}
			if (biDouble.Checked)
			{
				stiBorder.Style = StiPenStyle.Double;
			}
			if (biNone.Checked)
			{
				stiBorder.Style = StiPenStyle.None;
			}
			StiBorderSides stiBorderSides = StiBorderSides.None;
			if (buttonTop.Checked)
			{
				stiBorderSides |= StiBorderSides.Top;
			}
			if (buttonBottom.Checked)
			{
				stiBorderSides |= StiBorderSides.Bottom;
			}
			if (buttonLeft.Checked)
			{
				stiBorderSides |= StiBorderSides.Left;
			}
			if (buttonRight.Checked)
			{
				stiBorderSides |= StiBorderSides.Right;
			}
			stiBorder.Side = stiBorderSides;
		}
		return stiBorder;
	}

	public void SetBorder()
	{
		Color selectedColor;
		object tag;
		if (Border is StiAdvancedBorder)
		{
			StiAdvancedBorder stiAdvancedBorder = Border as StiAdvancedBorder;
			colorPickerTop.SelectedColor = stiAdvancedBorder.TopSide.Color;
			colorPickerBottom.SelectedColor = stiAdvancedBorder.BottomSide.Color;
			colorPickerLeft.SelectedColor = stiAdvancedBorder.LeftSide.Color;
			colorPickerRight.SelectedColor = stiAdvancedBorder.RightSide.Color;
			topSize = stiAdvancedBorder.TopSide.Size;
			bottomSize = stiAdvancedBorder.BottomSide.Size;
			leftSize = stiAdvancedBorder.LeftSide.Size;
			rightSize = stiAdvancedBorder.RightSide.Size;
			buttonTop.Checked = stiAdvancedBorder.IsTopBorderSidePresent;
			buttonBottom.Checked = stiAdvancedBorder.IsBottomBorderSidePresent;
			buttonLeft.Checked = stiAdvancedBorder.IsLeftBorderSidePresent;
			buttonRight.Checked = stiAdvancedBorder.IsRightBorderSidePresent;
			((Control)buttonTop_A).Tag = stiAdvancedBorder.TopSide.Style;
			((Control)buttonBottom_A).Tag = stiAdvancedBorder.BottomSide.Style;
			((Control)buttonLeft_A).Tag = stiAdvancedBorder.LeftSide.Style;
			((Control)buttonRight_A).Tag = stiAdvancedBorder.RightSide.Style;
			TabControl.SelectedIndex = 1;
		}
		else
		{
			if (Border == null)
			{
				return;
			}
			buttonTop.Checked = (Border.Side & StiBorderSides.Top) > StiBorderSides.None;
			buttonBottom.Checked = (Border.Side & StiBorderSides.Bottom) > StiBorderSides.None;
			buttonLeft.Checked = (Border.Side & StiBorderSides.Left) > StiBorderSides.None;
			buttonRight.Checked = (Border.Side & StiBorderSides.Right) > StiBorderSides.None;
			biSolid.Checked = Border.Style == StiPenStyle.Solid;
			biNone.Checked = Border.Style == StiPenStyle.None;
			biDash.Checked = Border.Style == StiPenStyle.Dash;
			biDashDot.Checked = Border.Style == StiPenStyle.DashDot;
			biDashDotDot.Checked = Border.Style == StiPenStyle.DashDotDot;
			biDot.Checked = Border.Style == StiPenStyle.Dot;
			biDouble.Checked = Border.Style == StiPenStyle.Double;
			ButtonX buttonX = buttonTop_A;
			ButtonX buttonX2 = buttonBottom_A;
			ButtonX buttonX3 = buttonLeft_A;
			object obj2 = (((Control)buttonRight_A).Tag = Border.Style);
			object obj4 = (((Control)buttonX3).Tag = obj2);
			tag = (((Control)buttonX2).Tag = obj4);
			((Control)buttonX).Tag = tag;
			int sizeIndex = GetSizeIndex(Border.Size);
			if (sizeIndex != -1)
			{
				biSize.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex], allowDispose: false);
			}
			((Control)biSize).Tag = Border.Size;
			topSize = (bottomSize = (leftSize = (rightSize = Border.Size)));
			ColorPickerButton colorPickerButton = colorPickerColor;
			ColorPickerDropDown colorPickerDropDown = colorPickerTop;
			ColorPickerDropDown colorPickerDropDown2 = colorPickerBottom;
			ColorPickerDropDown colorPickerDropDown3 = colorPickerLeft;
			Color color2 = (colorPickerRight.SelectedColor = Border.Color);
			Color color4 = (colorPickerDropDown3.SelectedColor = color2);
			Color color6 = (colorPickerDropDown2.SelectedColor = color4);
			selectedColor = (colorPickerDropDown.SelectedColor = color6);
			colorPickerButton.SelectedColor = selectedColor;
		}
		ButtonX buttonX4 = biDropShadow;
		bool @checked = (biDropShadow_A.Checked = Border.DropShadow);
		buttonX4.Checked = @checked;
		ColorPickerButton colorPickerButton2 = colorPickerShadowColor;
		selectedColor = (colorPickerShadowColor_A.SelectedColor = StiBrush.ToColor(Border.ShadowBrush));
		colorPickerButton2.SelectedColor = selectedColor;
		int sizeIndex2 = GetSizeIndex(Border.ShadowSize);
		if (sizeIndex2 != -1)
		{
			ButtonX buttonX5 = biShadowSize;
			Image image2 = (biShadowSize_A.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex2], allowDispose: false));
			buttonX5.Image = image2;
		}
		ButtonX buttonX6 = biShadowSize;
		tag = (((Control)biShadowSize_A).Tag = Border.ShadowSize);
		((Control)buttonX6).Tag = tag;
		SetPenStyleToButton(buttonTop_A, vertical: false);
		SetPenStyleToButton(buttonBottom_A, vertical: false);
		SetPenStyleToButton(buttonLeft_A, vertical: true);
		SetPenStyleToButton(buttonRight_A, vertical: true);
	}

	private void ResetPenStyle()
	{
		biSolid.Checked = false;
		biDash.Checked = false;
		biDashDot.Checked = false;
		biDashDotDot.Checked = false;
		biDot.Checked = false;
		biDouble.Checked = false;
		biNone.Checked = false;
	}

	private void SetPenStyleToButton(ButtonX button, bool vertical)
	{
		if (((Control)button).Tag is StiPenStyle)
		{
			switch ((StiPenStyle)((Control)button).Tag)
			{
			case StiPenStyle.Solid:
				button.Image = StiImageResizer.GetImage((!vertical) ? imageListPenStyle.Images["Solid.png"] : imageListPenStyleRotated.Images["Solid_Rotated.png"], allowDispose: false);
				break;
			case StiPenStyle.Dash:
				button.Image = StiImageResizer.GetImage((!vertical) ? imageListPenStyle.Images["Dash.png"] : imageListPenStyleRotated.Images["Dash_Rotated.png"], allowDispose: false);
				break;
			case StiPenStyle.DashDot:
				button.Image = StiImageResizer.GetImage((!vertical) ? imageListPenStyle.Images["DashDot.png"] : imageListPenStyleRotated.Images["DashDot_Rotated.png"], allowDispose: false);
				break;
			case StiPenStyle.DashDotDot:
				button.Image = StiImageResizer.GetImage((!vertical) ? imageListPenStyle.Images["DashDotDot.png"] : imageListPenStyleRotated.Images["DashDotDot_Rotated.png"], allowDispose: false);
				break;
			case StiPenStyle.Dot:
				button.Image = StiImageResizer.GetImage((!vertical) ? imageListPenStyle.Images["Dot.png"] : imageListPenStyleRotated.Images["Dot_Rotated.png"], allowDispose: false);
				break;
			case StiPenStyle.Double:
				button.Image = StiImageResizer.GetImage((!vertical) ? imageListPenStyle.Images["Double.png"] : imageListPenStyleRotated.Images["Double_Rotated.png"], allowDispose: false);
				break;
			case StiPenStyle.None:
				button.Image = StiImageResizer.GetImage((!vertical) ? imageListPenStyle.Images["None.png"] : imageListPenStyleRotated.Images["None_Rotated.png"], allowDispose: false);
				break;
			}
		}
	}

	private void ButtonTop_Click(object sender, EventArgs e)
	{
		((Control)panelSample).Invalidate();
	}

	private void ButtonBottom_Click(object sender, EventArgs e)
	{
		((Control)panelSample).Invalidate();
	}

	private void ButtonLeft_Click(object sender, EventArgs e)
	{
		((Control)panelSample).Invalidate();
	}

	private void ButtonRight_Click(object sender, EventArgs e)
	{
		((Control)panelSample).Invalidate();
	}

	private void ButtonAll_Click(object sender, EventArgs e)
	{
		buttonTop.Checked = true;
		buttonBottom.Checked = true;
		buttonLeft.Checked = true;
		buttonRight.Checked = true;
		((Control)panelSample).Invalidate();
	}

	private void ButtonNone_Click(object sender, EventArgs e)
	{
		buttonTop.Checked = false;
		buttonBottom.Checked = false;
		buttonLeft.Checked = false;
		buttonRight.Checked = false;
		((Control)panelSample).Invalidate();
	}

	private void ButtonSolid_Click(object sender, EventArgs e)
	{
		ResetPenStyle();
		ButtonItem buttonItem = sender as ButtonItem;
		buttonItem.Checked = true;
		((Control)panelSample).Invalidate();
	}

	private void ButtonTopSolid_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		ButtonX buttonX = null;
		bool vertical = false;
		if (buttonItem == buttonTopSolid)
		{
			buttonX = buttonTop_A;
			((Control)buttonX).Tag = StiPenStyle.Solid;
		}
		else if (buttonItem == buttonTopDash)
		{
			buttonX = buttonTop_A;
			((Control)buttonX).Tag = StiPenStyle.Dash;
		}
		else if (buttonItem == buttonTopDashDot)
		{
			buttonX = buttonTop_A;
			((Control)buttonX).Tag = StiPenStyle.DashDot;
		}
		else if (buttonItem == buttonTopDashDotDot)
		{
			buttonX = buttonTop_A;
			((Control)buttonX).Tag = StiPenStyle.DashDotDot;
		}
		else if (buttonItem == buttonTopDot)
		{
			buttonX = buttonTop_A;
			((Control)buttonX).Tag = StiPenStyle.Dot;
		}
		else if (buttonItem == buttonTopDouble)
		{
			buttonX = buttonTop_A;
			((Control)buttonX).Tag = StiPenStyle.Double;
		}
		else if (buttonItem == buttonTopNone)
		{
			buttonX = buttonTop_A;
			((Control)buttonX).Tag = StiPenStyle.None;
		}
		else if (buttonItem == buttonBottomSolid)
		{
			buttonX = buttonBottom_A;
			((Control)buttonX).Tag = StiPenStyle.Solid;
		}
		else if (buttonItem == buttonBottomDash)
		{
			buttonX = buttonBottom_A;
			((Control)buttonX).Tag = StiPenStyle.Dash;
		}
		else if (buttonItem == buttonBottomDashDot)
		{
			buttonX = buttonBottom_A;
			((Control)buttonX).Tag = StiPenStyle.DashDot;
		}
		else if (buttonItem == buttonBottomDashDotDot)
		{
			buttonX = buttonBottom_A;
			((Control)buttonX).Tag = StiPenStyle.DashDotDot;
		}
		else if (buttonItem == buttonBottomDot)
		{
			buttonX = buttonBottom_A;
			((Control)buttonX).Tag = StiPenStyle.Dot;
		}
		else if (buttonItem == buttonBottomDouble)
		{
			buttonX = buttonBottom_A;
			((Control)buttonX).Tag = StiPenStyle.Double;
		}
		else if (buttonItem == buttonBottomNone)
		{
			buttonX = buttonBottom_A;
			((Control)buttonX).Tag = StiPenStyle.None;
		}
		else if (buttonItem == buttonLeftSolid)
		{
			vertical = true;
			buttonX = buttonLeft_A;
			((Control)buttonX).Tag = StiPenStyle.Solid;
		}
		else if (buttonItem == buttonLeftDash)
		{
			vertical = true;
			buttonX = buttonLeft_A;
			((Control)buttonX).Tag = StiPenStyle.Dash;
		}
		else if (buttonItem == buttonLeftDashDot)
		{
			vertical = true;
			buttonX = buttonLeft_A;
			((Control)buttonX).Tag = StiPenStyle.DashDot;
		}
		else if (buttonItem == buttonLeftDashDotDot)
		{
			vertical = true;
			buttonX = buttonLeft_A;
			((Control)buttonX).Tag = StiPenStyle.DashDotDot;
		}
		else if (buttonItem == buttonLeftDot)
		{
			vertical = true;
			buttonX = buttonLeft_A;
			((Control)buttonX).Tag = StiPenStyle.Dot;
		}
		else if (buttonItem == buttonLeftDouble)
		{
			vertical = true;
			buttonX = buttonLeft_A;
			((Control)buttonX).Tag = StiPenStyle.Double;
		}
		else if (buttonItem == buttonLeftNone)
		{
			vertical = true;
			buttonX = buttonLeft_A;
			((Control)buttonX).Tag = StiPenStyle.None;
		}
		else if (buttonItem == buttonRightSolid)
		{
			vertical = true;
			buttonX = buttonRight_A;
			((Control)buttonX).Tag = StiPenStyle.Solid;
		}
		else if (buttonItem == buttonRightDash)
		{
			vertical = true;
			buttonX = buttonRight_A;
			((Control)buttonX).Tag = StiPenStyle.Dash;
		}
		else if (buttonItem == buttonRightDashDot)
		{
			vertical = true;
			buttonX = buttonRight_A;
			((Control)buttonX).Tag = StiPenStyle.DashDot;
		}
		else if (buttonItem == buttonRightDashDotDot)
		{
			vertical = true;
			buttonX = buttonRight_A;
			((Control)buttonX).Tag = StiPenStyle.DashDotDot;
		}
		else if (buttonItem == buttonRightDot)
		{
			vertical = true;
			buttonX = buttonRight_A;
			((Control)buttonX).Tag = StiPenStyle.Dot;
		}
		else if (buttonItem == buttonRightDouble)
		{
			vertical = true;
			buttonX = buttonRight_A;
			((Control)buttonX).Tag = StiPenStyle.Double;
		}
		else if (buttonItem == buttonRightNone)
		{
			vertical = true;
			buttonX = buttonRight_A;
			((Control)buttonX).Tag = StiPenStyle.None;
		}
		SetPenStyleToButton(buttonX, vertical);
		((Control)panel_Advanced).Invalidate();
	}

	private void ButtonTopSelector_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)buttonTop_A).Tag is StiPenStyle)
		{
			buttonTopSolid.Checked = (StiPenStyle)((Control)buttonTop_A).Tag == StiPenStyle.Solid;
			buttonTopDash.Checked = (StiPenStyle)((Control)buttonTop_A).Tag == StiPenStyle.Dash;
			buttonTopDashDot.Checked = (StiPenStyle)((Control)buttonTop_A).Tag == StiPenStyle.DashDot;
			buttonTopDashDotDot.Checked = (StiPenStyle)((Control)buttonTop_A).Tag == StiPenStyle.DashDotDot;
			buttonTopDot.Checked = (StiPenStyle)((Control)buttonTop_A).Tag == StiPenStyle.Dot;
			buttonTopDouble.Checked = (StiPenStyle)((Control)buttonTop_A).Tag == StiPenStyle.Double;
			buttonTopNone.Checked = (StiPenStyle)((Control)buttonTop_A).Tag == StiPenStyle.None;
			buttonTopSize1.Checked = topSize == 1.0;
			buttonTopSize2.Checked = topSize == 2.0;
			buttonTopSize3.Checked = topSize == 3.0;
			buttonTopSize4.Checked = topSize == 4.0;
			buttonTopSize5.Checked = topSize == 5.0;
		}
	}

	private void ButtonLeftSelector_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)buttonLeft_A).Tag is StiPenStyle)
		{
			buttonLeftSolid.Checked = (StiPenStyle)((Control)buttonLeft_A).Tag == StiPenStyle.Solid;
			buttonLeftDash.Checked = (StiPenStyle)((Control)buttonLeft_A).Tag == StiPenStyle.Dash;
			buttonLeftDashDot.Checked = (StiPenStyle)((Control)buttonLeft_A).Tag == StiPenStyle.DashDot;
			buttonLeftDashDotDot.Checked = (StiPenStyle)((Control)buttonLeft_A).Tag == StiPenStyle.DashDotDot;
			buttonLeftDot.Checked = (StiPenStyle)((Control)buttonLeft_A).Tag == StiPenStyle.Dot;
			buttonLeftDouble.Checked = (StiPenStyle)((Control)buttonLeft_A).Tag == StiPenStyle.Double;
			buttonLeftNone.Checked = (StiPenStyle)((Control)buttonLeft_A).Tag == StiPenStyle.None;
			buttonLeftSize1.Checked = leftSize == 1.0;
			buttonLeftSize2.Checked = leftSize == 2.0;
			buttonLeftSize3.Checked = leftSize == 3.0;
			buttonLeftSize4.Checked = leftSize == 4.0;
			buttonLeftSize5.Checked = leftSize == 5.0;
		}
	}

	private void ButtonRightSelector_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)buttonRight_A).Tag is StiPenStyle)
		{
			buttonRightSolid.Checked = (StiPenStyle)((Control)buttonRight_A).Tag == StiPenStyle.Solid;
			buttonRightDash.Checked = (StiPenStyle)((Control)buttonRight_A).Tag == StiPenStyle.Dash;
			buttonRightDashDot.Checked = (StiPenStyle)((Control)buttonRight_A).Tag == StiPenStyle.DashDot;
			buttonRightDashDotDot.Checked = (StiPenStyle)((Control)buttonRight_A).Tag == StiPenStyle.DashDotDot;
			buttonRightDot.Checked = (StiPenStyle)((Control)buttonRight_A).Tag == StiPenStyle.Dot;
			buttonRightDouble.Checked = (StiPenStyle)((Control)buttonRight_A).Tag == StiPenStyle.Double;
			buttonRightNone.Checked = (StiPenStyle)((Control)buttonRight_A).Tag == StiPenStyle.None;
			buttonRightSize1.Checked = rightSize == 1.0;
			buttonRightSize2.Checked = rightSize == 2.0;
			buttonRightSize3.Checked = rightSize == 3.0;
			buttonRightSize4.Checked = rightSize == 4.0;
			buttonRightSize5.Checked = rightSize == 5.0;
		}
	}

	private void ButtonBottomSelector_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)buttonBottom_A).Tag is StiPenStyle)
		{
			buttonBottomSolid.Checked = (StiPenStyle)((Control)buttonBottom_A).Tag == StiPenStyle.Solid;
			buttonBottomDash.Checked = (StiPenStyle)((Control)buttonBottom_A).Tag == StiPenStyle.Dash;
			buttonBottomDashDot.Checked = (StiPenStyle)((Control)buttonBottom_A).Tag == StiPenStyle.DashDot;
			buttonBottomDashDotDot.Checked = (StiPenStyle)((Control)buttonBottom_A).Tag == StiPenStyle.DashDotDot;
			buttonBottomDot.Checked = (StiPenStyle)((Control)buttonBottom_A).Tag == StiPenStyle.Dot;
			buttonBottomDouble.Checked = (StiPenStyle)((Control)buttonBottom_A).Tag == StiPenStyle.Double;
			buttonBottomNone.Checked = (StiPenStyle)((Control)buttonBottom_A).Tag == StiPenStyle.None;
			buttonBottomSize1.Checked = bottomSize == 1.0;
			buttonBottomSize2.Checked = bottomSize == 2.0;
			buttonBottomSize3.Checked = bottomSize == 3.0;
			buttonBottomSize4.Checked = bottomSize == 4.0;
			buttonBottomSize5.Checked = bottomSize == 5.0;
		}
	}

	private void PanelSample_Paint(object sender, PaintEventArgs e)
	{
		Rectangle clientRectangle = ((Control)panelSample).ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		e.Graphics.DrawRectangle(Pens.LightGray, clientRectangle);
		StiBorder border = GetBorder();
		RectangleD rect = RectangleD.CreateFromRectangle(((Control)panelSample).ClientRectangle);
		rect.Inflate(-StiScale.XXI(10.0), -StiScale.XXI(10.0));
		border.Draw(e.Graphics, rect, StiScale.I1);
	}

	private void ButtonSize_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)biSize).Tag is double)
		{
			biSize1.Checked = (double)((Control)biSize).Tag == 1.0;
			biSize2.Checked = (double)((Control)biSize).Tag == 2.0;
			biSize3.Checked = (double)((Control)biSize).Tag == 3.0;
			biSize4.Checked = (double)((Control)biSize).Tag == 4.0;
			biSize5.Checked = (double)((Control)biSize).Tag == 5.0;
		}
	}

	private void ButtonShadowSize_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)biShadowSize).Tag is double)
		{
			biShadowSize1.Checked = (double)((Control)biShadowSize).Tag == 1.0;
			biShadowSize2.Checked = (double)((Control)biShadowSize).Tag == 2.0;
			biShadowSize3.Checked = (double)((Control)biShadowSize).Tag == 3.0;
			biShadowSize4.Checked = (double)((Control)biShadowSize).Tag == 4.0;
			biShadowSize5.Checked = (double)((Control)biShadowSize).Tag == 5.0;
		}
	}

	private void ButtonTopSize1_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		topSize = (double)buttonItem.Tag;
		((Control)panel_Advanced).Invalidate();
	}

	private void ButtonLeftSize1_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		leftSize = (double)buttonItem.Tag;
		((Control)panel_Advanced).Invalidate();
	}

	private void ButtonBottomSize1_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		bottomSize = (double)buttonItem.Tag;
		((Control)panel_Advanced).Invalidate();
	}

	private void ButtonRightSize1_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		rightSize = (double)buttonItem.Tag;
		((Control)panel_Advanced).Invalidate();
	}

	private void ColorPickerColor_SelectedColorChanged(object sender, EventArgs e)
	{
		ColorPickerDropDown colorPickerDropDown = colorPickerTop;
		ColorPickerDropDown colorPickerDropDown2 = colorPickerBottom;
		ColorPickerDropDown colorPickerDropDown3 = colorPickerLeft;
		Color color = (colorPickerRight.SelectedColor = colorPickerColor.SelectedColor);
		Color color3 = (colorPickerDropDown3.SelectedColor = color);
		Color selectedColor2 = (colorPickerDropDown2.SelectedColor = color3);
		colorPickerDropDown.SelectedColor = selectedColor2;
		((Control)panelSample).Invalidate();
	}

	private void ButtonSize1_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		((Control)biSize).Tag = (double)buttonItem.Tag;
		topSize = (bottomSize = (leftSize = (rightSize = (double)((Control)biSize).Tag)));
		int sizeIndex = GetSizeIndex(topSize);
		if (sizeIndex != -1)
		{
			biSize.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex], allowDispose: false);
		}
		((Control)panelSample).Invalidate();
	}

	private void ButtonShadowSize1_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		ButtonX buttonX = biShadowSize;
		object tag = (((Control)biShadowSize_A).Tag = (double)buttonItem.Tag);
		((Control)buttonX).Tag = tag;
		int sizeIndex = GetSizeIndex((double)buttonItem.Tag);
		if (sizeIndex != -1)
		{
			ButtonX buttonX2 = biShadowSize;
			Image image2 = (biShadowSize_A.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex], allowDispose: false));
			buttonX2.Image = image2;
		}
		((Control)panelSample).Invalidate();
	}

	private void ButtonDropShadow_Click(object sender, EventArgs e)
	{
		((Control)panelSample).Invalidate();
	}

	private void ColorPickerShadowColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			locked = true;
			colorPickerShadowColor_A.SelectedColor = colorPickerShadowColor.SelectedColor;
			((Control)panelSample).Invalidate();
			locked = false;
		}
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (TabControl.SelectedIndex == 0)
		{
			((Control)this).Width = ((StiScale.Factor >= 2.5) ? StiScale.XXI(500.0) : StiScale.XXI(477.0));
			((Control)this).Height = StiScale.YYI(651.0);
		}
		else
		{
			((Control)this).Width = ((StiScale.Factor >= 2.5) ? StiScale.XXI(293.0) : StiScale.XXI(277.0));
			((Control)this).Height = StiScale.YYI(500.0);
		}
	}

	private void ButtonShadowSize_A_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)biShadowSize_A).Tag is double)
		{
			biShadowSize1_A.Checked = (double)((Control)biShadowSize_A).Tag == 1.0;
			biShadowSize2_A.Checked = (double)((Control)biShadowSize_A).Tag == 2.0;
			biShadowSize3_A.Checked = (double)((Control)biShadowSize_A).Tag == 3.0;
			biShadowSize4_A.Checked = (double)((Control)biShadowSize_A).Tag == 4.0;
			biShadowSize5_A.Checked = (double)((Control)biShadowSize_A).Tag == 5.0;
		}
	}

	private void ButtonShadowSize1_A_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		ButtonX buttonX = biShadowSize;
		object tag = (((Control)biShadowSize_A).Tag = (double)buttonItem.Tag);
		((Control)buttonX).Tag = tag;
		int sizeIndex = GetSizeIndex((double)buttonItem.Tag);
		if (sizeIndex != -1)
		{
			ButtonX buttonX2 = biShadowSize;
			Image image2 = (biShadowSize_A.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex], allowDispose: false));
			buttonX2.Image = image2;
		}
		((Control)panel_Advanced).Invalidate();
	}

	private void ButtonDropShadow_CheckedChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			locked = true;
			biDropShadow_A.Checked = biDropShadow.Checked;
			locked = false;
		}
	}

	private void ButtonDropShadow_A_CheckedChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			locked = true;
			biDropShadow.Checked = biDropShadow_A.Checked;
			((Control)panel_Advanced).Invalidate();
			locked = false;
		}
	}

	private void ColorPickerShadowColor_A_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			locked = true;
			colorPickerShadowColor.SelectedColor = colorPickerShadowColor_A.SelectedColor;
			((Control)panel_Advanced).Invalidate();
			locked = false;
		}
	}

	private void ColorPickerTop_SelectedColorChanged(object sender, EventArgs e)
	{
		((Control)panel_Advanced).Invalidate();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		Border = GetBorder();
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_appearance_borders_simple_borders.htm");
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Expected O, but got Unknown
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Expected O, but got Unknown
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Expected O, but got Unknown
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdb: Expected O, but got Unknown
		//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f75: Expected O, but got Unknown
		//IL_1012: Unknown result type (might be due to invalid IL or missing references)
		//IL_101c: Expected O, but got Unknown
		//IL_1d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7b: Expected O, but got Unknown
		//IL_21cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d90: Expected O, but got Unknown
		//IL_436c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4376: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiBorderEditorForm));
		imageListPenStyle = new ImageList(components);
		toolTip1 = new ToolTip(components);
		imageListPenStyleRotated = new ImageList(components);
		imageListShadowSizes = new ImageList(components);
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		stiGroupLine1 = new StiButtonPanelUX();
		TabControl = new StiTabulator();
		pageSimple = new StiTabulatorPage();
		groupSides = new StiOffice2013GroupBox();
		panelSample = new Panel();
		buttonRight = new ButtonX();
		buttonBottom = new ButtonX();
		buttonTop = new ButtonX();
		buttonLeft = new ButtonX();
		buttonAll = new ButtonX();
		buttonNone = new ButtonX();
		groupPenStyle = new StiOffice2013GroupBox();
		itemPanel1 = new ItemPanel();
		biSolid = new ButtonItem();
		biDash = new ButtonItem();
		biDashDot = new ButtonItem();
		biDashDotDot = new ButtonItem();
		biDot = new ButtonItem();
		biDouble = new ButtonItem();
		biNone = new ButtonItem();
		gbBorderColor = new StiOffice2013GroupBox();
		biSize = new ButtonX();
		biSize1 = new ButtonItem();
		biSize2 = new ButtonItem();
		biSize3 = new ButtonItem();
		biSize4 = new ButtonItem();
		biSize5 = new ButtonItem();
		colorPickerColor = new ColorPickerButton();
		labelSize = new Label();
		labelColor = new Label();
		groupShadow = new StiOffice2013GroupBox();
		labelShadowColor = new Label();
		labelShadowSize = new Label();
		labelDropShadow = new Label();
		biDropShadow = new ButtonX();
		biShadowSize = new ButtonX();
		biShadowSize1 = new ButtonItem();
		biShadowSize2 = new ButtonItem();
		biShadowSize3 = new ButtonItem();
		biShadowSize4 = new ButtonItem();
		biShadowSize5 = new ButtonItem();
		colorPickerShadowColor = new ColorPickerButton();
		pageAdvanced = new StiTabulatorPage();
		groupSides_A = new StiOffice2013GroupBox();
		buttonRight_A = new ButtonX();
		buttonBottom_A = new ButtonX();
		buttonTop_A = new ButtonX();
		buttonLeft_A = new ButtonX();
		buttonLeftSelector = new ButtonX();
		buttonLeftSolid = new ButtonItem();
		buttonLeftDash = new ButtonItem();
		buttonLeftDashDot = new ButtonItem();
		buttonLeftDashDotDot = new ButtonItem();
		buttonLeftDot = new ButtonItem();
		buttonLeftDouble = new ButtonItem();
		buttonLeftNone = new ButtonItem();
		colorPickerLeft = new ColorPickerDropDown();
		buttonLeftSize1 = new ButtonItem();
		buttonLeftSize2 = new ButtonItem();
		buttonLeftSize3 = new ButtonItem();
		buttonLeftSize4 = new ButtonItem();
		buttonLeftSize5 = new ButtonItem();
		buttonTopSelector = new ButtonX();
		buttonTopSolid = new ButtonItem();
		buttonTopDash = new ButtonItem();
		buttonTopDashDot = new ButtonItem();
		buttonTopDashDotDot = new ButtonItem();
		buttonTopDot = new ButtonItem();
		buttonTopDouble = new ButtonItem();
		buttonTopNone = new ButtonItem();
		colorPickerTop = new ColorPickerDropDown();
		buttonTopSize1 = new ButtonItem();
		buttonTopSize2 = new ButtonItem();
		buttonTopSize3 = new ButtonItem();
		buttonTopSize4 = new ButtonItem();
		buttonTopSize5 = new ButtonItem();
		buttonRightSelector = new ButtonX();
		buttonRightSolid = new ButtonItem();
		buttonRightDash = new ButtonItem();
		buttonRightDashDot = new ButtonItem();
		buttonRightDashDotDot = new ButtonItem();
		buttonRightDot = new ButtonItem();
		buttonRightDouble = new ButtonItem();
		buttonRightNone = new ButtonItem();
		colorPickerRight = new ColorPickerDropDown();
		buttonRightSize1 = new ButtonItem();
		buttonRightSize2 = new ButtonItem();
		buttonRightSize3 = new ButtonItem();
		buttonRightSize4 = new ButtonItem();
		buttonRightSize5 = new ButtonItem();
		buttonBottomSelector = new ButtonX();
		buttonBottomSolid = new ButtonItem();
		buttonBottomDash = new ButtonItem();
		buttonBottomDashDot = new ButtonItem();
		buttonBottomDashDotDot = new ButtonItem();
		buttonBottomDot = new ButtonItem();
		buttonBottomDouble = new ButtonItem();
		buttonBottomNone = new ButtonItem();
		colorPickerBottom = new ColorPickerDropDown();
		buttonBottomSize1 = new ButtonItem();
		buttonBottomSize2 = new ButtonItem();
		buttonBottomSize3 = new ButtonItem();
		buttonBottomSize4 = new ButtonItem();
		buttonBottomSize5 = new ButtonItem();
		panel_Advanced = new Panel();
		groupShadow_A = new StiOffice2013GroupBox();
		labelShadowColor_A = new Label();
		labelShadowSize_A = new Label();
		labelDropShadow_A = new Label();
		colorPickerShadowColor_A = new ColorPickerButton();
		biShadowSize_A = new ButtonX();
		biShadowSize1_A = new ButtonItem();
		biShadowSize2_A = new ButtonItem();
		biShadowSize3_A = new ButtonItem();
		biShadowSize4_A = new ButtonItem();
		biShadowSize5_A = new ButtonItem();
		biDropShadow_A = new ButtonX();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)TabControl).SuspendLayout();
		((Control)pageSimple).SuspendLayout();
		((Control)groupSides).SuspendLayout();
		((Control)groupPenStyle).SuspendLayout();
		((Control)gbBorderColor).SuspendLayout();
		((Control)groupShadow).SuspendLayout();
		((Control)pageAdvanced).SuspendLayout();
		((Control)groupSides_A).SuspendLayout();
		((Control)groupShadow_A).SuspendLayout();
		((Control)this).SuspendLayout();
		imageListPenStyle.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListPenStyle.ImageStream");
		imageListPenStyle.TransparentColor = Color.Transparent;
		imageListPenStyle.Images.SetKeyName(0, "Dash.png");
		imageListPenStyle.Images.SetKeyName(1, "DashDot.png");
		imageListPenStyle.Images.SetKeyName(2, "DashDotDot.png");
		imageListPenStyle.Images.SetKeyName(3, "Dot.png");
		imageListPenStyle.Images.SetKeyName(4, "Double.png");
		imageListPenStyle.Images.SetKeyName(5, "None.png");
		imageListPenStyle.Images.SetKeyName(6, "Solid.png");
		imageListPenStyleRotated.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListPenStyleRotated.ImageStream");
		imageListPenStyleRotated.TransparentColor = Color.Transparent;
		imageListPenStyleRotated.Images.SetKeyName(0, "Solid_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(1, "Dash_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(2, "DashDot_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(3, "DashDotDot_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(4, "Dot_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(5, "Double_Rotated.png");
		imageListPenStyleRotated.Images.SetKeyName(6, "None_Rotated.png");
		imageListShadowSizes.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListShadowSizes.ImageStream");
		imageListShadowSizes.TransparentColor = Color.Transparent;
		imageListShadowSizes.Images.SetKeyName(0, "Size1.png");
		imageListShadowSizes.Images.SetKeyName(1, "Size2.png");
		imageListShadowSizes.Images.SetKeyName(2, "Size3.png");
		imageListShadowSizes.Images.SetKeyName(3, "Size4.png");
		imageListShadowSizes.Images.SetKeyName(4, "Size5.png");
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(292, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(373, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 566);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(461, 46);
		((Control)stiGroupLine1).TabIndex = 3;
		((Control)TabControl).Anchor = (AnchorStyles)15;
		((Control)TabControl).Controls.Add((Control)(object)pageSimple);
		((Control)TabControl).Controls.Add((Control)(object)pageAdvanced);
		((Control)TabControl).Location = new Point(12, 10);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 22, 0, 0);
		((Control)TabControl).Size = new Size(435, 542);
		((Control)TabControl).TabIndex = 2;
		TabControl.TitleBackColor = Color.White;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageSimple).Controls.Add((Control)(object)groupSides);
		((Control)pageSimple).Controls.Add((Control)(object)groupPenStyle);
		((Control)pageSimple).Controls.Add((Control)(object)gbBorderColor);
		((Control)pageSimple).Controls.Add((Control)(object)groupShadow);
		pageSimple.Dock = (DockStyle)5;
		pageSimple.InvisibleVisible = true;
		((Control)pageSimple).Location = new Point(0, 22);
		((Control)pageSimple).Name = "pageSimple";
		((Control)pageSimple).Padding = new Padding(0, 0, 1, 0);
		((Control)pageSimple).Size = new Size(435, 520);
		((Control)pageSimple).TabIndex = 0;
		((Control)pageSimple).Text = "Simple";
		((Control)pageSimple).Visible = false;
		((Control)groupSides).Anchor = (AnchorStyles)7;
		((Control)groupSides).Controls.Add((Control)(object)panelSample);
		((Control)groupSides).Controls.Add((Control)(object)buttonRight);
		((Control)groupSides).Controls.Add((Control)(object)buttonBottom);
		((Control)groupSides).Controls.Add((Control)(object)buttonTop);
		((Control)groupSides).Controls.Add((Control)(object)buttonLeft);
		((Control)groupSides).Controls.Add((Control)(object)buttonAll);
		((Control)groupSides).Controls.Add((Control)(object)buttonNone);
		groupSides.HeaderHeight = 24;
		((Control)groupSides).Location = new Point(12, 12);
		((Control)groupSides).Name = "groupSides";
		((Control)groupSides).Size = new Size(195, 225);
		((Control)groupSides).TabIndex = 0;
		((Control)groupSides).TabStop = false;
		((Control)groupSides).Text = "Sides";
		((Control)panelSample).BackColor = Color.WhiteSmoke;
		((Control)panelSample).Location = new Point(53, 66);
		((Control)panelSample).Name = "panelSample";
		((Control)panelSample).Size = new Size(94, 91);
		((Control)panelSample).TabIndex = 3;
		((Control)panelSample).Paint += new PaintEventHandler(PanelSample_Paint);
		((Control)buttonRight).AccessibleRole = (AccessibleRole)43;
		buttonRight.AutoCheckOnClick = true;
		buttonRight.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonRight).Location = new Point(153, 66);
		buttonRight.LockClosePopup = false;
		((Control)buttonRight).Name = "buttonRight";
		((Control)buttonRight).Size = new Size(22, 91);
		((Control)buttonRight).TabIndex = 4;
		((Control)buttonRight).TabStop = false;
		((Control)buttonRight).Click += ButtonRight_Click;
		((Control)buttonBottom).AccessibleRole = (AccessibleRole)43;
		buttonBottom.AutoCheckOnClick = true;
		buttonBottom.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonBottom).Location = new Point(53, 163);
		buttonBottom.LockClosePopup = false;
		((Control)buttonBottom).Name = "buttonBottom";
		((Control)buttonBottom).Size = new Size(94, 22);
		((Control)buttonBottom).TabIndex = 5;
		((Control)buttonBottom).TabStop = false;
		((Control)buttonBottom).Click += ButtonBottom_Click;
		((Control)buttonTop).AccessibleRole = (AccessibleRole)43;
		buttonTop.AutoCheckOnClick = true;
		buttonTop.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonTop).Location = new Point(53, 38);
		buttonTop.LockClosePopup = false;
		((Control)buttonTop).Name = "buttonTop";
		((Control)buttonTop).Size = new Size(94, 22);
		((Control)buttonTop).TabIndex = 1;
		((Control)buttonTop).TabStop = false;
		((Control)buttonTop).Click += ButtonTop_Click;
		((Control)buttonLeft).AccessibleRole = (AccessibleRole)43;
		buttonLeft.AutoCheckOnClick = true;
		buttonLeft.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonLeft).Location = new Point(25, 66);
		buttonLeft.LockClosePopup = false;
		((Control)buttonLeft).Name = "buttonLeft";
		((Control)buttonLeft).Size = new Size(22, 91);
		((Control)buttonLeft).TabIndex = 2;
		((Control)buttonLeft).TabStop = false;
		((Control)buttonLeft).Click += ButtonLeft_Click;
		((Control)buttonAll).AccessibleRole = (AccessibleRole)43;
		buttonAll.ColorTable = eButtonColor.OrangeWithBackground;
		buttonAll.Image = (Image)componentResourceManager.GetObject("buttonAll.Image");
		((Control)buttonAll).Location = new Point(25, 38);
		buttonAll.LockClosePopup = false;
		((Control)buttonAll).Name = "buttonAll";
		((Control)buttonAll).Size = new Size(22, 22);
		((Control)buttonAll).TabIndex = 0;
		((Control)buttonAll).TabStop = false;
		((Control)buttonAll).Click += ButtonAll_Click;
		((Control)buttonNone).AccessibleRole = (AccessibleRole)43;
		buttonNone.ColorTable = eButtonColor.OrangeWithBackground;
		buttonNone.Image = (Image)componentResourceManager.GetObject("buttonNone.Image");
		((Control)buttonNone).Location = new Point(153, 163);
		buttonNone.LockClosePopup = false;
		((Control)buttonNone).Name = "buttonNone";
		((Control)buttonNone).Size = new Size(22, 22);
		((Control)buttonNone).TabIndex = 6;
		((Control)buttonNone).TabStop = false;
		((Control)buttonNone).Click += ButtonNone_Click;
		((Control)groupPenStyle).Anchor = (AnchorStyles)7;
		((Control)groupPenStyle).Controls.Add((Control)(object)itemPanel1);
		groupPenStyle.HeaderHeight = 24;
		((Control)groupPenStyle).Location = new Point(218, 12);
		((Control)groupPenStyle).Name = "groupPenStyle";
		((Control)groupPenStyle).Size = new Size(202, 225);
		((Control)groupPenStyle).TabIndex = 2;
		((Control)groupPenStyle).TabStop = false;
		((Control)groupPenStyle).Text = "Pen Style";
		itemPanel1.BackgroundStyle.BackColor = Color.White;
		itemPanel1.BackgroundStyle.BorderBottomWidth = 1;
		itemPanel1.BackgroundStyle.BorderColor = Color.FromArgb(127, 157, 185);
		itemPanel1.BackgroundStyle.BorderLeftWidth = 1;
		itemPanel1.BackgroundStyle.BorderRightWidth = 1;
		itemPanel1.BackgroundStyle.BorderTopWidth = 1;
		itemPanel1.BackgroundStyle.CornerType = eCornerType.Square;
		itemPanel1.BackgroundStyle.PaddingBottom = 1;
		itemPanel1.BackgroundStyle.PaddingLeft = 1;
		itemPanel1.BackgroundStyle.PaddingRight = 1;
		itemPanel1.BackgroundStyle.PaddingTop = 1;
		itemPanel1.ContainerControlProcessDialogKey = true;
		itemPanel1.Images = imageListPenStyle;
		itemPanel1.Items.AddRange(new BaseItem[7] { biSolid, biDash, biDashDot, biDashDotDot, biDot, biDouble, biNone });
		itemPanel1.LayoutOrientation = eOrientation.Vertical;
		((Control)itemPanel1).Location = new Point(0, 38);
		((Control)itemPanel1).Name = "itemPanel1";
		((Control)itemPanel1).Size = new Size(202, 188);
		((Control)itemPanel1).TabIndex = 0;
		((Control)itemPanel1).Text = "itemPanel1";
		biSolid.ButtonStyle = eButtonStyle.ImageAndText;
		biSolid.Checked = true;
		biSolid.ImageIndex = 6;
		biSolid.ImagePaddingHorizontal = 16;
		biSolid.ImagePaddingVertical = 12;
		biSolid.Name = "biSolid";
		biSolid.Text = "Solid";
		biSolid.Click += ButtonSolid_Click;
		biDash.ButtonStyle = eButtonStyle.ImageAndText;
		biDash.ImageIndex = 0;
		biDash.ImagePaddingHorizontal = 16;
		biDash.ImagePaddingVertical = 12;
		biDash.Name = "biDash";
		biDash.Text = "Dash";
		biDash.Click += ButtonSolid_Click;
		biDashDot.ButtonStyle = eButtonStyle.ImageAndText;
		biDashDot.ImageIndex = 1;
		biDashDot.ImagePaddingHorizontal = 16;
		biDashDot.ImagePaddingVertical = 12;
		biDashDot.Name = "biDashDot";
		biDashDot.Text = "Dash Dot";
		biDashDot.Click += ButtonSolid_Click;
		biDashDotDot.ButtonStyle = eButtonStyle.ImageAndText;
		biDashDotDot.ImageIndex = 2;
		biDashDotDot.ImagePaddingHorizontal = 16;
		biDashDotDot.ImagePaddingVertical = 12;
		biDashDotDot.Name = "biDashDotDot";
		biDashDotDot.Text = "Dash Dot Dot";
		biDashDotDot.Click += ButtonSolid_Click;
		biDot.ButtonStyle = eButtonStyle.ImageAndText;
		biDot.ImageIndex = 3;
		biDot.ImagePaddingHorizontal = 16;
		biDot.ImagePaddingVertical = 12;
		biDot.Name = "biDot";
		biDot.Text = "Dot";
		biDot.Click += ButtonSolid_Click;
		biDouble.ButtonStyle = eButtonStyle.ImageAndText;
		biDouble.ImageIndex = 4;
		biDouble.ImagePaddingHorizontal = 16;
		biDouble.ImagePaddingVertical = 12;
		biDouble.Name = "biDouble";
		biDouble.Text = "Double";
		biDouble.Click += ButtonSolid_Click;
		biNone.ButtonStyle = eButtonStyle.ImageAndText;
		biNone.ImageIndex = 5;
		biNone.ImagePaddingHorizontal = 16;
		biNone.ImagePaddingVertical = 12;
		biNone.Name = "biNone";
		biNone.Text = "None";
		biNone.Click += ButtonSolid_Click;
		((Control)gbBorderColor).Anchor = (AnchorStyles)6;
		((Control)gbBorderColor).Controls.Add((Control)(object)biSize);
		((Control)gbBorderColor).Controls.Add((Control)(object)colorPickerColor);
		((Control)gbBorderColor).Controls.Add((Control)(object)labelSize);
		((Control)gbBorderColor).Controls.Add((Control)(object)labelColor);
		gbBorderColor.HeaderHeight = 24;
		((Control)gbBorderColor).Location = new Point(12, 248);
		((Control)gbBorderColor).Name = "gbBorderColor";
		((Control)gbBorderColor).Size = new Size(408, 105);
		((Control)gbBorderColor).TabIndex = 22;
		((Control)gbBorderColor).TabStop = false;
		((Control)gbBorderColor).Text = "Border";
		((Control)biSize).AccessibleRole = (AccessibleRole)43;
		biSize.AutoExpandOnClick = true;
		biSize.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)biSize).Location = new Point(153, 36);
		biSize.LockClosePopup = false;
		((Control)biSize).Name = "biSize";
		((Control)biSize).Size = new Size(73, 24);
		biSize.SubItems.AddRange(new BaseItem[5] { biSize1, biSize2, biSize3, biSize4, biSize5 });
		((Control)biSize).TabIndex = 4;
		((Control)biSize).TabStop = false;
		biSize.PopupOpen += ButtonSize_PopupOpen;
		biSize1.GlobalItem = false;
		biSize1.ImagePaddingHorizontal = 16;
		biSize1.ImagePaddingVertical = 12;
		biSize1.Name = "biSize1";
		biSize1.Text = "1";
		biSize1.Click += ButtonSize1_Click;
		biSize2.GlobalItem = false;
		biSize2.ImagePaddingHorizontal = 16;
		biSize2.ImagePaddingVertical = 12;
		biSize2.Name = "biSize2";
		biSize2.Text = "2";
		biSize2.Click += ButtonSize1_Click;
		biSize3.GlobalItem = false;
		biSize3.ImagePaddingHorizontal = 16;
		biSize3.ImagePaddingVertical = 12;
		biSize3.Name = "biSize3";
		biSize3.Text = "3";
		biSize3.Click += ButtonSize1_Click;
		biSize4.GlobalItem = false;
		biSize4.ImagePaddingHorizontal = 16;
		biSize4.ImagePaddingVertical = 12;
		biSize4.Name = "biSize4";
		biSize4.Text = "4";
		biSize4.Click += ButtonSize1_Click;
		biSize5.GlobalItem = false;
		biSize5.ImagePaddingHorizontal = 16;
		biSize5.ImagePaddingVertical = 12;
		biSize5.Name = "biSize5";
		biSize5.Text = "5";
		biSize5.Click += ButtonSize1_Click;
		((Control)colorPickerColor).AccessibleRole = (AccessibleRole)43;
		colorPickerColor.AutoExpandOnClick = true;
		colorPickerColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerColor).Location = new Point(153, 72);
		colorPickerColor.LockClosePopup = false;
		((Control)colorPickerColor).Name = "colorPickerColor";
		((Control)colorPickerColor).Size = new Size(73, 24);
		((Control)colorPickerColor).TabIndex = 5;
		((Control)colorPickerColor).TabStop = false;
		colorPickerColor.SelectedColorChanged += ColorPickerColor_SelectedColorChanged;
		((Control)labelSize).ForeColor = Color.DimGray;
		((Control)labelSize).Location = new Point(12, 36);
		((Control)labelSize).Name = "labelSize";
		((Control)labelSize).Size = new Size(131, 24);
		((Control)labelSize).TabIndex = 20;
		((Control)labelSize).Text = "Size";
		labelSize.TextAlign = (ContentAlignment)16;
		((Control)labelColor).ForeColor = Color.DimGray;
		((Control)labelColor).Location = new Point(12, 72);
		((Control)labelColor).Name = "labelColor";
		((Control)labelColor).Size = new Size(128, 24);
		((Control)labelColor).TabIndex = 20;
		((Control)labelColor).Text = "Color";
		labelColor.TextAlign = (ContentAlignment)16;
		((Control)groupShadow).Anchor = (AnchorStyles)6;
		((Control)groupShadow).Controls.Add((Control)(object)labelShadowColor);
		((Control)groupShadow).Controls.Add((Control)(object)labelShadowSize);
		((Control)groupShadow).Controls.Add((Control)(object)labelDropShadow);
		((Control)groupShadow).Controls.Add((Control)(object)biDropShadow);
		((Control)groupShadow).Controls.Add((Control)(object)biShadowSize);
		((Control)groupShadow).Controls.Add((Control)(object)colorPickerShadowColor);
		groupShadow.HeaderHeight = 24;
		((Control)groupShadow).Location = new Point(12, 364);
		((Control)groupShadow).Name = "groupShadow";
		((Control)groupShadow).Size = new Size(408, 146);
		((Control)groupShadow).TabIndex = 1;
		((Control)groupShadow).TabStop = false;
		((Control)groupShadow).Text = "Shadow";
		((Control)labelShadowColor).ForeColor = Color.DimGray;
		((Control)labelShadowColor).Location = new Point(12, 110);
		((Control)labelShadowColor).Name = "labelShadowColor";
		((Control)labelShadowColor).Size = new Size(130, 24);
		((Control)labelShadowColor).TabIndex = 20;
		((Control)labelShadowColor).Text = "Color";
		labelShadowColor.TextAlign = (ContentAlignment)16;
		((Control)labelShadowSize).ForeColor = Color.DimGray;
		((Control)labelShadowSize).Location = new Point(11, 74);
		((Control)labelShadowSize).Name = "labelShadowSize";
		((Control)labelShadowSize).Size = new Size(130, 24);
		((Control)labelShadowSize).TabIndex = 20;
		((Control)labelShadowSize).Text = "Shadow Size";
		labelShadowSize.TextAlign = (ContentAlignment)16;
		((Control)labelDropShadow).ForeColor = Color.DimGray;
		((Control)labelDropShadow).Location = new Point(11, 38);
		((Control)labelDropShadow).Name = "labelDropShadow";
		((Control)labelDropShadow).Size = new Size(130, 24);
		((Control)labelDropShadow).TabIndex = 20;
		((Control)labelDropShadow).Text = "Drop Shadow";
		labelDropShadow.TextAlign = (ContentAlignment)16;
		((Control)biDropShadow).AccessibleRole = (AccessibleRole)43;
		biDropShadow.AutoCheckOnClick = true;
		biDropShadow.ColorTable = eButtonColor.OrangeWithBackground;
		biDropShadow.Image = (Image)componentResourceManager.GetObject("biDropShadow.Image");
		((Control)biDropShadow).Location = new Point(153, 38);
		biDropShadow.LockClosePopup = false;
		((Control)biDropShadow).Name = "biDropShadow";
		((Control)biDropShadow).Size = new Size(24, 24);
		((Control)biDropShadow).TabIndex = 0;
		((Control)biDropShadow).TabStop = false;
		biDropShadow.CheckedChanged += ButtonDropShadow_CheckedChanged;
		((Control)biDropShadow).Click += ButtonDropShadow_Click;
		((Control)biShadowSize).AccessibleRole = (AccessibleRole)43;
		biShadowSize.AutoExpandOnClick = true;
		biShadowSize.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)biShadowSize).Location = new Point(153, 74);
		biShadowSize.LockClosePopup = false;
		((Control)biShadowSize).Name = "biShadowSize";
		((Control)biShadowSize).Size = new Size(56, 24);
		biShadowSize.SubItems.AddRange(new BaseItem[5] { biShadowSize1, biShadowSize2, biShadowSize3, biShadowSize4, biShadowSize5 });
		((Control)biShadowSize).TabIndex = 1;
		((Control)biShadowSize).TabStop = false;
		biShadowSize.PopupOpen += ButtonShadowSize_PopupOpen;
		biShadowSize1.GlobalItem = false;
		biShadowSize1.ImagePaddingHorizontal = 16;
		biShadowSize1.ImagePaddingVertical = 12;
		biShadowSize1.Name = "biShadowSize1";
		biShadowSize1.Text = "1";
		biShadowSize1.Click += ButtonShadowSize1_Click;
		biShadowSize2.GlobalItem = false;
		biShadowSize2.ImagePaddingHorizontal = 16;
		biShadowSize2.ImagePaddingVertical = 12;
		biShadowSize2.Name = "biShadowSize2";
		biShadowSize2.Text = "2";
		biShadowSize2.Click += ButtonShadowSize1_Click;
		biShadowSize3.GlobalItem = false;
		biShadowSize3.ImagePaddingHorizontal = 16;
		biShadowSize3.ImagePaddingVertical = 12;
		biShadowSize3.Name = "biShadowSize3";
		biShadowSize3.Text = "3";
		biShadowSize3.Click += ButtonShadowSize1_Click;
		biShadowSize4.GlobalItem = false;
		biShadowSize4.ImagePaddingHorizontal = 16;
		biShadowSize4.ImagePaddingVertical = 12;
		biShadowSize4.Name = "biShadowSize4";
		biShadowSize4.Text = "4";
		biShadowSize4.Click += ButtonShadowSize1_Click;
		biShadowSize5.GlobalItem = false;
		biShadowSize5.ImagePaddingHorizontal = 16;
		biShadowSize5.ImagePaddingVertical = 12;
		biShadowSize5.Name = "biShadowSize5";
		biShadowSize5.Text = "5";
		biShadowSize5.Click += ButtonShadowSize1_Click;
		((Control)colorPickerShadowColor).AccessibleRole = (AccessibleRole)43;
		colorPickerShadowColor.AutoExpandOnClick = true;
		colorPickerShadowColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerShadowColor.ImagePosition = eImagePosition.Right;
		((Control)colorPickerShadowColor).Location = new Point(153, 110);
		colorPickerShadowColor.LockClosePopup = false;
		((Control)colorPickerShadowColor).Name = "colorPickerShadowColor";
		((Control)colorPickerShadowColor).Size = new Size(56, 24);
		((Control)colorPickerShadowColor).TabIndex = 2;
		((Control)colorPickerShadowColor).TabStop = false;
		colorPickerShadowColor.SelectedColorChanged += ColorPickerShadowColor_SelectedColorChanged;
		((Control)pageAdvanced).Controls.Add((Control)(object)groupSides_A);
		((Control)pageAdvanced).Controls.Add((Control)(object)groupShadow_A);
		pageAdvanced.Dock = (DockStyle)5;
		pageAdvanced.InvisibleVisible = false;
		((Control)pageAdvanced).Location = new Point(0, 22);
		((Control)pageAdvanced).Name = "pageAdvanced";
		((Control)pageAdvanced).Padding = new Padding(0, 0, 1, 0);
		((Control)pageAdvanced).Size = new Size(435, 520);
		((Control)pageAdvanced).TabIndex = 1;
		((Control)pageAdvanced).Text = "Advanced";
		((Control)pageAdvanced).Visible = false;
		((Control)groupSides_A).Controls.Add((Control)(object)buttonRight_A);
		((Control)groupSides_A).Controls.Add((Control)(object)buttonBottom_A);
		((Control)groupSides_A).Controls.Add((Control)(object)buttonTop_A);
		((Control)groupSides_A).Controls.Add((Control)(object)buttonLeft_A);
		((Control)groupSides_A).Controls.Add((Control)(object)buttonLeftSelector);
		((Control)groupSides_A).Controls.Add((Control)(object)buttonTopSelector);
		((Control)groupSides_A).Controls.Add((Control)(object)buttonRightSelector);
		((Control)groupSides_A).Controls.Add((Control)(object)buttonBottomSelector);
		((Control)groupSides_A).Controls.Add((Control)(object)panel_Advanced);
		groupSides_A.HeaderHeight = 24;
		((Control)groupSides_A).Location = new Point(12, 12);
		((Control)groupSides_A).Name = "groupSides_A";
		((Control)groupSides_A).Size = new Size(211, 202);
		((Control)groupSides_A).TabIndex = 0;
		((Control)groupSides_A).TabStop = false;
		((Control)groupSides_A).Text = "Sides";
		((Control)buttonRight_A).AccessibleRole = (AccessibleRole)43;
		buttonRight_A.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonRight_A).Location = new Point(153, 66);
		buttonRight_A.LockClosePopup = false;
		((Control)buttonRight_A).Name = "buttonRight_A";
		((Control)buttonRight_A).Size = new Size(22, 76);
		((Control)buttonRight_A).TabIndex = 8;
		((Control)buttonRight_A).TabStop = false;
		((Control)buttonBottom_A).AccessibleRole = (AccessibleRole)43;
		buttonBottom_A.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonBottom_A).Location = new Point(53, 163);
		buttonBottom_A.LockClosePopup = false;
		((Control)buttonBottom_A).Name = "buttonBottom_A";
		((Control)buttonBottom_A).Size = new Size(79, 22);
		((Control)buttonBottom_A).TabIndex = 9;
		((Control)buttonBottom_A).TabStop = false;
		((Control)buttonTop_A).AccessibleRole = (AccessibleRole)43;
		buttonTop_A.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonTop_A).Location = new Point(53, 38);
		buttonTop_A.LockClosePopup = false;
		((Control)buttonTop_A).Name = "buttonTop_A";
		((Control)buttonTop_A).Size = new Size(79, 22);
		((Control)buttonTop_A).TabIndex = 6;
		((Control)buttonTop_A).TabStop = false;
		((Control)buttonLeft_A).AccessibleRole = (AccessibleRole)43;
		buttonLeft_A.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonLeft_A).Location = new Point(25, 66);
		buttonLeft_A.LockClosePopup = false;
		((Control)buttonLeft_A).Name = "buttonLeft_A";
		((Control)buttonLeft_A).Size = new Size(22, 76);
		((Control)buttonLeft_A).TabIndex = 7;
		((Control)buttonLeft_A).TabStop = false;
		((Control)buttonLeftSelector).AccessibleRole = (AccessibleRole)43;
		buttonLeftSelector.AutoExpandOnClick = true;
		buttonLeftSelector.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonLeftSelector).Location = new Point(25, 141);
		buttonLeftSelector.LockClosePopup = false;
		((Control)buttonLeftSelector).Name = "buttonLeftSelector";
		((Control)buttonLeftSelector).Size = new Size(22, 16);
		buttonLeftSelector.SubItems.AddRange(new BaseItem[13]
		{
			buttonLeftSolid, buttonLeftDash, buttonLeftDashDot, buttonLeftDashDotDot, buttonLeftDot, buttonLeftDouble, buttonLeftNone, colorPickerLeft, buttonLeftSize1, buttonLeftSize2,
			buttonLeftSize3, buttonLeftSize4, buttonLeftSize5
		});
		((Control)buttonLeftSelector).TabIndex = 1;
		((Control)buttonLeftSelector).TabStop = false;
		buttonLeftSelector.PopupOpen += ButtonLeftSelector_PopupOpen;
		buttonLeftSolid.GlobalItem = false;
		buttonLeftSolid.ImagePaddingHorizontal = 16;
		buttonLeftSolid.ImagePaddingVertical = 12;
		buttonLeftSolid.Name = "buttonLeftSolid";
		buttonLeftSolid.Text = "Solid";
		buttonLeftSolid.Click += ButtonTopSolid_Click;
		buttonLeftDash.GlobalItem = false;
		buttonLeftDash.ImagePaddingHorizontal = 16;
		buttonLeftDash.ImagePaddingVertical = 12;
		buttonLeftDash.Name = "buttonLeftDash";
		buttonLeftDash.Text = "Dash";
		buttonLeftDash.Click += ButtonTopSolid_Click;
		buttonLeftDashDot.GlobalItem = false;
		buttonLeftDashDot.ImagePaddingHorizontal = 16;
		buttonLeftDashDot.ImagePaddingVertical = 12;
		buttonLeftDashDot.Name = "buttonLeftDashDot";
		buttonLeftDashDot.Text = "Dash Dot";
		buttonLeftDashDot.Click += ButtonTopSolid_Click;
		buttonLeftDashDotDot.GlobalItem = false;
		buttonLeftDashDotDot.ImagePaddingHorizontal = 16;
		buttonLeftDashDotDot.ImagePaddingVertical = 12;
		buttonLeftDashDotDot.Name = "buttonLeftDashDotDot";
		buttonLeftDashDotDot.Text = "Dash Dot Dot";
		buttonLeftDashDotDot.Click += ButtonTopSolid_Click;
		buttonLeftDot.GlobalItem = false;
		buttonLeftDot.ImagePaddingHorizontal = 16;
		buttonLeftDot.ImagePaddingVertical = 12;
		buttonLeftDot.Name = "buttonLeftDot";
		buttonLeftDot.Text = "Dot";
		buttonLeftDot.Click += ButtonTopSolid_Click;
		buttonLeftDouble.GlobalItem = false;
		buttonLeftDouble.ImagePaddingHorizontal = 16;
		buttonLeftDouble.ImagePaddingVertical = 12;
		buttonLeftDouble.Name = "buttonLeftDouble";
		buttonLeftDouble.Text = "Double";
		buttonLeftDouble.Click += ButtonTopSolid_Click;
		buttonLeftNone.GlobalItem = false;
		buttonLeftNone.ImagePaddingHorizontal = 16;
		buttonLeftNone.ImagePaddingVertical = 12;
		buttonLeftNone.Name = "buttonLeftNone";
		buttonLeftNone.Text = "None";
		buttonLeftNone.Click += ButtonTopSolid_Click;
		colorPickerLeft.BeginGroup = true;
		colorPickerLeft.GlobalItem = false;
		colorPickerLeft.ImagePaddingHorizontal = 16;
		colorPickerLeft.ImagePaddingVertical = 12;
		colorPickerLeft.Name = "colorPickerLeft";
		colorPickerLeft.Text = "Color";
		colorPickerLeft.SelectedColorChanged += ColorPickerTop_SelectedColorChanged;
		buttonLeftSize1.BeginGroup = true;
		buttonLeftSize1.GlobalItem = false;
		buttonLeftSize1.ImagePaddingHorizontal = 16;
		buttonLeftSize1.ImagePaddingVertical = 12;
		buttonLeftSize1.Name = "buttonLeftSize1";
		buttonLeftSize1.Text = "1";
		buttonLeftSize1.Click += ButtonLeftSize1_Click;
		buttonLeftSize2.GlobalItem = false;
		buttonLeftSize2.ImagePaddingHorizontal = 16;
		buttonLeftSize2.ImagePaddingVertical = 12;
		buttonLeftSize2.Name = "buttonLeftSize2";
		buttonLeftSize2.Text = "2";
		buttonLeftSize2.Click += ButtonLeftSize1_Click;
		buttonLeftSize3.GlobalItem = false;
		buttonLeftSize3.ImagePaddingHorizontal = 16;
		buttonLeftSize3.ImagePaddingVertical = 12;
		buttonLeftSize3.Name = "buttonLeftSize3";
		buttonLeftSize3.Text = "3";
		buttonLeftSize3.Click += ButtonLeftSize1_Click;
		buttonLeftSize4.GlobalItem = false;
		buttonLeftSize4.ImagePaddingHorizontal = 16;
		buttonLeftSize4.ImagePaddingVertical = 12;
		buttonLeftSize4.Name = "buttonLeftSize4";
		buttonLeftSize4.Text = "4";
		buttonLeftSize4.Click += ButtonLeftSize1_Click;
		buttonLeftSize5.GlobalItem = false;
		buttonLeftSize5.ImagePaddingHorizontal = 16;
		buttonLeftSize5.ImagePaddingVertical = 12;
		buttonLeftSize5.Name = "buttonLeftSize5";
		buttonLeftSize5.Text = "5";
		buttonLeftSize5.Click += ButtonLeftSize1_Click;
		((Control)buttonTopSelector).AccessibleRole = (AccessibleRole)43;
		buttonTopSelector.AutoExpandOnClick = true;
		buttonTopSelector.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonTopSelector).Location = new Point(131, 38);
		buttonTopSelector.LockClosePopup = false;
		((Control)buttonTopSelector).Name = "buttonTopSelector";
		((Control)buttonTopSelector).Size = new Size(16, 22);
		buttonTopSelector.SubItems.AddRange(new BaseItem[13]
		{
			buttonTopSolid, buttonTopDash, buttonTopDashDot, buttonTopDashDotDot, buttonTopDot, buttonTopDouble, buttonTopNone, colorPickerTop, buttonTopSize1, buttonTopSize2,
			buttonTopSize3, buttonTopSize4, buttonTopSize5
		});
		((Control)buttonTopSelector).TabIndex = 0;
		((Control)buttonTopSelector).TabStop = false;
		buttonTopSelector.PopupOpen += ButtonTopSelector_PopupOpen;
		buttonTopSolid.GlobalItem = false;
		buttonTopSolid.ImagePaddingHorizontal = 16;
		buttonTopSolid.ImagePaddingVertical = 12;
		buttonTopSolid.Name = "buttonTopSolid";
		buttonTopSolid.Text = "Solid";
		buttonTopSolid.Click += ButtonTopSolid_Click;
		buttonTopDash.GlobalItem = false;
		buttonTopDash.ImagePaddingHorizontal = 16;
		buttonTopDash.ImagePaddingVertical = 12;
		buttonTopDash.Name = "buttonTopDash";
		buttonTopDash.Text = "Dash";
		buttonTopDash.Click += ButtonTopSolid_Click;
		buttonTopDashDot.GlobalItem = false;
		buttonTopDashDot.ImagePaddingHorizontal = 16;
		buttonTopDashDot.ImagePaddingVertical = 12;
		buttonTopDashDot.Name = "buttonTopDashDot";
		buttonTopDashDot.Text = "Dash Dot";
		buttonTopDashDot.Click += ButtonTopSolid_Click;
		buttonTopDashDotDot.GlobalItem = false;
		buttonTopDashDotDot.ImagePaddingHorizontal = 16;
		buttonTopDashDotDot.ImagePaddingVertical = 12;
		buttonTopDashDotDot.Name = "buttonTopDashDotDot";
		buttonTopDashDotDot.Text = "Dash Dot Dot";
		buttonTopDashDotDot.Click += ButtonTopSolid_Click;
		buttonTopDot.GlobalItem = false;
		buttonTopDot.ImagePaddingHorizontal = 16;
		buttonTopDot.ImagePaddingVertical = 12;
		buttonTopDot.Name = "buttonTopDot";
		buttonTopDot.Text = "Dot";
		buttonTopDot.Click += ButtonTopSolid_Click;
		buttonTopDouble.GlobalItem = false;
		buttonTopDouble.ImagePaddingHorizontal = 16;
		buttonTopDouble.ImagePaddingVertical = 12;
		buttonTopDouble.Name = "buttonTopDouble";
		buttonTopDouble.Text = "Double";
		buttonTopDouble.Click += ButtonTopSolid_Click;
		buttonTopNone.GlobalItem = false;
		buttonTopNone.ImagePaddingHorizontal = 16;
		buttonTopNone.ImagePaddingVertical = 12;
		buttonTopNone.Name = "buttonTopNone";
		buttonTopNone.Text = "None";
		buttonTopNone.Click += ButtonTopSolid_Click;
		colorPickerTop.BeginGroup = true;
		colorPickerTop.GlobalItem = false;
		colorPickerTop.ImagePaddingHorizontal = 16;
		colorPickerTop.ImagePaddingVertical = 12;
		colorPickerTop.Name = "colorPickerTop";
		colorPickerTop.Text = "Color";
		colorPickerTop.SelectedColorChanged += ColorPickerTop_SelectedColorChanged;
		buttonTopSize1.BeginGroup = true;
		buttonTopSize1.GlobalItem = false;
		buttonTopSize1.ImagePaddingHorizontal = 16;
		buttonTopSize1.ImagePaddingVertical = 12;
		buttonTopSize1.Name = "buttonTopSize1";
		buttonTopSize1.Text = "1";
		buttonTopSize1.Click += ButtonTopSize1_Click;
		buttonTopSize2.GlobalItem = false;
		buttonTopSize2.ImagePaddingHorizontal = 16;
		buttonTopSize2.ImagePaddingVertical = 12;
		buttonTopSize2.Name = "buttonTopSize2";
		buttonTopSize2.Text = "2";
		buttonTopSize2.Click += ButtonTopSize1_Click;
		buttonTopSize3.GlobalItem = false;
		buttonTopSize3.ImagePaddingHorizontal = 16;
		buttonTopSize3.ImagePaddingVertical = 12;
		buttonTopSize3.Name = "buttonTopSize3";
		buttonTopSize3.Text = "3";
		buttonTopSize3.Click += ButtonTopSize1_Click;
		buttonTopSize4.GlobalItem = false;
		buttonTopSize4.ImagePaddingHorizontal = 16;
		buttonTopSize4.ImagePaddingVertical = 12;
		buttonTopSize4.Name = "buttonTopSize4";
		buttonTopSize4.Text = "4";
		buttonTopSize4.Click += ButtonTopSize1_Click;
		buttonTopSize5.GlobalItem = false;
		buttonTopSize5.ImagePaddingHorizontal = 16;
		buttonTopSize5.ImagePaddingVertical = 12;
		buttonTopSize5.Name = "buttonTopSize5";
		buttonTopSize5.Text = "5";
		buttonTopSize5.Click += ButtonTopSize1_Click;
		((Control)buttonRightSelector).AccessibleRole = (AccessibleRole)43;
		buttonRightSelector.AutoExpandOnClick = true;
		buttonRightSelector.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonRightSelector).Location = new Point(153, 141);
		buttonRightSelector.LockClosePopup = false;
		((Control)buttonRightSelector).Name = "buttonRightSelector";
		((Control)buttonRightSelector).Size = new Size(22, 16);
		buttonRightSelector.SubItems.AddRange(new BaseItem[13]
		{
			buttonRightSolid, buttonRightDash, buttonRightDashDot, buttonRightDashDotDot, buttonRightDot, buttonRightDouble, buttonRightNone, colorPickerRight, buttonRightSize1, buttonRightSize2,
			buttonRightSize3, buttonRightSize4, buttonRightSize5
		});
		((Control)buttonRightSelector).TabIndex = 3;
		((Control)buttonRightSelector).TabStop = false;
		buttonRightSelector.PopupOpen += ButtonRightSelector_PopupOpen;
		buttonRightSolid.GlobalItem = false;
		buttonRightSolid.ImagePaddingHorizontal = 16;
		buttonRightSolid.ImagePaddingVertical = 12;
		buttonRightSolid.Name = "buttonRightSolid";
		buttonRightSolid.Text = "Solid";
		buttonRightSolid.Click += ButtonTopSolid_Click;
		buttonRightDash.GlobalItem = false;
		buttonRightDash.ImagePaddingHorizontal = 16;
		buttonRightDash.ImagePaddingVertical = 12;
		buttonRightDash.Name = "buttonRightDash";
		buttonRightDash.Text = "Dash";
		buttonRightDash.Click += ButtonTopSolid_Click;
		buttonRightDashDot.GlobalItem = false;
		buttonRightDashDot.ImagePaddingHorizontal = 16;
		buttonRightDashDot.ImagePaddingVertical = 12;
		buttonRightDashDot.Name = "buttonRightDashDot";
		buttonRightDashDot.Text = "Dash Dot";
		buttonRightDashDot.Click += ButtonTopSolid_Click;
		buttonRightDashDotDot.GlobalItem = false;
		buttonRightDashDotDot.ImagePaddingHorizontal = 16;
		buttonRightDashDotDot.ImagePaddingVertical = 12;
		buttonRightDashDotDot.Name = "buttonRightDashDotDot";
		buttonRightDashDotDot.Text = "Dash Dot Dot";
		buttonRightDashDotDot.Click += ButtonTopSolid_Click;
		buttonRightDot.GlobalItem = false;
		buttonRightDot.ImagePaddingHorizontal = 16;
		buttonRightDot.ImagePaddingVertical = 12;
		buttonRightDot.Name = "buttonRightDot";
		buttonRightDot.Text = "Dot";
		buttonRightDot.Click += ButtonTopSolid_Click;
		buttonRightDouble.GlobalItem = false;
		buttonRightDouble.ImagePaddingHorizontal = 16;
		buttonRightDouble.ImagePaddingVertical = 12;
		buttonRightDouble.Name = "buttonRightDouble";
		buttonRightDouble.Text = "Double";
		buttonRightDouble.Click += ButtonTopSolid_Click;
		buttonRightNone.GlobalItem = false;
		buttonRightNone.ImagePaddingHorizontal = 16;
		buttonRightNone.ImagePaddingVertical = 12;
		buttonRightNone.Name = "buttonRightNone";
		buttonRightNone.Text = "None";
		buttonRightNone.Click += ButtonTopSolid_Click;
		colorPickerRight.BeginGroup = true;
		colorPickerRight.GlobalItem = false;
		colorPickerRight.ImagePaddingHorizontal = 16;
		colorPickerRight.ImagePaddingVertical = 12;
		colorPickerRight.Name = "colorPickerRight";
		colorPickerRight.Text = "Color";
		colorPickerRight.SelectedColorChanged += ColorPickerTop_SelectedColorChanged;
		buttonRightSize1.BeginGroup = true;
		buttonRightSize1.GlobalItem = false;
		buttonRightSize1.ImagePaddingHorizontal = 16;
		buttonRightSize1.ImagePaddingVertical = 12;
		buttonRightSize1.Name = "buttonRightSize1";
		buttonRightSize1.Text = "1";
		buttonRightSize1.Click += ButtonRightSize1_Click;
		buttonRightSize2.GlobalItem = false;
		buttonRightSize2.ImagePaddingHorizontal = 16;
		buttonRightSize2.ImagePaddingVertical = 12;
		buttonRightSize2.Name = "buttonRightSize2";
		buttonRightSize2.Text = "2";
		buttonRightSize2.Click += ButtonRightSize1_Click;
		buttonRightSize3.GlobalItem = false;
		buttonRightSize3.ImagePaddingHorizontal = 16;
		buttonRightSize3.ImagePaddingVertical = 12;
		buttonRightSize3.Name = "buttonRightSize3";
		buttonRightSize3.Text = "3";
		buttonRightSize3.Click += ButtonRightSize1_Click;
		buttonRightSize4.GlobalItem = false;
		buttonRightSize4.ImagePaddingHorizontal = 16;
		buttonRightSize4.ImagePaddingVertical = 12;
		buttonRightSize4.Name = "buttonRightSize4";
		buttonRightSize4.Text = "4";
		buttonRightSize4.Click += ButtonRightSize1_Click;
		buttonRightSize5.GlobalItem = false;
		buttonRightSize5.ImagePaddingHorizontal = 16;
		buttonRightSize5.ImagePaddingVertical = 12;
		buttonRightSize5.Name = "buttonRightSize5";
		buttonRightSize5.Text = "5";
		buttonRightSize5.Click += ButtonRightSize1_Click;
		((Control)buttonBottomSelector).AccessibleRole = (AccessibleRole)43;
		buttonBottomSelector.AutoExpandOnClick = true;
		buttonBottomSelector.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonBottomSelector).Location = new Point(131, 163);
		buttonBottomSelector.LockClosePopup = false;
		((Control)buttonBottomSelector).Name = "buttonBottomSelector";
		((Control)buttonBottomSelector).Size = new Size(16, 22);
		buttonBottomSelector.SubItems.AddRange(new BaseItem[13]
		{
			buttonBottomSolid, buttonBottomDash, buttonBottomDashDot, buttonBottomDashDotDot, buttonBottomDot, buttonBottomDouble, buttonBottomNone, colorPickerBottom, buttonBottomSize1, buttonBottomSize2,
			buttonBottomSize3, buttonBottomSize4, buttonBottomSize5
		});
		((Control)buttonBottomSelector).TabIndex = 4;
		((Control)buttonBottomSelector).TabStop = false;
		buttonBottomSelector.PopupOpen += ButtonBottomSelector_PopupOpen;
		buttonBottomSolid.GlobalItem = false;
		buttonBottomSolid.ImagePaddingHorizontal = 16;
		buttonBottomSolid.ImagePaddingVertical = 12;
		buttonBottomSolid.Name = "buttonBottomSolid";
		buttonBottomSolid.Text = "Solid";
		buttonBottomSolid.Click += ButtonTopSolid_Click;
		buttonBottomDash.GlobalItem = false;
		buttonBottomDash.ImagePaddingHorizontal = 16;
		buttonBottomDash.ImagePaddingVertical = 12;
		buttonBottomDash.Name = "buttonBottomDash";
		buttonBottomDash.Text = "Dash";
		buttonBottomDash.Click += ButtonTopSolid_Click;
		buttonBottomDashDot.GlobalItem = false;
		buttonBottomDashDot.ImagePaddingHorizontal = 16;
		buttonBottomDashDot.ImagePaddingVertical = 12;
		buttonBottomDashDot.Name = "buttonBottomDashDot";
		buttonBottomDashDot.Text = "Dash Dot";
		buttonBottomDashDot.Click += ButtonTopSolid_Click;
		buttonBottomDashDotDot.GlobalItem = false;
		buttonBottomDashDotDot.ImagePaddingHorizontal = 16;
		buttonBottomDashDotDot.ImagePaddingVertical = 12;
		buttonBottomDashDotDot.Name = "buttonBottomDashDotDot";
		buttonBottomDashDotDot.Text = "Dash Dot Dot";
		buttonBottomDashDotDot.Click += ButtonTopSolid_Click;
		buttonBottomDot.GlobalItem = false;
		buttonBottomDot.ImagePaddingHorizontal = 16;
		buttonBottomDot.ImagePaddingVertical = 12;
		buttonBottomDot.Name = "buttonBottomDot";
		buttonBottomDot.Text = "Dot";
		buttonBottomDot.Click += ButtonTopSolid_Click;
		buttonBottomDouble.GlobalItem = false;
		buttonBottomDouble.ImagePaddingHorizontal = 16;
		buttonBottomDouble.ImagePaddingVertical = 12;
		buttonBottomDouble.Name = "buttonBottomDouble";
		buttonBottomDouble.Text = "Double";
		buttonBottomDouble.Click += ButtonTopSolid_Click;
		buttonBottomNone.GlobalItem = false;
		buttonBottomNone.ImagePaddingHorizontal = 16;
		buttonBottomNone.ImagePaddingVertical = 12;
		buttonBottomNone.Name = "buttonBottomNone";
		buttonBottomNone.Text = "None";
		buttonBottomNone.Click += ButtonTopSolid_Click;
		colorPickerBottom.BeginGroup = true;
		colorPickerBottom.GlobalItem = false;
		colorPickerBottom.ImagePaddingHorizontal = 16;
		colorPickerBottom.ImagePaddingVertical = 12;
		colorPickerBottom.Name = "colorPickerBottom";
		colorPickerBottom.Text = "Color";
		colorPickerBottom.SelectedColorChanged += ColorPickerTop_SelectedColorChanged;
		buttonBottomSize1.BeginGroup = true;
		buttonBottomSize1.GlobalItem = false;
		buttonBottomSize1.ImagePaddingHorizontal = 16;
		buttonBottomSize1.ImagePaddingVertical = 12;
		buttonBottomSize1.Name = "buttonBottomSize1";
		buttonBottomSize1.Text = "1";
		buttonBottomSize1.Click += ButtonBottomSize1_Click;
		buttonBottomSize2.GlobalItem = false;
		buttonBottomSize2.ImagePaddingHorizontal = 16;
		buttonBottomSize2.ImagePaddingVertical = 12;
		buttonBottomSize2.Name = "buttonBottomSize2";
		buttonBottomSize2.Text = "2";
		buttonBottomSize2.Click += ButtonBottomSize1_Click;
		buttonBottomSize3.GlobalItem = false;
		buttonBottomSize3.ImagePaddingHorizontal = 16;
		buttonBottomSize3.ImagePaddingVertical = 12;
		buttonBottomSize3.Name = "buttonBottomSize3";
		buttonBottomSize3.Text = "3";
		buttonBottomSize3.Click += ButtonBottomSize1_Click;
		buttonBottomSize4.GlobalItem = false;
		buttonBottomSize4.ImagePaddingHorizontal = 16;
		buttonBottomSize4.ImagePaddingVertical = 12;
		buttonBottomSize4.Name = "buttonBottomSize4";
		buttonBottomSize4.Text = "4";
		buttonBottomSize4.Click += ButtonBottomSize1_Click;
		buttonBottomSize5.GlobalItem = false;
		buttonBottomSize5.ImagePaddingHorizontal = 16;
		buttonBottomSize5.ImagePaddingVertical = 12;
		buttonBottomSize5.Name = "buttonBottomSize5";
		buttonBottomSize5.Text = "5";
		buttonBottomSize5.Click += ButtonBottomSize1_Click;
		((Control)panel_Advanced).BackColor = Color.WhiteSmoke;
		((Control)panel_Advanced).Location = new Point(53, 66);
		((Control)panel_Advanced).Name = "panel_Advanced";
		((Control)panel_Advanced).Size = new Size(94, 91);
		((Control)panel_Advanced).TabIndex = 2;
		((Control)panel_Advanced).Paint += new PaintEventHandler(PanelSample_Paint);
		((Control)groupShadow_A).Controls.Add((Control)(object)labelShadowColor_A);
		((Control)groupShadow_A).Controls.Add((Control)(object)labelShadowSize_A);
		((Control)groupShadow_A).Controls.Add((Control)(object)labelDropShadow_A);
		((Control)groupShadow_A).Controls.Add((Control)(object)colorPickerShadowColor_A);
		((Control)groupShadow_A).Controls.Add((Control)(object)biShadowSize_A);
		((Control)groupShadow_A).Controls.Add((Control)(object)biDropShadow_A);
		groupShadow_A.HeaderHeight = 24;
		((Control)groupShadow_A).Location = new Point(12, 215);
		((Control)groupShadow_A).Name = "groupShadow_A";
		((Control)groupShadow_A).Size = new Size(211, 142);
		((Control)groupShadow_A).TabIndex = 1;
		((Control)groupShadow_A).TabStop = false;
		((Control)groupShadow_A).Text = "Shadow";
		((Control)labelShadowColor_A).ForeColor = Color.DimGray;
		((Control)labelShadowColor_A).Location = new Point(12, 38);
		((Control)labelShadowColor_A).Name = "labelShadowColor_A";
		((Control)labelShadowColor_A).Size = new Size(118, 24);
		((Control)labelShadowColor_A).TabIndex = 20;
		((Control)labelShadowColor_A).Text = "Color";
		labelShadowColor_A.TextAlign = (ContentAlignment)16;
		((Control)labelShadowSize_A).ForeColor = Color.DimGray;
		((Control)labelShadowSize_A).Location = new Point(12, 74);
		((Control)labelShadowSize_A).Name = "labelShadowSize_A";
		((Control)labelShadowSize_A).Size = new Size(106, 24);
		((Control)labelShadowSize_A).TabIndex = 20;
		((Control)labelShadowSize_A).Text = "Shadow Size";
		labelShadowSize_A.TextAlign = (ContentAlignment)16;
		((Control)labelDropShadow_A).ForeColor = Color.DimGray;
		((Control)labelDropShadow_A).Location = new Point(12, 110);
		((Control)labelDropShadow_A).Name = "labelDropShadow_A";
		((Control)labelDropShadow_A).Size = new Size(118, 24);
		((Control)labelDropShadow_A).TabIndex = 20;
		((Control)labelDropShadow_A).Text = "Drop Shadow";
		labelDropShadow_A.TextAlign = (ContentAlignment)16;
		((Control)colorPickerShadowColor_A).AccessibleRole = (AccessibleRole)43;
		colorPickerShadowColor_A.AutoExpandOnClick = true;
		colorPickerShadowColor_A.ColorTable = eButtonColor.OrangeWithBackground;
		colorPickerShadowColor_A.ImagePosition = eImagePosition.Right;
		((Control)colorPickerShadowColor_A).Location = new Point(135, 38);
		colorPickerShadowColor_A.LockClosePopup = false;
		((Control)colorPickerShadowColor_A).Name = "colorPickerShadowColor_A";
		((Control)colorPickerShadowColor_A).Size = new Size(56, 24);
		((Control)colorPickerShadowColor_A).TabIndex = 2;
		((Control)colorPickerShadowColor_A).TabStop = false;
		colorPickerShadowColor_A.SelectedColorChanged += ColorPickerShadowColor_A_SelectedColorChanged;
		((Control)biShadowSize_A).AccessibleRole = (AccessibleRole)43;
		biShadowSize_A.AutoExpandOnClick = true;
		biShadowSize_A.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)biShadowSize_A).Location = new Point(135, 74);
		biShadowSize_A.LockClosePopup = false;
		((Control)biShadowSize_A).Name = "biShadowSize_A";
		((Control)biShadowSize_A).Size = new Size(56, 24);
		biShadowSize_A.SubItems.AddRange(new BaseItem[5] { biShadowSize1_A, biShadowSize2_A, biShadowSize3_A, biShadowSize4_A, biShadowSize5_A });
		((Control)biShadowSize_A).TabIndex = 1;
		((Control)biShadowSize_A).TabStop = false;
		biShadowSize_A.PopupOpen += ButtonShadowSize_A_PopupOpen;
		biShadowSize1_A.GlobalItem = false;
		biShadowSize1_A.ImagePaddingHorizontal = 16;
		biShadowSize1_A.ImagePaddingVertical = 12;
		biShadowSize1_A.Name = "biShadowSize1_A";
		biShadowSize1_A.Text = "1";
		biShadowSize1_A.Click += ButtonShadowSize1_A_Click;
		biShadowSize2_A.GlobalItem = false;
		biShadowSize2_A.ImagePaddingHorizontal = 16;
		biShadowSize2_A.ImagePaddingVertical = 12;
		biShadowSize2_A.Name = "biShadowSize2_A";
		biShadowSize2_A.Text = "2";
		biShadowSize2_A.Click += ButtonShadowSize1_A_Click;
		biShadowSize3_A.GlobalItem = false;
		biShadowSize3_A.ImagePaddingHorizontal = 16;
		biShadowSize3_A.ImagePaddingVertical = 12;
		biShadowSize3_A.Name = "biShadowSize3_A";
		biShadowSize3_A.Text = "3";
		biShadowSize3_A.Click += ButtonShadowSize1_A_Click;
		biShadowSize4_A.GlobalItem = false;
		biShadowSize4_A.ImagePaddingHorizontal = 16;
		biShadowSize4_A.ImagePaddingVertical = 12;
		biShadowSize4_A.Name = "biShadowSize4_A";
		biShadowSize4_A.Text = "4";
		biShadowSize4_A.Click += ButtonShadowSize1_A_Click;
		biShadowSize5_A.GlobalItem = false;
		biShadowSize5_A.ImagePaddingHorizontal = 16;
		biShadowSize5_A.ImagePaddingVertical = 12;
		biShadowSize5_A.Name = "biShadowSize5_A";
		biShadowSize5_A.Text = "5";
		biShadowSize5_A.Click += ButtonShadowSize1_A_Click;
		((Control)biDropShadow_A).AccessibleRole = (AccessibleRole)43;
		biDropShadow_A.AutoCheckOnClick = true;
		biDropShadow_A.ColorTable = eButtonColor.OrangeWithBackground;
		biDropShadow_A.Image = (Image)componentResourceManager.GetObject("biDropShadow_A.Image");
		((Control)biDropShadow_A).Location = new Point(135, 110);
		biDropShadow_A.LockClosePopup = false;
		((Control)biDropShadow_A).Name = "biDropShadow_A";
		((Control)biDropShadow_A).Size = new Size(24, 24);
		((Control)biDropShadow_A).TabIndex = 0;
		((Control)biDropShadow_A).TabStop = false;
		biDropShadow_A.CheckedChanged += ButtonDropShadow_A_CheckedChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(461, 612);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiBorderEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Border";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageSimple).ResumeLayout(false);
		((Control)groupSides).ResumeLayout(false);
		((Control)groupPenStyle).ResumeLayout(false);
		((Control)gbBorderColor).ResumeLayout(false);
		((Control)groupShadow).ResumeLayout(false);
		((Control)pageAdvanced).ResumeLayout(false);
		((Control)groupSides_A).ResumeLayout(false);
		((Control)groupShadow_A).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
