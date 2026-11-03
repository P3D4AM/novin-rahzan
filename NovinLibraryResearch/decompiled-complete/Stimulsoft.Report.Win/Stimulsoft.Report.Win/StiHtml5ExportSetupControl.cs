using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Win;

[ToolboxItem(false)]
public class StiHtml5ExportSetupControl : UserControl
{
	private IContainer components;

	internal StiComboBox comboBoxImageResolution;

	private StiLabelInfo labelResolution;

	internal StiCheckBoxInfo checkBoxContinuousPages;

	private StiLabelInfo labelImageFormat;

	internal StiComboBox comboBoxImageFormat;

	internal StiComboBox comboBoxImageQuality;

	private StiLabelInfo labelImageQuality;

	internal StiCheckBoxInfo checkBoxCompressToArchive;

	internal Label labelDpi;

	internal Label labelPercent;

	internal bool ContinuousPages => ((CheckBox)checkBoxContinuousPages).Checked;

	internal ImageFormat ImageFormat => (ImageFormat)(((Control)comboBoxImageFormat).Text switch
	{
		"Bmp" => ImageFormat.Bmp, 
		"Gif" => ImageFormat.Gif, 
		"Png" => ImageFormat.Png, 
		_ => ImageFormat.Jpeg, 
	});

	internal float Resolution => float.Parse(((Control)comboBoxImageResolution).Text.Replace("%", "").Trim());

	internal float ImageQuality
	{
		get
		{
			int.TryParse(((Control)comboBoxImageQuality).Text, out var result);
			return (float)result / 100f;
		}
	}

	internal bool CompressToArchive => ((CheckBox)checkBoxCompressToArchive).Checked;

	public StiHtml5ExportSetupControl()
	{
		InitializeComponent();
		Localize();
		((ListControl)comboBoxImageFormat).SelectedIndex = 0;
		((ListControl)comboBoxImageResolution).SelectedIndex = 5;
		((ListControl)comboBoxImageQuality).SelectedIndex = 3;
		LocalizeSuperToolTip();
	}

	private void Localize()
	{
		((Control)labelImageFormat).Text = Loc.GetCleaned("Export", "ImageFormat").Replace("&", "_");
		((Control)labelImageQuality).Text = Loc.GetCleaned("Export", "ImageQuality").Replace("&", "_");
		((Control)labelResolution).Text = Loc.GetCleaned("Export", "ImageResolution");
		((Control)checkBoxCompressToArchive).Text = StiLocalization.Get("Export", "CompressToArchive");
		((Control)checkBoxContinuousPages).Text = StiLocalization.Get("Export", "ContinuousPages");
	}

