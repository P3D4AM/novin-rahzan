using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

public class StiCsvDatabaseEditForm : StiFormUX
{
	private string originalName;

	private StiReport report;

	private string oldValue;

	private bool isNameChanged;

	private IContainer components;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	public StiTextBox tbName;

	private StiButtonUX stiButton1;

	private StiButtonUX stiButton2;

	private StiButtonUX btMoveToResource;

	private ErrorProvider errorProvider;

	private StiLabelUX labelFilePath;

	public StiButtonEdit bePathData;

	private StiButtonPanelUX glButtons;

	private StiLabelUX labelName;

	private StiLabelUX labelAlias;

	public StiTextBox tbAlias;

	public StiTextBox textBoxCustomDelimeter;

	private StiComboBox comboBoxDelimiter;

	private StiLabelUX labelDelimiter;

	private StiLabelUX labelEncoding;

	private StiComboBox comboBoxCodePage;

	private StiOffice2013GroupBox groupResources;

	private ScrollableControl listResources;

	private ImageList images;

	private ToolTip toolTip;

	private Panel panelGuid;

	private StiSampleLabelUX labelGuidWebValue;

	private StiLabelUX labelGuidWebName;

	private StiLabelUX labelGuidFolderSeparator;

	private StiSampleLabelUX labelGuidFolderValue;

	private StiLabelUX labelGuidFolderName;

	private StiLabelUX labelGuidFileSeparator;

	private StiSampleLabelUX labelGuidFileValue;

	private StiLabelUX labelGuidFileName;

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(56.0);

