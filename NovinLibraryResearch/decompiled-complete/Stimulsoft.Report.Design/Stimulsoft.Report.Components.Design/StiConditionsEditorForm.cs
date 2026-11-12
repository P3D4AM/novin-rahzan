using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Components.Design;

public class StiConditionsEditorForm : Form
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private bool allowExpression;

	private IContainer components;

	private StiZoneContainerControl zcConditions;

	private ImageList ilButtons;

	private StiReport report;

	private Button buttonCancel;

	private Button buttonOk;

	private StiConditionsCollection conditions;

	private StiComponent component;

	private StiGroupLine stiGroupLine1;

	private ButtonX buttonAddCondition;

	private ButtonItem buttonNewHightlightCondition;

	private ButtonItem buttonNewDataBarCondition;

	private ButtonItem buttonNewColorScaleCondition;

	private ButtonItem buttonNewIconSetCondition;

	private static string NoCondition => "[" + StiLocalization.Get("Report", "No") + "]";

	public StiConditionsEditorForm(StiReport report, StiConditionsCollection conditions, StiComponent component)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			DoScaling();
			this.report = report;
			this.conditions = conditions;
			this.component = component;
			allowExpression = component is StiText;
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			BuildConditions(conditions);
			((Form)this).ShowIcon = false;
			zcConditions.EmptyImage = StiReportDesignImages.Actions.Conditions(StiImageSize.Double, allowCache: false);
			zcConditions.EmptyText = StiLocalization.Get("Chart", "NoConditions");
			buttonNewDataBarCondition.Enabled = allowExpression;
			buttonNewColorScaleCondition.Enabled = allowExpression;
			buttonNewIconSetCondition.Enabled = allowExpression;
		}
	}

	private void DoScaling()
	{
		buttonNewHightlightCondition.Image = StiReportDesignImages.Actions.NewHighlightCondition(StiImageSize.OneHalf);
		buttonNewDataBarCondition.Image = StiReportDesignImages.Actions.NewDataBarCondition(StiImageSize.OneHalf);
		buttonNewColorScaleCondition.Image = StiReportDesignImages.Actions.NewColorScaleCondition(StiImageSize.OneHalf);
		buttonNewIconSetCondition.Image = StiReportDesignImages.Actions.NewIconSetCondition(StiImageSize.OneHalf);
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormConditions", "title");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonAddCondition).Text = StiLocalization.Get("FormConditions", "AddCondition");
		buttonNewHightlightCondition.Text = StiLocalization.Get("PropertyMain", "HighlightCondition");
		buttonNewDataBarCondition.Text = StiLocalization.Get("PropertyMain", "DataBarCondition");
		buttonNewColorScaleCondition.Text = StiLocalization.Get("PropertyMain", "ColorScaleCondition");
		buttonNewIconSetCondition.Text = StiLocalization.Get("PropertyMain", "IconSetCondition");
	}

	private StiFilter GetFilterCondition(StiConditionItemZoneControl control)
	{
		StiFilterItem item = StiFilterItem.Value;
		StiFilterCondition stiFilterCondition = StiFilterCondition.EqualTo;
		string value = ((Control)control.tbValue1).Text;
		if (((Control)control.comboBoxBoolean).Visible)
		{
			value = ((Control)control.comboBoxBoolean).Text;
		}
		string value2 = ((Control)control.tbValue2).Text;
		string expression = "{" + ((Control)control.tbExpression).Text + "}";
		string text = ((Control)control.cbColumn).Text;
		if (text == NoCondition)
		{
			text = string.Empty;
		}
		StiFilterDataType stiFilterDataType = StiFilterDataType.String;
		if (((Control)control.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValue"))
		{
			item = StiFilterItem.Value;
			expression = string.Empty;
		}
		else if (((Control)control.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemExpression"))
		{
			item = StiFilterItem.Expression;
			value = (value2 = string.Empty);
		}
		stiFilterCondition = StiFilterHelper.ConvertStringToCondition(((Control)control.cbCondition).Text);
		if (((Control)control.cbCondition).Text != StiLocalization.Get("PropertyEnum", "StiFilterConditionBetween") && ((Control)control.cbCondition).Text != StiLocalization.Get("PropertyEnum", "StiFilterConditionNotBetween"))
		{
			value2 = string.Empty;
		}
		stiFilterDataType = StiFilterHelper.ConvertStringToDataType(((Control)control.cbDataType).Text);
		if (stiFilterDataType == StiFilterDataType.DateTime)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				value = control.dtpValue1.Value.ToShortDateString();
				value2 = control.dtpValue2.Value.ToShortDateString();
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		else
		{
			value = ((Control)control.tbValue1).Text;
			if (((Control)control.comboBoxBoolean).Visible)
			{
				value = ((Control)control.comboBoxBoolean).Text;
			}
			value2 = ((Control)control.tbValue2).Text;
		}
		return new StiFilter(item, text, stiFilterCondition, value, value2, stiFilterDataType, expression);
	}

	public void FillConditions(StiConditionsCollection conditions)
	{
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		conditions.Clear();
		for (int num = ((ArrangedElementCollection)((Control)zcConditions).Controls).Count - 1; num >= 0; num--)
		{
			if (((Control)zcConditions).Controls[num] is StiConditionZoneControl)
			{
				StiConditionZoneControl stiConditionZoneControl = ((Control)zcConditions).Controls[num] as StiConditionZoneControl;
				Color selectedColor = stiConditionZoneControl.tbTextColor.SelectedColor;
				Color selectedColor2 = stiConditionZoneControl.tbBackColor.SelectedColor;
				Font selectedFont = stiConditionZoneControl.SelectedFont;
				bool @checked = stiConditionZoneControl.cbEnabled.Checked;
				bool canAssignExpression = stiConditionZoneControl.checkBoxCanAssignExpression.Checked && allowExpression;
				string assignExpression = StiExpressionPacker.UnPackExpression(((Control)stiConditionZoneControl.textBoxAssignExpression).Text, report.Designer.DesignerControl, useBraces: false);
				bool checked2 = ((CheckBox)stiConditionZoneControl.checkBoxBreakIfTrue).Checked;
				StiConditionBorderSides stiConditionBorderSides = StiConditionBorderSides.NotAssigned;
				if (stiConditionZoneControl.buttonBordersAll.Checked)
				{
					stiConditionBorderSides = StiConditionBorderSides.All;
				}
				else if (stiConditionZoneControl.buttonBordersNone.Checked)
				{
					stiConditionBorderSides = StiConditionBorderSides.None;
				}
				else
				{
					stiConditionBorderSides = StiConditionBorderSides.None;
					if (stiConditionZoneControl.buttonBordersBottom.Checked)
					{
						stiConditionBorderSides |= StiConditionBorderSides.Bottom;
					}
					if (stiConditionZoneControl.buttonBordersTop.Checked)
					{
						stiConditionBorderSides |= StiConditionBorderSides.Top;
					}
					if (stiConditionZoneControl.buttonBordersLeft.Checked)
					{
						stiConditionBorderSides |= StiConditionBorderSides.Left;
					}
					if (stiConditionZoneControl.buttonBordersRight.Checked)
					{
						stiConditionBorderSides |= StiConditionBorderSides.Right;
					}
					if (stiConditionBorderSides == StiConditionBorderSides.None)
					{
						stiConditionBorderSides = StiConditionBorderSides.NotAssigned;
					}
				}
				StiConditionPermissions stiConditionPermissions = StiConditionPermissions.None;
				if (stiConditionZoneControl.buttonItemAllowFontName.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.Font;
				}
				if (stiConditionZoneControl.buttonItemAllowFontSize.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.FontSize;
				}
				if (stiConditionZoneControl.buttonItemAllowFontBold.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.FontStyleBold;
				}
				if (stiConditionZoneControl.buttonItemAllowFontItalic.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.FontStyleItalic;
				}
				if (stiConditionZoneControl.buttonItemAllowFontUnderline.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.FontStyleUnderline;
				}
				if (stiConditionZoneControl.buttonItemAllowFontStrikeout.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.FontStyleStrikeout;
				}
				if (stiConditionZoneControl.buttonItemAllowTextColor.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.TextColor;
				}
				if (stiConditionZoneControl.buttonItemAllowBackColor.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.BackColor;
				}
				if (stiConditionZoneControl.buttonItemAllowBorders.Checked)
				{
					stiConditionPermissions |= StiConditionPermissions.Borders;
				}
				string text = ((stiConditionZoneControl.SelectedStyle != null) ? stiConditionZoneControl.SelectedStyle.Name : string.Empty);
				if (stiConditionZoneControl.SelectedStyle != null)
				{
					foreach (StiStyle style in StiOptions.Designer.Styles)
					{
						if (stiConditionZoneControl.SelectedStyle == style)
						{
							text = "##" + text + "##";
							break;
						}
					}
				}
				if (((ArrangedElementCollection)((Control)stiConditionZoneControl.panelConditions).Controls).Count == 1)
				{
					StiFilter filterCondition = GetFilterCondition(((Control)stiConditionZoneControl.panelConditions).Controls[0] as StiConditionItemZoneControl);
					StiCondition stiCondition = new StiCondition(filterCondition.Item, filterCondition.Column, filterCondition.Condition, filterCondition.Value1, filterCondition.Value2, filterCondition.DataType, StiExpressionPacker.UnPackExpression(filterCondition.Expression, report.Designer.DesignerControl, useBraces: false), selectedColor, selectedColor2, selectedFont, @checked, canAssignExpression, assignExpression);
					stiCondition.BreakIfTrue = checked2;
					stiCondition.BorderSides = stiConditionBorderSides;
					stiCondition.Permissions = stiConditionPermissions;
					stiCondition.Style = text;
					stiCondition.Tag = ((Control)stiConditionZoneControl).Tag;
					conditions.Add(stiCondition);
				}
				else
				{
					StiMultiCondition stiMultiCondition = new StiMultiCondition(selectedColor, selectedColor2, selectedFont, @checked, (!stiConditionZoneControl.radioButtonAnd.Checked) ? StiFilterMode.Or : StiFilterMode.And, stiConditionZoneControl.checkBoxCanAssignExpression.Checked && allowExpression, assignExpression);
					stiMultiCondition.BorderSides = stiConditionBorderSides;
					stiMultiCondition.Permissions = stiConditionPermissions;
					stiMultiCondition.Style = text;
					stiMultiCondition.BreakIfTrue = checked2;
					stiMultiCondition.Tag = ((Control)stiConditionZoneControl).Tag;
					foreach (StiConditionItemZoneControl item in (ArrangedElementCollection)((Control)stiConditionZoneControl.panelConditions).Controls)
					{
						StiFilter filterCondition2 = GetFilterCondition(item);
						filterCondition2.Expression = StiExpressionPacker.UnPackExpression(filterCondition2.Expression, report.Designer.DesignerControl, useBraces: false);
						stiMultiCondition.Filters.Insert(0, filterCondition2);
					}
					conditions.Add(stiMultiCondition);
				}
			}
			if (((Control)zcConditions).Controls[num] is StiDataBarConditionZoneControl)
			{
				StiDataBarConditionZoneControl stiDataBarConditionZoneControl = ((Control)zcConditions).Controls[num] as StiDataBarConditionZoneControl;
				StiBrushType brushType = ((((ListControl)stiDataBarConditionZoneControl.comboBoxBrush).SelectedIndex != 0) ? StiBrushType.Gradient : StiBrushType.Solid);
				Color selectedColor3 = stiDataBarConditionZoneControl.colorBoxPositiveColor.SelectedColor;
				Color selectedColor4 = stiDataBarConditionZoneControl.colorBoxNegativeColor.SelectedColor;
				bool showBorder = ((ListControl)stiDataBarConditionZoneControl.comboBoxBorders).SelectedIndex != 0;
				Color selectedColor5 = stiDataBarConditionZoneControl.colorBoxPositiveBorderColor.SelectedColor;
				Color selectedColor6 = stiDataBarConditionZoneControl.colorBoxNegativeBorderColor.SelectedColor;
				StiMinimumType minimumType = StiMinimumType.Auto;
				if (((ListControl)stiDataBarConditionZoneControl.comboBoxMinimum).SelectedIndex == 1)
				{
					minimumType = StiMinimumType.Percent;
				}
				else if (((ListControl)stiDataBarConditionZoneControl.comboBoxMinimum).SelectedIndex == 2)
				{
					minimumType = StiMinimumType.Value;
				}
				else if (((ListControl)stiDataBarConditionZoneControl.comboBoxMinimum).SelectedIndex == 3)
				{
					minimumType = StiMinimumType.Minimum;
				}
				float minimumValue = 0f;
				try
				{
					minimumValue = float.Parse(((Control)stiDataBarConditionZoneControl.textBoxMinimum).Text);
				}
				catch
				{
				}
				StiMaximumType maximumType = StiMaximumType.Auto;
				if (((ListControl)stiDataBarConditionZoneControl.comboBoxMaximum).SelectedIndex == 1)
				{
					maximumType = StiMaximumType.Percent;
				}
				else if (((ListControl)stiDataBarConditionZoneControl.comboBoxMaximum).SelectedIndex == 2)
				{
					maximumType = StiMaximumType.Value;
				}
				else if (((ListControl)stiDataBarConditionZoneControl.comboBoxMaximum).SelectedIndex == 3)
				{
					maximumType = StiMaximumType.Maximum;
				}
				float maximumValue = 0f;
				try
				{
					maximumValue = float.Parse(((Control)stiDataBarConditionZoneControl.textBoxMaximum).Text);
				}
				catch
				{
				}
				StiDataBarDirection direction = StiDataBarDirection.Default;
				if (((ListControl)stiDataBarConditionZoneControl.comboBoxDirection).SelectedIndex == 1)
				{
					direction = StiDataBarDirection.LeftToRight;
				}
				if (((ListControl)stiDataBarConditionZoneControl.comboBoxDirection).SelectedIndex == 2)
				{
					direction = StiDataBarDirection.RighToLeft;
				}
				string text2 = ((Control)stiDataBarConditionZoneControl.cbColumn).Text;
				if (text2 == NoCondition)
				{
					text2 = string.Empty;
				}
				StiDataBarCondition stiDataBarCondition = new StiDataBarCondition(text2, brushType, selectedColor3, selectedColor4, showBorder, selectedColor5, selectedColor6, direction, minimumType, minimumValue, maximumType, maximumValue);
				stiDataBarCondition.Tag = ((Control)stiDataBarConditionZoneControl).Tag;
				conditions.Add(stiDataBarCondition);
			}
			if (((Control)zcConditions).Controls[num] is StiColorScaleConditionZoneControl)
			{
				StiColorScaleConditionZoneControl stiColorScaleConditionZoneControl = ((Control)zcConditions).Controls[num] as StiColorScaleConditionZoneControl;
				StiColorScaleType scaleType = ((((ListControl)stiColorScaleConditionZoneControl.comboBoxScaleType).SelectedIndex != 0) ? StiColorScaleType.Color3 : StiColorScaleType.Color2);
				Color selectedColor7 = stiColorScaleConditionZoneControl.colorBoxMinimum.SelectedColor;
				Color selectedColor8 = stiColorScaleConditionZoneControl.colorBoxMid.SelectedColor;
				Color selectedColor9 = stiColorScaleConditionZoneControl.colorBoxMaximum.SelectedColor;
				StiMinimumType minimumType2 = StiMinimumType.Auto;
				if (((ListControl)stiColorScaleConditionZoneControl.comboBoxMinimum).SelectedIndex == 1)
				{
					minimumType2 = StiMinimumType.Percent;
				}
				else if (((ListControl)stiColorScaleConditionZoneControl.comboBoxMinimum).SelectedIndex == 2)
				{
					minimumType2 = StiMinimumType.Value;
				}
				float minimumValue2 = 0f;
				try
				{
					minimumValue2 = float.Parse(((Control)stiColorScaleConditionZoneControl.textBoxMinimum).Text);
				}
				catch
				{
				}
				StiMidType midType = StiMidType.Auto;
				if (((ListControl)stiColorScaleConditionZoneControl.comboBoxMid).SelectedIndex == 1)
				{
					midType = StiMidType.Percent;
				}
				else if (((ListControl)stiColorScaleConditionZoneControl.comboBoxMid).SelectedIndex == 2)
				{
					midType = StiMidType.Value;
				}
				float midValue = 0f;
				try
				{
					midValue = float.Parse(((Control)stiColorScaleConditionZoneControl.textBoxMid).Text);
				}
				catch
				{
				}
				StiMaximumType maximumType2 = StiMaximumType.Auto;
				if (((ListControl)stiColorScaleConditionZoneControl.comboBoxMaximum).SelectedIndex == 1)
				{
					maximumType2 = StiMaximumType.Percent;
				}
				else if (((ListControl)stiColorScaleConditionZoneControl.comboBoxMaximum).SelectedIndex == 2)
				{
					maximumType2 = StiMaximumType.Value;
				}
				float maximumValue2 = 0f;
				try
				{
					maximumValue2 = float.Parse(((Control)stiColorScaleConditionZoneControl.textBoxMaximum).Text);
				}
				catch
				{
				}
				string text3 = ((Control)stiColorScaleConditionZoneControl.cbColumn).Text;
				if (text3 == NoCondition)
				{
					text3 = string.Empty;
				}
				StiColorScaleCondition stiColorScaleCondition = new StiColorScaleCondition(text3, scaleType, selectedColor7, selectedColor8, selectedColor9, minimumType2, minimumValue2, midType, midValue, maximumType2, maximumValue2);
				stiColorScaleCondition.Tag = ((Control)stiColorScaleConditionZoneControl).Tag;
				conditions.Add(stiColorScaleCondition);
			}
			if (((Control)zcConditions).Controls[num] is StiIconSetConditionZoneControl)
			{
				StiIconSetConditionZoneControl stiIconSetConditionZoneControl = ((Control)zcConditions).Controls[num] as StiIconSetConditionZoneControl;
				StiIconSet iconSet = (StiIconSet)((Control)stiIconSetConditionZoneControl.buttonIconSet).Tag;
				ContentAlignment contentAlignment = (ContentAlignment)16;
				if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 0)
				{
					contentAlignment = (ContentAlignment)1;
				}
				else if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 1)
				{
					contentAlignment = (ContentAlignment)2;
				}
				else if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 2)
				{
					contentAlignment = (ContentAlignment)4;
				}
				else if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 3)
				{
					contentAlignment = (ContentAlignment)16;
				}
				else if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 4)
				{
					contentAlignment = (ContentAlignment)32;
				}
				else if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 5)
				{
					contentAlignment = (ContentAlignment)64;
				}
				else if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 6)
				{
					contentAlignment = (ContentAlignment)256;
				}
				else if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 7)
				{
					contentAlignment = (ContentAlignment)512;
				}
				else if (((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex == 8)
				{
					contentAlignment = (ContentAlignment)1024;
				}
				StiIconSetItem itemFromIconSetControl = GetItemFromIconSetControl(stiIconSetConditionZoneControl.panelItem1, stiIconSetConditionZoneControl.buttonIcon1, stiIconSetConditionZoneControl.comboBoxOperation1, stiIconSetConditionZoneControl.comboBoxType1, stiIconSetConditionZoneControl.textBoxValue1);
				StiIconSetItem itemFromIconSetControl2 = GetItemFromIconSetControl(stiIconSetConditionZoneControl.panelItem2, stiIconSetConditionZoneControl.buttonIcon2, stiIconSetConditionZoneControl.comboBoxOperation2, stiIconSetConditionZoneControl.comboBoxType2, stiIconSetConditionZoneControl.textBoxValue2);
				StiIconSetItem itemFromIconSetControl3 = GetItemFromIconSetControl(stiIconSetConditionZoneControl.panelItem3, stiIconSetConditionZoneControl.buttonIcon3, stiIconSetConditionZoneControl.comboBoxOperation3, stiIconSetConditionZoneControl.comboBoxType3, stiIconSetConditionZoneControl.textBoxValue3);
				StiIconSetItem itemFromIconSetControl4 = GetItemFromIconSetControl(stiIconSetConditionZoneControl.panelItem4, stiIconSetConditionZoneControl.buttonIcon4, stiIconSetConditionZoneControl.comboBoxOperation4, stiIconSetConditionZoneControl.comboBoxType4, stiIconSetConditionZoneControl.textBoxValue4);
				StiIconSetItem itemFromIconSetControl5 = GetItemFromIconSetControl(stiIconSetConditionZoneControl.panelItem5, stiIconSetConditionZoneControl.buttonIcon5, stiIconSetConditionZoneControl.comboBoxOperation5, stiIconSetConditionZoneControl.comboBoxType5, stiIconSetConditionZoneControl.textBoxValue5);
				string text4 = ((Control)stiIconSetConditionZoneControl.cbColumn).Text;
				if (text4 == NoCondition)
				{
					text4 = string.Empty;
				}
				StiIconSetCondition stiIconSetCondition = new StiIconSetCondition(text4, iconSet, contentAlignment, itemFromIconSetControl, itemFromIconSetControl2, itemFromIconSetControl3, itemFromIconSetControl4, itemFromIconSetControl5);
				stiIconSetCondition.Tag = ((Control)stiIconSetConditionZoneControl).Tag;
				conditions.Add(stiIconSetCondition);
			}
		}
	}

	public void BuildConditions(StiConditionsCollection conditions)
	{
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		foreach (StiBaseCondition condition in conditions)
		{
			if (condition is StiDataBarCondition)
			{
				StiDataBarCondition stiDataBarCondition = condition as StiDataBarCondition;
				string column = stiDataBarCondition.Column;
				StiBrushType brushType = stiDataBarCondition.BrushType;
				Color positiveColor = stiDataBarCondition.PositiveColor;
				Color negativeColor = stiDataBarCondition.NegativeColor;
				bool showBorder = stiDataBarCondition.ShowBorder;
				Color positiveBorderColor = stiDataBarCondition.PositiveBorderColor;
				Color negativeBorderColor = stiDataBarCondition.NegativeBorderColor;
				StiDataBarDirection direction = stiDataBarCondition.Direction;
				StiMinimumType minimumType = stiDataBarCondition.MinimumType;
				float minimumValue = stiDataBarCondition.MinimumValue;
				StiMaximumType maximumType = stiDataBarCondition.MaximumType;
				float maximumValue = stiDataBarCondition.MaximumValue;
				object tag = stiDataBarCondition.Tag;
				AddLevelDataBar(column, brushType, positiveColor, negativeColor, showBorder, positiveBorderColor, negativeBorderColor, direction, minimumType, minimumValue, maximumType, maximumValue, tag);
			}
			else if (condition is StiColorScaleCondition)
			{
				StiColorScaleCondition stiColorScaleCondition = condition as StiColorScaleCondition;
				string column2 = stiColorScaleCondition.Column;
				StiColorScaleType scaleType = stiColorScaleCondition.ScaleType;
				Color minimumColor = stiColorScaleCondition.MinimumColor;
				Color midColor = stiColorScaleCondition.MidColor;
				Color maximumColor = stiColorScaleCondition.MaximumColor;
				StiMinimumType minimumType2 = stiColorScaleCondition.MinimumType;
				float minimumValue2 = stiColorScaleCondition.MinimumValue;
				StiMidType midType = stiColorScaleCondition.MidType;
				float midValue = stiColorScaleCondition.MidValue;
				StiMaximumType maximumType2 = stiColorScaleCondition.MaximumType;
				float maximumValue2 = stiColorScaleCondition.MaximumValue;
				object tag2 = stiColorScaleCondition.Tag;
				AddLevelColorScale(column2, scaleType, minimumColor, midColor, maximumColor, minimumType2, minimumValue2, midType, midValue, maximumType2, maximumValue2, tag2);
			}
			else if (condition is StiIconSetCondition)
			{
				StiIconSetCondition stiIconSetCondition = condition as StiIconSetCondition;
				AddLevelIconSet(stiIconSetCondition.Column, stiIconSetCondition.IconSet, stiIconSetCondition.ContentAlignment, stiIconSetCondition.IconSetItem1, stiIconSetCondition.IconSetItem2, stiIconSetCondition.IconSetItem3, stiIconSetCondition.IconSetItem4, stiIconSetCondition.IconSetItem5, stiIconSetCondition.Tag, applyDefault: false);
			}
			else if (condition is StiMultiCondition)
			{
				StiMultiCondition stiMultiCondition = condition as StiMultiCondition;
				Color textColor = stiMultiCondition.TextColor;
				Color backColor = stiMultiCondition.BackColor;
				Font font = stiMultiCondition.Font;
				bool enabled = stiMultiCondition.Enabled;
				StiConditionZoneControl stiConditionZoneControl = AddLevelHightlightControl(textColor, backColor, font, enabled, stiMultiCondition.FilterMode, stiMultiCondition.CanAssignExpression, StiExpressionPacker.PackExpression(stiMultiCondition.AssignExpression, report.Designer.DesignerControl, useBraces: false), stiMultiCondition.Style, stiMultiCondition.BorderSides, stiMultiCondition.Permissions, stiMultiCondition.BreakIfTrue);
				((Control)stiConditionZoneControl).Tag = stiMultiCondition.Tag;
				for (int num = stiMultiCondition.Filters.Count - 1; num >= 0; num--)
				{
					StiFilter stiFilter = stiMultiCondition.Filters[num];
					string text = "";
					string item = "";
					string text2 = "";
					string text3 = "";
					text = stiFilter.Expression.Value.Trim();
					if (text.StartsWith("{", StringComparison.InvariantCulture))
					{
						text = text.Substring(1);
					}
					if (text.EndsWith("}", StringComparison.InvariantCulture))
					{
						text = text.Substring(0, text.Length - 1);
					}
					text = StiExpressionPacker.PackExpression(text, report.Designer.DesignerControl, useBraces: false);
					switch (stiFilter.Item)
					{
					case StiFilterItem.Value:
						item = StiLocalization.Get("PropertyEnum", "StiFilterItemValue");
						break;
					case StiFilterItem.Expression:
						item = StiLocalization.Get("PropertyEnum", "StiFilterItemExpression");
						break;
					}
					text3 = StiFilterHelper.ConvertConditionToString(stiFilter.Condition);
					text2 = StiFilterHelper.ConvertDataTypeToString(stiFilter.DataType);
					StiConditionItemZoneControl stiConditionItemZoneControl = AddLevelFilterCondition(item, text3, stiFilter.Column, stiFilter.Value1, stiFilter.Value2, text2, text);
					stiConditionItemZoneControl.ConditionZoneControl = stiConditionZoneControl;
					((Control)stiConditionZoneControl.panelConditions).Controls.Add((Control)(object)stiConditionItemZoneControl);
				}
				stiConditionZoneControl.UpdateStates();
			}
			else
			{
				StiCondition stiCondition = condition as StiCondition;
				Color textColor2 = stiCondition.TextColor;
				Color backColor2 = stiCondition.BackColor;
				Font font2 = stiCondition.Font;
				bool enabled2 = stiCondition.Enabled;
				string item2 = null;
				string text4 = null;
				string column3 = stiCondition.Column;
				string text5 = stiCondition.Expression;
				string value = stiCondition.Value1;
				string value2 = stiCondition.Value2;
				string text6 = null;
				text5 = text5.Trim();
				if (text5.StartsWith("{", StringComparison.InvariantCulture))
				{
					text5 = text5.Substring(1);
				}
				if (text5.EndsWith("}", StringComparison.InvariantCulture))
				{
					text5 = text5.Substring(0, text5.Length - 1);
				}
				text5 = StiExpressionPacker.PackExpression(text5, report.Designer.DesignerControl, useBraces: false);
				switch (stiCondition.Item)
				{
				case StiFilterItem.Value:
					item2 = StiLocalization.Get("PropertyEnum", "StiFilterItemValue");
					break;
				case StiFilterItem.Expression:
					item2 = StiLocalization.Get("PropertyEnum", "StiFilterItemExpression");
					break;
				}
				text4 = StiFilterHelper.ConvertConditionToString(stiCondition.Condition);
				text6 = StiFilterHelper.ConvertDataTypeToString(stiCondition.DataType);
				AddLevelHightlight(item2, text4, column3, value, value2, text6, text5, textColor2, backColor2, font2, enabled2, stiCondition.CanAssignExpression, StiExpressionPacker.PackExpression(stiCondition.AssignExpression, report.Designer.DesignerControl, useBraces: false), stiCondition.Tag, stiCondition.Style, stiCondition.BorderSides, stiCondition.Permissions, stiCondition.BreakIfTrue);
			}
		}
	}

	private void ExpandTreeNode(TreeNode node)
	{
		if (node is TreeNodeEx node2 && node.Tag != null)
		{
			dataBuilder.ExpandedNode(node2, includeCalcColumn: true);
		}
	}

	private bool SelectNode(string[] strs, int index, TreeNodeCollection nodes, TreeView view)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (strs.Length <= index)
		{
			return false;
		}
		string text = strs[index];
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			string str = ((!(val.Tag is StiDataRelation)) ? val.Text : ((StiDataRelation)val.Tag).Name);
			str = StiNameValidator.CorrectName(str, report);
			if (!(str == text))
			{
				continue;
			}
			if (val.Nodes.Count == 0)
			{
				if (strs.Length - 1 != index)
				{
					return false;
				}
				view.SelectedNode = val;
				return true;
			}
			if (val.Nodes.Count == 1 && val.Nodes[0].Text == "Loading")
			{
				ExpandTreeNode(val);
				val.Expand();
			}
			return SelectNode(strs, index + 1, val.Nodes, view);
		}
		return false;
	}

	private void AddLevelHightlight()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		AddLevelHightlight(StiLocalization.Get("PropertyEnum", "StiFilterItemValue"), StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"), string.Empty, string.Empty, string.Empty, StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"), string.Empty, Color.Red, Color.Transparent, new Font("Arial", 8f), enabled: true, canAssignExpression: false, string.Empty, null, string.Empty, StiConditionBorderSides.NotAssigned, StiConditionPermissions.All, breakIfTrue: false);
	}

	private StiConditionZoneControl AddLevelHightlightControl(Color textColor, Color backColor, Font font, bool enabled, StiFilterMode filterMode, bool canAssignExpression, string assignExpression, string style, StiConditionBorderSides sides, StiConditionPermissions perms, bool breakIfTrue)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		StiConditionZoneControl stiConditionZoneControl = new StiConditionZoneControl(report, allowExpression, component);
		((Control)stiConditionZoneControl.toolButtonAddLevel).Click += ButtonAddLevel_Click;
		stiConditionZoneControl.blockUpdateSample = true;
		stiConditionZoneControl.tbTextColor.SelectedColor = textColor;
		stiConditionZoneControl.tbBackColor.SelectedColor = backColor;
		stiConditionZoneControl.SelectedFont = font;
		stiConditionZoneControl.cbEnabled.Checked = enabled;
		stiConditionZoneControl.tbFontStyleBold.Pushed = (font.Style & 1) > 0;
		stiConditionZoneControl.tbFontStyleItalic.Pushed = (font.Style & 2) > 0;
		stiConditionZoneControl.tbFontStyleUnderline.Pushed = (font.Style & 4) > 0;
		stiConditionZoneControl.tbFontStyleStrikeOutPushed = (font.Style & 8) > 0;
		stiConditionZoneControl.checkBoxCanAssignExpression.Checked = canAssignExpression;
		((Control)stiConditionZoneControl.textBoxAssignExpression).Text = assignExpression;
		stiConditionZoneControl.buttonBordersAll.Checked = sides == StiConditionBorderSides.All;
		stiConditionZoneControl.buttonBordersNone.Checked = sides == StiConditionBorderSides.None;
		stiConditionZoneControl.buttonBordersLeft.Checked = (sides & StiConditionBorderSides.Left) > StiConditionBorderSides.None;
		stiConditionZoneControl.buttonBordersRight.Checked = (sides & StiConditionBorderSides.Right) > StiConditionBorderSides.None;
		stiConditionZoneControl.buttonBordersTop.Checked = (sides & StiConditionBorderSides.Top) > StiConditionBorderSides.None;
		stiConditionZoneControl.buttonBordersBottom.Checked = (sides & StiConditionBorderSides.Bottom) > StiConditionBorderSides.None;
		stiConditionZoneControl.buttonItemAllowFontName.Checked = (perms & StiConditionPermissions.Font) > StiConditionPermissions.None;
		stiConditionZoneControl.buttonItemAllowFontSize.Checked = (perms & StiConditionPermissions.FontSize) > StiConditionPermissions.None;
		stiConditionZoneControl.buttonItemAllowFontBold.Checked = (perms & StiConditionPermissions.FontStyleBold) > StiConditionPermissions.None;
		stiConditionZoneControl.buttonItemAllowFontItalic.Checked = (perms & StiConditionPermissions.FontStyleItalic) > StiConditionPermissions.None;
		stiConditionZoneControl.buttonItemAllowFontUnderline.Checked = (perms & StiConditionPermissions.FontStyleUnderline) > StiConditionPermissions.None;
		stiConditionZoneControl.buttonItemAllowFontStrikeout.Checked = (perms & StiConditionPermissions.FontStyleStrikeout) > StiConditionPermissions.None;
		stiConditionZoneControl.buttonItemAllowTextColor.Checked = (perms & StiConditionPermissions.TextColor) > StiConditionPermissions.None;
		stiConditionZoneControl.buttonItemAllowBackColor.Checked = (perms & StiConditionPermissions.BackColor) > StiConditionPermissions.None;
		stiConditionZoneControl.buttonItemAllowBorders.Checked = (perms & StiConditionPermissions.Borders) > StiConditionPermissions.None;
		if (!string.IsNullOrEmpty(style))
		{
			foreach (StiBaseStyle style2 in report.Styles)
			{
				if (!(style2.Name != style))
				{
					stiConditionZoneControl.SelectedStyle = style2;
					break;
				}
			}
			if (stiConditionZoneControl.SelectedStyle == null)
			{
				foreach (StiBaseStyle style3 in StiOptions.Designer.Styles)
				{
					if (!("##" + style3.Name + "##" != style))
					{
						stiConditionZoneControl.SelectedStyle = style3;
						break;
					}
				}
			}
		}
		stiConditionZoneControl.radioButtonAnd.Checked = filterMode == StiFilterMode.And;
		stiConditionZoneControl.radioButtonOr.Checked = filterMode == StiFilterMode.Or;
		((CheckBox)stiConditionZoneControl.checkBoxBreakIfTrue).Checked = breakIfTrue;
		((Control)zcConditions).Controls.Add((Control)(object)stiConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiConditionZoneControl, 0);
		stiConditionZoneControl.blockUpdateSample = false;
		stiConditionZoneControl.UpdateSample();
		return stiConditionZoneControl;
	}

	private void AddLevelHightlight(string item, string condition, string column, string value1, string value2, string dataType, string expression, Color textColor, Color backColor, Font font, bool enabled, bool canAssignExpression, string assignExpression, object tag, string style, StiConditionBorderSides sides, StiConditionPermissions perms, bool breakIfTrue)
	{
		StiConditionZoneControl stiConditionZoneControl = AddLevelHightlightControl(textColor, backColor, font, enabled, StiFilterMode.And, canAssignExpression, assignExpression, style, sides, perms, breakIfTrue);
		((Control)stiConditionZoneControl).Tag = tag;
		StiConditionItemZoneControl stiConditionItemZoneControl = AddLevelFilterCondition(item, condition, column, value1, value2, dataType, expression);
		stiConditionItemZoneControl.ConditionZoneControl = stiConditionZoneControl;
		((Control)stiConditionZoneControl.panelConditions).Controls.Add((Control)(object)stiConditionItemZoneControl);
		stiConditionZoneControl.UpdateStates();
		((Control)zcConditions).Controls.Add((Control)(object)stiConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiConditionZoneControl, 0);
	}

	private void AddLevelDataBar()
	{
		AddLevelDataBar(string.Empty, StiBrushType.Gradient, Color.Green, Color.Red, showBorder: false, Color.DarkGreen, Color.DarkRed, StiDataBarDirection.Default, StiMinimumType.Auto, 0f, StiMaximumType.Auto, 100f, null);
	}

	private StiDataBarConditionZoneControl AddLevelDataBarControl(string column, StiBrushType brushType, Color positiveColor, Color negativeColor, bool showBorder, Color positiveBorderColor, Color negativeBorderColor, StiDataBarDirection direction, StiMinimumType minimum, float minimumValue, StiMaximumType maximum, float maximumValue, object tag)
	{
		StiDataBarConditionZoneControl stiDataBarConditionZoneControl = new StiDataBarConditionZoneControl(component as StiText);
		((Control)stiDataBarConditionZoneControl).Tag = tag;
		((ComboBox)stiDataBarConditionZoneControl.comboBoxBrush).Items.Clear();
		((ComboBox)stiDataBarConditionZoneControl.comboBoxBrush).Items.Add((object)StiLocalization.Get("Report", "StiSolidBrush"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxBrush).Items.Add((object)StiLocalization.Get("Report", "StiGradientBrush"));
		((ListControl)stiDataBarConditionZoneControl.comboBoxBrush).SelectedIndex = ((brushType == StiBrushType.Gradient) ? 1 : 0);
		((ComboBox)stiDataBarConditionZoneControl.comboBoxBorders).Items.Clear();
		((ComboBox)stiDataBarConditionZoneControl.comboBoxBorders).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiPenStyleNone"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxBorders).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiPenStyleSolid"));
		((ListControl)stiDataBarConditionZoneControl.comboBoxBorders).SelectedIndex = (showBorder ? 1 : 0);
		((ComboBox)stiDataBarConditionZoneControl.comboBoxDirection).Items.Clear();
		((ComboBox)stiDataBarConditionZoneControl.comboBoxDirection).Items.Add((object)StiLocalization.Get("PropertyMain", "Default"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendDirectionLeftToRight"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiLegendDirectionRightToLeft"));
		if (direction == StiDataBarDirection.Default)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxDirection).SelectedIndex = 0;
		}
		if (direction == StiDataBarDirection.LeftToRight)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxDirection).SelectedIndex = 1;
		}
		if (direction == StiDataBarDirection.RighToLeft)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxDirection).SelectedIndex = 2;
		}
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMinimum).Items.Clear();
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMinimum).Items.Add((object)StiLocalization.Get("PropertyMain", "Auto"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMinimum).Items.Add((object)StiLocalization.Get("FormFormatEditor", "Percentage"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMinimum).Items.Add((object)StiLocalization.Get("PropertyMain", "Value"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMinimum).Items.Add((object)StiLocalization.Get("PropertyMain", "Minimum"));
		if (minimum == StiMinimumType.Auto)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxMinimum).SelectedIndex = 0;
		}
		if (minimum == StiMinimumType.Percent)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxMinimum).SelectedIndex = 1;
		}
		if (minimum == StiMinimumType.Value)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxMinimum).SelectedIndex = 2;
		}
		if (minimum == StiMinimumType.Minimum)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxMinimum).SelectedIndex = 3;
		}
		((Control)stiDataBarConditionZoneControl.textBoxMinimum).Text = minimumValue.ToString();
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMaximum).Items.Clear();
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMaximum).Items.Add((object)StiLocalization.Get("PropertyMain", "Auto"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMaximum).Items.Add((object)StiLocalization.Get("FormFormatEditor", "Percentage"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMaximum).Items.Add((object)StiLocalization.Get("PropertyMain", "Value"));
		((ComboBox)stiDataBarConditionZoneControl.comboBoxMaximum).Items.Add((object)StiLocalization.Get("PropertyMain", "Maximum"));
		if (maximum == StiMaximumType.Auto)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxMaximum).SelectedIndex = 0;
		}
		if (maximum == StiMaximumType.Percent)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxMaximum).SelectedIndex = 1;
		}
		if (maximum == StiMaximumType.Value)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxMaximum).SelectedIndex = 2;
		}
		if (maximum == StiMaximumType.Maximum)
		{
			((ListControl)stiDataBarConditionZoneControl.comboBoxMaximum).SelectedIndex = 3;
		}
		((Control)stiDataBarConditionZoneControl.textBoxMaximum).Text = maximumValue.ToString();
		if (string.IsNullOrEmpty(column))
		{
			column = NoCondition;
		}
		((Control)stiDataBarConditionZoneControl.cbColumn).Text = column;
		stiDataBarConditionZoneControl.colorBoxPositiveColor.SelectedColor = positiveColor;
		stiDataBarConditionZoneControl.colorBoxNegativeColor.SelectedColor = negativeColor;
		stiDataBarConditionZoneControl.colorBoxPositiveBorderColor.SelectedColor = positiveBorderColor;
		stiDataBarConditionZoneControl.colorBoxNegativeBorderColor.SelectedColor = negativeBorderColor;
		stiDataBarConditionZoneControl.cbColumn.ValueChanged += TreeViewBoxColumn_ValueChanged;
		stiDataBarConditionZoneControl.cbColumn.TreeNodesFill += TreeViewBoxColumn_TreeNodesFill;
		((Control)zcConditions).Controls.Add((Control)(object)stiDataBarConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiDataBarConditionZoneControl, 0);
		stiDataBarConditionZoneControl.UpdateSample();
		return stiDataBarConditionZoneControl;
	}

	private void AddLevelDataBar(string column, StiBrushType brushType, Color positiveColor, Color negativeColor, bool showBorder, Color positiveBorderColor, Color negativeBorderColor, StiDataBarDirection direction, StiMinimumType minimum, float minimumValue, StiMaximumType maximum, float maximumValue, object tag)
	{
		StiDataBarConditionZoneControl stiDataBarConditionZoneControl = AddLevelDataBarControl(column, brushType, positiveColor, negativeColor, showBorder, positiveBorderColor, negativeBorderColor, direction, minimum, minimumValue, maximum, maximumValue, tag);
		((Control)zcConditions).Controls.Add((Control)(object)stiDataBarConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiDataBarConditionZoneControl, 0);
	}

	private void AddLevelColorScale()
	{
		AddLevelColorScale(string.Empty, StiColorScaleType.Color2, Color.Red, Color.Yellow, Color.Green, StiMinimumType.Auto, 0f, StiMidType.Auto, 50f, StiMaximumType.Auto, 100f, null);
	}

	private StiColorScaleConditionZoneControl AddLevelColorScaleControl(string column, StiColorScaleType scaleType, Color minimumColor, Color midColor, Color maximumColor, StiMinimumType minimum, float minimumValue, StiMidType mid, float midValue, StiMaximumType maximum, float maximumValue, object tag)
	{
		StiColorScaleConditionZoneControl stiColorScaleConditionZoneControl = new StiColorScaleConditionZoneControl();
		((Control)stiColorScaleConditionZoneControl).Tag = tag;
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxScaleType).Items.Clear();
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxScaleType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiColorScaleTypeColor2"));
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxScaleType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiColorScaleTypeColor3"));
		((ListControl)stiColorScaleConditionZoneControl.comboBoxScaleType).SelectedIndex = ((scaleType != 0) ? 1 : 0);
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMinimum).Items.Clear();
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMinimum).Items.Add((object)StiLocalization.Get("PropertyMain", "Auto"));
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMinimum).Items.Add((object)StiLocalization.Get("FormFormatEditor", "Percentage"));
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMinimum).Items.Add((object)StiLocalization.Get("PropertyMain", "Value"));
		if (minimum == StiMinimumType.Auto)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMinimum).SelectedIndex = 0;
		}
		if (minimum == StiMinimumType.Percent)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMinimum).SelectedIndex = 1;
		}
		if (minimum == StiMinimumType.Value)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMinimum).SelectedIndex = 2;
		}
		((Control)stiColorScaleConditionZoneControl.textBoxMinimum).Text = minimumValue.ToString();
		stiColorScaleConditionZoneControl.colorBoxMinimum.SelectedColor = minimumColor;
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMid).Items.Clear();
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMid).Items.Add((object)StiLocalization.Get("PropertyMain", "Auto"));
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMid).Items.Add((object)StiLocalization.Get("FormFormatEditor", "Percentage"));
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMid).Items.Add((object)StiLocalization.Get("PropertyMain", "Value"));
		if (mid == StiMidType.Auto)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMid).SelectedIndex = 0;
		}
		if (mid == StiMidType.Percent)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMid).SelectedIndex = 1;
		}
		if (mid == StiMidType.Value)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMid).SelectedIndex = 2;
		}
		((Control)stiColorScaleConditionZoneControl.textBoxMid).Text = midValue.ToString();
		stiColorScaleConditionZoneControl.colorBoxMid.SelectedColor = midColor;
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMaximum).Items.Clear();
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMaximum).Items.Add((object)StiLocalization.Get("PropertyMain", "Auto"));
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMaximum).Items.Add((object)StiLocalization.Get("FormFormatEditor", "Percentage"));
		((ComboBox)stiColorScaleConditionZoneControl.comboBoxMaximum).Items.Add((object)StiLocalization.Get("PropertyMain", "Value"));
		if (maximum == StiMaximumType.Auto)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMaximum).SelectedIndex = 0;
		}
		if (maximum == StiMaximumType.Percent)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMaximum).SelectedIndex = 1;
		}
		if (maximum == StiMaximumType.Value)
		{
			((ListControl)stiColorScaleConditionZoneControl.comboBoxMaximum).SelectedIndex = 2;
		}
		((Control)stiColorScaleConditionZoneControl.textBoxMaximum).Text = maximumValue.ToString();
		stiColorScaleConditionZoneControl.colorBoxMaximum.SelectedColor = maximumColor;
		if (string.IsNullOrEmpty(column))
		{
			column = NoCondition;
		}
		((Control)stiColorScaleConditionZoneControl.cbColumn).Text = column;
		stiColorScaleConditionZoneControl.cbColumn.ValueChanged += TreeViewBoxColumn_ValueChanged;
		stiColorScaleConditionZoneControl.cbColumn.TreeNodesFill += TreeViewBoxColumn_TreeNodesFill;
		((Control)zcConditions).Controls.Add((Control)(object)stiColorScaleConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiColorScaleConditionZoneControl, 0);
		stiColorScaleConditionZoneControl.UpdateSample();
		return stiColorScaleConditionZoneControl;
	}

	private void AddLevelColorScale(string column, StiColorScaleType scaleType, Color minimumColor, Color midColor, Color maximumColor, StiMinimumType minimum, float minimumValue, StiMidType mid, float midValue, StiMaximumType maximum, float maximumValue, object tag)
	{
		StiColorScaleConditionZoneControl stiColorScaleConditionZoneControl = AddLevelColorScaleControl(column, scaleType, minimumColor, midColor, maximumColor, minimum, minimumValue, mid, midValue, maximum, maximumValue, tag);
		((Control)zcConditions).Controls.Add((Control)(object)stiColorScaleConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiColorScaleConditionZoneControl, 0);
	}

	private void AddLevelIconSet()
	{
		AddLevelIconSet(string.Empty, StiIconSet.None, (ContentAlignment)16, new StiIconSetItem(StiIcon.None, StiIconSetOperation.MoreThan, StiIconSetValueType.Percent, 0f), new StiIconSetItem(StiIcon.None, StiIconSetOperation.MoreThan, StiIconSetValueType.Percent, 0f), new StiIconSetItem(StiIcon.None, StiIconSetOperation.MoreThan, StiIconSetValueType.Percent, 0f), new StiIconSetItem(StiIcon.None, StiIconSetOperation.MoreThan, StiIconSetValueType.Percent, 0f), new StiIconSetItem(StiIcon.None, StiIconSetOperation.MoreThan, StiIconSetValueType.Percent, 0f), null, applyDefault: true);
	}

	private StiIconSetConditionZoneControl AddLevelIconSetControl(string column, StiIconSet iconSet, ContentAlignment contentAlignment, StiIconSetItem item1, StiIconSetItem item2, StiIconSetItem item3, StiIconSetItem item4, StiIconSetItem item5, object tag, bool applyDefault)
	{
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Invalid comparison between Unknown and I4
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Invalid comparison between Unknown and I4
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Invalid comparison between Unknown and I4
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Invalid comparison between Unknown and I4
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Invalid comparison between Unknown and I4
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Invalid comparison between Unknown and I4
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Invalid comparison between Unknown and I4
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Invalid comparison between Unknown and I4
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Invalid comparison between Unknown and I4
		StiIconSetConditionZoneControl stiIconSetConditionZoneControl = new StiIconSetConditionZoneControl();
		((Control)stiIconSetConditionZoneControl).Tag = tag;
		((Control)stiIconSetConditionZoneControl.buttonIconSet).Tag = iconSet;
		stiIconSetConditionZoneControl.buttonIconSet.Image = stiIconSetConditionZoneControl.GetIconSet(iconSet);
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Clear();
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft"));
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter"));
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight"));
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft"));
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter"));
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight"));
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft"));
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter"));
		((ComboBox)stiIconSetConditionZoneControl.comboBoxAlignment).Items.Add((object)StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight"));
		if ((int)contentAlignment == 1)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 0;
		}
		else if ((int)contentAlignment == 2)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 1;
		}
		else if ((int)contentAlignment == 4)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 2;
		}
		else if ((int)contentAlignment == 16)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 3;
		}
		else if ((int)contentAlignment == 32)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 4;
		}
		else if ((int)contentAlignment == 64)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 5;
		}
		else if ((int)contentAlignment == 256)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 6;
		}
		else if ((int)contentAlignment == 512)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 7;
		}
		else if ((int)contentAlignment == 1024)
		{
			((ListControl)stiIconSetConditionZoneControl.comboBoxAlignment).SelectedIndex = 8;
		}
		SetItemToIconSetControl(item1, stiIconSetConditionZoneControl.panelItem1, stiIconSetConditionZoneControl.buttonIcon1, stiIconSetConditionZoneControl.comboBoxOperation1, stiIconSetConditionZoneControl.comboBoxType1, stiIconSetConditionZoneControl.textBoxValue1);
		SetItemToIconSetControl(item2, stiIconSetConditionZoneControl.panelItem2, stiIconSetConditionZoneControl.buttonIcon2, stiIconSetConditionZoneControl.comboBoxOperation2, stiIconSetConditionZoneControl.comboBoxType2, stiIconSetConditionZoneControl.textBoxValue2);
		SetItemToIconSetControl(item3, stiIconSetConditionZoneControl.panelItem3, stiIconSetConditionZoneControl.buttonIcon3, stiIconSetConditionZoneControl.comboBoxOperation3, stiIconSetConditionZoneControl.comboBoxType3, stiIconSetConditionZoneControl.textBoxValue3);
		SetItemToIconSetControl(item4, stiIconSetConditionZoneControl.panelItem4, stiIconSetConditionZoneControl.buttonIcon4, stiIconSetConditionZoneControl.comboBoxOperation4, stiIconSetConditionZoneControl.comboBoxType4, stiIconSetConditionZoneControl.textBoxValue4);
		SetItemToIconSetControl(item5, stiIconSetConditionZoneControl.panelItem5, stiIconSetConditionZoneControl.buttonIcon5, stiIconSetConditionZoneControl.comboBoxOperation5, stiIconSetConditionZoneControl.comboBoxType5, stiIconSetConditionZoneControl.textBoxValue5);
		if (string.IsNullOrEmpty(column))
		{
			column = NoCondition;
		}
		((Control)stiIconSetConditionZoneControl.cbColumn).Text = column;
		stiIconSetConditionZoneControl.cbColumn.ValueChanged += TreeViewBoxColumn_ValueChanged;
		stiIconSetConditionZoneControl.cbColumn.TreeNodesFill += TreeViewBoxColumn_TreeNodesFill;
		if (applyDefault)
		{
			stiIconSetConditionZoneControl.ApplyIconSet(StiIconSet.TrafficLightsUnrimmed3);
		}
		((Control)zcConditions).Controls.Add((Control)(object)stiIconSetConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiIconSetConditionZoneControl, 0);
		return stiIconSetConditionZoneControl;
	}

	private static void SetItemToIconSetControl(StiIconSetItem item, Panel panel, ButtonX buttonIcon, StiComboBox comboBoxOperation, StiComboBox comboBoxType, StiTextBox textBoxValue)
	{
		if (item != null)
		{
			((Control)buttonIcon).Tag = item.Icon;
			buttonIcon.Image = StiIconSetHelper.GetIcon(item.Icon);
			((ListControl)comboBoxOperation).SelectedIndex = ((item.Operation == StiIconSetOperation.MoreThan) ? 1 : 0);
			((ComboBox)comboBoxType).Items.Clear();
			((ComboBox)comboBoxType).Items.Add((object)StiLocalization.Get("FormFormatEditor", "Percentage"));
			((ComboBox)comboBoxType).Items.Add((object)StiLocalization.Get("PropertyMain", "Value"));
			((ListControl)comboBoxType).SelectedIndex = ((item.ValueType != StiIconSetValueType.Percent) ? 1 : 0);
			((Control)textBoxValue).Text = item.Value.ToString();
			((Control)panel).Visible = true;
		}
		else
		{
			((Control)buttonIcon).Tag = StiIcon.None;
			buttonIcon.Image = null;
			((ListControl)comboBoxOperation).SelectedIndex = 0;
			((ListControl)comboBoxType).SelectedIndex = 0;
			((Control)textBoxValue).Text = "0";
			((Control)panel).Visible = false;
		}
	}

	private static StiIconSetItem GetItemFromIconSetControl(Panel panel, ButtonX buttonIcon, StiComboBox comboBoxOperation, StiComboBox comboBoxType, StiTextBox textBoxValue)
	{
		if (!((Control)panel).Visible)
		{
			return null;
		}
		StiIconSetItem stiIconSetItem = new StiIconSetItem();
		stiIconSetItem.Icon = (StiIcon)((Control)buttonIcon).Tag;
		stiIconSetItem.Operation = ((((ListControl)comboBoxOperation).SelectedIndex == 0) ? StiIconSetOperation.MoreThanOrEqual : StiIconSetOperation.MoreThan);
		stiIconSetItem.ValueType = ((((ListControl)comboBoxType).SelectedIndex == 0) ? StiIconSetValueType.Percent : StiIconSetValueType.Value);
		try
		{
			stiIconSetItem.Value = float.Parse(((Control)textBoxValue).Text);
		}
		catch
		{
		}
		return stiIconSetItem;
	}

	private static void CheckHeightIconSet(StiIconSetConditionZoneControl control)
	{
		int num = 0;
		if (((Control)control.panelItem1).Visible)
		{
			num += ((Control)control.panelItem1).Height;
		}
		if (((Control)control.panelItem2).Visible)
		{
			num += ((Control)control.panelItem2).Height;
		}
		if (((Control)control.panelItem3).Visible)
		{
			num += ((Control)control.panelItem3).Height;
		}
		if (((Control)control.panelItem4).Visible)
		{
			num += ((Control)control.panelItem4).Height;
		}
		if (((Control)control.panelItem5).Visible)
		{
			num += ((Control)control.panelItem5).Height;
		}
		((Control)control.panelItems).Height = num;
		((Control)control).Height = ((Control)control.panelItems).Top + num + 10;
	}

	private void AddLevelIconSet(string column, StiIconSet iconSet, ContentAlignment contentAlignment, StiIconSetItem item1, StiIconSetItem item2, StiIconSetItem item3, StiIconSetItem item4, StiIconSetItem item5, object tag, bool applyDefault)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		StiIconSetConditionZoneControl stiIconSetConditionZoneControl = AddLevelIconSetControl(column, iconSet, contentAlignment, item1, item2, item3, item4, item5, tag, applyDefault: false);
		if (applyDefault)
		{
			((Control)stiIconSetConditionZoneControl).SuspendLayout();
			stiIconSetConditionZoneControl.ApplyIconSet(StiIconSet.TrafficLightsUnrimmed3);
			((Control)stiIconSetConditionZoneControl).ResumeLayout();
		}
		CheckHeightIconSet(stiIconSetConditionZoneControl);
		((Control)zcConditions).Controls.Add((Control)(object)stiIconSetConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiIconSetConditionZoneControl, 0);
	}

	private StiConditionItemZoneControl AddLevelFilterCondition(string item, string condition, string column, string value1, string value2, string dataType, string expression)
	{
		StiConditionItemZoneControl stiConditionItemZoneControl = new StiConditionItemZoneControl(report);
		((ComboBox)stiConditionItemZoneControl.cbItem).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValue"));
		((ComboBox)stiConditionItemZoneControl.cbItem).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemExpression"));
		((Control)stiConditionItemZoneControl.cbItem).Text = item;
		if (string.IsNullOrEmpty(column))
		{
			column = NoCondition;
		}
		((Control)stiConditionItemZoneControl.cbColumn).Text = column;
		((Control)stiConditionItemZoneControl.tbExpression).Text = expression;
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBetween"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotBetween"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNull"));
		((ComboBox)stiConditionItemZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNotNull"));
		((Control)stiConditionItemZoneControl.cbCondition).Text = condition;
		((ComboBox)stiConditionItemZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"));
		((ComboBox)stiConditionItemZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeNumeric"));
		((ComboBox)stiConditionItemZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"));
		((ComboBox)stiConditionItemZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"));
		((ComboBox)stiConditionItemZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeExpression"));
		((Control)stiConditionItemZoneControl.cbDataType).Text = dataType;
		if (dataType == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"))
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				stiConditionItemZoneControl.dtpValue1.Value = DateTime.Parse(value1);
				stiConditionItemZoneControl.dtpValue2.Value = DateTime.Parse(value2);
			}
			catch
			{
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		else
		{
			((Control)stiConditionItemZoneControl.tbValue1).Text = value1;
			((Control)stiConditionItemZoneControl.comboBoxBoolean).Text = value1;
			((Control)stiConditionItemZoneControl.tbValue2).Text = value2;
		}
		((Control)stiConditionItemZoneControl).Dock = (DockStyle)1;
		stiConditionItemZoneControl.Closed += ButtonRemoveLevel_Click;
		stiConditionItemZoneControl.cbColumn.ValueChanged += TreeViewBoxColumn_ValueChanged;
		stiConditionItemZoneControl.cbColumn.TreeNodesFill += TreeViewBoxColumn_TreeNodesFill;
		return stiConditionItemZoneControl;
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_conditional_formatting.htm");
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void ThisOk_Click(object sender, EventArgs e)
	{
		component.Report.Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Conditions"));
		FillConditions(conditions);
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void ThisCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void ButtonAddLevel_Click(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		StiConditionItemZoneControl stiConditionItemZoneControl = AddLevelFilterCondition(StiLocalization.Get("PropertyEnum", "StiFilterItemValue"), StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"), string.Empty, string.Empty, string.Empty, StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"), string.Empty);
		StiConditionZoneControl stiConditionZoneControl2 = (stiConditionItemZoneControl.ConditionZoneControl = ((Control)sender).Parent as StiConditionZoneControl);
		((Control)stiConditionZoneControl2.panelConditions).Controls.Add((Control)(object)stiConditionItemZoneControl);
		((Control)stiConditionZoneControl2.panelConditions).Controls.SetChildIndex((Control)(object)stiConditionItemZoneControl, 0);
	}

	private void ButtonRemoveLevel_Click(object sender, EventArgs e)
	{
		StiConditionItemZoneControl stiConditionItemZoneControl = sender as StiConditionItemZoneControl;
		((Control)stiConditionItemZoneControl).Parent.Controls.Remove((Control)(object)stiConditionItemZoneControl);
		StiConditionZoneControl conditionZoneControl = stiConditionItemZoneControl.ConditionZoneControl;
		((Control)conditionZoneControl).Height = ((Control)conditionZoneControl).Height - ((Control)stiConditionItemZoneControl).Height;
	}

	private void ButtonRemoveCondition_Click(object sender, EventArgs e)
	{
		zcConditions.RemoveFocused();
	}

	private void TreeViewBoxColumn_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		((TreeView)e.TreeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		TreeNode val = new TreeNode(NoCondition);
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		((TreeView)e.TreeView).ImageList = StiDataBuilder.Images;
		e.Nodes.Add(val);
		dataBuilder.Build(e.Nodes, report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		if (e.Nodes.Count <= 1)
		{
			return;
		}
		StiTreeViewBox stiTreeViewBox = sender as StiTreeViewBox;
		if (((Control)stiTreeViewBox).Text == NoCondition)
		{
			((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
			return;
		}
		string[] strs = ((Control)stiTreeViewBox).Text.Split(new char[1] { '.' });
		if (!SelectNode(strs, 0, e.Nodes, (TreeView)(object)e.TreeView))
		{
			((TreeView)e.TreeView).SelectedNode = ((TreeView)e.TreeView).Nodes[0];
		}
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		ExpandTreeNode(e.Node);
	}

	private void TreeViewBoxColumn_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		if (!(e.Text != NoCondition))
		{
			return;
		}
		TreeNode selectedNode = ((TreeView)e.TreeView).SelectedNode;
		if (selectedNode != null)
		{
			if (!(selectedNode.Tag is StiDataColumn))
			{
				e.Text = NoCondition;
			}
			else
			{
				e.Text = StiDataBuilder.GetColumnPathFromNode(selectedNode);
			}
		}
		else
		{
			e.Text = NoCondition;
		}
	}

	private void ButtonMoveUp_Click(object sender, EventArgs e)
	{
		zcConditions.MoveUpFocused();
	}

	private void ButtonMoveDown_Click(object sender, EventArgs e)
	{
		zcConditions.MoveDownFocused();
	}

	private void ButtonNewHightlightCondition_Click(object sender, EventArgs e)
	{
		AddLevelHightlight();
	}

	private void ButtonNewDataBarCondition_Click(object sender, EventArgs e)
	{
		AddLevelDataBar();
	}

	private void ButtonNewColorScaleCondition_Click(object sender, EventArgs e)
	{
		AddLevelColorScale();
	}

	private void ButtonNewIconSetCondition_Click(object sender, EventArgs e)
	{
		AddLevelIconSet();
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiConditionsEditorForm));
		ilButtons = new ImageList(components);
		buttonCancel = new Button();
		buttonOk = new Button();
		stiGroupLine1 = new StiGroupLine();
		buttonAddCondition = new ButtonX();
		buttonNewHightlightCondition = new ButtonItem();
		buttonNewDataBarCondition = new ButtonItem();
		buttonNewColorScaleCondition = new ButtonItem();
		buttonNewIconSetCondition = new ButtonItem();
		zcConditions = new StiZoneContainerControl();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "AddCondition.png");
		ilButtons.Images.SetKeyName(1, "");
		ilButtons.Images.SetKeyName(2, "");
		ilButtons.Images.SetKeyName(3, "");
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(577, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 63;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += ThisCancel_Click;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(496, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 62;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += ThisOk_Click;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 441);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(664, 46);
		((Control)stiGroupLine1).TabIndex = 67;
		((Control)buttonAddCondition).AccessibleRole = (AccessibleRole)43;
		buttonAddCondition.AutoExpandOnClick = true;
		buttonAddCondition.ColorTable = eButtonColor.OrangeWithBackground;
		buttonAddCondition.Is20Button = false;
		((Control)buttonAddCondition).Location = new Point(12, 12);
		buttonAddCondition.LockClosePopup = false;
		((Control)buttonAddCondition).Name = "buttonAddCondition";
		((Control)buttonAddCondition).Size = new Size(126, 24);
		buttonAddCondition.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonAddCondition.SubItems.AddRange(new BaseItem[4] { buttonNewHightlightCondition, buttonNewDataBarCondition, buttonNewColorScaleCondition, buttonNewIconSetCondition });
		((Control)buttonAddCondition).TabIndex = 1;
		((Control)buttonAddCondition).Text = "Add Condition";
		buttonAddCondition.ThemeAware = true;
		buttonNewHightlightCondition.GlobalItem = false;
		buttonNewHightlightCondition.Name = "buttonNewHightlightCondition";
		buttonNewHightlightCondition.Text = "Hightlight Condition";
		buttonNewHightlightCondition.ThemeAware = true;
		buttonNewHightlightCondition.Click += ButtonNewHightlightCondition_Click;
		buttonNewDataBarCondition.GlobalItem = false;
		buttonNewDataBarCondition.Name = "buttonNewDataBarCondition";
		buttonNewDataBarCondition.Text = "Data Bar Condition";
		buttonNewDataBarCondition.ThemeAware = true;
		buttonNewDataBarCondition.Click += ButtonNewDataBarCondition_Click;
		buttonNewColorScaleCondition.GlobalItem = false;
		buttonNewColorScaleCondition.Name = "buttonNewColorScaleCondition";
		buttonNewColorScaleCondition.Text = "Color Scale Condition";
		buttonNewColorScaleCondition.ThemeAware = true;
		buttonNewColorScaleCondition.Click += ButtonNewColorScaleCondition_Click;
		buttonNewIconSetCondition.GlobalItem = false;
		buttonNewIconSetCondition.Name = "buttonNewIconSetCondition";
		buttonNewIconSetCondition.Text = "Icon Set Condition";
		buttonNewIconSetCondition.ThemeAware = true;
		buttonNewIconSetCondition.Click += ButtonNewIconSetCondition_Click;
		((Control)zcConditions).AllowDrop = true;
		((Control)zcConditions).Anchor = (AnchorStyles)15;
		((ScrollableControl)zcConditions).AutoScroll = true;
		((Control)zcConditions).BackColor = Color.White;
		zcConditions.EmptyImage = null;
		zcConditions.EmptyText = null;
		((Control)zcConditions).Location = new Point(12, 49);
		((Control)zcConditions).Margin = new Padding(2, 2, 2, 2);
		((Control)zcConditions).Name = "zcConditions";
		((Control)zcConditions).Padding = new Padding(2);
		zcConditions.ShowBorder = true;
		((Control)zcConditions).Size = new Size(642, 381);
		((Control)zcConditions).TabIndex = 61;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(664, 487);
		((Control)this).Controls.Add((Control)(object)buttonAddCondition);
		((Control)this).Controls.Add((Control)(object)zcConditions);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(680, 526);
		((Control)this).Name = "StiConditionsEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Conditions";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
