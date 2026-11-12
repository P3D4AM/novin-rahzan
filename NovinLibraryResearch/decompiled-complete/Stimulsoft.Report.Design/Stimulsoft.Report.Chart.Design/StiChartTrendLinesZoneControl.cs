using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Components.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiChartTrendLinesZoneControl : StiZoneControl
{
	private StiTrendLine trendLine;

	private StiPenStyle penStyle;

	private IContainer components;

	private ColorPickerButton colorBoxColor;

	private StiHitTestVisibleLabel labelColor;

	private StiHitTestVisibleLabel labelType;

	private StiHitTestVisibleLabel labelStyle;

	private StiTypesButton buttonBorderStyle;

	private ButtonItem buttonSolid;

	private ButtonItem buttonDash;

	private ButtonItem buttonDashDot;

	private ButtonItem buttonDashDotDot;

	private ButtonItem buttonDot;

	private ButtonItem buttonNone;

	private ImageList imageListPenStyle;

	private ButtonX buttonSize;

	private StiHitTestVisibleLabel labelWidth;

	private ImageList imageListShadowSizes;

	public StiComboBox comboBoxType;

	private ButtonItem biSize1;

	private ButtonItem biSize2;

	private ButtonItem biSize3;

	private ButtonItem biSize4;

	private ButtonItem biSize5;

	private CheckBox checkBoxAllowApplyStyle;

	private StiDropDownBox cbFontSize;

	public StiToolButton tbFontStyleStrikeout;

	public StiComboBox cbFont;

	public StiToolButton tbFontStyleUnderline;

	public StiToolButton tbFontStyleItalic;

	public StiToolButton tbFontStyleBold;

	private StiHitTestVisibleLabel labelFont;

	public Stimulsoft.Controls.StiTextBox textBoxText;

	private StiHitTestVisibleLabel labelText;

	public StiComboBox comboBoxPosition;

	private StiHitTestVisibleLabel labelPosition;

	public StiChartTrendLinesZoneControl(StiTrendLine trendLine)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			buttonSolid.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Solid.png"], allowDispose: false);
			buttonDash.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dash.png"], allowDispose: false);
			buttonDashDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDot.png"], allowDispose: false);
			buttonDashDotDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDotDot.png"], allowDispose: false);
			buttonDot.Image = StiImageResizer.GetImage(imageListPenStyle.Images["Dot.png"], allowDispose: false);
			buttonNone.Image = StiImageResizer.GetImage(imageListPenStyle.Images["None.png"], allowDispose: false);
			Localize();
			base.DrawBorder = false;
			DoScaling();
			biSize1.Tag = 1f;
			biSize2.Tag = 2f;
			biSize3.Tag = 3f;
			biSize4.Tag = 4f;
			biSize5.Tag = 5f;
			this.trendLine = trendLine;
			Build();
		}
	}

	private void DoScaling()
	{
		biSize1.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[0], allowDispose: false);
		biSize2.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[1], allowDispose: false);
		biSize3.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[2], allowDispose: false);
		biSize4.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[3], allowDispose: false);
		biSize5.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[4], allowDispose: false);
		((ButtonBase)tbFontStyleBold).Image = StiReportDesignImages.Editor.FontStyleBold();
		((ButtonBase)tbFontStyleItalic).Image = StiReportDesignImages.Editor.FontStyleItalic();
		((ButtonBase)tbFontStyleUnderline).Image = StiReportDesignImages.Editor.FontStyleUnderline();
		((ButtonBase)tbFontStyleStrikeout).Image = StiReportDesignImages.Editor.FontStyleStrikethrough();
	}

	public void Localize()
	{
		buttonSolid.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
		buttonDash.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
		buttonDashDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
		buttonDashDotDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
		buttonDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
		buttonNone.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
		((Control)labelType).Text = StiLocalization.Get("PropertyMain", "Type");
		((Control)labelColor).Text = StiLocalization.Get("PropertyMain", "Color");
		((Control)labelStyle).Text = StiLocalization.Get("PropertyMain", "Style");
		((Control)labelWidth).Text = StiLocalization.Get("PropertyMain", "Width");
		((Control)labelFont).Text = StiLocalization.Get("PropertyMain", "Font");
		((Control)labelText).Text = StiLocalization.Get("PropertyMain", "Text");
		((Control)labelPosition).Text = StiLocalization.Get("PropertyMain", "Position");
		((Control)checkBoxAllowApplyStyle).Text = StiLocalization.Get("PropertyMain", "AllowApplyStyle");
	}

	private void Build()
	{
		BuildFont();
		FillTypes();
		FillPosition();
		checkBoxAllowApplyStyle.Checked = trendLine.AllowApplyStyle;
		((Control)textBoxText).Text = trendLine.Text;
		colorBoxColor.SelectedColor = trendLine.LineColor;
		penStyle = trendLine.LineStyle;
		RefreshButtonBorderStyle();
		int sizeIndex = GetSizeIndex(trendLine.LineWidth);
		if (sizeIndex != -1)
		{
			buttonSize.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex], allowDispose: false);
		}
		((Control)buttonSize).Tag = trendLine.LineWidth;
	}

	private void BuildFont()
	{
		List<FontFamily> fontFamilies = StiFontCollection.GetFontFamilies();
		((ComboBox)cbFont).Items.Clear();
		fontFamilies.FindAll((FontFamily font) => !string.IsNullOrEmpty(font.GetName(0))).ForEach(delegate(FontFamily font)
		{
			((ComboBox)cbFont).Items.Add((object)font.GetName(0));
		});
		StiComboBox stiComboBox = cbFont;
		Font font2 = trendLine.Font;
		((Control)stiComboBox).Text = ((font2 != null) ? font2.Name : null) ?? ((ComboBox)cbFont).Items[0].ToString();
		StiDropDownBox stiDropDownBox = cbFontSize;
		Font font3 = trendLine.Font;
		((Control)stiDropDownBox).Text = ((font3 != null) ? font3.SizeInPoints.ToString() : null) ?? "8";
		StiToolButton stiToolButton = tbFontStyleBold;
		Font font4 = trendLine.Font;
		stiToolButton.Pushed = font4 != null && font4.Bold;
		StiToolButton stiToolButton2 = tbFontStyleItalic;
		Font font5 = trendLine.Font;
		stiToolButton2.Pushed = font5 != null && font5.Italic;
		StiToolButton stiToolButton3 = tbFontStyleUnderline;
		Font font6 = trendLine.Font;
		stiToolButton3.Pushed = font6 != null && font6.Underline;
		StiToolButton stiToolButton4 = tbFontStyleStrikeout;
		Font font7 = trendLine.Font;
		stiToolButton4.Pushed = font7 != null && font7.Strikeout;
	}

	public StiTrendLine GetTrendLine()
	{
		StiTrendLine trendLineBySelectedType = GetTrendLineBySelectedType();
		trendLineBySelectedType.LineColor = colorBoxColor.SelectedColor;
		trendLineBySelectedType.LineStyle = penStyle;
		trendLineBySelectedType.LineWidth = (float)((Control)buttonSize).Tag;
		trendLineBySelectedType.Font = GetFont();
		trendLineBySelectedType.AllowApplyStyle = checkBoxAllowApplyStyle.Checked;
		trendLineBySelectedType.Position = GetTextPosition();
		trendLineBySelectedType.Text = ((Control)textBoxText).Text;
		return trendLineBySelectedType;
	}

	private Font GetFont()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		FontStyle val = (FontStyle)0;
		if (tbFontStyleBold.Pushed)
		{
			val = (FontStyle)(val | 1);
		}
		if (tbFontStyleItalic.Pushed)
		{
			val = (FontStyle)(val | 2);
		}
		if (tbFontStyleUnderline.Pushed)
		{
			val = (FontStyle)(val | 4);
		}
		if (tbFontStyleStrikeout.Pushed)
		{
			val = (FontStyle)(val | 8);
		}
		float result = 8f;
		if (!float.TryParse(((Control)cbFontSize).Text, out result))
		{
			result = 8f;
		}
		return new Font(((Control)cbFont).Text, result, val);
	}

	private static int GetSizeIndex(float size)
	{
		if (size == 1f)
		{
			return 0;
		}
		if (size == 2f)
		{
			return 1;
		}
		if (size == 3f)
		{
			return 2;
		}
		if (size == 4f)
		{
			return 3;
		}
		if (size == 5f)
		{
			return 4;
		}
		return -1;
	}

	private void RefreshButtonBorderStyle()
	{
		Image image = null;
		string text = string.Empty;
		switch (penStyle)
		{
		case StiPenStyle.Dash:
			image = StiImageResizer.GetImage(imageListPenStyle.Images["Dash.png"], allowDispose: false);
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
			break;
		case StiPenStyle.DashDot:
			image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDot.png"], allowDispose: false);
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
			break;
		case StiPenStyle.DashDotDot:
			image = StiImageResizer.GetImage(imageListPenStyle.Images["DashDotDot.png"], allowDispose: false);
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
			break;
		case StiPenStyle.Dot:
			image = StiImageResizer.GetImage(imageListPenStyle.Images["Dot.png"], allowDispose: false);
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
			break;
		case StiPenStyle.None:
			image = StiImageResizer.GetImage(imageListPenStyle.Images["None.png"], allowDispose: false);
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
			break;
		case StiPenStyle.Solid:
			image = StiImageResizer.GetImage(imageListPenStyle.Images["Solid.png"], allowDispose: false);
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
			break;
		case StiPenStyle.Double:
			image = StiImageResizer.GetImage(imageListPenStyle.Images["None.png"], allowDispose: false);
			text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
			break;
		}
		Size size = TextRenderer.MeasureText(text, ((Control)buttonBorderStyle).Font);
		if (size.Width > 100)
		{
			float num = (float)size.Width / (float)text.Length;
			int length = (int)(100f / num);
			((Control)buttonBorderStyle).Text = text.Substring(0, length) + "...";
		}
		else
		{
			((Control)buttonBorderStyle).Text = text;
		}
		buttonBorderStyle.Image = image;
	}

	private void FillPosition()
	{
		int selectedIndex = 0;
		int num = 0;
		foreach (StiTrendLine.StiTextPosition value in Enum.GetValues(typeof(StiTrendLine.StiTextPosition)))
		{
			if (trendLine.Position == value)
			{
				selectedIndex = num;
			}
			((ComboBox)comboBoxPosition).Items.Add((object)StiLocalization.Get("PropertyEnum", $"StiTextPosition{value}"));
			num++;
		}
		((ListControl)comboBoxPosition).SelectedIndex = selectedIndex;
	}

	private void FillTypes()
	{
		int selectedIndex = 0;
		int num = 0;
		foreach (StiChartTrendLineType value in Enum.GetValues(typeof(StiChartTrendLineType)))
		{
			if (IsEqualType(trendLine, value))
			{
				selectedIndex = num;
			}
			((ComboBox)comboBoxType).Items.Add((object)StiLocalization.Get("PropertyEnum", $"StiChartTrendLineType{value}"));
			num++;
		}
		((ListControl)comboBoxType).SelectedIndex = selectedIndex;
	}

	private StiTrendLine GetTrendLineBySelectedType()
	{
		string text = ((ComboBox)comboBoxType).SelectedItem.ToString();
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiChartTrendLineTypeNone")))
		{
			return new StiTrendLineNone();
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiChartTrendLineTypeLinear")))
		{
			return new StiTrendLineLinear();
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiChartTrendLineTypeLogarithmic")))
		{
			return new StiTrendLineLogarithmic();
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiChartTrendLineTypeExponential")))
		{
			return new StiTrendLineExponential();
		}
		return new StiTrendLineNone();
	}

	private StiTrendLine.StiTextPosition GetTextPosition()
	{
		string text = ((ComboBox)comboBoxPosition).SelectedItem.ToString();
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiTextPositionLeftBottom")))
		{
			return StiTrendLine.StiTextPosition.LeftBottom;
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiTextPositionLeftTop")))
		{
			return StiTrendLine.StiTextPosition.LeftTop;
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiTextPositionRightBottom")))
		{
			return StiTrendLine.StiTextPosition.RightBottom;
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiTextPositionRightTop")))
		{
			return StiTrendLine.StiTextPosition.RightTop;
		}
		return StiTrendLine.StiTextPosition.LeftTop;
	}

	private bool IsEqualType(StiTrendLine line, StiChartTrendLineType trendLineType)
	{
		if ((!(line is StiTrendLineNone) || trendLineType != 0) && (!(line is StiTrendLineExponential) || trendLineType != StiChartTrendLineType.Exponential) && (!(line is StiTrendLineLinear) || trendLineType != StiChartTrendLineType.Linear))
		{
			if (line is StiTrendLineLogarithmic)
			{
				return trendLineType == StiChartTrendLineType.Logarithmic;
			}
			return false;
		}
		return true;
	}

	private void ButtonTypeSelected_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		if (buttonItem == buttonSolid)
		{
			penStyle = StiPenStyle.Solid;
		}
		else if (buttonItem == buttonDash)
		{
			penStyle = StiPenStyle.Dash;
		}
		else if (buttonItem == buttonDashDot)
		{
			penStyle = StiPenStyle.DashDot;
		}
		else if (buttonItem == buttonDashDotDot)
		{
			penStyle = StiPenStyle.DashDotDot;
		}
		else if (buttonItem == buttonDot)
		{
			penStyle = StiPenStyle.Dot;
		}
		else if (buttonItem == buttonNone)
		{
			penStyle = StiPenStyle.None;
		}
		RefreshButtonBorderStyle();
	}

	private void ButtonSize_PopupOpen(object sender, EventArgs e)
	{
		if (((Control)buttonSize).Tag is float)
		{
			biSize1.Checked = (float)((Control)buttonSize).Tag == 1f;
			biSize2.Checked = (float)((Control)buttonSize).Tag == 2f;
			biSize3.Checked = (float)((Control)buttonSize).Tag == 3f;
			biSize4.Checked = (float)((Control)buttonSize).Tag == 4f;
			biSize5.Checked = (float)((Control)buttonSize).Tag == 5f;
		}
	}

	private void ButtonSize1_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		((Control)buttonSize).Tag = (float)buttonItem.Tag;
		int sizeIndex = GetSizeIndex((float)((Control)buttonSize).Tag);
		if (sizeIndex != -1)
		{
			buttonSize.Image = StiImageResizer.GetImage(imageListShadowSizes.Images[sizeIndex], allowDispose: false);
		}
	}

	private void CheckBoxAllowApplyStyle_CheckedChanged(object sender, EventArgs e)
	{
		((Control)colorBoxColor).Enabled = !checkBoxAllowApplyStyle.Checked;
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
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4c: Expected O, but got Unknown
		//IL_0e40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4a: Expected O, but got Unknown
		//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4a: Expected O, but got Unknown
		//IL_0fd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdd: Expected O, but got Unknown
		//IL_1066: Unknown result type (might be due to invalid IL or missing references)
		//IL_1070: Expected O, but got Unknown
		//IL_12c3: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiChartTrendLinesZoneControl));
		colorBoxColor = new ColorPickerButton();
		labelColor = new StiHitTestVisibleLabel();
		labelType = new StiHitTestVisibleLabel();
		labelStyle = new StiHitTestVisibleLabel();
		buttonBorderStyle = new StiTypesButton();
		buttonSolid = new ButtonItem();
		buttonDash = new ButtonItem();
		buttonDashDot = new ButtonItem();
		buttonDashDotDot = new ButtonItem();
		buttonDot = new ButtonItem();
		buttonNone = new ButtonItem();
		imageListPenStyle = new ImageList(components);
		buttonSize = new ButtonX();
		biSize1 = new ButtonItem();
		biSize2 = new ButtonItem();
		biSize3 = new ButtonItem();
		biSize4 = new ButtonItem();
		biSize5 = new ButtonItem();
		labelWidth = new StiHitTestVisibleLabel();
		imageListShadowSizes = new ImageList(components);
		comboBoxType = new StiComboBox();
		checkBoxAllowApplyStyle = new CheckBox();
		cbFontSize = new StiDropDownBox();
		tbFontStyleStrikeout = new StiToolButton();
		cbFont = new StiComboBox();
		tbFontStyleUnderline = new StiToolButton();
		tbFontStyleItalic = new StiToolButton();
		tbFontStyleBold = new StiToolButton();
		labelFont = new StiHitTestVisibleLabel();
		textBoxText = new Stimulsoft.Controls.StiTextBox();
		labelText = new StiHitTestVisibleLabel();
		comboBoxPosition = new StiComboBox();
		labelPosition = new StiHitTestVisibleLabel();
		((Control)this).SuspendLayout();
		((Control)colorBoxColor).AccessibleRole = (AccessibleRole)43;
		colorBoxColor.AutoExpandOnClick = true;
		colorBoxColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxColor.EnableMarkup = false;
		colorBoxColor.Is20Button = false;
		((Control)colorBoxColor).Location = new Point(145, 28);
		colorBoxColor.LockClosePopup = false;
		((Control)colorBoxColor).Name = "colorBoxColor";
		((Control)colorBoxColor).Size = new Size(118, 22);
		((Control)colorBoxColor).TabIndex = 6;
		((Control)colorBoxColor).TabStop = false;
		colorBoxColor.TextAlignment = eButtonTextAlignment.Right;
		((Control)labelColor).BackColor = Color.Transparent;
		((Control)labelColor).ForeColor = Color.DimGray;
		((Control)labelColor).Location = new Point(145, 12);
		((Control)labelColor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColor).Name = "labelColor";
		((Control)labelColor).Size = new Size(99, 14);
		((Control)labelColor).TabIndex = 5;
		((Control)labelColor).Text = "Color";
		((Label)labelColor).TextAlign = (ContentAlignment)16;
		((Control)labelType).BackColor = Color.Transparent;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(14, 12);
		((Control)labelType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(128, 14);
		((Control)labelType).TabIndex = 3;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Control)labelStyle).BackColor = Color.Transparent;
		((Control)labelStyle).ForeColor = Color.DimGray;
		((Control)labelStyle).Location = new Point(276, 12);
		((Control)labelStyle).Name = "labelStyle";
		((Control)labelStyle).Size = new Size(97, 14);
		((Control)labelStyle).TabIndex = 7;
		((Control)labelStyle).Text = "Style";
		((Label)labelStyle).TextAlign = (ContentAlignment)16;
		((Control)buttonBorderStyle).AccessibleRole = (AccessibleRole)43;
		buttonBorderStyle.AutoExpandOnClick = true;
		buttonBorderStyle.ColorTable = eButtonColor.OrangeWithBackground;
		buttonBorderStyle.Is20Button = false;
		((Control)buttonBorderStyle).Location = new Point(275, 28);
		buttonBorderStyle.LockClosePopup = false;
		((Control)buttonBorderStyle).Margin = new Padding(4);
		((Control)buttonBorderStyle).Name = "buttonBorderStyle";
		buttonBorderStyle.PopupSide = ePopupSide.Bottom;
		buttonBorderStyle.ShowArrow = false;
		((Control)buttonBorderStyle).Size = new Size(121, 22);
		buttonBorderStyle.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonBorderStyle.SubItems.AddRange(new BaseItem[6] { buttonSolid, buttonDash, buttonDashDot, buttonDashDotDot, buttonDot, buttonNone });
		((Control)buttonBorderStyle).TabIndex = 8;
		((Control)buttonBorderStyle).Text = "buttonX1";
		buttonBorderStyle.TextAlignment = eButtonTextAlignment.Left;
		buttonSolid.GlobalItem = false;
		buttonSolid.LockClosePopup = false;
		buttonSolid.Name = "buttonSolid";
		buttonSolid.Text = "Solid";
		buttonSolid.Click += ButtonTypeSelected_Click;
		buttonDash.GlobalItem = false;
		buttonDash.LockClosePopup = false;
		buttonDash.Name = "buttonDash";
		buttonDash.Text = "Dash";
		buttonDash.Click += ButtonTypeSelected_Click;
		buttonDashDot.GlobalItem = false;
		buttonDashDot.LockClosePopup = false;
		buttonDashDot.Name = "buttonDashDot";
		buttonDashDot.Text = "Dash Dot";
		buttonDashDot.Click += ButtonTypeSelected_Click;
		buttonDashDotDot.GlobalItem = false;
		buttonDashDotDot.LockClosePopup = false;
		buttonDashDotDot.Name = "buttonDashDotDot";
		buttonDashDotDot.Text = "Dash Dot Dot";
		buttonDashDotDot.Click += ButtonTypeSelected_Click;
		buttonDot.GlobalItem = false;
		buttonDot.LockClosePopup = false;
		buttonDot.Name = "buttonDot";
		buttonDot.Text = "Dot";
		buttonDot.Click += ButtonTypeSelected_Click;
		buttonNone.GlobalItem = false;
		buttonNone.LockClosePopup = false;
		buttonNone.Name = "buttonNone";
		buttonNone.Text = "None";
		buttonNone.Click += ButtonTypeSelected_Click;
		imageListPenStyle.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListPenStyle.ImageStream");
		imageListPenStyle.TransparentColor = Color.Transparent;
		imageListPenStyle.Images.SetKeyName(0, "Dash.png");
		imageListPenStyle.Images.SetKeyName(1, "DashDot.png");
		imageListPenStyle.Images.SetKeyName(2, "DashDotDot.png");
		imageListPenStyle.Images.SetKeyName(3, "Dot.png");
		imageListPenStyle.Images.SetKeyName(4, "Double.png");
		imageListPenStyle.Images.SetKeyName(5, "None.png");
		imageListPenStyle.Images.SetKeyName(6, "Solid.png");
		((Control)buttonSize).AccessibleRole = (AccessibleRole)43;
		buttonSize.AutoExpandOnClick = true;
		buttonSize.ColorTable = eButtonColor.OrangeWithBackground;
		buttonSize.Is20Button = false;
		((Control)buttonSize).Location = new Point(408, 28);
		buttonSize.LockClosePopup = false;
		((Control)buttonSize).Name = "buttonSize";
		((Control)buttonSize).Size = new Size(88, 22);
		buttonSize.SubItems.AddRange(new BaseItem[5] { biSize1, biSize2, biSize3, biSize4, biSize5 });
		((Control)buttonSize).TabIndex = 10;
		((Control)buttonSize).TabStop = false;
		buttonSize.PopupOpen += ButtonSize_PopupOpen;
		biSize1.GlobalItem = false;
		biSize1.ImagePaddingHorizontal = 16;
		biSize1.ImagePaddingVertical = 12;
		biSize1.LockClosePopup = false;
		biSize1.Name = "biSize1";
		biSize1.Text = "1";
		biSize1.Click += ButtonSize1_Click;
		biSize2.GlobalItem = false;
		biSize2.ImagePaddingHorizontal = 16;
		biSize2.ImagePaddingVertical = 12;
		biSize2.LockClosePopup = false;
		biSize2.Name = "biSize2";
		biSize2.Text = "2";
		biSize2.Click += ButtonSize1_Click;
		biSize3.GlobalItem = false;
		biSize3.ImagePaddingHorizontal = 16;
		biSize3.ImagePaddingVertical = 12;
		biSize3.LockClosePopup = false;
		biSize3.Name = "biSize3";
		biSize3.Text = "3";
		biSize3.Click += ButtonSize1_Click;
		biSize4.GlobalItem = false;
		biSize4.ImagePaddingHorizontal = 16;
		biSize4.ImagePaddingVertical = 12;
		biSize4.LockClosePopup = false;
		biSize4.Name = "biSize4";
		biSize4.Text = "4";
		biSize4.Click += ButtonSize1_Click;
		biSize5.GlobalItem = false;
		biSize5.ImagePaddingHorizontal = 16;
		biSize5.ImagePaddingVertical = 12;
		biSize5.LockClosePopup = false;
		biSize5.Name = "biSize5";
		biSize5.Text = "5";
		biSize5.Click += ButtonSize1_Click;
		((Control)labelWidth).BackColor = Color.Transparent;
		((Control)labelWidth).ForeColor = Color.DimGray;
		((Control)labelWidth).Location = new Point(410, 12);
		((Control)labelWidth).Name = "labelWidth";
		((Control)labelWidth).Size = new Size(97, 14);
		((Control)labelWidth).TabIndex = 9;
		((Control)labelWidth).Text = "Size";
		((Label)labelWidth).TextAlign = (ContentAlignment)16;
		imageListShadowSizes.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListShadowSizes.ImageStream");
		imageListShadowSizes.TransparentColor = Color.Transparent;
		imageListShadowSizes.Images.SetKeyName(0, "Size1.png");
		imageListShadowSizes.Images.SetKeyName(1, "Size2.png");
		imageListShadowSizes.Images.SetKeyName(2, "Size3.png");
		imageListShadowSizes.Images.SetKeyName(3, "Size4.png");
		imageListShadowSizes.Images.SetKeyName(4, "Size5.png");
		comboBoxType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxType).FormattingEnabled = true;
		((Control)comboBoxType).Location = new Point(14, 28);
		((Control)comboBoxType).Name = "comboBoxType";
		((Control)comboBoxType).Size = new Size(120, 21);
		((Control)comboBoxType).TabIndex = 4;
		((Control)checkBoxAllowApplyStyle).AutoSize = true;
		((Control)checkBoxAllowApplyStyle).BackColor = Color.Transparent;
		((Control)checkBoxAllowApplyStyle).Location = new Point(17, 112);
		((Control)checkBoxAllowApplyStyle).Name = "checkBoxAllowApplyStyle";
		((Control)checkBoxAllowApplyStyle).Size = new Size(106, 17);
		((Control)checkBoxAllowApplyStyle).TabIndex = 72;
		((Control)checkBoxAllowApplyStyle).Text = "Allow Apply Style";
		((ButtonBase)checkBoxAllowApplyStyle).UseVisualStyleBackColor = false;
		checkBoxAllowApplyStyle.CheckedChanged += CheckBoxAllowApplyStyle_CheckedChanged;
		((Control)cbFontSize).BackColor = SystemColors.Window;
		cbFontSize.DrawMode = (DrawMode)2;
		((Control)cbFontSize).ForeColor = SystemColors.ControlText;
		cbFontSize.IsEditable = true;
		cbFontSize.Items.AddRange(new object[19]
		{
			"5", "6", "7", "8", "9", "10", "11", "12", "14", "16",
			"18", "20", "22", "24", "26", "28", "36", "48", "76"
		});
		((Control)cbFontSize).Location = new Point(408, 78);
		((Control)cbFontSize).Name = "cbFontSize";
		cbFontSize.SelectedIndex = -1;
		cbFontSize.SelectedItem = null;
		cbFontSize.SelectedValue = null;
		((Control)cbFontSize).Size = new Size(39, 21);
		((Control)cbFontSize).TabIndex = 80;
		tbFontStyleStrikeout.Draw3DButton = false;
		tbFontStyleStrikeout.HotDragDrop = false;
		((ButtonBase)tbFontStyleStrikeout).Image = (Image)componentResourceManager.GetObject("tbFontStyleStrikeout.Image");
		((Control)tbFontStyleStrikeout).Location = new Point(531, 77);
		((Control)tbFontStyleStrikeout).Name = "tbFontStyleStrikeout";
		((Control)tbFontStyleStrikeout).Size = new Size(24, 24);
		tbFontStyleStrikeout.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleStrikeout).TabIndex = 77;
		((ButtonBase)tbFontStyleStrikeout).UseVisualStyleBackColor = false;
		cbFont.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbFont).FormattingEnabled = true;
		((Control)cbFont).Location = new Point(275, 78);
		((ComboBox)cbFont).MaxDropDownItems = 30;
		((Control)cbFont).Name = "cbFont";
		((Control)cbFont).Size = new Size(123, 21);
		((Control)cbFont).TabIndex = 73;
		tbFontStyleUnderline.Draw3DButton = false;
		tbFontStyleUnderline.HotDragDrop = false;
		((ButtonBase)tbFontStyleUnderline).Image = (Image)componentResourceManager.GetObject("tbFontStyleUnderline.Image");
		((Control)tbFontStyleUnderline).Location = new Point(507, 77);
		((Control)tbFontStyleUnderline).Name = "tbFontStyleUnderline";
		((Control)tbFontStyleUnderline).Size = new Size(24, 24);
		tbFontStyleUnderline.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleUnderline).TabIndex = 76;
		((ButtonBase)tbFontStyleUnderline).UseVisualStyleBackColor = false;
		tbFontStyleItalic.Draw3DButton = false;
		tbFontStyleItalic.HotDragDrop = false;
		((ButtonBase)tbFontStyleItalic).Image = (Image)componentResourceManager.GetObject("tbFontStyleItalic.Image");
		((Control)tbFontStyleItalic).Location = new Point(483, 77);
		((Control)tbFontStyleItalic).Name = "tbFontStyleItalic";
		((Control)tbFontStyleItalic).Size = new Size(24, 24);
		tbFontStyleItalic.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleItalic).TabIndex = 75;
		((ButtonBase)tbFontStyleItalic).UseVisualStyleBackColor = false;
		tbFontStyleBold.Draw3DButton = false;
		tbFontStyleBold.HotDragDrop = false;
		((ButtonBase)tbFontStyleBold).Image = (Image)componentResourceManager.GetObject("tbFontStyleBold.Image");
		((Control)tbFontStyleBold).Location = new Point(459, 77);
		((Control)tbFontStyleBold).Name = "tbFontStyleBold";
		((Control)tbFontStyleBold).Size = new Size(24, 24);
		tbFontStyleBold.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleBold).TabIndex = 74;
		((ButtonBase)tbFontStyleBold).UseVisualStyleBackColor = false;
		((Control)labelFont).BackColor = Color.Transparent;
		((Control)labelFont).ForeColor = Color.DimGray;
		((Control)labelFont).Location = new Point(276, 62);
		((Control)labelFont).Name = "labelFont";
		((Control)labelFont).Size = new Size(104, 14);
		((Control)labelFont).TabIndex = 81;
		((Control)labelFont).Text = "Font";
		((Control)textBoxText).BackColor = Color.White;
		((Control)textBoxText).ForeColor = SystemColors.ControlText;
		((Control)textBoxText).Location = new Point(14, 78);
		((Control)textBoxText).Name = "textBoxText";
		((Control)textBoxText).Size = new Size(119, 21);
		((Control)textBoxText).TabIndex = 82;
		((Control)labelText).BackColor = Color.Transparent;
		((Control)labelText).ForeColor = Color.DimGray;
		((Control)labelText).Location = new Point(14, 62);
		((Control)labelText).Name = "labelText";
		((Control)labelText).Size = new Size(125, 13);
		((Control)labelText).TabIndex = 83;
		((Control)labelText).Text = "Text";
		comboBoxPosition.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxPosition).FormattingEnabled = true;
		((Control)comboBoxPosition).Location = new Point(145, 78);
		((Control)comboBoxPosition).Name = "comboBoxPosition";
		((Control)comboBoxPosition).Size = new Size(120, 21);
		((Control)comboBoxPosition).TabIndex = 85;
		((Control)labelPosition).BackColor = Color.Transparent;
		((Control)labelPosition).ForeColor = Color.DimGray;
		((Control)labelPosition).Location = new Point(145, 61);
		((Control)labelPosition).Margin = new Padding(2, 0, 2, 0);
		((Control)labelPosition).Name = "labelPosition";
		((Control)labelPosition).Size = new Size(81, 14);
		((Control)labelPosition).TabIndex = 84;
		((Control)labelPosition).Text = "Position";
		((Label)labelPosition).TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)comboBoxPosition);
		((Control)this).Controls.Add((Control)(object)labelPosition);
		((Control)this).Controls.Add((Control)(object)textBoxText);
		((Control)this).Controls.Add((Control)(object)labelText);
		((Control)this).Controls.Add((Control)(object)cbFontSize);
		((Control)this).Controls.Add((Control)(object)tbFontStyleStrikeout);
		((Control)this).Controls.Add((Control)(object)cbFont);
		((Control)this).Controls.Add((Control)(object)tbFontStyleUnderline);
		((Control)this).Controls.Add((Control)(object)tbFontStyleItalic);
		((Control)this).Controls.Add((Control)(object)tbFontStyleBold);
		((Control)this).Controls.Add((Control)(object)labelFont);
		((Control)this).Controls.Add((Control)(object)checkBoxAllowApplyStyle);
		((Control)this).Controls.Add((Control)(object)comboBoxType);
		((Control)this).Controls.Add((Control)(object)buttonSize);
		((Control)this).Controls.Add((Control)(object)labelWidth);
		((Control)this).Controls.Add((Control)(object)labelStyle);
		((Control)this).Controls.Add((Control)(object)buttonBorderStyle);
		((Control)this).Controls.Add((Control)(object)colorBoxColor);
		((Control)this).Controls.Add((Control)(object)labelColor);
		((Control)this).Controls.Add((Control)(object)labelType);
		((Control)this).Name = "StiChartTrendLinesZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(685, 145);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