	private void LocalizeSuperToolTip()
	{
		labelImageFormat.SetToolTip(StiLocalization.Get("HelpViewer", "ImageFormat"));
		labelResolution.SetToolTip(StiLocalization.Get("HelpViewer", "ImageResolution"));
		labelImageQuality.SetToolTip(StiLocalization.Get("HelpViewer", "ImageQuality"));
		checkBoxCompressToArchive.SetToolTip(StiLocalization.Get("HelpViewer", "CompressToArchive"));
		checkBoxContinuousPages.SetToolTip(StiLocalization.Get("HelpViewer", "ContinuousPages"));
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		labelDpi = new Label();
		comboBoxImageResolution = new StiComboBox();
		labelResolution = new StiLabelInfo();
		checkBoxContinuousPages = new StiCheckBoxInfo();
		labelImageFormat = new StiLabelInfo();
		comboBoxImageFormat = new StiComboBox();
		comboBoxImageQuality = new StiComboBox();
		labelPercent = new Label();
		labelImageQuality = new StiLabelInfo();
		checkBoxCompressToArchive = new StiCheckBoxInfo();
		((Control)this).SuspendLayout();
		((Control)labelDpi).Anchor = (AnchorStyles)9;
		((Control)labelDpi).AutoSize = true;
		((Control)labelDpi).ForeColor = Color.DimGray;
		((Control)labelDpi).Location = new Point(283, 36);
		((Control)labelDpi).Name = "labelDpi";
		((Control)labelDpi).Size = new Size(21, 13);
		((Control)labelDpi).TabIndex = 11;
		((Control)labelDpi).Text = "dpi";
		labelDpi.TextAlign = (ContentAlignment)64;
		((Control)comboBoxImageResolution).Anchor = (AnchorStyles)13;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolution).Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(184, 33);
		((ComboBox)comboBoxImageResolution).MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(90, 21);
		((Control)comboBoxImageResolution).TabIndex = 1;
		((Control)labelResolution).AutoSize = true;
		((Control)labelResolution).ForeColor = Color.DimGray;
		((Control)labelResolution).Location = new Point(12, 36);
		((Control)labelResolution).Name = "labelResolution";
		((Control)labelResolution).Padding = new Padding(0, 0, 12, 0);
		((Control)labelResolution).Size = new Size(101, 13);
		((Control)labelResolution).TabIndex = 9;
		((Control)labelResolution).Text = "Image Resolution";
		((Label)labelResolution).TextAlign = (ContentAlignment)16;
		((Control)checkBoxContinuousPages).AutoSize = true;
		((Control)checkBoxContinuousPages).Location = new Point(15, 130);
		((Control)checkBoxContinuousPages).Name = "checkBoxContinuousPages";
		((Control)checkBoxContinuousPages).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxContinuousPages).Size = new Size(124, 17);
		((Control)checkBoxContinuousPages).TabIndex = 4;
		((Control)checkBoxContinuousPages).Text = "Continuous Pages";
		((Control)labelImageFormat).AutoSize = true;
		((Control)labelImageFormat).ForeColor = Color.DimGray;
		((Control)labelImageFormat).Location = new Point(12, 3);
		((Control)labelImageFormat).Name = "labelImageFormat";
		((Control)labelImageFormat).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageFormat).Size = new Size(83, 13);
		((Control)labelImageFormat).TabIndex = 7;
		((Control)labelImageFormat).Text = "Image Format";
		((Label)labelImageFormat).TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageFormat).Anchor = (AnchorStyles)13;
		((Control)comboBoxImageFormat).BackColor = SystemColors.Window;
		((ListControl)comboBoxImageFormat).DisplayMember = "Png";
		comboBoxImageFormat.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageFormat).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageFormat).Items.AddRange(new object[4] { "Jpeg", "Gif", "Bmp", "Png" });
		((Control)comboBoxImageFormat).Location = new Point(184, 0);
		((Control)comboBoxImageFormat).Name = "comboBoxImageFormat";
		((Control)comboBoxImageFormat).Size = new Size(90, 21);
		((Control)comboBoxImageFormat).TabIndex = 0;
		((Control)comboBoxImageQuality).Anchor = (AnchorStyles)13;
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageQuality).Items.AddRange(new object[4] { "25", "50", "75", "100" });
		((Control)comboBoxImageQuality).Location = new Point(184, 66);
		((ComboBox)comboBoxImageQuality).MaxDropDownItems = 20;
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		((Control)comboBoxImageQuality).Size = new Size(90, 21);
		((Control)comboBoxImageQuality).TabIndex = 2;
		((Control)labelPercent).Anchor = (AnchorStyles)9;
		((Control)labelPercent).AutoSize = true;
		((Control)labelPercent).ForeColor = Color.DimGray;
		((Control)labelPercent).Location = new Point(283, 70);
		((Control)labelPercent).Name = "labelPercent";
		((Control)labelPercent).Size = new Size(15, 13);
		((Control)labelPercent).TabIndex = 19;
		((Control)labelPercent).Text = "%";
		labelPercent.TextAlign = (ContentAlignment)64;
		((Control)labelImageQuality).AutoSize = true;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 70);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageQuality).Size = new Size(83, 13);
		((Control)labelImageQuality).TabIndex = 20;
		((Control)labelImageQuality).Text = "Image Quality";
		((Label)labelImageQuality).TextAlign = (ContentAlignment)16;
		((Control)checkBoxCompressToArchive).AutoSize = true;
		((Control)checkBoxCompressToArchive).Location = new Point(15, 101);
		((Control)checkBoxCompressToArchive).Name = "checkBoxCompressToArchive";
		((Control)checkBoxCompressToArchive).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxCompressToArchive).Size = new Size(139, 17);
		((Control)checkBoxCompressToArchive).TabIndex = 3;
		((Control)checkBoxCompressToArchive).Text = "Compress To Archive";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Control)this).Controls.Add((Control)(object)checkBoxCompressToArchive);
		((Control)this).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)this).Controls.Add((Control)(object)labelPercent);
		((Control)this).Controls.Add((Control)(object)labelImageQuality);
		((Control)this).Controls.Add((Control)(object)labelDpi);
		((Control)this).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)this).Controls.Add((Control)(object)labelResolution);
		((Control)this).Controls.Add((Control)(object)checkBoxContinuousPages);
		((Control)this).Controls.Add((Control)(object)labelImageFormat);
		((Control)this).Controls.Add((Control)(object)comboBoxImageFormat);
		((Control)this).Name = "StiHtml5ExportSetupControl";
		((Control)this).Size = new Size(306, 161);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
