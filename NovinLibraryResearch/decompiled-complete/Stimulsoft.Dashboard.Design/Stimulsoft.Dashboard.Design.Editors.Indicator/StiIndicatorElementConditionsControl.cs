using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Components.FontIcons;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors.Indicator;

public class StiIndicatorElementConditionsControl : StiZoneControl
{
	private StiIndicatorElement indicatorElement;

	private StiIndicatorElementCondition condition;

	private IContainer components;

	private StiHitTestVisibleLabel lbFieldIs;

	public Stimulsoft.Controls.StiTextBox tbValue;

	public StiComboBox cbItem;

	public StiComboBox cbCondition;

	private StiHitTestVisibleLabel lbCondition;

	private StiHitTestVisibleLabel lbValue;

	public StiComboBox comboBoxIconAlign;

	public StiGroupLine groupLine;

	public StiColorButtonXF tbBackColor;

	public StiColorButtonXF tbTextColor;

	public StiToolButton tbFontStyleUnderline;

	public StiToolButton tbFontStyleItalic;

	public StiToolButton tbFontStyleBold;

	private StiOperationButton buttonOpen;

	private StiOperationButton buttonRemove;

	private Panel panelCustomIcon;

	private StiHitTestVisibleLabel lbFont;

	private StiHitTestVisibleLabel lbIcon;

	public StiComboBox cbFont;

	private ButtonX buttonPermissions;

	internal ButtonItem buttonItemAllowFontName;

	internal ButtonItem buttonItemAllowFontSize;

	internal ButtonItem buttonItemAllowFontBold;

	internal ButtonItem buttonItemAllowFontItalic;

	internal ButtonItem buttonItemAllowFontUnderline;

	internal ButtonItem buttonItemAllowFontStrikeout;

	internal ButtonItem buttonItemAllowTextColor;

	internal ButtonItem buttonItemAllowBackColor;

	public StiToolButton tbFontStyleStrikeout;

	private StiDropDownBox cbFontSize;

	private ColorPickerButton colorBoxIconColor;

	private Panel panelLine;

	private StiHitTestVisibleLabel lbTagetIcon;

	private ColorPickerButton colorBoxTargetIconColor;

	private StiFontIconSelectionButton fontIconButtonIconTaget;

	public StiComboBox comboBoxTargetIconAlign;

	private ButtonItem buttonItemAllowIcon;

	private ButtonItem buttonItemAllowTargetIcon;

	private StiFontIconSelectionButton fontIconButtonIcon;

