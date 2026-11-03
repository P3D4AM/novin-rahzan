using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiOdsExportSetupForm : Form
{
	private IContainer components;

	private Button buttonCancel;

	private Button buttonOk;

	private StiPagesRangeControl PagesRangeControl;

	private StiGroupBox groupBoxExportSettings;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiGroupLine glButtons;

	private StiComboBox comboBoxImageQuality;

	private Label label1;

	private StiLabelInfo labelImageQuality;

	private Label label6;

	private StiComboBox comboBoxImageResolution;

	private StiLabelInfo labelResolution;

	private Panel panelOpenAfterExport;

	public StiPagesRange PagesRange => PagesRangeControl.GetPagesRange();

	public bool OpenAfterExport => ((CheckBox)checkBoxOpenAfterExport).Checked;

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

	public float Resolution => float.Parse(((Control)comboBoxImageResolution).Text.Replace("%", "").Trim());

	public float ImageQuality
	{
		get
		{
			int.TryParse(((Control)comboBoxImageQuality).Text, out var result);
			return result;
		}
	}

	public StiOdsExportSetupForm()
	{
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
		StiSettings.Load();
		((CheckBox)checkBoxOpenAfterExport).Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		groupBoxExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		((Control)comboBoxImageResolution).Handle.ToString();
		((ListControl)comboBoxImageResolution).SelectedIndex = 4;
		((ComboBox)comboBoxImageResolution).SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageResolution", ((ComboBox)comboBoxImageResolution).SelectedItem.ToString());
		((Control)comboBoxImageQuality).Text = StiSettings.GetStr(((Control)this).Name, "ImageQuality", "75");
		LocalizeSuperToolTip();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "ImageResolution", ((ComboBox)comboBoxImageResolution).SelectedItem.ToString());
		StiSettings.Set(((Control)this).Name, "ImageQuality", ((Control)comboBoxImageQuality).Text);
		StiSettings.Save();
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

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_spreadsheets_ods.htm");
	}

	private void Localize()
	{
		((Control)this).Text = string.Format("{0} - {1}", StiLocalization.Get("Export", "ExportTypeCalcFile").Replace("...", ""), StiLocalization.Get("Export", "title").Replace("&", "_"));
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)labelImageQuality).Text = StiLocalization.GetCleaned("Export", "ImageQuality");
		((Control)labelResolution).Text = StiLocalization.GetCleaned("Export", "ImageResolution");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		labelResolution.SetToolTip(StiLocalization.Get("HelpViewer", "ImageResolution"));
		labelImageQuality.SetToolTip(StiLocalization.Get("HelpViewer", "ImageQualityPdf"));
		checkBoxOpenAfterExport.SetToolTip(StiLocalization.Get("HelpViewer", "OpenAfterExport"));
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		buttonCancel = new Button();
		buttonOk = new Button();
		PagesRangeControl = new StiPagesRangeControl();
		groupBoxExportSettings = new StiGroupBox();
		comboBoxImageQuality = new StiComboBox();
		label1 = new Label();
		labelImageQuality = new StiLabelInfo();
		label6 = new Label();
		comboBoxImageResolution = new StiComboBox();
		labelResolution = new StiLabelInfo();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		glButtons = new StiGroupLine();
		panelOpenAfterExport = new Panel();
		((Control)groupBoxExportSettings).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)this).SuspendLayout();
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
		((Control)PagesRangeControl).Anchor = (AnchorStyles)13;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(12, 8);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(345, 159);
		((Control)PagesRangeControl).TabIndex = 0;
		groupBoxExportSettings.AllowCollapse = true;
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label1);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label6);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelResolution);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "groupBoxExportSettings";
		groupBoxExportSettings.ResHeight = 136;
		((Control)groupBoxExportSettings).Size = new Size(345, 88);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageQuality).Items.AddRange(new object[4] { "25", "50", "75", "100" });
		((Control)comboBoxImageQuality).Location = new Point(213, 53);
		((ComboBox)comboBoxImageQuality).MaxDropDownItems = 20;
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		((Control)comboBoxImageQuality).Size = new Size(90, 21);
		((Control)comboBoxImageQuality).TabIndex = 1;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(313, 55);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(14, 17);
		((Control)label1).TabIndex = 28;
		((Control)label1).Text = "%";
		label1.TextAlign = (ContentAlignment)16;
		label1.UseCompatibleTextRendering = true;
		((Control)labelImageQuality).AutoSize = true;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 55);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageQuality).Size = new Size(86, 17);
		((Control)labelImageQuality).TabIndex = 29;
		((Control)labelImageQuality).Text = "Image Quality";
		((Label)labelImageQuality).TextAlign = (ContentAlignment)16;
		((Label)labelImageQuality).UseCompatibleTextRendering = true;
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(313, 22);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(19, 17);
		((Control)label6).TabIndex = 27;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		label6.UseCompatibleTextRendering = true;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolution).Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(213, 20);
		((ComboBox)comboBoxImageResolution).MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(90, 21);
		((Control)comboBoxImageResolution).TabIndex = 0;
		((Control)labelResolution).AutoSize = true;
		((Control)labelResolution).ForeColor = Color.DimGray;
		((Control)labelResolution).Location = new Point(12, 22);
		((Control)labelResolution).Name = "labelResolution";
		((Control)labelResolution).Padding = new Padding(0, 0, 12, 0);
		((Control)labelResolution).Size = new Size(104, 17);
		((Control)labelResolution).TabIndex = 25;
		((Control)labelResolution).Text = "Image Resolution";
		((Label)labelResolution).TextAlign = (ContentAlignment)16;
		((Label)labelResolution).UseCompatibleTextRendering = true;
		((Control)checkBoxOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(120, 18);
		((Control)checkBoxOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((ButtonBase)checkBoxOpenAfterExport).UseCompatibleTextRendering = true;
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 307);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(370, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 265);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(370, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(370, 353);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiOdsExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiOdsExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)groupBoxExportSettings).ResumeLayout(false);
		((Control)groupBoxExportSettings).PerformLayout();
		((Control)glButtons).ResumeLayout(false);
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelOpenAfterExport).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
