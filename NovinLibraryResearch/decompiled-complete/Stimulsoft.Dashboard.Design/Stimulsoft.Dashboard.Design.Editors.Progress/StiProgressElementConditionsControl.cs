using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors.Progress;

public class StiProgressElementConditionsControl : StiZoneControl
{
	private StiProgressElement progressElement;

	private StiProgressElementCondition condition;

	private IContainer components;

	private StiHitTestVisibleLabel lbFieldIs;

	public Stimulsoft.Controls.StiTextBox tbValue;

	public StiComboBox cbItem;

	public StiComboBox cbCondition;

	private StiHitTestVisibleLabel lbCondition;

	private StiHitTestVisibleLabel lbValue;

	public StiGroupLine groupLine;

	public StiColorButtonXF tbTextColor;

	public StiToolButton tbFontStyleUnderline;

	public StiToolButton tbFontStyleItalic;

	public StiToolButton tbFontStyleBold;

	private StiHitTestVisibleLabel lbColor;

	private ButtonX buttonPermissions;

	internal ButtonItem buttonItemAllowFontBold;

	internal ButtonItem buttonItemAllowFontItalic;

	internal ButtonItem buttonItemAllowFontUnderline;

	internal ButtonItem buttonItemAllowFontStrikeout;

	internal ButtonItem buttonItemAllowTextColor;

	public StiToolButton tbFontStyleStrikeout;

	private ColorPickerButton colorBoxColor;

	private Panel panelLine;

	private ButtonItem buttonItemAllowColor;

	private ButtonItem buttonItemAllowTrackColor;

	private ColorPickerButton colorBoxTrackColor;

	private StiHitTestVisibleLabel lbTrackColor;

	public StiProgressElementConditionsControl(StiProgressElement progressElement, StiProgressElementCondition condition, bool isNew = true)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			base.DrawBorder = false;
			Localize();
			this.progressElement = progressElement;
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
		((Control)lbColor).Text = StiLocalization.Get("PropertyMain", "Color");
		((Control)lbTrackColor).Text = StiLocalization.Get("PropertyMain", "TrackColor");
		buttonItemAllowFontBold.Text = StiLocalization.Get("PropertyMain", "FontBold");
		buttonItemAllowFontItalic.Text = StiLocalization.Get("PropertyMain", "FontItalic");
		buttonItemAllowFontUnderline.Text = StiLocalization.Get("PropertyMain", "FontUnderline");
		buttonItemAllowFontStrikeout.Text = StiLocalization.Get("PropertyMain", "FontStrikeout");
		buttonItemAllowTextColor.Text = StiLocalization.Get("PropertyMain", "TextColor");
		buttonItemAllowColor.Text = StiLocalization.Get("PropertyMain", "Color");
		buttonItemAllowTrackColor.Text = StiLocalization.Get("PropertyMain", "TrackColor");
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

	private void ComboBoxIconAlign_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void DoScaling()
	{
		((ButtonBase)tbFontStyleBold).Image = StiReportDesignImages.Editor.FontStyleBold();
		((ButtonBase)tbFontStyleItalic).Image = StiReportDesignImages.Editor.FontStyleItalic();
		((ButtonBase)tbFontStyleUnderline).Image = StiReportDesignImages.Editor.FontStyleUnderline();
		((ButtonBase)tbTextColor).Image = StiReportDesignImages.Editor.FontColor();
		((ButtonBase)tbFontStyleStrikeout).Image = StiReportDesignImages.Editor.FontStyleStrikethrough();
		buttonPermissions.Image = StiReportDesignImages.Actions.SelectPermissions();
	}

