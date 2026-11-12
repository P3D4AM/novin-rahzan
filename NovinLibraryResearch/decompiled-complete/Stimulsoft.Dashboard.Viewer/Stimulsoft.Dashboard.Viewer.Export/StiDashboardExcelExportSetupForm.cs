using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Viewer.Export;

public class StiDashboardExcelExportSetupForm : Form
{
	private IContainer components;

	private StiGroupLine groupLineButtons;

	private Button buttonOk;

	private Button buttonCancel;

	private Panel panelOpenAfterExport;

	private CheckBox checkBoxOpenAfterExport;

	private CheckBox checkBoxExportDataOnly;

	private Panel panelExportDataOnly;

	private Panel panelImageResolution;

	private Label labelImageQuality;

	private StiDropDownBox comboBoxImageQuality;

	public StiDashboardExcelExportSetupForm()
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

	public StiDashboardExcelExportSetupForm(IStiElement element)
		: this()
	{
		UpdateState(element);
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Export", "ExportTypeExcelFile").Replace("...", "") + " - " + Loc.Get("Export", "title").Replace("&", "_");
		((Control)labelImageQuality).Text = Loc.Get("Export", "ImageQuality").Replace(":", "");
		((Control)checkBoxExportDataOnly).Text = Loc.Get("Export", "ExportDataOnly");
		((Control)checkBoxOpenAfterExport).Text = Loc.Get("Export", "OpenAfterExport");
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
	}

	private void UpdateState(IStiElement element)
	{
		((Control)panelExportDataOnly).Enabled = element is IStiTableElement || element is IStiPivotTableElement;
	}

	public StiDashboardExportSettings GetExportSettings()
	{
		int.TryParse(((Control)comboBoxImageQuality).Text.Replace("%", ""), out var result);
		return new StiExcelDashboardExportSettings
		{
			ExportDataOnly = (((Control)panelExportDataOnly).Enabled && checkBoxExportDataOnly.Checked),
			OpenAfterExport = checkBoxOpenAfterExport.Checked,
			ImageQuality = result
		};
	}

	private void LoadSettings()
	{
		StiSettings.Load();
		((Control)comboBoxImageQuality).Text = StiSettings.GetStr(((Control)this).Name, "ImageQuality", "200%");
		checkBoxExportDataOnly.Checked = StiSettings.GetBool(((Control)this).Name, "ExportDataOnly", nullValue: false);
		checkBoxOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", nullValue: true);
	}

	private void SaveSettings()
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "ExportDataOnly", checkBoxExportDataOnly.Checked);
		StiSettings.Set(((Control)this).Name, "ImageQuality", ((Control)comboBoxImageQuality).Text.EndsWith("%") ? ((Control)comboBoxImageQuality).Text : (((Control)comboBoxImageQuality).Text + "%"));
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", checkBoxOpenAfterExport.Checked);
		StiSettings.Save();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpDefaultBrowser("user-manual/index.html?exports_dashboards.htm#excelexportsettings");
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

	private void This_Shown(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		((Control)this).Height = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val.Enabled)
			{
				((Control)this).Height = ((Control)this).Height + val.Height;
			}
			else
			{
				val.Visible = false;
			}
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Expected O, but got Unknown
		groupLineButtons = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		panelOpenAfterExport = new Panel();
		checkBoxOpenAfterExport = new CheckBox();
		checkBoxExportDataOnly = new CheckBox();
		panelExportDataOnly = new Panel();
		panelImageResolution = new Panel();
		labelImageQuality = new Label();
		comboBoxImageQuality = new StiDropDownBox();
		((Control)groupLineButtons).SuspendLayout();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)panelExportDataOnly).SuspendLayout();
		((Control)panelImageResolution).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 112);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(344, 46);
		((Control)groupLineButtons).TabIndex = 3;
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
		((Control)panelOpenAfterExport).Location = new Point(0, 70);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(344, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
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
		((Control)checkBoxExportDataOnly).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportDataOnly).Location = new Point(12, 12);
		((Control)checkBoxExportDataOnly).Name = "checkBoxExportDataOnly";
		((Control)checkBoxExportDataOnly).Padding = new Padding(2, 0, 0, 0);
		((Control)checkBoxExportDataOnly).Size = new Size(320, 22);
		((Control)checkBoxExportDataOnly).TabIndex = 0;
		((Control)checkBoxExportDataOnly).Text = "ExportDataOnly";
		((Control)panelExportDataOnly).Controls.Add((Control)(object)checkBoxExportDataOnly);
		((Control)panelExportDataOnly).Dock = (DockStyle)2;
		((Control)panelExportDataOnly).Location = new Point(0, 35);
		((Control)panelExportDataOnly).Name = "panelExportDataOnly";
		((Control)panelExportDataOnly).Size = new Size(344, 35);
		((Control)panelExportDataOnly).TabIndex = 1;
		((Control)panelImageResolution).Controls.Add((Control)(object)labelImageQuality);
		((Control)panelImageResolution).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)panelImageResolution).Dock = (DockStyle)2;
		((Control)panelImageResolution).Location = new Point(0, 0);
		((Control)panelImageResolution).Name = "panelImageResolution";
		((Control)panelImageResolution).Size = new Size(344, 35);
		((Control)panelImageResolution).TabIndex = 0;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 12);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Size = new Size(157, 22);
		((Control)labelImageQuality).TabIndex = 0;
		((Control)labelImageQuality).Text = "ImageQuality";
		labelImageQuality.TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageQuality).Anchor = (AnchorStyles)9;
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DrawMode = (DrawMode)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		comboBoxImageQuality.IsEditable = true;
		comboBoxImageQuality.Items.AddRange(new object[7] { "50%", "75%", "100%", "150%", "200%", "300%", "500%" });
		((Control)comboBoxImageQuality).Location = new Point(175, 12);
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		comboBoxImageQuality.SelectedIndex = -1;
		comboBoxImageQuality.SelectedItem = null;
		comboBoxImageQuality.SelectedValue = null;
		((Control)comboBoxImageQuality).Size = new Size(75, 21);
		((Control)comboBoxImageQuality).TabIndex = 1;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(344, 158);
		((Control)this).Controls.Add((Control)(object)panelImageResolution);
		((Control)this).Controls.Add((Control)(object)panelExportDataOnly);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDashboardExcelExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiExcelExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Shown += This_Shown;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelExportDataOnly).ResumeLayout(false);
		((Control)panelImageResolution).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
