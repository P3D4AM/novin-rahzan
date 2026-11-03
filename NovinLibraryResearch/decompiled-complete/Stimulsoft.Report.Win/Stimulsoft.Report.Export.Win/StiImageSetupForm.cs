using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiImageSetupForm : Form
{
	private ComboBoxItem comboBoxItemColor = new ComboBoxItem();

	private ComboBoxItem comboBoxItemGrayscale = new ComboBoxItem();

	private ComboBoxItem comboBoxItemMonochrome = new ComboBoxItem();

	private IContainer components;

	private Panel panelOpenAfterExport;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiGroupBox groupBoxExportSettings;

	private Panel panelType;

	private StiLabelInfo labelType;

	private StiComboBox comboBoxType;

	private StiPagesRangeControl pagesRange;

	private StiGroupLine glButtons;

	private Button buttonCancel;

	private Button buttonOk;

	private StiLabelInfo labelScale;

	private StiDropDownBox comboBoxScale;

	private Label label6;

	private StiComboBox comboBoxImageResolution;

	private StiLabelInfo labelResolution;

	private StiComboBox comboBoxImageType;

	private StiLabelInfo labelImageType;

	private StiLabelInfo labelDitheringType;

	private StiComboBox comboBoxDitheringType;

	private StiLabelInfo labelTiffCompressionScheme;

	private StiComboBox comboBoxTiffCompressionScheme;

	private StiCheckBoxInfo checkBoxMultipleFiles;

	private StiCheckBoxInfo checkBoxCutEdges;

	internal StiCheckBoxInfo checkBoxCompressToArchive;

	private Label label1;

	public StiImageType ImageType
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (StiImageType)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxType).SelectedItem).Tag;
		}
		set
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			foreach (object item in ((ComboBox)comboBoxType).Items)
			{
				if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals((int)value))
				{
					break;
				}
				num++;
			}
			((ListControl)comboBoxType).SelectedIndex = ((num < ((ComboBox)comboBoxType).Items.Count) ? num : 0);
		}
	}

	public StiPagesRange PagesRange => pagesRange.GetPagesRange();

	public bool OpenAfterExport => ((CheckBox)checkBoxOpenAfterExport).Checked;

	public int CurrentPage
	{
		get
		{
			return pagesRange.CurrentPage;
		}
		set
		{
			pagesRange.CurrentPage = value;
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

	public bool MultipleFiles => ((CheckBox)checkBoxMultipleFiles).Checked;

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

	public bool CutEdges => ((CheckBox)checkBoxCutEdges).Checked;

	public double ImageZoom
	{
		get
		{
			if (double.TryParse(((Control)comboBoxScale).Text, out var result))
			{
				return result / 100.0;
			}
			return 1.0;
		}
	}

	public int ImageResolution
	{
		get
		{
			int num = 100;
			try
			{
				string text = ((Control)comboBoxImageResolution).Text;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (char item in text.Where((char ch) => char.IsDigit(ch)))
				{
					stringBuilder.Append(item);
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
			if (((ListControl)comboBoxImageType).SelectedIndex == 1)
			{
				return StiImageFormat.Grayscale;
			}
			if (((ListControl)comboBoxImageType).SelectedIndex == 2)
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
			return ((BaseItem)comboBoxItemMonochrome).Enabled;
		}
		set
		{
			if (!value && ((ListControl)comboBoxImageType).SelectedIndex == 2)
			{
				((ListControl)comboBoxImageType).SelectedIndex = 1;
			}
			((BaseItem)comboBoxItemMonochrome).Enabled = value;
		}
	}

	public bool TiffCompressionSchemeEnabled
	{
		get
		{
			return ((Control)comboBoxTiffCompressionScheme).Enabled;
		}
		set
		{
			((Control)labelTiffCompressionScheme).Enabled = value;
			((Control)comboBoxTiffCompressionScheme).Enabled = value;
			if (!value)
			{
				((ListControl)comboBoxTiffCompressionScheme).SelectedIndex = 0;
			}
		}
	}

	public StiTiffCompressionScheme TiffCompressionScheme
	{
		get
		{
			if (((ListControl)comboBoxTiffCompressionScheme).SelectedIndex == 1)
			{
				return StiTiffCompressionScheme.LZW;
			}
			if (((ListControl)comboBoxTiffCompressionScheme).SelectedIndex == 2)
			{
				return StiTiffCompressionScheme.CCITT3;
			}
			if (((ListControl)comboBoxTiffCompressionScheme).SelectedIndex == 3)
			{
				return StiTiffCompressionScheme.CCITT4;
			}
			if (((ListControl)comboBoxTiffCompressionScheme).SelectedIndex == 4)
			{
				return StiTiffCompressionScheme.Rle;
			}
			if (((ListControl)comboBoxTiffCompressionScheme).SelectedIndex == 5)
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
			if (((ListControl)comboBoxDitheringType).SelectedIndex == 0)
			{
				return StiMonochromeDitheringType.None;
			}
			if (((ListControl)comboBoxDitheringType).SelectedIndex == 1)
			{
				return StiMonochromeDitheringType.FloydSteinberg;
			}
			if (((ListControl)comboBoxDitheringType).SelectedIndex == 2)
			{
				return StiMonochromeDitheringType.Ordered;
			}
			return StiMonochromeDitheringType.FloydSteinberg;
		}
	}

	public bool CompressToArchive => ((CheckBox)checkBoxCompressToArchive).Checked;

	public StiImageSetupForm()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
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
		Build();
		Localize();
		DoScaling();
		if (StiOptions.Viewer.Windows.Exports.ShowBmp)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "BMP",
				Tag = StiImageType.Bmp
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowGif)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "GIF",
				Tag = StiImageType.Gif
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowJpeg)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "JPEG",
				Tag = StiImageType.Jpeg
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowPcx)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "PCX",
				Tag = StiImageType.Pcx
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowPng)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "PNG",
				Tag = StiImageType.Png
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowTiff)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "TIFF",
				Tag = StiImageType.Tiff
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowMetafile)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "EMF",
				Tag = StiImageType.Emf
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowSvg)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "SVG",
				Tag = StiImageType.Svg
			});
		}
		if (StiOptions.Viewer.Windows.Exports.ShowSvgz)
		{
			((ComboBox)comboBoxType).Items.Add((object)new ComboBoxItem
			{
				Text = "SVGZ",
				Tag = StiImageType.Svgz
			});
		}
		((Form)this).Load += This_Load;
		((Form)this).Closed += This_Closed;
		((ComboBox)comboBoxImageType).SelectedIndexChanged += ComboBoxImageType_SelectedIndexChanged;
		((ListControl)comboBoxImageType).SelectedIndex = 0;
		((ComboBox)comboBoxType).SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
		((ListControl)comboBoxType).SelectedIndex = 0;
		((ListControl)comboBoxDitheringType).SelectedIndex = 0;
		((ListControl)comboBoxImageResolution).SelectedIndex = 5;
		LocalizeSuperToolTip();
		StiSettings.Load();
		((CheckBox)checkBoxCompressToArchive).Checked = StiSettings.GetBool(((Control)this).Name, "CompressToArchive", nullValue: false);
		((CheckBox)checkBoxOpenAfterExport).Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		comboBoxScale.SelectedIndex = StiSettings.GetInt(((Control)this).Name, "Scale", comboBoxScale.SelectedIndex);
		((ListControl)comboBoxImageResolution).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ImageResolution", ((ListControl)comboBoxImageResolution).SelectedIndex);
		((CheckBox)checkBoxCutEdges).Checked = StiSettings.GetBool(((Control)this).Name, "CutEdges", ((CheckBox)checkBoxCutEdges).Checked);
		((ListControl)comboBoxImageType).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "ImageType", ((ListControl)comboBoxImageType).SelectedIndex);
		((ListControl)comboBoxTiffCompressionScheme).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "TiffCompressionScheme", 0);
		((ListControl)comboBoxDitheringType).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "DitheringType", 1);
		if (ImageType == StiImageType.Tiff)
		{
			((CheckBox)checkBoxMultipleFiles).Checked = StiSettings.GetBool(((Control)this).Name, "MultipleFiles", ((CheckBox)checkBoxMultipleFiles).Checked);
		}
		groupBoxExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings");
		int @int = StiSettings.GetInt(((Control)this).Name, "Type", 3);
		int num = 0;
		foreach (object item in ((ComboBox)comboBoxType).Items)
		{
			if (((int)((BaseItem)(ComboBoxItem)item).Tag).Equals(@int))
			{
				break;
			}
			num++;
		}
		((ListControl)comboBoxType).SelectedIndex = ((num < ((ComboBox)comboBoxType).Items.Count) ? num : 0);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_images.htm");
	}

	private void This_Load(object sender, EventArgs e)
	{
		CheckState();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "CompressToArchive", ((CheckBox)checkBoxCompressToArchive).Checked);
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		StiSettings.Set(((Control)this).Name, "Scale", comboBoxScale.SelectedIndex);
		StiSettings.Set(((Control)this).Name, "Type", (int)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxType).SelectedItem).Tag);
		StiSettings.Set(((Control)this).Name, "ImageResolution", ((ListControl)comboBoxImageResolution).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "CutEdges", ((CheckBox)checkBoxCutEdges).Checked);
		StiSettings.Set(((Control)this).Name, "ImageType", ((ListControl)comboBoxImageType).SelectedIndex);
		if (((Control)comboBoxTiffCompressionScheme).Enabled)
		{
			StiSettings.Set(((Control)this).Name, "TiffCompressionScheme", ((ListControl)comboBoxTiffCompressionScheme).SelectedIndex);
		}
		StiSettings.Set(((Control)this).Name, "DitheringType", ((ListControl)comboBoxDitheringType).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "MultipleFiles", ((CheckBox)checkBoxMultipleFiles).Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", groupBoxExportSettings.Collapsed);
		StiSettings.Save();
	}

	private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckState();
	}

	private void ComboBoxImageType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)comboBoxImageType).SelectedIndex == 2)
		{
			((Control)labelDitheringType).Enabled = true;
			((Control)comboBoxDitheringType).Enabled = true;
		}
		else
		{
			((Control)labelDitheringType).Enabled = false;
			((Control)comboBoxDitheringType).Enabled = false;
		}
	}

	private void LocalizeSuperToolTip()
	{
		labelType.SetToolTip(StiLocalization.Get("HelpViewer", "TypeExport"));
		labelScale.SetToolTip(StiLocalization.Get("HelpViewer", "ScaleImage"));
		labelResolution.SetToolTip(StiLocalization.Get("HelpViewer", "ImageResolution"));
		labelImageType.SetToolTip(StiLocalization.Get("HelpViewer", "ImageType"));
		labelDitheringType.SetToolTip(StiLocalization.Get("HelpViewer", "DitheringType"));
		labelTiffCompressionScheme.SetToolTip(StiLocalization.Get("HelpViewer", "TiffCompressionScheme"));
		checkBoxCompressToArchive.SetToolTip(StiLocalization.Get("HelpViewer", "CompressToArchive"));
		checkBoxCutEdges.SetToolTip(StiLocalization.Get("HelpViewer", "CutEdges"));
		checkBoxMultipleFiles.SetToolTip(StiLocalization.Get("HelpViewer", "MultipleFiles"));
		checkBoxOpenAfterExport.SetToolTip(StiLocalization.Get("HelpViewer", "OpenAfterExport"));
	}

	private void Localize()
	{
		((Control)this).Text = string.Format("{0} - {1}", StiLocalization.Get("Export", "ExportTypeImageFile").Replace("...", ""), StiLocalization.Get("Export", "title").Replace("&", "_"));
		((Control)labelType).Text = Loc.GetCleaned("Export", "Type");
		((Control)labelScale).Text = Loc.GetCleaned("Export", "Scale").Replace("&", "_");
		((Control)labelResolution).Text = Loc.GetCleaned("Export", "ImageResolution");
		((Control)labelImageType).Text = Loc.GetCleaned("Export", "ImageType").Replace("&", "_");
		((Control)labelDitheringType).Text = Loc.GetCleaned("Export", "MonochromeDitheringType");
		((Control)labelTiffCompressionScheme).Text = Loc.GetCleaned("Export", "TiffCompressionScheme");
		((Control)checkBoxCutEdges).Text = StiLocalization.Get("Export", "ImageCutEdges").Replace("&", "_");
		((Control)checkBoxMultipleFiles).Text = StiLocalization.Get("Export", "MultipleFiles").Replace("&", "_");
		((BaseItem)comboBoxItemColor).Text = StiLocalization.Get("Export", "Color").Replace("&", "_");
		((BaseItem)comboBoxItemGrayscale).Text = StiLocalization.Get("Export", "ImageGrayscale").Replace("&", "_");
		((BaseItem)comboBoxItemMonochrome).Text = StiLocalization.Get("Export", "ImageMonochrome").Replace("&", "_");
		((Control)checkBoxCompressToArchive).Text = StiLocalization.Get("Export", "CompressToArchive");
		((Control)groupBoxExportSettings).Text = StiLocalization.Get("Export", "Settings").Replace("&", "_");
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport").Replace("&", "_");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		((Control)comboBoxScale).Width = ((Control)comboBoxImageResolution).Width;
		((ComboBox)comboBoxType).DropDownHeight = StiScaleUI.I(200);
	}

	private void CheckState()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		ImageType = (StiImageType)((BaseItem)(ComboBoxItem)((ComboBox)comboBoxType).SelectedItem).Tag;
		if (ImageType != StiImageType.Tiff)
		{
			((CheckBox)checkBoxMultipleFiles).Checked = true;
		}
		MultipleFilesEnabled = ImageType == StiImageType.Tiff;
		TiffCompressionSchemeEnabled = ImageType == StiImageType.Tiff;
		MonochromeEnabled = ImageType != StiImageType.Emf && ImageType != StiImageType.Svg && ImageType != StiImageType.Svgz;
	}

	private void Build()
	{
		((ComboBox)comboBoxImageType).Items.Add((object)comboBoxItemColor);
		((ComboBox)comboBoxImageType).Items.Add((object)comboBoxItemGrayscale);
		((ComboBox)comboBoxImageType).Items.Add((object)comboBoxItemMonochrome);
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
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_103e: Unknown result type (might be due to invalid IL or missing references)
		//IL_11be: Unknown result type (might be due to invalid IL or missing references)
		panelOpenAfterExport = new Panel();
		groupBoxExportSettings = new StiGroupBox();
		comboBoxDitheringType = new StiComboBox();
		comboBoxTiffCompressionScheme = new StiComboBox();
		label6 = new Label();
		comboBoxImageType = new StiComboBox();
		comboBoxImageResolution = new StiComboBox();
		comboBoxScale = new StiDropDownBox();
		panelType = new Panel();
		comboBoxType = new StiComboBox();
		glButtons = new StiGroupLine();
		buttonCancel = new Button();
		buttonOk = new Button();
		label1 = new Label();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		checkBoxCompressToArchive = new StiCheckBoxInfo();
		checkBoxMultipleFiles = new StiCheckBoxInfo();
		checkBoxCutEdges = new StiCheckBoxInfo();
		labelDitheringType = new StiLabelInfo();
		labelTiffCompressionScheme = new StiLabelInfo();
		labelImageType = new StiLabelInfo();
		labelResolution = new StiLabelInfo();
		labelScale = new StiLabelInfo();
		labelType = new StiLabelInfo();
		pagesRange = new StiPagesRangeControl();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)groupBoxExportSettings).SuspendLayout();
		((Control)panelType).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 489);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(356, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		groupBoxExportSettings.AllowCollapse = true;
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkBoxCompressToArchive);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkBoxMultipleFiles);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)checkBoxCutEdges);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelDitheringType);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxDitheringType);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelTiffCompressionScheme);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxTiffCompressionScheme);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label1);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)label6);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageType);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxImageResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelImageType);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelResolution);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)labelScale);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)comboBoxScale);
		((Control)groupBoxExportSettings).Controls.Add((Control)(object)panelType);
		((Control)groupBoxExportSettings).Location = new Point(12, 175);
		((Control)groupBoxExportSettings).Name = "groupBoxExportSettings";
		groupBoxExportSettings.ResHeight = 122;
		((Control)groupBoxExportSettings).Size = new Size(330, 313);
		((Control)groupBoxExportSettings).TabIndex = 1;
		((GroupBox)groupBoxExportSettings).TabStop = false;
		((Control)groupBoxExportSettings).Text = "ExportSettings";
		((Control)comboBoxDitheringType).Anchor = (AnchorStyles)13;
		((Control)comboBoxDitheringType).BackColor = SystemColors.Window;
		comboBoxDitheringType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxDitheringType).Enabled = false;
		((Control)comboBoxDitheringType).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxDitheringType).Items.AddRange(new object[3] { "None", "FloydSteinberg", "Ordered" });
		((Control)comboBoxDitheringType).Location = new Point(186, 157);
		((Control)comboBoxDitheringType).Name = "comboBoxDitheringType";
		((Control)comboBoxDitheringType).Size = new Size(130, 21);
		((Control)comboBoxDitheringType).TabIndex = 4;
		((Control)comboBoxTiffCompressionScheme).Anchor = (AnchorStyles)13;
		((Control)comboBoxTiffCompressionScheme).BackColor = SystemColors.Window;
		comboBoxTiffCompressionScheme.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxTiffCompressionScheme).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxTiffCompressionScheme).Items.AddRange(new object[6] { "Default", "LZW", "CCITT3", "CCITT4", "Rle", "None" });
		((Control)comboBoxTiffCompressionScheme).Location = new Point(186, 190);
		((Control)comboBoxTiffCompressionScheme).Name = "comboBoxTiffCompressionScheme";
		((Control)comboBoxTiffCompressionScheme).Size = new Size(130, 21);
		((Control)comboBoxTiffCompressionScheme).TabIndex = 5;
		((Control)label6).Anchor = (AnchorStyles)9;
		((Control)label6).ForeColor = Color.DimGray;
		((Control)label6).Location = new Point(290, 91);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(25, 21);
		((Control)label6).TabIndex = 15;
		((Control)label6).Text = "dpi";
		label6.TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageType).Anchor = (AnchorStyles)13;
		((Control)comboBoxImageType).BackColor = SystemColors.Window;
		comboBoxImageType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageType).ForeColor = SystemColors.ControlText;
		((Control)comboBoxImageType).Location = new Point(186, 124);
		((ComboBox)comboBoxImageType).MaxDropDownItems = 20;
		((Control)comboBoxImageType).Name = "comboBoxImageType";
		((Control)comboBoxImageType).Size = new Size(130, 21);
		((Control)comboBoxImageType).TabIndex = 3;
		((Control)comboBoxImageResolution).Anchor = (AnchorStyles)13;
		((Control)comboBoxImageResolution).BackColor = SystemColors.Window;
		comboBoxImageResolution.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageResolution).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageResolution).Items.AddRange(new object[9] { "10", "25", "50", "75", "100", "200", "300", "400", "500" });
		((Control)comboBoxImageResolution).Location = new Point(187, 91);
		((ComboBox)comboBoxImageResolution).MaxDropDownItems = 20;
		((Control)comboBoxImageResolution).Name = "comboBoxImageResolution";
		((Control)comboBoxImageResolution).Size = new Size(97, 21);
		((Control)comboBoxImageResolution).TabIndex = 2;
		((Control)comboBoxScale).Anchor = (AnchorStyles)13;
		comboBoxScale.DrawMode = (DrawMode)2;
		comboBoxScale.IsEditable = true;
		comboBoxScale.Items.AddRange(new object[7] { "25", "50", "75", "100", "125", "150", "200" });
		((Control)comboBoxScale).Location = new Point(187, 58);
		((Control)comboBoxScale).Name = "comboBoxScale";
		comboBoxScale.SelectedIndex = -1;
		comboBoxScale.SelectedItem = null;
		comboBoxScale.SelectedValue = null;
		((Control)comboBoxScale).Size = new Size(97, 21);
		((Control)comboBoxScale).TabIndex = 1;
		((Control)comboBoxScale).Text = "100";
		((Control)panelType).Controls.Add((Control)(object)labelType);
		((Control)panelType).Controls.Add((Control)(object)comboBoxType);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(3, 16);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(324, 41);
		((Control)panelType).TabIndex = 0;
		((Control)comboBoxType).Anchor = (AnchorStyles)13;
		((Control)comboBoxType).BackColor = SystemColors.Window;
		comboBoxType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxType).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxType).IntegralHeight = false;
		((Control)comboBoxType).Location = new Point(184, 8);
		((Control)comboBoxType).Name = "comboBoxType";
		((Control)comboBoxType).Size = new Size(130, 21);
		((Control)comboBoxType).TabIndex = 0;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 531);
		((Control)glButtons).Margin = new Padding(0, 3, 3, 3);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(356, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(267, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(186, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(290, 58);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(25, 21);
		((Control)label1).TabIndex = 15;
		((Control)label1).Text = "%";
		label1.TextAlign = (ContentAlignment)16;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 0;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)checkBoxCompressToArchive).AutoSize = true;
		((Control)checkBoxCompressToArchive).Location = new Point(15, 226);
		((Control)checkBoxCompressToArchive).Name = "checkBoxCompressToArchive";
		((Control)checkBoxCompressToArchive).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxCompressToArchive).Size = new Size(139, 17);
		((Control)checkBoxCompressToArchive).TabIndex = 19;
		((Control)checkBoxCompressToArchive).Text = "Compress To Archive";
		((Control)checkBoxMultipleFiles).ForeColor = SystemColors.ControlText;
		((Control)checkBoxMultipleFiles).Location = new Point(15, 284);
		((Control)checkBoxMultipleFiles).Name = "checkBoxMultipleFiles";
		((Control)checkBoxMultipleFiles).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxMultipleFiles).Size = new Size(303, 17);
		((Control)checkBoxMultipleFiles).TabIndex = 7;
		((Control)checkBoxMultipleFiles).Text = "Multiple Files";
		((Control)checkBoxCutEdges).ForeColor = SystemColors.ControlText;
		((Control)checkBoxCutEdges).Location = new Point(15, 255);
		((Control)checkBoxCutEdges).Name = "checkBoxCutEdges";
		((Control)checkBoxCutEdges).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxCutEdges).Size = new Size(303, 17);
		((Control)checkBoxCutEdges).TabIndex = 6;
		((Control)checkBoxCutEdges).Text = "Cut edges";
		((Control)labelDitheringType).Enabled = false;
		((Control)labelDitheringType).ForeColor = Color.DimGray;
		((Control)labelDitheringType).Location = new Point(12, 157);
		((Control)labelDitheringType).Name = "labelDitheringType";
		((Control)labelDitheringType).Padding = new Padding(0, 0, 12, 0);
		((Control)labelDitheringType).Size = new Size(163, 21);
		((Control)labelDitheringType).TabIndex = 16;
		((Control)labelDitheringType).Text = "Monochrome Dithering Type";
		((Label)labelDitheringType).TextAlign = (ContentAlignment)16;
		((Control)labelTiffCompressionScheme).ForeColor = Color.DimGray;
		((Control)labelTiffCompressionScheme).Location = new Point(12, 190);
		((Control)labelTiffCompressionScheme).Name = "labelTiffCompressionScheme";
		((Control)labelTiffCompressionScheme).Padding = new Padding(0, 0, 12, 0);
		((Control)labelTiffCompressionScheme).Size = new Size(163, 21);
		((Control)labelTiffCompressionScheme).TabIndex = 18;
		((Control)labelTiffCompressionScheme).Text = "TIFF compression scheme";
		((Label)labelTiffCompressionScheme).TextAlign = (ContentAlignment)16;
		((Control)labelImageType).ForeColor = Color.DimGray;
		((Control)labelImageType).Location = new Point(12, 124);
		((Control)labelImageType).Name = "labelImageType";
		((Control)labelImageType).Padding = new Padding(0, 0, 12, 0);
		((Control)labelImageType).Size = new Size(163, 21);
		((Control)labelImageType).TabIndex = 13;
		((Control)labelImageType).Text = "Image Type";
		((Label)labelImageType).TextAlign = (ContentAlignment)16;
		((Control)labelResolution).ForeColor = Color.DimGray;
		((Control)labelResolution).Location = new Point(12, 91);
		((Control)labelResolution).Name = "labelResolution";
		((Control)labelResolution).Padding = new Padding(0, 0, 12, 0);
		((Control)labelResolution).Size = new Size(169, 21);
		((Control)labelResolution).TabIndex = 13;
		((Control)labelResolution).Text = "Image Resolution";
		((Label)labelResolution).TextAlign = (ContentAlignment)16;
		((Control)labelScale).ForeColor = Color.DimGray;
		((Control)labelScale).Location = new Point(12, 58);
		((Control)labelScale).Name = "labelScale";
		((Control)labelScale).Padding = new Padding(0, 0, 12, 0);
		((Control)labelScale).Size = new Size(169, 21);
		((Control)labelScale).TabIndex = 12;
		((Control)labelScale).Text = "Scale";
		((Label)labelScale).TextAlign = (ContentAlignment)16;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(9, 8);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Padding = new Padding(0, 0, 12, 0);
		((Control)labelType).Size = new Size(166, 21);
		((Control)labelType).TabIndex = 2;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Control)pagesRange).Anchor = (AnchorStyles)13;
		((Control)pagesRange).BackColor = Color.White;
		pagesRange.CurrentPage = 0;
		((Control)pagesRange).Location = new Point(12, 8);
		((Control)pagesRange).Name = "pagesRange";
		((Control)pagesRange).Size = new Size(330, 159);
		((Control)pagesRange).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(356, 577);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)groupBoxExportSettings);
		((Control)this).Controls.Add((Control)(object)pagesRange);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiImageSetupForm";
		((Control)this).Padding = new Padding(0, 4, 0, 0);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiImageSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelOpenAfterExport).PerformLayout();
		((Control)groupBoxExportSettings).ResumeLayout(false);
		((Control)groupBoxExportSettings).PerformLayout();
		((Control)panelType).ResumeLayout(false);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