	public string Separator
	{
		get
		{
			return ((ListControl)comboBoxDelimiter).SelectedIndex switch
			{
				1 => "\t", 
				2 => ";", 
				3 => ",", 
				4 => " ", 
				_ => ((Control)textBoxCustomDelimeter).Text, 
			};
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				switch (value)
				{
				case "\t":
					((ListControl)comboBoxDelimiter).SelectedIndex = 1;
					break;
				case ";":
					((ListControl)comboBoxDelimiter).SelectedIndex = 2;
					break;
				case ",":
					((ListControl)comboBoxDelimiter).SelectedIndex = 3;
					break;
				case " ":
					((ListControl)comboBoxDelimiter).SelectedIndex = 4;
					break;
				default:
					((ListControl)comboBoxDelimiter).SelectedIndex = 5;
					((Control)textBoxCustomDelimeter).Text = value;
					break;
				}
			}
			if (((ListControl)comboBoxDelimiter).SelectedIndex == -1)
			{
				((ListControl)comboBoxDelimiter).SelectedIndex = 0;
			}
		}
	}

	public int CodePage
	{
		get
		{
			return StiCsvHelper.CodePageCodes[((ListControl)comboBoxCodePage).SelectedIndex];
		}
		set
		{
			for (int i = 0; i < StiCsvHelper.CodePageCodes.Length; i++)
			{
				if (StiCsvHelper.CodePageCodes[i] == value)
				{
					((ListControl)comboBoxCodePage).SelectedIndex = i;
					break;
				}
			}
			if (((ListControl)comboBoxCodePage).SelectedIndex == -1)
			{
				((ListControl)comboBoxCodePage).SelectedIndex = 0;
			}
		}
	}

	public StiXmlDatabase XmlDatabase { get; private set; }

	public StiCsvDatabaseEditForm(StiReport report, string originalName)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			DoTheming();
			DoScaling();
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.report = report;
			this.originalName = originalName;
			Localize();
			((Control)labelEncoding).Visible = (((Control)comboBoxCodePage).Visible = true);
			ObjectCollection items = ((ComboBox)comboBoxCodePage).Items;
			object[] codePageNames = StiCsvHelper.CodePageNames;
			items.AddRange(codePageNames);
			((Control)labelDelimiter).Visible = (((Control)comboBoxDelimiter).Visible = true);
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSystem"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorTab"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSemicolon"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorComma"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSpace"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorOther"));
			((ListControl)comboBoxDelimiter).SelectedIndex = 0;
			((Control)bePathData).TextChanged += OnPathDataTextChanged;
		}
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelFilePath).Text = Loc.GetCleaned("FormDatabaseEdit", "PathToData");
		((Control)labelEncoding).Text = Loc.GetCleaned("Export", "Encoding");
		((Control)labelDelimiter).Text = Loc.GetCleaned("FormDictionaryDesigner", "LabelSeparator");
		((Control)groupResources).Text = StiLocalization.Get("PropertyMain", "Resources");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)btMoveToResource).Text = StiLocalization.Get("Buttons", "MoveToResource");
		((Control)labelGuidFileName).Text = Loc.GetMain("File") + ":";
		((Control)labelGuidFolderName).Text = Loc.Get("Cloud", "RibbonButtonFolder") + ":";
		((Control)labelGuidWebName).Text = Loc.Get("FormColorBoxPopup", "Web") + ":";
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
			StiCsvDatabaseEditForm stiCsvDatabaseEditForm = this;
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
			((Control)stiCsvDatabaseEditForm).Invoke((Delegate)(object)obj2);
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
		StiImageButton stiImageButton = new StiImageButton();
		stiImageButton.ShowFooter = true;
		((ButtonBase)stiImageButton).Image = image;
		((Control)stiImageButton).Left = x;
		((Control)stiImageButton).Top = y;
		((Control)stiImageButton).Width = (int)((double)thumbWidth * 1.5);
		((Control)stiImageButton).Height = thumbWidth;
		((Control)stiImageButton).Text = name;
		((Control)stiImageButton).Tag = tag;
		return stiImageButton;
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
			where v.Type == StiResourceType.Csv && v.Content != null
			select v).ToList();
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
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceCsv" + bigStepName + ".png");
			images.ImageSize = ((Image)image).Size;
			images.Images.Add("Csv", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceCsv" + bigStepName + ".png"));
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
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Invalid comparison between Unknown and I4
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Invalid comparison between Unknown and I4
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Invalid comparison between Unknown and I4
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
			if (!((Control)bePathData).Text.ToLowerInvariant().EndsWithInvariant(".csv"))
			{
				List<StiDataLoaderHelper.Data> list = StiUniversalDataLoader.LoadMutiple(report, ((Control)bePathData).Text, "*.csv");
				if (list != null)
				{
					DataSet dataSet = null;
					foreach (StiDataLoaderHelper.Data item in list)
					{
						try
						{
							DataTable table = StiCsvHelper.GetTable(item.Array, CodePage, Separator);
							if (table != null)
							{
								if (dataSet == null)
								{
									dataSet = new DataSet
									{
										EnforceConstraints = true
									};
								}
								table.TableName = item.Name;
								dataSet.Tables.Add(table);
							}
						}
						catch
						{
						}
					}
					if (dataSet != null)
					{
						string text = StiNameCreation.CreateResourceName(report, Path.GetFileNameWithoutExtension(((Control)bePathData).Text).Normalize() + "Xml");
						string text2 = StiNameCreation.CreateResourceName(report, Path.GetFileNameWithoutExtension(((Control)bePathData).Text).Normalize() + "Xsd");
						StiResource resource = null;
						StiResource resource2 = null;
						using (MemoryStream memoryStream = new MemoryStream())
						{
							dataSet.WriteXml(memoryStream);
							memoryStream.Flush();
							if (memoryStream.Length > StiOptions.Engine.ReportResources.MaximumSize)
							{
								DialogResult val = MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("Notices", "MessageMaximumFileSizeExceeded"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4, (MessageBoxIcon)32);
								if ((int)val == 7)
								{
									((Control)bePathData).Focus();
									return;
								}
							}
							resource = new StiResource(text, text, StiResourceType.Xml, memoryStream.ToArray());
						}
						using (MemoryStream memoryStream2 = new MemoryStream())
						{
							dataSet.WriteXmlSchema(memoryStream2);
							memoryStream2.Flush();
							if (memoryStream2.Length > StiOptions.Engine.ReportResources.MaximumSize)
							{
								DialogResult val2 = MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("Notices", "MessageMaximumFileSizeExceeded"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4, (MessageBoxIcon)32);
								if ((int)val2 == 7)
								{
									((Control)bePathData).Focus();
									return;
								}
							}
							resource2 = new StiResource(text2, text2, StiResourceType.Xsd, memoryStream2.ToArray());
						}
						dataSet.Dispose();
						report.Dictionary.Resources.Add(resource);
						report.Dictionary.Resources.Add(resource2);
						XmlDatabase = new StiXmlDatabase
						{
							Name = ((Control)tbName).Text,
							Alias = ((Control)tbAlias).Text,
							PathData = "resource://" + text,
							PathSchema = "resource://" + text2
						};
					}
				}
			}
			else if (!((Control)bePathData).Text.ToLowerInvariant().StartsWithInvariant("resource://") && ((Control)bePathData).Text.ToLowerInvariant().EndsWithInvariant(".csv"))
			{
				string text3 = StiNameCreation.CreateResourceName(report, Path.GetFileNameWithoutExtension(((Control)bePathData).Text).Normalize());
				StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(Path.GetExtension(((Control)bePathData).Text).ToLowerInvariant());
				if (!typeFromExtension.HasValue)
				{
					return;
				}
				if (new FileInfo(((Control)bePathData).Text).Length > StiOptions.Engine.ReportResources.MaximumSize)
				{
					DialogResult val3 = MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("Notices", "MessageMaximumFileSizeExceeded"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)4, (MessageBoxIcon)32);
					if ((int)val3 == 7)
					{
						((Control)bePathData).Focus();
						return;
					}
				}
				StiResource resource3 = new StiResource(text3, text3, typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(((Control)bePathData).Text));
				report.Dictionary.Resources.Add(resource3);
				((Control)bePathData).Text = "resource://" + text3;
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
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		FolderBrowserDialog val = new FolderBrowserDialog();
		try
		{
			if (!string.IsNullOrEmpty(((Control)bePathData).Text) && !((Control)bePathData).Text.StartsWithInvariant("resource://"))
			{
				val.SelectedPath = Path.GetDirectoryName(((Control)bePathData).Text);
			}
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				string baseName = Path.GetFileNameWithoutExtension(val.SelectedPath).Normalize();
				if (!isNameChanged && !IsEditMode())
				{
					((Control)tbName).Text = StiNameCreation.CreateConnectionName(report, baseName);
				}
				((Control)tbAlias).Text = ((Control)tbName).Text;
				((Control)bePathData).Text = val.SelectedPath;
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

	private void comboBoxDelimiter_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)textBoxCustomDelimeter).Visible = ((ComboBox)comboBoxDelimiter).SelectedItem.ToString().Equals(StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorOther"));
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Expected O, but got Unknown
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Expected O, but got Unknown
		//IL_0d2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d34: Expected O, but got Unknown
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6d: Expected O, but got Unknown
		//IL_0e05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0f: Expected O, but got Unknown
		//IL_0eae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Expected O, but got Unknown
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f33: Expected O, but got Unknown
		//IL_0f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6c: Expected O, but got Unknown
		//IL_1001: Unknown result type (might be due to invalid IL or missing references)
		//IL_100b: Expected O, but got Unknown
		//IL_10a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b1: Expected O, but got Unknown
		//IL_111f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1129: Expected O, but got Unknown
		//IL_1158: Unknown result type (might be due to invalid IL or missing references)
		//IL_1162: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCsvDatabaseEditForm));
		labelName = new StiLabelUX();
		tbName = new StiTextBox();
		stiButton1 = new StiButtonUX();
		stiButton2 = new StiButtonUX();
		labelFilePath = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		labelAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		textBoxCustomDelimeter = new StiTextBox();
		comboBoxDelimiter = new StiComboBox();
		labelDelimiter = new StiLabelUX();
		labelEncoding = new StiLabelUX();
		btMoveToResource = new StiButtonUX();
		comboBoxCodePage = new StiComboBox();
		images = new ImageList(components);
		toolTip = new ToolTip(components);
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		groupResources = new StiOffice2013GroupBox();
		listResources = new ScrollableControl();
		glButtons = new StiButtonPanelUX();
		bePathData = new StiButtonEdit();
		panelGuid = new Panel();
		labelGuidWebValue = new StiSampleLabelUX();
		labelGuidWebName = new StiLabelUX();
		labelGuidFolderSeparator = new StiLabelUX();
		labelGuidFolderValue = new StiSampleLabelUX();
		labelGuidFolderName = new StiLabelUX();
		labelGuidFileSeparator = new StiLabelUX();
		labelGuidFileValue = new StiSampleLabelUX();
		labelGuidFileName = new StiLabelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)groupResources).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)panelGuid).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(131, 20);
		((Control)labelName).TabIndex = 6;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(152, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(272, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += OnNameEnter;
		((Control)tbName).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)tbName).Leave += OnNameLeave;
		((Control)stiButton1).Anchor = (AnchorStyles)10;
		((Button)stiButton1).DialogResult = (DialogResult)2;
		((Control)stiButton1).Location = new Point(460, 392);
		((Control)stiButton1).Name = "stiButton1";
		((Control)stiButton1).Size = new Size(75, 23);
		((Control)stiButton1).TabIndex = 13;
		((Control)stiButton1).Text = "Cancel";
		((ButtonBase)stiButton1).UseVisualStyleBackColor = true;
		((Control)stiButton2).Anchor = (AnchorStyles)10;
		((Control)stiButton2).Location = new Point(380, 392);
		((Control)stiButton2).Name = "stiButton2";
		((Control)stiButton2).Size = new Size(75, 23);
		((Control)stiButton2).TabIndex = 12;
		((Control)stiButton2).Text = "Ok";
		((ButtonBase)stiButton2).UseVisualStyleBackColor = true;
		((Control)labelFilePath).Location = new Point(12, 78);
		((Control)labelFilePath).Name = "labelFilePath";
		((Control)labelFilePath).Size = new Size(131, 20);
		((Control)labelFilePath).TabIndex = 8;
		((Control)labelFilePath).Text = "Path Data";
		((Label)labelFilePath).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(131, 20);
		((Control)labelAlias).TabIndex = 7;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(152, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(272, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)tbAlias).KeyDown += new KeyEventHandler(TextBox_KeyDown);
		((Control)textBoxCustomDelimeter).Location = new Point(352, 157);
		((Control)textBoxCustomDelimeter).Name = "textBoxCustomDelimeter";
		((Control)textBoxCustomDelimeter).Size = new Size(89, 21);
		((Control)textBoxCustomDelimeter).TabIndex = 5;
		((Control)textBoxCustomDelimeter).Visible = false;
		comboBoxDelimiter.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxDelimiter).DropDownWidth = 250;
		((Control)comboBoxDelimiter).Location = new Point(152, 157);
		((ComboBox)comboBoxDelimiter).MaxDropDownItems = 20;
		((Control)comboBoxDelimiter).Name = "comboBoxDelimiter";
		((Control)comboBoxDelimiter).Size = new Size(194, 21);
		((Control)comboBoxDelimiter).TabIndex = 4;
		((Control)comboBoxDelimiter).Visible = false;
		((ComboBox)comboBoxDelimiter).SelectedIndexChanged += comboBoxDelimiter_SelectedIndexChanged;
		((Control)labelDelimiter).Location = new Point(12, 158);
		((Control)labelDelimiter).Name = "labelDelimiter";
		((Control)labelDelimiter).Size = new Size(131, 20);
		((Control)labelDelimiter).TabIndex = 10;
		((Control)labelDelimiter).Text = "Delimiter";
		((Label)labelDelimiter).TextAlign = (ContentAlignment)16;
		((Control)labelDelimiter).Visible = false;
		((Control)labelEncoding).Location = new Point(12, 124);
		((Control)labelEncoding).Name = "labelEncoding";
		((Control)labelEncoding).Size = new Size(131, 20);
		((Control)labelEncoding).TabIndex = 9;
		((Control)labelEncoding).Text = "Encoding";
		((Label)labelEncoding).TextAlign = (ContentAlignment)16;
		((Control)labelEncoding).Visible = false;
		((Control)btMoveToResource).Anchor = (AnchorStyles)6;
		((Control)btMoveToResource).Location = new Point(12, 12);
		((Control)btMoveToResource).Name = "btMoveToResource";
		((Control)btMoveToResource).Size = new Size(150, 24);
		((Control)btMoveToResource).TabIndex = 0;
		((Control)btMoveToResource).Text = "Move to Resource";
		((ButtonBase)btMoveToResource).UseVisualStyleBackColor = true;
		((Control)btMoveToResource).Click += OnMoveToResourceClick;
		comboBoxCodePage.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxCodePage).DropDownWidth = 250;
		((Control)comboBoxCodePage).Location = new Point(152, 124);
		((ComboBox)comboBoxCodePage).MaxDropDownItems = 20;
		((Control)comboBoxCodePage).Name = "comboBoxCodePage";
		((Control)comboBoxCodePage).Size = new Size(194, 21);
		((Control)comboBoxCodePage).TabIndex = 3;
		((Control)comboBoxCodePage).Visible = false;
		images.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("images.ImageStream");
		images.TransparentColor = Color.Transparent;
		images.Images.SetKeyName(0, "Csv");
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(371, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += OnOkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(452, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)groupResources).Controls.Add((Control)(object)listResources);
		((Control)groupResources).Dock = (DockStyle)2;
		groupResources.HeaderHeight = 24;
		((Control)groupResources).Location = new Point(0, 192);
		((Control)groupResources).Name = "groupResources";
		((Control)groupResources).Size = new Size(538, 121);
		((Control)groupResources).TabIndex = 11;
		((Control)groupResources).TabStop = false;
		((Control)groupResources).Text = "Resources";
		((Control)groupResources).Visible = false;
		((Control)listResources).Anchor = (AnchorStyles)15;
		listResources.AutoScroll = true;
		((Control)listResources).BackColor = Color.White;
		((Control)listResources).Location = new Point(10, 29);
		((Control)listResources).Name = "listResources";
		((Control)listResources).Size = new Size(517, 87);
		((Control)listResources).TabIndex = 0;
		((Control)glButtons).Controls.Add((Control)(object)btMoveToResource);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 313);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(538, 46);
		((Control)glButtons).TabIndex = 12;
		((Control)bePathData).Anchor = (AnchorStyles)13;
		bePathData.ButtonIconKind = StiButtonIconKind.Dots;
		bePathData.Flat = false;
		((Control)bePathData).Location = new Point(152, 78);
		((Control)bePathData).Name = "bePathData";
		((Control)bePathData).Size = new Size(375, 21);
		((Control)bePathData).TabIndex = 2;
		bePathData.ButtonClick += OnPathDataClick;
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidWebName);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFolderSeparator);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFolderValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFolderName);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileSeparator);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileValue);
		((Control)panelGuid).Controls.Add((Control)(object)labelGuidFileName);
		((Control)panelGuid).Location = new Point(152, 103);
		((Control)panelGuid).Name = "panelGuid";
		((Control)panelGuid).Size = new Size(375, 19);
		((Control)panelGuid).TabIndex = 21;
		((Label)labelGuidWebValue).AutoEllipsis = true;
		((Control)labelGuidWebValue).AutoSize = true;
		((Control)labelGuidWebValue).Dock = (DockStyle)3;
		((Control)labelGuidWebValue).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidWebValue).Location = new Point(219, 0);
		((Control)labelGuidWebValue).Name = "labelGuidWebValue";
		((Control)labelGuidWebValue).Size = new Size(122, 12);
		((Control)labelGuidWebValue).TabIndex = 22;
		((Control)labelGuidWebValue).Text = "https://www.site.com/data/";
		((Label)labelGuidWebValue).TextAlign = (ContentAlignment)16;
		((Control)labelGuidWebValue).MouseDown += new MouseEventHandler(LabelGuid_MouseDown);
		((Control)labelGuidWebName).AutoSize = true;
		((Control)labelGuidWebName).Dock = (DockStyle)3;
		((Control)labelGuidWebName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidWebName).Location = new Point(188, 0);
		((Control)labelGuidWebName).Name = "labelGuidWebName";
		((Control)labelGuidWebName).Size = new Size(31, 12);
		((Control)labelGuidWebName).TabIndex = 23;
		((Control)labelGuidWebName).Text = "Web:";
		((Label)labelGuidWebName).TextAlign = (ContentAlignment)16;
		((Label)labelGuidFolderSeparator).AutoEllipsis = true;
		((Control)labelGuidFolderSeparator).AutoSize = true;
		((Control)labelGuidFolderSeparator).Dock = (DockStyle)3;
		((Control)labelGuidFolderSeparator).Font = new Font("Microsoft Sans Serif", 6.75f);
		((Control)labelGuidFolderSeparator).Location = new Point(173, 0);
		((Control)labelGuidFolderSeparator).Name = "labelGuidFolderSeparator";
		((Control)labelGuidFolderSeparator).Size = new Size(15, 12);
		((Control)labelGuidFolderSeparator).TabIndex = 25;
		((Control)labelGuidFolderSeparator).Text = "  |  ";
		((Label)labelGuidFolderSeparator).TextAlign = (ContentAlignment)16;
		((Label)labelGuidFolderValue).AutoEllipsis = true;
		((Control)labelGuidFolderValue).AutoSize = true;
		((Control)labelGuidFolderValue).Dock = (DockStyle)3;
		((Control)labelGuidFolderValue).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidFolderValue).Location = new Point(131, 0);
		((Control)labelGuidFolderValue).Name = "labelGuidFolderValue";
		((Control)labelGuidFolderValue).Size = new Size(42, 12);
		((Control)labelGuidFolderValue).TabIndex = 20;
		((Control)labelGuidFolderValue).Text = "c:\\folder\\";
		((Label)labelGuidFolderValue).TextAlign = (ContentAlignment)16;
		((Control)labelGuidFolderValue).MouseDown += new MouseEventHandler(LabelGuid_MouseDown);
		((Control)labelGuidFolderName).AutoSize = true;
		((Control)labelGuidFolderName).Dock = (DockStyle)3;
		((Control)labelGuidFolderName).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelGuidFolderName).Location = new Point(90, 0);
		((Control)labelGuidFolderName).Name = "labelGuidFolderName";
		((Control)labelGuidFolderName).Size = new Size(41, 12);
		((Control)labelGuidFolderName).TabIndex = 21;
		((Control)labelGuidFolderName).Text = "Folder:";
		((Label)labelGuidFolderName).TextAlign = (ContentAlignment)16;
		((Label)labelGuidFileSeparator).AutoEllipsis = true;
		((Control)labelGuidFileSeparator).AutoSize = true;
		((Control)labelGuidFileSeparator).Dock = (DockStyle)3;
		((Control)labelGuidFileSeparator).Font = new Font("Microsoft Sans Serif", 6.75f);
		((Control)labelGuidFileSeparator).Location = new Point(75, 0);
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
		((Control)labelGuidFileValue).Size = new Size(47, 12);
		((Control)labelGuidFileValue).TabIndex = 18;
		((Control)labelGuidFileValue).Text = "c:\\file.csv";
		((Label)labelGuidFileValue).TextAlign = (ContentAlignment)16;
		((Control)labelGuidFileValue).MouseDown += new MouseEventHandler(LabelGuid_MouseDown);
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
		((Form)this).ClientSize = new Size(538, 359);
		((Control)this).Controls.Add((Control)(object)panelGuid);
		((Control)this).Controls.Add((Control)(object)groupResources);
		((Control)this).Controls.Add((Control)(object)textBoxCustomDelimeter);
		((Control)this).Controls.Add((Control)(object)comboBoxDelimiter);
		((Control)this).Controls.Add((Control)(object)labelDelimiter);
		((Control)this).Controls.Add((Control)(object)labelEncoding);
		((Control)this).Controls.Add((Control)(object)comboBoxCodePage);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)bePathData);
		((Control)this).Controls.Add((Control)(object)labelFilePath);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)stiButton1);
		((Control)this).Controls.Add((Control)(object)stiButton2);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiCsvDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "CSV Edit";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Load += ThisLoad;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)groupResources).ResumeLayout(false);
		((Control)glButtons).ResumeLayout(false);
		((Control)panelGuid).ResumeLayout(false);
		((Control)panelGuid).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
