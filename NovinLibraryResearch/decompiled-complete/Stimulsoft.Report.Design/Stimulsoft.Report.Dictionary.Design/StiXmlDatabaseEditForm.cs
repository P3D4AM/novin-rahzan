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

public class StiXmlDatabaseEditForm : StiFormUX
{
	private string originalName;

	private string oldValue;

	private StiReport report;

	private int resFormHeight;

	private bool isNameChanged;

	private IContainer components;

	private StiButtonUX buttonCancel;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonMoveToResource;

	private StiTextBox textBoxName;

	private ErrorProvider errorProvider;

	private StiLabelUX labelPathSchema;

	private StiLabelUX labelPathData;

	private StiButtonEdit buttonEditPathSchema;

	private StiButtonEdit buttonEditPathData;

	private StiButtonPanelUX groupLineButtons;

	private StiLabelUX labelName;

	private StiLabelUX labelAlias;

	private StiTextBox textBoxAlias;

	private StiLabelUX labelXmlType;

	private StiComboBox comboBoxXmlType;

	private StiOffice2013GroupBox groupResources;

	private ScrollableControl listResources;

	private ToolTip toolTip;

	private ImageList images;

	private StiComboBox comboBoxRelationDirection;

	private StiLabelUX labelRelationDirection;

	private Panel panelGuid;

	private StiLabelUX labelGuidWebName;

	private StiLabelUX labelGuidFileSeparator;

	private StiLabelUX labelGuidFileName;

	private StiPanelUX panelSchemaGuid;

	private StiLabelUX labelSchemaGuidWebName;

	private StiLabelUX labelSchemaGuidFileSeparator;

	private StiLabelUX labelSchemaGuidFileName;

	private StiSampleLabelUX labelGuidWebValue;

	private StiSampleLabelUX labelGuidFileValue;

	private StiSampleLabelUX labelSchemaGuidWebValue;

	private StiSampleLabelUX labelSchemaGuidFileValue;

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

	public string DataName
	{
		get
		{
			return ((Control)textBoxName).Text;
		}
		set
		{
			((Control)textBoxName).Text = value;
		}
	}

	public string DataAlias
	{
		get
		{
			return ((Control)textBoxAlias).Text;
		}
		set
		{
			((Control)textBoxAlias).Text = value;
		}
	}

	public StiXmlType DataXmlType
	{
		get
		{
			if (((ListControl)comboBoxXmlType).SelectedIndex != 0)
			{
				return StiXmlType.Xml;
			}
			return StiXmlType.AdoNetXml;
		}
		set
		{
			((ListControl)comboBoxXmlType).SelectedIndex = ((value != 0) ? 1 : 0);
		}
	}

	public string FilePathSchema
	{
		get
		{
			return ((Control)buttonEditPathSchema).Text;
		}
		set
		{
			((Control)buttonEditPathSchema).Text = value;
		}
	}

	public string FilePathData
	{
		get
		{
			return ((Control)buttonEditPathData).Text;
		}
		set
		{
			((Control)buttonEditPathData).Text = value;
		}
	}

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(56.0);

