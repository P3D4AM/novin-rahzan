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

public class StiRtfExportSetupForm : Form
{
	private ComboBoxItem comboBoxItemFrame = new ComboBoxItem();

	private ComboBoxItem comboBoxItemTable = new ComboBoxItem();

	private IContainer components;

	private Button buttonCancel;

	private Button buttonOk;

	private StiPagesRangeControl PagesRangeControl;

	private StiGroupBox groupBoxExportSettings;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiGroupLine glButtons;

	private StiCheckBoxInfo checkboxUsePageHeadersAndFooters;

	private StiComboBox comboBoxImageResolution;

	private StiLabelInfo labelResolution;

	private Label label6;

	private StiCheckBoxInfo checkboxRemoveEmptySpaceAtBottom;

	private StiComboBox comboBoxImageQuality;

	private Label label1;

	private StiLabelInfo labelImageQuality;

	private StiComboBox comboBoxExportMode;

	private StiLabelInfo labelExportMode;

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
			return (float)result / 100f;
		}
	}

	public bool UsePageHeadersAndFooters => ((CheckBox)checkboxUsePageHeadersAndFooters).Checked;

	public bool RemoveEmptySpaceAtBottom => ((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked;

	public StiRtfExportMode ExportMode
	{
		get
		{
			if (((ListControl)comboBoxExportMode).SelectedIndex != 0)
			{
				return StiRtfExportMode.Frame;
			}
			return StiRtfExportMode.Table;
		}
	}

	public StiRtfExportSetupForm()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
		((ComboBox)comboBoxExportMode).SelectedIndexChanged += ComboBoxExportMode_SelectedIndexChanged;
		((ComboBox)comboBoxExportMode).Items.Add((object)comboBoxItemTable);
		((ComboBox)comboBoxExportMode).Items.Add((object)comboBoxItemFrame);
		((ListControl)comboBoxExportMode).SelectedIndex = 0;
		StiSettings.Load();
		((CheckBox)checkBoxOpenAfterExport).Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		groupBoxExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		((ListControl)comboBoxExportMode).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ExportMode", 0);
		((CheckBox)checkboxUsePageHeadersAndFooters).Checked = StiSettings.GetBool(((Control)this).Name, "UsePageHeadersAndFooters", ((CheckBox)checkboxUsePageHeadersAndFooters).Checked);
		((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked = StiSettings.GetBool(((Control)this).Name, "RemoveEmptySpaceAtBottom", StiOptions.Export.RichText.RemoveEmptySpaceAtBottom);
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
		StiSettings.Set(((Control)this).Name, "UsePageHeadersAndFooters", ((CheckBox)checkboxUsePageHeadersAndFooters).Checked);
		StiSettings.Set(((Control)this).Name, "RemoveEmptySpaceAtBottom", ((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked);
		StiSettings.Set(((Control)this).Name, "ExportMode", ((ListControl)comboBoxExportMode).SelectedIndex);
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
		StiHelpProvider.ShowHelpViewer("user-manual/exports_text_formats_rtf.htm");
	}

	private void ComboBoxExportMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (checkboxUsePageHeadersAndFooters != null)
		{
			((Control)checkboxUsePageHeadersAndFooters).Enabled = ExportMode == StiRtfExportMode.Table;
		}
		if (checkboxRemoveEmptySpaceAtBottom != null)
		{
			((Control)checkboxRemoveEmptySpaceAtBottom).Enabled = ExportMode == StiRtfExportMode.Table;
		}
	}

	private void Localize()
	{
		((Control)this).Text = string.Format("{0} - {1}", StiLocalization.Get("Export", "ExportTypeRtfFile").Replace("...", ""), StiLocalization.Get("Export", "title").Replace("&", "_"));
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)labelExportMode).Text = StiLocalization.GetCleaned("Export", "ExportMode");
		((BaseItem)comboBoxItemFrame).Text = StiLocalization.Get("Export", "ExportModeFrame");
		((BaseItem)comboBoxItemTable).Text = StiLocalization.Get("Export", "ExportModeTable");
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)checkboxUsePageHeadersAndFooters).Text = StiLocalization.Get("Export", "UsePageHeadersAndFooters");
		((Control)checkboxRemoveEmptySpaceAtBottom).Text = StiLocalization.Get("Export", "RemoveEmptySpaceAtBottom");
		((Control)labelImageQuality).Text = StiLocalization.GetCleaned("Export", "ImageQuality");
		((Control)labelResolution).Text = StiLocalization.GetCleaned("Export", "ImageResolution");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		labelResolution.SetToolTip(StiLocalization.Get("HelpViewer", "ImageResolution"));
		labelImageQuality.SetToolTip(StiLocalization.Get("HelpViewer", "ImageQuality"));
		labelExportMode.SetToolTip(StiLocalization.Get("HelpViewer", "ExportModeRtf"));
		checkboxUsePageHeadersAndFooters.SetToolTip(StiLocalization.Get("HelpViewer", "UsePageHeadersAndFooters"));
		checkboxRemoveEmptySpaceAtBottom.SetToolTip(StiLocalization.Get("HelpViewer", "RemoveEmptySpaceAtBottom"));
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Expected O, but got Unknown
		buttonCancel = new Button();
		buttonOk = new Button();
		PagesRangeControl = new StiPagesRangeControl();
		groupBoxExportSettings = new StiGroupBox();
		comboBoxExportMode = new StiComboBox();
		labelExportMode = new StiLabelInfo();
		comboBoxImageQuality = new StiComboBox();
		label1 = new Label();
		labelImageQuality = new StiLabelInfo();
		checkboxRemoveEmptySpaceAtBottom = new StiCheckBoxInfo();
		label6 = new Label();
		comboBoxImageResolution = new StiComboBox();
		labelResolution = new StiLabelInfo();
		checkboxUsePageHeadersAndFooters = new StiCheckBoxInfo();
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
		((Control)PagesRangeControl).Anchor = (AnchorStyles)13;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(12, 8);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(345, 159);
		((Control)PagesRangeControl).TabIndex = 0;
		groupBoxExportSettings.AllowCollapse = true;
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxExportMode);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelExportMode);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label1);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelImageQuality);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkboxRemoveEmptySpaceAtBottom);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label6);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkboxUsePageHeadersAndFooters);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "groupBoxExportSettings";
		groupBoxExportSettings.ResHeight = 266;
		((Control)groupBoxExportSettings).Size = new Size(345, 178);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)comboBoxExportMode).BackColor = SystemColors.Window;
		comboBoxExportMode.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxExportMode).ForeColor = SystemColors.ControlText;
		((Control)comboBoxExportMode).Location = new Point(216, 85);
		((ComboBox)comboBoxExportMode).MaxDropDownItems = 20;
		((Control)comboBoxExportMode).Name = "comboBoxExportMode";
		((Control)comboBoxExportMode).Size = new Size(90, 21);
		((Control)comboBoxExportMode).TabIndex = 2;
		((Control)labelExportMode).AutoSize = true;
		((Control)labelExportMode).ForeColor = Color.DimGray;
		((Control)labelExportMode).Location = new Point(12, 88);
		((Control)labelExportMode).Name = "labelExportMode";
		((Control)labelExportMode).Padding = new Padding(0, 0, 12, 0);
		((Control)labelExportMode).Size = new Size(79, 13);
		((Control)labelExportMode).TabIndex = 25;
		((Control)labelExportMode).Text = "Export Mode";
		((Label)labelExportMode).TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageQuality).Items.AddRange(new object[4] { "25", "50", "75", "100" });
		((Control)comboBoxImageQuality).Location = new Point(216, 53);
		((ComboBox)comboBoxImageQuality).MaxDropDownItems = 20;
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		((Control)comboBoxImageQuality).Size = new Size(90, 21);
		((Control)comboBoxImageQuality).TabIndex = 1;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(314, 56);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(15, 13);
		((Control)label1).TabIndex = 22;
		((Control)label1).Text = "%";
		label1.TextAlign = (ContentAlignment)16;
		((Control)labelImageQuality).AutoSize = true;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 56);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageQuality).Size = new Size(83, 13);
		((Control)labelImageQuality).TabIndex = 23;
		((Control)labelImageQuality).Text = "Image Quality";
		((Label)labelImageQuality).TextAlign = (ContentAlignment)16;
		((Control)checkboxRemoveEmptySpaceAtBottom).AutoSize = true;
		((CheckBox)checkboxRemoveEmptySpaceAtBottom).Checked = true;
		((CheckBox)checkboxRemoveEmptySpaceAtBottom).CheckState = (CheckState)1;
		((Control)checkboxRemoveEmptySpaceAtBottom).ForeColor = SystemColors.ControlText;
		((Control)checkboxRemoveEmptySpaceAtBottom).Location = new Point(15, 149);
		((Control)checkboxRemoveEmptySpaceAtBottom).Name = "checkboxRemoveEmptySpaceAtBottom";
		((Control)checkboxRemoveEmptySpaceAtBottom).Padding = new Padding(0, 0, 12, 0);
		((Control)checkboxRemoveEmptySpaceAtBottom).Size = new Size(245, 17);
		((Control)checkboxRemoveEmptySpaceAtBottom).TabIndex = 4;
		((Control)checkboxRemoveEmptySpaceAtBottom).Text = "Remove empty space at bottom of the page";
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(314, 25);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(21, 13);
		((Control)label6).TabIndex = 10;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolution).Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(216, 20);
		((ComboBox)comboBoxImageResolution).MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(90, 21);
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
		((Control)checkboxUsePageHeadersAndFooters).AutoSize = true;
		((Control)checkboxUsePageHeadersAndFooters).ForeColor = SystemColors.ControlText;
		((Control)checkboxUsePageHeadersAndFooters).Location = new Point(15, 118);
		((Control)checkboxUsePageHeadersAndFooters).Name = "checkboxUsePageHeadersAndFooters";
		((Control)checkboxUsePageHeadersAndFooters).Padding = new Padding(0, 0, 12, 0);
		((Control)checkboxUsePageHeadersAndFooters).Size = new Size(181, 17);
		((Control)checkboxUsePageHeadersAndFooters).TabIndex = 3;
		((Control)checkboxUsePageHeadersAndFooters).Text = "Use page headers and footers";
		((Control)checkBoxOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 396);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(370, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 354);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(370, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(370, 442);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiRtfExportSetupForm";
		((Control)this).Padding = new Padding(0, 3, 0, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiRtfExportSetupForm";
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