	public StiIndicatorElementConditionsControl(StiIndicatorElement indicatorElement, StiIndicatorElementCondition condition, bool isNew = true)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			base.DrawBorder = false;
			Localize();
			this.indicatorElement = indicatorElement;
			this.condition = condition;
			Build(isNew);
			if (!VisualStyleInformation.IsEnabledByUser)
			{
				buttonPermissions.ThemeAware = false;
				buttonPermissions.Style = eDotNetBarStyle.OfficeXP;
			}
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)lbCondition).Text = StiLocalization.Get("PropertyMain", "Condition");
		((Control)lbFieldIs).Text = StiLocalization.Get("PropertyMain", "FieldIs");
		((Control)lbValue).Text = StiLocalization.Get("PropertyMain", "Value");
		((Control)lbFont).Text = StiLocalization.Get("PropertyMain", "Font");
		((Control)lbIcon).Text = StiLocalization.Get("PropertyMain", "Icon");
		((Control)lbTagetIcon).Text = StiLocalization.Get("PropertyMain", "TargetIcon");
		buttonItemAllowFontName.Text = StiLocalization.Get("PropertyMain", "FontName");
		buttonItemAllowFontSize.Text = StiLocalization.Get("PropertyMain", "FontSize");
		buttonItemAllowFontBold.Text = StiLocalization.Get("PropertyMain", "FontBold");
		buttonItemAllowFontItalic.Text = StiLocalization.Get("PropertyMain", "FontItalic");
		buttonItemAllowFontUnderline.Text = StiLocalization.Get("PropertyMain", "FontUnderline");
		buttonItemAllowFontStrikeout.Text = StiLocalization.Get("PropertyMain", "FontStrikeout");
		buttonItemAllowTextColor.Text = StiLocalization.Get("PropertyMain", "TextColor");
		buttonItemAllowBackColor.Text = StiLocalization.Get("PropertyMain", "BackColor");
		buttonItemAllowIcon.Text = StiLocalization.Get("PropertyMain", "Icon");
		buttonItemAllowTargetIcon.Text = StiLocalization.Get("PropertyMain", "TargetIcon");
	}

	private void ComboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ComboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ButtonItemAllowFontName_CheckedChanged(object sender, EventArgs e)
	{
		CheckAllows();
	}

	private void PanelImage_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			foreach (string item in array.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiResourceTypeHelper.IsImageType(ext))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelCustomIcon, e.Graphics, condition.CustomIcon, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void PanelImage_DoubleClick(object sender, EventArgs e)
	{
		ButtonOpen_Click(sender, e);
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		condition.CustomIcon = null;
		RefreshImageAndStatus();
	}

	private void ColorBoxIconColor_SelectedColorChanged(object sender, EventArgs e)
	{
		fontIconButtonIcon.IconColor = colorBoxIconColor.SelectedColor;
	}

	private void ComboBoxIconAlign_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			try
			{
				Control parent = ((Control)this).Parent;
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)((parent is Form) ? parent : null), ((FileDialog)val).FileName))
				{
					condition.CustomIcon = LoadImage(((FileDialog)val).FileName);
					RefreshImageAndStatus();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void colorPickerButtonTargetIcon_SelectedColorChanged(object sender, EventArgs e)
	{
		fontIconButtonIconTaget.IconColor = colorBoxTargetIconColor.SelectedColor;
	}

	private void DoScaling()
	{
		((ButtonBase)tbFontStyleBold).Image = StiReportDesignImages.Editor.FontStyleBold();
		((ButtonBase)tbFontStyleItalic).Image = StiReportDesignImages.Editor.FontStyleItalic();
		((ButtonBase)tbFontStyleUnderline).Image = StiReportDesignImages.Editor.FontStyleUnderline();
		((ButtonBase)tbTextColor).Image = StiReportDesignImages.Editor.FontColor();
		((ButtonBase)tbBackColor).Image = StiReportDesignImages.Editor.Fill();
		((ButtonBase)tbFontStyleStrikeout).Image = StiReportDesignImages.Editor.FontStyleStrikethrough();
		((ButtonBase)buttonOpen).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
		buttonPermissions.Image = StiReportDesignImages.Actions.SelectPermissions();
		ArrangeComponents();
	}

	private void ArrangeComponents()
	{
		StiHitTestVisibleLabel stiHitTestVisibleLabel = lbTagetIcon;
		StiComboBox stiComboBox = comboBoxTargetIconAlign;
		StiFontIconSelectionButton stiFontIconSelectionButton = fontIconButtonIconTaget;
		int num2 = (((Control)colorBoxTargetIconColor).Left = ((Control)fontIconButtonIcon).Right + StiScale.I12);
		int num4 = (((Control)stiFontIconSelectionButton).Left = num2);
		int left = (((Control)stiComboBox).Left = num4);
		((Control)stiHitTestVisibleLabel).Left = left;
		((Control)comboBoxIconAlign).Width = ((Control)fontIconButtonIcon).Width;
		((Control)comboBoxTargetIconAlign).Width = ((Control)fontIconButtonIconTaget).Width;
	}

	private void UpdateState()
	{
		((Control)buttonRemove).Enabled = condition.CustomIcon != null;
		StiFontIconSelectionButton stiFontIconSelectionButton = fontIconButtonIcon;
		bool visible = (((Control)colorBoxIconColor).Visible = condition.CustomIcon == null);
		((Control)stiFontIconSelectionButton).Visible = visible;
		((Control)panelCustomIcon).Visible = !((Control)fontIconButtonIcon).Visible;
		StiFontIconSelectionButton stiFontIconSelectionButton2 = fontIconButtonIcon;
		ColorPickerButton colorPickerButton = colorBoxIconColor;
		bool flag3 = (((Control)panelCustomIcon).Enabled = ((ListControl)comboBoxIconAlign).SelectedIndex > 0 && buttonItemAllowIcon.Checked);
		visible = (((Control)colorPickerButton).Enabled = flag3);
		((Control)stiFontIconSelectionButton2).Enabled = visible;
		StiFontIconSelectionButton stiFontIconSelectionButton3 = fontIconButtonIconTaget;
		visible = (((Control)colorBoxTargetIconColor).Enabled = ((ListControl)comboBoxTargetIconAlign).SelectedIndex > 0 && buttonItemAllowTargetIcon.Checked);
		((Control)stiFontIconSelectionButton3).Enabled = visible;
		string text = ((Control)cbCondition).Text;
		((ComboBox)cbCondition).Items.Clear();
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		if (((Control)cbItem).Text == StiLocalization.Get("PropertyEnum", "StiChartLabelsStyleValue") || ((Control)cbItem).Text == StiLocalization.Get("PropertyMain", "Target") || ((Control)cbItem).Text == StiLocalization.Get("PropertyMain", "Variation"))
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		}
		if (((Control)cbItem).Text == StiLocalization.Get("Chart", "Series"))
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		}
		foreach (string item in ((ComboBox)cbCondition).Items)
		{
			if (item == text)
			{
				((Control)cbCondition).Text = text;
				return;
			}
		}
		((ListControl)cbCondition).SelectedIndex = 0;
	}

	private void Build(bool isNew)
	{
		string text = StiFilterHelper.ConvertConditionToString(condition.Condition);
		FillFieldsIs(isNew);
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		((Control)cbCondition).Text = text;
		((Control)tbValue).Text = condition.Value;
		((ComboBox)comboBoxIconAlign).Items.Clear();
		((ComboBox)comboBoxIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiIconAlignmentNone"));
		((ComboBox)comboBoxIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiIconAlignmentLeft"));
		((ComboBox)comboBoxIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiIconAlignmentRight"));
		((ComboBox)comboBoxIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiIconAlignmentTop"));
		((ComboBox)comboBoxIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiIconAlignmentBottom"));
		if (condition.IconAlignment == StiIconAlignment.None)
		{
			((ComboBox)comboBoxIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiIconAlignmentNone");
		}
		else if (condition.IconAlignment == StiIconAlignment.Left)
		{
			((ComboBox)comboBoxIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiIconAlignmentLeft");
		}
		else if (condition.IconAlignment == StiIconAlignment.Right)
		{
			((ComboBox)comboBoxIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiIconAlignmentRight");
		}
		else if (condition.IconAlignment == StiIconAlignment.Top)
		{
			((ComboBox)comboBoxIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiIconAlignmentTop");
		}
		else if (condition.IconAlignment == StiIconAlignment.Bottom)
		{
			((ComboBox)comboBoxIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiIconAlignmentBottom");
		}
		((ComboBox)comboBoxTargetIconAlign).Items.Clear();
		((ComboBox)comboBoxTargetIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiIconAlignmentNone"));
		((ComboBox)comboBoxTargetIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiIconAlignmentLeft"));
		((ComboBox)comboBoxTargetIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiIconAlignmentRight"));
		if (condition.TargetIconAlignment == StiIconAlignment.None)
		{
			((ComboBox)comboBoxTargetIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiIconAlignmentNone");
		}
		else if (condition.TargetIconAlignment == StiIconAlignment.Left)
		{
			((ComboBox)comboBoxTargetIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiIconAlignmentLeft");
		}
		else
		{
			((ComboBox)comboBoxTargetIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiIconAlignmentRight");
		}
		List<FontFamily> fontFamilies = StiFontCollection.GetFontFamilies();
		((ComboBox)cbFont).Items.Clear();
		fontFamilies.FindAll((FontFamily font) => !string.IsNullOrEmpty(font.GetName(0))).ForEach(delegate(FontFamily font)
		{
			((ComboBox)cbFont).Items.Add((object)font.GetName(0));
		});
		StiComboBox stiComboBox = cbFont;
		Font font2 = condition.Font;
		((Control)stiComboBox).Text = ((font2 != null) ? font2.Name : null) ?? ((ComboBox)cbFont).Items[0].ToString();
		StiDropDownBox stiDropDownBox = cbFontSize;
		Font font3 = condition.Font;
		((Control)stiDropDownBox).Text = ((font3 != null) ? font3.SizeInPoints.ToString() : null) ?? "8";
		StiToolButton stiToolButton = tbFontStyleBold;
		Font font4 = condition.Font;
		stiToolButton.Pushed = font4 != null && font4.Bold;
		StiToolButton stiToolButton2 = tbFontStyleItalic;
		Font font5 = condition.Font;
		stiToolButton2.Pushed = font5 != null && font5.Italic;
		StiToolButton stiToolButton3 = tbFontStyleUnderline;
		Font font6 = condition.Font;
		stiToolButton3.Pushed = font6 != null && font6.Underline;
		StiToolButton stiToolButton4 = tbFontStyleStrikeout;
		Font font7 = condition.Font;
		stiToolButton4.Pushed = font7 != null && font7.Strikeout;
		tbTextColor.SelectedColor = condition.TextColor;
		tbBackColor.SelectedColor = condition.BackColor;
		fontIconButtonIcon.Icon = (isNew ? indicatorElement.Icon : condition.Icon);
		fontIconButtonIcon.IconColor = condition.IconColor;
		colorBoxIconColor.SelectedColor = condition.IconColor;
		fontIconButtonIconTaget.Icon = condition.TargetIcon;
		fontIconButtonIconTaget.IconColor = condition.TargetIconColor;
		colorBoxTargetIconColor.SelectedColor = condition.TargetIconColor;
		StiIndicatorConditionPermissions permissions = condition.Permissions;
		buttonItemAllowFontName.Checked = (permissions & StiIndicatorConditionPermissions.Font) > StiIndicatorConditionPermissions.None;
		buttonItemAllowFontSize.Checked = (permissions & StiIndicatorConditionPermissions.FontSize) > StiIndicatorConditionPermissions.None;
		buttonItemAllowFontBold.Checked = (permissions & StiIndicatorConditionPermissions.FontStyleBold) > StiIndicatorConditionPermissions.None;
		buttonItemAllowFontItalic.Checked = (permissions & StiIndicatorConditionPermissions.FontStyleItalic) > StiIndicatorConditionPermissions.None;
		buttonItemAllowFontUnderline.Checked = (permissions & StiIndicatorConditionPermissions.FontStyleUnderline) > StiIndicatorConditionPermissions.None;
		buttonItemAllowFontStrikeout.Checked = (permissions & StiIndicatorConditionPermissions.FontStyleStrikeout) > StiIndicatorConditionPermissions.None;
		buttonItemAllowTextColor.Checked = (permissions & StiIndicatorConditionPermissions.TextColor) > StiIndicatorConditionPermissions.None;
		buttonItemAllowBackColor.Checked = (permissions & StiIndicatorConditionPermissions.BackColor) > StiIndicatorConditionPermissions.None;
		buttonItemAllowIcon.Checked = (permissions & StiIndicatorConditionPermissions.Icon) > StiIndicatorConditionPermissions.None;
		buttonItemAllowTargetIcon.Checked = (permissions & StiIndicatorConditionPermissions.TargetIcon) > StiIndicatorConditionPermissions.None;
	}

	private void FillFieldsIs(bool isNew)
	{
		((ComboBox)cbItem).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiChartLabelsStyleValue"));
		((ComboBox)cbItem).Items.Add((object)StiLocalization.Get("Chart", "Series"));
		((ComboBox)cbItem).Items.Add((object)StiLocalization.Get("PropertyMain", "Target"));
		((ComboBox)cbItem).Items.Add((object)StiLocalization.Get("PropertyMain", "Variation"));
		if (isNew)
		{
			((ListControl)cbItem).SelectedIndex = 0;
			return;
		}
		switch (condition.Field)
		{
		case StiIndicatorFieldCondition.Value:
			((ListControl)cbItem).SelectedIndex = 0;
			break;
		case StiIndicatorFieldCondition.Series:
			((ListControl)cbItem).SelectedIndex = 1;
			break;
		case StiIndicatorFieldCondition.Target:
			((ListControl)cbItem).SelectedIndex = 2;
			break;
		case StiIndicatorFieldCondition.Variation:
			((ListControl)cbItem).SelectedIndex = 3;
			break;
		}
	}

	public StiIndicatorElementCondition GetIndicatorElementCondition(object sender = null)
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		StiIndicatorFieldCondition field = StiIndicatorFieldCondition.Value;
		switch (((ListControl)cbItem).SelectedIndex)
		{
		case 0:
			field = StiIndicatorFieldCondition.Value;
			break;
		case 1:
			field = StiIndicatorFieldCondition.Series;
			break;
		case 2:
			field = StiIndicatorFieldCondition.Target;
			break;
		case 3:
			field = StiIndicatorFieldCondition.Variation;
			break;
		}
		Stimulsoft.Report.Components.StiFilterCondition stiFilterCondition = StiFilterHelper.ConvertStringToCondition(((Control)cbCondition).Text);
		string text = ((Control)tbValue).Text;
		StiIndicatorConditionPermissions stiIndicatorConditionPermissions = StiIndicatorConditionPermissions.None;
		if (buttonItemAllowFontName.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.Font;
		}
		if (buttonItemAllowFontSize.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.FontSize;
		}
		if (buttonItemAllowFontBold.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.FontStyleBold;
		}
		if (buttonItemAllowFontItalic.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.FontStyleItalic;
		}
		if (buttonItemAllowFontUnderline.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.FontStyleUnderline;
		}
		if (buttonItemAllowFontStrikeout.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.FontStyleStrikeout;
		}
		if (buttonItemAllowTextColor.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.TextColor;
		}
		if (buttonItemAllowBackColor.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.BackColor;
		}
		if (buttonItemAllowIcon.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.Icon;
		}
		if (buttonItemAllowTargetIcon.Checked)
		{
			stiIndicatorConditionPermissions |= StiIndicatorConditionPermissions.TargetIcon;
		}
		FontStyle val = (FontStyle)0;
		if (buttonItemAllowFontBold.Checked && tbFontStyleBold.Pushed)
		{
			val = (FontStyle)(val | 1);
		}
		if (buttonItemAllowFontItalic.Checked && tbFontStyleItalic.Pushed)
		{
			val = (FontStyle)(val | 2);
		}
		if (buttonItemAllowFontUnderline.Checked && tbFontStyleUnderline.Pushed)
		{
			val = (FontStyle)(val | 4);
		}
		if (buttonItemAllowFontStrikeout.Checked && tbFontStyleStrikeout.Pushed)
		{
			val = (FontStyle)(val | 8);
		}
		string text2 = ((!buttonItemAllowFontName.Checked) ? "Arial" : (text2 = ((Control)cbFont).Text));
		float result = 8f;
		if (!float.TryParse((sender != null) ? ((Control)((sender is ListBox) ? sender : null)).Text : ((Control)cbFontSize).Text, out result))
		{
			result = 8f;
		}
		float num = (buttonItemAllowFontSize.Checked ? result : 8f);
		Font font = new Font(text2, num, val);
		Color textColor = (buttonItemAllowTextColor.Checked ? tbTextColor.SelectedColor : Color.Black);
		Color backColor = (buttonItemAllowBackColor.Checked ? tbBackColor.SelectedColor : Color.Transparent);
		StiIconAlignment stiIconAlignment = StiIconAlignment.None;
		switch (((ListControl)comboBoxIconAlign).SelectedIndex)
		{
		case 1:
			stiIconAlignment = StiIconAlignment.Left;
			break;
		case 2:
			stiIconAlignment = StiIconAlignment.Right;
			break;
		case 3:
			stiIconAlignment = StiIconAlignment.Top;
			break;
		case 4:
			stiIconAlignment = StiIconAlignment.Bottom;
			break;
		}
		StiIconAlignment targetIconAlignment = StiIconAlignment.None;
		switch (((ListControl)comboBoxTargetIconAlign).SelectedIndex)
		{
		case 1:
			targetIconAlignment = StiIconAlignment.Left;
			break;
		case 2:
			targetIconAlignment = StiIconAlignment.Right;
			break;
		}
		StiFontIcons icon = fontIconButtonIcon.Icon;
		byte[] customIcon = condition.CustomIcon;
		Color iconColor = colorBoxIconColor.SelectedColor;
		if (stiIconAlignment == StiIconAlignment.None)
		{
			icon = StiFontIcons.Rating4;
			customIcon = null;
			iconColor = Color.Maroon;
		}
		Color selectedColor = colorBoxTargetIconColor.SelectedColor;
		StiFontIcons icon2 = fontIconButtonIconTaget.Icon;
		return new StiIndicatorElementCondition(field, stiFilterCondition, text, icon, iconColor, icon2, selectedColor, customIcon, stiIconAlignment, targetIconAlignment, stiIndicatorConditionPermissions, font, textColor, backColor);
	}

	private void PanelImage_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array == null)
		{
			return;
		}
		foreach (string item in array.Where((string f) => f != null))
		{
			string ext = Path.GetExtension(item).ToLowerInvariant();
			if (StiResourceTypeHelper.IsImageType(ext))
			{
				Control parent = ((Control)this).Parent;
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)((parent is Form) ? parent : null), item))
				{
					condition.CustomIcon = LoadImage(item);
					RefreshImageAndStatus();
				}
				break;
			}
		}
	}

	private byte[] LoadImage(string file)
	{
		try
		{
			byte[] array = StiImageHelper.TryBytesFromFile(file);
			if (!StiSvgHelper.IsSvg(array))
			{
				using (MemoryStream memoryStream = new MemoryStream(array))
				{
					Image val = Image.FromStream((Stream)memoryStream);
					try
					{
						double num = Math.Min(1.0, 50.0 / (double)Math.Max(val.Width, val.Height));
						if (num < 1.0)
						{
							Bitmap val2 = StiImageUtils.ResizeImage(val, (int)Math.Round((double)val.Width * num), (int)Math.Round((double)val.Height * num));
							try
							{
								return StiImageConverter.ImageToBytes((Image)(object)val2);
							}
							finally
							{
								((IDisposable)val2)?.Dispose();
							}
						}
						return array;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			object document = StiSvgHelper.OpenSvg(array);
			SizeF svgSize = StiSvgHelper.GetSvgSize(document);
			double num2 = Math.Min(1.0, 50.0 / (double)Math.Max(svgSize.Width, svgSize.Height));
			int width = (int)Math.Round((double)svgSize.Width * num2);
			int height = (int)Math.Round((double)svgSize.Height * num2);
			Image val3 = StiSvgHelper.ConvertSvgToImage(array, width, height, stretch: true, aspectRatio: true);
			try
			{
				return StiImageConverter.ImageToBytes(val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		catch
		{
			return null;
		}
	}

	private void RefreshImageAndStatus()
	{
		UpdateState();
	}

	private void CheckAllows()
	{
		((Control)cbFont).Enabled = buttonItemAllowFontName.Checked;
		((Control)cbFontSize).Enabled = buttonItemAllowFontSize.Checked;
		((Control)tbTextColor).Enabled = buttonItemAllowTextColor.Checked;
		((Control)tbBackColor).Enabled = buttonItemAllowBackColor.Checked;
		((Control)tbFontStyleBold).Enabled = buttonItemAllowFontBold.Checked;
		((Control)tbFontStyleItalic).Enabled = buttonItemAllowFontItalic.Checked;
		((Control)tbFontStyleUnderline).Enabled = buttonItemAllowFontUnderline.Checked;
		((Control)tbFontStyleStrikeout).Enabled = buttonItemAllowFontStrikeout.Checked;
		((Control)comboBoxIconAlign).Enabled = buttonItemAllowIcon.Checked;
		((Control)comboBoxTargetIconAlign).Enabled = buttonItemAllowTargetIcon.Checked;
		StiFontIconSelectionButton stiFontIconSelectionButton = fontIconButtonIcon;
		ColorPickerButton colorPickerButton = colorBoxIconColor;
		bool flag2 = (((Control)panelCustomIcon).Enabled = ((ListControl)comboBoxIconAlign).SelectedIndex > 0 && buttonItemAllowIcon.Checked);
		bool enabled = (((Control)colorPickerButton).Enabled = flag2);
		((Control)stiFontIconSelectionButton).Enabled = enabled;
		StiFontIconSelectionButton stiFontIconSelectionButton2 = fontIconButtonIconTaget;
		enabled = (((Control)colorBoxTargetIconColor).Enabled = ((ListControl)comboBoxTargetIconAlign).SelectedIndex > 0 && buttonItemAllowTargetIcon.Checked);
		((Control)stiFontIconSelectionButton2).Enabled = enabled;
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
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Expected O, but got Unknown
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Expected O, but got Unknown
		//IL_0856: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Expected O, but got Unknown
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Expected O, but got Unknown
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Expected O, but got Unknown
		//IL_09ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acf: Expected O, but got Unknown
		//IL_0adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae6: Expected O, but got Unknown
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Expected O, but got Unknown
		//IL_0c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0def: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df9: Expected O, but got Unknown
		//IL_139e: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a8: Expected O, but got Unknown
		//IL_16a2: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiIndicatorElementConditionsControl));
		lbFieldIs = new StiHitTestVisibleLabel();
		tbValue = new Stimulsoft.Controls.StiTextBox();
		cbItem = new StiComboBox();
		cbCondition = new StiComboBox();
		lbCondition = new StiHitTestVisibleLabel();
		lbValue = new StiHitTestVisibleLabel();
		comboBoxIconAlign = new StiComboBox();
		groupLine = new StiGroupLine();
		tbBackColor = new StiColorButtonXF();
		tbTextColor = new StiColorButtonXF();
		tbFontStyleUnderline = new StiToolButton();
		tbFontStyleItalic = new StiToolButton();
		tbFontStyleBold = new StiToolButton();
		buttonOpen = new StiOperationButton();
		buttonRemove = new StiOperationButton();
		panelCustomIcon = new Panel();
		fontIconButtonIcon = new StiFontIconSelectionButton();
		colorBoxIconColor = new ColorPickerButton();
		lbFont = new StiHitTestVisibleLabel();
		lbIcon = new StiHitTestVisibleLabel();
		cbFont = new StiComboBox();
		buttonPermissions = new ButtonX();
		buttonItemAllowFontName = new ButtonItem();
		buttonItemAllowFontSize = new ButtonItem();
		buttonItemAllowFontBold = new ButtonItem();
		buttonItemAllowFontItalic = new ButtonItem();
		buttonItemAllowFontUnderline = new ButtonItem();
		buttonItemAllowFontStrikeout = new ButtonItem();
		buttonItemAllowTextColor = new ButtonItem();
		buttonItemAllowBackColor = new ButtonItem();
		buttonItemAllowIcon = new ButtonItem();
		buttonItemAllowTargetIcon = new ButtonItem();
		tbFontStyleStrikeout = new StiToolButton();
		cbFontSize = new StiDropDownBox();
		panelLine = new Panel();
		lbTagetIcon = new StiHitTestVisibleLabel();
		colorBoxTargetIconColor = new ColorPickerButton();
		fontIconButtonIconTaget = new StiFontIconSelectionButton();
		comboBoxTargetIconAlign = new StiComboBox();
		((Control)panelCustomIcon).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lbFieldIs).BackColor = Color.Transparent;
		((Control)lbFieldIs).ForeColor = Color.DimGray;
		((Control)lbFieldIs).Location = new Point(14, 12);
		((Control)lbFieldIs).Name = "lbFieldIs";
		((Control)lbFieldIs).Size = new Size(104, 13);
		((Control)lbFieldIs).TabIndex = 34;
		((Control)lbFieldIs).Text = "Field Is";
		((Control)tbValue).Anchor = (AnchorStyles)13;
		((Control)tbValue).BackColor = Color.White;
		((Control)tbValue).ForeColor = SystemColors.ControlText;
		((Control)tbValue).Location = new Point(289, 28);
		((Control)tbValue).Name = "tbValue";
		((Control)tbValue).Size = new Size(247, 21);
		((Control)tbValue).TabIndex = 2;
		((Control)cbItem).BackColor = SystemColors.Window;
		cbItem.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbItem).Location = new Point(14, 28);
		((Control)cbItem).Name = "cbItem";
		((Control)cbItem).Size = new Size(104, 21);
		((Control)cbItem).TabIndex = 0;
		((ComboBox)cbItem).SelectedIndexChanged += ComboBoxItem_SelectedIndexChanged;
		cbCondition.AutoDropDownWidth = false;
		((Control)cbCondition).BackColor = SystemColors.Window;
		cbCondition.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbCondition).DropDownWidth = 300;
		((Control)cbCondition).Location = new Point(130, 28);
		((Control)cbCondition).Name = "cbCondition";
		((Control)cbCondition).Size = new Size(147, 21);
		((Control)cbCondition).TabIndex = 1;
		((Control)lbCondition).BackColor = Color.Transparent;
		((Control)lbCondition).ForeColor = Color.DimGray;
		((Control)lbCondition).Location = new Point(130, 12);
		((Control)lbCondition).Name = "lbCondition";
		((Control)lbCondition).Size = new Size(147, 13);
		((Control)lbCondition).TabIndex = 35;
		((Control)lbCondition).Text = "Condition";
		((Control)lbValue).BackColor = Color.Transparent;
		((Control)lbValue).ForeColor = Color.DimGray;
		((Control)lbValue).Location = new Point(289, 12);
		((Control)lbValue).Name = "lbValue";
		((Control)lbValue).Size = new Size(63, 13);
		((Control)lbValue).TabIndex = 33;
		((Control)lbValue).Text = "Value";
		comboBoxIconAlign.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxIconAlign).FormattingEnabled = true;
		((Control)comboBoxIconAlign).Location = new Point(289, 88);
		((Control)comboBoxIconAlign).Name = "comboBoxIconAlign";
		((Control)comboBoxIconAlign).Size = new Size(90, 21);
		((Control)comboBoxIconAlign).TabIndex = 5;
		((ComboBox)comboBoxIconAlign).SelectedIndexChanged += ComboBoxIconAlign_SelectedIndexChanged;
		((Control)groupLine).BackColor = Color.FromArgb(250, 250, 250);
		((Control)groupLine).Dock = (DockStyle)2;
		groupLine.LinePosition = StiLinePosition.Bottom;
		groupLine.LineStyle = StiLineStyle.Dot;
		((Control)groupLine).Location = new Point(0, 185);
		((Control)groupLine).Name = "groupLine";
		((Control)groupLine).Size = new Size(555, 2);
		((Control)groupLine).TabIndex = 72;
		tbBackColor.Draw3DButton = false;
		((Control)tbBackColor).ForeColor = Color.Transparent;
		tbBackColor.HotDragDrop = false;
		((ButtonBase)tbBackColor).Image = (Image)componentResourceManager.GetObject("tbBackColor.Image");
		((Control)tbBackColor).Location = new Point(202, 118);
		((Control)tbBackColor).Name = "tbBackColor";
		tbBackColor.SelectedColor = Color.Transparent;
		((Control)tbBackColor).Size = new Size(32, 24);
		tbBackColor.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbBackColor).TabIndex = 12;
		((ButtonBase)tbBackColor).UseVisualStyleBackColor = false;
		tbTextColor.Draw3DButton = false;
		((Control)tbTextColor).ForeColor = Color.Red;
		tbTextColor.HotDragDrop = false;
		((ButtonBase)tbTextColor).Image = (Image)componentResourceManager.GetObject("tbTextColor.Image");
		((Control)tbTextColor).Location = new Point(166, 118);
		((Control)tbTextColor).Name = "tbTextColor";
		tbTextColor.SelectedColor = Color.Transparent;
		((Control)tbTextColor).Size = new Size(32, 24);
		tbTextColor.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbTextColor).TabIndex = 11;
		((ButtonBase)tbTextColor).UseVisualStyleBackColor = false;
		tbFontStyleUnderline.Draw3DButton = false;
		tbFontStyleUnderline.HotDragDrop = false;
		((ButtonBase)tbFontStyleUnderline).Image = (Image)componentResourceManager.GetObject("tbFontStyleUnderline.Image");
		((Control)tbFontStyleUnderline).Location = new Point(113, 118);
		((Control)tbFontStyleUnderline).Name = "tbFontStyleUnderline";
		((Control)tbFontStyleUnderline).Size = new Size(24, 24);
		tbFontStyleUnderline.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleUnderline).TabIndex = 9;
		((ButtonBase)tbFontStyleUnderline).UseVisualStyleBackColor = false;
		tbFontStyleItalic.Draw3DButton = false;
		tbFontStyleItalic.HotDragDrop = false;
		((ButtonBase)tbFontStyleItalic).Image = (Image)componentResourceManager.GetObject("tbFontStyleItalic.Image");
		((Control)tbFontStyleItalic).Location = new Point(89, 118);
		((Control)tbFontStyleItalic).Name = "tbFontStyleItalic";
		((Control)tbFontStyleItalic).Size = new Size(24, 24);
		tbFontStyleItalic.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleItalic).TabIndex = 8;
		((ButtonBase)tbFontStyleItalic).UseVisualStyleBackColor = false;
		tbFontStyleBold.Draw3DButton = false;
		tbFontStyleBold.HotDragDrop = false;
		((ButtonBase)tbFontStyleBold).Image = (Image)componentResourceManager.GetObject("tbFontStyleBold.Image");
		((Control)tbFontStyleBold).Location = new Point(65, 118);
		((Control)tbFontStyleBold).Name = "tbFontStyleBold";
		((Control)tbFontStyleBold).Size = new Size(24, 24);
		tbFontStyleBold.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleBold).TabIndex = 7;
		((ButtonBase)tbFontStyleBold).UseVisualStyleBackColor = false;
		((Control)buttonOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonOpen).Image = (Image)componentResourceManager.GetObject("buttonOpen.Image");
		((Control)buttonOpen).Location = new Point(60, 3);
		((Control)buttonOpen).Margin = new Padding(1);
		((Control)buttonOpen).Name = "buttonOpen";
		((Control)buttonOpen).Size = new Size(24, 23);
		((Control)buttonOpen).TabIndex = 160;
		((Control)buttonOpen).TabStop = false;
		((ButtonBase)buttonOpen).UseVisualStyleBackColor = true;
		((Control)buttonOpen).Click += ButtonOpen_Click;
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(60, 28);
		((Control)buttonRemove).Margin = new Padding(1);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(24, 23);
		((Control)buttonRemove).TabIndex = 170;
		((Control)buttonRemove).TabStop = false;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)panelCustomIcon).AllowDrop = true;
		((Control)panelCustomIcon).Controls.Add((Control)(object)buttonOpen);
		((Control)panelCustomIcon).Controls.Add((Control)(object)buttonRemove);
		((Control)panelCustomIcon).Location = new Point(289, 118);
		((Control)panelCustomIcon).Margin = new Padding(2);
		((Control)panelCustomIcon).Name = "panelCustomIcon";
		((Control)panelCustomIcon).Size = new Size(90, 54);
		((Control)panelCustomIcon).TabIndex = 15;
		((Control)panelCustomIcon).Visible = false;
		((Control)panelCustomIcon).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelCustomIcon).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelCustomIcon).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelCustomIcon).DoubleClick += PanelImage_DoubleClick;
		fontIconButtonIcon.Icon = StiFontIcons.Latin5;
		fontIconButtonIcon.IconColor = Color.FromArgb(68, 114, 196);
		((Control)fontIconButtonIcon).Location = new Point(289, 118);
		((Control)fontIconButtonIcon).Name = "fontIconButtonIcon";
		fontIconButtonIcon.ShowCustomIcon = true;
		((Control)fontIconButtonIcon).Size = new Size(90, 25);
		((Control)fontIconButtonIcon).TabIndex = 17;
		fontIconButtonIcon.CustomIconClick += ButtonOpen_Click;
		((Control)colorBoxIconColor).AccessibleRole = (AccessibleRole)43;
		colorBoxIconColor.AutoExpandOnClick = true;
		colorBoxIconColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxIconColor.EnableMarkup = false;
		colorBoxIconColor.Is20Button = false;
		((Control)colorBoxIconColor).Location = new Point(289, 148);
		((Control)colorBoxIconColor).Margin = new Padding(2);
		((Control)colorBoxIconColor).Name = "colorBoxIconColor";
		((Control)colorBoxIconColor).Size = new Size(90, 24);
		((Control)colorBoxIconColor).TabIndex = 19;
		colorBoxIconColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxIconColor.SelectedColorChanged += ColorBoxIconColor_SelectedColorChanged;
		((Control)lbFont).BackColor = Color.Transparent;
		((Control)lbFont).ForeColor = Color.DimGray;
		((Control)lbFont).Location = new Point(65, 72);
		((Control)lbFont).Name = "lbFont";
		((Control)lbFont).Size = new Size(104, 14);
		((Control)lbFont).TabIndex = 34;
		((Control)lbFont).Text = "Font";
		((Control)lbIcon).BackColor = Color.Transparent;
		((Control)lbIcon).ForeColor = Color.DimGray;
		((Control)lbIcon).Location = new Point(290, 72);
		((Control)lbIcon).Name = "lbIcon";
		((Control)lbIcon).Size = new Size(104, 13);
		((Control)lbIcon).TabIndex = 34;
		((Control)lbIcon).Text = "Icon";
		cbFont.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbFont).FormattingEnabled = true;
		((Control)cbFont).Location = new Point(65, 88);
		((ComboBox)cbFont).MaxDropDownItems = 30;
		((Control)cbFont).Name = "cbFont";
		((Control)cbFont).Size = new Size(212, 21);
		((Control)cbFont).TabIndex = 4;
		((Control)buttonPermissions).AccessibleRole = (AccessibleRole)43;
		buttonPermissions.AutoExpandOnClick = true;
		buttonPermissions.ColorTable = eButtonColor.OrangeWithBackground;
		buttonPermissions.Image = (Image)componentResourceManager.GetObject("buttonPermissions.Image");
		buttonPermissions.Is20Button = false;
		((Control)buttonPermissions).Location = new Point(13, 87);
		((Control)buttonPermissions).Name = "buttonPermissions";
		((Control)buttonPermissions).Size = new Size(41, 23);
		buttonPermissions.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonPermissions.SubItems.AddRange(new BaseItem[10] { buttonItemAllowFontName, buttonItemAllowFontSize, buttonItemAllowFontBold, buttonItemAllowFontItalic, buttonItemAllowFontUnderline, buttonItemAllowFontStrikeout, buttonItemAllowTextColor, buttonItemAllowBackColor, buttonItemAllowIcon, buttonItemAllowTargetIcon });
		((Control)buttonPermissions).TabIndex = 3;
		buttonPermissions.ThemeAware = true;
		buttonItemAllowFontName.AutoCheckOnClick = true;
		buttonItemAllowFontName.AutoCollapseOnClick = false;
		buttonItemAllowFontName.Checked = true;
		buttonItemAllowFontName.GlobalItem = false;
		buttonItemAllowFontName.Name = "buttonItemAllowFontName";
		buttonItemAllowFontName.Text = "Font Name";
		buttonItemAllowFontName.ThemeAware = true;
		buttonItemAllowFontName.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowFontSize.AutoCheckOnClick = true;
		buttonItemAllowFontSize.AutoCollapseOnClick = false;
		buttonItemAllowFontSize.Checked = true;
		buttonItemAllowFontSize.GlobalItem = false;
		buttonItemAllowFontSize.Name = "buttonItemAllowFontSize";
		buttonItemAllowFontSize.Text = "Font Size";
		buttonItemAllowFontSize.ThemeAware = true;
		buttonItemAllowFontSize.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowFontBold.AutoCheckOnClick = true;
		buttonItemAllowFontBold.AutoCollapseOnClick = false;
		buttonItemAllowFontBold.BeginGroup = true;
		buttonItemAllowFontBold.Checked = true;
		buttonItemAllowFontBold.GlobalItem = false;
		buttonItemAllowFontBold.Name = "buttonItemAllowFontBold";
		buttonItemAllowFontBold.Text = "Font Bold";
		buttonItemAllowFontBold.ThemeAware = true;
		buttonItemAllowFontBold.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowFontItalic.AutoCheckOnClick = true;
		buttonItemAllowFontItalic.AutoCollapseOnClick = false;
		buttonItemAllowFontItalic.Checked = true;
		buttonItemAllowFontItalic.GlobalItem = false;
		buttonItemAllowFontItalic.Name = "buttonItemAllowFontItalic";
		buttonItemAllowFontItalic.Text = "Font Italic";
		buttonItemAllowFontItalic.ThemeAware = true;
		buttonItemAllowFontItalic.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowFontUnderline.AutoCheckOnClick = true;
		buttonItemAllowFontUnderline.AutoCollapseOnClick = false;
		buttonItemAllowFontUnderline.Checked = true;
		buttonItemAllowFontUnderline.GlobalItem = false;
		buttonItemAllowFontUnderline.Name = "buttonItemAllowFontUnderline";
		buttonItemAllowFontUnderline.Text = "Font Underline";
		buttonItemAllowFontUnderline.ThemeAware = true;
		buttonItemAllowFontUnderline.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowFontStrikeout.AutoCheckOnClick = true;
		buttonItemAllowFontStrikeout.AutoCollapseOnClick = false;
		buttonItemAllowFontStrikeout.Checked = true;
		buttonItemAllowFontStrikeout.GlobalItem = false;
		buttonItemAllowFontStrikeout.Name = "buttonItemAllowFontStrikeout";
		buttonItemAllowFontStrikeout.Text = "Font Strikeout";
		buttonItemAllowFontStrikeout.ThemeAware = true;
		buttonItemAllowFontStrikeout.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowTextColor.AutoCheckOnClick = true;
		buttonItemAllowTextColor.AutoCollapseOnClick = false;
		buttonItemAllowTextColor.BeginGroup = true;
		buttonItemAllowTextColor.Checked = true;
		buttonItemAllowTextColor.GlobalItem = false;
		buttonItemAllowTextColor.Name = "buttonItemAllowTextColor";
		buttonItemAllowTextColor.Text = "Text Color";
		buttonItemAllowTextColor.ThemeAware = true;
		buttonItemAllowTextColor.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowBackColor.AutoCheckOnClick = true;
		buttonItemAllowBackColor.AutoCollapseOnClick = false;
		buttonItemAllowBackColor.Checked = true;
		buttonItemAllowBackColor.GlobalItem = false;
		buttonItemAllowBackColor.Name = "buttonItemAllowBackColor";
		buttonItemAllowBackColor.Text = "Back Color";
		buttonItemAllowBackColor.ThemeAware = true;
		buttonItemAllowBackColor.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowIcon.AutoCheckOnClick = true;
		buttonItemAllowIcon.AutoCollapseOnClick = false;
		buttonItemAllowIcon.BeginGroup = true;
		buttonItemAllowIcon.Checked = true;
		buttonItemAllowIcon.GlobalItem = false;
		buttonItemAllowIcon.Name = "buttonItemAllowIcon";
		buttonItemAllowIcon.Text = "Icon";
		buttonItemAllowIcon.ThemeAware = true;
		buttonItemAllowIcon.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowTargetIcon.AutoCheckOnClick = true;
		buttonItemAllowTargetIcon.AutoCollapseOnClick = false;
		buttonItemAllowTargetIcon.Checked = true;
		buttonItemAllowTargetIcon.GlobalItem = false;
		buttonItemAllowTargetIcon.Name = "buttonItemAllowTargetIcon";
		buttonItemAllowTargetIcon.Text = "Target Icon";
		buttonItemAllowTargetIcon.ThemeAware = true;
		buttonItemAllowTargetIcon.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		tbFontStyleStrikeout.Draw3DButton = false;
		tbFontStyleStrikeout.HotDragDrop = false;
		((ButtonBase)tbFontStyleStrikeout).Image = (Image)componentResourceManager.GetObject("tbFontStyleStrikeout.Image");
		((Control)tbFontStyleStrikeout).Location = new Point(137, 118);
		((Control)tbFontStyleStrikeout).Name = "tbFontStyleStrikeout";
		((Control)tbFontStyleStrikeout).Size = new Size(24, 24);
		tbFontStyleStrikeout.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleStrikeout).TabIndex = 10;
		((ButtonBase)tbFontStyleStrikeout).UseVisualStyleBackColor = false;
		((Control)cbFontSize).BackColor = SystemColors.Window;
		cbFontSize.DrawMode = (DrawMode)2;
		((Control)cbFontSize).ForeColor = SystemColors.ControlText;
		cbFontSize.IsEditable = true;
		cbFontSize.Items.AddRange(new object[19]
		{
			"5", "6", "7", "8", "9", "10", "11", "12", "14", "16",
			"18", "20", "22", "24", "26", "28", "36", "48", "76"
		});
		((Control)cbFontSize).Location = new Point(238, 120);
		((Control)cbFontSize).Name = "cbFontSize";
		cbFontSize.SelectedIndex = -1;
		cbFontSize.SelectedItem = null;
		cbFontSize.SelectedValue = null;
		((Control)cbFontSize).Size = new Size(39, 21);
		((Control)cbFontSize).TabIndex = 13;
		((Control)panelLine).Anchor = (AnchorStyles)13;
		((Control)panelLine).BackColor = Color.Gainsboro;
		((Control)panelLine).Location = new Point(14, 61);
		((Control)panelLine).Name = "panelLine";
		((Control)panelLine).Size = new Size(529, 1);
		((Control)panelLine).TabIndex = 89;
		((Control)lbTagetIcon).BackColor = Color.Transparent;
		((Control)lbTagetIcon).ForeColor = Color.DimGray;
		((Control)lbTagetIcon).Location = new Point(391, 72);
		((Control)lbTagetIcon).Name = "lbTagetIcon";
		((Control)lbTagetIcon).Size = new Size(104, 13);
		((Control)lbTagetIcon).TabIndex = 191;
		((Control)lbTagetIcon).Text = "Target Icon";
		((Control)colorBoxTargetIconColor).AccessibleRole = (AccessibleRole)43;
		colorBoxTargetIconColor.AutoExpandOnClick = true;
		colorBoxTargetIconColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxTargetIconColor.EnableMarkup = false;
		colorBoxTargetIconColor.Is20Button = false;
		((Control)colorBoxTargetIconColor).Location = new Point(394, 148);
		((Control)colorBoxTargetIconColor).Margin = new Padding(2);
		((Control)colorBoxTargetIconColor).Name = "colorBoxTargetIconColor";
		((Control)colorBoxTargetIconColor).Size = new Size(90, 24);
		((Control)colorBoxTargetIconColor).TabIndex = 20;
		colorBoxTargetIconColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxTargetIconColor.SelectedColorChanged += colorPickerButtonTargetIcon_SelectedColorChanged;
		fontIconButtonIconTaget.Icon = StiFontIcons.Latin5;
		fontIconButtonIconTaget.IconColor = Color.FromArgb(68, 114, 196);
		((Control)fontIconButtonIconTaget).Location = new Point(394, 118);
		((Control)fontIconButtonIconTaget).Name = "fontIconButtonIconTaget";
		fontIconButtonIconTaget.ShowCustomIcon = false;
		((Control)fontIconButtonIconTaget).Size = new Size(90, 25);
		((Control)fontIconButtonIconTaget).TabIndex = 18;
		comboBoxTargetIconAlign.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxTargetIconAlign).FormattingEnabled = true;
		((Control)comboBoxTargetIconAlign).Location = new Point(394, 88);
		((Control)comboBoxTargetIconAlign).Name = "comboBoxTargetIconAlign";
		((Control)comboBoxTargetIconAlign).Size = new Size(90, 21);
		((Control)comboBoxTargetIconAlign).TabIndex = 6;
		((ComboBox)comboBoxTargetIconAlign).SelectedIndexChanged += ComboBoxItem_SelectedIndexChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panelCustomIcon);
		((Control)this).Controls.Add((Control)(object)fontIconButtonIcon);
		((Control)this).Controls.Add((Control)(object)comboBoxTargetIconAlign);
		((Control)this).Controls.Add((Control)(object)fontIconButtonIconTaget);
		((Control)this).Controls.Add((Control)(object)colorBoxTargetIconColor);
		((Control)this).Controls.Add((Control)(object)lbTagetIcon);
		((Control)this).Controls.Add((Control)(object)panelLine);
		((Control)this).Controls.Add((Control)(object)cbFontSize);
		((Control)this).Controls.Add((Control)(object)colorBoxIconColor);
		((Control)this).Controls.Add((Control)(object)tbFontStyleStrikeout);
		((Control)this).Controls.Add((Control)(object)buttonPermissions);
		((Control)this).Controls.Add((Control)(object)cbFont);
		((Control)this).Controls.Add((Control)(object)tbBackColor);
		((Control)this).Controls.Add((Control)(object)tbTextColor);
		((Control)this).Controls.Add((Control)(object)tbFontStyleUnderline);
		((Control)this).Controls.Add((Control)(object)tbFontStyleItalic);
		((Control)this).Controls.Add((Control)(object)tbFontStyleBold);
		((Control)this).Controls.Add((Control)(object)groupLine);
		((Control)this).Controls.Add((Control)(object)comboBoxIconAlign);
		((Control)this).Controls.Add((Control)(object)lbIcon);
		((Control)this).Controls.Add((Control)(object)lbFont);
		((Control)this).Controls.Add((Control)(object)lbFieldIs);
		((Control)this).Controls.Add((Control)(object)tbValue);
		((Control)this).Controls.Add((Control)(object)cbItem);
		((Control)this).Controls.Add((Control)(object)cbCondition);
		((Control)this).Controls.Add((Control)(object)lbCondition);
		((Control)this).Controls.Add((Control)(object)lbValue);
		((Control)this).Name = "StiIndicatorElementConditionsControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(555, 187);
		((Control)panelCustomIcon).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
