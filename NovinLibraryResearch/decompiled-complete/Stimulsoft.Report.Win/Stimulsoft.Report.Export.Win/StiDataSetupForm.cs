using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiDataSetupForm : Form
{
	private IContainer components;

	private StiGroupLine groupButtons;

	private Button buttonCancel;

	private Button buttonOk;

	private StiPagesRangeControl pagesRange;

	private StiGroupBox groupBoxExportSettings;

	private StiDifSylkExportSetupControl difSylkExportSetupControl;

	private StiDbfExportSetupControl dbfExportSetupControl;

	private StiCsvExportSetupControl csvExportSetupControl;

	private StiXmlExportSetupControl xmlExportSetupControl;

	private Panel panelType;

	private StiLabelInfo labelType;

	private StiComboBox comboBoxType;

	private Panel panelOpenAfterExport;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	public StiExportFormat ExportFormat { get; set; } = StiExportFormat.Excel;


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

	public string Separator => csvExportSetupControl.Separator;

	public bool SkipColumnHeaders => csvExportSetupControl.SkipColumnHeaders;

	public int DataExportMode
	{
		get
		{
			int result = 0;
			if (ExportFormat == StiExportFormat.Csv)
			{
				result = csvExportSetupControl.DataExportMode;
			}
			else if (ExportFormat == StiExportFormat.Xml || ExportFormat == StiExportFormat.Json)
			{
				result = xmlExportSetupControl.DataExportMode;
			}
			return result;
		}
	}

	public Encoding Encoding
	{
		get
		{
			Encoding result = null;
			if (ExportFormat == StiExportFormat.Csv)
			{
				result = csvExportSetupControl.Encoding;
			}
			else if (ExportFormat == StiExportFormat.Dbf)
			{
				result = dbfExportSetupControl.Encoding;
			}
			else if (ExportFormat == StiExportFormat.Dif)
			{
				result = difSylkExportSetupControl.Encoding;
			}
			else if (ExportFormat == StiExportFormat.Sylk)
			{
				result = difSylkExportSetupControl.Encoding;
			}
			return result;
		}
	}

	public int EncodingSelectedIndex => dbfExportSetupControl.EncodingSelectedIndex;

	public bool ExportDataOnly => difSylkExportSetupControl.ExportDataOnly;

	public bool UseDefaultSystemEncoding => difSylkExportSetupControl.UseDefaultSystemEncoding;

	public StiDataSetupForm()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
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
		if (StiOptions.Viewer.Windows.Exports.ShowCsv)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Csv",
				Tag = StiExportFormat.Csv
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowDbf)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Dbf",
				Tag = StiExportFormat.Dbf
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowXml)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Xml",
				Tag = StiExportFormat.Xml
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowDif)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Dif",
				Tag = StiExportFormat.Dif
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowSylk)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Sylk",
				Tag = StiExportFormat.Sylk
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowJson)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "Json",
				Tag = StiExportFormat.Json
			});
		}
		((Form)this).Closed += This_Closed;
		groupBoxExportSettings.CollapsedChange += GroupBoxExportSettings_CollapsedChange;
		((ComboBox)comboBoxType).SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
		((ListControl)comboBoxType).SelectedIndex = 0;
		StiSettings.Load();
		((CheckBox)checkBoxOpenAfterExport).Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		((Control)csvExportSetupControl.textBoxSeparator).Text = StiSettings.GetStr(((Control)this).Name, "Separator", ((Control)csvExportSetupControl.textBoxSeparator).Text);
		((CheckBox)csvExportSetupControl.checkBoxSkipColumnHeaders).Checked = StiSettings.GetBool(((Control)this).Name, "SkipColumnHeaders", ((CheckBox)csvExportSetupControl.checkBoxSkipColumnHeaders).Checked);
		((ListControl)csvExportSetupControl.comboBoxDataExportMode).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "DataExportMode", 0);
		int @int = StiSettings.GetInt(((Control)this).Name, "Encoding", 0);
		if (@int < ((ComboBox)csvExportSetupControl.comboBoxEncoding).Items.Count)
		{
			((ListControl)csvExportSetupControl.comboBoxEncoding).SelectedIndex = @int;
		}
		if (@int < ((ComboBox)dbfExportSetupControl.comboBoxEncoding).Items.Count)
		{
			((ListControl)dbfExportSetupControl.comboBoxEncoding).SelectedIndex = @int;
		}
		if (@int < difSylkExportSetupControl.comboBoxEncoding.Items.Count)
		{
			difSylkExportSetupControl.comboBoxEncoding.SelectedIndex = @int;
		}
		((CheckBox)difSylkExportSetupControl.checkBoxExportDataOnly).Checked = StiSettings.GetBool(((Control)this).Name, "ExportDataOnly", ((CheckBox)difSylkExportSetupControl.checkBoxExportDataOnly).Checked);
		((CheckBox)difSylkExportSetupControl.checkBoxUseDefaultSystemEncoding).Checked = StiSettings.GetBool(((Control)this).Name, "UseDefaultSystemEncoding", ((CheckBox)difSylkExportSetupControl.checkBoxUseDefaultSystemEncoding).Checked);
		groupBoxExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings");
		int int2 = StiSettings.GetInt(((Control)this).Name, "Type", 17);
		int num = 0;
		foreach (object item in ((ComboBox)comboBoxType).Items)
		{
			if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals(int2))
			{
				break;
			}
			num++;
		}
		((ListControl)comboBoxType).SelectedIndex = ((num < ((ComboBox)comboBoxType).Items.Count) ? num : 0);
		LocalizeSuperToolTip();
		((Control)xmlExportSetupControl.comboBoxDataExportMode).Width = ((Control)comboBoxType).Width;
		((Control)dbfExportSetupControl.comboBoxEncoding).Width = ((Control)comboBoxType).Width;
		((Control)difSylkExportSetupControl.comboBoxEncoding).Width = ((Control)comboBoxType).Width;
		((Control)csvExportSetupControl.comboBoxDataExportMode).Width = ((Control)comboBoxType).Width;
		((Control)csvExportSetupControl.comboBoxEncoding).Width = ((Control)comboBoxType).Width;
		((Control)csvExportSetupControl.textBoxSeparator).Width = ((Control)comboBoxType).Width / 2;
		((ComboBox)comboBoxType).DropDownWidth = StiScale.XXI((double)((ComboBox)comboBoxType).DropDownWidth);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_data.htm");
	}

	private void This_Closed(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		StiSettings.Set(((Control)this).Name, "Type", (int)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxType).SelectedItem).Tag);
		StiSettings.Set(((Control)this).Name, "Separator", ((Control)csvExportSetupControl.textBoxSeparator).Text);
		if (ExportFormat == StiExportFormat.Csv)
		{
			StiSettings.Set(((Control)this).Name, "Encoding", ((ListControl)csvExportSetupControl.comboBoxEncoding).SelectedIndex);
		}
		else if (ExportFormat == StiExportFormat.Dbf)
		{
			StiSettings.Set(((Control)this).Name, "Encoding", ((ListControl)dbfExportSetupControl.comboBoxEncoding).SelectedIndex);
		}
		else if (ExportFormat == StiExportFormat.Dif || ExportFormat == StiExportFormat.Sylk)
		{
			StiSettings.Set(((Control)this).Name, "Encoding", difSylkExportSetupControl.comboBoxEncoding.SelectedIndex);
		}
		StiSettings.Set(((Control)this).Name, "SkipColumnHeaders", ((CheckBox)csvExportSetupControl.checkBoxSkipColumnHeaders).Checked);
		StiSettings.Set(((Control)this).Name, "DataExportMode", ((ListControl)csvExportSetupControl.comboBoxDataExportMode).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "ExportDataOnly", ((CheckBox)difSylkExportSetupControl.checkBoxExportDataOnly).Checked);
		StiSettings.Set(((Control)this).Name, "UseDefaultSystemEncoding", ((CheckBox)difSylkExportSetupControl.checkBoxUseDefaultSystemEncoding).Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		StiSettings.Save();
	}

	private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckState();
	}

	private void GroupBoxExportSettings_CollapsedChange(object sender, EventArgs e)
	{
		if (!groupBoxExportSettings.Collapsed)
		{
			CheckState();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Export", "ExportTypeDataFile").Replace("...", "") + " - " + StiLocalization.Get("Export", "title").Replace("&", "_");
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings").Replace("&", "_");
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport").Replace("&", "_");
		((Control)labelType).Text = Loc.GetCleaned("Export", "Type");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		labelType.SetToolTip(StiLocalization.Get("HelpViewer", "TypeExport"));
		checkBoxOpenAfterExport.SetToolTip(StiLocalization.Get("HelpViewer", "OpenAfterExport"));
	}

	private void CheckState()
	{
		int num = ((Control)panelType).Height + StiScale.YYI(21.0);
		switch (((ListControl)comboBoxType).SelectedIndex)
		{
		case 0:
			ExportFormat = StiExportFormat.Csv;
			((Control)csvExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			((Control)dbfExportSetupControl).Visible = false;
			((Control)difSylkExportSetupControl).Visible = false;
			((Control)xmlExportSetupControl).Visible = false;
			num += ((Control)csvExportSetupControl).Height;
			break;
		case 1:
			ExportFormat = StiExportFormat.Dbf;
			((Control)csvExportSetupControl).Visible = false;
			((Control)dbfExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			((Control)difSylkExportSetupControl).Visible = false;
			((Control)xmlExportSetupControl).Visible = false;
			num += ((Control)dbfExportSetupControl).Height;
			break;
		case 2:
			ExportFormat = StiExportFormat.Xml;
			((Control)csvExportSetupControl).Visible = false;
			((Control)dbfExportSetupControl).Visible = false;
			((Control)difSylkExportSetupControl).Visible = false;
			((Control)xmlExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			num += ((Control)xmlExportSetupControl).Height;
			break;
		case 3:
			ExportFormat = StiExportFormat.Dif;
			((Control)csvExportSetupControl).Visible = false;
			((Control)dbfExportSetupControl).Visible = false;
			((Control)difSylkExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			((Control)xmlExportSetupControl).Visible = false;
			num += ((Control)difSylkExportSetupControl).Height;
			break;
		case 4:
			ExportFormat = StiExportFormat.Sylk;
			((Control)csvExportSetupControl).Visible = false;
			((Control)dbfExportSetupControl).Visible = false;
			((Control)difSylkExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			((Control)xmlExportSetupControl).Visible = false;
			num += ((Control)difSylkExportSetupControl).Height;
			break;
		case 5:
			ExportFormat = StiExportFormat.Json;
			((Control)csvExportSetupControl).Visible = false;
			((Control)dbfExportSetupControl).Visible = false;
			((Control)difSylkExportSetupControl).Visible = false;
			((Control)xmlExportSetupControl).Visible = !groupBoxExportSettings.Collapsed;
			num += ((Control)xmlExportSetupControl).Height;
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		groupButtons = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		groupBoxExportSettings = new StiGroupBox();
		panelType = new Panel();
		comboBoxType = new StiComboBox();
		panelOpenAfterExport = new Panel();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		difSylkExportSetupControl = new StiDifSylkExportSetupControl();
		dbfExportSetupControl = new StiDbfExportSetupControl();
		csvExportSetupControl = new StiCsvExportSetupControl();
		xmlExportSetupControl = new StiXmlExportSetupControl();
		labelType = new StiLabelInfo();
		pagesRange = new StiPagesRangeControl();
		((Control)groupButtons).SuspendLayout();
		((Control)groupBoxExportSettings).SuspendLayout();
		((Control)panelType).SuspendLayout();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupButtons).BackColor = Color.White;
		((Control)groupButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupButtons).Dock = (DockStyle)2;
		groupButtons.LinePosition = (StiLinePosition)0;
		groupButtons.LineStyle = (StiLineStyle)0;
		((Control)groupButtons).Location = new Point(0, 579);
		((Control)groupButtons).Margin = new Padding(0, 3, 3, 3);
		((Control)groupButtons).Name = "glButtons";
		((Control)groupButtons).Size = new Size(370, 46);
		((Control)groupButtons).TabIndex = 3;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(282, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(201, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		groupBoxExportSettings.AllowCollapse = true;
		((Control)groupBoxExportSettings).Anchor = (AnchorStyles)13;
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)difSylkExportSetupControl);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)dbfExportSetupControl);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)csvExportSetupControl);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)xmlExportSetupControl);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)panelType);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "groupBoxExportSettings";
		groupBoxExportSettings.ResHeight = 122;
		((Control)groupBoxExportSettings).Size = new Size(345, 349);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)panelType).Controls.Add((Control)(object)labelType);
		((Control)panelType).Controls.Add((Control)(object)comboBoxType);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(3, 16);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(339, 41);
		((Control)panelType).TabIndex = 0;
		((Control)comboBoxType).Anchor = (AnchorStyles)13;
		((Control)comboBoxType).BackColor = SystemColors.Window;
		comboBoxType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxType).ForeColor = SystemColors.ControlText;
		((Control)comboBoxType).Location = new Point(184, 8);
		((Control)comboBoxType).Name = "comboBoxType";
		((Control)comboBoxType).Size = new Size(144, 21);
		((Control)comboBoxType).TabIndex = 0;
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 537);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(370, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 7;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)difSylkExportSetupControl).BackColor = Color.White;
		((Control)difSylkExportSetupControl).Dock = (DockStyle)1;
		((Control)difSylkExportSetupControl).Location = new Point(3, 253);
		((Control)difSylkExportSetupControl).Margin = new Padding(6);
		((Control)difSylkExportSetupControl).Name = "difSylkExportSetupControl";
		((Control)difSylkExportSetupControl).Padding = new Padding(0, 0, 0, 10);
		((Control)difSylkExportSetupControl).Size = new Size(339, 94);
		((Control)difSylkExportSetupControl).TabIndex = 3;
		((Control)dbfExportSetupControl).BackColor = Color.White;
		((Control)dbfExportSetupControl).Dock = (DockStyle)1;
		((Control)dbfExportSetupControl).Location = new Point(3, 219);
		((Control)dbfExportSetupControl).Margin = new Padding(6);
		((Control)dbfExportSetupControl).Name = "dbfExportSetupControl";
		((Control)dbfExportSetupControl).Size = new Size(339, 34);
		((Control)dbfExportSetupControl).TabIndex = 2;
		((Control)csvExportSetupControl).BackColor = Color.White;
		((Control)csvExportSetupControl).Dock = (DockStyle)1;
		((Control)csvExportSetupControl).Location = new Point(3, 91);
		((Control)csvExportSetupControl).Margin = new Padding(6);
		((Control)csvExportSetupControl).Name = "csvExportSetupControl";
		((Control)csvExportSetupControl).Size = new Size(339, 128);
		((Control)csvExportSetupControl).TabIndex = 1;
		((Control)xmlExportSetupControl).BackColor = Color.White;
		((Control)xmlExportSetupControl).Dock = (DockStyle)1;
		((Control)xmlExportSetupControl).Location = new Point(3, 57);
		((Control)xmlExportSetupControl).Margin = new Padding(6);
		((Control)xmlExportSetupControl).Name = "xmlExportSetupControl";
		((Control)xmlExportSetupControl).Size = new Size(339, 34);
		((Control)xmlExportSetupControl).TabIndex = 1;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 8);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Padding = new Padding(0, 0, 12, 0);
		((Control)labelType).Size = new Size(166, 21);
		((Control)labelType).TabIndex = 2;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
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
		((Form)this).ClientSize = new Size(370, 625);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Control)this).Controls.Add((Control)(object)pagesRange);
		((Control)this).Controls.Add((Control)(object)groupButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataSetupForm";
		((Control)this).Padding = new Padding(0, 4, 0, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiDataSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)groupButtons).ResumeLayout(false);
		((Control)groupBoxExportSettings).ResumeLayout(false);
		((Control)panelType).ResumeLayout(false);
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelOpenAfterExport).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
