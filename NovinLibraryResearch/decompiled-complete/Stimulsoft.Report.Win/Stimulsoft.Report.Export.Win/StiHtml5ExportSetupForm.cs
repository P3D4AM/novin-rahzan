using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Export.Win;

public class StiHtml5ExportSetupForm : Form
{
	private IContainer components;

	private ComboBox cbImageFormat;

	private StiPagesRangeControl PagesRangeControl;

	private CheckBox cbOpenAfterExport;

	private CheckBox checkBoxContinuousPages;

	private Button btCancel;

	private Button btOk;

	private Label lbImageFormat;

	private StiGroupBox gbExportSettings;

	private StiGroupLine glButtons;

	private ComboBox comboBoxImageResolution;

	private Label lbResolution;

	private StiGroupBox gbImageQuality;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	private TrackBar tbImageQuality;

	private Label label6;

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

	public bool ContinuousPages => checkBoxContinuousPages.Checked;

	public ImageFormat ImageFormat => (ImageFormat)(((Control)cbImageFormat).Text switch
	{
		"Bmp" => ImageFormat.Bmp, 
		"Gif" => ImageFormat.Gif, 
		"Png" => ImageFormat.Png, 
		_ => ImageFormat.Jpeg, 
	});

	public float Resolution => float.Parse(((Control)comboBoxImageResolution).Text.Replace("%", "").Trim());

	public float ImageQuality => (float)tbImageQuality.Value / 100f;

