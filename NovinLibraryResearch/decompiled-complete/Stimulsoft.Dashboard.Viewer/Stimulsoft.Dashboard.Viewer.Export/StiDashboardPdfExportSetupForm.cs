using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Viewer.Export;

public class StiDashboardPdfExportSetupForm : Form
{
	private IContainer components;

	private StiGroupLine glButtons;

	private Button buttonOk;

	private Button buttonCancel;

	private CheckBox checkBoxOpenAfterExport;

	private StiComboBox comboBoxOrientation;

	private Label labelOrientation;

	private Label labelImageQuality;

	private StiDropDownBox comboBoxImageQuality;

	public StiComboBox comboBoxPaperSize;

	private Label labelPaperSize;

	public StiDashboardPdfExportSetupForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			InitializePaperSizes();
			Localize();
			LoadSettings();
		}
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Export", "ExportTypePdfFile").Replace("...", "") + " - " + Loc.Get("Export", "title").Replace("&", "_");
		((Control)labelPaperSize).Text = Loc.Get("PropertyMain", "PaperSize");
		((Control)labelImageQuality).Text = Loc.Get("Export", "ImageQuality").Replace(":", "");
		((Control)labelOrientation).Text = Loc.Get("FormPageSetup", "Orientation");
		((Control)checkBoxOpenAfterExport).Text = Loc.Get("Export", "OpenAfterExport");
		((ComboBox)comboBoxOrientation).Items.Clear();
		((ComboBox)comboBoxOrientation).Items.Add((object)Loc.Get("FormPageSetup", "PageOrientationPortrait"));
		((ComboBox)comboBoxOrientation).Items.Add((object)Loc.Get("FormPageSetup", "PageOrientationLandscape"));
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
	}

	private void InitializePaperSizes()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		PaperSizeCollection val = StiOptions.Print.CustomPaperSizes;
		if (val == null)
		{
			PrinterSettings val2 = new PrinterSettings();
			val = val2.PaperSizes;
		}
		((ComboBox)comboBoxPaperSize).Items.Clear();
		foreach (PaperSize item in val)
		{
			PaperSize val3 = item;
			if ((int)val3.Kind != 0)
			{
				((ComboBox)comboBoxPaperSize).Items.Add((object)val3.PaperName);
			}
		}
	}

	private PaperKind GetPaperKind()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(((Control)comboBoxPaperSize).Text))
		{
			PaperSizeCollection val = StiOptions.Print.CustomPaperSizes;
			if (val == null)
			{
				PrinterSettings val2 = new PrinterSettings();
				val = val2.PaperSizes;
			}
			((ComboBox)comboBoxPaperSize).Items.Clear();
			foreach (PaperSize item in val)
			{
				PaperSize val3 = item;
				if ((int)val3.Kind != 0 && val3.PaperName == ((Control)comboBoxPaperSize).Text)
				{
					return val3.Kind;
				}
			}
		}
		return (PaperKind)0;
	}

	public StiDashboardExportSettings GetExportSettings()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		int.TryParse(((Control)comboBoxImageQuality).Text.Replace("%", ""), out var result);
		return new StiPdfDashboardExportSettings
		{
			PaperSize = GetPaperKind(),
			Orientation = ((((ListControl)comboBoxOrientation).SelectedIndex != 0) ? StiPageOrientation.Landscape : StiPageOrientation.Portrait),
			OpenAfterExport = checkBoxOpenAfterExport.Checked,
			ImageQuality = result
		};
	}

	private void LoadSettings()
	{
		StiSettings.Load();
		((Control)comboBoxPaperSize).Text = StiSettings.GetStr(((Control)this).Name, "PaperSize", "A4");
		((ListControl)comboBoxPaperSize).SelectedIndex = Math.Max(0, ((ListControl)comboBoxPaperSize).SelectedIndex);
		((ListControl)comboBoxOrientation).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "Orientation", 1);
		((Control)comboBoxImageQuality).Text = StiSettings.GetStr(((Control)this).Name, "ImageQuality", "200%");
		checkBoxOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", nullValue: true);
	}

	private void SaveSettings()
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "PaperSize", ((Control)comboBoxPaperSize).Text);
		StiSettings.Set(((Control)this).Name, "Orientation", ((ListControl)comboBoxOrientation).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "ImageQuality", ((Control)comboBoxImageQuality).Text.EndsWith("%") ? ((Control)comboBoxImageQuality).Text : (((Control)comboBoxImageQuality).Text + "%"));
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", checkBoxOpenAfterExport.Checked);
		StiSettings.Save();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpDefaultBrowser("user-manual/index.html?exports_dashboards.htm#pdfexportsettings");
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Expected O, but got Unknown
		glButtons = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		checkBoxOpenAfterExport = new CheckBox();
		comboBoxOrientation = new StiComboBox();
		labelOrientation = new Label();
		labelImageQuality = new Label();
		comboBoxImageQuality = new StiDropDownBox();
		comboBoxPaperSize = new StiComboBox();
		labelPaperSize = new Label();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 144);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(344, 46);
		((Control)glButtons).TabIndex = 7;
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
		checkBoxOpenAfterExport.Checked = true;
		checkBoxOpenAfterExport.CheckState = (CheckState)1;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 111);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(2, 0, 0, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(320, 22);
		((Control)checkBoxOpenAfterExport).TabIndex = 6;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((ButtonBase)checkBoxOpenAfterExport).UseVisualStyleBackColor = true;
		((Control)comboBoxOrientation).Anchor = (AnchorStyles)9;
		((Control)comboBoxOrientation).BackColor = SystemColors.Window;
		comboBoxOrientation.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxOrientation).ForeColor = SystemColors.ControlText;
		((Control)comboBoxOrientation).Location = new Point(175, 45);
		((ComboBox)comboBoxOrientation).MaxDropDownItems = 20;
		((Control)comboBoxOrientation).Name = "comboBoxOrientation";
		((Control)comboBoxOrientation).Size = new Size(160, 21);
		((Control)comboBoxOrientation).TabIndex = 2;
		((Control)labelOrientation).ForeColor = Color.DimGray;
		((Control)labelOrientation).Location = new Point(12, 45);
		((Control)labelOrientation).Name = "labelOrientation";
		((Control)labelOrientation).Size = new Size(158, 22);
		((Control)labelOrientation).TabIndex = 3;
		((Control)labelOrientation).Text = "Orientation";
		labelOrientation.TextAlign = (ContentAlignment)16;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 78);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Size = new Size(158, 22);
		((Control)labelImageQuality).TabIndex = 5;
		((Control)labelImageQuality).Text = "ImageQuality";
		labelImageQuality.TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageQuality).Anchor = (AnchorStyles)9;
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DrawMode = (DrawMode)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		comboBoxImageQuality.IsEditable = true;
		comboBoxImageQuality.Items.AddRange(new object[7] { "50%", "75%", "100%", "150%", "200%", "300%", "500%" });
		((Control)comboBoxImageQuality).Location = new Point(175, 78);
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		comboBoxImageQuality.SelectedIndex = -1;
		comboBoxImageQuality.SelectedItem = null;
		comboBoxImageQuality.SelectedValue = null;
		((Control)comboBoxImageQuality).Size = new Size(75, 21);
		((Control)comboBoxImageQuality).TabIndex = 4;
		((Control)comboBoxPaperSize).Anchor = (AnchorStyles)9;
		((Control)comboBoxPaperSize).BackColor = SystemColors.Window;
		comboBoxPaperSize.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxPaperSize).ForeColor = SystemColors.ControlText;
		((Control)comboBoxPaperSize).Location = new Point(175, 12);
		((ComboBox)comboBoxPaperSize).MaxDropDownItems = 20;
		((Control)comboBoxPaperSize).Name = "comboBoxPaperSize";
		((Control)comboBoxPaperSize).Size = new Size(160, 21);
		((Control)comboBoxPaperSize).TabIndex = 0;
		((Control)labelPaperSize).ForeColor = Color.DimGray;
		((Control)labelPaperSize).Location = new Point(12, 12);
		((Control)labelPaperSize).Name = "labelPaperSize";
		((Control)labelPaperSize).Size = new Size(158, 22);
		((Control)labelPaperSize).TabIndex = 1;
		((Control)labelPaperSize).Text = "PaperSize";
		labelPaperSize.TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(344, 190);
		((Control)this).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)labelImageQuality);
		((Control)this).Controls.Add((Control)(object)labelOrientation);
		((Control)this).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)this).Controls.Add((Control)(object)comboBoxOrientation);
		((Control)this).Controls.Add((Control)(object)comboBoxPaperSize);
		((Control)this).Controls.Add((Control)(object)labelPaperSize);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDashboardPdfExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiDashboardPdfExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
