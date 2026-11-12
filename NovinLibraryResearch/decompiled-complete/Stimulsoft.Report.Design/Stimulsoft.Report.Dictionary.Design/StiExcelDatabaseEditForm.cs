using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiExcelDatabaseEditForm : StiFormUX
{
	private StiReport report;

	private string oldValue;

	private string originalName;

	private bool isNameChanged;

	private IContainer components;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	private StiButtonUX btMoveToResource;

	public StiTextBox tbName;

	private ErrorProvider errorProvider;

	private StiLabelUX labelFilePath;

	public StiButtonEdit bePathData;

	private StiButtonPanelUX glButtons;

	private StiLabelUX labelName;

	private StiLabelUX labelAlias;

	public StiTextBox tbAlias;

	public StiCheckBoxUX checkBoxFirstRowIsHeader;

	private StiOffice2013GroupBox groupResources;

	private ScrollableControl listResources;

	private ImageList images;

	private ToolTip toolTip;

	private StiPanelUX panelGuid;

	private StiSampleLabelUX labelGuidWebValue;

	private StiLabelUX labelGuidWebName;

	private StiLabelUX labelGuidFileSeparator;

	private StiSampleLabelUX labelGuidFileValue;

	private StiLabelUX labelGuidFileName;

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(56.0);

	public StiExcelDatabaseEditForm(StiReport report, string originalName)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			DoTheming();
			DoScaling();
			this.report = report;
			this.originalName = originalName;
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			((Control)bePathData).TextChanged += OnPathDataTextChanged;
		}
	}

	private void BuildGallery()
	{
		List<StiResource> resources = GetResources();
		((Control)groupResources).Visible = resources.Any();
		((Control)listResources).Controls.Clear();
		if (!((Control)groupResources).Visible)
		{
			((Control)this).Height = ((Control)this).Height - ((Control)groupResources).Height;
		}
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
							toolTip.SetToolTip((Control)(object)button, string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "Resource"), resource.Name));
							((Control)listResources).Controls.Add((Control)(object)button);
						}
					});
					num += ((Control)button).Width + ItemGalleryStep;
				}
				catch
				{
				}
			}
			StiExcelDatabaseEditForm stiExcelDatabaseEditForm = this;
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
			((Control)stiExcelDatabaseEditForm).Invoke((Delegate)(object)obj2);
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
			where v.Type == StiResourceType.Excel && v.Content != null
			select v).ToList();
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelFilePath).Text = Loc.GetCleaned("FormDatabaseEdit", "PathToData");
		((Control)checkBoxFirstRowIsHeader).Text = StiLocalization.Get("FormDatabaseEdit", "FirstRowIsHeader");
		((Control)groupResources).Text = StiLocalization.Get("PropertyMain", "Resources");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)btMoveToResource).Text = StiLocalization.Get("Buttons", "MoveToResource");
		((Control)labelGuidFileName).Text = Loc.GetMain("File") + ":";
		((Control)labelGuidWebName).Text = Loc.Get("FormColorBoxPopup", "Web") + ":";
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)listResources).BackColor = StiUX.Background;
		}
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
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceExcel" + bigStepName + ".png");
			images.ImageSize = ((Image)image).Size;
			images.Images.Add("Excel", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceExcel" + bigStepName + ".png"));
		}
		if (bePathData.ButtonBitmap != null)
		{
			bePathData.ButtonBitmap = (Image)(object)StiImageUtils.ResizeImage(bePathData.ButtonBitmap, StiScale.I(bePathData.ButtonBitmap.Width), StiScale.I(bePathData.ButtonBitmap.Height));
		}
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
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
				StiResource resource = new StiResource(text, text, typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(((Control)bePathData).Text));
				report.Dictionary.Resources.Add(resource);
				((Control)bePathData).Text = "resource://" + text;
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
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			if (!string.IsNullOrEmpty(((Control)bePathData).Text) && !((Control)bePathData).Text.StartsWithInvariant("resource://"))
			{
				((FileDialog)val).InitialDirectory = Path.GetDirectoryName(((Control)bePathData).Text);
				((FileDialog)val).FileName = Path.GetFileName(((Control)bePathData).Text);
			}
			((FileDialog)val).DefaultExt = "xlsx";
			((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "ExcelAllFiles");
			((FileDialog)val).RestoreDirectory = true;
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
	}

	private void TextBox_KeyDown(object sender, KeyEventArgs e)
	{
		isNameChanged = true;
	}

	private void LabelGuidFileValue_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)bePathData).Text = "c:\\file.xlsx";
	}

	private void LabelGuidWebValue_MouseDown(object sender, MouseEventArgs e)
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Expected O, but got Unknown
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Expected O, but got Unknown
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a96: Expected O, but got Unknown
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3c: Expected O, but got Unknown
		//IL_0baa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Expected O, but got Unknown
		//IL_0be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bed: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiExcelDatabaseEditForm));
		labelName = new StiLabelUX();
		tbName = new StiTextBox();
		labelFilePath = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		labelAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		checkBoxFirstRowIsHeader = new StiCheckBoxUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		btMoveToResource = new StiButtonUX();
		glButtons = new StiButtonPanelUX();
		bePathData = new StiButtonEdit();
		groupResources = new StiOffice2013GroupBox();
		listResources = new ScrollableControl();
		images = new ImageList(components);
		toolTip = new ToolTip(components);
		panelGuid = new StiPanelUX();
		labelGuidWebValue = new StiSampleLabelUX();
		labelGuidWebName = new StiLabelUX();
		labelGuidFileSeparator = new StiLabelUX();
		labelGuidFileValue = new StiSampleLabelUX();
		labelGuidFileName = new StiLabelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)groupResources).SuspendLayout();
		((Control)panelGuid).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(126, 20);
		((Control)labelName).TabIndex = 5;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(152, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(251, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += OnNameEnter;
		((Control)tbName).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)tbName).Leave += OnNameLeave;
		((Control)labelFilePath).Location = new Point(12, 78);
		((Control)labelFilePath).Name = "labelFilePath";
		((Control)labelFilePath).Size = new Size(126, 20);
		((Control)labelFilePath).TabIndex = 7;
		((Control)labelFilePath).Text = "Path Data";
		((Label)labelFilePath).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(126, 20);
		((Control)labelAlias).TabIndex = 6;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(152, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(251, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)tbAlias).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)checkBoxFirstRowIsHeader).Anchor = (AnchorStyles)13;
		((CheckBox)checkBoxFirstRowIsHeader).Checked = true;
		((CheckBox)checkBoxFirstRowIsHeader).CheckState = (CheckState)1;
		((Control)checkBoxFirstRowIsHeader).Location = new Point(152, 126);
		((Control)checkBoxFirstRowIsHeader).Name = "checkBoxFirstRowIsHeader";
		((Control)checkBoxFirstRowIsHeader).Size = new Size(353, 21);
		((Control)checkBoxFirstRowIsHeader).TabIndex = 3;
		((Control)checkBoxFirstRowIsHeader).Text = "First Row is Header";
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(347, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += OnOkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(430, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btMoveToResource).Anchor = (AnchorStyles)6;
		((Control)btMoveToResource).Location = new Point(12, 12);
		((Control)btMoveToResource).Name = "btMoveToResource";
		((Control)btMoveToResource).Size = new Size(150, 23);
		((Control)btMoveToResource).TabIndex = 0;
		((Control)btMoveToResource).Text = "Move to Resource";
		((ButtonBase)btMoveToResource).UseVisualStyleBackColor = true;
		((Control)btMoveToResource).Click += OnMoveToResourceClick;
		((Control)glButtons).Controls.Add((Control)(object)btMoveToResource);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 272);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(517, 46);
		((Control)glButtons).TabIndex = 5;
		((Control)bePathData).Anchor = (AnchorStyles)13;
		bePathData.ButtonIconKind = StiButtonIconKind.Dots;
		bePathData.Flat = false;
		((Control)bePathData).Location = new Point(152, 78);
		((Control)bePathData).Name = "bePathData";
		((Control)bePathData).Size = new Size(353, 21);
		((Control)bePathData).TabIndex = 2;
		bePathData.ButtonClick += OnPathDataClick;
		((Control)groupResources).Controls.Add((Control)(object)listResources);
		((Control)groupResources).Dock = (DockStyle)2;
		groupResources.HeaderHeight = 24;
		((Control)groupResources).Location = new Point(0, 151);
		((Control)groupResources).Name = "groupResources";
		((Control)groupResources).Size = new Size(517, 121);
		((Control)groupResources).TabIndex = 4;
		((Control)groupResources).TabStop = false;
		((Control)groupResources).Text = "Resources";
		((Control)groupResources).Visible = false;
		((Control)listResources).Anchor = (AnchorStyles)15;
		listResources.AutoScroll = true;
		((Control)listResources).BackColor = Color.White;
		((Control)listResources).Location = new Point(4, 29);
		((Control)listResources).Name = "listResources";
		((Control)listResources).Size = new Size(508, 87);
		((Control)listResources).TabIndex = 0;
		images.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("images.ImageStream");
		images.TransparentColor = Color.Transparent;
		images.Images.SetKeyName(0, "Excel");
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebName);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileSeparator);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileName);
		((Control)panelGuid).Location = new Point(152, 103);
		((Control)panelGuid).Name = "panelGuid";
		((Control)panelGuid).Size = new Size(361, 19);
		((Control)panelGuid).TabIndex = 21;
		((Label)labelGuidWebValue).AutoEllipsis = true;
		((Control)labelGuidWebValue).AutoSize = true;
		((Control)labelGuidWebValue).Dock = (DockStyle)3;
		((Control)labelGuidWebValue).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidWebValue).Location = new Point(165, 0);
		((Control)labelGuidWebValue).Name = "labelGuidWebValue";
		((Control)labelGuidWebValue).Size = new Size(122, 12);
		((Control)labelGuidWebValue).TabIndex = 22;
		((Control)labelGuidWebValue).Text = "https://www.site.com/data/";
		((Label)labelGuidWebValue).TextAlign = (ContentAlignment)16;
		((Control)labelGuidWebValue).MouseDown += new MouseEventHandler(LabelGuidWebValue_MouseDown);
		((Control)labelGuidWebName).AutoSize = true;
		((Control)labelGuidWebName).Dock = (DockStyle)3;
		((Control)labelGuidWebName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidWebName).Location = new Point(134, 0);
		((Control)labelGuidWebName).Name = "labelGuidWebName";
		((Control)labelGuidWebName).Size = new Size(31, 12);
		((Control)labelGuidWebName).TabIndex = 23;
		((Control)labelGuidWebName).Text = "Web:";
		((Label)labelGuidWebName).TextAlign = (ContentAlignment)16;
		((Label)labelGuidFileSeparator).AutoEllipsis = true;
		((Control)labelGuidFileSeparator).AutoSize = true;
		((Control)labelGuidFileSeparator).Dock = (DockStyle)3;
		((Control)labelGuidFileSeparator).Font = new Font("Microsoft Sans Serif", 6.75f);
		((Control)labelGuidFileSeparator).Location = new Point(119, 0);
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
		((Control)labelGuidFileValue).Size = new Size(91, 12);
		((Control)labelGuidFileValue).TabIndex = 18;
		((Control)labelGuidFileValue).Text = "c:\\file.xlsx; c:\\file.xls";
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
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(517, 318);
		((Control)this).Controls.Add((Control)(object)panelGuid);
		((Control)this).Controls.Add((Control)(object)groupResources);
		((Control)this).Controls.Add((Control)(object)checkBoxFirstRowIsHeader);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)bePathData);
		((Control)this).Controls.Add((Control)(object)labelFilePath);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiExcelDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Excel Edit";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Load += ThisLoad;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((Control)groupResources).ResumeLayout(false);
		((Control)panelGuid).ResumeLayout(false);
		((Control)panelGuid).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
