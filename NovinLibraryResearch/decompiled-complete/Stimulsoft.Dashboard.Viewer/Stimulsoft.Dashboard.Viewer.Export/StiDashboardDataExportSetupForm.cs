using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Viewer.Export;

public class StiDashboardDataExportSetupForm : Form
{
	private IContainer components;

	private StiGroupLine groupLineButtons;

	private Button buttonOk;

	private Button buttonCancel;

	private Panel panelOpenAfterExport;

	private CheckBox checkBoxOpenAfterExport;

	private Panel panelImageResolution;

	private Label labelDataType;

	private StiComboBox comboBoxDataType;

	public StiDashboardDataExportSetupForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			LoadSettings();
		}
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Export", "ExportTypeDataFile").Replace("...", "") + " - " + Loc.Get("Export", "title").Replace("&", "_");
		((Control)labelDataType).Text = Loc.Get("PropertyMain", "DataType");
		((Control)checkBoxOpenAfterExport).Text = Loc.Get("Export", "OpenAfterExport");
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
		((ComboBox)comboBoxDataType).BeginUpdate();
		((ComboBox)comboBoxDataType).Items.Clear();
		if (StiOptions.Viewer.Windows.Exports.ShowCsv)
		{
			((ComboBox)comboBoxDataType).Items.Add((object)"CSV");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowDbf)
		{
			((ComboBox)comboBoxDataType).Items.Add((object)"DBF");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowXml)
		{
			((ComboBox)comboBoxDataType).Items.Add((object)"XML");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowJson)
		{
			((ComboBox)comboBoxDataType).Items.Add((object)"JSON");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowDif)
		{
			((ComboBox)comboBoxDataType).Items.Add((object)"DIF");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowSylk)
		{
			((ComboBox)comboBoxDataType).Items.Add((object)"SYLK");
		}
		((ComboBox)comboBoxDataType).EndUpdate();
	}

	public StiDashboardExportSettings GetExportSettings()
	{
		return new StiDataDashboardExportSettings
		{
			DataType = GetDataType(),
			OpenAfterExport = checkBoxOpenAfterExport.Checked
		};
	}

	private StiDataType GetDataType()
	{
		return ((Control)comboBoxDataType).Text switch
		{
			"CSV" => StiDataType.Csv, 
			"DBF" => StiDataType.Dbf, 
			"XML" => StiDataType.Xml, 
			"JSON" => StiDataType.Json, 
			"DIF" => StiDataType.Dif, 
			"SYLK" => StiDataType.Sylk, 
			_ => throw new StiTypeNotRecognizedException(((Control)comboBoxDataType).Text), 
		};
	}

	private void LoadSettings()
	{
		StiSettings.Load();
		((Control)comboBoxDataType).Text = StiSettings.GetStr(((Control)this).Name, "DataType", "CSV");
		checkBoxOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", nullValue: true);
	}

	private void SaveSettings()
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "DataType", ((Control)comboBoxDataType).Text);
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", checkBoxOpenAfterExport.Checked);
		StiSettings.Save();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpDefaultBrowser("user-manual/index.html?exports_dashboards.htm#exportsettingsofdata");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		SaveSettings();
		((Form)this).DialogResult = (DialogResult)1;
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
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		groupLineButtons = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		panelOpenAfterExport = new Panel();
		checkBoxOpenAfterExport = new CheckBox();
		panelImageResolution = new Panel();
		labelDataType = new Label();
		comboBoxDataType = new StiComboBox();
		((Control)groupLineButtons).SuspendLayout();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)panelImageResolution).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 77);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(344, 46);
		((Control)groupLineButtons).TabIndex = 2;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(175, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(258, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 35);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(344, 42);
		((Control)panelOpenAfterExport).TabIndex = 1;
		checkBoxOpenAfterExport.Checked = true;
		checkBoxOpenAfterExport.CheckState = (CheckState)1;
		((Control)checkBoxOpenAfterExport).ForeColor = SystemColors.ControlText;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(2, 0, 0, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(320, 22);
		((Control)checkBoxOpenAfterExport).TabIndex = 0;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((ButtonBase)checkBoxOpenAfterExport).UseVisualStyleBackColor = true;
		((Control)panelImageResolution).Controls.Add((Control)(object)labelDataType);
		((Control)panelImageResolution).Controls.Add((Control)(object)comboBoxDataType);
		((Control)panelImageResolution).Dock = (DockStyle)2;
		((Control)panelImageResolution).Location = new Point(0, 0);
		((Control)panelImageResolution).Name = "panelImageResolution";
		((Control)panelImageResolution).Size = new Size(344, 35);
		((Control)panelImageResolution).TabIndex = 0;
		((Control)labelDataType).ForeColor = Color.DimGray;
		((Control)labelDataType).Location = new Point(12, 12);
		((Control)labelDataType).Name = "labelDataType";
		((Control)labelDataType).Size = new Size(155, 22);
		((Control)labelDataType).TabIndex = 0;
		((Control)labelDataType).Text = "DataType";
		labelDataType.TextAlign = (ContentAlignment)16;
		((Control)comboBoxDataType).Anchor = (AnchorStyles)9;
		((Control)comboBoxDataType).BackColor = SystemColors.Window;
		comboBoxDataType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxDataType).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxDataType).Items.AddRange(new object[6] { "CSV", "DBF", "XML", "JSON", "DIF", "SYLK" });
		((Control)comboBoxDataType).Location = new Point(175, 12);
		((Control)comboBoxDataType).Name = "comboBoxDataType";
		((Control)comboBoxDataType).Size = new Size(160, 21);
		((Control)comboBoxDataType).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(344, 123);
		((Control)this).Controls.Add((Control)(object)panelImageResolution);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDashboardDataExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiExcelExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelImageResolution).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
