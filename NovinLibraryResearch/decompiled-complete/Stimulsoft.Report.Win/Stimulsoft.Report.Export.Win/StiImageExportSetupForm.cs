using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Export.Win;

public class StiImageExportSetupForm : Form
{
	private StiExportFormat exportFormat = StiExportFormat.ImageBmp;

	private IContainer components;

	private CheckBox cbCutEdges;

	private RadioButton rbMonochrome;

	private RadioButton rbGrayscale;

	private RadioButton rbColor;

	private ComboBox cbZoom;

	private StiPagesRangeControl PagesRangeControl;

	private CheckBox cbOpenAfterExport;

	private CheckBox checkBoxMultipleFiles;

	private Button btCancel;

	private Button btOk;

	private Label lbZoom;

	private StiGroupBox gbType;

	private StiGroupBox gbExportSettings;

	private StiGroupLine glButtons;

	private Label lbResolution;

	private ComboBox cbResolution;

	private Label lbDpi;

	private Label lbTiffCompressionScheme;

	private ComboBox cbTiffCompressionScheme;

	private Label lbDitheringType;

	private ComboBox cbDitheringType;

	public StiExportFormat ExportFormat
	{
		set
		{
			exportFormat = value;
		}
	}

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

	public bool MultipleFiles => checkBoxMultipleFiles.Checked;

	public bool MultipleFilesEnabled
	{
		get
		{
			return ((Control)checkBoxMultipleFiles).Enabled;
		}
		set
		{
			((Control)checkBoxMultipleFiles).Enabled = value;
		}
	}

	public bool CutEdges => cbCutEdges.Checked;

	public double ImageZoom
	{
		get
		{
			double num = 100.0;
			try
			{
				string text = ((Control)cbZoom).Text;
				if (text[text.Length - 1] == '%')
				{
					text = text.Substring(0, text.Length - 1);
				}
				num = double.Parse(text);
			}
			catch
			{
				num = 100.0;
			}
			return num / 100.0;
		}
	}

	public int ImageResolution
	{
		get
		{
			int num = 100;
			try
			{
				string text = ((Control)cbResolution).Text;
				StringBuilder stringBuilder = new StringBuilder();
				string text2 = text;
				foreach (char c in text2)
				{
					if (char.IsDigit(c))
					{
						stringBuilder.Append(c);
					}
				}
				return int.Parse(stringBuilder.ToString());
			}
			catch
			{
				return 100;
			}
		}
	}

	public StiImageFormat ImageFormat
	{
		get
		{
			if (rbGrayscale.Checked)
			{
				return StiImageFormat.Grayscale;
			}
			if (rbMonochrome.Checked)
			{
				return StiImageFormat.Monochrome;
			}
			return StiImageFormat.Color;
		}
	}

	public bool MonochromeEnabled
	{
		get
		{
			return ((Control)rbMonochrome).Enabled;
		}
		set
		{
			if (!value && rbMonochrome.Checked)
			{
				rbGrayscale.Checked = true;
			}
			((Control)rbMonochrome).Enabled = value;
		}
	}

	public bool TiffCompressionSchemeEnabled
	{
		get
		{
			return ((Control)cbTiffCompressionScheme).Enabled;
		}
		set
		{
			((Control)lbTiffCompressionScheme).Enabled = value;
			((Control)cbTiffCompressionScheme).Enabled = value;
			if (!value)
			{
				((ListControl)cbTiffCompressionScheme).SelectedIndex = 0;
			}
		}
	}

	public StiTiffCompressionScheme TiffCompressionScheme
	{
		get
		{
			if (((ListControl)cbTiffCompressionScheme).SelectedIndex == 1)
			{
				return StiTiffCompressionScheme.LZW;
			}
			if (((ListControl)cbTiffCompressionScheme).SelectedIndex == 2)
			{
				return StiTiffCompressionScheme.CCITT3;
			}
			if (((ListControl)cbTiffCompressionScheme).SelectedIndex == 3)
			{
				return StiTiffCompressionScheme.CCITT4;
			}
			if (((ListControl)cbTiffCompressionScheme).SelectedIndex == 4)
			{
				return StiTiffCompressionScheme.Rle;
			}
			if (((ListControl)cbTiffCompressionScheme).SelectedIndex == 5)
			{
				return StiTiffCompressionScheme.None;
			}
			return StiTiffCompressionScheme.Default;
		}
	}

