using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Win;

[ToolboxItem(false)]
public class StiHtmlExportSetupControl : UserControl
{
	private IContainer components;

	internal StiComboBox comboBoxExportMode;

	private StiLabelInfo labelScale;

	private StiLabelInfo labelImageFormat;

	private StiLabelInfo labelExportMode;

	internal StiDropDownBox comboBoxScale;

	internal StiComboBox comboBoxImageFormat;

	internal StiCheckBoxInfo checkBoxAddPageBreaks;

	internal StiCheckBoxInfo checkBoxCompressToArchive;

	internal StiCheckBoxInfo checkBoxEmbeddedImageData;

	internal Label labelPercent;

	internal bool AddPageBreaks => ((CheckBox)checkBoxAddPageBreaks).Checked;

	internal float Zoom
	{
		get
		{
			if (float.TryParse(((Control)comboBoxScale).Text, out var result))
			{
				return result / 100f;
			}
			return 1f;
		}
	}

	internal ImageFormat ImageFormat => (ImageFormat)(((Control)comboBoxImageFormat).Text switch
	{
		"Bmp" => ImageFormat.Bmp, 
		"Gif" => ImageFormat.Gif, 
		"Png" => ImageFormat.Png, 
		_ => ImageFormat.Jpeg, 
	});

	internal StiHtmlExportMode ExportMode
	{
		get
		{
			if (((ListControl)comboBoxExportMode).SelectedIndex == 1)
			{
				return StiHtmlExportMode.Span;
			}
			if (((ListControl)comboBoxExportMode).SelectedIndex == 2)
			{
				return StiHtmlExportMode.Div;
			}
			return StiHtmlExportMode.Table;
		}
	}

	internal bool CompressToArchive => ((CheckBox)checkBoxCompressToArchive).Checked;

	internal bool UseEmbeddedImages => ((CheckBox)checkBoxEmbeddedImageData).Checked;

	internal bool EmbeddedImageDataVisible
	{
		set
		{
			((Control)checkBoxEmbeddedImageData).Visible = value;
			((Control)this).Height = (value ? ((Control)checkBoxEmbeddedImageData).Bottom : ((Control)checkBoxAddPageBreaks).Bottom) + StiScale.YYI(12.0);
		}
	}

	public StiHtmlExportSetupControl()
	{
		InitializeComponent();
		Localize();
		((ListControl)comboBoxImageFormat).SelectedIndex = 0;
		((ListControl)comboBoxExportMode).SelectedIndex = 0;
		LocalizeSuperToolTip();
	}

	private void Localize()
	{
		((Control)labelImageFormat).Text = Loc.GetCleaned("Export", "ImageFormat").Replace("&", "_");
		((Control)labelScale).Text = Loc.GetCleaned("Export", "Scale").Replace("&", "_");
		((Control)labelExportMode).Text = Loc.GetCleaned("Export", "ExportMode").Replace("&", "_");
		((Control)checkBoxCompressToArchive).Text = StiLocalization.Get("Export", "CompressToArchive");
		((Control)checkBoxEmbeddedImageData).Text = StiLocalization.Get("Export", "EmbeddedImageData");
		((Control)checkBoxAddPageBreaks).Text = StiLocalization.Get("Export", "AddPageBreaks").Replace("&", "_");
	}

