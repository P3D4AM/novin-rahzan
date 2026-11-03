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

public class StiXpsExportSetupForm : Form
{
	private StiExportFormat exportFormat = StiExportFormat.Xps;

	private IContainer components;

	private Button buttonCancel;

	private Button buttonOk;

	private StiPagesRangeControl PagesRangeControl;

	private StiGroupBox groupBoxExportSettings;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiLabelInfo labelResolution;

	private StiComboBox comboBoxImageResolution;

	private StiGroupLine glButtons;

	private StiCheckBoxInfo checkBoxExportRtfTextAsImage;

	private Label label6;

	private StiComboBox comboBoxImageQuality;

	private Label label1;

	private StiLabelInfo labelImageQuality;

	public StiExportFormat ExportFormat
	{
		set
		{
			exportFormat = value;
		}
	}

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
			return (float)result / 100f;
		}
	}

	public bool ExportRtfTextAsImage => ((CheckBox)checkBoxExportRtfTextAsImage).Checked;

	public StiXpsExportSetupForm()
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
		((CheckBox)checkBoxExportRtfTextAsImage).Checked = true;
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
		StiSettings.Set(((Control)this).Name, "ExportRtfTextAsImage", ((CheckBox)checkBoxExportRtfTextAsImage).Checked);
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
		string url = ((exportFormat == StiExportFormat.Xps) ? "user-manual/exports_formats_with_fixed_page_layout_xps.htm" : "user-manual/exports_formats_with_fixed_page_layout_microsoft_power_point_2007_2010.htm");
		StiHelpProvider.ShowHelpViewer(url);
	}

	private void This_Load(object sender, EventArgs e)
	{
		((Control)this).Text = ((exportFormat == StiExportFormat.Xps) ? (Loc.Get("Export", "ExportTypeXpsFile").Replace("...", "") + " - " + Loc.Get("Export", "title").Replace("&", "_")) : (Loc.Get("Export", "ExportTypePpt2007File").Replace("...", "") + " - " + Loc.Get("Export", "title").Replace("&", "_")));
	}

	private void Localize()
	{
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)labelImageQuality).Text = Loc.GetCleaned("Export", "ImageQuality");
		((Control)labelResolution).Text = Loc.GetCleaned("Export", "ImageResolution");
		((Control)checkBoxOpenAfterExport).Text = Loc.GetCleaned("Export", "OpenAfterExport");
		((Control)checkBoxExportRtfTextAsImage).Text = StiLocalization.Get("Export", "ExportRtfTextAsImage");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		labelResolution.SetToolTip(StiLocalization.Get("HelpViewer", "ImageResolution"));
		labelImageQuality.SetToolTip(StiLocalization.Get("HelpViewer", "ImageQuality"));
		checkBoxExportRtfTextAsImage.SetToolTip(StiLocalization.Get("HelpViewer", "ExportRtfTextAsImage"));
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Expected O, but got Unknown
		buttonCancel = new Button();
		buttonOk = new Button();
		groupBoxExportSettings = new StiGroupBox();
		comboBoxImageQuality = new StiComboBox();
		label1 = new Label();
		labelImageQuality = new StiLabelInfo();
		label6 = new Label();
		comboBoxImageResolution = new StiComboBox();
		labelResolution = new StiLabelInfo();
		checkBoxExportRtfTextAsImage = new StiCheckBoxInfo();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		glButtons = new StiGroupLine();
		PagesRangeControl = new StiPagesRangeControl();
		((Control)groupBoxExportSettings).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
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
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label1);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label6);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelResolution);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "groupBoxExportSettings";
		groupBoxExportSettings.ResHeight = 162;
		((Control)groupBoxExportSettings).Size = new Size(345, 90);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageQuality).Items.AddRange(new object[4] { "25", "50", "75", "100" });
		((Control)comboBoxImageQuality).Location = new Point(184, 53);
		((ComboBox)comboBoxImageQuality).MaxDropDownItems = 20;
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		((Control)comboBoxImageQuality).Size = new Size(122, 21);
		((Control)comboBoxImageQuality).TabIndex = 1;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(312, 57);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(15, 13);
		((Control)label1).TabIndex = 19;
		((Control)label1).Text = "%";
		label1.TextAlign = (ContentAlignment)16;
		((Control)labelImageQuality).AutoSize = true;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 57);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageQuality).Size = new Size(83, 13);
		((Control)labelImageQuality).TabIndex = 20;
		((Control)labelImageQuality).Text = "Image Quality";
		((Label)labelImageQuality).TextAlign = (ContentAlignment)16;
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(312, 25);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(21, 13);
		((Control)label6).TabIndex = 11;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolution).Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(184, 20);
		((ComboBox)comboBoxImageResolution).MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(122, 21);
		((Control)comboBoxImageResolution).TabIndex = 0;
		((Control)labelResolution).AutoSize = true;
		((Control)labelResolution).ForeColor = Color.DimGray;
		((Control)labelResolution).Location = new Point(12, 25);
		((Control)labelResolution).Name = "labelResolution";
		((Control)labelResolution).Padding = new Padding(0, 0, 12, 0);
		((Control)labelResolution).Size = new Size(101, 13);
		((Control)labelResolution).TabIndex = 0;
		((Control)labelResolution).Text = "Image Resolution";
		((Label)labelResolution).TextAlign = (ContentAlignment)16;
		((Control)checkBoxExportRtfTextAsImage).Anchor = (AnchorStyles)6;
		((Control)checkBoxExportRtfTextAsImage).AutoSize = true;
		((CheckBox)checkBoxExportRtfTextAsImage).Checked = true;
		((CheckBox)checkBoxExportRtfTextAsImage).CheckState = (CheckState)1;
		((Control)checkBoxExportRtfTextAsImage).ForeColor = SystemColors.ControlText;
		((Control)checkBoxExportRtfTextAsImage).Location = new Point(12, 280);
		((Control)checkBoxExportRtfTextAsImage).Name = "checkBoxExportRtfTextAsImage";
		((Control)checkBoxExportRtfTextAsImage).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxExportRtfTextAsImage).Size = new Size(155, 17);
		((Control)checkBoxExportRtfTextAsImage).TabIndex = 2;
		((Control)checkBoxExportRtfTextAsImage).Text = "Export Rtf Text as Image";
		((Control)checkBoxOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 308);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 3;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 337);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(370, 46);
		((Control)glButtons).TabIndex = 4;
		((Control)PagesRangeControl).Anchor = (AnchorStyles)13;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(12, 12);
		((Control)PagesRangeControl).Margin = new Padding(6);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(345, 159);
		((Control)PagesRangeControl).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(370, 383);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Control)this).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Control)this).Controls.Add((Control)(object)checkBoxExportRtfTextAsImage);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiXpsExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiXpsExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)groupBoxExportSettings).ResumeLayout(false);
		((Control)groupBoxExportSettings).PerformLayout();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
