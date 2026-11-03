using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiExcelExportSetupForm : Form
{
	private StiExportFormat exportFormat = StiExportFormat.Excel;

	private IContainer components;

	private StiPagesRangeControl PagesRangeControl;

	private CheckBox cbOpenAfterExport;

	private CheckBox checkBoxUseOnePageHeaderAndFooter;

	private CheckBox checkBoxExportDataOnly;

	private CheckBox checkBoxExportObjectFormatting;

	private CheckBox checkBoxExportEachPageToSheet;

	private CheckBox checkBoxExportPageBreaks;

	private ComboBox comboBoxImageResolution;

	private TrackBar tbImageQuality;

	private Button btCancel;

	private Button btOk;

	private StiGroupLine glButtons;

	private StiGroupBox gbExportSettings;

	private Label lbResolution;

	private StiGroupBox gbImageQuality;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	private Label label6;

	private ComboBox comboBoxRestrictEditing;

	private StiLabelInfo labelRestrictEditing;

	public StiExportFormat ExportFormat
	{
		set
		{
			exportFormat = value;
		}
	}

	public StiPagesRange PagesRange => PagesRangeControl.GetPagesRange();

	public bool OpenAfterExport => cbOpenAfterExport.Checked;

	public int CurrentPage
	{
		get
		{
			return PagesRangeControl.CurrentPage;
		}
		set
		{
			PagesRangeControl.CurrentPage = value;
		}
	}

	public bool OpenAfterExportVisible
	{
		get
		{
			return ((Control)cbOpenAfterExport).Visible;
		}
		set
		{
			((Control)cbOpenAfterExport).Visible = value;
		}
	}

	public bool OpenAfterExportEnabled
	{
		get
		{
			return ((Control)cbOpenAfterExport).Enabled;
		}
		set
		{
			((Control)cbOpenAfterExport).Enabled = value;
		}
	}

	public bool UseOnePageHeaderAndFooter => checkBoxUseOnePageHeaderAndFooter.Checked;

	public bool ExportDataOnly => checkBoxExportDataOnly.Checked;

	public bool ExportObjectFormatting => checkBoxExportObjectFormatting.Checked;

	public bool ExportEachPageToSheet => checkBoxExportEachPageToSheet.Checked;

	public bool ExportPageBreaks => checkBoxExportPageBreaks.Checked;

	public float Resolution => float.Parse(((Control)comboBoxImageResolution).Text.Replace("%", "").Trim());

	public float ImageQuality => (float)tbImageQuality.Value / 100f;

	public StiExcel2007RestrictEditing RestrictEditing => (StiExcel2007RestrictEditing)((BaseItem)(ComboBoxItem)comboBoxRestrictEditing.SelectedItem).Tag;

	public StiExcelExportSetupForm()
	{
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
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
		StiSettings.Load();
		cbOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		checkBoxUseOnePageHeaderAndFooter.Checked = StiSettings.GetBool(((Control)this).Name, "UseOnePageHeaderAndFooter", checkBoxUseOnePageHeaderAndFooter.Checked);
		checkBoxExportDataOnly.Checked = StiSettings.GetBool(((Control)this).Name, "ExportDataOnly", checkBoxExportDataOnly.Checked);
		checkBoxExportObjectFormatting.Checked = StiSettings.GetBool(((Control)this).Name, "ExportObjectFormatting", checkBoxExportObjectFormatting.Checked);
		checkBoxExportEachPageToSheet.Checked = StiSettings.GetBool(((Control)this).Name, "ExportEachPageToSheet", checkBoxExportEachPageToSheet.Checked);
		checkBoxExportPageBreaks.Checked = StiSettings.GetBool(((Control)this).Name, "ExportPageBreaks", checkBoxExportPageBreaks.Checked);
		gbExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		((Control)comboBoxImageResolution).Handle.ToString();
		((ListControl)comboBoxImageResolution).SelectedIndex = 4;
		comboBoxImageResolution.SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageResolution", comboBoxImageResolution.SelectedItem.ToString());
		((Control)tbImageQuality).Handle.ToString();
		tbImageQuality.Value = StiSettings.GetInt(((Control)this).Name, "ImageQuality", tbImageQuality.Value);
		int @int = StiSettings.GetInt(((Control)this).Name, "RestrictEditing", 1);
		int num = 0;
		foreach (object item in comboBoxRestrictEditing.Items)
		{
			if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals(@int))
			{
				break;
			}
			num++;
		}
		((ListControl)comboBoxRestrictEditing).SelectedIndex = ((num < comboBoxRestrictEditing.Items.Count) ? num : 0);
		Localize();
		Build();
	}

	private void Localize()
	{
		((Control)cbOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)checkBoxUseOnePageHeaderAndFooter).Text = StiLocalization.Get("Export", "UseOnePageHeaderAndFooter");
		((Control)checkBoxExportDataOnly).Text = StiLocalization.Get("Export", "ExportDataOnly");
		((Control)checkBoxExportObjectFormatting).Text = StiLocalization.Get("Export", "ExportObjectFormatting");
		((Control)checkBoxExportEachPageToSheet).Text = StiLocalization.Get("Export", "ExportEachPageToSheet");
		((Control)checkBoxExportPageBreaks).Text = StiLocalization.Get("Export", "ExportPageBreaks");
		((Control)labelRestrictEditing).Text = StiLocalization.Get("Export", "RestrictEditing");
		((Control)gbExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)gbImageQuality).Text = StiLocalization.Get("Export", "ImageQuality");
		((Control)lbResolution).Text = StiLocalization.Get("Export", "ImageResolution");
		((Control)this).Text = StiLocalization.Get("Export", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
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
		comboBoxRestrictEditing.Items.Add((object)val);
		ComboBoxItem val2 = new ComboBoxItem();
		((BaseItem)val2).Text = StiLocalization.Get("Export", "ExceptEditableFields");
		((BaseItem)val2).Tag = StiExcel2007RestrictEditing.ExceptEditableFields;
		comboBoxRestrictEditing.Items.Add((object)val2);
		ComboBoxItem val3 = new ComboBoxItem();
		((BaseItem)val3).Text = StiLocalization.Get("Buttons", "Yes").Replace("&", "");
		((BaseItem)val3).Tag = StiExcel2007RestrictEditing.Yes;
		comboBoxRestrictEditing.Items.Add((object)val3);
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		StiSettings.Set(((Control)this).Name, "ExportDataOnly", checkBoxExportDataOnly.Checked);
		StiSettings.Set(((Control)this).Name, "ExportObjectFormatting", checkBoxExportObjectFormatting.Checked);
		StiSettings.Set(((Control)this).Name, "UseOnePageHeaderAndFooter", checkBoxUseOnePageHeaderAndFooter.Checked);
		StiSettings.Set(((Control)this).Name, "ExportEachPageToSheet", checkBoxExportEachPageToSheet.Checked);
		StiSettings.Set(((Control)this).Name, "ExportPageBreaks", checkBoxExportPageBreaks.Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "ImageResolution", comboBoxImageResolution.SelectedItem.ToString());
		StiSettings.Set(((Control)this).Name, "ImageQuality", tbImageQuality.Value);
		StiSettings.Set(((Control)this).Name, "RestrictEditing", (int)((BaseItem)(ComboBoxItem)comboBoxRestrictEditing.SelectedItem).Tag);
		StiSettings.Save();
	}

	private void checkboxExportDataOnly_CheckedChanged(object sender, EventArgs e)
	{
		if (checkBoxExportDataOnly.Checked)
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

	private void ThisKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		string url = ((exportFormat == StiExportFormat.Excel) ? "user-manual/exports_spreadsheets_excel.htm" : "user-manual/exports_spreadsheets_excel2007_2010.htm");
		StiHelpProvider.ShowHelpViewer(url);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efa: Expected O, but got Unknown
		btCancel = new Button();
		btOk = new Button();
		PagesRangeControl = new StiPagesRangeControl();
		cbOpenAfterExport = new CheckBox();
		glButtons = new StiGroupLine();
		checkBoxUseOnePageHeaderAndFooter = new CheckBox();
		checkBoxExportDataOnly = new CheckBox();
		checkBoxExportObjectFormatting = new CheckBox();
		gbExportSettings = new StiGroupBox();
		comboBoxRestrictEditing = new ComboBox();
		labelRestrictEditing = new StiLabelInfo();
		comboBoxImageResolution = new ComboBox();
		label6 = new Label();
		lbResolution = new Label();
		gbImageQuality = new StiGroupBox();
		label5 = new Label();
		label4 = new Label();
		label3 = new Label();
		label2 = new Label();
		label1 = new Label();
		tbImageQuality = new TrackBar();
		checkBoxExportPageBreaks = new CheckBox();
		checkBoxExportEachPageToSheet = new CheckBox();
		((Control)glButtons).SuspendLayout();
		((Control)gbExportSettings).SuspendLayout();
		((Control)gbImageQuality).SuspendLayout();
		((ISupportInitialize)tbImageQuality).BeginInit();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(230, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(150, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(4, 4);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(304, 136);
		((Control)PagesRangeControl).TabIndex = 0;
		((Control)cbOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)cbOpenAfterExport).AutoSize = true;
		((Control)cbOpenAfterExport).Location = new Point(12, 440);
		((Control)cbOpenAfterExport).Name = "cbOpenAfterExport";
		((Control)cbOpenAfterExport).Size = new Size(104, 17);
		((Control)cbOpenAfterExport).TabIndex = 2;
		((Control)cbOpenAfterExport).Text = "OpenAfterExport";
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 464);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(312, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)checkBoxUseOnePageHeaderAndFooter).AutoSize = true;
		((Control)checkBoxUseOnePageHeaderAndFooter).ForeColor = SystemColors.ControlText;
		((Control)checkBoxUseOnePageHeaderAndFooter).Location = new Point(8, 214);
		((Control)checkBoxUseOnePageHeaderAndFooter).Name = "checkBoxUseOnePageHeaderAndFooter";
		((Control)checkBoxUseOnePageHeaderAndFooter).Size = new Size(180, 17);
		((Control)checkBoxUseOnePageHeaderAndFooter).TabIndex = 5;
		((Control)checkBoxUseOnePageHeaderAndFooter).Text = "Use one page header and footer";
		((Control)checkBoxExportDataOnly).AutoSize = true;
		((Control)checkBoxExportDataOnly).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportDataOnly).Location = new Point(8, 166);
		((Control)checkBoxExportDataOnly).Name = "checkBoxExportDataOnly";
		((Control)checkBoxExportDataOnly).Size = new Size(102, 17);
		((Control)checkBoxExportDataOnly).TabIndex = 3;
		((Control)checkBoxExportDataOnly).Text = "Export data only";
		checkBoxExportDataOnly.CheckedChanged += checkboxExportDataOnly_CheckedChanged;
		((Control)checkBoxExportObjectFormatting).AutoSize = true;
		checkBoxExportObjectFormatting.Checked = true;
		checkBoxExportObjectFormatting.CheckState = (CheckState)1;
		((Control)checkBoxExportObjectFormatting).Enabled = false;
		((Control)checkBoxExportObjectFormatting).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportObjectFormatting).Location = new Point(8, 190);
		((Control)checkBoxExportObjectFormatting).Name = "checkBoxExportObjectFormatting";
		((Control)checkBoxExportObjectFormatting).Size = new Size(137, 17);
		((Control)checkBoxExportObjectFormatting).TabIndex = 4;
		((Control)checkBoxExportObjectFormatting).Text = "Export object formatting";
		gbExportSettings.AllowCollapse = true;
		((Control)gbExportSettings).Controls.Add((Control)(object)comboBoxRestrictEditing);
		((Control)gbExportSettings).Controls.Add((Control)(object)labelRestrictEditing);
		((Control)gbExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)gbExportSettings).Controls.Add((Control)(object)label6);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbResolution);
		((Control)gbExportSettings).Controls.Add((Control)(object)gbImageQuality);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxExportPageBreaks);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxExportEachPageToSheet);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxExportObjectFormatting);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxExportDataOnly);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxUseOnePageHeaderAndFooter);
		((Control)gbExportSettings).Location = new Point(4, 144);
		((Control)gbExportSettings).Name = "gbExportSettings";
		gbExportSettings.ResHeight = 264;
		((Control)gbExportSettings).Size = new Size(304, 287);
		((Control)gbExportSettings).TabIndex = 1;
		((GroupBox)gbExportSettings).TabStop = false;
		((Control)gbExportSettings).Text = "ExportSettings";
		((Control)comboBoxRestrictEditing).BackColor = SystemColors.Window;
		comboBoxRestrictEditing.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxRestrictEditing).ForeColor = SystemColors.ControlText;
		((Control)comboBoxRestrictEditing).Location = new Point(155, 53);
		comboBoxRestrictEditing.MaxDropDownItems = 20;
		((Control)comboBoxRestrictEditing).Name = "comboBoxRestrictEditing";
		((Control)comboBoxRestrictEditing).Size = new Size(133, 21);
		((Control)comboBoxRestrictEditing).TabIndex = 1;
		((Control)labelRestrictEditing).AutoSize = true;
		((Control)labelRestrictEditing).ForeColor = Color.DimGray;
		((Control)labelRestrictEditing).Location = new Point(5, 56);
		((Control)labelRestrictEditing).Name = "labelRestrictEditing";
		((Control)labelRestrictEditing).Padding = new Padding(0, 0, 12, 0);
		((Control)labelRestrictEditing).Size = new Size(90, 13);
		((Control)labelRestrictEditing).TabIndex = 27;
		((Control)labelRestrictEditing).Text = "Restrict Editing";
		((Label)labelRestrictEditing).TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		comboBoxImageResolution.Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(155, 24);
		comboBoxImageResolution.MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(80, 21);
		((Control)comboBoxImageResolution).TabIndex = 0;
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(241, 28);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(21, 13);
		((Control)label6).TabIndex = 0;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		((Control)lbResolution).AutoSize = true;
		((Control)lbResolution).ForeColor = Color.DimGray;
		((Control)lbResolution).Location = new Point(5, 28);
		((Control)lbResolution).Name = "lbResolution";
		((Control)lbResolution).Size = new Size(89, 13);
		((Control)lbResolution).TabIndex = 0;
		((Control)lbResolution).Text = "Image Resolution";
		lbResolution.TextAlign = (ContentAlignment)16;
		((Control)gbImageQuality).Anchor = (AnchorStyles)13;
		((Control)gbImageQuality).Controls.Add((Control)(object)label5);
		((Control)gbImageQuality).Controls.Add((Control)(object)label4);
		((Control)gbImageQuality).Controls.Add((Control)(object)label3);
		((Control)gbImageQuality).Controls.Add((Control)(object)label2);
		((Control)gbImageQuality).Controls.Add((Control)(object)label1);
		((Control)gbImageQuality).Controls.Add((Control)(object)tbImageQuality);
		((Control)gbImageQuality).Location = new Point(8, 78);
		((Control)gbImageQuality).Name = "gbImageQuality";
		gbImageQuality.ResHeight = 0;
		((Control)gbImageQuality).Size = new Size(288, 80);
		((Control)gbImageQuality).TabIndex = 2;
		((GroupBox)gbImageQuality).TabStop = false;
		((Control)gbImageQuality).Text = "Image Quality";
		((Control)label5).ForeColor = SystemColors.ControlText;
		((Control)label5).Location = new Point(72, 48);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(32, 16);
		((Control)label5).TabIndex = 2;
		((Control)label5).Text = "25%";
		((Control)label4).ForeColor = SystemColors.ControlText;
		((Control)label4).Location = new Point(184, 48);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(32, 16);
		((Control)label4).TabIndex = 4;
		((Control)label4).Text = "75%";
		((Control)label3).ForeColor = SystemColors.ControlText;
		((Control)label3).Location = new Point(128, 48);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(32, 16);
		((Control)label3).TabIndex = 3;
		((Control)label3).Text = "50%";
		((Control)label2).ForeColor = SystemColors.ControlText;
		((Control)label2).Location = new Point(240, 48);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(34, 16);
		((Control)label2).TabIndex = 5;
		((Control)label2).Text = "100%";
		((Control)label1).ForeColor = SystemColors.ControlText;
		((Control)label1).Location = new Point(16, 48);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(32, 16);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "0%";
		tbImageQuality.LargeChange = 10;
		((Control)tbImageQuality).Location = new Point(8, 16);
		tbImageQuality.Maximum = 100;
		((Control)tbImageQuality).Name = "tbImageQuality";
		((Control)tbImageQuality).Size = new Size(274, 45);
		((Control)tbImageQuality).TabIndex = 0;
		tbImageQuality.TickFrequency = 5;
		tbImageQuality.Value = 75;
		((Control)checkBoxExportPageBreaks).AutoSize = true;
		((Control)checkBoxExportPageBreaks).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportPageBreaks).Location = new Point(8, 262);
		((Control)checkBoxExportPageBreaks).Name = "checkBoxExportPageBreaks";
		((Control)checkBoxExportPageBreaks).Size = new Size(120, 17);
		((Control)checkBoxExportPageBreaks).TabIndex = 7;
		((Control)checkBoxExportPageBreaks).Text = "Export Page Breaks";
		((Control)checkBoxExportEachPageToSheet).AutoSize = true;
		((Control)checkBoxExportEachPageToSheet).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportEachPageToSheet).Location = new Point(8, 238);
		((Control)checkBoxExportEachPageToSheet).Name = "checkBoxExportEachPageToSheet";
		((Control)checkBoxExportEachPageToSheet).Size = new Size(155, 17);
		((Control)checkBoxExportEachPageToSheet).TabIndex = 6;
		((Control)checkBoxExportEachPageToSheet).Text = "Export Each Page to Sheet";
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(312, 510);
		((Control)this).Controls.Add((Control)(object)gbExportSettings);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Control)this).Controls.Add((Control)(object)cbOpenAfterExport);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiExcelExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "ExportSetupForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += ThisClosed;
		((Control)this).KeyDown += new KeyEventHandler(ThisKeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)gbExportSettings).ResumeLayout(false);
		((Control)gbExportSettings).PerformLayout();
		((Control)gbImageQuality).ResumeLayout(false);
		((Control)gbImageQuality).PerformLayout();
		((ISupportInitialize)tbImageQuality).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