	private void UpdateState()
	{
		((Control)colorBoxColor).Enabled = buttonItemAllowColor.Checked;
		((Control)colorBoxTrackColor).Enabled = buttonItemAllowTrackColor.Checked;
		string text = ((Control)cbCondition).Text;
		((ComboBox)cbCondition).Items.Clear();
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		if (((Control)cbItem).Text == StiLocalization.Get("PropertyEnum", "StiChartLabelsStyleValue") || ((Control)cbItem).Text == StiLocalization.Get("PropertyMain", "Target") || ((Control)cbItem).Text == StiLocalization.Get("FormFormatEditor", "Percentage"))
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
		StiToolButton stiToolButton = tbFontStyleBold;
		Font font = condition.Font;
		stiToolButton.Pushed = font != null && font.Bold;
		StiToolButton stiToolButton2 = tbFontStyleItalic;
		Font font2 = condition.Font;
		stiToolButton2.Pushed = font2 != null && font2.Italic;
		StiToolButton stiToolButton3 = tbFontStyleUnderline;
		Font font3 = condition.Font;
		stiToolButton3.Pushed = font3 != null && font3.Underline;
		StiToolButton stiToolButton4 = tbFontStyleStrikeout;
		Font font4 = condition.Font;
		stiToolButton4.Pushed = font4 != null && font4.Strikeout;
		tbTextColor.SelectedColor = condition.TextColor;
		colorBoxColor.SelectedColor = condition.Color;
		colorBoxTrackColor.SelectedColor = condition.TrackColor;
		StiProgressConditionPermissions permissions = condition.Permissions;
		buttonItemAllowFontBold.Checked = (permissions & StiProgressConditionPermissions.FontStyleBold) > StiProgressConditionPermissions.None;
		buttonItemAllowFontItalic.Checked = (permissions & StiProgressConditionPermissions.FontStyleItalic) > StiProgressConditionPermissions.None;
		buttonItemAllowFontUnderline.Checked = (permissions & StiProgressConditionPermissions.FontStyleUnderline) > StiProgressConditionPermissions.None;
		buttonItemAllowFontStrikeout.Checked = (permissions & StiProgressConditionPermissions.FontStyleStrikeout) > StiProgressConditionPermissions.None;
		buttonItemAllowTextColor.Checked = (permissions & StiProgressConditionPermissions.TextColor) > StiProgressConditionPermissions.None;
		buttonItemAllowColor.Checked = (permissions & StiProgressConditionPermissions.Color) > StiProgressConditionPermissions.None;
		buttonItemAllowTrackColor.Checked = (permissions & StiProgressConditionPermissions.TrackColor) > StiProgressConditionPermissions.None;
	}

	private void FillFieldsIs(bool isNew)
	{
		((ComboBox)cbItem).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiChartLabelsStyleValue"));
		((ComboBox)cbItem).Items.Add((object)StiLocalization.Get("Chart", "Series"));
		((ComboBox)cbItem).Items.Add((object)StiLocalization.Get("PropertyMain", "Target"));
		((ComboBox)cbItem).Items.Add((object)StiLocalization.Get("FormFormatEditor", "Percentage"));
		if (isNew)
		{
			((ListControl)cbItem).SelectedIndex = 0;
			return;
		}
		switch (condition.Field)
		{
		case StiProgressFieldCondition.Value:
			((ListControl)cbItem).SelectedIndex = 0;
			break;
		case StiProgressFieldCondition.Series:
			((ListControl)cbItem).SelectedIndex = 1;
			break;
		case StiProgressFieldCondition.Target:
			((ListControl)cbItem).SelectedIndex = 2;
			break;
		case StiProgressFieldCondition.Percentage:
			((ListControl)cbItem).SelectedIndex = 3;
			break;
		}
	}

	public StiProgressElementCondition GetProgressElementCondition(object sender = null)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		StiProgressFieldCondition field = StiProgressFieldCondition.Value;
		switch (((ListControl)cbItem).SelectedIndex)
		{
		case 0:
			field = StiProgressFieldCondition.Value;
			break;
		case 1:
			field = StiProgressFieldCondition.Series;
			break;
		case 2:
			field = StiProgressFieldCondition.Target;
			break;
		case 3:
			field = StiProgressFieldCondition.Percentage;
			break;
		}
		Stimulsoft.Report.Components.StiFilterCondition stiFilterCondition = StiFilterHelper.ConvertStringToCondition(((Control)cbCondition).Text);
		string text = ((Control)tbValue).Text;
		StiProgressConditionPermissions stiProgressConditionPermissions = StiProgressConditionPermissions.None;
		if (buttonItemAllowFontBold.Checked)
		{
			stiProgressConditionPermissions |= StiProgressConditionPermissions.FontStyleBold;
		}
		if (buttonItemAllowFontItalic.Checked)
		{
			stiProgressConditionPermissions |= StiProgressConditionPermissions.FontStyleItalic;
		}
		if (buttonItemAllowFontUnderline.Checked)
		{
			stiProgressConditionPermissions |= StiProgressConditionPermissions.FontStyleUnderline;
		}
		if (buttonItemAllowFontStrikeout.Checked)
		{
			stiProgressConditionPermissions |= StiProgressConditionPermissions.FontStyleStrikeout;
		}
		if (buttonItemAllowTextColor.Checked)
		{
			stiProgressConditionPermissions |= StiProgressConditionPermissions.TextColor;
		}
		if (buttonItemAllowColor.Checked)
		{
			stiProgressConditionPermissions |= StiProgressConditionPermissions.Color;
		}
		if (buttonItemAllowTrackColor.Checked)
		{
			stiProgressConditionPermissions |= StiProgressConditionPermissions.TrackColor;
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
		Font font = new Font("Arial", 8f, val);
		Color textColor = (buttonItemAllowTextColor.Checked ? tbTextColor.SelectedColor : Color.Black);
		Color selectedColor = colorBoxColor.SelectedColor;
		Color selectedColor2 = colorBoxTrackColor.SelectedColor;
		return new StiProgressElementCondition(field, stiFilterCondition, text, stiProgressConditionPermissions, font, textColor, selectedColor, selectedColor2);
	}

