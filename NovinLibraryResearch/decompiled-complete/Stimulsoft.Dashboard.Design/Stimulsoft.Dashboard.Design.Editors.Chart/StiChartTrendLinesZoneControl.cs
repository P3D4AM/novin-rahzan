using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Components.Controls;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Editors.Chart;

[ToolboxItem(false)]
public class StiChartTrendLinesZoneControl : StiZoneControl
{
	private StiChartElement chartElement;

	private StiChartTrendLine trendLine;

	private StiPenStyle penStyle;

	private IContainer components;

	private ColorPickerButton colorBoxColor;

	private StiHitTestVisibleLabel labelColor;

	private StiHitTestVisibleLabel lbFieldIs;

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

	public StiGroupLine groupLine;

	public StiComboBox comboBoxField;

	public StiComboBox comboBoxType;

	private ButtonItem biSize1;

	private ButtonItem biSize2;

	private ButtonItem biSize3;

	private ButtonItem biSize4;

	private ButtonItem biSize5;

	public StiChartTrendLinesZoneControl(StiChartElement chartElement, StiChartTrendLine trendLine)
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
			this.chartElement = chartElement;
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
		StiComboBox stiComboBox = comboBoxType;
		ColorPickerButton colorPickerButton = colorBoxColor;
		int num = (((Control)buttonBorderStyle).Width = ((Control)comboBoxField).Width);
		int width2 = (((Control)colorPickerButton).Width = num);
		((Control)stiComboBox).Width = width2;
	}

	public void Localize()
	{
		buttonSolid.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
		buttonDash.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
		buttonDashDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
		buttonDashDotDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
		buttonDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
		buttonNone.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
		((Control)lbFieldIs).Text = StiLocalization.Get("PropertyMain", "FieldIs");
		((Control)labelType).Text = StiLocalization.Get("PropertyMain", "Type");
		((Control)labelColor).Text = StiLocalization.Get("PropertyMain", "Color");
		((Control)labelStyle).Text = StiLocalization.Get("PropertyMain", "Style");
		((Control)labelWidth).Text = StiLocalization.Get("PropertyMain", "Width");
	}

	private void Build()
	{
		FillFieldsIs();
		FillTypes();
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

	public StiChartTrendLine GetTrendLine()
	{
		return new StiChartTrendLine
		{
			KeyValueMeter = ((((ComboBox)comboBoxField).SelectedItem != null) ? ((StiChartMeterComboBoxItem)((ComboBox)comboBoxField).SelectedItem).Key : ""),
			Type = GetSelectedType(),
			LineColor = colorBoxColor.SelectedColor,
			LineStyle = penStyle,
			LineWidth = (float)((Control)buttonSize).Tag
		};
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

	private void FillFieldsIs()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (StiValueChartMeter value in chartElement.Values)
		{
			string label = StiLabelHelper.GetLabel(value);
			if (dictionary.ContainsKey(label))
			{
				dictionary[label] = 2;
			}
			else
			{
				dictionary.Add(label, 1);
			}
		}
		bool flag = true;
		foreach (StiValueChartMeter value2 in chartElement.Values)
		{
			string text = StiLabelHelper.GetLabel(value2);
			if (dictionary[text] != 1)
			{
				text = text + " - " + StiLocalization.Get("PropertyMain", "Value");
			}
			StiChartMeterComboBoxItem stiChartMeterComboBoxItem = new StiChartMeterComboBoxItem
			{
				Text = text,
				Key = value2.Key,
				SeriesType = value2.SeriesType,
				Field = StiChartConditionalField.Value
			};
			((ComboBox)comboBoxField).Items.Add((object)stiChartMeterComboBoxItem);
			if (value2.Key == trendLine.KeyValueMeter)
			{
				((ComboBox)comboBoxField).SelectedItem = stiChartMeterComboBoxItem;
				flag = false;
			}
		}
		if (flag && ((ComboBox)comboBoxField).Items.Count > 0)
		{
			((ListControl)comboBoxField).SelectedIndex = 0;
		}
	}

	private void FillTypes()
	{
		int selectedIndex = 0;
		int num = 0;
		foreach (StiChartTrendLineType value in Enum.GetValues(typeof(StiChartTrendLineType)))
		{
			if (value == trendLine.Type)
			{
				selectedIndex = num;
			}
			((ComboBox)comboBoxType).Items.Add((object)StiLocalization.Get("PropertyEnum", $"StiChartTrendLineType{value}"));
			num++;
		}
		((ListControl)comboBoxType).SelectedIndex = selectedIndex;
	}

	private StiChartTrendLineType GetSelectedType()
	{
		string text = ((ComboBox)comboBoxType).SelectedItem.ToString();
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiChartTrendLineTypeNone")))
		{
			return StiChartTrendLineType.None;
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiChartTrendLineTypeLinear")))
		{
			return StiChartTrendLineType.Linear;
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiChartTrendLineTypeLogarithmic")))
		{
			return StiChartTrendLineType.Logarithmic;
		}
		if (text.Equals(StiLocalization.Get("PropertyEnum", "StiChartTrendLineTypeExponential")))
		{
			return StiChartTrendLineType.Exponential;
		}
		return StiChartTrendLineType.None;
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
		if (((Control)buttonSize).Tag is double)
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
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiChartTrendLinesZoneControl));
		colorBoxColor = new ColorPickerButton();
		labelColor = new StiHitTestVisibleLabel();
		lbFieldIs = new StiHitTestVisibleLabel();
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
		groupLine = new StiGroupLine();
		comboBoxField = new StiComboBox();
		comboBoxType = new StiComboBox();
		((Control)this).SuspendLayout();
		((Control)colorBoxColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxColor).Anchor = (AnchorStyles)13;
		colorBoxColor.AutoExpandOnClick = true;
		colorBoxColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxColor.EnableMarkup = false;
		colorBoxColor.Is20Button = false;
		((Control)colorBoxColor).Location = new Point(116, 79);
		colorBoxColor.LockClosePopup = false;
		((Control)colorBoxColor).Name = "colorBoxColor";
		((Control)colorBoxColor).Size = new Size(120, 22);
		((Control)colorBoxColor).TabIndex = 6;
		((Control)colorBoxColor).TabStop = false;
		colorBoxColor.TextAlignment = eButtonTextAlignment.Right;
		((Control)labelColor).BackColor = Color.Transparent;
		((Control)labelColor).ForeColor = Color.DimGray;
		((Control)labelColor).Location = new Point(12, 79);
		((Control)labelColor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColor).Name = "labelColor";
		((Control)labelColor).Size = new Size(99, 20);
		((Control)labelColor).TabIndex = 5;
		((Control)labelColor).Text = "Color";
		((Label)labelColor).TextAlign = (ContentAlignment)16;
		((Control)lbFieldIs).BackColor = Color.Transparent;
		((Control)lbFieldIs).ForeColor = Color.DimGray;
		((Control)lbFieldIs).Location = new Point(12, 13);
		((Control)lbFieldIs).Margin = new Padding(2, 0, 2, 0);
		((Control)lbFieldIs).Name = "lbFieldIs";
		((Control)lbFieldIs).Size = new Size(97, 21);
		((Control)lbFieldIs).TabIndex = 1;
		((Control)lbFieldIs).Text = "Field Is";
		((Label)lbFieldIs).TextAlign = (ContentAlignment)16;
		((Control)labelType).BackColor = Color.Transparent;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 46);
		((Control)labelType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(97, 21);
		((Control)labelType).TabIndex = 3;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Control)labelStyle).BackColor = Color.Transparent;
		((Control)labelStyle).ForeColor = Color.DimGray;
		((Control)labelStyle).Location = new Point(12, 112);
		((Control)labelStyle).Name = "labelStyle";
		((Control)labelStyle).Size = new Size(97, 23);
		((Control)labelStyle).TabIndex = 7;
		((Control)labelStyle).Text = "Style";
		((Label)labelStyle).TextAlign = (ContentAlignment)16;
		((Control)buttonBorderStyle).AccessibleRole = (AccessibleRole)43;
		((Control)buttonBorderStyle).Anchor = (AnchorStyles)13;
		buttonBorderStyle.AutoExpandOnClick = true;
		buttonBorderStyle.ColorTable = eButtonColor.OrangeWithBackground;
		buttonBorderStyle.Is20Button = false;
		((Control)buttonBorderStyle).Location = new Point(116, 112);
		buttonBorderStyle.LockClosePopup = false;
		((Control)buttonBorderStyle).Margin = new Padding(4);
		((Control)buttonBorderStyle).Name = "buttonBorderStyle";
		buttonBorderStyle.PopupSide = ePopupSide.Bottom;
		buttonBorderStyle.ShowArrow = false;
		((Control)buttonBorderStyle).Size = new Size(120, 22);
		buttonBorderStyle.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonBorderStyle.SubItems.AddRange(new BaseItem[6] { buttonSolid, buttonDash, buttonDashDot, buttonDashDotDot, buttonDot, buttonNone });
		((Control)buttonBorderStyle).TabIndex = 8;
		((Control)buttonBorderStyle).Text = "buttonX1";
		buttonBorderStyle.TextAlignment = eButtonTextAlignment.Left;
		buttonSolid.GlobalItem = false;
		buttonSolid.Name = "buttonSolid";
		buttonSolid.Text = "Solid";
		buttonSolid.Click += ButtonTypeSelected_Click;
		buttonDash.GlobalItem = false;
		buttonDash.Name = "buttonDash";
		buttonDash.Text = "Dash";
		buttonDash.Click += ButtonTypeSelected_Click;
		buttonDashDot.GlobalItem = false;
		buttonDashDot.Name = "buttonDashDot";
		buttonDashDot.Text = "Dash Dot";
		buttonDashDot.Click += ButtonTypeSelected_Click;
		buttonDashDotDot.GlobalItem = false;
		buttonDashDotDot.Name = "buttonDashDotDot";
		buttonDashDotDot.Text = "Dash Dot Dot";
		buttonDashDotDot.Click += ButtonTypeSelected_Click;
		buttonDot.GlobalItem = false;
		buttonDot.Name = "buttonDot";
		buttonDot.Text = "Dot";
		buttonDot.Click += ButtonTypeSelected_Click;
		buttonNone.GlobalItem = false;
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
		((Control)buttonSize).Location = new Point(116, 145);
		buttonSize.LockClosePopup = false;
		((Control)buttonSize).Name = "buttonSize";
		((Control)buttonSize).Size = new Size(88, 24);
		buttonSize.SubItems.AddRange(new BaseItem[5] { biSize1, biSize2, biSize3, biSize4, biSize5 });
		((Control)buttonSize).TabIndex = 10;
		((Control)buttonSize).TabStop = false;
		buttonSize.PopupOpen += ButtonSize_PopupOpen;
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
		((Control)labelWidth).BackColor = Color.Transparent;
		((Control)labelWidth).ForeColor = Color.DimGray;
		((Control)labelWidth).Location = new Point(12, 144);
		((Control)labelWidth).Name = "labelWidth";
		((Control)labelWidth).Size = new Size(97, 24);
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
		((Control)groupLine).BackColor = Color.FromArgb(250, 250, 250);
		((Control)groupLine).Dock = (DockStyle)2;
		groupLine.LinePosition = StiLinePosition.Bottom;
		groupLine.LineStyle = StiLineStyle.Dot;
		((Control)groupLine).Location = new Point(0, 183);
		((Control)groupLine).Name = "groupLine";
		((Control)groupLine).Size = new Size(257, 2);
		((Control)groupLine).TabIndex = 71;
		((Control)comboBoxField).Anchor = (AnchorStyles)13;
		((Control)comboBoxField).BackColor = SystemColors.Window;
		comboBoxField.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxField).Location = new Point(116, 13);
		((Control)comboBoxField).Name = "comboBoxField";
		((Control)comboBoxField).Size = new Size(120, 21);
		((Control)comboBoxField).TabIndex = 2;
		((Control)comboBoxType).Anchor = (AnchorStyles)13;
		comboBoxType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxType).FormattingEnabled = true;
		((Control)comboBoxType).Location = new Point(116, 46);
		((Control)comboBoxType).Name = "comboBoxType";
		((Control)comboBoxType).Size = new Size(120, 21);
		((Control)comboBoxType).TabIndex = 4;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)comboBoxType);
		((Control)this).Controls.Add((Control)(object)comboBoxField);
		((Control)this).Controls.Add((Control)(object)groupLine);
		((Control)this).Controls.Add((Control)(object)buttonSize);
		((Control)this).Controls.Add((Control)(object)labelWidth);
		((Control)this).Controls.Add((Control)(object)labelStyle);
		((Control)this).Controls.Add((Control)(object)buttonBorderStyle);
		((Control)this).Controls.Add((Control)(object)colorBoxColor);
		((Control)this).Controls.Add((Control)(object)labelColor);
		((Control)this).Controls.Add((Control)(object)labelType);
		((Control)this).Controls.Add((Control)(object)lbFieldIs);
		((Control)this).Name = "StiChartTrendLinesZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(257, 185);
		((Control)this).ResumeLayout(false);
	}
}