	public StiMonochromeDitheringType MonochromeDitheringType
	{
		get
		{
			if (((ListControl)cbDitheringType).SelectedIndex == 0)
			{
				return StiMonochromeDitheringType.None;
			}
			if (((ListControl)cbDitheringType).SelectedIndex == 1)
			{
				return StiMonochromeDitheringType.FloydSteinberg;
			}
			if (((ListControl)cbDitheringType).SelectedIndex == 2)
			{
				return StiMonochromeDitheringType.Ordered;
			}
			return StiMonochromeDitheringType.FloydSteinberg;
		}
	}

	public StiImageExportSetupForm()
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
		cbOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		gbExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		((Control)cbZoom).Handle.ToString();
		((Control)cbZoom).Text = StiSettings.GetStr(((Control)this).Name, "Zoom", ((Control)cbZoom).Text);
		((Control)cbResolution).Handle.ToString();
		((Control)cbResolution).Text = StiSettings.GetStr(((Control)this).Name, "ImageResolution", ((Control)cbResolution).Text);
		cbCutEdges.Checked = StiSettings.GetBool(((Control)this).Name, "CutEdges", cbCutEdges.Checked);
		rbMonochrome.Checked = StiSettings.GetBool(((Control)this).Name, "Monochrome", rbMonochrome.Checked);
		rbGrayscale.Checked = StiSettings.GetBool(((Control)this).Name, "Grayscale", rbGrayscale.Checked);
		rbColor.Checked = StiSettings.GetBool(((Control)this).Name, "Color", rbColor.Checked);
		((Control)cbTiffCompressionScheme).Handle.ToString();
		((ListControl)cbTiffCompressionScheme).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "TiffCompressionScheme", 0);
		((Control)cbDitheringType).Handle.ToString();
		((ListControl)cbDitheringType).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "DitheringType", 1);
		checkBoxMultipleFiles.Checked = StiSettings.GetBool(((Control)this).Name, "MultipleFiles", checkBoxMultipleFiles.Checked);
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Export", "title");
		((Control)lbZoom).Text = StiLocalization.Get("Export", "Zoom");
		((Control)lbResolution).Text = StiLocalization.Get("Export", "ImageResolution");
		((Control)cbCutEdges).Text = StiLocalization.Get("Export", "ImageCutEdges");
		((Control)gbType).Text = StiLocalization.Get("Export", "ImageType");
		((Control)rbColor).Text = StiLocalization.Get("Export", "Color");
		((Control)rbGrayscale).Text = StiLocalization.Get("Export", "ImageGrayscale");
		((Control)rbMonochrome).Text = StiLocalization.Get("Export", "ImageMonochrome");
		((Control)lbDitheringType).Text = StiLocalization.Get("Export", "MonochromeDitheringType");
		((Control)gbExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)cbOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)checkBoxMultipleFiles).Text = StiLocalization.Get("Export", "MultipleFiles");
		((Control)lbTiffCompressionScheme).Text = StiLocalization.Get("Export", "TiffCompressionScheme");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "Zoom", ((Control)cbZoom).Text);
		StiSettings.Set(((Control)this).Name, "ImageResolution", ((Control)cbResolution).Text);
		if (((Control)cbTiffCompressionScheme).Enabled)
		{
			StiSettings.Set(((Control)this).Name, "TiffCompressionScheme", ((ListControl)cbTiffCompressionScheme).SelectedIndex);
		}
		StiSettings.Set(((Control)this).Name, "DitheringType", ((ListControl)cbDitheringType).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "CutEdges", cbCutEdges.Checked);
		StiSettings.Set(((Control)this).Name, "Monochrome", rbMonochrome.Checked);
		StiSettings.Set(((Control)this).Name, "Grayscale", rbGrayscale.Checked);
		StiSettings.Set(((Control)this).Name, "Color", rbColor.Checked);
		StiSettings.Set(((Control)this).Name, "MultipleFiles", checkBoxMultipleFiles.Checked);
		StiSettings.Save();
	}

	private void ThisKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void rbMonochrome_CheckedChanged(object sender, EventArgs e)
	{
		if (rbMonochrome.Checked)
		{
			((Control)lbDitheringType).Enabled = true;
			((Control)cbDitheringType).Enabled = true;
		}
		else
		{
			((Control)lbDitheringType).Enabled = false;
			((Control)cbDitheringType).Enabled = false;
		}
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		string url = null;
		switch (exportFormat)
		{
		case StiExportFormat.ImageBmp:
			url = "user-manual/exports_images_bmp.htm";
			break;
		case StiExportFormat.ImageGif:
			url = "user-manual/exports_images_gif.htm";
			break;
		case StiExportFormat.ImagePng:
			url = "user-manual/exports_images_png.htm";
			break;
		case StiExportFormat.ImageTiff:
			url = "user-manual/exports_images_tiff.htm";
			break;
		case StiExportFormat.ImageJpeg:
			url = "user-manual/exports_images_jpeg.htm";
			break;
		case StiExportFormat.ImagePcx:
			url = "user-manual/exports_images_pcx.htm";
			break;
		case StiExportFormat.ImageEmf:
			url = "user-manual/exports_images_emf.htm";
			break;
		case StiExportFormat.ImageSvg:
			url = "user-manual/exports_images_svg.htm";
			break;
		case StiExportFormat.ImageSvgz:
			url = "user-manual/exports_images_compressed_svg.htm";
			break;
		}
		StiHelpProvider.ShowHelpViewer(url);
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
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_0e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8c: Expected O, but got Unknown
		btCancel = new Button();
		btOk = new Button();
		cbCutEdges = new CheckBox();
		rbMonochrome = new RadioButton();
		rbGrayscale = new RadioButton();
		rbColor = new RadioButton();
		lbZoom = new Label();
		gbType = new StiGroupBox();
		cbZoom = new ComboBox();
		gbExportSettings = new StiGroupBox();
		lbDitheringType = new Label();
		cbDitheringType = new ComboBox();
		lbTiffCompressionScheme = new Label();
		cbTiffCompressionScheme = new ComboBox();
		lbDpi = new Label();
		lbResolution = new Label();
		cbResolution = new ComboBox();
		checkBoxMultipleFiles = new CheckBox();
		cbOpenAfterExport = new CheckBox();
		glButtons = new StiGroupLine();
		PagesRangeControl = new StiPagesRangeControl();
		((Control)gbType).SuspendLayout();
		((Control)gbExportSettings).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(240, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(160, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)cbCutEdges).AutoSize = true;
		((Control)cbCutEdges).ForeColor = SystemColors.ControlText;
		((Control)cbCutEdges).Location = new Point(8, 68);
		((Control)cbCutEdges).Name = "cbCutEdges";
		((Control)cbCutEdges).Size = new Size(74, 17);
		((Control)cbCutEdges).TabIndex = 2;
		((Control)cbCutEdges).Text = "Cut edges";
		((Control)rbMonochrome).AutoSize = true;
		((Control)rbMonochrome).ForeColor = SystemColors.ControlText;
		((Control)rbMonochrome).Location = new Point(8, 64);
		((Control)rbMonochrome).Name = "rbMonochrome";
		((Control)rbMonochrome).Size = new Size(87, 17);
		((Control)rbMonochrome).TabIndex = 2;
		((Control)rbMonochrome).Text = "Monochrome";
		rbMonochrome.CheckedChanged += rbMonochrome_CheckedChanged;
		((Control)rbGrayscale).AutoSize = true;
		((Control)rbGrayscale).ForeColor = SystemColors.ControlText;
		((Control)rbGrayscale).Location = new Point(8, 40);
		((Control)rbGrayscale).Name = "rbGrayscale";
		((Control)rbGrayscale).Size = new Size(72, 17);
		((Control)rbGrayscale).TabIndex = 1;
		((Control)rbGrayscale).Text = "Grayscale";
		((Control)rbColor).AutoSize = true;
		rbColor.Checked = true;
		((Control)rbColor).ForeColor = SystemColors.ControlText;
		((Control)rbColor).Location = new Point(8, 16);
		((Control)rbColor).Name = "rbColor";
		((Control)rbColor).Size = new Size(49, 17);
		((Control)rbColor).TabIndex = 0;
		rbColor.TabStop = true;
		((Control)rbColor).Text = "Color";
		((Control)lbZoom).AutoSize = true;
		((Control)lbZoom).ForeColor = Color.DimGray;
		((Control)lbZoom).Location = new Point(5, 19);
		((Control)lbZoom).Name = "lbZoom";
		((Control)lbZoom).Size = new Size(34, 13);
		((Control)lbZoom).TabIndex = 0;
		((Control)lbZoom).Text = "Zoom";
		lbZoom.TextAlign = (ContentAlignment)16;
		((Control)gbType).Controls.Add((Control)(object)rbColor);
		((Control)gbType).Controls.Add((Control)(object)rbGrayscale);
		((Control)gbType).Controls.Add((Control)(object)rbMonochrome);
		((Control)gbType).Location = new Point(8, 116);
		((Control)gbType).Name = "gbType";
		gbType.ResHeight = 0;
		((Control)gbType).Size = new Size(300, 96);
		((Control)gbType).TabIndex = 4;
		((GroupBox)gbType).TabStop = false;
		((Control)gbType).Text = "Type";
		((Control)cbZoom).BackColor = SystemColors.Window;
		((Control)cbZoom).ForeColor = SystemColors.ControlText;
		cbZoom.Items.AddRange(new object[6] { "25%", "50%", "75%", "100%", "125%", "150%" });
		((Control)cbZoom).Location = new Point(181, 16);
		((Control)cbZoom).Name = "cbZoom";
		((Control)cbZoom).Size = new Size(75, 21);
		((Control)cbZoom).TabIndex = 0;
		((Control)cbZoom).Text = "100%";
		gbExportSettings.AllowCollapse = true;
		((Control)gbExportSettings).Controls.Add((Control)(object)lbDitheringType);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbDitheringType);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbTiffCompressionScheme);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbTiffCompressionScheme);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbDpi);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbResolution);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbResolution);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxMultipleFiles);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbZoom);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbZoom);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbCutEdges);
		((Control)gbExportSettings).Controls.Add((Control)(object)gbType);
		((Control)gbExportSettings).Location = new Point(4, 142);
		((Control)gbExportSettings).Name = "gbExportSettings";
		gbExportSettings.ResHeight = 270;
		((Control)gbExportSettings).Size = new Size(316, 274);
		((Control)gbExportSettings).TabIndex = 1;
		((GroupBox)gbExportSettings).TabStop = false;
		((Control)gbExportSettings).Text = "ExportSettings";
		((Control)lbDitheringType).AutoSize = true;
		((Control)lbDitheringType).Enabled = false;
		((Control)lbDitheringType).ForeColor = Color.DimGray;
		((Control)lbDitheringType).Location = new Point(5, 222);
		((Control)lbDitheringType).Name = "lbDitheringType";
		((Control)lbDitheringType).Size = new Size(141, 13);
		((Control)lbDitheringType).TabIndex = 5;
		((Control)lbDitheringType).Text = "Monochrome Dithering Type";
		lbDitheringType.TextAlign = (ContentAlignment)16;
		((Control)cbDitheringType).BackColor = SystemColors.Window;
		cbDitheringType.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbDitheringType).Enabled = false;
		((Control)cbDitheringType).ForeColor = SystemColors.ControlText;
		cbDitheringType.Items.AddRange(new object[3] { "None", "FloydSteinberg", "Ordered" });
		((Control)cbDitheringType).Location = new Point(194, 219);
		((Control)cbDitheringType).Name = "cbDitheringType";
		((Control)cbDitheringType).Size = new Size(114, 21);
		((Control)cbDitheringType).TabIndex = 6;
		((Control)lbTiffCompressionScheme).AutoSize = true;
		((Control)lbTiffCompressionScheme).ForeColor = Color.DimGray;
		((Control)lbTiffCompressionScheme).Location = new Point(5, 246);
		((Control)lbTiffCompressionScheme).Name = "lbTiffCompressionScheme";
		((Control)lbTiffCompressionScheme).Size = new Size(131, 13);
		((Control)lbTiffCompressionScheme).TabIndex = 7;
		((Control)lbTiffCompressionScheme).Text = "TIFF compression scheme";
		lbTiffCompressionScheme.TextAlign = (ContentAlignment)16;
		((Control)cbTiffCompressionScheme).BackColor = SystemColors.Window;
		cbTiffCompressionScheme.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbTiffCompressionScheme).ForeColor = SystemColors.ControlText;
		cbTiffCompressionScheme.Items.AddRange(new object[6] { "Default", "LZW", "CCITT3", "CCITT4", "Rle", "None" });
		((Control)cbTiffCompressionScheme).Location = new Point(194, 243);
		((Control)cbTiffCompressionScheme).Name = "cbTiffCompressionScheme";
		((Control)cbTiffCompressionScheme).Size = new Size(114, 21);
		((Control)cbTiffCompressionScheme).TabIndex = 8;
		((Control)lbDpi).AutoSize = true;
		((Control)lbDpi).ForeColor = Color.DimGray;
		((Control)lbDpi).Location = new Point(262, 43);
		((Control)lbDpi).Name = "lbDpi";
		((Control)lbDpi).Size = new Size(21, 13);
		((Control)lbDpi).TabIndex = 4;
		((Control)lbDpi).Text = "dpi";
		lbDpi.TextAlign = (ContentAlignment)16;
		((Control)lbResolution).AutoSize = true;
		((Control)lbResolution).ForeColor = Color.DimGray;
		((Control)lbResolution).Location = new Point(5, 43);
		((Control)lbResolution).Name = "lbResolution";
		((Control)lbResolution).Size = new Size(89, 13);
		((Control)lbResolution).TabIndex = 2;
		((Control)lbResolution).Text = "Image Resolution";
		lbResolution.TextAlign = (ContentAlignment)16;
		((Control)cbResolution).BackColor = SystemColors.Window;
		((Control)cbResolution).ForeColor = SystemColors.ControlText;
		cbResolution.Items.AddRange(new object[7] { "50", "75", "100", "150", "200", "300", "600" });
		((Control)cbResolution).Location = new Point(181, 40);
		((Control)cbResolution).Name = "cbResolution";
		((Control)cbResolution).Size = new Size(75, 21);
		((Control)cbResolution).TabIndex = 1;
		((Control)cbResolution).Text = "100";
		((Control)checkBoxMultipleFiles).AutoSize = true;
		((Control)checkBoxMultipleFiles).ForeColor = SystemColors.ControlText;
		((Control)checkBoxMultipleFiles).Location = new Point(8, 92);
		((Control)checkBoxMultipleFiles).Name = "checkBoxMultipleFiles";
		((Control)checkBoxMultipleFiles).Size = new Size(86, 17);
		((Control)checkBoxMultipleFiles).TabIndex = 3;
		((Control)checkBoxMultipleFiles).Text = "Multiple Files";
		((Control)cbOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)cbOpenAfterExport).AutoSize = true;
		((Control)cbOpenAfterExport).Location = new Point(12, 426);
		((Control)cbOpenAfterExport).Name = "cbOpenAfterExport";
		((Control)cbOpenAfterExport).Size = new Size(104, 17);
		((Control)cbOpenAfterExport).TabIndex = 2;
		((Control)cbOpenAfterExport).Text = "OpenAfterExport";
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 446);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(324, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(4, 4);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(316, 136);
		((Control)PagesRangeControl).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(324, 492);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)gbExportSettings);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Control)this).Controls.Add((Control)(object)cbOpenAfterExport);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiImageExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiImageExportSetupForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += ThisClosed;
		((Control)this).KeyDown += new KeyEventHandler(ThisKeyDown);
		((Control)gbType).ResumeLayout(false);
		((Control)gbType).PerformLayout();
		((Control)gbExportSettings).ResumeLayout(false);
		((Control)gbExportSettings).PerformLayout();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