	private void CheckAllows()
	{
		((Control)tbTextColor).Enabled = buttonItemAllowTextColor.Checked;
		((Control)tbFontStyleBold).Enabled = buttonItemAllowFontBold.Checked;
		((Control)tbFontStyleItalic).Enabled = buttonItemAllowFontItalic.Checked;
		((Control)tbFontStyleUnderline).Enabled = buttonItemAllowFontUnderline.Checked;
		((Control)tbFontStyleStrikeout).Enabled = buttonItemAllowFontStrikeout.Checked;
		((Control)colorBoxColor).Enabled = buttonItemAllowColor.Checked;
		((Control)colorBoxTrackColor).Enabled = buttonItemAllowTrackColor.Checked;
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
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Expected O, but got Unknown
		//IL_0c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7b: Expected O, but got Unknown
		//IL_0d9a: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiProgressElementConditionsControl));
		lbFieldIs = new StiHitTestVisibleLabel();
		tbValue = new Stimulsoft.Controls.StiTextBox();
		cbItem = new StiComboBox();
		cbCondition = new StiComboBox();
		lbCondition = new StiHitTestVisibleLabel();
		lbValue = new StiHitTestVisibleLabel();
		groupLine = new StiGroupLine();
		tbTextColor = new StiColorButtonXF();
		tbFontStyleUnderline = new StiToolButton();
		tbFontStyleItalic = new StiToolButton();
		tbFontStyleBold = new StiToolButton();
		colorBoxColor = new ColorPickerButton();
		lbColor = new StiHitTestVisibleLabel();
		buttonPermissions = new ButtonX();
		buttonItemAllowFontBold = new ButtonItem();
		buttonItemAllowFontItalic = new ButtonItem();
		buttonItemAllowFontUnderline = new ButtonItem();
		buttonItemAllowFontStrikeout = new ButtonItem();
		buttonItemAllowTextColor = new ButtonItem();
		buttonItemAllowColor = new ButtonItem();
		buttonItemAllowTrackColor = new ButtonItem();
		tbFontStyleStrikeout = new StiToolButton();
		panelLine = new Panel();
		colorBoxTrackColor = new ColorPickerButton();
		lbTrackColor = new StiHitTestVisibleLabel();
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
		((Control)tbValue).Location = new Point(246, 28);
		((Control)tbValue).Name = "tbValue";
		((Control)tbValue).Size = new Size(173, 21);
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
		((Control)cbCondition).Size = new Size(104, 21);
		((Control)cbCondition).TabIndex = 1;
		((Control)lbCondition).BackColor = Color.Transparent;
		((Control)lbCondition).ForeColor = Color.DimGray;
		((Control)lbCondition).Location = new Point(130, 12);
		((Control)lbCondition).Name = "lbCondition";
		((Control)lbCondition).Size = new Size(104, 13);
		((Control)lbCondition).TabIndex = 35;
		((Control)lbCondition).Text = "Condition";
		((Control)lbValue).BackColor = Color.Transparent;
		((Control)lbValue).ForeColor = Color.DimGray;
		((Control)lbValue).Location = new Point(246, 12);
		((Control)lbValue).Name = "lbValue";
		((Control)lbValue).Size = new Size(152, 13);
		((Control)lbValue).TabIndex = 33;
		((Control)lbValue).Text = "Value";
		((Control)groupLine).BackColor = Color.FromArgb(250, 250, 250);
		((Control)groupLine).Dock = (DockStyle)2;
		groupLine.LinePosition = StiLinePosition.Bottom;
		groupLine.LineStyle = StiLineStyle.Dot;
		((Control)groupLine).Location = new Point(0, 122);
		((Control)groupLine).Name = "groupLine";
		((Control)groupLine).Size = new Size(438, 2);
		((Control)groupLine).TabIndex = 72;
		tbTextColor.Draw3DButton = false;
		((Control)tbTextColor).ForeColor = Color.Red;
		tbTextColor.HotDragDrop = false;
		((ButtonBase)tbTextColor).Image = (Image)componentResourceManager.GetObject("tbTextColor.Image");
		((Control)tbTextColor).Location = new Point(166, 86);
		((Control)tbTextColor).Name = "tbTextColor";
		tbTextColor.SelectedColor = Color.Transparent;
		((Control)tbTextColor).Size = new Size(32, 24);
		tbTextColor.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbTextColor).TabIndex = 11;
		((ButtonBase)tbTextColor).UseVisualStyleBackColor = false;
		tbFontStyleUnderline.Draw3DButton = false;
		tbFontStyleUnderline.HotDragDrop = false;
		((ButtonBase)tbFontStyleUnderline).Image = (Image)componentResourceManager.GetObject("tbFontStyleUnderline.Image");
		((Control)tbFontStyleUnderline).Location = new Point(113, 86);
		((Control)tbFontStyleUnderline).Name = "tbFontStyleUnderline";
		((Control)tbFontStyleUnderline).Size = new Size(24, 24);
		tbFontStyleUnderline.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleUnderline).TabIndex = 9;
		((ButtonBase)tbFontStyleUnderline).UseVisualStyleBackColor = false;
		tbFontStyleItalic.Draw3DButton = false;
		tbFontStyleItalic.HotDragDrop = false;
		((ButtonBase)tbFontStyleItalic).Image = (Image)componentResourceManager.GetObject("tbFontStyleItalic.Image");
		((Control)tbFontStyleItalic).Location = new Point(89, 86);
		((Control)tbFontStyleItalic).Name = "tbFontStyleItalic";
		((Control)tbFontStyleItalic).Size = new Size(24, 24);
		tbFontStyleItalic.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleItalic).TabIndex = 8;
		((ButtonBase)tbFontStyleItalic).UseVisualStyleBackColor = false;
		tbFontStyleBold.Draw3DButton = false;
		tbFontStyleBold.HotDragDrop = false;
		((ButtonBase)tbFontStyleBold).Image = (Image)componentResourceManager.GetObject("tbFontStyleBold.Image");
		((Control)tbFontStyleBold).Location = new Point(65, 86);
		((Control)tbFontStyleBold).Name = "tbFontStyleBold";
		((Control)tbFontStyleBold).Size = new Size(24, 24);
		tbFontStyleBold.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleBold).TabIndex = 7;
		((ButtonBase)tbFontStyleBold).UseVisualStyleBackColor = false;
		((Control)colorBoxColor).AccessibleRole = (AccessibleRole)43;
		colorBoxColor.AutoExpandOnClick = true;
		colorBoxColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxColor.EnableMarkup = false;
		colorBoxColor.Is20Button = false;
		((Control)colorBoxColor).Location = new Point(213, 86);
		((Control)colorBoxColor).Margin = new Padding(2);
		((Control)colorBoxColor).Name = "colorBoxColor";
		((Control)colorBoxColor).Size = new Size(90, 24);
		((Control)colorBoxColor).TabIndex = 19;
		colorBoxColor.TextAlignment = eButtonTextAlignment.Right;
		((Control)lbColor).BackColor = Color.Transparent;
		((Control)lbColor).ForeColor = Color.DimGray;
		((Control)lbColor).Location = new Point(211, 70);
		((Control)lbColor).Name = "lbColor";
		((Control)lbColor).Size = new Size(104, 13);
		((Control)lbColor).TabIndex = 34;
		((Control)lbColor).Text = "Color";
		((Control)buttonPermissions).AccessibleRole = (AccessibleRole)43;
		buttonPermissions.AutoExpandOnClick = true;
		buttonPermissions.ColorTable = eButtonColor.OrangeWithBackground;
		buttonPermissions.Image = (Image)componentResourceManager.GetObject("buttonPermissions.Image");
		buttonPermissions.Is20Button = false;
		((Control)buttonPermissions).Location = new Point(14, 87);
		((Control)buttonPermissions).Name = "buttonPermissions";
		((Control)buttonPermissions).Size = new Size(41, 23);
		buttonPermissions.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonPermissions.SubItems.AddRange(new BaseItem[7] { buttonItemAllowFontBold, buttonItemAllowFontItalic, buttonItemAllowFontUnderline, buttonItemAllowFontStrikeout, buttonItemAllowTextColor, buttonItemAllowColor, buttonItemAllowTrackColor });
		((Control)buttonPermissions).TabIndex = 3;
		buttonPermissions.ThemeAware = true;
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
		buttonItemAllowColor.AutoCheckOnClick = true;
		buttonItemAllowColor.AutoCollapseOnClick = false;
		buttonItemAllowColor.BeginGroup = true;
		buttonItemAllowColor.Checked = true;
		buttonItemAllowColor.GlobalItem = false;
		buttonItemAllowColor.Name = "buttonItemAllowColor";
		buttonItemAllowColor.Text = "Color";
		buttonItemAllowColor.ThemeAware = true;
		buttonItemAllowColor.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowTrackColor.AutoCheckOnClick = true;
		buttonItemAllowTrackColor.AutoCollapseOnClick = false;
		buttonItemAllowTrackColor.Checked = true;
		buttonItemAllowTrackColor.GlobalItem = false;
		buttonItemAllowTrackColor.Name = "buttonItemAllowTrackColor";
		buttonItemAllowTrackColor.Text = "Track Color";
		buttonItemAllowTrackColor.ThemeAware = true;
		buttonItemAllowTrackColor.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		tbFontStyleStrikeout.Draw3DButton = false;
		tbFontStyleStrikeout.HotDragDrop = false;
		((ButtonBase)tbFontStyleStrikeout).Image = (Image)componentResourceManager.GetObject("tbFontStyleStrikeout.Image");
		((Control)tbFontStyleStrikeout).Location = new Point(137, 86);
		((Control)tbFontStyleStrikeout).Name = "tbFontStyleStrikeout";
		((Control)tbFontStyleStrikeout).Size = new Size(24, 24);
		tbFontStyleStrikeout.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleStrikeout).TabIndex = 10;
		((ButtonBase)tbFontStyleStrikeout).UseVisualStyleBackColor = false;
		((Control)panelLine).Anchor = (AnchorStyles)13;
		((Control)panelLine).BackColor = Color.Gainsboro;
		((Control)panelLine).Location = new Point(14, 61);
		((Control)panelLine).Name = "panelLine";
		((Control)panelLine).Size = new Size(412, 1);
		((Control)panelLine).TabIndex = 89;
		((Control)colorBoxTrackColor).AccessibleRole = (AccessibleRole)43;
		colorBoxTrackColor.AutoExpandOnClick = true;
		colorBoxTrackColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxTrackColor.EnableMarkup = false;
		colorBoxTrackColor.Is20Button = false;
		((Control)colorBoxTrackColor).Location = new Point(317, 86);
		((Control)colorBoxTrackColor).Margin = new Padding(2);
		((Control)colorBoxTrackColor).Name = "colorBoxTrackColor";
		((Control)colorBoxTrackColor).Size = new Size(90, 24);
		((Control)colorBoxTrackColor).TabIndex = 90;
		colorBoxTrackColor.TextAlignment = eButtonTextAlignment.Right;
		((Control)lbTrackColor).BackColor = Color.Transparent;
		((Control)lbTrackColor).ForeColor = Color.DimGray;
		((Control)lbTrackColor).Location = new Point(315, 70);
		((Control)lbTrackColor).Name = "lbTrackColor";
		((Control)lbTrackColor).Size = new Size(104, 13);
		((Control)lbTrackColor).TabIndex = 91;
		((Control)lbTrackColor).Text = "Track Color";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)colorBoxTrackColor);
		((Control)this).Controls.Add((Control)(object)lbTrackColor);
		((Control)this).Controls.Add((Control)(object)panelLine);
		((Control)this).Controls.Add((Control)(object)colorBoxColor);
		((Control)this).Controls.Add((Control)(object)tbFontStyleStrikeout);
		((Control)this).Controls.Add((Control)(object)buttonPermissions);
		((Control)this).Controls.Add((Control)(object)tbTextColor);
		((Control)this).Controls.Add((Control)(object)tbFontStyleUnderline);
		((Control)this).Controls.Add((Control)(object)tbFontStyleItalic);
		((Control)this).Controls.Add((Control)(object)tbFontStyleBold);
		((Control)this).Controls.Add((Control)(object)groupLine);
		((Control)this).Controls.Add((Control)(object)lbColor);
		((Control)this).Controls.Add((Control)(object)lbFieldIs);
		((Control)this).Controls.Add((Control)(object)tbValue);
		((Control)this).Controls.Add((Control)(object)cbItem);
		((Control)this).Controls.Add((Control)(object)cbCondition);
		((Control)this).Controls.Add((Control)(object)lbCondition);
		((Control)this).Controls.Add((Control)(object)lbValue);
		((Control)this).Name = "StiProgressElementConditionsControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(438, 124);
		((Control)this).ResumeLayout(false);
	}
}