	public StiHtml5ExportSetupForm()
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
		((Control)cbImageFormat).Text = "Jpeg";
		Localize();
		StiSettings.Load();
		cbOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		checkBoxContinuousPages.Checked = StiSettings.GetBool(((Control)this).Name, "ContinuousPages", checkBoxContinuousPages.Checked);
		gbExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		cbImageFormat.SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageFormat", ((Control)cbImageFormat).Text);
		((Control)comboBoxImageResolution).Handle.ToString();
		((ListControl)comboBoxImageResolution).SelectedIndex = 4;
		comboBoxImageResolution.SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageResolution", comboBoxImageResolution.SelectedItem.ToString());
		((Control)tbImageQuality).Handle.ToString();
		tbImageQuality.Value = StiSettings.GetInt(((Control)this).Name, "ImageQuality", tbImageQuality.Value);
	}

	private void Localize()
	{
		((Control)lbImageFormat).Text = StiLocalization.Get("Export", "ImageFormat");
		((Control)gbExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)cbOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)checkBoxContinuousPages).Text = StiLocalization.Get("Export", "ContinuousPages");
		((Control)gbImageQuality).Text = StiLocalization.Get("Export", "ImageQuality");
		((Control)lbResolution).Text = StiLocalization.Get("Export", "ImageResolution");
		((Control)this).Text = StiLocalization.Get("Export", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void StiHtmlExportSetupForm_Closed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "ContinuousPages", checkBoxContinuousPages.Checked);
		StiSettings.Set(((Control)this).Name, "ImageResolution", comboBoxImageResolution.SelectedItem.ToString());
		StiSettings.Set(((Control)this).Name, "ImageQuality", tbImageQuality.Value);
		StiSettings.Set(((Control)this).Name, "ImageFormat", cbImageFormat.SelectedItem);
		StiSettings.Save();
	}

	private void StiHtmlExportSetupForm_KeyDown(object sender, KeyEventArgs e)
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
		StiHelpProvider.ShowHelpViewer("user-manual/exports_web_documents_html5.htm");
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
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
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca1: Expected O, but got Unknown
		btCancel = new Button();
		btOk = new Button();
		cbImageFormat = new ComboBox();
		lbImageFormat = new Label();
		gbExportSettings = new StiGroupBox();
		label6 = new Label();
		comboBoxImageResolution = new ComboBox();
		lbResolution = new Label();
		checkBoxContinuousPages = new CheckBox();
		gbImageQuality = new StiGroupBox();
		label5 = new Label();
		label4 = new Label();
		label3 = new Label();
		label2 = new Label();
		label1 = new Label();
		tbImageQuality = new TrackBar();
		cbOpenAfterExport = new CheckBox();
		glButtons = new StiGroupLine();
		PagesRangeControl = new StiPagesRangeControl();
		((Control)gbExportSettings).SuspendLayout();
		((Control)gbImageQuality).SuspendLayout();
		((ISupportInitialize)tbImageQuality).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)6;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(232, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)6;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(152, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)cbImageFormat).BackColor = SystemColors.Window;
		((ListControl)cbImageFormat).DisplayMember = "Png";
		cbImageFormat.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbImageFormat).ForeColor = SystemColors.ControlText;
		cbImageFormat.Items.AddRange(new object[4] { "Jpeg", "Gif", "Bmp", "Png" });
		((Control)cbImageFormat).Location = new Point(179, 24);
		((Control)cbImageFormat).Name = "cbImageFormat";
		((Control)cbImageFormat).Size = new Size(88, 21);
		((Control)cbImageFormat).TabIndex = 0;
		((Control)lbImageFormat).AutoSize = true;
		((Control)lbImageFormat).ForeColor = Color.DimGray;
		((Control)lbImageFormat).Location = new Point(5, 27);
		((Control)lbImageFormat).Name = "lbImageFormat";
		((Control)lbImageFormat).Size = new Size(68, 13);
		((Control)lbImageFormat).TabIndex = 0;
		((Control)lbImageFormat).Text = "ImageFormat";
		lbImageFormat.TextAlign = (ContentAlignment)16;
		gbExportSettings.AllowCollapse = true;
		((Control)gbExportSettings).Controls.Add((Control)(object)label6);
		((Control)gbExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbResolution);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxContinuousPages);
		((Control)gbExportSettings).Controls.Add((Control)(object)gbImageQuality);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbImageFormat);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbImageFormat);
		((Control)gbExportSettings).Location = new Point(4, 142);
		((Control)gbExportSettings).Name = "gbExportSettings";
		gbExportSettings.ResHeight = 187;
		((Control)gbExportSettings).Size = new Size(304, 189);
		((Control)gbExportSettings).TabIndex = 1;
		((GroupBox)gbExportSettings).TabStop = false;
		((Control)gbExportSettings).Text = "ExportSettings";
		((Control)label6).AutoSize = true;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(273, 51);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(21, 13);
		((Control)label6).TabIndex = 4;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		comboBoxImageResolution.Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(179, 48);
		comboBoxImageResolution.MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(88, 21);
		((Control)comboBoxImageResolution).TabIndex = 1;
		((Control)lbResolution).AutoSize = true;
		((Control)lbResolution).ForeColor = Color.DimGray;
		((Control)lbResolution).Location = new Point(5, 51);
		((Control)lbResolution).Name = "lbResolution";
		((Control)lbResolution).Size = new Size(89, 13);
		((Control)lbResolution).TabIndex = 2;
		((Control)lbResolution).Text = "Image Resolution";
		lbResolution.TextAlign = (ContentAlignment)16;
		((Control)checkBoxContinuousPages).Anchor = (AnchorStyles)13;
		((Control)checkBoxContinuousPages).AutoSize = true;
		((Control)checkBoxContinuousPages).Location = new Point(8, 162);
		((Control)checkBoxContinuousPages).Name = "checkBoxContinuousPages";
		((Control)checkBoxContinuousPages).Size = new Size(112, 17);
		((Control)checkBoxContinuousPages).TabIndex = 3;
		((Control)checkBoxContinuousPages).Text = "Continuous Pages";
		((Control)gbImageQuality).Anchor = (AnchorStyles)13;
		((Control)gbImageQuality).Controls.Add((Control)(object)label5);
		((Control)gbImageQuality).Controls.Add((Control)(object)label4);
		((Control)gbImageQuality).Controls.Add((Control)(object)label3);
		((Control)gbImageQuality).Controls.Add((Control)(object)label2);
		((Control)gbImageQuality).Controls.Add((Control)(object)label1);
		((Control)gbImageQuality).Controls.Add((Control)(object)tbImageQuality);
		((Control)gbImageQuality).Location = new Point(8, 75);
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
		((Control)cbOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)cbOpenAfterExport).AutoSize = true;
		((Control)cbOpenAfterExport).BackColor = Color.Transparent;
		((Control)cbOpenAfterExport).Location = new Point(12, 341);
		((Control)cbOpenAfterExport).Name = "cbOpenAfterExport";
		((Control)cbOpenAfterExport).Size = new Size(104, 17);
		((Control)cbOpenAfterExport).TabIndex = 2;
		((Control)cbOpenAfterExport).Text = "OpenAfterExport";
		((ButtonBase)cbOpenAfterExport).UseVisualStyleBackColor = false;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 365);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(312, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(4, 4);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(304, 136);
		((Control)PagesRangeControl).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(312, 411);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Control)this).Controls.Add((Control)(object)gbExportSettings);
		((Control)this).Controls.Add((Control)(object)cbOpenAfterExport);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiHtml5ExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiHtml5ExportSetupForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += StiHtmlExportSetupForm_Closed;
		((Control)this).KeyDown += new KeyEventHandler(StiHtmlExportSetupForm_KeyDown);
		((Control)gbExportSettings).ResumeLayout(false);
		((Control)gbExportSettings).PerformLayout();
		((Control)gbImageQuality).ResumeLayout(false);
		((Control)gbImageQuality).PerformLayout();
		((ISupportInitialize)tbImageQuality).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
