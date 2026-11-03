using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Win.Images;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(StiProgressInformation), "Toolbox.StiDotMatrixViewerControl.bmp")]
public class StiDotMatrixViewerControl : UserControl, IStiViewerControl
{
	private ImageList ilReport;

	private Bar tbTool;

	internal ButtonItem tbClose;

	private ButtonItem sep1;

	internal ButtonItem tbSave;

	internal ButtonItem tbOpen;

	internal ButtonItem tbPrint;

	private StiDotMatrixViewControl view;

	internal ButtonItem buttonHelp;

	private IContainer components;

	internal SuperTooltip superToolTip = new SuperTooltip();

	internal readonly eTooltipColor CurrentTooltipColor;

	private IStiProgressInformation progress;

	private string LoadedReportFileName;

	private bool LockDotMatrixUpdate;

	private bool LockSaveConfig;

	private string prevZoomX;

	private string prevZoomY;

	private StiReport report;

	private bool showPrint = true;

	private bool showOpen = true;

	private bool showSave = true;

	private bool showCloseButton = true;

	public StiReport Report
	{
		get
		{
			return report;
		}
		set
		{
			if (report != null)
			{
				SaveConfig();
			}
			if (report != value)
			{
				LoadedReportFileName = null;
				if (report != null)
				{
					report.RefreshViewer -= OnRefreshViewer;
					if (report.CompiledReport != null)
					{
						report.CompiledReport.RefreshViewer -= OnRefreshViewer;
					}
				}
				if (value != null)
				{
					value.RefreshViewer += OnRefreshViewer;
					if (value.CompiledReport != null)
					{
						value.CompiledReport.RefreshViewer += OnRefreshViewer;
					}
				}
				CurrentPrintPage = 0;
			}
			report = value;
			LoadConfig();
			if (CurrentReport == null || CurrentReport.PreviewMode == StiPreviewMode.Standard)
			{
				RefreshDotMatrixReport();
			}
			((Control)this).Refresh();
		}
	}