	private void LocalizeSuperToolTip()
	{
		labelImageFormat.SetToolTip(StiLocalization.Get("HelpViewer", "ImageFormat"));
		labelScale.SetToolTip(StiLocalization.Get("HelpViewer", "ScaleHtml"));
		labelExportMode.SetToolTip(StiLocalization.Get("HelpViewer", "ExportModeHtml"));
		checkBoxCompressToArchive.SetToolTip(StiLocalization.Get("HelpViewer", "CompressToArchive"));
		checkBoxEmbeddedImageData.SetToolTip(StiLocalization.Get("HelpViewer", "EmbeddedImageData"));
		checkBoxAddPageBreaks.SetToolTip(StiLocalization.Get("HelpViewer", "AddPageBreaks"));
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		comboBoxExportMode = new StiComboBox();
		comboBoxScale = new StiDropDownBox();
		comboBoxImageFormat = new StiComboBox();
		checkBoxEmbeddedImageData = new StiCheckBoxInfo();
		checkBoxCompressToArchive = new StiCheckBoxInfo();
		checkBoxAddPageBreaks = new StiCheckBoxInfo();
		labelScale = new StiLabelInfo();
		labelImageFormat = new StiLabelInfo();
		labelExportMode = new StiLabelInfo();
		labelPercent = new Label();
		((Control)this).SuspendLayout();
		((Control)comboBoxExportMode).Anchor = (AnchorStyles)13;
		((Control)comboBoxExportMode).BackColor = SystemColors.Window;
		((ListControl)comboBoxExportMode).DisplayMember = "Jpeg";
		comboBoxExportMode.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxExportMode).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxExportMode).Items.AddRange(new object[3] { "Table", "Span", "Div" });
		((Control)comboBoxExportMode).Location = new Point(184, 66);
		((Control)comboBoxExportMode).Name = "comboBoxExportMode";
		((Control)comboBoxExportMode).Size = new Size(90, 21);
		((Control)comboBoxExportMode).TabIndex = 2;
		((Control)comboBoxScale).Anchor = (AnchorStyles)13;
		((Control)comboBoxScale).BackColor = Color.White;
		comboBoxScale.DrawMode = (DrawMode)2;
		((Control)comboBoxScale).ForeColor = SystemColors.ControlText;
		comboBoxScale.IsEditable = true;
		comboBoxScale.Items.AddRange(new object[7] { "25", "50", "75", "100", "125", "150", "200" });
		((Control)comboBoxScale).Location = new Point(184, 33);
		((Control)comboBoxScale).Name = "comboBoxScale";
		comboBoxScale.SelectedIndex = -1;
		comboBoxScale.SelectedItem = null;
		comboBoxScale.SelectedValue = null;
		((Control)comboBoxScale).Size = new Size(90, 21);
		((Control)comboBoxScale).TabIndex = 1;
		((Control)comboBoxScale).Text = "100%";
		((Control)comboBoxImageFormat).Anchor = (AnchorStyles)13;
		((Control)comboBoxImageFormat).BackColor = SystemColors.Window;
		((ListControl)comboBoxImageFormat).DisplayMember = "Jpeg";
		comboBoxImageFormat.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageFormat).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageFormat).Items.AddRange(new object[4] { "Jpeg", "Gif", "Bmp", "Png" });
		((Control)comboBoxImageFormat).Location = new Point(184, 0);
		((Control)comboBoxImageFormat).Name = "comboBoxImageFormat";
		((Control)comboBoxImageFormat).Size = new Size(90, 21);
		((Control)comboBoxImageFormat).TabIndex = 0;
		((Control)checkBoxEmbeddedImageData).AutoSize = true;
		((Control)checkBoxEmbeddedImageData).Location = new Point(15, 159);
		((Control)checkBoxEmbeddedImageData).Name = "checkBoxEmbeddedImageData";
		((Control)checkBoxEmbeddedImageData).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxEmbeddedImageData).Size = new Size(147, 17);
		((Control)checkBoxEmbeddedImageData).TabIndex = 5;
		((Control)checkBoxEmbeddedImageData).Text = "Embedded Image Data";
		((Control)checkBoxCompressToArchive).AutoSize = true;
		((Control)checkBoxCompressToArchive).Location = new Point(15, 101);
		((Control)checkBoxCompressToArchive).Name = "checkBoxCompressToArchive";
		((Control)checkBoxCompressToArchive).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxCompressToArchive).Size = new Size(139, 17);
		((Control)checkBoxCompressToArchive).TabIndex = 3;
		((Control)checkBoxCompressToArchive).Text = "Compress To Archive";
		((Control)checkBoxAddPageBreaks).AutoSize = true;
		((Control)checkBoxAddPageBreaks).Location = new Point(15, 130);
		((Control)checkBoxAddPageBreaks).Name = "checkBoxAddPageBreaks";
		((Control)checkBoxAddPageBreaks).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxAddPageBreaks).Size = new Size(121, 17);
		((Control)checkBoxAddPageBreaks).TabIndex = 4;
		((Control)checkBoxAddPageBreaks).Text = "Add Page Breaks";
		((Control)labelScale).AutoSize = true;
		((Control)labelScale).ForeColor = Color.DimGray;
		((Control)labelScale).Location = new Point(12, 37);
		((Control)labelScale).Name = "labelScale";
		((Control)labelScale).Padding = new Padding(0, 0, 12, 0);
		((Control)labelScale).Size = new Size(46, 13);
		((Control)labelScale).TabIndex = 10;
		((Control)labelScale).Text = "Scale";
		((Label)labelScale).TextAlign = (ContentAlignment)16;
		((Control)labelImageFormat).AutoSize = true;
		((Control)labelImageFormat).ForeColor = Color.DimGray;
		((Control)labelImageFormat).Location = new Point(12, 3);
		((Control)labelImageFormat).Name = "labelImageFormat";
		((Control)labelImageFormat).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageFormat).Size = new Size(83, 13);
		((Control)labelImageFormat).TabIndex = 11;
		((Control)labelImageFormat).Text = "Image Format";
		((Label)labelImageFormat).TextAlign = (ContentAlignment)16;
		((Control)labelExportMode).AutoSize = true;
		((Control)labelExportMode).ForeColor = Color.DimGray;
		((Control)labelExportMode).Location = new Point(12, 69);
		((Control)labelExportMode).Name = "labelExportMode";
		((Control)labelExportMode).Padding = new Padding(0, 0, 12, 0);
		((Control)labelExportMode).Size = new Size(79, 13);
		((Control)labelExportMode).TabIndex = 12;
		((Control)labelExportMode).Text = "Export Mode";
		((Label)labelExportMode).TextAlign = (ContentAlignment)16;
		((Control)labelPercent).Anchor = (AnchorStyles)9;
		((Control)labelPercent).AutoSize = true;
		((Control)labelPercent).ForeColor = Color.DimGray;
		((Control)labelPercent).Location = new Point(283, 37);
		((Control)labelPercent).Name = "labelPercent";
		((Control)labelPercent).Size = new Size(15, 13);
		((Control)labelPercent).TabIndex = 20;
		((Control)labelPercent).Text = "%";
		labelPercent.TextAlign = (ContentAlignment)64;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)labelPercent);
		((Control)this).Controls.Add((Control)(object)checkBoxEmbeddedImageData);
		((Control)this).Controls.Add((Control)(object)checkBoxCompressToArchive);
		((Control)this).Controls.Add((Control)(object)checkBoxAddPageBreaks);
		((Control)this).Controls.Add((Control)(object)comboBoxExportMode);
		((Control)this).Controls.Add((Control)(object)labelScale);
		((Control)this).Controls.Add((Control)(object)labelImageFormat);
		((Control)this).Controls.Add((Control)(object)labelExportMode);
		((Control)this).Controls.Add((Control)(object)comboBoxScale);
		((Control)this).Controls.Add((Control)(object)comboBoxImageFormat);
		((Control)this).Name = "StiHtmlExportSetupControl";
		((Control)this).Size = new Size(306, 189);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
