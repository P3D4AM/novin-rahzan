using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors.PivotTable;

public class StiPivotTableElementConditionsControl : StiZoneControl
{
	private StiPivotTableElement pivotTableElement;

	private StiPivotTableElementCondition condition;

	private StiPopup iconsPopup;

	private StiHintTextBox textBoxSearch;

	private Panel panelIcons;

	private IContainer components;

	public DateTimePicker dtpValue;

	private StiHitTestVisibleLabel lbFieldIs;

	public Stimulsoft.Controls.StiTextBox tbValue;

	public StiComboBox cbDataType;

	public StiComboBox cbItem;

	public StiComboBox cbCondition;

	private StiHitTestVisibleLabel lbCondition;

	private StiHitTestVisibleLabel lbDataType;

	private StiHitTestVisibleLabel lbValue;

	public StiComboBox comboBoxIconAlign;

	public StiGroupLine groupLine;

	private StiOperationButton buttonExpression;

	public StiColorButtonXF tbBackColor;

	public StiColorButtonXF tbTextColor;

	public StiToolButton tbFontStyleUnderline;

	public StiToolButton tbFontStyleItalic;

	public StiToolButton tbFontStyleBold;

	private Label lbAaBbCc;

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

	private Panel panelBoxIcon;

	private StiOperationButton buttonCustomIcon;

	private StiOperationButton buttonIcon;

	private ColorPickerButton colorBoxIconColor;

	private Panel panelLine;

	private Button buttonRemove;

	private Button buttonOpen;

	private Panel panelCustomIcon;