	[Category("Parameters")]
	[DefaultValue(StiGuiStandardStyle.Office2007)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Obsolete("Please use StiOptions.Windows.GlobalGuiStyle static property instead Style property of StiDotMatrixViewerControl!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public StiGuiStandardStyle Style
	{
		get
		{
			if (StiOptions.Windows.GuiStyle == StiGuiStyle.Office2003)
			{
				return StiGuiStandardStyle.Office2003;
			}
			if (StiOptions.Windows.GuiStyle == StiGuiStyle.OfficeXP)
			{
				return StiGuiStandardStyle.OfficeXP;
			}
			if (StiOptions.Windows.GuiStyle == StiGuiStyle.Default)
			{
				return StiGuiStandardStyle.Default;
			}
			return StiGuiStandardStyle.Office2007;
		}
		set
		{
			if (value == StiGuiStandardStyle.Default)
			{
				StiOptions.Windows.GuiStyle = StiGuiStyle.Default;
			}
			if (value == StiGuiStandardStyle.Office2003)
			{
				StiOptions.Windows.GuiStyle = StiGuiStyle.Office2003;
			}
			if (value == StiGuiStandardStyle.Office2007)
			{
				StiOptions.Windows.GuiStyle = StiGuiStyle.Office2007Blue;
			}
			if (value == StiGuiStandardStyle.OfficeXP)
			{
				StiOptions.Windows.GuiStyle = StiGuiStyle.OfficeXP;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal int CurrentPrintPage
	{
		get
		{
			if (CurrentReport == null)
			{
				return 0;
			}
			return CurrentReport.CurrentPrintPage;
		}
		set
		{
			if (CurrentReport != null)
			{
				CurrentReport.CurrentPrintPage = value;
			}
		}
	}

	internal StiReport CurrentReport
	{
		get
		{
			if (report == null)
			{
				return null;
			}
			if (report.CompiledReport != null)
			{
				return report.CompiledReport;
			}
			return report;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public virtual bool ShowToolbar
	{
		get
		{
			return tbTool.Visible;
		}
		set
		{
			tbTool.Visible = value;
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowPrint
	{
		get
		{
			return showPrint;
		}
		set
		{
			((BaseItem)tbPrint).Visible = (showPrint = value);
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowOpen
	{
		get
		{
			return showOpen;
		}
		set
		{
			((BaseItem)tbOpen).Visible = (showOpen = value);
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowSave
	{
		get
		{
			return showSave;
		}
		set
		{
			((BaseItem)tbSave).Visible = (showSave = value);
		}
	}

	[Category("Parameters")]
	[DefaultValue(true)]
	public bool ShowCloseButton
	{
		get
		{
			return showCloseButton;
		}
		set
		{
			((BaseItem)tbClose).Visible = (showCloseButton = value);
		}
	}

	[Category("PreviewControl")]
	public event EventHandler Close;

	[Category("PreviewControl")]
	public event EventHandler ClickPrintButton;

	[Category("PreviewControl")]
	public event EventHandler ClickOpenButton;

	[Category("PreviewControl")]
	public event EventHandler ClickSaveButton;

	public StiDotMatrixViewerControl()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			CurrentTooltipColor = (eTooltipColor)19;
			Localize();
			if (!StiOptions.Appearance.ShowTooltipsHelp)
			{
				((CollectionBase)(object)((BaseItem)buttonHelp).Shortcuts).Clear();
			}
			view.settings.cbEncoding.Items.Clear();
			ObjectCollection items = view.settings.cbEncoding.Items;
			object[] encodingNames = GetEncodingNames();
			items.AddRange(encodingNames);
			((ComboBox)view.settings.cbEncoding).SelectedItem = Encoding.Default.EncodingName;
			view.settings.cbEscapeCodesCollectionList.Items.Clear();
			ObjectCollection items2 = view.settings.cbEscapeCodesCollectionList.Items;
			encodingNames = GetEscapeCodesCollectionsNames();
			items2.AddRange(encodingNames);
			if (view.settings.cbEscapeCodesCollectionList.Items.Count > 0)
			{
				((ListControl)view.settings.cbEscapeCodesCollectionList).SelectedIndex = 0;
			}
			view.settings.rbSimple.CheckedChanged += checkBox_Checked;
			view.settings.rbUnicodeSingle.CheckedChanged += checkBox_Checked;
			view.settings.rbUnicodeDouble.CheckedChanged += checkBox_Checked;
			view.settings.cbDrawBorder.CheckedChanged += checkBox_Checked;
			view.settings.cbKillSpaceLines.CheckedChanged += checkBox_Checked;
			view.settings.cbPutFeedPageCode.CheckedChanged += checkBox_Checked;
			view.settings.checkBoxCutLongLines.CheckedChanged += checkBox_Checked;
			view.settings.checkBoxKillSpaceGraphLines.CheckedChanged += checkBox_Checked;
			view.settings.cbUseEscapeCodes.CheckedChanged += checkBox_Checked;
			((ComboBox)view.settings.cbEscapeCodesCollectionList).SelectedIndexChanged += comboBox_SelectedIndexChanged;
			((ComboBox)view.settings.cbEncoding).SelectedIndexChanged += comboBox_SelectedIndexChanged;
			((ComboBox)view.settings.cbZoomX).SelectedIndexChanged += comboBox_SelectedIndexChanged;
			((ComboBox)view.settings.cbZoomY).SelectedIndexChanged += comboBox_SelectedIndexChanged;
			((Control)view.settings.buttonRefresh).Click += buttonRefresh_Click;
			view.settings.view = this;
			ApplyStyle();
			InitImage();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	public void Localize()
	{
		try
		{
			((BaseItem)tbPrint).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Print"));
			((BaseItem)tbOpen).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((BaseItem)tbSave).Text = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Save"));
			((BaseItem)tbClose).Text = StiLocalization.Get("FormViewer", "Close");
			LocalizeSuperToolTip();
		}
		catch
		{
		}
	}

	private void LocalizeSuperToolTip()
	{
		superToolTip.RemoveAllSuperTooltip();
		Graphics g = ((Control)this).CreateGraphics();
		string url = "user-manual/viewing_reports_dot_matrix_viewer_toolbar.htm";
		SetToolTip((BaseItem)(object)tbPrint, g, ((BaseItem)tbPrint).Text, "Print", StiScale.I(140), headerVisible: true, url);
		SetToolTip((BaseItem)(object)tbOpen, g, ((BaseItem)tbOpen).Text, "Open", StiScale.I(264), headerVisible: true, url);
		SetToolTip((BaseItem)(object)tbSave, g, ((BaseItem)tbSave).Text, "Save", StiScale.I(200), headerVisible: true, url);
		SetToolTip((BaseItem)(object)tbClose, g, ((BaseItem)tbClose).Text, "CloseDotMatrix", StiScale.I(200), headerVisible: true, url);
	}

	private void SetToolTip(BaseItem item, Graphics g, string header, string localizeName, int width, bool headerVisible, string url)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		if (item != null)
		{
			string text = StiLocalization.Get("HelpDesigner", "PressF1");
			SizeF sizeF = superToolTip.MeasureFooterText(g, text);
			sizeF.Width += StiScale.I(40);
			string text2 = StiLocalization.Get("HelpViewer", localizeName);
			SuperTooltipInfo val = new SuperTooltipInfo(header, text, text2, (Image)null, (Image)null, CurrentTooltipColor)
			{
				HeaderVisible = headerVisible,
				Hyperlink = url,
				BodyImage = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Help" + StiScale.StepName + ".png")
			};
			if (sizeF.Width > (float)width)
			{
				width = (int)sizeF.Width;
			}
			int height = (int)superToolTip.MeasureBodyText(g, text2, width - StiScale.I(28)).Height + StiScale.I(52);
			val.CustomSize = new Size(width, height);
			superToolTip.SetSuperTooltip((IComponent)item, val);
		}
	}

	private void InitializeComponent()
	{
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
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDotMatrixViewerControl));
		ilReport = new ImageList(components);
		tbTool = new Bar();
		tbPrint = new ButtonItem();
		tbOpen = new ButtonItem();
		tbSave = new ButtonItem();
		tbClose = new ButtonItem();
		sep1 = new ButtonItem();
		buttonHelp = new ButtonItem();
		view = new StiDotMatrixViewControl();
		((ISupportInitialize)tbTool).BeginInit();
		((Control)this).SuspendLayout();
		ilReport.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilReport.ImageStream");
		ilReport.TransparentColor = Color.Magenta;
		ilReport.Images.SetKeyName(0, "");
		ilReport.Images.SetKeyName(1, "");
		ilReport.Images.SetKeyName(2, "");
		ilReport.Images.SetKeyName(3, "");
		ilReport.Images.SetKeyName(4, "");
		ilReport.Images.SetKeyName(5, "");
		ilReport.Images.SetKeyName(6, "");
		ilReport.Images.SetKeyName(7, "");
		ilReport.Images.SetKeyName(8, "");
		ilReport.Images.SetKeyName(9, "");
		ilReport.Images.SetKeyName(10, "");
		ilReport.Images.SetKeyName(11, "");
		ilReport.Images.SetKeyName(12, "");
		ilReport.Images.SetKeyName(13, "");
		ilReport.Images.SetKeyName(14, "");
		ilReport.Images.SetKeyName(15, "");
		ilReport.Images.SetKeyName(16, "");
		ilReport.Images.SetKeyName(17, "");
		ilReport.Images.SetKeyName(18, "");
		ilReport.Images.SetKeyName(19, "");
		ilReport.Images.SetKeyName(20, "");
		ilReport.Images.SetKeyName(21, "");
		ilReport.Images.SetKeyName(22, "");
		ilReport.Images.SetKeyName(23, "");
		ilReport.Images.SetKeyName(24, "");
		ilReport.Images.SetKeyName(25, "");
		ilReport.Images.SetKeyName(26, "");
		ilReport.Images.SetKeyName(27, "");
		ilReport.Images.SetKeyName(28, "");
		ilReport.Images.SetKeyName(29, "");
		ilReport.Images.SetKeyName(30, "");
		ilReport.Images.SetKeyName(31, "");
		ilReport.Images.SetKeyName(32, "");
		ilReport.Images.SetKeyName(33, "");
		((Control)tbTool).Dock = (DockStyle)1;
		((Control)tbTool).Font = new Font("Segoe UI", 9f);
		tbTool.Items.AddRange((BaseItem[])(object)new BaseItem[6]
		{
			(BaseItem)tbPrint,
			(BaseItem)tbOpen,
			(BaseItem)tbSave,
			(BaseItem)tbClose,
			(BaseItem)sep1,
			(BaseItem)buttonHelp
		});
		tbTool.Location = new Point(0, 0);
		tbTool.Name = "tbTool";
		tbTool.Padding = new Padding(12, 3, 3, 3);
		tbTool.RoundCorners = false;
		tbTool.Size = new Size(716, 25);
		tbTool.Stretch = true;
		tbTool.Style = (eDotNetBarStyle)8;
		tbTool.TabIndex = 52;
		tbTool.TabStop = false;
		((Control)tbTool).Text = "stiToolBar1";
		tbPrint.ButtonStyle = (eButtonStyle)2;
		((BaseItem)tbPrint).Name = "tbPrint";
		((BaseItem)tbPrint).Shortcuts.Add((eShortcut)131152);
		((BaseItem)tbPrint).Text = "Print";
		((BaseItem)tbPrint).Click += buttonPrint_Click;
		tbOpen.ButtonStyle = (eButtonStyle)2;
		((BaseItem)tbOpen).Name = "tbOpen";
		((BaseItem)tbOpen).Shortcuts.Add((eShortcut)131151);
		((BaseItem)tbOpen).Text = "Open";
		((BaseItem)tbOpen).Click += buttonOpen_Click;
		tbSave.ButtonStyle = (eButtonStyle)2;
		((BaseItem)tbSave).Name = "tbSave";
		((BaseItem)tbSave).Shortcuts.Add((eShortcut)131155);
		((BaseItem)tbSave).Text = "Save";
		((BaseItem)tbSave).Click += buttonSave_Click;
		((BaseItem)tbClose).BeginGroup = true;
		tbClose.FixedSize = new Size(70, 1);
		((BaseItem)tbClose).Name = "tbClose";
		((BaseItem)tbClose).Text = "Close";
		((BaseItem)tbClose).Click += buttonClose_Click;
		((BaseItem)sep1).Name = "sep1";
		buttonHelp.ButtonStyle = (eButtonStyle)2;
		((BaseItem)buttonHelp).ItemAlignment = (eItemAlignment)1;
		((BaseItem)buttonHelp).Name = "buttonHelp";
		((BaseItem)buttonHelp).Shortcuts.Add((eShortcut)112);
		((BaseItem)buttonHelp).Click += buttonHelp_Click;
		((Control)view).Dock = (DockStyle)5;
		((Control)view).Location = new Point(0, 25);
		((Control)view).Name = "view";
		((Control)view).Size = new Size(716, 567);
		((Control)view).TabIndex = 53;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Control)this).Controls.Add((Control)(object)view);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Control)this).Name = "StiDotMatrixViewerControl";
		((Control)this).Size = new Size(716, 592);
		((ISupportInitialize)tbTool).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void OnRefreshViewer(object sender, EventArgs e)
	{
		((Control)this).Refresh();
	}

	protected virtual void OnClose(object sender)
	{
		if (CurrentReport != null)
		{
			SaveConfig();
			Report = null;
		}
	}

	public void InvokeClose()
	{
		if (this.Close != null)
		{
			this.Close(this, EventArgs.Empty);
		}
		OnClose(this);
	}

	protected virtual void OnClickPrintButton(object sender)
	{
		if (CurrentReport != null)
		{
			StiTxtBorderType borderType = StiTxtBorderType.Simple;
			if (view.settings.rbUnicodeSingle.Checked)
			{
				borderType = StiTxtBorderType.UnicodeSingle;
			}
			else if (view.settings.rbUnicodeDouble.Checked)
			{
				borderType = StiTxtBorderType.UnicodeDouble;
			}
			float zoomX = 1f;
			float zoomY = 1f;
			try
			{
				zoomX = float.Parse(((Control)view.settings.cbZoomX).Text.Substring(0, ((Control)view.settings.cbZoomX).Text.Length - 1)) / 100f;
				zoomY = float.Parse(((Control)view.settings.cbZoomY).Text.Substring(0, ((Control)view.settings.cbZoomY).Text.Length - 1)) / 100f;
			}
			catch
			{
			}
			StiDotMatrixPrintProvider.PrintToDotMatrixPrinter(CurrentReport, CurrentReport.PrinterSettings.PrinterName, StiEncodingHelper.GetEncodingFromName(((ComboBox)view.settings.cbEncoding).SelectedItem as string, GetEncodings()), view.settings.cbDrawBorder.Checked, borderType, view.settings.cbKillSpaceLines.Checked, view.settings.checkBoxKillSpaceGraphLines.Checked, view.settings.cbPutFeedPageCode.Checked, view.settings.checkBoxCutLongLines.Checked, zoomX, zoomY, -1, -1, view.settings.cbUseEscapeCodes.Checked, ((Control)view.settings.cbEscapeCodesCollectionList).Text);
		}
	}

	public void InvokeClickPrintButton()
	{
		if (this.ClickPrintButton != null)
		{
			this.ClickPrintButton(this, EventArgs.Empty);
		}
		else
		{
			OnClickPrintButton(this);
		}
	}

	protected virtual void OnClickOpenButton(object sender)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Invalid comparison between Unknown and I4
		if (CurrentReport == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			StiSettings.Load();
			string text = StiOptions.Viewer.Windows.ReportSaveLoadPath;
			if (text == null || text.Length == 0)
			{
				text = StiSettings.GetStr("Viewer", "ReportSaveLoadPath", string.Empty);
			}
			if (text != null && text.Length > 0 && Directory.Exists(text))
			{
				((FileDialog)val).InitialDirectory = text;
			}
			((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "TxtFiles");
			((FileDialog)val).RestoreDirectory = true;
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
			{
				StiSettings.Load();
				string text2 = StiOptions.Viewer.Windows.ReportSaveLoadPath;
				if (text2 == null || text2.Length == 0)
				{
					text2 = StiSettings.GetStr("Viewer", "ReportSaveLoadPath", string.Empty);
				}
				if (text2 != null && text2.Length > 0 && Directory.Exists(text2))
				{
					((FileDialog)val).InitialDirectory = text2;
				}
				StreamReader streamReader = new StreamReader(((FileDialog)val).FileName, StiEncodingHelper.GetEncodingFromName(((ComboBox)view.settings.cbEncoding).SelectedItem as string, GetEncodings()));
				((Control)view.textBoxReport).Text = streamReader.ReadToEnd();
				streamReader.Close();
				LoadedReportFileName = ((FileDialog)val).FileName;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnLoading(object sender, EventArgs e)
	{
		Application.DoEvents();
		if (progress != null && progress.IsBreaked)
		{
			throw new StopProgressException();
		}
	}

	public void InvokeClickOpenButton()
	{
		if (this.ClickOpenButton != null)
		{
			this.ClickOpenButton(this, EventArgs.Empty);
		}
		else
		{
			OnClickOpenButton(this);
		}
	}

	protected virtual void OnClickSaveButton(object sender)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		if (CurrentReport == null)
		{
			return;
		}
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			StiSettings.Load();
			string text = StiOptions.Viewer.Windows.ReportSaveLoadPath;
			if (text == null || text.Length == 0)
			{
				text = StiSettings.GetStr("Viewer", "ReportSaveLoadPath", string.Empty);
			}
			if (text != null && text.Length > 0 && Directory.Exists(text))
			{
				((FileDialog)val).InitialDirectory = text;
			}
			((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "TxtFiles");
			((FileDialog)val).RestoreDirectory = true;
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Save"));
			((FileDialog)val).FileName = CurrentReport.ReportAlias;
			((FileDialog)val).DefaultExt = "txt";
			if ((int)((CommonDialog)val).ShowDialog() != 1 || ((FileDialog)val).FileName.Length <= 0)
			{
				return;
			}
			StiSettings.Load();
			string text2 = StiOptions.Viewer.Windows.ReportSaveLoadPath;
			if (text2 == null || text2.Length == 0)
			{
				text2 = StiSettings.GetStr("Viewer", "ReportSaveLoadPath", string.Empty);
			}
			if (text2 != null && text2.Length > 0 && Directory.Exists(text2))
			{
				((FileDialog)val).InitialDirectory = text2;
			}
			using StreamWriter streamWriter = new StreamWriter(((FileDialog)val).FileName, append: false, StiEncodingHelper.GetEncodingFromName(((ComboBox)view.settings.cbEncoding).SelectedItem as string, GetEncodings()));
			StiTxtBorderType borderType = StiTxtBorderType.Simple;
			if (view.settings.rbUnicodeSingle.Checked)
			{
				borderType = StiTxtBorderType.UnicodeSingle;
			}
			else if (view.settings.rbUnicodeDouble.Checked)
			{
				borderType = StiTxtBorderType.UnicodeDouble;
			}
			float zoomX = 1f;
			float zoomY = 1f;
			try
			{
				if (((Control)view.settings.cbZoomX).Text.Length > 0)
				{
					zoomX = float.Parse(((Control)view.settings.cbZoomX).Text.Substring(0, ((Control)view.settings.cbZoomX).Text.Length - 1)) / 100f;
				}
				if (((Control)view.settings.cbZoomY).Text.Length > 0)
				{
					zoomY = float.Parse(((Control)view.settings.cbZoomY).Text.Substring(0, ((Control)view.settings.cbZoomY).Text.Length - 1)) / 100f;
				}
			}
			catch
			{
			}
			string reportForDotMatrixReport = StiDotMatrixPrintProvider.GetReportForDotMatrixReport(CurrentReport, Encoding.UTF8, view.settings.cbDrawBorder.Checked, borderType, view.settings.cbKillSpaceLines.Checked, view.settings.checkBoxKillSpaceGraphLines.Checked, view.settings.cbPutFeedPageCode.Checked, view.settings.checkBoxCutLongLines.Checked, zoomX, zoomY, StiPagesRange.All, view.settings.cbUseEscapeCodes.Checked, ((Control)view.settings.cbEscapeCodesCollectionList).Text);
			streamWriter.Write(reportForDotMatrixReport);
			streamWriter.Flush();
			streamWriter.Close();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnSaving(object sender, EventArgs e)
	{
		Application.DoEvents();
		if (progress.IsBreaked)
		{
			throw new StopProgressException();
		}
	}

	public void InvokeClickSaveButton()
	{
		if (this.ClickSaveButton != null)
		{
			this.ClickSaveButton(this, EventArgs.Empty);
		}
		else
		{
			OnClickSaveButton(this);
		}
	}

	private void InitImage()
	{
		tbPrint.Image = StiReportViewerImages.Actions.ReportPrint((StiImageSize)0);
		tbOpen.Image = StiReportViewerImages.Actions.ReportOpen((StiImageSize)0);
		tbSave.Image = StiReportViewerImages.Actions.ReportSave((StiImageSize)0);
		buttonHelp.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Win", "Stimulsoft.Report.Win.Resources.Help" + StiScale.StepName + ".png");
	}

	private void ApplyStyle()
	{
		StiGuiHelper.ApplyStyle((Control)(object)this);
		((Control)view).BackColor = StiGuiHelper.GetControlColor();
	}

	private Encoding[] GetEncodings()
	{
		if (StiOptions.Viewer.DotMatrix.DefaultEncodings != null)
		{
			return StiOptions.Viewer.DotMatrix.DefaultEncodings;
		}
		return StiEncodingHelper.GetEncodings();
	}

	private string[] GetEncodingNames()
	{
		if (StiOptions.Viewer.DotMatrix.DefaultEncodings != null)
		{
			return StiEncodingHelper.GetEncodingNames(StiOptions.Viewer.DotMatrix.DefaultEncodings);
		}
		return StiEncodingHelper.GetEncodingNames();
	}

	private string[] GetEscapeCodesCollectionsNames()
	{
		string[] array = new string[StiOptions.Export.Text.EscapeCodesCollectionList.Count];
		for (int i = 0; i < array.Length; i++)
		{
			StiEscapeCodesCollection stiEscapeCodesCollection = StiOptions.Export.Text.EscapeCodesCollectionList[i];
			string text = stiEscapeCodesCollection.Name;
			if (text == null || text.Length == 0)
			{
				text = $"unknown_{i + 1}";
			}
			array[i] = text;
		}
		return array;
	}

	public void LoadConfig()
	{
		tbTool.SuspendLayout();
		StiDotMatrixViewerConfigService stiDotMatrixViewerConfigService = StiOptions.Services.DotMatrixViewerConfig.FirstOrDefault();
		if (stiDotMatrixViewerConfigService != null)
		{
			StiPreviewSettings settings = ((CurrentReport != null) ? ((StiPreviewSettings)CurrentReport.PreviewSettings) : StiPreviewSettings.Default);
			StiSettings.Load();
			LockSaveConfig = true;
			LockDotMatrixUpdate = true;
			view.settings.groupBoxBorderType.Expanded = !StiSettings.GetBool("Viewer", "groupBoxBorderType", !view.settings.groupBoxBorderType.Expanded);
			view.settings.groupBoxEncoding.Expanded = !StiSettings.GetBool("Viewer", "groupBoxEncoding", !view.settings.groupBoxEncoding.Expanded);
			view.settings.groupBoxEscapeCodes.Expanded = !StiSettings.GetBool("Viewer", "groupBoxEscapeCodes", !view.settings.groupBoxEscapeCodes.Expanded);
			view.settings.groupBoxRefresh.Expanded = !StiSettings.GetBool("Viewer", "groupBoxRefresh", !view.settings.groupBoxRefresh.Expanded);
			view.settings.groupBoxSettings.Expanded = !StiSettings.GetBool("Viewer", "groupBoxSettings", !view.settings.groupBoxSettings.Expanded);
			view.settings.groupBoxZoom.Expanded = !StiSettings.GetBool("Viewer", "groupBoxZoom", !view.settings.groupBoxZoom.Expanded);
			view.settings.checkBoxCutLongLines.Checked = StiSettings.GetBool("Viewer", "CutLongLines", view.settings.checkBoxCutLongLines.Checked);
			view.settings.cbDrawBorder.Checked = StiSettings.GetBool("Viewer", "DrawBorder", view.settings.cbDrawBorder.Checked);
			view.settings.cbKillSpaceLines.Checked = StiSettings.GetBool("Viewer", "KillSpaceLines", view.settings.cbKillSpaceLines.Checked);
			view.settings.checkBoxKillSpaceGraphLines.Checked = StiSettings.GetBool("Viewer", "KillSpaceGraphLines", view.settings.checkBoxKillSpaceGraphLines.Checked);
			view.settings.cbPutFeedPageCode.Checked = StiSettings.GetBool("Viewer", "PutFeedPageCode", view.settings.cbPutFeedPageCode.Checked);
			view.settings.cbUseEscapeCodes.Checked = StiSettings.GetBool("Viewer", "UseEscapeCodes", view.settings.cbUseEscapeCodes.Checked);
			view.settings.rbSimple.Checked = StiSettings.GetBool("Viewer", "Simple", view.settings.rbSimple.Checked);
			view.settings.rbUnicodeSingle.Checked = StiSettings.GetBool("Viewer", "UnicodeSingle", view.settings.rbUnicodeSingle.Checked);
			view.settings.rbUnicodeDouble.Checked = StiSettings.GetBool("Viewer", "UnicodeDouble", view.settings.rbUnicodeDouble.Checked);
			view.settings.checkBoxAutoRefresh.Checked = StiSettings.GetBool("Viewer", "AutoRefresh", view.settings.checkBoxAutoRefresh.Checked);
			((Control)view.settings.cbZoomX).Text = StiSettings.GetStr("Viewer", "ZoomX", ((Control)view.settings.cbZoomX).Text);
			((Control)view.settings.cbZoomY).Text = StiSettings.GetStr("Viewer", "ZoomY", ((Control)view.settings.cbZoomY).Text);
			((ComboBox)view.settings.cbEscapeCodesCollectionList).SelectedItem = StiSettings.GetStr("Viewer", "EscapeCodesCollectionName", ((Control)view.settings.cbEscapeCodesCollectionList).Text);
			int @int = StiSettings.GetInt("Viewer", "Encoding", 0);
			if (@int < view.settings.cbEncoding.Items.Count)
			{
				((ListControl)view.settings.cbEncoding).SelectedIndex = @int;
			}
			LockDotMatrixUpdate = false;
			LockSaveConfig = false;
			((BaseItem)tbPrint).Visible = GetPrintVisibility(settings, stiDotMatrixViewerConfigService);
			((BaseItem)tbSave).Visible = GetSaveVisibility(settings, stiDotMatrixViewerConfigService);
			((BaseItem)tbOpen).Visible = GetOpenVisibility(settings, stiDotMatrixViewerConfigService);
			((BaseItem)tbClose).Visible = GetCloseVisibility(settings, stiDotMatrixViewerConfigService);
		}
		if (CurrentReport != null && (CurrentReport.PreviewMode == StiPreviewMode.DotMatrix || CurrentReport.PreviewMode == StiPreviewMode.StandardAndDotMatrix))
		{
			RefreshDotMatrixReport();
		}
		tbTool.ResumeLayout();
	}

	public void SaveConfig()
	{
		if (!LockSaveConfig)
		{
			StiSettings.Load();
			StiSettings.Set("Viewer", "groupBoxBorderType", !view.settings.groupBoxBorderType.Expanded);
			StiSettings.Set("Viewer", "groupBoxEscapeCodes", !view.settings.groupBoxEscapeCodes.Expanded);
			StiSettings.Set("Viewer", "groupBoxEncoding", !view.settings.groupBoxEncoding.Expanded);
			StiSettings.Set("Viewer", "groupBoxRefresh", !view.settings.groupBoxRefresh.Expanded);
			StiSettings.Set("Viewer", "groupBoxSettings", !view.settings.groupBoxSettings.Expanded);
			StiSettings.Set("Viewer", "groupBoxZoom", !view.settings.groupBoxZoom.Expanded);
			StiSettings.Set("Viewer", "UseEscapeCodes", view.settings.cbUseEscapeCodes.Checked);
			StiSettings.Set("Viewer", "CutLongLines", view.settings.checkBoxCutLongLines.Checked);
			StiSettings.Set("Viewer", "DrawBorder", view.settings.cbDrawBorder.Checked);
			StiSettings.Set("Viewer", "KillSpaceLines", view.settings.cbKillSpaceLines.Checked);
			StiSettings.Set("Viewer", "KillSpaceGraphLines", view.settings.checkBoxKillSpaceGraphLines.Checked);
			StiSettings.Set("Viewer", "PutFeedPageCode", view.settings.cbPutFeedPageCode.Checked);
			StiSettings.Set("Viewer", "Simple", view.settings.rbSimple.Checked);
			StiSettings.Set("Viewer", "UnicodeSingle", view.settings.rbUnicodeSingle.Checked);
			StiSettings.Set("Viewer", "UnicodeDouble", view.settings.rbUnicodeDouble.Checked);
			StiSettings.Set("Viewer", "AutoRefresh", view.settings.checkBoxAutoRefresh.Checked);
			if (!string.IsNullOrEmpty(((Control)view.settings.cbZoomX).Text))
			{
				StiSettings.Set("Viewer", "ZoomX", ((Control)view.settings.cbZoomX).Text);
				prevZoomX = ((Control)view.settings.cbZoomX).Text;
			}
			else if (!string.IsNullOrEmpty(prevZoomX))
			{
				StiSettings.Set("Viewer", "ZoomX", prevZoomX);
			}
			if (!string.IsNullOrEmpty(((Control)view.settings.cbZoomY).Text))
			{
				StiSettings.Set("Viewer", "ZoomY", ((Control)view.settings.cbZoomY).Text);
				prevZoomY = ((Control)view.settings.cbZoomY).Text;
			}
			else if (!string.IsNullOrEmpty(prevZoomY))
			{
				StiSettings.Set("Viewer", "ZoomY", prevZoomY);
			}
			StiSettings.Set("Viewer", "Encoding", ((ListControl)view.settings.cbEncoding).SelectedIndex);
			StiSettings.Set("Viewer", "EscapeCodesCollectionName", ((ComboBox)view.settings.cbEscapeCodesCollectionList).SelectedItem.ToString());
			StiSettings.Save();
		}
	}

	private bool GetPrintVisibility(StiPreviewSettings settings, StiDotMatrixViewerConfigService config)
	{
		if (config != null && config.PrintEnabled.HasValue)
		{
			return config.PrintEnabled.GetValueOrDefault(true);
		}
		if ((settings & StiPreviewSettings.Print) > StiPreviewSettings.None)
		{
			return ShowPrint;
		}
		return false;
	}

	private bool GetSaveVisibility(StiPreviewSettings settings, StiDotMatrixViewerConfigService config)
	{
		if (config != null && config.SaveEnabled.HasValue)
		{
			return config.SaveEnabled.GetValueOrDefault(true);
		}
		if ((settings & StiPreviewSettings.Save) > StiPreviewSettings.None)
		{
			return ShowSave;
		}
		return false;
	}

	private bool GetOpenVisibility(StiPreviewSettings settings, StiDotMatrixViewerConfigService config)
	{
		if (config != null && config.OpenEnabled.HasValue)
		{
			return config.OpenEnabled.GetValueOrDefault(true);
		}
		if ((settings & StiPreviewSettings.Open) > StiPreviewSettings.None)
		{
			return ShowOpen;
		}
		return false;
	}

	private bool GetCloseVisibility(StiPreviewSettings settings, StiDotMatrixViewerConfigService config)
	{
		if (config != null && config.CloseEnabled.HasValue)
		{
			return config.CloseEnabled.GetValueOrDefault(true);
		}
		if ((settings & StiPreviewSettings.Close) <= StiPreviewSettings.None)
		{
			return ShowCloseButton;
		}
		return true;
	}

	private void RefreshDotMatrixReport()
	{
		if (CurrentReport == null || LockDotMatrixUpdate)
		{
			return;
		}
		LockDotMatrixUpdate = true;
		StiTxtBorderType borderType = StiTxtBorderType.Simple;
		if (view.settings.rbUnicodeSingle.Checked)
		{
			borderType = StiTxtBorderType.UnicodeSingle;
		}
		else if (view.settings.rbUnicodeDouble.Checked)
		{
			borderType = StiTxtBorderType.UnicodeDouble;
		}
		float zoomX = 1f;
		float zoomY = 1f;
		try
		{
			if (((Control)view.settings.cbZoomX).Text.Length > 0)
			{
				zoomX = float.Parse(((Control)view.settings.cbZoomX).Text.Substring(0, ((Control)view.settings.cbZoomX).Text.Length - 1)) / 100f;
			}
			if (((Control)view.settings.cbZoomY).Text.Length > 0)
			{
				zoomY = float.Parse(((Control)view.settings.cbZoomY).Text.Substring(0, ((Control)view.settings.cbZoomY).Text.Length - 1)) / 100f;
			}
		}
		catch
		{
		}
		string text = "{\\rtf1\\ansi\\ansicpg1251\\deff0\\deflang1049{\\fonttbl{\\f0\\fnil\\fcharset204{\\*\\fname Courier New;}Courier New CYR;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n";
		string reportForDotMatrixReport = StiDotMatrixPrintProvider.GetReportForDotMatrixReport(CurrentReport, Encoding.UTF8, view.settings.cbDrawBorder.Checked, borderType, view.settings.cbKillSpaceLines.Checked, view.settings.checkBoxKillSpaceGraphLines.Checked, view.settings.cbPutFeedPageCode.Checked, view.settings.checkBoxCutLongLines.Checked, zoomX, zoomY, StiPagesRange.All, view.settings.cbUseEscapeCodes.Checked, ((Control)view.settings.cbEscapeCodesCollectionList).Text);
		StringBuilder stringBuilder = new StringBuilder();
		string text2 = reportForDotMatrixReport;
		foreach (char c in text2)
		{
			if (c > '\u007f')
			{
				int num = c;
				stringBuilder.Append($"\\u{num.ToString()}?");
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		reportForDotMatrixReport = stringBuilder.ToString();
		Hashtable hashtable = new Hashtable();
		hashtable["b"] = "\\b ";
		hashtable["/b"] = "\\b0 ";
		hashtable["i"] = "\\i ";
		hashtable["/i"] = "\\i0 ";
		hashtable["u"] = "\\ul ";
		hashtable["/u"] = "\\ul0 ";
		hashtable["sup"] = "\\super ";
		hashtable["/sup"] = "\\nosupersub ";
		hashtable["sub"] = "\\sub ";
		hashtable["/sub"] = "\\nosupersub ";
		hashtable["condensed"] = "\\charscalex59 ";
		hashtable["/condensed"] = "\\charscalex100 ";
		hashtable["elite"] = "\\charscalex83 ";
		hashtable["pica"] = "\\charscalex100 ";
		hashtable["doublewidth"] = "\\charscalex200 ";
		hashtable["/doublewidth"] = "\\charscalex100 ";
		hashtable["ff"] = "<#ff>\\par ";
		StiEscapeCodesCollection escapeCodesCollection = StiEscapeCodesCollection.GetEscapeCodesCollection(((Control)view.settings.cbEscapeCodesCollectionList).Text);
		foreach (DictionaryEntry item in escapeCodesCollection)
		{
			string oldValue = (string)item.Value;
			string text3 = (string)item.Key;
			text3 = ((!hashtable.Contains(text3)) ? ("<#" + text3 + ">") : ((string)hashtable[text3]));
			reportForDotMatrixReport = reportForDotMatrixReport.Replace(oldValue, text3);
		}
		reportForDotMatrixReport = reportForDotMatrixReport.Replace("\r\n", "\\par ");
		view.textBoxReport.Rtf = text + reportForDotMatrixReport + "}";
		((TextBoxBase)view.textBoxReport).SelectionLength = 0;
		LockDotMatrixUpdate = false;
	}

	private void buttonHelp_Click(object sender, EventArgs e)
	{
		StiHelpProvider.ShowHelpViewer(SuperTooltip.CurrentSuperTooltipHyperlink);
	}

	private void checkBox_Checked(object sender, EventArgs e)
	{
		if (view.settings.checkBoxAutoRefresh != null && view.settings.checkBoxAutoRefresh.Checked)
		{
			RefreshDotMatrixReport();
		}
	}

	private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (view.settings.checkBoxAutoRefresh != null && view.settings.checkBoxAutoRefresh.Checked)
		{
			RefreshDotMatrixReport();
		}
	}

	private void buttonClose_Click(object sender, EventArgs e)
	{
		InvokeClose();
	}

	private void buttonPrint_Click(object sender, EventArgs e)
	{
		InvokeClickPrintButton();
	}

	private void buttonOpen_Click(object sender, EventArgs e)
	{
		InvokeClickOpenButton();
	}

	private void buttonSave_Click(object sender, EventArgs e)
	{
		InvokeClickSaveButton();
	}

	private void buttonRefresh_Click(object sender, EventArgs e)
	{
		RefreshDotMatrixReport();
	}
}
