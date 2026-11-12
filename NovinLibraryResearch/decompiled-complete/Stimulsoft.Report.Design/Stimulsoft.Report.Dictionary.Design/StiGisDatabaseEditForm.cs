using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiGisDatabaseEditForm : StiFormUX
{
	private string originalName;

	private string oldValue;

	private StiReport report;

	private bool isNameChanged;

	private IContainer components;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	public StiTextBox tbName;

	private StiButtonUX btMoveToResource;

	private ErrorProvider errorProvider;

	private StiLabelUX labelFilePath;

	public StiButtonEdit bePathData;

	private StiButtonPanelUX glButtons;

	private StiLabelUX labelName;

	private StiLabelUX labelAlias;

	public StiTextBox tbAlias;

	private StiLabelUX labelDataType;

	private StiComboBox comboBoxDataType;

	private StiOffice2013GroupBox groupResources;

	private ScrollableControl listResources;

	private ToolTip toolTip;

	private ImageList images;

	private StiLabelUX labelSeparator;

	public StiTextBox textBoxSeparator;

	private StiPanelUX panelName;

	private StiPanelUX panelAlias;

	private StiPanelUX panelPathData;

	private StiPanelUX panelDataType;

	private StiPanelUX panelSeparator;

	private StiPanelUX panelGuid;

	private StiSampleLabelUX labelGuidWebValue;

	private StiLabelUX labelGuidWebName;

	private StiLabelUX labelGuidFileSeparator;

	private StiSampleLabelUX labelGuidFileValue;

	private StiLabelUX labelGuidFileName;

	public string Separator
	{
		get
		{
			return ((Control)textBoxSeparator).Text;
		}
		set
		{
			((Control)textBoxSeparator).Text = value;
		}
	}

	public StiGisDataType DataType
	{
		get
		{
			if (((ListControl)comboBoxDataType).SelectedIndex != 0)
			{
				return StiGisDataType.GeoJSON;
			}
			return StiGisDataType.Wkt;
		}
		set
		{
			((ListControl)comboBoxDataType).SelectedIndex = ((value != 0) ? 1 : 0);
		}
	}

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(56.0);

	public StiGisDatabaseEditForm(StiReport report, string originalName)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			DoScaling();
			this.report = report;
			this.originalName = originalName;
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			CalculateSize();
			((ComboBox)comboBoxDataType).SelectedIndexChanged += ComboBoxDataType_SelectedIndexChanged;
			((Control)bePathData).TextChanged += OnPathDataTextChanged;
			((Form)this).Load += This_Load;
		}
	}

	private void BuildGallery()
	{
		List<StiResource> resources = GetResources();
		((Control)groupResources).Visible = resources.Any();
		((Control)listResources).Controls.Clear();
		MethodInvoker val = default(MethodInvoker);
		Task.Factory.StartNew(delegate
		{
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_0186: Expected O, but got Unknown
			int num = ItemGalleryStep;
			int itemGalleryStep = ItemGalleryStep;
			string resourceNameFromHyperlink = StiHyperlinkProcessor.GetResourceNameFromHyperlink(((Control)bePathData).Text);
			foreach (StiResource resource in resources)
			{
				try
				{
					StiImageButton button = CreateGalleryButton(resource.Name, images.Images[0], resource, num, itemGalleryStep, ItemGallerySize);
					((CheckBox)button).Checked = resourceNameFromHyperlink == resource.Name;
					((CheckBox)button).CheckedChanged += delegate(object sender, EventArgs e)
					{
						ItemGalleryClick(sender);
					};
					((CheckBox)button).DoubleClick += OnOkClick;
					((Control)this).Invoke((Delegate)(MethodInvoker)delegate
					{
						if (!IsFormDestructed())
						{
							toolTip.SetToolTip((Control)(object)button, StiLocalization.Get("PropertyMain", "Resource") + ": " + resource.Name);
							((Control)listResources).Controls.Add((Control)(object)button);
						}
					});
					num += ((Control)button).Width + ItemGalleryStep;
				}
				catch
				{
				}
			}
			StiGisDatabaseEditForm stiGisDatabaseEditForm = this;
			MethodInvoker obj2 = val;
			if (obj2 == null)
			{
				MethodInvoker val2 = delegate
				{
					if (!IsFormDestructed())
					{
						ScrollToActiveButton(listResources);
					}
				};
				MethodInvoker val3 = val2;
				val = val2;
				obj2 = val3;
			}
			((Control)stiGisDatabaseEditForm).Invoke((Delegate)(object)obj2);
		});
	}

	private void ItemGalleryClick(object sender)
	{
		(from Control c in (IEnumerable)((Control)listResources).Controls
			where c is StiImageButton && c != sender
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton && ((CheckBox)stiImageButton).Checked && ((Control)stiImageButton).Tag is StiResource stiResource)
		{
			((Control)bePathData).Text = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
		}
	}

	private StiImageButton CreateGalleryButton(string name, Image image, object tag, int x, int y, int thumbWidth)
	{
		StiImageButton obj = new StiImageButton
		{
			ShowFooter = true
		};
		((ButtonBase)obj).Image = image;
		((Control)obj).Left = x;
		((Control)obj).Top = y;
		((Control)obj).Width = (int)((double)thumbWidth * 1.5);
		((Control)obj).Height = thumbWidth;
		((Control)obj).Text = name;
		((Control)obj).Tag = tag;
		return obj;
	}

	private bool IsFormDestructed()
	{
		if (!((Control)this).IsDisposed && !((Control)this).Disposing)
		{
			return !((Control)this).IsHandleCreated;
		}
		return true;
	}

	private void ScrollToActiveButton(ScrollableControl gallery)
	{
		Control val = ((IEnumerable)((Control)gallery).Controls).Cast<Control>().FirstOrDefault((Func<Control, bool>)((Control c) => c is StiImageButton && ((CheckBox)(StiImageButton)(object)c).Checked));
		if (val != null)
		{
			gallery.ScrollControlIntoView(val);
		}
	}

	private List<StiResource> GetResources()
	{
		return (from v in report.Dictionary.Resources.ToList()
			where v.Type == StiResourceType.Gis && v.Content != null
			select v).ToList();
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelFilePath).Text = Loc.GetCleaned("FormDatabaseEdit", "PathGisData");
		((Control)groupResources).Text = StiLocalization.Get("PropertyMain", "Resources");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)btMoveToResource).Text = StiLocalization.Get("Buttons", "MoveToResource");
		((Control)labelSeparator).Text = Loc.GetCleaned("Export", "Separator");
		((Control)labelDataType).Text = Loc.GetCleaned("PropertyMain", "DataType");
		((ComboBox)comboBoxDataType).Items.Add((object)"WKT");
		((ComboBox)comboBoxDataType).Items.Add((object)"GeoJson");
		((Control)labelGuidFileName).Text = Loc.GetMain("File") + ":";
		((Control)labelGuidWebName).Text = Loc.Get("FormColorBoxPopup", "Web") + ":";
	}

	private void DoScaling()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		double step = StiScale.Step;
		if (step >= 1.5)
		{
			string bigStepName = StiScale.BigStepName;
			images = new ImageList
			{
				TransparentColor = Color.Transparent
			};
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceGis" + bigStepName + ".png");
			images.ImageSize = ((Image)image).Size;
			images.Images.Add("Excel", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceGis" + bigStepName + ".png"));
		}
		int num = (((Control)groupResources).Visible ? 121 : 0);
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void CalculateSize()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		int num = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is Button) && val.Visible)
			{
				num += val.Height;
			}
		}
		((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, num);
	}

	private bool IsEditMode()
	{
		return originalName != null;
	}

	private void UpdateMoveToResourceState()
	{
		bool enabled = ((Control)bePathData).Text.Length > 0 && !((Control)bePathData).Text.StartsWithInvariant("resource://");
		((Control)btMoveToResource).Enabled = enabled;
	}

	private void This_Load(object sender, EventArgs e)
	{
		CalculateSize();
	}

	private void ComboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (DataType == StiGisDataType.Wkt)
		{
			((Control)panelSeparator).Visible = true;
			if (!string.IsNullOrEmpty(((Control)bePathData).Text) && ((Control)bePathData).Text.ToLowerInvariant().EndsWith(".json"))
			{
				((Control)bePathData).Text = string.Empty;
			}
			((Control)labelGuidFileValue).Text = "c:\\wkt-file.wkt";
			((Control)labelGuidWebValue).Text = "https://www.site.com/wkt/";
		}
		else
		{
			((Control)panelSeparator).Visible = false;
			if (!string.IsNullOrEmpty(((Control)bePathData).Text) && ((Control)bePathData).Text.ToLowerInvariant().EndsWith(".wkt"))
			{
				((Control)bePathData).Text = string.Empty;
			}
			((Control)labelGuidFileValue).Text = "c:\\geojson-file.json";
			((Control)labelGuidWebValue).Text = "https://www.site.com/geo/";
		}
		CalculateSize();
	}

	private void OnPathDataTextChanged(object sender, EventArgs e)
	{
		UpdateMoveToResourceState();
	}

	private void OnMoveToResourceClick(object sender, EventArgs e)
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Invalid comparison between Unknown and I4
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameRequire);
			((Control)tbName).Focus();
			return;
		}
		if (((Control)tbName).Text != originalName && StiNameCreation.IsConnectionNameExists(report, ((Control)tbName).Text))
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameExists(((Control)tbName).Text));
			((Control)tbName).Focus();
			return;
		}
		try
		{
			if (!((Control)bePathData).Text.ToLowerInvariant().StartsWithInvariant("resource://"))
			{
				string text = StiNameCreation.CreateResourceName(report, Path.GetFileNameWithoutExtension(((Control)bePathData).Text).Normalize());
				StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(Path.GetExtension(((Control)bePathData).Text).ToLowerInvariant());
				if (!typeFromExtension.HasValue)
				{
					return;
				}
				if (new FileInfo(((Control)bePathData).Text).Length > StiOptions.Engine.ReportResources.MaximumSize)
				{
					DialogResult val = MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("Notices", "MessageMaximumFileSizeExceeded"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4, (MessageBoxIcon)32);
					if ((int)val == 7)
					{
						((Control)bePathData).Focus();
						return;
					}
				}
				else
				{
					StiResource resource = new StiResource(text, text, typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(((Control)bePathData).Text));
					report.Dictionary.Resources.Add(resource);
					((Control)bePathData).Text = "resource://" + text;
				}
			}
			((Form)this).DialogResult = (DialogResult)1;
		}
		catch
		{
		}
	}

	private void OnOkClick(object sender, EventArgs e)
	{
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameRequire);
			((Control)tbName).Focus();
		}
		else if (((Control)tbName).Text != originalName && StiNameCreation.IsConnectionNameExists(report, ((Control)tbName).Text))
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameExists(((Control)tbName).Text));
			((Control)tbName).Focus();
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void OnPathDataClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			if (DataType == StiGisDataType.Wkt)
			{
				((FileDialog)val).DefaultExt = "wkt";
				((FileDialog)val).Filter = "Wkt files|*.wkt";
			}
			else
			{
				((FileDialog)val).DefaultExt = "json";
				((FileDialog)val).Filter = "GeoJSON files|*.json;*.geojson";
			}
			((FileDialog)val).RestoreDirectory = true;
			if (!string.IsNullOrEmpty(((Control)bePathData).Text) && !((Control)bePathData).Text.StartsWithInvariant("resource://"))
			{
				((FileDialog)val).InitialDirectory = Path.GetDirectoryName(((Control)bePathData).Text);
				((FileDialog)val).FileName = Path.GetFileName(((Control)bePathData).Text);
			}
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				string baseName = Path.GetFileNameWithoutExtension(((FileDialog)val).FileName).Normalize();
				if (!isNameChanged && !IsEditMode())
				{
					((Control)tbName).Text = StiNameCreation.CreateConnectionName(report, baseName);
				}
				((Control)tbAlias).Text = ((Control)tbName).Text;
				((Control)bePathData).Text = ((FileDialog)val).FileName;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnNameEnter(object sender, EventArgs e)
	{
		oldValue = ((Control)tbName).Text;
	}

	private void OnNameLeave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)tbAlias).Text)
		{
			((Control)tbAlias).Text = ((Control)tbName).Text;
		}
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm");
	}

	private void ThisLoad(object sender, EventArgs e)
	{
		BuildGallery();
		DoScaling();
	}

	private void TextBox_KeyDown(object sender, KeyEventArgs e)
	{
		isNameChanged = true;
	}

	private void LabelGuidFileValue_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)bePathData).Text = ((Control)(sender as StiSampleLabelUX)).Text;
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Expected O, but got Unknown
		//IL_0c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c91: Expected O, but got Unknown
		//IL_0cff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d09: Expected O, but got Unknown
		//IL_0d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d42: Expected O, but got Unknown
		//IL_0dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de1: Expected O, but got Unknown
		//IL_0e7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e87: Expected O, but got Unknown
		//IL_0ef5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eff: Expected O, but got Unknown
		//IL_0f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f38: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiGisDatabaseEditForm));
		btCancel = new StiButtonUX();
		btOk = new StiButtonUX();
		btMoveToResource = new StiButtonUX();
		labelName = new StiLabelUX();
		tbName = new StiTextBox();
		labelFilePath = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		bePathData = new StiButtonEdit();
		glButtons = new StiButtonPanelUX();
		labelAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		groupResources = new StiOffice2013GroupBox();
		listResources = new ScrollableControl();
		toolTip = new ToolTip(components);
		images = new ImageList(components);
		labelSeparator = new StiLabelUX();
		textBoxSeparator = new StiTextBox();
		labelDataType = new StiLabelUX();
		comboBoxDataType = new StiComboBox();
		panelName = new StiPanelUX();
		panelAlias = new StiPanelUX();
		panelPathData = new StiPanelUX();
		panelGuid = new StiPanelUX();
		labelGuidWebValue = new StiSampleLabelUX();
		labelGuidWebName = new StiLabelUX();
		labelGuidFileSeparator = new StiLabelUX();
		labelGuidFileValue = new StiSampleLabelUX();
		labelGuidFileName = new StiLabelUX();
		panelDataType = new StiPanelUX();
		panelSeparator = new StiPanelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)groupResources).SuspendLayout();
		((Control)panelName).SuspendLayout();
		((Control)panelAlias).SuspendLayout();
		((Control)panelPathData).SuspendLayout();
		((Control)panelGuid).SuspendLayout();
		((Control)panelDataType).SuspendLayout();
		((Control)panelSeparator).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(412, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 25);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(331, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 25);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += OnOkClick;
		((Control)btMoveToResource).Anchor = (AnchorStyles)6;
		((Control)btMoveToResource).Location = new Point(12, 12);
		((Control)btMoveToResource).Name = "btMoveToResource";
		((Control)btMoveToResource).Size = new Size(150, 25);
		((Control)btMoveToResource).TabIndex = 0;
		((Control)btMoveToResource).Text = "Move to Resource";
		((ButtonBase)btMoveToResource).UseVisualStyleBackColor = true;
		((Control)btMoveToResource).Click += OnMoveToResourceClick;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(136, 16);
		((Control)labelName).TabIndex = 1;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(152, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(231, 21);
		((Control)tbName).TabIndex = 2;
		((Control)tbName).Enter += OnNameEnter;
		((Control)tbName).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)tbName).Leave += OnNameLeave;
		((Control)labelFilePath).Location = new Point(12, 10);
		((Control)labelFilePath).Name = "labelFilePath";
		((Control)labelFilePath).Size = new Size(136, 20);
		((Control)labelFilePath).TabIndex = 5;
		((Control)labelFilePath).Text = "Path Data";
		((Label)labelFilePath).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)bePathData).Anchor = (AnchorStyles)13;
		bePathData.ButtonIconKind = StiButtonIconKind.Dots;
		bePathData.Flat = false;
		((Control)bePathData).Location = new Point(152, 6);
		((Control)bePathData).Name = "bePathData";
		((Control)bePathData).Size = new Size(335, 21);
		((Control)bePathData).TabIndex = 6;
		bePathData.ButtonClick += OnPathDataClick;
		((Control)glButtons).Controls.Add((Control)(object)btMoveToResource);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 322);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(497, 48);
		((Control)glButtons).TabIndex = 6;
		((Control)labelAlias).Location = new Point(12, 10);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(136, 16);
		((Control)labelAlias).TabIndex = 3;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(152, 6);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(231, 21);
		((Control)tbAlias).TabIndex = 4;
		((Control)tbAlias).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)groupResources).Controls.Add((Control)(object)listResources);
		((Control)groupResources).Dock = (DockStyle)2;
		groupResources.HeaderHeight = 24;
		((Control)groupResources).Location = new Point(0, 201);
		((Control)groupResources).Name = "groupResources";
		((Control)groupResources).Size = new Size(497, 121);
		((Control)groupResources).TabIndex = 5;
		((Control)groupResources).TabStop = false;
		((Control)groupResources).Text = "Resources";
		((Control)groupResources).Visible = false;
		((Control)listResources).Anchor = (AnchorStyles)15;
		listResources.AutoScroll = true;
		((Control)listResources).BackColor = Color.White;
		((Control)listResources).Location = new Point(10, 29);
		((Control)listResources).Name = "listResources";
		((Control)listResources).Size = new Size(480, 86);
		((Control)listResources).TabIndex = 0;
		images.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("images.ImageStream");
		images.TransparentColor = Color.Transparent;
		images.Images.SetKeyName(0, "Json");
		((Label)labelSeparator).ImageAlign = (ContentAlignment)64;
		((Control)labelSeparator).Location = new Point(12, 10);
		((Control)labelSeparator).Name = "labelSeparator";
		((Control)labelSeparator).Size = new Size(136, 17);
		((Control)labelSeparator).TabIndex = 9;
		((Control)labelSeparator).Text = "Separator";
		((Label)labelSeparator).TextAlign = (ContentAlignment)16;
		((Control)textBoxSeparator).Location = new Point(152, 6);
		textBoxSeparator.MaxLength = 1;
		((Control)textBoxSeparator).Name = "textBoxSeparator";
		((Control)textBoxSeparator).Size = new Size(173, 21);
		((Control)textBoxSeparator).TabIndex = 10;
		((Control)textBoxSeparator).Text = "|";
		((Label)labelDataType).ImageAlign = (ContentAlignment)64;
		((Control)labelDataType).Location = new Point(12, 10);
		((Control)labelDataType).Name = "labelDataType";
		((Control)labelDataType).Size = new Size(136, 17);
		((Control)labelDataType).TabIndex = 7;
		((Control)labelDataType).Text = "Data Type";
		((Label)labelDataType).TextAlign = (ContentAlignment)16;
		comboBoxDataType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDataType).FormattingEnabled = true;
		((Control)comboBoxDataType).Location = new Point(152, 6);
		((Control)comboBoxDataType).Name = "comboBoxDataType";
		((Control)comboBoxDataType).Size = new Size(173, 21);
		((Control)comboBoxDataType).TabIndex = 8;
		((Control)panelName).Controls.Add((Control)(object)labelName);
		((Control)panelName).Controls.Add((Control)(object)tbName);
		((Control)panelName).Dock = (DockStyle)1;
		((Control)panelName).Location = new Point(0, 0);
		((Control)panelName).Name = "panelName";
		((Control)panelName).Size = new Size(497, 39);
		((Control)panelName).TabIndex = 0;
		((Control)panelAlias).Controls.Add((Control)(object)labelAlias);
		((Control)panelAlias).Controls.Add((Control)(object)tbAlias);
		((Control)panelAlias).Dock = (DockStyle)1;
		((Control)panelAlias).Location = new Point(0, 39);
		((Control)panelAlias).Name = "panelAlias";
		((Control)panelAlias).Size = new Size(497, 33);
		((Control)panelAlias).TabIndex = 1;
		((Control)panelPathData).Controls.Add((Control)(object)panelGuid);
		((Control)panelPathData).Controls.Add((Control)(object)bePathData);
		((Control)panelPathData).Controls.Add((Control)(object)labelFilePath);
		((Control)panelPathData).Dock = (DockStyle)1;
		((Control)panelPathData).Location = new Point(0, 72);
		((Control)panelPathData).Name = "panelPathData";
		((Control)panelPathData).Size = new Size(497, 51);
		((Control)panelPathData).TabIndex = 2;
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebName);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileSeparator);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileName);
		((Control)panelGuid).Location = new Point(154, 31);
		((Control)panelGuid).Name = "panelGuid";
		((Control)panelGuid).Size = new Size(333, 19);
		((Control)panelGuid).TabIndex = 23;
		((Label)labelGuidWebValue).AutoEllipsis = true;
		((Control)labelGuidWebValue).AutoSize = true;
		((Control)labelGuidWebValue).Dock = (DockStyle)3;
		((Control)labelGuidWebValue).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidWebValue).Location = new Point(122, 0);
		((Control)labelGuidWebValue).Name = "labelGuidWebValue";
		((Control)labelGuidWebValue).Size = new Size(122, 12);
		((Control)labelGuidWebValue).TabIndex = 22;
		((Control)labelGuidWebValue).Text = "https://www.site.com/data/";
		((Label)labelGuidWebValue).TextAlign = (ContentAlignment)16;
		((Control)labelGuidWebValue).MouseDown += new MouseEventHandler(LabelGuidFileValue_MouseDown);
		((Control)labelGuidWebName).AutoSize = true;
		((Control)labelGuidWebName).Dock = (DockStyle)3;
		((Control)labelGuidWebName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidWebName).Location = new Point(91, 0);
		((Control)labelGuidWebName).Name = "labelGuidWebName";
		((Control)labelGuidWebName).Size = new Size(31, 12);
		((Control)labelGuidWebName).TabIndex = 23;
		((Control)labelGuidWebName).Text = "Web:";
		((Label)labelGuidWebName).TextAlign = (ContentAlignment)16;
		((Label)labelGuidFileSeparator).AutoEllipsis = true;
		((Control)labelGuidFileSeparator).AutoSize = true;
		((Control)labelGuidFileSeparator).Dock = (DockStyle)3;
		((Control)labelGuidFileSeparator).Font = new Font("Microsoft Sans Serif", 6.75f);
		((Control)labelGuidFileSeparator).Location = new Point(76, 0);
		((Control)labelGuidFileSeparator).Name = "labelGuidFileSeparator";
		((Control)labelGuidFileSeparator).Size = new Size(15, 12);
		((Control)labelGuidFileSeparator).TabIndex = 24;
		((Control)labelGuidFileSeparator).Text = "  |  ";
		((Label)labelGuidFileSeparator).TextAlign = (ContentAlignment)16;
		((Label)labelGuidFileValue).AutoEllipsis = true;
		((Control)labelGuidFileValue).AutoSize = true;
		((Control)labelGuidFileValue).Dock = (DockStyle)3;
		((Control)labelGuidFileValue).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidFileValue).Location = new Point(28, 0);
		((Control)labelGuidFileValue).Name = "labelGuidFileValue";
		((Control)labelGuidFileValue).Size = new Size(48, 12);
		((Control)labelGuidFileValue).TabIndex = 18;
		((Control)labelGuidFileValue).Text = "c:\\file.json";
		((Label)labelGuidFileValue).TextAlign = (ContentAlignment)16;
		((Control)labelGuidFileValue).MouseDown += new MouseEventHandler(LabelGuidFileValue_MouseDown);
		((Control)labelGuidFileName).AutoSize = true;
		((Control)labelGuidFileName).Dock = (DockStyle)3;
		((Control)labelGuidFileName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidFileName).Location = new Point(0, 0);
		((Control)labelGuidFileName).Name = "labelGuidFileName";
		((Control)labelGuidFileName).Size = new Size(28, 12);
		((Control)labelGuidFileName).TabIndex = 19;
		((Control)labelGuidFileName).Text = "File:";
		((Label)labelGuidFileName).TextAlign = (ContentAlignment)16;
		((Control)panelDataType).Controls.Add((Control)(object)labelDataType);
		((Control)panelDataType).Controls.Add((Control)(object)comboBoxDataType);
		((Control)panelDataType).Dock = (DockStyle)1;
		((Control)panelDataType).Location = new Point(0, 123);
		((Control)panelDataType).Name = "panelDataType";
		((Control)panelDataType).Size = new Size(497, 33);
		((Control)panelDataType).TabIndex = 3;
		((Control)panelSeparator).Controls.Add((Control)(object)labelSeparator);
		((Control)panelSeparator).Controls.Add((Control)(object)textBoxSeparator);
		((Control)panelSeparator).Dock = (DockStyle)1;
		((Control)panelSeparator).Location = new Point(0, 156);
		((Control)panelSeparator).Name = "panelSeparator";
		((Control)panelSeparator).Size = new Size(497, 33);
		((Control)panelSeparator).TabIndex = 4;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(497, 370);
		((Control)this).Controls.Add((Control)(object)panelSeparator);
		((Control)this).Controls.Add((Control)(object)panelDataType);
		((Control)this).Controls.Add((Control)(object)panelPathData);
		((Control)this).Controls.Add((Control)(object)panelAlias);
		((Control)this).Controls.Add((Control)(object)panelName);
		((Control)this).Controls.Add((Control)(object)groupResources);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiGisDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Json Edit";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Load += ThisLoad;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)groupResources).ResumeLayout(false);
		((Control)panelName).ResumeLayout(false);
		((Control)panelAlias).ResumeLayout(false);
		((Control)panelPathData).ResumeLayout(false);
		((Control)panelGuid).ResumeLayout(false);
		((Control)panelGuid).PerformLayout();
		((Control)panelDataType).ResumeLayout(false);
		((Control)panelSeparator).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