	public StiPivotTableElementConditionsControl(StiPivotTableElement pivotTableElement, StiPivotTableElementCondition condition, bool isNew)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			base.DrawBorder = false;
			Localize();
			dtpValue.Value = DateTime.Now;
			this.pivotTableElement = pivotTableElement;
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
		((Control)lbDataType).Text = StiLocalization.Get("PropertyMain", "DataType");
		((Control)lbValue).Text = StiLocalization.Get("PropertyMain", "Value");
		((Control)lbFont).Text = StiLocalization.Get("PropertyMain", "Font");
		((Control)lbIcon).Text = StiLocalization.Get("PropertyMain", "Icon");
		buttonItemAllowFontName.Text = StiLocalization.Get("PropertyMain", "FontName");
		buttonItemAllowFontSize.Text = StiLocalization.Get("PropertyMain", "FontSize");
		buttonItemAllowFontBold.Text = StiLocalization.Get("PropertyMain", "FontBold");
		buttonItemAllowFontItalic.Text = StiLocalization.Get("PropertyMain", "FontItalic");
		buttonItemAllowFontUnderline.Text = StiLocalization.Get("PropertyMain", "FontUnderline");
		buttonItemAllowFontStrikeout.Text = StiLocalization.Get("PropertyMain", "FontStrikeout");
		buttonItemAllowTextColor.Text = StiLocalization.Get("PropertyMain", "TextColor");
		buttonItemAllowBackColor.Text = StiLocalization.Get("PropertyMain", "BackColor");
	}

	private void ComboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ComboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void CbFontSize_SelectedValueChanged(object sender, EventArgs e)
	{
		UpdateSample(sender);
	}

	private void CbFont_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void TbFontStyleBold_Click(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void TbFontStyleItalic_Click(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void TbFontStyleUnderline_Click(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void TbFontStyleStrikeout_Click(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void TbTextColor_StyleChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void TbTextColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void TbBackColor_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateSample();
	}

	private void ButtonItemAllowFontName_CheckedChanged(object sender, EventArgs e)
	{
		CheckAllows();
		UpdateSample();
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

	private void ButtonIcon_Click(object sender, EventArgs e)
	{
		CheckColorIcons();
		((ToolStripDropDown)iconsPopup).Show(new Point(((Control)panelBoxIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonIcon).Height + StiScale.I2));
		((Control)textBoxSearch).Focus();
	}

	private void ButtonCustomIcon_CustomIconClick(object sender, EventArgs e)
	{
		((Control)iconsPopup).Hide();
		ButtonOpen_Click(sender, e);
	}

	private void StiPivotTableElementConditionsControl_Load(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		iconsPopup = new StiPopup((Control)(object)GetIconPanel());
		((Control)panelBoxIcon).Paint += new PaintEventHandler(PanelBoxIcon_Paint);
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

	private void TextBoxSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildIconPanel(filterName);
	}

	private void FontIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		condition.Icon = ((StiFontIconSelectionControl)sender).SelectedIcon.GetValueOrDefault();
		RefreshSampleIcon();
		((Control)iconsPopup).Hide();
	}

	private void DtpValue_Layout(object sender, LayoutEventArgs e)
	{
		ArrangeComponents();
	}

	private void PanelBoxIcon_Paint(object sender, PaintEventArgs e)
	{
		RefreshSampleIcon();
	}

	private void ColorBoxIconColor_SelectedColorChanged(object sender, EventArgs e)
	{
		RefreshSampleIcon();
	}

	private void ComboBoxIconAlign_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void DtpValue_Resize(object sender, EventArgs e)
	{
		ArrangeComponents();
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

	private void DoScaling()
	{
		((ButtonBase)buttonExpression).Image = StiReportDesignImages.Actions.EditSmall();
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
		((Control)cbFont).Width = ((Control)tbBackColor).Right - ((Control)cbFont).Left;
		((Control)cbFontSize).Width = ((Control)cbCondition).Right - ((Control)cbFontSize).Left;
		((Control)buttonPermissions).Left = ((Control)cbFontSize).Left - 1;
		((Control)buttonPermissions).Width = ((Control)cbFontSize).Right - ((Control)buttonPermissions).Left + 1;
		((Control)lbAaBbCc).Width = ((Control)cbItem).Width;
		((Control)panelCustomIcon).Width = ((Control)dtpValue).Right - ((Control)panelCustomIcon).Left;
		((Control)colorBoxIconColor).Width = ((Control)dtpValue).Right - ((Control)colorBoxIconColor).Left;
		((Control)panelBoxIcon).Width = ((Control)dtpValue).Right - ((Control)panelBoxIcon).Left;
		((Control)panelLine).Width = ((Control)dtpValue).Right - ((Control)panelLine).Left;
	}

	private void UpdateState()
	{
		((Control)dtpValue).Visible = ((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime");
		((Control)tbValue).Visible = ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime");
		((Control)buttonExpression).Visible = ((Control)cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemExpression");
		((Control)cbCondition).Enabled = !((Control)buttonExpression).Visible;
		((Control)lbCondition).Enabled = !((Control)buttonExpression).Visible;
		((Control)cbDataType).Enabled = !((Control)buttonExpression).Visible;
		((Control)lbDataType).Enabled = !((Control)buttonExpression).Visible;
		((Control)buttonRemove).Enabled = condition.CustomIcon != null;
		Panel obj = panelBoxIcon;
		bool visible = (((Control)colorBoxIconColor).Visible = condition.CustomIcon == null);
		((Control)obj).Visible = visible;
		((Control)panelCustomIcon).Visible = !((Control)panelBoxIcon).Visible;
		Panel obj2 = panelBoxIcon;
		ColorPickerButton colorPickerButton = colorBoxIconColor;
		bool flag3 = (((Control)panelCustomIcon).Enabled = ((ListControl)comboBoxIconAlign).SelectedIndex > 0);
		visible = (((Control)colorPickerButton).Enabled = flag3);
		((Control)obj2).Enabled = visible;
		string text = ((Control)cbCondition).Text;
		((ComboBox)cbCondition).Items.Clear();
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		if (((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString") && ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"))
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		}
		if (((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"))
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
		string text2 = StiFilterHelper.ConvertDataTypeToString(condition.DataType);
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
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeNumeric"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"));
		((Control)cbDataType).Text = text2;
		if (text2 == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"))
		{
			DateTime.TryParse(condition.Value, new CultureInfo("en-US", useUserOverride: false), DateTimeStyles.None, out var result);
			dtpValue.Value = result;
		}
		else
		{
			((Control)tbValue).Text = condition.Value;
		}
		((ComboBox)comboBoxIconAlign).Items.Clear();
		((ComboBox)comboBoxIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiBorderSidesNone"));
		((ComboBox)comboBoxIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiBorderSidesRight"));
		((ComboBox)comboBoxIconAlign).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiBorderSidesLeft"));
		if (condition.IconAlignment == StiIconAlignment.None)
		{
			((ComboBox)comboBoxIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiBorderSidesNone");
		}
		else if (condition.IconAlignment == StiIconAlignment.Left)
		{
			((ComboBox)comboBoxIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiBorderSidesLeft");
		}
		else if (condition.IconAlignment == StiIconAlignment.Right)
		{
			((ComboBox)comboBoxIconAlign).SelectedItem = StiLocalization.Get("PropertyEnum", "StiBorderSidesRight");
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
		colorBoxIconColor.SelectedColor = condition.IconColor;
		StiConditionPermissions permissions = condition.Permissions;
		buttonItemAllowFontName.Checked = (permissions & StiConditionPermissions.Font) > StiConditionPermissions.None;
		buttonItemAllowFontSize.Checked = (permissions & StiConditionPermissions.FontSize) > StiConditionPermissions.None;
		buttonItemAllowFontBold.Checked = (permissions & StiConditionPermissions.FontStyleBold) > StiConditionPermissions.None;
		buttonItemAllowFontItalic.Checked = (permissions & StiConditionPermissions.FontStyleItalic) > StiConditionPermissions.None;
		buttonItemAllowFontUnderline.Checked = (permissions & StiConditionPermissions.FontStyleUnderline) > StiConditionPermissions.None;
		buttonItemAllowFontStrikeout.Checked = (permissions & StiConditionPermissions.FontStyleStrikeout) > StiConditionPermissions.None;
		buttonItemAllowTextColor.Checked = (permissions & StiConditionPermissions.TextColor) > StiConditionPermissions.None;
		buttonItemAllowBackColor.Checked = (permissions & StiConditionPermissions.BackColor) > StiConditionPermissions.None;
	}

	private void FillFieldsIs(bool isNew)
	{
		bool flag = true;
		StiPivotMeterComboBoxItem selectedItem = null;
		List<StiMeter> list = new List<StiMeter>();
		list.AddRange(pivotTableElement.Columns);
		list.AddRange(pivotTableElement.Rows);
		list.AddRange(pivotTableElement.Summaries);
		foreach (StiMeter item in list)
		{
			string label = StiLabelHelper.GetLabel(item);
			StiPivotMeterComboBoxItem stiPivotMeterComboBoxItem = new StiPivotMeterComboBoxItem(label, item.Key);
			if (!((ComboBox)cbItem).Items.Contains((object)stiPivotMeterComboBoxItem))
			{
				((ComboBox)cbItem).Items.Add((object)stiPivotMeterComboBoxItem);
			}
			if (item.Key == condition.KeyValueMeter || condition.KeyValueMeter == label)
			{
				flag = false;
				selectedItem = stiPivotMeterComboBoxItem;
			}
		}
		if (flag && ((ComboBox)cbItem).Items.Count > 0 && isNew)
		{
			((ListControl)cbItem).SelectedIndex = 0;
		}
		else
		{
			((ComboBox)cbItem).SelectedItem = selectedItem;
		}
	}

	public StiPivotTableElementCondition GetPivotTableElementCondition(object sender = null)
	{
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		string keyValueMeter = ((((ComboBox)cbItem).SelectedItem != null) ? (((ComboBox)cbItem).SelectedItem as StiPivotMeterComboBoxItem).Key : condition.KeyValueMeter);
		Stimulsoft.Report.Components.StiFilterCondition stiFilterCondition = StiFilterHelper.ConvertStringToCondition(((Control)cbCondition).Text);
		Stimulsoft.Report.Components.StiFilterDataType stiFilterDataType = StiFilterHelper.ConvertStringToDataType(((Control)cbDataType).Text);
		string value = null;
		if (stiFilterDataType == Stimulsoft.Report.Components.StiFilterDataType.DateTime)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				value = dtpValue.Value.ToShortDateString();
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		else
		{
			value = ((Control)tbValue).Text;
		}
		StiConditionPermissions stiConditionPermissions = StiConditionPermissions.None;
		if (buttonItemAllowFontName.Checked)
		{
			stiConditionPermissions |= StiConditionPermissions.Font;
		}
		if (buttonItemAllowFontSize.Checked)
		{
			stiConditionPermissions |= StiConditionPermissions.FontSize;
		}
		if (buttonItemAllowFontBold.Checked)
		{
			stiConditionPermissions |= StiConditionPermissions.FontStyleBold;
		}
		if (buttonItemAllowFontItalic.Checked)
		{
			stiConditionPermissions |= StiConditionPermissions.FontStyleItalic;
		}
		if (buttonItemAllowFontUnderline.Checked)
		{
			stiConditionPermissions |= StiConditionPermissions.FontStyleUnderline;
		}
		if (buttonItemAllowFontStrikeout.Checked)
		{
			stiConditionPermissions |= StiConditionPermissions.FontStyleStrikeout;
		}
		if (buttonItemAllowTextColor.Checked)
		{
			stiConditionPermissions |= StiConditionPermissions.TextColor;
		}
		if (buttonItemAllowBackColor.Checked)
		{
			stiConditionPermissions |= StiConditionPermissions.BackColor;
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
		string text = ((!buttonItemAllowFontName.Checked) ? "Arial" : (text = ((Control)cbFont).Text));
		float result = 8f;
		if (!float.TryParse((sender != null) ? ((Control)((sender is ListBox) ? sender : null)).Text : ((Control)cbFontSize).Text, out result))
		{
			result = 8f;
		}
		float num = (buttonItemAllowFontSize.Checked ? result : 8f);
		Font font = new Font(text, num, val);
		Color textColor = (buttonItemAllowTextColor.Checked ? tbTextColor.SelectedColor : Color.Black);
		Color backColor = (buttonItemAllowBackColor.Checked ? tbBackColor.SelectedColor : Color.Transparent);
		StiIconAlignment stiIconAlignment = StiIconAlignment.None;
		if (((ListControl)comboBoxIconAlign).SelectedIndex == 1)
		{
			stiIconAlignment = StiIconAlignment.Right;
		}
		else if (((ListControl)comboBoxIconAlign).SelectedIndex == 2)
		{
			stiIconAlignment = StiIconAlignment.Left;
		}
		StiFontIcons icon = condition.Icon;
		byte[] customIcon = condition.CustomIcon;
		Color iconColor = colorBoxIconColor.SelectedColor;
		if (stiIconAlignment == StiIconAlignment.None)
		{
			icon = StiFontIcons.Adjust;
			customIcon = null;
			iconColor = Color.Maroon;
		}
		return new StiPivotTableElementCondition(keyValueMeter, stiFilterDataType, stiFilterCondition, value, font, textColor, backColor, stiConditionPermissions, icon, stiIconAlignment, customIcon, iconColor);
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

	private Panel GetIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxSearch = stiHintTextBox;
		((Control)textBoxSearch).TextChanged += TextBoxSearch_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxSearch);
		panelIcons = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelIcons);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildIconPanel();
		return val;
	}

	private void RefreshSampleIcon()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		Graphics val = ((Control)panelBoxIcon).CreateGraphics();
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font val2 = new Font(fontFamilyIcons, 13f);
		try
		{
			SolidBrush val3 = new SolidBrush(colorBoxIconColor.SelectedColor);
			try
			{
				val.FillRectangle(Brushes.White, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width - StiScale.I4, ((Control)panelBoxIcon).Height - StiScale.I6));
				val.DrawString(StiFontIconsHelper.GetContent(condition.Icon), val2, (Brush)(object)val3, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width, ((Control)panelBoxIcon).Height));
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		val.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)panelBoxIcon).Width - 1, ((Control)panelBoxIcon).Height - 1);
	}

	private void BuildIconPanel(string filterName = null)
	{
		((Control)panelIcons).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(condition.Icon, condition.IconColor);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		StiFontIconSelectionControl stiFontIconSelectionControl2 = stiFontIconSelectionControl;
		stiFontIconSelectionControl2.ValueChanged += FontIconSelectionControl_ValueChanged;
		((Control)panelIcons).Controls.Add((Control)(object)stiFontIconSelectionControl2);
	}

	private void CheckColorIcons()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)iconsPopup.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = condition.IconColor;
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
		((Control)panelCustomIcon).Invalidate();
		UpdateState();
	}

	internal void UpdateSample(object sender = null)
	{
		StiPivotTableElementCondition pivotTableElementCondition = GetPivotTableElementCondition(sender);
		((Control)lbAaBbCc).Font = pivotTableElementCondition.Font;
		((Control)lbAaBbCc).ForeColor = pivotTableElementCondition.TextColor;
		((Control)lbAaBbCc).BackColor = pivotTableElementCondition.BackColor;
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_0a3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a46: Expected O, but got Unknown
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Expected O, but got Unknown
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e52: Expected O, but got Unknown
		//IL_12f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1302: Expected O, but got Unknown
		//IL_1554: Unknown result type (might be due to invalid IL or missing references)
		//IL_1578: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e3: Expected O, but got Unknown
		//IL_15fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1797: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a1: Expected O, but got Unknown
		//IL_17bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1847: Unknown result type (might be due to invalid IL or missing references)
		//IL_1851: Expected O, but got Unknown
		//IL_186b: Unknown result type (might be due to invalid IL or missing references)
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1992: Unknown result type (might be due to invalid IL or missing references)
		//IL_199c: Expected O, but got Unknown
		//IL_19a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b3: Expected O, but got Unknown
		//IL_19c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ca: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiPivotTableElementConditionsControl));
		dtpValue = new DateTimePicker();
		lbFieldIs = new StiHitTestVisibleLabel();
		tbValue = new Stimulsoft.Controls.StiTextBox();
		cbDataType = new StiComboBox();
		cbItem = new StiComboBox();
		cbCondition = new StiComboBox();
		lbCondition = new StiHitTestVisibleLabel();
		lbDataType = new StiHitTestVisibleLabel();
		lbValue = new StiHitTestVisibleLabel();
		comboBoxIconAlign = new StiComboBox();
		groupLine = new StiGroupLine();
		buttonExpression = new StiOperationButton();
		tbBackColor = new StiColorButtonXF();
		tbTextColor = new StiColorButtonXF();
		tbFontStyleUnderline = new StiToolButton();
		tbFontStyleItalic = new StiToolButton();
		tbFontStyleBold = new StiToolButton();
		lbAaBbCc = new Label();
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
		tbFontStyleStrikeout = new StiToolButton();
		cbFontSize = new StiDropDownBox();
		panelBoxIcon = new Panel();
		buttonCustomIcon = new StiOperationButton();
		buttonIcon = new StiOperationButton();
		panelLine = new Panel();
		buttonRemove = (Button)(object)new StiOperationButton();
		buttonOpen = (Button)(object)new StiOperationButton();
		panelCustomIcon = new Panel();
		((Control)panelBoxIcon).SuspendLayout();
		((Control)panelCustomIcon).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)dtpValue).Anchor = (AnchorStyles)13;
		dtpValue.Format = (DateTimePickerFormat)2;
		((Control)dtpValue).Location = new Point(362, 28);
		((Control)dtpValue).Name = "dtpValue";
		((Control)dtpValue).Size = new Size(189, 20);
		((Control)dtpValue).TabIndex = 30;
		((Control)dtpValue).Layout += new LayoutEventHandler(DtpValue_Layout);
		((Control)dtpValue).Resize += DtpValue_Resize;
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
		((Control)tbValue).Location = new Point(362, 28);
		((Control)tbValue).Name = "tbValue";
		((Control)tbValue).Size = new Size(189, 21);
		((Control)tbValue).TabIndex = 5;
		((Control)cbDataType).BackColor = SystemColors.Window;
		cbDataType.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbDataType).Location = new Point(130, 28);
		((Control)cbDataType).Name = "cbDataType";
		((Control)cbDataType).Size = new Size(104, 21);
		((Control)cbDataType).TabIndex = 10;
		((ComboBox)cbDataType).SelectedIndexChanged += ComboBoxDataType_SelectedIndexChanged;
		((Control)cbItem).BackColor = SystemColors.Window;
		cbItem.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbItem).Location = new Point(14, 28);
		((Control)cbItem).Name = "cbItem";
		((Control)cbItem).Size = new Size(104, 21);
		((ComboBox)cbItem).Sorted = true;
		((Control)cbItem).TabIndex = 1;
		((ComboBox)cbItem).SelectedIndexChanged += ComboBoxItem_SelectedIndexChanged;
		cbCondition.AutoDropDownWidth = false;
		((Control)cbCondition).BackColor = SystemColors.Window;
		cbCondition.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbCondition).DropDownWidth = 300;
		((Control)cbCondition).Location = new Point(246, 28);
		((Control)cbCondition).Name = "cbCondition";
		((Control)cbCondition).Size = new Size(104, 21);
		((Control)cbCondition).TabIndex = 20;
		((Control)lbCondition).BackColor = Color.Transparent;
		((Control)lbCondition).ForeColor = Color.DimGray;
		((Control)lbCondition).Location = new Point(246, 12);
		((Control)lbCondition).Name = "lbCondition";
		((Control)lbCondition).Size = new Size(104, 13);
		((Control)lbCondition).TabIndex = 35;
		((Control)lbCondition).Text = "Condition";
		((Control)lbDataType).BackColor = Color.Transparent;
		((Control)lbDataType).ForeColor = Color.DimGray;
		((Control)lbDataType).Location = new Point(130, 12);
		((Control)lbDataType).Name = "lbDataType";
		((Control)lbDataType).Size = new Size(104, 13);
		((Control)lbDataType).TabIndex = 32;
		((Control)lbDataType).Text = "Data Type";
		((Control)lbValue).BackColor = Color.Transparent;
		((Control)lbValue).ForeColor = Color.DimGray;
		((Control)lbValue).Location = new Point(363, 12);
		((Control)lbValue).Name = "lbValue";
		((Control)lbValue).Size = new Size(112, 13);
		((Control)lbValue).TabIndex = 33;
		((Control)lbValue).Text = "Value";
		comboBoxIconAlign.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxIconAlign).FormattingEnabled = true;
		((Control)comboBoxIconAlign).Location = new Point(362, 88);
		((Control)comboBoxIconAlign).Name = "comboBoxIconAlign";
		((Control)comboBoxIconAlign).Size = new Size(74, 21);
		((Control)comboBoxIconAlign).TabIndex = 140;
		((ComboBox)comboBoxIconAlign).SelectedIndexChanged += ComboBoxIconAlign_SelectedIndexChanged;
		((Control)groupLine).BackColor = Color.FromArgb(250, 250, 250);
		((Control)groupLine).Dock = (DockStyle)2;
		groupLine.LinePosition = StiLinePosition.Bottom;
		groupLine.LineStyle = StiLineStyle.Dot;
		((Control)groupLine).Location = new Point(0, 156);
		((Control)groupLine).Name = "groupLine";
		((Control)groupLine).Size = new Size(564, 2);
		((Control)groupLine).TabIndex = 72;
		((Control)buttonExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpression).Image = (Image)componentResourceManager.GetObject("buttonExpression.Image");
		((Control)buttonExpression).Location = new Point(528, 27);
		((Control)buttonExpression).Name = "buttonExpression";
		((Control)buttonExpression).Size = new Size(25, 22);
		((Control)buttonExpression).TabIndex = 6;
		((ButtonBase)buttonExpression).UseVisualStyleBackColor = true;
		((Control)buttonExpression).Visible = false;
		tbBackColor.Draw3DButton = false;
		((Control)tbBackColor).ForeColor = Color.Transparent;
		tbBackColor.HotDragDrop = false;
		((ButtonBase)tbBackColor).Image = (Image)componentResourceManager.GetObject("tbBackColor.Image");
		((Control)tbBackColor).Location = new Point(267, 118);
		((Control)tbBackColor).Name = "tbBackColor";
		tbBackColor.SelectedColor = Color.Transparent;
		((Control)tbBackColor).Size = new Size(32, 24);
		tbBackColor.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbBackColor).TabIndex = 120;
		((ButtonBase)tbBackColor).UseVisualStyleBackColor = false;
		tbBackColor.SelectedColorChanged += TbBackColor_SelectedIndexChanged;
		tbTextColor.Draw3DButton = false;
		((Control)tbTextColor).ForeColor = Color.Red;
		tbTextColor.HotDragDrop = false;
		((ButtonBase)tbTextColor).Image = (Image)componentResourceManager.GetObject("tbTextColor.Image");
		((Control)tbTextColor).Location = new Point(231, 118);
		((Control)tbTextColor).Name = "tbTextColor";
		tbTextColor.SelectedColor = Color.Transparent;
		((Control)tbTextColor).Size = new Size(32, 24);
		tbTextColor.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbTextColor).TabIndex = 110;
		((ButtonBase)tbTextColor).UseVisualStyleBackColor = false;
		tbTextColor.SelectedColorChanged += TbTextColor_SelectedIndexChanged;
		tbFontStyleUnderline.Draw3DButton = false;
		tbFontStyleUnderline.HotDragDrop = false;
		((ButtonBase)tbFontStyleUnderline).Image = (Image)componentResourceManager.GetObject("tbFontStyleUnderline.Image");
		((Control)tbFontStyleUnderline).Location = new Point(178, 118);
		((Control)tbFontStyleUnderline).Name = "tbFontStyleUnderline";
		((Control)tbFontStyleUnderline).Size = new Size(24, 24);
		tbFontStyleUnderline.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleUnderline).TabIndex = 90;
		((ButtonBase)tbFontStyleUnderline).UseVisualStyleBackColor = false;
		((Control)tbFontStyleUnderline).Click += TbFontStyleUnderline_Click;
		tbFontStyleItalic.Draw3DButton = false;
		tbFontStyleItalic.HotDragDrop = false;
		((ButtonBase)tbFontStyleItalic).Image = (Image)componentResourceManager.GetObject("tbFontStyleItalic.Image");
		((Control)tbFontStyleItalic).Location = new Point(154, 118);
		((Control)tbFontStyleItalic).Name = "tbFontStyleItalic";
		((Control)tbFontStyleItalic).Size = new Size(24, 24);
		tbFontStyleItalic.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleItalic).TabIndex = 80;
		((ButtonBase)tbFontStyleItalic).UseVisualStyleBackColor = false;
		((Control)tbFontStyleItalic).Click += TbFontStyleItalic_Click;
		tbFontStyleBold.Draw3DButton = false;
		tbFontStyleBold.HotDragDrop = false;
		((ButtonBase)tbFontStyleBold).Image = (Image)componentResourceManager.GetObject("tbFontStyleBold.Image");
		((Control)tbFontStyleBold).Location = new Point(130, 118);
		((Control)tbFontStyleBold).Name = "tbFontStyleBold";
		((Control)tbFontStyleBold).Size = new Size(24, 24);
		tbFontStyleBold.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleBold).TabIndex = 70;
		((ButtonBase)tbFontStyleBold).UseVisualStyleBackColor = false;
		((Control)tbFontStyleBold).Click += TbFontStyleBold_Click;
		lbAaBbCc.BorderStyle = (BorderStyle)1;
		((Control)lbAaBbCc).Location = new Point(14, 88);
		((Control)lbAaBbCc).Name = "lbAaBbCc";
		((Control)lbAaBbCc).Size = new Size(104, 55);
		((Control)lbAaBbCc).TabIndex = 40;
		((Control)lbAaBbCc).Text = "AaBbCcYyZz";
		lbAaBbCc.TextAlign = (ContentAlignment)32;
		((Control)colorBoxIconColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxIconColor).Anchor = (AnchorStyles)13;
		colorBoxIconColor.AutoExpandOnClick = true;
		colorBoxIconColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxIconColor.EnableMarkup = false;
		colorBoxIconColor.Is20Button = false;
		((Control)colorBoxIconColor).Location = new Point(445, 118);
		((Control)colorBoxIconColor).Margin = new Padding(2);
		((Control)colorBoxIconColor).Name = "colorBoxIconColor";
		((Control)colorBoxIconColor).Size = new Size(108, 24);
		((Control)colorBoxIconColor).TabIndex = 190;
		colorBoxIconColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxIconColor.SelectedColorChanged += ColorBoxIconColor_SelectedColorChanged;
		((Control)lbFont).BackColor = Color.Transparent;
		((Control)lbFont).ForeColor = Color.DimGray;
		((Control)lbFont).Location = new Point(14, 72);
		((Control)lbFont).Name = "lbFont";
		((Control)lbFont).Size = new Size(104, 14);
		((Control)lbFont).TabIndex = 34;
		((Control)lbFont).Text = "Font";
		((Control)lbIcon).BackColor = Color.Transparent;
		((Control)lbIcon).ForeColor = Color.DimGray;
		((Control)lbIcon).Location = new Point(363, 72);
		((Control)lbIcon).Name = "lbIcon";
		((Control)lbIcon).Size = new Size(104, 13);
		((Control)lbIcon).TabIndex = 34;
		((Control)lbIcon).Text = "Icon";
		cbFont.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)cbFont).FormattingEnabled = true;
		((Control)cbFont).Location = new Point(130, 88);
		((ComboBox)cbFont).MaxDropDownItems = 30;
		((Control)cbFont).Name = "cbFont";
		((Control)cbFont).Size = new Size(169, 21);
		((Control)cbFont).TabIndex = 50;
		((ComboBox)cbFont).SelectedIndexChanged += CbFont_SelectedIndexChanged;
		((Control)buttonPermissions).AccessibleRole = (AccessibleRole)43;
		buttonPermissions.AutoExpandOnClick = true;
		buttonPermissions.ColorTable = eButtonColor.OrangeWithBackground;
		buttonPermissions.Image = (Image)componentResourceManager.GetObject("buttonPermissions.Image");
		buttonPermissions.Is20Button = false;
		((Control)buttonPermissions).Location = new Point(309, 118);
		((Control)buttonPermissions).Name = "buttonPermissions";
		((Control)buttonPermissions).Size = new Size(42, 25);
		buttonPermissions.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonPermissions.SubItems.AddRange(new BaseItem[8] { buttonItemAllowFontName, buttonItemAllowFontSize, buttonItemAllowFontBold, buttonItemAllowFontItalic, buttonItemAllowFontUnderline, buttonItemAllowFontStrikeout, buttonItemAllowTextColor, buttonItemAllowBackColor });
		((Control)buttonPermissions).TabIndex = 130;
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
		tbFontStyleStrikeout.Draw3DButton = false;
		tbFontStyleStrikeout.HotDragDrop = false;
		((ButtonBase)tbFontStyleStrikeout).Image = (Image)componentResourceManager.GetObject("tbFontStyleStrikeout.Image");
		((Control)tbFontStyleStrikeout).Location = new Point(202, 118);
		((Control)tbFontStyleStrikeout).Name = "tbFontStyleStrikeout";
		((Control)tbFontStyleStrikeout).Size = new Size(24, 24);
		tbFontStyleStrikeout.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleStrikeout).TabIndex = 100;
		((ButtonBase)tbFontStyleStrikeout).UseVisualStyleBackColor = false;
		((Control)tbFontStyleStrikeout).Click += TbFontStyleStrikeout_Click;
		((Control)cbFontSize).BackColor = SystemColors.Window;
		cbFontSize.DrawMode = (DrawMode)2;
		((Control)cbFontSize).ForeColor = SystemColors.ControlText;
		cbFontSize.IsEditable = true;
		cbFontSize.Items.AddRange(new object[19]
		{
			"5", "6", "7", "8", "9", "10", "11", "12", "14", "16",
			"18", "20", "22", "24", "26", "28", "36", "48", "76"
		});
		((Control)cbFontSize).Location = new Point(309, 88);
		((Control)cbFontSize).Name = "cbFontSize";
		cbFontSize.SelectedIndex = -1;
		cbFontSize.SelectedItem = null;
		cbFontSize.SelectedValue = null;
		((Control)cbFontSize).Size = new Size(41, 21);
		((Control)cbFontSize).TabIndex = 60;
		cbFontSize.SelectedValueChanged += CbFontSize_SelectedValueChanged;
		((Control)panelBoxIcon).Anchor = (AnchorStyles)13;
		((Control)panelBoxIcon).BackColor = Color.White;
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonCustomIcon);
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIcon);
		((Control)panelBoxIcon).Location = new Point(445, 88);
		((Control)panelBoxIcon).Margin = new Padding(1);
		((Control)panelBoxIcon).Name = "panelBoxIcon";
		((Control)panelBoxIcon).Padding = new Padding(0, 1, 1, 1);
		((Control)panelBoxIcon).Size = new Size(108, 25);
		((Control)panelBoxIcon).TabIndex = 180;
		((Control)panelBoxIcon).Click += ButtonIcon_Click;
		((Control)buttonCustomIcon).Dock = (DockStyle)4;
		((Control)buttonCustomIcon).Font = new Font("Microsoft Sans Serif", 6.5f);
		((Control)buttonCustomIcon).Location = new Point(61, 1);
		((Control)buttonCustomIcon).Margin = new Padding(0);
		((Control)buttonCustomIcon).Name = "buttonCustomIcon";
		((Control)buttonCustomIcon).Size = new Size(23, 23);
		((Control)buttonCustomIcon).TabIndex = 4;
		((Control)buttonCustomIcon).TabStop = false;
		((Control)buttonCustomIcon).Tag = ".";
		((Control)buttonCustomIcon).Text = "...";
		((ButtonBase)buttonCustomIcon).UseVisualStyleBackColor = true;
		((Control)buttonCustomIcon).Click += ButtonCustomIcon_CustomIconClick;
		((Control)buttonIcon).Dock = (DockStyle)4;
		((Control)buttonIcon).Location = new Point(84, 1);
		((Control)buttonIcon).Margin = new Padding(0);
		((Control)buttonIcon).Name = "buttonIcon";
		((Control)buttonIcon).Size = new Size(23, 23);
		((Control)buttonIcon).TabIndex = 1;
		((Control)buttonIcon).TabStop = false;
		((ButtonBase)buttonIcon).UseVisualStyleBackColor = true;
		((Control)buttonIcon).Click += ButtonIcon_Click;
		((Control)panelLine).Anchor = (AnchorStyles)13;
		((Control)panelLine).BackColor = Color.Gainsboro;
		((Control)panelLine).Location = new Point(14, 61);
		((Control)panelLine).Name = "panelLine";
		((Control)panelLine).Size = new Size(538, 1);
		((Control)panelLine).TabIndex = 89;
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(78, 28);
		((Control)buttonRemove).Margin = new Padding(1);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(24, 23);
		((Control)buttonRemove).TabIndex = 170;
		((Control)buttonRemove).TabStop = false;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)buttonOpen).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonOpen).Image = (Image)componentResourceManager.GetObject("buttonOpen.Image");
		((Control)buttonOpen).Location = new Point(78, 3);
		((Control)buttonOpen).Margin = new Padding(1);
		((Control)buttonOpen).Name = "buttonOpen";
		((Control)buttonOpen).Size = new Size(24, 23);
		((Control)buttonOpen).TabIndex = 160;
		((Control)buttonOpen).TabStop = false;
		((ButtonBase)buttonOpen).UseVisualStyleBackColor = true;
		((Control)buttonOpen).Click += ButtonOpen_Click;
		((Control)panelCustomIcon).AllowDrop = true;
		((Control)panelCustomIcon).Anchor = (AnchorStyles)13;
		((Control)panelCustomIcon).Controls.Add((Control)(object)buttonOpen);
		((Control)panelCustomIcon).Controls.Add((Control)(object)buttonRemove);
		((Control)panelCustomIcon).Location = new Point(445, 88);
		((Control)panelCustomIcon).Margin = new Padding(2);
		((Control)panelCustomIcon).Name = "panelCustomIcon";
		((Control)panelCustomIcon).Size = new Size(108, 54);
		((Control)panelCustomIcon).TabIndex = 150;
		((Control)panelCustomIcon).Visible = false;
		((Control)panelCustomIcon).DragDrop += new DragEventHandler(PanelImage_DragDrop);
		((Control)panelCustomIcon).DragOver += new DragEventHandler(PanelImage_DragOver);
		((Control)panelCustomIcon).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelCustomIcon).DoubleClick += PanelImage_DoubleClick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panelLine);
		((Control)this).Controls.Add((Control)(object)panelCustomIcon);
		((Control)this).Controls.Add((Control)(object)panelBoxIcon);
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
		((Control)this).Controls.Add((Control)(object)lbAaBbCc);
		((Control)this).Controls.Add((Control)(object)groupLine);
		((Control)this).Controls.Add((Control)(object)comboBoxIconAlign);
		((Control)this).Controls.Add((Control)(object)buttonExpression);
		((Control)this).Controls.Add((Control)(object)dtpValue);
		((Control)this).Controls.Add((Control)(object)lbIcon);
		((Control)this).Controls.Add((Control)(object)lbFont);
		((Control)this).Controls.Add((Control)(object)lbFieldIs);
		((Control)this).Controls.Add((Control)(object)tbValue);
		((Control)this).Controls.Add((Control)(object)cbDataType);
		((Control)this).Controls.Add((Control)(object)cbItem);
		((Control)this).Controls.Add((Control)(object)cbCondition);
		((Control)this).Controls.Add((Control)(object)lbCondition);
		((Control)this).Controls.Add((Control)(object)lbDataType);
		((Control)this).Controls.Add((Control)(object)lbValue);
		((Control)this).Name = "StiPivotTableElementConditionsControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(564, 158);
		((UserControl)this).Load += StiPivotTableElementConditionsControl_Load;
		((Control)panelBoxIcon).ResumeLayout(false);
		((Control)panelCustomIcon).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
