using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiExcelSetupForm : Form
{
	private StiExportFormat exportFormat = StiExportFormat.Excel2007;

	private IContainer components;

	private StiPagesRangeControl pagesRange;

	private Panel panelOpenAfterExport;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiGroupLine groupButtons;

	private Button buttonCancel;

	private Button buttonOk;

	private StiGroupBox groupBoxExportSettings;

	private Panel panelType;

	private StiLabelInfo labelType;

	private StiComboBox comboBoxType;

	private Panel panelExportSettings;

	private StiComboBox comboBoxImageQuality;

	private Label label1;

	private StiLabelInfo labelImageQuality;

	private StiComboBox comboBoxImageResolution;

	private Label label6;

	private StiLabelInfo labelResolution;

	private StiCheckBoxInfo checkBoxExportPageBreaks;

	private StiCheckBoxInfo checkBoxExportEachPageToSheet;

	private StiCheckBoxInfo checkBoxExportObjectFormatting;

	private StiCheckBoxInfo checkBoxUseOnePageHeaderAndFooter;

	private Panel panelExcel2007;

	private StiComboBox comboBoxRestrictEditing;

	private StiLabelInfo labelRestrictEditing;

	private StiLabelInfo labelDataExportMode;

	internal StiComboBox comboBoxDataExportMode;

	public StiExportFormat ExportFormat
	{
		get
		{
			return exportFormat;
		}
		set
		{
			exportFormat = value;
			SetTypeSelectedIndex(exportFormat);
			((Control)comboBoxType).Enabled = false;
		}
	}

	public StiPagesRange PagesRange => pagesRange.GetPagesRange();

	public bool OpenAfterExport => ((CheckBox)checkBoxOpenAfterExport).Checked;

	public int CurrentPage
	{
		get
		{
			return pagesRange.CurrentPage;
		}
		set
		{
			pagesRange.CurrentPage = value;
		}
	}

	public bool OpenAfterExportVisible
	{
		get
		{
			return ((Control)checkBoxOpenAfterExport).Visible;
		}
		set
		{
			((Control)checkBoxOpenAfterExport).Visible = value;
		}
	}

	public bool OpenAfterExportEnabled
	{
		get
		{
			return ((Control)checkBoxOpenAfterExport).Enabled;
		}
		set
		{
			((Control)checkBoxOpenAfterExport).Enabled = value;
		}
	}

	public bool UseOnePageHeaderAndFooter => ((CheckBox)checkBoxUseOnePageHeaderAndFooter).Checked;

	public bool ExportObjectFormatting => ((CheckBox)checkBoxExportObjectFormatting).Checked;

	public bool ExportEachPageToSheet => ((CheckBox)checkBoxExportEachPageToSheet).Checked;

	public bool ExportPageBreaks => ((CheckBox)checkBoxExportPageBreaks).Checked;

	public float Resolution => float.Parse(((Control)comboBoxImageResolution).Text.Replace("%", "").Trim());

	public float ImageQuality
	{
		get
		{
			int.TryParse(((Control)comboBoxImageQuality).Text, out var result);
			return (float)result / 100f;
		}
	}

	public StiExcel2007RestrictEditing RestrictEditing => (StiExcel2007RestrictEditing)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxRestrictEditing).SelectedItem).Tag;

	public StiDataExportMode DataExportMode => ((ListControl)comboBoxDataExportMode).SelectedIndex switch
	{
		0 => StiDataExportMode.Data, 
		1 => StiDataExportMode.DataAndHeaders, 
		2 => StiDataExportMode.DataAndHeadersFooters, 
		_ => StiDataExportMode.AllBands, 
	};

	public StiExcelSetupForm()
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		if (((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0)
		{
			Form val = Application.OpenForms[((ReadOnlyCollectionBase)(object)Application.OpenForms).Count - 1];
			if (val != null && val.TopMost)
			{
				((Form)this).TopMost = true;
			}
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		Localize();
		if (StiOptions.Viewer.Windows.Exports.ShowExcel)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Excel",
				Tag = StiExportFormat.Excel2007
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowExcel2007)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Excel 97-2003",
				Tag = StiExportFormat.Excel
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowExcelXml)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Excel Xml 2003",
				Tag = StiExportFormat.ExcelXml
			});
		}
		((Form)this).Closed += This_Closed;
		groupBoxExportSettings.CollapsedChange += GroupBoxExportSettings_CollapsedChange;
		((ComboBox)comboBoxType).SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
		((ListControl)comboBoxType).SelectedIndex = 0;
		((ListControl)comboBoxImageResolution).SelectedIndex = 4;
		((ListControl)comboBoxImageQuality).SelectedIndex = 3;
		((ComboBox)comboBoxDataExportMode).SelectedIndexChanged += ComboBoxDataExportMode_SelectedIndexChanged;
		((ListControl)comboBoxDataExportMode).SelectedIndex = 3;
		Build();
		StiSettings.Load();
		((CheckBox)checkBoxOpenAfterExport).Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		((CheckBox)checkBoxUseOnePageHeaderAndFooter).Checked = StiSettings.GetBool(((Control)this).Name, "UseOnePageHeaderAndFooter", ((CheckBox)checkBoxUseOnePageHeaderAndFooter).Checked);
		((CheckBox)checkBoxExportObjectFormatting).Checked = StiSettings.GetBool(((Control)this).Name, "ExportObjectFormatting", ((CheckBox)checkBoxExportObjectFormatting).Checked);
		((CheckBox)checkBoxExportEachPageToSheet).Checked = StiSettings.GetBool(((Control)this).Name, "ExportEachPageToSheet", ((CheckBox)checkBoxExportEachPageToSheet).Checked);
		((CheckBox)checkBoxExportPageBreaks).Checked = StiSettings.GetBool(((Control)this).Name, "ExportPageBreaks", ((CheckBox)checkBoxExportPageBreaks).Checked);
		((ListControl)comboBoxImageResolution).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ImageResolution", ((ListControl)comboBoxImageResolution).SelectedIndex);
		((Control)comboBoxImageQuality).Text = StiSettings.GetStr(((Control)this).Name, "ImageQuality", "75");
		groupBoxExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings");
		int @int = StiSettings.GetInt(((Control)this).Name, "Type", 14);
		SetTypeSelectedIndex((StiExportFormat)@int);
		int int2 = StiSettings.GetInt(((Control)this).Name, "RestrictEditing", 1);
		int num = 0;
		foreach (object item in ((ComboBox)comboBoxRestrictEditing).Items)
		{
			if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals(int2))
			{
				break;
			}
			num++;
		}
		((ListControl)comboBoxRestrictEditing).SelectedIndex = ((num < ((ComboBox)comboBoxRestrictEditing).Items.Count) ? num : 0);
		int int3 = StiSettings.GetInt(((Control)this).Name, "DataExportMode", -1);
		if (int3 == -1 && StiSettings.GetBool(((Control)this).Name, "ExportDataOnly", nullValue: false))
		{
			((ListControl)comboBoxDataExportMode).SelectedIndex = 1;
		}
		if (int3 >= 0 && int3 <= 3)
		{
			((ListControl)comboBoxDataExportMode).SelectedIndex = int3;
		}
		LocalizeSuperToolTip();
		((ComboBox)comboBoxDataExportMode).DropDownWidth = StiScale.XXI((double)((ComboBox)comboBoxDataExportMode).DropDownWidth);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_spreadsheets.htm");
	}

	private void This_Closed(object sender, EventArgs e)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(((Control)comboBoxImageQuality).Text))
		{
			((Control)comboBoxImageQuality).Text = "100";
		}
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		StiSettings.Set(((Control)this).Name, "ExportObjectFormatting", ((CheckBox)checkBoxExportObjectFormatting).Checked);
		StiSettings.Set(((Control)this).Name, "UseOnePageHeaderAndFooter", ((CheckBox)checkBoxUseOnePageHeaderAndFooter).Checked);
		StiSettings.Set(((Control)this).Name, "ExportEachPageToSheet", ((CheckBox)checkBoxExportEachPageToSheet).Checked);
		StiSettings.Set(((Control)this).Name, "ExportPageBreaks", ((CheckBox)checkBoxExportPageBreaks).Checked);
		StiSettings.Set(((Control)this).Name, "Type", (int)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxType).SelectedItem).Tag);
		StiSettings.Set(((Control)this).Name, "ImageResolution", ((ListControl)comboBoxImageResolution).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "ImageQuality", ((Control)comboBoxImageQuality).Text);
		StiSettings.Set(((Control)this).Name, "RestrictEditing", (int)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxRestrictEditing).SelectedItem).Tag);
		StiSettings.Set(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "ExportDataOnly", null);
		StiSettings.Set(((Control)this).Name, "DataExportMode", ((ListControl)comboBoxDataExportMode).SelectedIndex);
		StiSettings.Save();
	}

	private void GroupBoxExportSettings_CollapsedChange(object sender, EventArgs e)
	{
		if (!groupBoxExportSettings.Collapsed)
		{
			CheckState();
		}
	}

	private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!groupBoxExportSettings.Collapsed)
		{
			CheckState();
		}
	}

	private void ComboBoxDataExportMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)comboBoxDataExportMode).SelectedIndex < 3)
		{
			((Control)checkBoxExportObjectFormatting).Enabled = true;
			((Control)checkBoxUseOnePageHeaderAndFooter).Enabled = false;
		}
		else
		{
			((Control)checkBoxExportObjectFormatting).Enabled = false;
			((Control)checkBoxUseOnePageHeaderAndFooter).Enabled = true;
		}
	}

	private void Localize()
	{
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)checkBoxUseOnePageHeaderAndFooter).Text = StiLocalization.Get("Export", "UseOnePageHeaderAndFooter");
		((Control)checkBoxExportObjectFormatting).Text = StiLocalization.Get("Export", "ExportObjectFormatting");
		((Control)checkBoxExportEachPageToSheet).Text = StiLocalization.Get("Export", "ExportEachPageToSheet");
		((Control)checkBoxExportPageBreaks).Text = StiLocalization.Get("Export", "ExportPageBreaks");
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)labelType).Text = Loc.GetCleaned("Export", "Type");
		((Control)labelImageQuality).Text = Loc.GetCleaned("Export", "ImageQuality");
		((Control)labelResolution).Text = Loc.GetCleaned("Export", "ImageResolution");
		((Control)labelRestrictEditing).Text = Loc.GetCleaned("Export", "RestrictEditing");
		((Control)labelDataExportMode).Text = Loc.GetCleaned("Export", "BandsFilter");
		((ComboBox)comboBoxDataExportMode).Items.Clear();
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "DataOnly"));
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "DataAndHeaders"));
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "DataAndHeadersFooters"));
		((ComboBox)comboBoxDataExportMode).Items.Add((object)StiLocalization.Get("Export", "AllBands"));
		((Control)this).Text = string.Format("{0} - {1}", StiLocalization.Get("Export", "ExportTypeExcelFile").Replace("...", ""), StiLocalization.Get("Export", "title").Replace("&", "_"));
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		labelType.SetToolTip(StiLocalization.Get("HelpViewer", "TypeExport"));
		labelResolution.SetToolTip(StiLocalization.Get("HelpViewer", "ImageResolution"));
		labelImageQuality.SetToolTip(StiLocalization.Get("HelpViewer", "ImageQuality"));
		labelRestrictEditing.SetToolTip(StiLocalization.Get("HelpViewer", "RestrictEditing"));
		checkBoxOpenAfterExport.SetToolTip(StiLocalization.Get("HelpViewer", "OpenAfterExport"));
		labelDataExportMode.SetToolTip(StiLocalization.Get("HelpViewer", "ExportMode"));
		checkBoxExportObjectFormatting.SetToolTip(StiLocalization.Get("HelpViewer", "ExportObjectFormatting"));
		checkBoxUseOnePageHeaderAndFooter.SetToolTip(StiLocalization.Get("HelpViewer", "UseOnePageHeaderAndFooter"));
		checkBoxExportEachPageToSheet.SetToolTip(StiLocalization.Get("HelpViewer", "ExportEachPageToSheet"));
		checkBoxExportPageBreaks.SetToolTip(StiLocalization.Get("HelpViewer", "ExportPageBreaks"));
	}

	private void Build()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		ComboBoxItem val = new ComboBoxItem();
		((BaseItem)val).Text = StiLocalization.Get("Buttons", "No").Replace("&", "");
		((BaseItem)val).Tag = StiExcel2007RestrictEditing.No;
		((ComboBox)comboBoxRestrictEditing).Items.Add((object)val);
		ComboBoxItem val2 = new ComboBoxItem();
		((BaseItem)val2).Text = StiLocalization.Get("Export", "ExceptEditableFields");
		((BaseItem)val2).Tag = StiExcel2007RestrictEditing.ExceptEditableFields;
		((ComboBox)comboBoxRestrictEditing).Items.Add((object)val2);
		ComboBoxItem val3 = new ComboBoxItem();
		((BaseItem)val3).Text = StiLocalization.Get("Buttons", "Yes").Replace("&", "");
		((BaseItem)val3).Tag = StiExcel2007RestrictEditing.Yes;
		((ComboBox)comboBoxRestrictEditing).Items.Add((object)val3);
	}

	private void SetTypeSelectedIndex(StiExportFormat format)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (object item in ((ComboBox)comboBoxType).Items)
		{
			if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals((int)format))
			{
				break;
			}
			num++;
		}
		((ListControl)comboBoxType).SelectedIndex = ((num < ((ComboBox)comboBoxType).Items.Count) ? num : 0);
	}

	private void CheckState()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		int num = ((Control)panelType).Height + StiScale.YYI(21.0);
		((Control)panelExportSettings).Visible = ((ListControl)comboBoxType).SelectedIndex != 2;
		switch ((StiExportFormat)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxType).SelectedItem).Tag)
		{
		case StiExportFormat.Excel2007:
			exportFormat = StiExportFormat.Excel2007;
			num += ((Control)panelExportSettings).Height;
			num += ((Control)panelExcel2007).Height;
			((Control)panelExportSettings).Visible = !groupBoxExportSettings.Collapsed;
			((Control)panelExcel2007).Visible = !groupBoxExportSettings.Collapsed;
			break;
		case StiExportFormat.Excel:
			exportFormat = StiExportFormat.Excel;
			num += ((Control)panelExportSettings).Height;
			((Control)panelExportSettings).Visible = !groupBoxExportSettings.Collapsed;
			((Control)panelExcel2007).Visible = false;
			break;
		case StiExportFormat.ExcelXml:
			exportFormat = StiExportFormat.ExcelXml;
			((Control)panelExportSettings).Visible = false;
			((Control)panelExcel2007).Visible = false;
			break;
		}
		if (groupBoxExportSettings.Collapsed)
		{
			num = groupBoxExportSettings.HeaderSize;
		}
		((Control)groupBoxExportSettings).Height = num;
		((Control)this).Height = ((Control)pagesRange).Height + num + ((Control)panelOpenAfterExport).Height + ((Control)groupButtons).Height + StiScale.YYI(54.0);
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ba: Unknown result type (might be due to invalid IL or missing references)
		panelOpenAfterExport = new Panel();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		groupButtons = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		groupBoxExportSettings = new StiGroupBox();
		panelExportSettings = new Panel();
		labelDataExportMode = new StiLabelInfo();
		comboBoxDataExportMode = new StiComboBox();
		comboBoxImageQuality = new StiComboBox();
		label1 = new Label();
		labelImageQuality = new StiLabelInfo();
		comboBoxImageResolution = new StiComboBox();
		label6 = new Label();
		labelResolution = new StiLabelInfo();
		checkBoxExportPageBreaks = new StiCheckBoxInfo();
		checkBoxExportEachPageToSheet = new StiCheckBoxInfo();
		checkBoxExportObjectFormatting = new StiCheckBoxInfo();
		checkBoxUseOnePageHeaderAndFooter = new StiCheckBoxInfo();
		panelExcel2007 = new Panel();
		comboBoxRestrictEditing = new StiComboBox();
		labelRestrictEditing = new StiLabelInfo();
		panelType = new Panel();
		labelType = new StiLabelInfo();
		comboBoxType = new StiComboBox();
		pagesRange = new StiPagesRangeControl();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)groupButtons).SuspendLayout();
		((Control)groupBoxExportSettings).SuspendLayout();
		((Control)panelExportSettings).SuspendLayout();
		((Control)panelExcel2007).SuspendLayout();
		((Control)panelType).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 489);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(370, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 0;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)groupButtons).BackColor = Color.White;
		((Control)groupButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupButtons).Dock = (DockStyle)2;
		groupButtons.LinePosition = (StiLinePosition)0;
		groupButtons.LineStyle = (StiLineStyle)0;
		((Control)groupButtons).Location = new Point(0, 531);
		((Control)groupButtons).Margin = new Padding(0, 3, 3, 3);
		((Control)groupButtons).Name = "groupButtons";
		((Control)groupButtons).Size = new Size(370, 46);
		((Control)groupButtons).TabIndex = 3;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(282, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(201, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		groupBoxExportSettings.AllowCollapse = true;
		((Control)groupBoxExportSettings).Anchor = (AnchorStyles)13;
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)panelExportSettings);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)panelExcel2007);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)panelType);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "groupBoxExportSettings";
		groupBoxExportSettings.ResHeight = 122;
		((Control)groupBoxExportSettings).Size = new Size(345, 311);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)panelExportSettings).Controls.Add((Control)(object)labelDataExportMode);
		((Control)panelExportSettings).Controls.Add((Control)(object)comboBoxDataExportMode);
		((Control)panelExportSettings).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)panelExportSettings).Controls.Add((Control)(object)label1);
		((Control)panelExportSettings).Controls.Add((Control)(object)labelImageQuality);
		((Control)panelExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)panelExportSettings).Controls.Add((Control)(object)label6);
		((Control)panelExportSettings).Controls.Add((Control)(object)labelResolution);
		((Control)panelExportSettings).Controls.Add((Control)(object)checkBoxExportPageBreaks);
		((Control)panelExportSettings).Controls.Add((Control)(object)checkBoxExportEachPageToSheet);
		((Control)panelExportSettings).Controls.Add((Control)(object)checkBoxExportObjectFormatting);
		((Control)panelExportSettings).Controls.Add((Control)(object)checkBoxUseOnePageHeaderAndFooter);
		((Control)panelExportSettings).Dock = (DockStyle)1;
		((Control)panelExportSettings).Location = new Point(3, 89);
		((Control)panelExportSettings).Name = "panelExportSettings";
		((Control)panelExportSettings).Size = new Size(339, 216);
		((Control)panelExportSettings).TabIndex = 2;
		((Control)labelDataExportMode).AutoSize = true;
		((Control)labelDataExportMode).ForeColor = Color.DimGray;
		((Control)labelDataExportMode).Location = new Point(12, 71);
		((Control)labelDataExportMode).Name = "labelDataExportMode";
		((Control)labelDataExportMode).Padding = new Padding(0, 0, 12, 0);
		((Control)labelDataExportMode).Size = new Size(71, 13);
		((Control)labelDataExportMode).TabIndex = 19;
		((Control)labelDataExportMode).Text = "Bands filter";
		((Label)labelDataExportMode).TextAlign = (ContentAlignment)16;
		((Control)comboBoxDataExportMode).BackColor = SystemColors.Window;
		comboBoxDataExportMode.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxDataExportMode).DropDownWidth = 150;
		((Control)comboBoxDataExportMode).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxDataExportMode).Items.AddRange(new object[3] { "Data only", "Data and Headers/Footers", "All bands" });
		((Control)comboBoxDataExportMode).Location = new Point(184, 67);
		((Control)comboBoxDataExportMode).Name = "comboBoxDataExportMode";
		((Control)comboBoxDataExportMode).Size = new Size(144, 21);
		((Control)comboBoxDataExportMode).TabIndex = 2;
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageQuality).Items.AddRange(new object[4] { "25", "50", "75", "100" });
		((Control)comboBoxImageQuality).Location = new Point(184, 34);
		((ComboBox)comboBoxImageQuality).MaxDropDownItems = 20;
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		((Control)comboBoxImageQuality).Size = new Size(111, 21);
		((Control)comboBoxImageQuality).TabIndex = 1;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(301, 38);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(15, 13);
		((Control)label1).TabIndex = 16;
		((Control)label1).Text = "%";
		label1.TextAlign = (ContentAlignment)16;
		((Control)labelImageQuality).AutoSize = true;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 38);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageQuality).Size = new Size(83, 13);
		((Control)labelImageQuality).TabIndex = 17;
		((Control)labelImageQuality).Text = "Image Quality";
		((Label)labelImageQuality).TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolution).Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(184, 2);
		((ComboBox)comboBoxImageResolution).MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(111, 21);
		((Control)comboBoxImageResolution).TabIndex = 0;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(301, 2);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(24, 20);
		((Control)label6).TabIndex = 8;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		((Control)labelResolution).AutoSize = true;
		((Control)labelResolution).ForeColor = Color.DimGray;
		((Control)labelResolution).Location = new Point(12, 8);
		((Control)labelResolution).Name = "labelResolution";
		((Control)labelResolution).Padding = new Padding(0, 0, 12, 0);
		((Control)labelResolution).Size = new Size(101, 13);
		((Control)labelResolution).TabIndex = 9;
		((Control)labelResolution).Text = "Image Resolution";
		((Label)labelResolution).TextAlign = (ContentAlignment)16;
		((Control)checkBoxExportPageBreaks).AutoSize = true;
		((Control)checkBoxExportPageBreaks).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportPageBreaks).Location = new Point(15, 189);
		((Control)checkBoxExportPageBreaks).Name = "checkBoxExportPageBreaks";
		((Control)checkBoxExportPageBreaks).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxExportPageBreaks).Size = new Size(132, 17);
		((Control)checkBoxExportPageBreaks).TabIndex = 6;
		((Control)checkBoxExportPageBreaks).Text = "Export Page Breaks";
		((Control)checkBoxExportEachPageToSheet).AutoSize = true;
		((Control)checkBoxExportEachPageToSheet).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportEachPageToSheet).Location = new Point(15, 160);
		((Control)checkBoxExportEachPageToSheet).Name = "checkBoxExportEachPageToSheet";
		((Control)checkBoxExportEachPageToSheet).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxExportEachPageToSheet).Size = new Size(167, 17);
		((Control)checkBoxExportEachPageToSheet).TabIndex = 5;
		((Control)checkBoxExportEachPageToSheet).Text = "Export Each Page to Sheet";
		((Control)checkBoxExportObjectFormatting).AutoSize = true;
		((CheckBox)checkBoxExportObjectFormatting).Checked = true;
		((CheckBox)checkBoxExportObjectFormatting).CheckState = (CheckState)1;
		((Control)checkBoxExportObjectFormatting).Enabled = false;
		((Control)checkBoxExportObjectFormatting).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportObjectFormatting).Location = new Point(15, 102);
		((Control)checkBoxExportObjectFormatting).Name = "checkBoxExportObjectFormatting";
		((Control)checkBoxExportObjectFormatting).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxExportObjectFormatting).Size = new Size(149, 17);
		((Control)checkBoxExportObjectFormatting).TabIndex = 3;
		((Control)checkBoxExportObjectFormatting).Text = "Export object formatting";
		((Control)checkBoxUseOnePageHeaderAndFooter).AutoSize = true;
		((Control)checkBoxUseOnePageHeaderAndFooter).ForeColor = SystemColors.ControlText;
		((Control)checkBoxUseOnePageHeaderAndFooter).Location = new Point(15, 131);
		((Control)checkBoxUseOnePageHeaderAndFooter).Name = "checkBoxUseOnePageHeaderAndFooter";
		((Control)checkBoxUseOnePageHeaderAndFooter).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxUseOnePageHeaderAndFooter).Size = new Size(192, 17);
		((Control)checkBoxUseOnePageHeaderAndFooter).TabIndex = 4;
		((Control)checkBoxUseOnePageHeaderAndFooter).Text = "Use one page header and footer";
		((Control)panelExcel2007).Controls.Add((Control)(object)comboBoxRestrictEditing);
		((Control)panelExcel2007).Controls.Add((Control)(object)labelRestrictEditing);
		((Control)panelExcel2007).Dock = (DockStyle)1;
		((Control)panelExcel2007).Location = new Point(3, 57);
		((Control)panelExcel2007).Name = "panelExcel2007";
		((Control)panelExcel2007).Size = new Size(339, 32);
		((Control)panelExcel2007).TabIndex = 1;
		((Control)comboBoxRestrictEditing).BackColor = SystemColors.Window;
		comboBoxRestrictEditing.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxRestrictEditing).ForeColor = SystemColors.ControlText;
		((Control)comboBoxRestrictEditing).Location = new Point(184, 0);
		((ComboBox)comboBoxRestrictEditing).MaxDropDownItems = 20;
		((Control)comboBoxRestrictEditing).Name = "comboBoxRestrictEditing";
		((Control)comboBoxRestrictEditing).Size = new Size(144, 21);
		((Control)comboBoxRestrictEditing).TabIndex = 28;
		((Control)labelRestrictEditing).AutoSize = true;
		((Control)labelRestrictEditing).ForeColor = Color.DimGray;
		((Control)labelRestrictEditing).Location = new Point(12, 5);
		((Control)labelRestrictEditing).Name = "labelRestrictEditing";
		((Control)labelRestrictEditing).Padding = new Padding(0, 0, 12, 0);
		((Control)labelRestrictEditing).Size = new Size(90, 13);
		((Control)labelRestrictEditing).TabIndex = 29;
		((Control)labelRestrictEditing).Text = "Restrict Editing";
		((Label)labelRestrictEditing).TextAlign = (ContentAlignment)16;
		((Control)panelType).Controls.Add((Control)(object)labelType);
		((Control)panelType).Controls.Add((Control)(object)comboBoxType);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(3, 16);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(339, 41);
		((Control)panelType).TabIndex = 0;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 8);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Padding = new Padding(0, 0, 12, 0);
		((Control)labelType).Size = new Size(164, 21);
		((Control)labelType).TabIndex = 2;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Control)comboBoxType).BackColor = SystemColors.Window;
		comboBoxType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxType).ForeColor = SystemColors.ControlText;
		((Control)comboBoxType).Location = new Point(184, 8);
		((Control)comboBoxType).Name = "comboBoxType";
		((Control)comboBoxType).Size = new Size(144, 21);
		((Control)comboBoxType).TabIndex = 0;
		((Control)pagesRange).Anchor = (AnchorStyles)13;
		((Control)pagesRange).BackColor = Color.White;
		pagesRange.CurrentPage = 0;
		((Control)pagesRange).Location = new Point(12, 8);
		((Control)pagesRange).Name = "pagesRange";
		((Control)pagesRange).Size = new Size(345, 159);
		((Control)pagesRange).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(370, 577);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)groupButtons);
		((Control)this).Controls.Add((Control)(object)pagesRange);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiExcelSetupForm";
		((Control)this).Padding = new Padding(0, 4, 0, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiExcelSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelOpenAfterExport).PerformLayout();
		((Control)groupButtons).ResumeLayout(false);
		((Control)groupBoxExportSettings).ResumeLayout(false);
		((Control)panelExportSettings).ResumeLayout(false);
		((Control)panelExportSettings).PerformLayout();
		((Control)panelExcel2007).ResumeLayout(false);
		((Control)panelExcel2007).PerformLayout();
		((Control)panelType).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