	public StiXmlDatabaseEditForm(StiReport report, string originalName)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		DoTheming();
		DoScaling();
		resFormHeight = ((Control)this).Height - ((Control)groupResources).Height - ((Control)buttonEditPathSchema).Height - (int)Math.Ceiling(12.0 * StiScale.Y);
		if (!((Component)(object)this).DesignMode)
		{
			this.report = report;
			this.originalName = originalName;
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			((Control)buttonEditPathSchema).TextChanged += ButtonEditPathSchema_TextChanged;
			((Control)buttonEditPathData).TextChanged += ButtonEditPathData_TextChanged;
		}
	}

	private void BuildGallery()
	{
		List<StiResource> resources = GetResources();
		((Control)groupResources).Visible = resources.Any();
		((Control)listResources).Controls.Clear();
		SetFormSize();
		MethodInvoker val = default(MethodInvoker);
		Task.Factory.StartNew(delegate
		{
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			//IL_01d5: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Expected O, but got Unknown
			int num = ItemGalleryStep;
			int itemGalleryStep = ItemGalleryStep;
			string resourceNameFromHyperlink = StiHyperlinkProcessor.GetResourceNameFromHyperlink(((Control)buttonEditPathSchema).Text);
			string resourceNameFromHyperlink2 = StiHyperlinkProcessor.GetResourceNameFromHyperlink(((Control)buttonEditPathData).Text);
			foreach (StiResource resource in resources)
			{
				try
				{
					StiImageButton button = CreateGalleryButton(resource.Name, GetResourceImage(resource), resource, num, itemGalleryStep, ItemGallerySize);
					if (resource.Type == StiResourceType.Xsd)
					{
						((CheckBox)button).Checked = resourceNameFromHyperlink == resource.Name;
					}
					if (resource.Type == StiResourceType.Xml)
					{
						((CheckBox)button).Checked = resourceNameFromHyperlink2 == resource.Name;
					}
					((CheckBox)button).CheckedChanged += delegate(object sender, EventArgs e)
					{
						ItemGalleryClick(sender);
					};
					((CheckBox)button).DoubleClick += This_OkClick;
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
			StiXmlDatabaseEditForm stiXmlDatabaseEditForm = this;
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
			((Control)stiXmlDatabaseEditForm).Invoke((Delegate)(object)obj2);
		});
	}

	private void ItemGalleryClick(object sender)
	{
		if (!(sender is StiImageButton stiImageButton) || !((CheckBox)stiImageButton).Checked)
		{
			return;
		}
		StiResource resource = ((Control)stiImageButton).Tag as StiResource;
		if (resource == null)
		{
			return;
		}
		(from StiImageButton r in from Control c in (IEnumerable)((Control)listResources).Controls
				where c is StiImageButton && c != sender
				select c
			where ((Control)r).Tag is StiResource && ((StiResource)((Control)r).Tag).Type == resource.Type
			select r).ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (resource.Type == StiResourceType.Xml)
		{
			((Control)buttonEditPathData).Text = StiHyperlinkProcessor.CreateResourceName(resource.Name);
		}
		else if (resource.Type == StiResourceType.Xsd)
		{
			if (((ListControl)comboBoxXmlType).SelectedIndex != 0)
			{
				((ListControl)comboBoxXmlType).SelectedIndex = 0;
			}
			((Control)buttonEditPathSchema).Text = StiHyperlinkProcessor.CreateResourceName(resource.Name);
		}
	}

	private Image GetResourceImage(StiResource resource)
	{
		if (resource.Type != StiResourceType.Xml)
		{
			return images.Images["Xsd"];
		}
		return images.Images["Xml"];
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

	private void SetFormSize()
	{
		((Control)this).Height = resFormHeight + GetPathSchemaHeight() + GetReverseCheckHeight() + GetGalleryHeight() + (int)Math.Ceiling(6.0 * StiScale.Y);
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
			where (v.Type == StiResourceType.Xml || v.Type == StiResourceType.Xsd) && v.Content != null
			select v).ToList();
	}

	private int GetGalleryHeight()
	{
		if (!((Control)groupResources).Visible)
		{
			return 0;
		}
		return ((Control)groupResources).Height;
	}

	private int GetPathSchemaHeight()
	{
		if (!((Control)buttonEditPathSchema).Visible)
		{
			return 0;
		}
		return ((Control)buttonEditPathSchema).Height + (int)Math.Ceiling(12.0 * StiScale.Y);
	}

	private int GetReverseCheckHeight()
	{
		if (!((Control)comboBoxRelationDirection).Visible)
		{
			return 0;
		}
		return ((Control)buttonEditPathSchema).Height + (int)Math.Ceiling(12.0 * StiScale.Y);
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelPathSchema).Text = Loc.GetCleaned("FormDatabaseEdit", "PathSchema");
		((Control)labelPathData).Text = Loc.GetCleaned("FormDatabaseEdit", "PathData");
		((Control)labelXmlType).Text = Loc.GetCleaned("FormDatabaseEdit", "XmlType");
		((Control)groupResources).Text = StiLocalization.Get("PropertyMain", "Resources");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)buttonMoveToResource).Text = StiLocalization.Get("Buttons", "MoveToResource");
		((ComboBox)comboBoxXmlType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiXmlTypeAdoNetXml"));
		((ComboBox)comboBoxXmlType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiXmlTypeXml"));
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		double step = StiScale.Step;
		if (step >= 1.5)
		{
			string bigStepName = StiScale.BigStepName;
			images = new ImageList();
			images.TransparentColor = Color.Transparent;
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceXml" + bigStepName + ".png");
			images.ImageSize = ((Image)image).Size;
			images.Images.Add("Xml", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceXml" + bigStepName + ".png"));
			images.Images.Add("Xsd", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceXsd" + bigStepName + ".png"));
		}
		if (buttonEditPathData.ButtonBitmap != null)
		{
			buttonEditPathData.ButtonBitmap = (Image)(object)StiImageUtils.ResizeImage(buttonEditPathData.ButtonBitmap, StiScale.I(buttonEditPathData.ButtonBitmap.Width), StiScale.I(buttonEditPathData.ButtonBitmap.Height));
		}
		if (buttonEditPathSchema.ButtonBitmap != null)
		{
			buttonEditPathSchema.ButtonBitmap = (Image)(object)StiImageUtils.ResizeImage(buttonEditPathSchema.ButtonBitmap, StiScale.I(buttonEditPathSchema.ButtonBitmap.Width), StiScale.I(buttonEditPathSchema.ButtonBitmap.Height));
		}
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private bool IsEditMode()
	{
		return originalName != null;
	}

	private void UpdateMoveToResourceState()
	{
		bool flag = false;
		flag = ((((ListControl)comboBoxXmlType).SelectedIndex != 0) ? (((Control)buttonEditPathData).Text.Length > 0 && !((Control)buttonEditPathData).Text.StartsWithInvariant("resource://")) : ((((Control)buttonEditPathSchema).Text.Length > 0 && !((Control)buttonEditPathSchema).Text.StartsWithInvariant("resource://")) || (((Control)buttonEditPathData).Text.Length > 0 && !((Control)buttonEditPathData).Text.StartsWithInvariant("resource://"))));
		((Control)buttonMoveToResource).Enabled = flag;
	}

	private void ButtonEditPathData_TextChanged(object sender, EventArgs e)
	{
		UpdateMoveToResourceState();
	}

	private void ButtonEditPathSchema_TextChanged(object sender, EventArgs e)
	{
		UpdateMoveToResourceState();
	}

	private void ButtonMoveToResource_Click(object sender, EventArgs e)
	{
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Invalid comparison between Unknown and I4
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Invalid comparison between Unknown and I4
		if (((Control)textBoxName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)textBoxName, StiFieldChecker.NameRequire);
			((Control)textBoxName).Focus();
			return;
		}
		if (originalName != ((Control)textBoxName).Text && StiNameCreation.IsConnectionNameExists(report, ((Control)textBoxName).Text))
		{
			errorProvider.SetError((Control)(object)textBoxName, StiFieldChecker.NameExists(((Control)textBoxName).Text));
			((Control)textBoxName).Focus();
			return;
		}
		try
		{
			if (((Control)buttonEditPathSchema).Text.Length != 0 && !((Control)buttonEditPathSchema).Text.ToLowerInvariant().StartsWithInvariant("resource://"))
			{
				string text = StiNameCreation.CreateResourceName(report, Path.GetFileNameWithoutExtension(((Control)buttonEditPathSchema).Text).Normalize());
				StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(Path.GetExtension(((Control)buttonEditPathSchema).Text).ToLowerInvariant());
				if (!typeFromExtension.HasValue)
				{
					return;
				}
				if (new FileInfo(((Control)buttonEditPathSchema).Text).Length > StiOptions.Engine.ReportResources.MaximumSize)
				{
					DialogResult val = MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("Notices", "MessageMaximumFileSizeExceeded"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4, (MessageBoxIcon)32);
					if ((int)val == 7)
					{
						((Control)buttonEditPathSchema).Focus();
						return;
					}
				}
				else
				{
					StiResource resource = new StiResource(text, text, typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(((Control)buttonEditPathSchema).Text));
					report.Dictionary.Resources.Add(resource);
					((Control)buttonEditPathSchema).Text = "resource://" + text;
				}
			}
			if (((Control)buttonEditPathData).Text.Length != 0 && !((Control)buttonEditPathData).Text.ToLowerInvariant().StartsWithInvariant("resource://"))
			{
				string text2 = StiNameCreation.CreateResourceName(report, Path.GetFileNameWithoutExtension(((Control)buttonEditPathData).Text).Normalize());
				StiResourceType? typeFromExtension2 = StiResourceTypeHelper.GetTypeFromExtension(Path.GetExtension(((Control)buttonEditPathData).Text).ToLowerInvariant());
				if (!typeFromExtension2.HasValue)
				{
					return;
				}
				if (new FileInfo(((Control)buttonEditPathData).Text).Length > StiOptions.Engine.ReportResources.MaximumSize)
				{
					DialogResult val2 = MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("Notices", "MessageMaximumFileSizeExceeded"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4, (MessageBoxIcon)32);
					if ((int)val2 == 7)
					{
						((Control)buttonEditPathData).Focus();
						return;
					}
				}
				else
				{
					StiResource resource2 = new StiResource(text2, text2, typeFromExtension2.GetValueOrDefault(), File.ReadAllBytes(((Control)buttonEditPathData).Text));
					report.Dictionary.Resources.Add(resource2);
					((Control)buttonEditPathData).Text = "resource://" + text2;
				}
			}
			((Form)this).DialogResult = (DialogResult)1;
		}
		catch
		{
		}
	}

	private void ButtonEditPathSchema_ButtonClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).DefaultExt = "xsd";
			((FileDialog)val).Filter = "XSD Schema|*.xsd|XML files|*.xml";
			((FileDialog)val).RestoreDirectory = true;
			if (!string.IsNullOrEmpty(((Control)buttonEditPathSchema).Text) && !((Control)buttonEditPathSchema).Text.StartsWithInvariant("resource://"))
			{
				((FileDialog)val).InitialDirectory = Path.GetDirectoryName(((Control)buttonEditPathSchema).Text);
				((FileDialog)val).FileName = Path.GetFileName(((Control)buttonEditPathSchema).Text);
			}
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				((Control)buttonEditPathSchema).Text = ((FileDialog)val).FileName;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonEditPathData_ButtonClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).DefaultExt = "xml";
			((FileDialog)val).Filter = "XML files|*.xml";
			((FileDialog)val).RestoreDirectory = true;
			if (!string.IsNullOrEmpty(((Control)buttonEditPathData).Text) && !((Control)buttonEditPathData).Text.StartsWithInvariant("resource://"))
			{
				((FileDialog)val).InitialDirectory = Path.GetDirectoryName(((Control)buttonEditPathData).Text);
				((FileDialog)val).FileName = Path.GetFileName(((Control)buttonEditPathData).Text);
			}
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				string baseName = Path.GetFileNameWithoutExtension(((FileDialog)val).FileName).Normalize();
				if (!isNameChanged && !IsEditMode())
				{
					((Control)textBoxName).Text = StiNameCreation.CreateConnectionName(report, baseName);
				}
				((Control)textBoxAlias).Text = ((Control)textBoxName).Text;
				((Control)buttonEditPathData).Text = ((FileDialog)val).FileName;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnNameEnter(object sender, EventArgs e)
	{
		oldValue = ((Control)textBoxName).Text;
	}

	private void TextBoxName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)textBoxAlias).Text)
		{
			((Control)textBoxAlias).Text = ((Control)textBoxName).Text;
		}
	}

	private void ComboBoxXmlType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)comboBoxXmlType).SelectedIndex == 0)
		{
			((Control)labelPathSchema).Visible = true;
			((Control)buttonEditPathSchema).Visible = true;
			((Control)panelSchemaGuid).Visible = true;
			((Control)comboBoxRelationDirection).Visible = false;
			((Control)labelRelationDirection).Visible = false;
			((Control)buttonEditPathData).Top = ((Control)buttonEditPathSchema).Top + StiScale.I(46);
			((Control)labelPathData).Top = ((Control)labelPathSchema).Top + StiScale.I(46);
		}
		else
		{
			((Control)labelPathSchema).Visible = false;
			((Control)buttonEditPathSchema).Visible = false;
			((Control)panelSchemaGuid).Visible = false;
			((Control)labelRelationDirection).Visible = true;
			((Control)comboBoxRelationDirection).Visible = true;
			((Control)buttonEditPathData).Top = ((Control)buttonEditPathSchema).Top + StiScale.I(34);
			((Control)labelPathData).Top = ((Control)labelPathSchema).Top + StiScale.I(34);
		}
		((Control)panelGuid).Top = ((Control)buttonEditPathData).Bottom + StiScale.I4;
		SetFormSize();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (((Control)textBoxName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)textBoxName, StiFieldChecker.NameRequire);
			((Control)textBoxName).Focus();
		}
		else if (originalName != ((Control)textBoxName).Text && StiNameCreation.IsConnectionNameExists(report, ((Control)textBoxName).Text))
		{
			errorProvider.SetError((Control)(object)textBoxName, StiFieldChecker.NameExists(((Control)textBoxName).Text));
			((Control)textBoxName).Focus();
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm");
	}

	private void This_Load(object sender, EventArgs e)
	{
		BuildGallery();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		isNameChanged = true;
	}

	private void LabelSchema_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)buttonEditPathSchema).Text = ((Control)(sender as StiSampleLabelUX)).Text;
	}

	private void LabelData_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)buttonEditPathData).Text = ((Control)(sender as StiSampleLabelUX)).Text;
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
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Expected O, but got Unknown
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Expected O, but got Unknown
		//IL_0acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Expected O, but got Unknown
		//IL_0c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Expected O, but got Unknown
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8e: Expected O, but got Unknown
		//IL_0cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0c: Expected O, but got Unknown
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e84: Expected O, but got Unknown
		//IL_0eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebd: Expected O, but got Unknown
		//IL_1014: Unknown result type (might be due to invalid IL or missing references)
		//IL_101e: Expected O, but got Unknown
		//IL_108f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1099: Expected O, but got Unknown
		//IL_10c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d2: Expected O, but got Unknown
		//IL_1167: Unknown result type (might be due to invalid IL or missing references)
		//IL_1171: Expected O, but got Unknown
		//IL_120d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1217: Expected O, but got Unknown
		//IL_1285: Unknown result type (might be due to invalid IL or missing references)
		//IL_128f: Expected O, but got Unknown
		//IL_12be: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c8: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiXmlDatabaseEditForm));
		labelName = new StiLabelUX();
		textBoxName = new StiTextBox();
		labelPathSchema = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		labelPathData = new StiLabelUX();
		labelAlias = new StiLabelUX();
		textBoxAlias = new StiTextBox();
		comboBoxXmlType = new StiComboBox();
		labelXmlType = new StiLabelUX();
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		buttonMoveToResource = new StiButtonUX();
		groupLineButtons = new StiButtonPanelUX();
		buttonEditPathData = new StiButtonEdit();
		buttonEditPathSchema = new StiButtonEdit();
		groupResources = new StiOffice2013GroupBox();
		listResources = new ScrollableControl();
		labelRelationDirection = new StiLabelUX();
		comboBoxRelationDirection = new StiComboBox();
		toolTip = new ToolTip(components);
		images = new ImageList(components);
		panelGuid = new Panel();
		labelGuidWebValue = new StiSampleLabelUX();
		labelGuidWebName = new StiLabelUX();
		labelGuidFileSeparator = new StiLabelUX();
		labelGuidFileValue = new StiSampleLabelUX();
		labelGuidFileName = new StiLabelUX();
		panelSchemaGuid = new StiPanelUX();
		labelSchemaGuidWebValue = new StiSampleLabelUX();
		labelSchemaGuidWebName = new StiLabelUX();
		labelSchemaGuidFileSeparator = new StiLabelUX();
		labelSchemaGuidFileValue = new StiSampleLabelUX();
		labelSchemaGuidFileName = new StiLabelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)groupLineButtons).SuspendLayout();
		((Control)groupResources).SuspendLayout();
		((Control)panelGuid).SuspendLayout();
		((Control)panelSchemaGuid).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(134, 20);
		((Control)labelName).TabIndex = 5;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)textBoxName).Anchor = (AnchorStyles)13;
		((Control)textBoxName).Location = new Point(152, 12);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(230, 21);
		((Control)textBoxName).TabIndex = 0;
		((Control)textBoxName).Enter += OnNameEnter;
		((Control)textBoxName).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)textBoxName).Leave += TextBoxName_Leave;
		((Control)labelPathSchema).Location = new Point(12, 113);
		((Control)labelPathSchema).Name = "labelPathSchema";
		((Control)labelPathSchema).Size = new Size(134, 21);
		((Control)labelPathSchema).TabIndex = 8;
		((Control)labelPathSchema).Text = "PathSchema";
		((Label)labelPathSchema).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)labelPathData).Location = new Point(12, 159);
		((Control)labelPathData).Name = "labelPathData";
		((Control)labelPathData).Size = new Size(138, 20);
		((Control)labelPathData).TabIndex = 9;
		((Control)labelPathData).Text = "PathData";
		((Label)labelPathData).TextAlign = (ContentAlignment)16;
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(134, 20);
		((Control)labelAlias).TabIndex = 6;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)textBoxAlias).Anchor = (AnchorStyles)13;
		((Control)textBoxAlias).Location = new Point(152, 45);
		((Control)textBoxAlias).Name = "textBoxAlias";
		((Control)textBoxAlias).Size = new Size(230, 21);
		((Control)textBoxAlias).TabIndex = 1;
		((Control)textBoxAlias).KeyDown += new KeyEventHandler(This_KeyDown);
		comboBoxXmlType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxXmlType).FormattingEnabled = true;
		((Control)comboBoxXmlType).Location = new Point(152, 78);
		((Control)comboBoxXmlType).Name = "comboBoxXmlType";
		((Control)comboBoxXmlType).Size = new Size(173, 21);
		((Control)comboBoxXmlType).TabIndex = 2;
		((ComboBox)comboBoxXmlType).SelectedIndexChanged += ComboBoxXmlType_SelectedIndexChanged;
		((Control)labelXmlType).Location = new Point(12, 78);
		((Control)labelXmlType).Name = "labelXmlType";
		((Control)labelXmlType).Size = new Size(134, 21);
		((Control)labelXmlType).TabIndex = 7;
		((Control)labelXmlType).Text = "Xml Type";
		((Label)labelXmlType).TextAlign = (ContentAlignment)16;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(377, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(458, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonMoveToResource).Anchor = (AnchorStyles)6;
		((Control)buttonMoveToResource).Location = new Point(12, 12);
		((Control)buttonMoveToResource).Name = "buttonMoveToResource";
		((Control)buttonMoveToResource).Size = new Size(145, 24);
		((Control)buttonMoveToResource).TabIndex = 0;
		((Control)buttonMoveToResource).Text = "Move to Resource";
		((ButtonBase)buttonMoveToResource).UseVisualStyleBackColor = true;
		((Control)buttonMoveToResource).Click += ButtonMoveToResource_Click;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonMoveToResource);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 334);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(545, 46);
		((Control)groupLineButtons).TabIndex = 11;
		((Control)buttonEditPathData).Anchor = (AnchorStyles)13;
		buttonEditPathData.ButtonIconKind = StiButtonIconKind.Dots;
		buttonEditPathData.Flat = false;
		((Control)buttonEditPathData).Location = new Point(152, 159);
		((Control)buttonEditPathData).Name = "buttonEditPathData";
		((Control)buttonEditPathData).Size = new Size(381, 21);
		((Control)buttonEditPathData).TabIndex = 4;
		buttonEditPathData.ButtonClick += ButtonEditPathData_ButtonClick;
		((Control)buttonEditPathSchema).Anchor = (AnchorStyles)13;
		buttonEditPathSchema.ButtonIconKind = StiButtonIconKind.Dots;
		buttonEditPathSchema.Flat = false;
		((Control)buttonEditPathSchema).Location = new Point(152, 111);
		((Control)buttonEditPathSchema).Name = "buttonEditPathSchema";
		((Control)buttonEditPathSchema).Size = new Size(381, 21);
		((Control)buttonEditPathSchema).TabIndex = 3;
		buttonEditPathSchema.ButtonClick += ButtonEditPathSchema_ButtonClick;
		((Control)groupResources).Controls.Add((Control)(object)listResources);
		((Control)groupResources).Dock = (DockStyle)2;
		groupResources.HeaderHeight = 24;
		((Control)groupResources).Location = new Point(0, 213);
		((Control)groupResources).Name = "groupResources";
		((Control)groupResources).Size = new Size(545, 121);
		((Control)groupResources).TabIndex = 10;
		((Control)groupResources).TabStop = false;
		((Control)groupResources).Text = "Resources";
		((Control)groupResources).Visible = false;
		((Control)listResources).Anchor = (AnchorStyles)15;
		listResources.AutoScroll = true;
		((Control)listResources).BackColor = Color.White;
		((Control)listResources).Location = new Point(10, 29);
		((Control)listResources).Name = "listResources";
		((Control)listResources).Size = new Size(525, 87);
		((Control)listResources).TabIndex = 0;
		((Control)labelRelationDirection).Location = new Point(12, 113);
		((Control)labelRelationDirection).Name = "labelRelationDirection";
		((Control)labelRelationDirection).Size = new Size(134, 21);
		((Control)labelRelationDirection).TabIndex = 6;
		((Control)labelRelationDirection).Text = "Relation direction";
		((Label)labelRelationDirection).TextAlign = (ContentAlignment)16;
		((Control)labelRelationDirection).Visible = false;
		comboBoxRelationDirection.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxRelationDirection).FormattingEnabled = true;
		((Control)comboBoxRelationDirection).Location = new Point(152, 111);
		((Control)comboBoxRelationDirection).Name = "comboBoxRelationDirection";
		((Control)comboBoxRelationDirection).Size = new Size(173, 21);
		((Control)comboBoxRelationDirection).TabIndex = 5;
		((Control)comboBoxRelationDirection).Visible = false;
		images.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("images.ImageStream");
		images.TransparentColor = Color.Transparent;
		images.Images.SetKeyName(0, "Xml");
		images.Images.SetKeyName(1, "Xsd");
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebName);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileSeparator);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileName);
		((Control)panelGuid).Location = new Point(152, 182);
		((Control)panelGuid).Name = "panelGuid";
		((Control)panelGuid).Size = new Size(333, 19);
		((Control)panelGuid).TabIndex = 24;
		((Label)labelGuidWebValue).AutoEllipsis = true;
		((Control)labelGuidWebValue).AutoSize = true;
		((Control)labelGuidWebValue).Dock = (DockStyle)3;
		((Control)labelGuidWebValue).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidWebValue).Location = new Point(120, 0);
		((Control)labelGuidWebValue).Name = "labelGuidWebValue";
		((Control)labelGuidWebValue).Size = new Size(122, 12);
		((Control)labelGuidWebValue).TabIndex = 22;
		((Control)labelGuidWebValue).Text = "https://www.site.com/data/";
		((Label)labelGuidWebValue).TextAlign = (ContentAlignment)16;
		((Control)labelGuidWebValue).MouseDown += new MouseEventHandler(LabelData_MouseDown);
		((Control)labelGuidWebName).AutoSize = true;
		((Control)labelGuidWebName).Dock = (DockStyle)3;
		((Control)labelGuidWebName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidWebName).Location = new Point(89, 0);
		((Control)labelGuidWebName).Name = "labelGuidWebName";
		((Control)labelGuidWebName).Size = new Size(31, 12);
		((Control)labelGuidWebName).TabIndex = 23;
		((Control)labelGuidWebName).Text = "Web:";
		((Label)labelGuidWebName).TextAlign = (ContentAlignment)16;
		((Label)labelGuidFileSeparator).AutoEllipsis = true;
		((Control)labelGuidFileSeparator).AutoSize = true;
		((Control)labelGuidFileSeparator).Dock = (DockStyle)3;
		((Control)labelGuidFileSeparator).Font = new Font("Microsoft Sans Serif", 6.75f);
		((Control)labelGuidFileSeparator).Location = new Point(74, 0);
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
		((Control)labelGuidFileValue).Size = new Size(46, 12);
		((Control)labelGuidFileValue).TabIndex = 18;
		((Control)labelGuidFileValue).Text = "c:\\file.xml";
		((Label)labelGuidFileValue).TextAlign = (ContentAlignment)16;
		((Control)labelGuidFileValue).MouseDown += new MouseEventHandler(LabelData_MouseDown);
		((Control)labelGuidFileName).AutoSize = true;
		((Control)labelGuidFileName).Dock = (DockStyle)3;
		((Control)labelGuidFileName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidFileName).Location = new Point(0, 0);
		((Control)labelGuidFileName).Name = "labelGuidFileName";
		((Control)labelGuidFileName).Size = new Size(28, 12);
		((Control)labelGuidFileName).TabIndex = 19;
		((Control)labelGuidFileName).Text = "File:";
		((Label)labelGuidFileName).TextAlign = (ContentAlignment)16;
		((Control)panelSchemaGuid).Controls.Add((Control)(object)labelSchemaGuidWebValue);
		((Control)panelSchemaGuid).Controls.Add((Control)(object)labelSchemaGuidWebName);
		((Control)panelSchemaGuid).Controls.Add((Control)(object)labelSchemaGuidFileSeparator);
		((Control)panelSchemaGuid).Controls.Add((Control)(object)labelSchemaGuidFileValue);
		((Control)panelSchemaGuid).Controls.Add((Control)(object)labelSchemaGuidFileName);
		((Control)panelSchemaGuid).Location = new Point(152, 137);
		((Control)panelSchemaGuid).Name = "panelSchemaGuid";
		((Control)panelSchemaGuid).Size = new Size(333, 19);
		((Control)panelSchemaGuid).TabIndex = 25;
		((Label)labelSchemaGuidWebValue).AutoEllipsis = true;
		((Control)labelSchemaGuidWebValue).AutoSize = true;
		((Control)labelSchemaGuidWebValue).Dock = (DockStyle)3;
		((Control)labelSchemaGuidWebValue).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelSchemaGuidWebValue).Location = new Point(120, 0);
		((Control)labelSchemaGuidWebValue).Name = "labelSchemaGuidWebValue";
		((Control)labelSchemaGuidWebValue).Size = new Size(137, 12);
		((Control)labelSchemaGuidWebValue).TabIndex = 22;
		((Control)labelSchemaGuidWebValue).Text = "https://www.site.com/schema/";
		((Label)labelSchemaGuidWebValue).TextAlign = (ContentAlignment)16;
		((Control)labelSchemaGuidWebValue).MouseDown += new MouseEventHandler(LabelSchema_MouseDown);
		((Control)labelSchemaGuidWebName).AutoSize = true;
		((Control)labelSchemaGuidWebName).Dock = (DockStyle)3;
		((Control)labelSchemaGuidWebName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelSchemaGuidWebName).Location = new Point(89, 0);
		((Control)labelSchemaGuidWebName).Name = "labelSchemaGuidWebName";
		((Control)labelSchemaGuidWebName).Size = new Size(31, 12);
		((Control)labelSchemaGuidWebName).TabIndex = 23;
		((Control)labelSchemaGuidWebName).Text = "Web:";
		((Label)labelSchemaGuidWebName).TextAlign = (ContentAlignment)16;
		((Label)labelSchemaGuidFileSeparator).AutoEllipsis = true;
		((Control)labelSchemaGuidFileSeparator).AutoSize = true;
		((Control)labelSchemaGuidFileSeparator).Dock = (DockStyle)3;
		((Control)labelSchemaGuidFileSeparator).Font = new Font("Microsoft Sans Serif", 6.75f);
		((Control)labelSchemaGuidFileSeparator).Location = new Point(74, 0);
		((Control)labelSchemaGuidFileSeparator).Name = "labelSchemaGuidFileSeparator";
		((Control)labelSchemaGuidFileSeparator).Size = new Size(15, 12);
		((Control)labelSchemaGuidFileSeparator).TabIndex = 24;
		((Control)labelSchemaGuidFileSeparator).Text = "  |  ";
		((Label)labelSchemaGuidFileSeparator).TextAlign = (ContentAlignment)16;
		((Label)labelSchemaGuidFileValue).AutoEllipsis = true;
		((Control)labelSchemaGuidFileValue).AutoSize = true;
		((Control)labelSchemaGuidFileValue).Dock = (DockStyle)3;
		((Control)labelSchemaGuidFileValue).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelSchemaGuidFileValue).Location = new Point(28, 0);
		((Control)labelSchemaGuidFileValue).Name = "labelSchemaGuidFileValue";
		((Control)labelSchemaGuidFileValue).Size = new Size(46, 12);
		((Control)labelSchemaGuidFileValue).TabIndex = 18;
		((Control)labelSchemaGuidFileValue).Text = "c:\\file.xsd";
		((Label)labelSchemaGuidFileValue).TextAlign = (ContentAlignment)16;
		((Control)labelSchemaGuidFileValue).MouseDown += new MouseEventHandler(LabelSchema_MouseDown);
		((Control)labelSchemaGuidFileName).AutoSize = true;
		((Control)labelSchemaGuidFileName).Dock = (DockStyle)3;
		((Control)labelSchemaGuidFileName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelSchemaGuidFileName).Location = new Point(0, 0);
		((Control)labelSchemaGuidFileName).Name = "labelSchemaGuidFileName";
		((Control)labelSchemaGuidFileName).Size = new Size(28, 12);
		((Control)labelSchemaGuidFileName).TabIndex = 19;
		((Control)labelSchemaGuidFileName).Text = "File:";
		((Label)labelSchemaGuidFileName).TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(545, 380);
		((Control)this).Controls.Add((Control)(object)panelSchemaGuid);
		((Control)this).Controls.Add((Control)(object)panelGuid);
		((Control)this).Controls.Add((Control)(object)groupResources);
		((Control)this).Controls.Add((Control)(object)labelXmlType);
		((Control)this).Controls.Add((Control)(object)comboBoxXmlType);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)textBoxAlias);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Control)this).Controls.Add((Control)(object)buttonEditPathData);
		((Control)this).Controls.Add((Control)(object)buttonEditPathSchema);
		((Control)this).Controls.Add((Control)(object)labelPathData);
		((Control)this).Controls.Add((Control)(object)labelPathSchema);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)textBoxName);
		((Control)this).Controls.Add((Control)(object)comboBoxRelationDirection);
		((Control)this).Controls.Add((Control)(object)labelRelationDirection);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiXmlDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Xml Edit";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)groupResources).ResumeLayout(false);
		((Control)panelGuid).ResumeLayout(false);
		((Control)panelGuid).PerformLayout();
		((Control)panelSchemaGuid).ResumeLayout(false);
		((Control)panelSchemaGuid).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
