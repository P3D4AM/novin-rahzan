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

public class StiJsonDatabaseEditForm : StiFormUX
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

	private StiOffice2013GroupBox groupResources;

	private ScrollableControl listResources;

	private ToolTip toolTip;

	private ImageList images;

	private StiLabelUX labelRelationDirection;

	public StiComboBox comboBoxRelationDirection;

	private StiLabelUX labelGuidFileName;

	private StiSampleLabelUX labelGuidFileValue;

	private StiPanelUX panelGuid;

	private StiSampleLabelUX labelGuidWebValue;

	private StiLabelUX labelGuidWebName;

	private StiLabelUX labelGuidFileSeparator;

	public StiRelationDirection RelationDirection
	{
		get
		{
			if (((ListControl)comboBoxRelationDirection).SelectedIndex != 1)
			{
				return StiRelationDirection.ParentToChild;
			}
			return StiRelationDirection.ChildToParent;
		}
		set
		{
			if (value == StiRelationDirection.ChildToParent)
			{
				((ListControl)comboBoxRelationDirection).SelectedIndex = 1;
			}
			else
			{
				((ListControl)comboBoxRelationDirection).SelectedIndex = 0;
			}
		}
	}

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(56.0);

	public StiJsonDatabaseEditForm(StiReport report, string originalName)
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
			StiJsonDatabaseEditForm stiJsonDatabaseEditForm = this;
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
			((Control)stiJsonDatabaseEditForm).Invoke((Delegate)(object)obj2);
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
			where v.Type == StiResourceType.Json && v.Content != null
			select v).ToList();
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelFilePath).Text = Loc.GetCleaned("FormDatabaseEdit", "PathJsonData");
		((Control)groupResources).Text = StiLocalization.Get("PropertyMain", "Resources");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)btMoveToResource).Text = StiLocalization.Get("Buttons", "MoveToResource");
		((Control)labelRelationDirection).Text = Loc.GetCleaned("FormDatabaseEdit", "RelationDirection");
		((ComboBox)comboBoxRelationDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "RelationDirectionParentToChild"));
		((ComboBox)comboBoxRelationDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "RelationDirectionChildToParent"));
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
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceJson" + bigStepName + ".png");
			images.ImageSize = ((Image)image).Size;
			images.Images.Add("Excel", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceJson" + bigStepName + ".png"));
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
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).DefaultExt = "json";
			((FileDialog)val).Filter = "JSON files|*.json";
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
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm.htm");
	}

	private void ThisLoad(object sender, EventArgs e)
	{
		BuildGallery();
	}

	private void TextBox_KeyDown(object sender, KeyEventArgs e)
	{
		isNameChanged = true;
	}

	private void LabelGuid_MouseDown(object sender, MouseEventArgs e)
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
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Expected O, but got Unknown
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Expected O, but got Unknown
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Expected O, but got Unknown
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Expected O, but got Unknown
		//IL_0982: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Expected O, but got Unknown
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Expected O, but got Unknown
		//IL_0af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b02: Expected O, but got Unknown
		//IL_0b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Expected O, but got Unknown
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Expected O, but got Unknown
		//IL_0c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiJsonDatabaseEditForm));
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
		labelRelationDirection = new StiLabelUX();
		comboBoxRelationDirection = new StiComboBox();
		labelGuidFileName = new StiLabelUX();
		labelGuidFileValue = new StiSampleLabelUX();
		panelGuid = new StiPanelUX();
		labelGuidWebValue = new StiSampleLabelUX();
		labelGuidWebName = new StiLabelUX();
		labelGuidFileSeparator = new StiLabelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((Control)groupResources).SuspendLayout();
		((Control)panelGuid).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(428, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 25);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(347, 12);
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
		((Control)labelName).TabIndex = 5;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(152, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(247, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += OnNameEnter;
		((Control)tbName).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)tbName).Leave += OnNameLeave;
		((Control)labelFilePath).Location = new Point(12, 78);
		((Control)labelFilePath).Name = "labelFilePath";
		((Control)labelFilePath).Size = new Size(136, 20);
		((Control)labelFilePath).TabIndex = 7;
		((Control)labelFilePath).Text = "Path Data";
		((Label)labelFilePath).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)bePathData).Anchor = (AnchorStyles)13;
		bePathData.ButtonIconKind = StiButtonIconKind.Dots;
		bePathData.Flat = false;
		((Control)bePathData).Location = new Point(152, 78);
		((Control)bePathData).Name = "bePathData";
		((Control)bePathData).Size = new Size(351, 21);
		((Control)bePathData).TabIndex = 2;
		bePathData.ButtonClick += OnPathDataClick;
		((Control)glButtons).Controls.Add((Control)(object)btMoveToResource);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 278);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(513, 48);
		((Control)glButtons).TabIndex = 5;
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(136, 16);
		((Control)labelAlias).TabIndex = 6;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(152, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(247, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)tbAlias).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)groupResources).Controls.Add((Control)(object)listResources);
		((Control)groupResources).Dock = (DockStyle)2;
		groupResources.HeaderHeight = 24;
		((Control)groupResources).Location = new Point(0, 157);
		((Control)groupResources).Name = "groupResources";
		((Control)groupResources).Size = new Size(513, 121);
		((Control)groupResources).TabIndex = 4;
		((Control)groupResources).TabStop = false;
		((Control)groupResources).Text = "Resources";
		((Control)groupResources).Visible = false;
		((Control)listResources).Anchor = (AnchorStyles)15;
		listResources.AutoScroll = true;
		((Control)listResources).BackColor = Color.White;
		((Control)listResources).Location = new Point(10, 29);
		((Control)listResources).Name = "listResources";
		((Control)listResources).Size = new Size(496, 86);
		((Control)listResources).TabIndex = 0;
		images.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("images.ImageStream");
		images.TransparentColor = Color.Transparent;
		images.Images.SetKeyName(0, "Json");
		((Label)labelRelationDirection).ImageAlign = (ContentAlignment)64;
		((Control)labelRelationDirection).Location = new Point(12, 128);
		((Control)labelRelationDirection).Name = "labelRelationDirection";
		((Control)labelRelationDirection).Size = new Size(136, 17);
		((Control)labelRelationDirection).TabIndex = 6;
		((Control)labelRelationDirection).Text = "Relation direction";
		((Label)labelRelationDirection).TextAlign = (ContentAlignment)16;
		comboBoxRelationDirection.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxRelationDirection).FormattingEnabled = true;
		((Control)comboBoxRelationDirection).Location = new Point(152, 126);
		((Control)comboBoxRelationDirection).Name = "comboBoxRelationDirection";
		((Control)comboBoxRelationDirection).Size = new Size(173, 21);
		((Control)comboBoxRelationDirection).TabIndex = 3;
		((Control)labelGuidFileName).AutoSize = true;
		((Control)labelGuidFileName).Dock = (DockStyle)3;
		((Control)labelGuidFileName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidFileName).Location = new Point(0, 0);
		((Control)labelGuidFileName).Name = "labelGuidFileName";
		((Control)labelGuidFileName).Size = new Size(28, 12);
		((Control)labelGuidFileName).TabIndex = 19;
		((Control)labelGuidFileName).Text = "File:";
		((Label)labelGuidFileName).TextAlign = (ContentAlignment)16;
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
		((Control)labelGuidFileValue).MouseDown += new MouseEventHandler(LabelGuid_MouseDown);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebName);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileSeparator);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileName);
		((Control)panelGuid).Location = new Point(152, 103);
		((Control)panelGuid).Name = "panelGuid";
		((Control)panelGuid).Size = new Size(361, 19);
		((Control)panelGuid).TabIndex = 20;
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
		((Control)labelGuidWebValue).MouseDown += new MouseEventHandler(LabelGuid_MouseDown);
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
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(513, 326);
		((Control)this).Controls.Add((Control)(object)panelGuid);
		((Control)this).Controls.Add((Control)(object)labelRelationDirection);
		((Control)this).Controls.Add((Control)(object)comboBoxRelationDirection);
		((Control)this).Controls.Add((Control)(object)groupResources);
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
		((Control)this).Name = "StiJsonDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Json Edit";
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
