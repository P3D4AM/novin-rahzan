using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components.Design;

public class StiImageEditorForm : StiFormUX
{
	public enum StartTab
	{
		None,
		Image,
		ImageData,
		ImageURL,
		DataColumn,
		File,
		Icon
	}

	private StiImage imageComp;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private byte[] imageHyperlink;

	private byte[] imageFile;

	private bool isVariableGalleryActivated;

	private bool isResourceGalleryActivated;

	private bool isColumnGalleryActivated;

	private string dataColumn;

	private List<string> fileHistoryList;

	private List<string> hyperlinkHistoryList;

	private StiFontIcons? imageIcon;

	private Color imageIconColor;

	private StiPopup iconsPopup;

	private Panel panelIcons;

	private StiHintTextBox textBoxSearch;

	private StiFontIconSelectionControl fontIconSelectionControl;

	private byte[] imageBytes;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageImage;

	private StiCloudTabulatorPage pageHyperlink;

	private StiCloudTabulatorPage pageExpression;

	private StiCloudTabulatorPage pageDataColumn;

	private StiCloudTabulatorPage pageFile;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiTreeViewUX treeViewDataColumn;

	private HintTextBox textBoxFile;

	private StiButtonPanelUX footer;

	private StiPanelUX panelImage;

	private StiButtonUX buttonOpenFile;

	private StiButtonUX buttonRemove;

	private ToolTip toolTip;

	private StiPanelBox panelBoxFile;

	private StiButtonUX buttonFile;

	private StiLabelUX labelFile;

	private StiLabelUX labelHyperlink;

	private StiPanelBox panelHyperlink;

	private HintTextBox textBoxHyperlink;

	private StiButtonUX buttonHyperlink;

	private StiLabelUX labelExpression;

	private StiPanelBox panelExpression;

	private HintTextBox textBoxExpression;

	private StiButtonUX buttonExpression;

	private StiPanelUX imageHyperlinkPreview;

	private ProgressBar progressHyperlinkPreview;

	private Timer timerHyperlinkPreview;

	private StiPanelUX imageFilePreview;

	private ScrollableControl listGallery;

	private StiLabelUX labelSpecification;

	private StiOffice2013GroupBox groupGallery;

	private StiOffice2013GroupBox groupFileHistory;

	private ScrollableControl listFileHistory;

	private ContextMenuStrip contextMenuHistory;

	private ToolStripMenuItem deleteFileHistoryItem;

	private ToolStripMenuItem deleteAllFileHistoryItem;

	private StiOffice2013GroupBox groupHyperlinkHistory;

	private ScrollableControl listHyperlinkHistory;

	private StiPanelUX panel1;

	private StiPanelUX panel2;

	private StiPanelUX panel3;

	private StiPanelUX panel4;

	private StiPanelUX panel5;

	private StiOffice2013GroupBox groupDataColumnGallery;

	private ScrollableControl listDataColumnGallery;

	private StiPanelUX panelProgressDataColumn;

	private ProgressBar progressDataColumn;

	private StiPanelUX panelImageContainer;

	private StiPanelUX panel6;

	private StiButtonUX buttonSaveFile;

	private StiCloudTabulatorPage pageIcon;

	private StiPanelUX panel7;

	private StiPanelUX panelSampleIcon;

	private StiPanelUX panelIcon;

	private ColorPickerButton colorBoxIconColor;

	private StiPanelUX panelBoxIcon;

	private StiOperationButton buttonIcon;

	private StiLabelUX labelIcon;

	private StiOperationButton buttonIconRemove;

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(75.0);

	public bool IsVariableFromGallery
	{
		get
		{
			string exp = GetVariableNameFromExpression();
			return GetVariables().FirstOrDefault((StiVariable v) => v.Name != null && v.Name == exp) != null;
		}
	}

	public bool IsResourceFromGallery
	{
		get
		{
			string resourceName = GetResourceNameFromHyperlink();
			return GetResources().FirstOrDefault((StiResource v) => v.Name != null && v.Name == resourceName) != null;
		}
	}

	public byte[] ImageBytes
	{
		get
		{
			return imageBytes;
		}
		set
		{
			imageBytes = value;
			CheckImageStates();
			((Control)panelImage).Invalidate();
		}
	}

	public string Specification
	{
		get
		{
			return ((Control)labelSpecification).Text;
		}
		set
		{
			((Control)labelSpecification).Text = value;
			((Control)labelSpecification).Visible = !string.IsNullOrWhiteSpace(value);
		}
	}

	public StiImageEditorForm(StiImage image)
		: this(image, StartTab.None)
	{
	}

	public StiImageEditorForm(StiImage image, StartTab startTab)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			imageComp = image;
			ImageBytes = image.TakeImage();
			((Control)textBoxExpression).Text = StiExpressionPacker.PackExpression(image.ImageData.Value, image.Report.Designer, useBraces: true);
			((Control)textBoxHyperlink).Text = StiExpressionPacker.PackExpression(image.ImageURL.Value, image.Report.Designer, useBraces: true);
			((Control)textBoxFile).Text = image.File;
			Localize();
			DoTheming();
			DoScaling();
			BuildDataColumn();
			StiSettings.Load();
			fileHistoryList = StringToList(StiSettings.GetStr(((Control)this).Name, "ImageFileHistory", null));
			hyperlinkHistoryList = StringToList(StiSettings.GetStr(((Control)this).Name, "ImageHyperlinkHistory", null));
			StiFormSettings.Load((Form)(object)this);
			BuildComponentOptions(startTab);
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Image");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)pageDataColumn).Text = StiLocalization.Get("PropertyMain", "DataColumn");
		((Control)pageImage).Text = StiLocalization.Get("PropertyMain", "Image");
		((Control)pageHyperlink).Text = StiLocalization.Get("PropertyMain", "Hyperlink");
		((Control)pageExpression).Text = StiLocalization.Get("PropertyMain", "Expression");
		((Control)pageFile).Text = StiLocalization.Get("PropertyMain", "File");
		((Control)pageIcon).Text = StiLocalization.Get("PropertyMain", "Icon");
		((Control)labelExpression).Text = Loc.GetCleaned("Report", "LabelExpression");
		((Control)labelFile).Text = Loc.GetCleaned("Cloud", "labelFileName");
		((Control)labelHyperlink).Text = Loc.GetCleaned("PropertyMain", "Hyperlink");
		((Control)labelIcon).Text = Loc.Get("PropertyMain", "Icon");
		((ToolStripItem)deleteFileHistoryItem).Text = StiLocalization.Get("Buttons", "Delete");
		((ToolStripItem)deleteAllFileHistoryItem).Text = StiLocalization.Get("Cloud", "ButtonDeleteAll");
		((Control)groupGallery).Text = StiLocalization.Get("Report", "Gallery");
		((Control)groupDataColumnGallery).Text = StiLocalization.Get("Report", "Gallery");
		((Control)groupFileHistory).Text = StiLocalization.Get("Report", "History");
		((Control)groupHyperlinkHistory).Text = StiLocalization.Get("Report", "History");
		toolTip.SetToolTip((Control)(object)buttonExpression, StiLocalization.Get("FormTitles", "ExpressionEditorForm"));
		toolTip.SetToolTip((Control)(object)buttonHyperlink, StiLocalization.Get("PropertyMain", "ImageURL"));
		toolTip.SetToolTip((Control)(object)buttonFile, StiLocalization.Get("Buttons", "Open"));
		toolTip.SetToolTip((Control)(object)buttonOpenFile, StiLocalization.Get("Buttons", "Open"));
		toolTip.SetToolTip((Control)(object)buttonSaveFile, StiLocalization.Get("Buttons", "Save"));
		toolTip.SetToolTip((Control)(object)buttonRemove, StiLocalization.Get("Buttons", "Remove"));
		textBoxExpression.Hint = "{Image.FromFile(\"c:\\folder\\image.png\")}";
		textBoxHyperlink.Hint = "http://site.com/image.png";
		textBoxFile.Hint = "c:\\folder\\image.png";
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			ScrollableControl obj = listGallery;
			ScrollableControl obj2 = listDataColumnGallery;
			ScrollableControl obj3 = listHyperlinkHistory;
			Color color = (((Control)listFileHistory).BackColor = StiUX.Background);
			Color color3 = (((Control)obj3).BackColor = color);
			Color backColor = (((Control)obj2).BackColor = color3);
			((Control)obj).BackColor = backColor;
			((Control)labelSpecification).ForeColor = StiUX.InputForegroundHint;
			((Control)labelSpecification).BackColor = StiUX.Background;
		}
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageFile.Image = StiReportDesignImages.Tasks.ImageFile(StiImageSize.Double);
		pageDataColumn.Image = StiReportDesignImages.Tasks.ImageDataColumn(StiImageSize.Double);
		pageExpression.Image = StiReportDesignImages.Tasks.ImageExpression(StiImageSize.Double);
		pageHyperlink.Image = StiReportDesignImages.Tasks.ImageHyperlink(StiImageSize.Double);
		pageImage.Image = StiReportDesignImages.Tasks.ImageImage(StiImageSize.Double);
		pageIcon.Image = StiReportDesignImages.Tasks.ImageIcon(StiImageSize.Double);
		((ButtonBase)buttonExpression).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonSaveFile).Image = StiReportDesignImages.Actions.SaveFile();
		((ButtonBase)buttonOpenFile).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonHyperlink).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonFile).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonIconRemove).Image = StiReportDesignImages.Actions.Remove();
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxIcon);
	}

	private void BuildComponentOptions(StartTab startTab)
	{
		switch (startTab)
		{
		case StartTab.None:
			if (imageComp.ExistImage())
			{
				TabControl.SelectedTab = pageImage;
			}
			else if (imageComp.DataColumn != null && imageComp.DataColumn.Trim().Length > 0)
			{
				TabControl.SelectedTab = pageDataColumn;
				((Control)treeViewDataColumn).Focus();
			}
			else if (imageComp.ImageData != null && imageComp.ImageData.Value.Trim().Length > 0)
			{
				TabControl.SelectedTab = pageExpression;
				((Control)textBoxExpression).Select();
				((TextBoxBase)textBoxExpression).Select(0, 0);
			}
			else if (imageComp.ImageURL != null && imageComp.ImageURL.Value.Trim().Length > 0)
			{
				TabControl.SelectedTab = pageHyperlink;
				((Control)textBoxHyperlink).Select();
				((TextBoxBase)textBoxHyperlink).Select(0, 0);
			}
			else if (imageComp.File != null && imageComp.File.Trim().Length > 0)
			{
				TabControl.SelectedTab = pageFile;
				((Control)textBoxFile).Select();
				((TextBoxBase)textBoxFile).Select(0, 0);
			}
			else if (imageComp.Icon.HasValue)
			{
				TabControl.SelectedTab = pageIcon;
			}
			else
			{
				TabControl.SelectedIndex = 0;
			}
			break;
		case StartTab.Image:
			TabControl.SelectedTab = pageImage;
			break;
		case StartTab.ImageURL:
			TabControl.SelectedTab = pageHyperlink;
			break;
		case StartTab.ImageData:
			TabControl.SelectedTab = pageExpression;
			break;
		case StartTab.DataColumn:
			TabControl.SelectedTab = pageDataColumn;
			break;
		case StartTab.File:
			TabControl.SelectedTab = pageFile;
			break;
		case StartTab.Icon:
			TabControl.SelectedTab = pageIcon;
			break;
		}
		((Form)this).ShowIcon = false;
		pageImage.Invisible = !StiOptions.Designer.Editors.ImageEditor.ShowImageTabPage;
		pageDataColumn.Invisible = !StiOptions.Designer.Editors.ImageEditor.ShowDataColumnTabPage;
		pageExpression.Invisible = !StiOptions.Designer.Editors.ImageEditor.ShowImageDataTabPage || ((StiOptions.Designer.IsBICreatorDesigner || StiOptions.Designer.IsBeginnerDesigner) && string.IsNullOrEmpty(((Control)textBoxExpression).Text));
		pageHyperlink.Invisible = !StiOptions.Designer.Editors.ImageEditor.ShowImageURLTabPage;
		pageFile.Invisible = !StiOptions.Designer.Editors.ImageEditor.ShowFileTabPage;
		if (!pageExpression.Invisible && StiOptions.Designer.PropertyGrid.PropertyLevel != StiLevel.Professional)
		{
			pageExpression.Invisible = true;
		}
		if (!StiOptions.Designer.IsBICreatorDesigner && !StiOptions.Designer.IsBeginnerDesigner)
		{
			return;
		}
		if (imageComp.ImageData != null)
		{
			StiImageDataExpression imageData = imageComp.ImageData;
			if (imageData == null || imageData.Value.Trim().Length != 0)
			{
				return;
			}
		}
		pageExpression.Invisible = true;
	}

	private bool IsFormDestructed()
	{
		if (!((Control)this).IsDisposed && !((Control)this).Disposing)
		{
			return !((Control)this).IsHandleCreated;
		}
		return true;
	}

	private void SaveSettings()
	{
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "ImageFileHistory", ListToString(fileHistoryList));
		StiSettings.Set(((Control)this).Name, "ImageHyperlinkHistory", ListToString(hyperlinkHistoryList));
		StiSettings.Save();
	}

	private StiImageButton CreateGalleryButton(string name, byte[] image, object tag, int x, int y, int thumbWidth)
	{
		return CreateGalleryButton(name, StiImageConverter.TryBytesToImage(image), tag, x, y, thumbWidth);
	}

	private StiImageButton CreateGalleryButton(string name, Image image, object tag, int x, int y, int thumbWidth)
	{
		StiImageButton stiImageButton = new StiImageButton();
		((ButtonBase)stiImageButton).Image = image;
		((Control)stiImageButton).Left = x;
		((Control)stiImageButton).Top = y;
		((Control)stiImageButton).Width = thumbWidth;
		((Control)stiImageButton).Height = thumbWidth;
		((Control)stiImageButton).Text = name;
		((Control)stiImageButton).Tag = tag;
		return stiImageButton;
	}

	private string ListToString(List<string> list)
	{
		return JsonConvert.SerializeObject(list, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	private List<string> StringToList(string str)
	{
		List<string> list = new List<string>();
		try
		{
			if (str != null)
			{
				JsonConvert.PopulateObject(str, list);
			}
		}
		catch
		{
		}
		return list;
	}

	private void ScrollToActiveButton(ScrollableControl gallery)
	{
		Control val = ((IEnumerable)((Control)gallery).Controls).Cast<Control>().FirstOrDefault((Func<Control, bool>)((Control c) => c is StiImageButton && ((CheckBox)(StiImageButton)(object)c).Checked));
		if (val != null)
		{
			gallery.ScrollControlIntoView(val);
		}
	}

	private void ResetGalleryAndHistorySelection()
	{
		ResetImageGallerySelection();
		ResetDataColumnGallerySelection();
		ResetFileHistorySelection();
		ResetHyperlinkHistorySelection();
	}

	private void BuildImageGallery()
	{
		List<StiVariable> variables = GetVariables();
		List<StiResource> resources = GetResources();
		List<StiDataColumn> columns = GetColumns();
		((Control)groupGallery).Visible = columns.Any() || variables.Any() || resources.Any();
		((Control)listGallery).Controls.Clear();
		Task.Factory.StartNew(delegate
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_043d: Expected O, but got Unknown
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Expected O, but got Unknown
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Expected O, but got Unknown
			int x = ItemGalleryStep;
			int y = ItemGalleryStep;
			string variableName = GetVariableNameFromExpression();
			string resourceNameFromHyperlink = GetResourceNameFromHyperlink();
			foreach (StiVariable variable in variables)
			{
				try
				{
					((Control)this).Invoke((Delegate)(MethodInvoker)delegate
					{
						StiImageEditorForm stiImageEditorForm = this;
						string name = variable.Name;
						object valueObject = variable.ValueObject;
						StiImageButton stiImageButton = stiImageEditorForm.CreateGalleryButton(name, (Image)((valueObject is Image) ? valueObject : null), variable, x, y, ItemGallerySize);
						if (variableName == variable.Name)
						{
							((CheckBox)stiImageButton).Checked = true;
							ImageGalleryClick(stiImageButton, EventArgs.Empty);
						}
						((CheckBox)stiImageButton).CheckedChanged += ImageGalleryClick;
						((CheckBox)stiImageButton).DoubleClick += This_OkClicked;
						if (!IsFormDestructed())
						{
							toolTip.SetToolTip((Control)(object)stiImageButton, string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "Variable"), variable.Name));
							((Control)listGallery).Controls.Add((Control)(object)stiImageButton);
						}
					});
					x += ItemGallerySize + ItemGalleryStep;
				}
				catch
				{
				}
			}
			foreach (StiResource resource in resources)
			{
				try
				{
					StiImageButton button2 = CreateGalleryButton(resource.Name, resource.Content, resource, x, y, ItemGallerySize);
					if (resourceNameFromHyperlink == resource.Name)
					{
						((CheckBox)button2).Checked = true;
						ImageGalleryClick(button2, EventArgs.Empty);
					}
					((CheckBox)button2).CheckedChanged += ImageGalleryClick;
					((CheckBox)button2).DoubleClick += This_OkClicked;
					((Control)this).Invoke((Delegate)(MethodInvoker)delegate
					{
						if (!IsFormDestructed())
						{
							toolTip.SetToolTip((Control)(object)button2, string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "Resource"), resource.Name));
							((Control)listGallery).Controls.Add((Control)(object)button2);
						}
					});
					x += ItemGallerySize + ItemGalleryStep;
				}
				catch
				{
				}
			}
			int imageCounter = 0;
			try
			{
				List<StiDataSource> dataSources = columns.Select((StiDataColumn c) => c.DataSource).Distinct().ToList();
				if (StiOptions.Designer.Editors.AllowConnectToDataInGallery)
				{
					imageComp.Report.Dictionary.TryConnect(loadData: true, dataSources);
				}
				foreach (StiDataColumn item in columns)
				{
					try
					{
						byte[] imageFromColumn = GetImageFromColumn(item);
						if (imageFromColumn != null)
						{
							string columnPath = item.GetColumnPath();
							StiImageButton button = CreateGalleryButton(columnPath, imageFromColumn, item, x, y, ItemGallerySize);
							((CheckBox)button).CheckedChanged += ImageGalleryClick;
							((CheckBox)button).DoubleClick += This_OkClicked;
							((Control)button).Width = ItemGallerySize * 2;
							((Control)this).Invoke((Delegate)(MethodInvoker)delegate
							{
								if (!IsFormDestructed())
								{
									toolTip.SetToolTip((Control)(object)button, string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "DataColumn"), columnPath));
									((Control)listGallery).Controls.Add((Control)(object)button);
									if (imageComp.DataColumn == columnPath)
									{
										((CheckBox)button).Checked = true;
										ImageGalleryClick(button, EventArgs.Empty);
									}
								}
							});
							x += ((Control)button).Width + ItemGalleryStep;
							int num = imageCounter;
							imageCounter = num + 1;
						}
					}
					catch
					{
					}
				}
			}
			finally
			{
				if (StiOptions.Designer.Editors.AllowConnectToDataInGallery)
				{
					imageComp.Report.Dictionary.TryDisconnect();
				}
			}
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				if (!IsFormDestructed())
				{
					ScrollToActiveButton(listGallery);
					((Control)groupGallery).Visible = variables.Any() || resources.Any() || imageCounter > 0;
				}
			});
		});
	}

	private byte[] GetImageFromColumn(StiDataColumn column)
	{
		string columnPath = column.GetColumnPath();
		List<object> dataListFromDataColumn = StiDataColumn.GetDataListFromDataColumn(imageComp.Report.Dictionary, columnPath, 3);
		foreach (object item in dataListFromDataColumn)
		{
			if (item == null || !StiImageHelper.IsImage(item))
			{
				continue;
			}
			try
			{
				byte[] imageBytesFromObject = StiImageHelper.GetImageBytesFromObject(item);
				if (imageBytesFromObject != null)
				{
					return imageBytesFromObject;
				}
			}
			catch
			{
			}
		}
		return null;
	}

	private string GetVariableNameFromExpression()
	{
		return StiVariableImageProcessor.GetVariableNameFromExpression(((Control)textBoxExpression).Text);
	}

	private string GetResourceNameFromHyperlink()
	{
		return StiHyperlinkProcessor.GetResourceNameFromHyperlink(((Control)textBoxHyperlink).Text);
	}

	private List<StiResource> GetResources()
	{
		return (from v in imageComp.Report.Dictionary.Resources.ToList()
			where v.Type == StiResourceType.Image && v.Content != null
			select v).ToList();
	}

	private List<StiVariable> GetVariables()
	{
		return (from v in imageComp.Report.Dictionary.Variables.ToList()
			where v.ValueObject is Image
			select v).ToList();
	}

	private void ImageGalleryClick(object sender, EventArgs e)
	{
		ResetDataColumnGallerySelection();
		ResetFileHistorySelection();
		ResetHyperlinkHistorySelection();
		Specification = null;
		isResourceGalleryActivated = false;
		isVariableGalleryActivated = false;
		isColumnGalleryActivated = false;
		(from Control c in (IEnumerable)((Control)listGallery).Controls
			where c is StiImageButton && c != sender
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton && ((CheckBox)stiImageButton).Checked)
		{
			ImageBytes = StiImageConverter.ImageToBytes(((ButtonBase)stiImageButton).Image);
			if (((Control)stiImageButton).Tag is StiResource stiResource)
			{
				((Control)textBoxHyperlink).Text = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
				Specification = stiResource.Name;
				isResourceGalleryActivated = true;
			}
			if (((Control)stiImageButton).Tag is StiVariable stiVariable)
			{
				((Control)textBoxExpression).Text = $"{{{stiVariable.Name}}}";
				Specification = stiVariable.Name;
				isVariableGalleryActivated = true;
			}
			if (((Control)stiImageButton).Tag is StiDataColumn stiDataColumn)
			{
				dataColumn = stiDataColumn.GetColumnPath();
				SelectDataColumn(dataColumn);
				Specification = stiDataColumn.GetColumnPath();
				isColumnGalleryActivated = true;
			}
		}
	}

	private void ResetImageGallerySelection()
	{
		foreach (object item in (ArrangedElementCollection)((Control)listGallery).Controls)
		{
			if (item is StiImageButton stiImageButton)
			{
				((CheckBox)stiImageButton).Checked = false;
			}
		}
	}

	internal void CheckImageStates()
	{
		StiButtonUX stiButtonUX = buttonSaveFile;
		bool enabled = (((Control)buttonRemove).Enabled = ImageBytes != null);
		((Control)stiButtonUX).Enabled = enabled;
	}

	private void ResetDataColumnGallerySelection()
	{
		foreach (object item in (ArrangedElementCollection)((Control)listDataColumnGallery).Controls)
		{
			if (item is StiImageButton stiImageButton)
			{
				((CheckBox)stiImageButton).Checked = false;
			}
		}
	}

	private void BuildDataColumnGallery()
	{
		List<StiDataColumn> columns = GetColumns();
		List<StiDataSource> dataSources = columns.Select((StiDataColumn c) => c.DataSource).Distinct().ToList();
		if (StiOptions.Designer.Editors.AllowConnectToDataInGallery)
		{
			imageComp.Report.Dictionary.TryConnect(loadData: true, dataSources);
		}
		((Control)listDataColumnGallery).Controls.Clear();
		string currentColumn = GetCurrentDataColumn();
		Task.Factory.StartNew(delegate
		{
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Expected O, but got Unknown
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Expected O, but got Unknown
			try
			{
				int imageCount = 0;
				int num = ItemGalleryStep;
				int itemGalleryStep = ItemGalleryStep;
				SetProgressVisible((Control)(object)panelProgressDataColumn, visible: true);
				foreach (StiDataColumn item in columns)
				{
					byte[] imageFromColumn = GetImageFromColumn(item);
					if (imageFromColumn != null)
					{
						string columnPath = item.GetColumnPath();
						StiImageButton button = CreateGalleryButton(columnPath, imageFromColumn, columnPath, num, itemGalleryStep, ItemGallerySize);
						((CheckBox)button).Checked = columnPath == currentColumn;
						((CheckBox)button).CheckedChanged += delegate(object sender, EventArgs e)
						{
							DataColumnGalleryClick(sender);
						};
						((CheckBox)button).DoubleClick += This_OkClicked;
						((Control)button).Width = ItemGallerySize * 2;
						num += ((Control)button).Width + ItemGalleryStep;
						int num2 = imageCount;
						imageCount = num2 + 1;
						((Control)this).Invoke((Delegate)(MethodInvoker)delegate
						{
							if (!IsFormDestructed())
							{
								toolTip.SetToolTip((Control)(object)button, columnPath);
								((Control)listDataColumnGallery).Controls.Add((Control)(object)button);
							}
						});
					}
				}
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					if (!IsFormDestructed())
					{
						ScrollToActiveButton(listDataColumnGallery);
						((Control)groupDataColumnGallery).Visible = imageCount > 0;
					}
				});
			}
			finally
			{
				if (StiOptions.Designer.Editors.AllowConnectToDataInGallery)
				{
					imageComp.Report.Dictionary.TryDisconnect();
				}
				SetProgressVisible((Control)(object)panelProgressDataColumn, visible: false);
			}
		});
	}

	private List<StiDataColumn> GetColumns()
	{
		IEnumerable<StiDataColumn> source = imageComp.Report.Dictionary.DataSources.ToList().SelectMany((StiDataSource d) => d.Columns.ToList());
		return source.Where((StiDataColumn c) => c.Type == typeof(Image) || c.Type == typeof(byte[]) || c.Type == typeof(string)).ToList();
	}

	private string GetCurrentDataColumn()
	{
		return StiDataBuilder.GetColumnPathFromNode(((TreeView)treeViewDataColumn).SelectedNode, useRelationName: true);
	}

	private void DataColumnGalleryClick(object sender)
	{
		(from Control c in (IEnumerable)((Control)listDataColumnGallery).Controls
			where c is StiImageButton && c != sender
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (!(sender is StiImageButton stiImageButton))
		{
			return;
		}
		string dataColumn = ((Control)stiImageButton).Tag as string;
		if (dataColumn != null)
		{
			SelectDataColumn(dataColumn);
			StiImageButton stiImageButton2 = ((IEnumerable)((Control)listGallery).Controls).Cast<StiImageButton>().FirstOrDefault((StiImageButton c) => ((Control)c).Tag is StiDataColumn && ((StiDataColumn)((Control)c).Tag).GetColumnPath() == dataColumn);
			if (stiImageButton2 != null)
			{
				((CheckBox)stiImageButton2).Checked = true;
			}
		}
	}

	private void BuildDataColumn()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		((TreeView)treeViewDataColumn).ImageList = StiDataBuilder.Images;
		TreeNode val = new TreeNode(StiLocalization.Get("Report", "No"));
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		((TreeView)treeViewDataColumn).Nodes.Add(val);
		dataBuilder.Build(((TreeView)treeViewDataColumn).Nodes, imageComp.Report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		SelectDataColumn(imageComp.DataColumn);
	}

	private void SelectDataColumn(string columnPath)
	{
		bool flag = false;
		if (!string.IsNullOrEmpty(columnPath))
		{
			string[] strs = columnPath.Split(new char[1] { '.' });
			flag = SelectNode(strs, 0, ((TreeView)treeViewDataColumn).Nodes, (TreeView)(object)treeViewDataColumn);
		}
		if (!flag)
		{
			((TreeView)treeViewDataColumn).SelectedNode = ((TreeView)treeViewDataColumn).Nodes[0];
			((TreeView)treeViewDataColumn).SelectedNode.EnsureVisible();
		}
	}

	private bool SelectNode(string[] strs, int index, TreeNodeCollection nodes, TreeView view)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (strs.Length <= index)
		{
			return false;
		}
		string text = strs[index];
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			string text2 = null;
			string str;
			if (val.Tag is StiDataRelation stiDataRelation)
			{
				str = stiDataRelation.Name;
				text2 = stiDataRelation.NameInSource;
			}
			else if (val.Tag is StiDataSource stiDataSource)
			{
				str = stiDataSource.Name;
				text2 = (stiDataSource as StiDataStoreSource)?.NameInSource;
			}
			else
			{
				str = val.Text;
			}
			str = StiNameValidator.CorrectName(str, imageComp.Report);
			text2 = ((text2 != null) ? StiNameValidator.CorrectName(text2, imageComp.Report) : null);
			if (!(str == text) && !(text2 == text))
			{
				continue;
			}
			if (val.Nodes.Count == 0)
			{
				if (strs.Length - 1 != index)
				{
					return false;
				}
				view.SelectedNode = val;
				return true;
			}
			if (val.Nodes.Count == 1 && val.Nodes[0].Text == "Loading")
			{
				ExpandTreeNode(val);
				val.Expand();
			}
			return SelectNode(strs, index + 1, val.Nodes, view);
		}
		return false;
	}

	private void ExpandTreeNode(TreeNode node)
	{
		if (node is TreeNodeEx node2 && node.Tag != null)
		{
			dataBuilder.ExpandedNode(node2, includeCalcColumn: true);
		}
	}

	private void BuildHyperlinkHistory()
	{
		((Control)groupHyperlinkHistory).Visible = hyperlinkHistoryList.Count > 0;
		((Control)listHyperlinkHistory).Controls.Clear();
		Task.Factory.StartNew(delegate
		{
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Expected O, but got Unknown
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			int num = ItemGalleryStep;
			int itemGalleryStep = ItemGalleryStep;
			int width = (int)((double)ItemGallerySize * 1.5);
			foreach (string hyperlink in hyperlinkHistoryList)
			{
				byte[] image = null;
				try
				{
					image = StiHyperlinkProcessor.GetBytes(imageComp.Report, hyperlink);
				}
				catch
				{
				}
				if (IsFormDestructed())
				{
					return;
				}
				string fileName = Path.GetFileName(hyperlink);
				StiImageButton button = CreateGalleryButton(fileName, image, hyperlink, num, itemGalleryStep, ItemGallerySize);
				((CheckBox)button).Checked = ((Control)textBoxHyperlink).Text == fileName;
				((Control)button).Click += HyperlinkHistory_Click;
				((CheckBox)button).DoubleClick += This_OkClicked;
				((Control)button).Width = width;
				((Control)button).ContextMenuStrip = contextMenuHistory;
				num += ((Control)button).Width + ItemGalleryStep;
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					if (!IsFormDestructed())
					{
						toolTip.SetToolTip((Control)(object)button, hyperlink);
						((Control)listHyperlinkHistory).Controls.Add((Control)(object)button);
					}
				});
			}
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				if (!IsFormDestructed())
				{
					ScrollToActiveButton(listHyperlinkHistory);
				}
			});
		});
	}

	private void TryLoadImageFromHyperlink()
	{
		if (string.IsNullOrWhiteSpace(((Control)textBoxHyperlink).Text))
		{
			return;
		}
		Task.Factory.StartNew(delegate
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			try
			{
				Thread.Sleep(1000);
				if (TabControl.SelectedTab == pageHyperlink)
				{
					SetProgressVisible((Control)(object)progressHyperlinkPreview, visible: true);
					if (!string.IsNullOrWhiteSpace(((Control)textBoxHyperlink).Text))
					{
						imageHyperlink = StiHyperlinkProcessor.GetBytes(imageComp.Report, ((Control)textBoxHyperlink).Text);
						if (!IsFormDestructed())
						{
							((Control)this).Invoke((Delegate)(MethodInvoker)delegate
							{
								((Control)imageHyperlinkPreview).Invalidate();
							});
						}
					}
				}
			}
			catch
			{
				imageHyperlink = null;
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					((Control)imageHyperlinkPreview).Invalidate();
				});
			}
			finally
			{
				SetProgressVisible((Control)(object)progressHyperlinkPreview, visible: false);
			}
		});
	}

	private void SetProgressVisible(Control progress, bool visible)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		try
		{
			if (!IsFormDestructed())
			{
				progress.Invoke((Delegate)(MethodInvoker)delegate
				{
					progress.Visible = visible;
				});
			}
		}
		catch
		{
		}
	}

	private void HyperlinkHistory_Click(object sender, EventArgs e)
	{
		ResetImageGallerySelection();
		ResetDataColumnGallerySelection();
		ResetFileHistorySelection();
		(from Control c in (IEnumerable)((Control)listHyperlinkHistory).Controls
			where c is StiImageButton
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton)
		{
			ImageBytes = StiImageConverter.ImageToBytes(((ButtonBase)stiImageButton).Image);
			if (((Control)stiImageButton).Tag is string text)
			{
				((Control)textBoxHyperlink).Text = text;
				TryLoadImageFromHyperlink();
			}
		}
	}

	private void ResetHyperlinkHistorySelection()
	{
		foreach (object item in (ArrangedElementCollection)((Control)listHyperlinkHistory).Controls)
		{
			if (item is StiImageButton stiImageButton)
			{
				((CheckBox)stiImageButton).Checked = false;
			}
		}
	}

	private void AddToHyperlinkHistory(string hyperlink)
	{
		if (!string.IsNullOrWhiteSpace(hyperlink) && !StiHyperlinkProcessor.IsResourceHyperlink(hyperlink) && hyperlinkHistoryList.All((string f) => f != hyperlink))
		{
			hyperlinkHistoryList.Add(hyperlink);
		}
	}

	private void RemoveHyperlinkFromHistory(string hyperlink)
	{
		if (hyperlink != null)
		{
			hyperlinkHistoryList.Remove(hyperlink);
		}
	}

	private void BuildFileHistory()
	{
		((Control)groupFileHistory).Visible = fileHistoryList.Count > 0;
		((Control)listFileHistory).Controls.Clear();
		Task.Factory.StartNew(delegate
		{
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			int num = ItemGalleryStep;
			int itemGalleryStep = ItemGalleryStep;
			foreach (string file in fileHistoryList)
			{
				byte[] image = StiImageHelper.TryBytesFromFile(file);
				if (IsFormDestructed())
				{
					return;
				}
				string fileName = Path.GetFileName(file);
				StiImageButton button = CreateGalleryButton(fileName, image, file, num, itemGalleryStep, ItemGallerySize);
				((CheckBox)button).Checked = ((Control)textBoxFile).Text == fileName;
				((Control)button).Click += FileHistory_Click;
				((CheckBox)button).DoubleClick += This_OkClicked;
				((Control)button).Width = (int)((double)ItemGallerySize * 1.5);
				((Control)button).ContextMenuStrip = contextMenuHistory;
				num += ((Control)button).Width + ItemGalleryStep;
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					toolTip.SetToolTip((Control)(object)button, file);
					((Control)listFileHistory).Controls.Add((Control)(object)button);
				});
			}
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				ScrollToActiveButton(listFileHistory);
			});
		});
	}

	private void FileHistory_Click(object sender, EventArgs e)
	{
		ResetImageGallerySelection();
		ResetDataColumnGallerySelection();
		ResetHyperlinkHistorySelection();
		(from Control c in (IEnumerable)((Control)listFileHistory).Controls
			where c is StiImageButton
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton)
		{
			ImageBytes = StiImageConverter.ImageToBytes(((ButtonBase)stiImageButton).Image);
			if (((Control)stiImageButton).Tag is string text)
			{
				((Control)textBoxFile).Text = text;
				TryLoadImageFromFile();
			}
		}
	}

	private void ResetFileHistorySelection()
	{
		foreach (object item in (ArrangedElementCollection)((Control)listFileHistory).Controls)
		{
			if (item is StiImageButton stiImageButton)
			{
				((CheckBox)stiImageButton).Checked = false;
			}
		}
	}

	private void AddToFileHistory(string file)
	{
		if (!string.IsNullOrWhiteSpace(file) && fileHistoryList.All((string f) => f != file))
		{
			fileHistoryList.Add(file);
		}
	}

	private void RemoveFileFromHistory(string file)
	{
		if (file != null)
		{
			fileHistoryList.Remove(file.ToLowerInvariant());
		}
	}

	private void TryLoadImageFromFile()
	{
		try
		{
			imageFile = null;
			if (!string.IsNullOrWhiteSpace(((Control)textBoxFile).Text) && File.Exists(((Control)textBoxFile).Text))
			{
				imageFile = StiImageHelper.BytesFromFile(((Control)textBoxFile).Text);
			}
		}
		catch
		{
			imageFile = null;
		}
		finally
		{
			((Control)imageFilePreview).Invalidate();
		}
	}

	private void BuildIconControls()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		iconsPopup = new StiPopup((Control)(object)GetIconPanel());
		((Control)panelBoxIcon).Paint += new PaintEventHandler(PanelBoxIcon_Paint);
		imageIcon = imageComp.Icon;
		imageIconColor = imageComp.IconColor;
		colorBoxIconColor.SelectedColor = imageIconColor;
		((Control)buttonIconRemove).Visible = imageIcon.HasValue;
	}

	private void RefreshSampleIcon()
	{
		DrawBoxIcon();
		DrawSampleIcon();
	}

	private void DrawSampleIcon()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Graphics val = ((Control)panelSampleIcon).CreateGraphics();
		val.Clear(StiUX.BackgroundDark);
		val.TextRenderingHint = (TextRenderingHint)3;
		Pen val2 = new Pen(StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.ControlDark);
		try
		{
			if (!StiUX.IsDark)
			{
				val2.DashStyle = (DashStyle)1;
			}
			val.DrawRectangle(val2, 0, 0, ((Control)panelSampleIcon).Width - StiScale.I1, ((Control)panelSampleIcon).Height - StiScale.I1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (!imageIcon.HasValue)
		{
			return;
		}
		SolidBrush val3 = new SolidBrush(imageIconColor);
		try
		{
			Font val4 = new Font(fontFamilyIcons, 180f);
			try
			{
				StringFormat val5 = new StringFormat();
				try
				{
					val5.Alignment = (StringAlignment)1;
					val5.LineAlignment = (StringAlignment)1;
					val.DrawString(StiFontIconsHelper.GetContent(imageIcon.GetValueOrDefault()), val4, (Brush)(object)val3, (RectangleF)new Rectangle(StiScale.I2, StiScale.I20, ((Control)panelSampleIcon).Width, ((Control)panelSampleIcon).Height - StiScale.I20), val5);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	private void DrawBoxIcon()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Graphics val = ((Control)panelBoxIcon).CreateGraphics();
		val.TextRenderingHint = (TextRenderingHint)3;
		StiDrawing.FillRectangle(val, StiUX.Background, new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width - StiScale.I4, ((Control)panelBoxIcon).Height - StiScale.I6));
		if (imageIcon.HasValue)
		{
			SolidBrush val2 = new SolidBrush(imageIconColor);
			try
			{
				Font val3 = new Font(fontFamilyIcons, 13f);
				try
				{
					StringFormat val4 = new StringFormat();
					try
					{
						val.DrawString(StiFontIconsHelper.GetContent(imageIcon.GetValueOrDefault()), val3, (Brush)(object)val2, (RectangleF)new Rectangle(StiScale.I2, StiScale.I4, ((Control)panelBoxIcon).Width, ((Control)panelBoxIcon).Height), val4);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		StiDrawing.DrawRectangle(val, StiUX.IsDark ? StiUX.Separator : SystemColors.ControlDark, 0.0, 0.0, ((Control)panelBoxIcon).Width - 1, ((Control)panelBoxIcon).Height - 1);
	}

	private Panel GetIconPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		Panel val = new Panel
		{
			Width = StiScale.XXI(274.0),
			Height = StiScale.YYI(342.0),
			BackColor = StiUX.Background,
			Padding = new Padding(StiScale.I1)
		};
		((Control)val).Paint += new PaintEventHandler(StiIconControlHelper.PanelRootPainter);
		Panel val2 = new Panel
		{
			Dock = (DockStyle)1,
			Size = new Size(StiScale.XXI(150.0), StiScale.YYI(28.0)),
			Padding = new Padding(StiScale.I10, StiScale.I6, StiScale.I10, StiScale.I6),
			BackColor = StiUX.Background
		};
		StiHintTextBox stiHintTextBox = new StiHintTextBox();
		((Control)stiHintTextBox).Dock = (DockStyle)5;
		stiHintTextBox.AllowDrawBorder = false;
		stiHintTextBox.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)stiHintTextBox).Font = new Font("Arial", 10f);
		textBoxSearch = stiHintTextBox;
		((Control)textBoxSearch).TextChanged += TextBoxSearch_TextChanged;
		((Control)val2).Controls.Add((Control)(object)textBoxSearch);
		panelIcons = new Panel
		{
			AutoScroll = true,
			Dock = (DockStyle)5
		};
		((Control)val).Controls.Add((Control)(object)panelIcons);
		((Control)val).Controls.Add((Control)(object)val2);
		BuildIconPanel();
		return val;
	}

	private void BuildIconPanel(string filterName = null)
	{
		((Control)panelIcons).Controls.Clear();
		StiFontIconSelectionControl stiFontIconSelectionControl = new StiFontIconSelectionControl(imageIcon, imageIconColor);
		((Control)stiFontIconSelectionControl).Location = new Point(StiScale.I2, StiScale.I2);
		((Control)stiFontIconSelectionControl).Width = ((Control)panelIcons).Width;
		((Control)stiFontIconSelectionControl).Anchor = (AnchorStyles)13;
		stiFontIconSelectionControl.TextFilter = filterName;
		fontIconSelectionControl = stiFontIconSelectionControl;
		fontIconSelectionControl.ValueChanged += FontIconSelectionControl_ValueChanged;
		((Control)panelIcons).Controls.Add((Control)(object)fontIconSelectionControl);
	}

	private void CheckColorIcons()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		foreach (object item in (ArrangedElementCollection)((Control)(Panel)iconsPopup.Content).Controls)
		{
			if (item is StiFontIconButton stiFontIconButton)
			{
				((Control)stiFontIconButton).ForeColor = imageIconColor;
			}
		}
	}

	private void This_OkClicked(object sender, EventArgs e)
	{
		imageComp.ResetAllImageProperties();
		string value = StiExpressionPacker.UnPackExpression(((Control)textBoxExpression).Text, imageComp.Report.Designer, useBraces: true);
		string text = StiExpressionPacker.UnPackExpression(((Control)textBoxHyperlink).Text, imageComp.Report.Designer, useBraces: true);
		if (TabControl.SelectedTab == pageImage && isResourceGalleryActivated)
		{
			imageComp.ImageURL.Value = text;
		}
		else if (TabControl.SelectedTab == pageImage && isVariableGalleryActivated)
		{
			imageComp.ImageData.Value = value;
		}
		else if (TabControl.SelectedTab == pageImage && isColumnGalleryActivated)
		{
			imageComp.DataColumn = dataColumn;
		}
		else if (TabControl.SelectedTab == pageImage)
		{
			imageComp.PutImage(ImageBytes);
		}
		else if (TabControl.SelectedTab == pageDataColumn)
		{
			if (((TreeView)treeViewDataColumn).SelectedNode != null)
			{
				if (((TreeView)treeViewDataColumn).SelectedNode.Text == StiLocalization.Get("Report", "No"))
				{
					imageComp.DataColumn = string.Empty;
				}
				if (((TreeView)treeViewDataColumn).SelectedNode.Tag is StiDataColumn)
				{
					imageComp.DataColumn = GetCurrentDataColumn();
				}
			}
		}
		else if (TabControl.SelectedTab == pageExpression)
		{
			imageComp.ImageData.Value = value;
		}
		else if (TabControl.SelectedTab == pageHyperlink)
		{
			imageComp.ImageURL.Value = text;
			AddToHyperlinkHistory(text);
		}
		else if (TabControl.SelectedTab == pageFile)
		{
			imageComp.File = ((Control)textBoxFile).Text;
			AddToFileHistory(((Control)textBoxFile).Text);
		}
		else if (TabControl.SelectedTab == pageIcon)
		{
			imageComp.Icon = imageIcon;
			imageComp.IconColor = imageIconColor;
		}
		((Form)this).Close();
	}

	private void This_Load(object sender, EventArgs e)
	{
		ScrollToActiveButton(listGallery);
		BuildImageGallery();
		BuildDataColumnGallery();
		BuildFileHistory();
		BuildHyperlinkHistory();
		BuildIconControls();
	}

	private void This_SizeChanged(object sender, EventArgs e)
	{
		((Control)panelImage).Invalidate();
		((Control)imageHyperlinkPreview).Invalidate();
		((Control)imageFilePreview).Invalidate();
		((Control)panelIcon).Invalidate();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		SaveSettings();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_graphic_information_output_resources_of_images.htm");
	}

	private void PanelImage_DoubleClick(object sender, EventArgs e)
	{
		ButtonOpen_Click(sender, e);
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		ExpandTreeNode(e.Node);
	}

	private void ButtonOpen_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			try
			{
				if (StiMaximumResourceSizeMessageBox.Check((Form)(object)this, ((FileDialog)val).FileName))
				{
					ImageBytes = StiImageHelper.BytesFromFile(((FileDialog)val).FileName);
					ResetGalleryAndHistorySelection();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonSaveFile_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			((FileDialog)val).FileName = Loc.Get("Components", "StiImage");
			((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			try
			{
				if (StiSvgHelper.IsSvg(ImageBytes))
				{
					File.WriteAllBytes(((FileDialog)val).FileName, ImageBytes);
					return;
				}
				ImageFormat val2 = ImageFormat.Png;
				switch (Path.GetExtension(((FileDialog)val).FileName)?.ToLowerInvariant())
				{
				case ".bmp":
					val2 = ImageFormat.Bmp;
					break;
				case ".gif":
					val2 = ImageFormat.Gif;
					break;
				case ".jpeg":
				case ".jpg":
					val2 = ImageFormat.Jpeg;
					break;
				case ".tiff":
					val2 = ImageFormat.Tiff;
					break;
				}
				Image val3 = StiImageConverter.BytesToImage(ImageBytes);
				try
				{
					if (val3 != null)
					{
						val3.Save(((FileDialog)val).FileName, val2);
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		ImageBytes = null;
		Specification = null;
		if (IsResourceFromGallery)
		{
			((Control)textBoxHyperlink).Text = string.Empty;
		}
		if (IsVariableFromGallery)
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		ResetGalleryAndHistorySelection();
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (TabControl.SelectedTab == pageExpression)
		{
			((Control)textBoxExpression).Focus();
		}
		if (TabControl.SelectedTab == pageFile)
		{
			((Control)textBoxFile).Focus();
			TryLoadImageFromFile();
		}
		if (TabControl.SelectedTab == pageHyperlink)
		{
			((Control)textBoxHyperlink).Focus();
			TryLoadImageFromHyperlink();
		}
		if (TabControl.SelectedTab == pageIcon)
		{
			DrawSampleIcon();
		}
	}

	private void ButtonExpression_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxExpression).Text;
		text = StiExpressionPacker.PackExpression(text, imageComp.Report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(imageComp.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxExpression).Text = StiExpressionPacker.UnPackExpression(text, imageComp.Report.Designer, useBraces: false);
		}
		((Control)textBoxExpression).Focus();
		((TextBoxBase)textBoxExpression).Select(0, 0);
	}

	private void PanelImage_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelImage, e.Graphics, ImageBytes, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void ButtonHyperlink_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxHyperlink).Text;
		text = StiExpressionPacker.PackExpression(text, imageComp.Report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(imageComp.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxHyperlink).Text = StiExpressionPacker.UnPackExpression(text, imageComp.Report.Designer, useBraces: false);
			TryLoadImageFromHyperlink();
		}
		((Control)textBoxHyperlink).Focus();
		((TextBoxBase)textBoxHyperlink).Select(0, 0);
	}

	private void TextBoxHyperlink_Leave(object sender, EventArgs e)
	{
		TryLoadImageFromHyperlink();
	}

	private void TextBoxHyperlink_TextChanged(object sender, EventArgs e)
	{
		timerHyperlinkPreview.Stop();
		timerHyperlinkPreview.Start();
	}

	private void TimerHyperlinkPreview_Tick(object sender, EventArgs e)
	{
		timerHyperlinkPreview.Stop();
		TryLoadImageFromHyperlink();
	}

	private void ImageHyperlinkPreview_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)imageHyperlinkPreview, e.Graphics, imageHyperlink, drawDragDrop: false, drawBorder: true, isImage: true);
	}

	private void ButtonFile_Click(object sender, EventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		string initialDirectory = null;
		try
		{
			initialDirectory = ((!string.IsNullOrWhiteSpace(((Control)textBoxFile).Text)) ? Path.GetDirectoryName(((Control)textBoxFile).Text) : null);
		}
		catch
		{
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiFileDialogHelper.GetImageFilter();
			((FileDialog)val).InitialDirectory = initialDirectory;
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				((Control)textBoxFile).Text = ((FileDialog)val).FileName;
				TryLoadImageFromFile();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		((Control)textBoxFile).Focus();
	}

	private void DeleteHistoryItem_Click(object sender, EventArgs e)
	{
		ToolStripMenuItem val = (ToolStripMenuItem)((sender is ToolStripMenuItem) ? sender : null);
		if (val == null)
		{
			return;
		}
		ToolStrip owner = ((ToolStripItem)val).Owner;
		ContextMenuStrip val2 = (ContextMenuStrip)(object)((owner is ContextMenuStrip) ? owner : null);
		if (val2 == null || !(val2.SourceControl is StiImageButton stiImageButton) || !(((Control)stiImageButton).Tag is string text))
		{
			return;
		}
		if (((Control)stiImageButton).Parent == listFileHistory)
		{
			RemoveFileFromHistory(text);
			((Control)listFileHistory).Controls.Remove((Control)(object)stiImageButton);
			int num = ItemGalleryStep;
			foreach (StiImageButton item in (ArrangedElementCollection)((Control)listFileHistory).Controls)
			{
				((Control)item).Left = num;
				num += ((Control)item).Width + ItemGalleryStep;
			}
		}
		else if (((Control)stiImageButton).Parent == listHyperlinkHistory)
		{
			RemoveHyperlinkFromHistory(text);
			((Control)listHyperlinkHistory).Controls.Remove((Control)(object)stiImageButton);
			int num2 = ItemGalleryStep;
			foreach (StiImageButton item2 in (ArrangedElementCollection)((Control)listHyperlinkHistory).Controls)
			{
				((Control)item2).Left = num2;
				num2 += ((Control)item2).Width + ItemGalleryStep;
			}
		}
		SaveSettings();
	}

	private void DeleteAllHistory_Click(object sender, EventArgs e)
	{
		ToolStripMenuItem val = (ToolStripMenuItem)((sender is ToolStripMenuItem) ? sender : null);
		if (val == null)
		{
			return;
		}
		ToolStrip owner = ((ToolStripItem)val).Owner;
		ContextMenuStrip val2 = (ContextMenuStrip)(object)((owner is ContextMenuStrip) ? owner : null);
		if (val2 != null && val2.SourceControl is StiImageButton stiImageButton)
		{
			if (((Control)stiImageButton).Parent == listFileHistory)
			{
				((Control)listFileHistory).Controls.Clear();
				fileHistoryList.Clear();
			}
			if (((Control)stiImageButton).Parent == listHyperlinkHistory)
			{
				((Control)listHyperlinkHistory).Controls.Clear();
				hyperlinkHistoryList.Clear();
			}
		}
	}

	private void ImageFilePreview_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)imageFilePreview, e.Graphics, imageFile, drawDragDrop: true, drawBorder: true, isImage: true);
	}

	private void TextBoxFile_Leave(object sender, EventArgs e)
	{
		TryLoadImageFromFile();
	}

	private void FontIconSelectionControl_ValueChanged(object sender, EventArgs e)
	{
		imageIcon = ((StiFontIconSelectionControl)sender).SelectedIcon;
		RefreshSampleIcon();
		((Control)iconsPopup).Hide();
		((Control)buttonIconRemove).Visible = imageIcon.HasValue;
	}

	private void TextBoxSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		string filterName = ((Control)stiHintTextBox).Text.ToLower();
		BuildIconPanel(filterName);
	}

	private void PanelBoxIcon_Paint(object sender, PaintEventArgs e)
	{
		DrawBoxIcon();
	}

	private void ButtonIcon_Click(object sender, EventArgs e)
	{
		CheckColorIcons();
		((ToolStripDropDown)iconsPopup).Show(new Point(((Control)panelBoxIcon).RectangleToScreen(Rectangle.Empty).Left, ((Control)buttonIcon).RectangleToScreen(Rectangle.Empty).Bottom + ((Control)buttonIcon).Height + StiScale.I2));
		((Control)textBoxSearch).Focus();
	}

	private void ColorBoxIconColor_SelectedColorChanged(object sender, EventArgs e)
	{
		imageIconColor = colorBoxIconColor.SelectedColor;
		BuildIconPanel();
		RefreshSampleIcon();
	}

	private void ButtonIconRemove_Click(object sender, EventArgs e)
	{
		imageIcon = null;
		((Control)buttonIconRemove).Visible = false;
		RefreshSampleIcon();
	}

	private void PanelSampleIcon_Paint(object sender, PaintEventArgs e)
	{
		DrawSampleIcon();
	}

	private void Panel_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array == null)
		{
			return;
		}
		foreach (string item in array.Where((string f) => f != null))
		{
			string ext = Path.GetExtension(item).ToLowerInvariant();
			if (StiResourceTypeHelper.IsImageType(ext))
			{
				if (sender == panelImage && StiMaximumResourceSizeMessageBox.Check((Form)(object)this, item))
				{
					ImageBytes = StiImageHelper.TryBytesFromFile(item);
				}
				if (sender == imageFilePreview)
				{
					((Control)textBoxFile).Text = item;
					TryLoadImageFromFile();
				}
				break;
			}
		}
	}

	private void Panel_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			foreach (string item in array.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiResourceTypeHelper.IsImageType(ext))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Expected O, but got Unknown
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Expected O, but got Unknown
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Expected O, but got Unknown
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Expected O, but got Unknown
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a73: Expected O, but got Unknown
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Expected O, but got Unknown
		//IL_0c3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Expected O, but got Unknown
		//IL_0e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e30: Expected O, but got Unknown
		//IL_0e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb6: Expected O, but got Unknown
		//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Expected O, but got Unknown
		//IL_1237: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f1: Expected O, but got Unknown
		//IL_14c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cf: Expected O, but got Unknown
		//IL_1507: Unknown result type (might be due to invalid IL or missing references)
		//IL_1646: Unknown result type (might be due to invalid IL or missing references)
		//IL_1650: Expected O, but got Unknown
		//IL_18cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d7: Expected O, but got Unknown
		//IL_1abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Expected O, but got Unknown
		//IL_1b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c33: Expected O, but got Unknown
		//IL_1c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4a: Expected O, but got Unknown
		//IL_1c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c61: Expected O, but got Unknown
		//IL_1e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e63: Expected O, but got Unknown
		//IL_2035: Unknown result type (might be due to invalid IL or missing references)
		//IL_203f: Expected O, but got Unknown
		//IL_2077: Unknown result type (might be due to invalid IL or missing references)
		//IL_219f: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a9: Expected O, but got Unknown
		//IL_2210: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2351: Unknown result type (might be due to invalid IL or missing references)
		//IL_23aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b4: Expected O, but got Unknown
		//IL_23ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_245e: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ed: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiImageEditorForm));
		toolTip = new ToolTip(components);
		timerHyperlinkPreview = new Timer(components);
		contextMenuHistory = new ContextMenuStrip(components);
		deleteFileHistoryItem = new ToolStripMenuItem();
		deleteAllFileHistoryItem = new ToolStripMenuItem();
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		TabControl = new StiCloudTabulator();
		pageImage = new StiCloudTabulatorPage();
		panel5 = new StiPanelUX();
		panelImageContainer = new StiPanelUX();
		panelImage = new StiPanelUX();
		buttonSaveFile = new StiButtonUX();
		labelSpecification = new StiLabelUX();
		buttonOpenFile = new StiButtonUX();
		buttonRemove = new StiButtonUX();
		groupGallery = new StiOffice2013GroupBox();
		listGallery = new ScrollableControl();
		pageDataColumn = new StiCloudTabulatorPage();
		panel6 = new StiPanelUX();
		treeViewDataColumn = new StiTreeViewUX();
		groupDataColumnGallery = new StiOffice2013GroupBox();
		listDataColumnGallery = new ScrollableControl();
		panelProgressDataColumn = new StiPanelUX();
		progressDataColumn = new ProgressBar();
		pageExpression = new StiCloudTabulatorPage();
		labelExpression = new StiLabelUX();
		panelExpression = new StiPanelBox();
		textBoxExpression = new HintTextBox();
		buttonExpression = new StiButtonUX();
		pageHyperlink = new StiCloudTabulatorPage();
		panel4 = new StiPanelUX();
		imageHyperlinkPreview = new StiPanelUX();
		progressHyperlinkPreview = new ProgressBar();
		panel3 = new StiPanelUX();
		labelHyperlink = new StiLabelUX();
		panelHyperlink = new StiPanelBox();
		textBoxHyperlink = new HintTextBox();
		buttonHyperlink = new StiButtonUX();
		groupHyperlinkHistory = new StiOffice2013GroupBox();
		listHyperlinkHistory = new ScrollableControl();
		pageFile = new StiCloudTabulatorPage();
		panel1 = new StiPanelUX();
		imageFilePreview = new StiPanelUX();
		panel2 = new StiPanelUX();
		labelFile = new StiLabelUX();
		panelBoxFile = new StiPanelBox();
		textBoxFile = new HintTextBox();
		buttonFile = new StiButtonUX();
		groupFileHistory = new StiOffice2013GroupBox();
		listFileHistory = new ScrollableControl();
		pageIcon = new StiCloudTabulatorPage();
		panel7 = new StiPanelUX();
		panelSampleIcon = new StiPanelUX();
		panelIcon = new StiPanelUX();
		colorBoxIconColor = new ColorPickerButton();
		panelBoxIcon = new StiPanelUX();
		buttonIconRemove = new StiOperationButton();
		buttonIcon = new StiOperationButton();
		labelIcon = new StiLabelUX();
		footer = new StiButtonPanelUX();
		((Control)contextMenuHistory).SuspendLayout();
		((Control)TabControl).SuspendLayout();
		((Control)pageImage).SuspendLayout();
		((Control)panel5).SuspendLayout();
		((Control)panelImageContainer).SuspendLayout();
		((Control)panelImage).SuspendLayout();
		((Control)groupGallery).SuspendLayout();
		((Control)pageDataColumn).SuspendLayout();
		((Control)panel6).SuspendLayout();
		((Control)groupDataColumnGallery).SuspendLayout();
		((Control)panelProgressDataColumn).SuspendLayout();
		((Control)pageExpression).SuspendLayout();
		((Control)panelExpression).SuspendLayout();
		((Control)pageHyperlink).SuspendLayout();
		((Control)panel4).SuspendLayout();
		((Control)imageHyperlinkPreview).SuspendLayout();
		((Control)panel3).SuspendLayout();
		((Control)panelHyperlink).SuspendLayout();
		((Control)groupHyperlinkHistory).SuspendLayout();
		((Control)pageFile).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)panelBoxFile).SuspendLayout();
		((Control)groupFileHistory).SuspendLayout();
		((Control)pageIcon).SuspendLayout();
		((Control)panel7).SuspendLayout();
		((Control)panelIcon).SuspendLayout();
		((Control)panelBoxIcon).SuspendLayout();
		((Control)footer).SuspendLayout();
		((Control)this).SuspendLayout();
		timerHyperlinkPreview.Interval = 2000;
		timerHyperlinkPreview.Tick += TimerHyperlinkPreview_Tick;
		((ToolStrip)contextMenuHistory).ImageScalingSize = new Size(20, 20);
		((ToolStrip)contextMenuHistory).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[2]
		{
			(ToolStripItem)deleteFileHistoryItem,
			(ToolStripItem)deleteAllFileHistoryItem
		});
		((Control)contextMenuHistory).Name = "contextMenuHistory";
		((Control)contextMenuHistory).Size = new Size(119, 48);
		((ToolStripItem)deleteFileHistoryItem).Name = "deleteFileHistoryItem";
		((ToolStripItem)deleteFileHistoryItem).Size = new Size(118, 22);
		((ToolStripItem)deleteFileHistoryItem).Text = "Delete";
		((ToolStripItem)deleteFileHistoryItem).Click += DeleteHistoryItem_Click;
		((ToolStripItem)deleteAllFileHistoryItem).Name = "deleteAllFileHistoryItem";
		((ToolStripItem)deleteAllFileHistoryItem).Size = new Size(118, 22);
		((ToolStripItem)deleteAllFileHistoryItem).Text = "Clear All";
		((ToolStripItem)deleteAllFileHistoryItem).Click += DeleteAllHistory_Click;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(602, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 59;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClicked;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(683, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 60;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)TabControl).Controls.Add((Control)(object)pageImage);
		((Control)TabControl).Controls.Add((Control)(object)pageDataColumn);
		((Control)TabControl).Controls.Add((Control)(object)pageExpression);
		((Control)TabControl).Controls.Add((Control)(object)pageHyperlink);
		((Control)TabControl).Controls.Add((Control)(object)pageFile);
		((Control)TabControl).Controls.Add((Control)(object)pageIcon);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Margin = new Padding(0, 3, 3, 3);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(771, 382);
		((Control)TabControl).TabIndex = 58;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageImage).Controls.Add((Control)(object)panel5);
		((Control)pageImage).Controls.Add((Control)(object)groupGallery);
		pageImage.Dock = (DockStyle)5;
		pageImage.Image = (Image)componentResourceManager.GetObject("pageImage.Image");
		pageImage.InvisibleVisible = true;
		((Control)pageImage).Location = new Point(0, 0);
		((Control)pageImage).Name = "pageImage";
		((Control)pageImage).Padding = new Padding(0, 0, 1, 0);
		((Control)pageImage).Size = new Size(652, 382);
		((Control)pageImage).TabIndex = 0;
		((Control)pageImage).Text = "Image";
		((Control)pageImage).Visible = false;
		((Control)panel5).Controls.Add((Control)(object)panelImageContainer);
		((Control)panel5).Dock = (DockStyle)5;
		((Control)panel5).Location = new Point(0, 0);
		((Control)panel5).Name = "panel5";
		((Control)panel5).Size = new Size(651, 249);
		((Control)panel5).TabIndex = 61;
		((Control)panelImageContainer).Controls.Add((Control)(object)panelImage);
		((Control)panelImageContainer).Dock = (DockStyle)5;
		((Control)panelImageContainer).Location = new Point(0, 0);
		((Control)panelImageContainer).Margin = new Padding(2);
		((Control)panelImageContainer).Name = "panelImageContainer";
		((Control)panelImageContainer).Size = new Size(651, 249);
		((Control)panelImageContainer).TabIndex = 59;
		((Control)panelImage).AllowDrop = true;
		((Control)panelImage).Anchor = (AnchorStyles)15;
		((Control)panelImage).Controls.Add((Control)(object)buttonSaveFile);
		((Control)panelImage).Controls.Add((Control)(object)labelSpecification);
		((Control)panelImage).Controls.Add((Control)(object)buttonOpenFile);
		((Control)panelImage).Controls.Add((Control)(object)buttonRemove);
		((Control)panelImage).Location = new Point(12, 12);
		((Control)panelImage).Name = "panelImage";
		((Control)panelImage).Size = new Size(627, 223);
		((Control)panelImage).TabIndex = 58;
		((Control)panelImage).DragDrop += new DragEventHandler(Panel_DragDrop);
		((Control)panelImage).DragOver += new DragEventHandler(Panel_DragOver);
		((Control)panelImage).Paint += new PaintEventHandler(PanelImage_Paint);
		((Control)panelImage).DoubleClick += PanelImage_DoubleClick;
		((Control)buttonSaveFile).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonSaveFile).Image = (Image)componentResourceManager.GetObject("buttonSaveFile.Image");
		((Control)buttonSaveFile).Location = new Point(597, 32);
		((Control)buttonSaveFile).Name = "buttonSaveFile";
		((Control)buttonSaveFile).Size = new Size(24, 24);
		((Control)buttonSaveFile).TabIndex = 3;
		((Control)buttonSaveFile).TabStop = false;
		((ButtonBase)buttonSaveFile).UseVisualStyleBackColor = true;
		((Control)buttonSaveFile).Click += ButtonSaveFile_Click;
		((Control)labelSpecification).Anchor = (AnchorStyles)6;
		((Control)labelSpecification).AutoSize = true;
		((Control)labelSpecification).BackColor = SystemColors.ControlDark;
		((Control)labelSpecification).Location = new Point(7, 198);
		((Control)labelSpecification).Name = "labelSpecification";
		((Control)labelSpecification).Padding = new Padding(3);
		((Control)labelSpecification).Size = new Size(74, 19);
		((Control)labelSpecification).TabIndex = 4;
		((Control)labelSpecification).Text = "Specification";
		((Control)labelSpecification).Visible = false;
		((Control)buttonOpenFile).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonOpenFile).Image = (Image)componentResourceManager.GetObject("buttonOpenFile.Image");
		((Control)buttonOpenFile).Location = new Point(597, 5);
		((Control)buttonOpenFile).Name = "buttonOpenFile";
		((Control)buttonOpenFile).Size = new Size(24, 24);
		((Control)buttonOpenFile).TabIndex = 2;
		((Control)buttonOpenFile).TabStop = false;
		((ButtonBase)buttonOpenFile).UseVisualStyleBackColor = true;
		((Control)buttonOpenFile).Click += ButtonOpen_Click;
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(597, 59);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(24, 24);
		((Control)buttonRemove).TabIndex = 4;
		((Control)buttonRemove).TabStop = false;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)groupGallery).Controls.Add((Control)(object)listGallery);
		((Control)groupGallery).Dock = (DockStyle)2;
		groupGallery.HeaderHeight = 24;
		((Control)groupGallery).Location = new Point(0, 249);
		((Control)groupGallery).Name = "groupGallery";
		((Control)groupGallery).Size = new Size(651, 133);
		((Control)groupGallery).TabIndex = 60;
		((Control)groupGallery).TabStop = false;
		((Control)groupGallery).Text = "Gallery";
		((Control)listGallery).Anchor = (AnchorStyles)15;
		listGallery.AutoScroll = true;
		((Control)listGallery).BackColor = Color.White;
		((Control)listGallery).Location = new Point(4, 27);
		((Control)listGallery).Name = "listGallery";
		((Control)listGallery).Size = new Size(643, 101);
		((Control)listGallery).TabIndex = 59;
		((Control)pageDataColumn).Controls.Add((Control)(object)panel6);
		((Control)pageDataColumn).Controls.Add((Control)(object)groupDataColumnGallery);
		((Control)pageDataColumn).Controls.Add((Control)(object)panelProgressDataColumn);
		pageDataColumn.Dock = (DockStyle)5;
		pageDataColumn.Image = (Image)componentResourceManager.GetObject("pageDataColumn.Image");
		pageDataColumn.InvisibleVisible = false;
		((Control)pageDataColumn).Location = new Point(0, 0);
		((Control)pageDataColumn).Name = "pageDataColumn";
		((Control)pageDataColumn).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataColumn).Size = new Size(652, 382);
		((Control)pageDataColumn).TabIndex = 3;
		((Control)pageDataColumn).Text = "Data Column";
		((Control)pageDataColumn).Visible = false;
		((Control)panel6).Controls.Add((Control)(object)treeViewDataColumn);
		((Control)panel6).Dock = (DockStyle)5;
		((Control)panel6).Location = new Point(0, 0);
		((Control)panel6).Margin = new Padding(2);
		((Control)panel6).Name = "panel6";
		((Control)panel6).Size = new Size(651, 232);
		((Control)panel6).TabIndex = 64;
		((Control)treeViewDataColumn).Anchor = (AnchorStyles)15;
		((TreeView)treeViewDataColumn).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewDataColumn).HideSelection = false;
		((Control)treeViewDataColumn).Location = new Point(12, 12);
		((Control)treeViewDataColumn).Name = "treeViewDataColumn";
		((Control)treeViewDataColumn).Size = new Size(627, 205);
		((Control)treeViewDataColumn).TabIndex = 0;
		((TreeView)treeViewDataColumn).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		((Control)treeViewDataColumn).DoubleClick += This_OkClicked;
		((Control)groupDataColumnGallery).Controls.Add((Control)(object)listDataColumnGallery);
		((Control)groupDataColumnGallery).Dock = (DockStyle)2;
		groupDataColumnGallery.HeaderHeight = 24;
		((Control)groupDataColumnGallery).Location = new Point(0, 232);
		((Control)groupDataColumnGallery).Name = "groupDataColumnGallery";
		((Control)groupDataColumnGallery).Size = new Size(651, 133);
		((Control)groupDataColumnGallery).TabIndex = 61;
		((Control)groupDataColumnGallery).TabStop = false;
		((Control)groupDataColumnGallery).Text = "Gallery";
		((Control)listDataColumnGallery).Anchor = (AnchorStyles)15;
		listDataColumnGallery.AutoScroll = true;
		((Control)listDataColumnGallery).BackColor = Color.White;
		((Control)listDataColumnGallery).Location = new Point(4, 27);
		((Control)listDataColumnGallery).Name = "listDataColumnGallery";
		((Control)listDataColumnGallery).Size = new Size(643, 101);
		((Control)listDataColumnGallery).TabIndex = 59;
		((Control)panelProgressDataColumn).Controls.Add((Control)(object)progressDataColumn);
		((Control)panelProgressDataColumn).Dock = (DockStyle)2;
		((Control)panelProgressDataColumn).Location = new Point(0, 365);
		((Control)panelProgressDataColumn).Name = "panelProgressDataColumn";
		((Control)panelProgressDataColumn).Size = new Size(651, 17);
		((Control)panelProgressDataColumn).TabIndex = 63;
		((Control)panelProgressDataColumn).Visible = false;
		((Control)progressDataColumn).Anchor = (AnchorStyles)13;
		((Control)progressDataColumn).Location = new Point(3, 3);
		((Control)progressDataColumn).Name = "progressDataColumn";
		((Control)progressDataColumn).Size = new Size(643, 10);
		progressDataColumn.Style = (ProgressBarStyle)2;
		((Control)progressDataColumn).TabIndex = 62;
		((Control)pageExpression).Controls.Add((Control)(object)labelExpression);
		((Control)pageExpression).Controls.Add((Control)(object)panelExpression);
		pageExpression.Dock = (DockStyle)5;
		pageExpression.Image = (Image)componentResourceManager.GetObject("pageExpression.Image");
		pageExpression.InvisibleVisible = false;
		((Control)pageExpression).Location = new Point(0, 0);
		((Control)pageExpression).Name = "pageExpression";
		((Control)pageExpression).Padding = new Padding(0, 0, 1, 0);
		((Control)pageExpression).Size = new Size(652, 382);
		((Control)pageExpression).TabIndex = 2;
		((Control)pageExpression).Text = "Expression";
		((Control)pageExpression).Visible = false;
		((Control)labelExpression).Location = new Point(12, 12);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(150, 26);
		((Control)labelExpression).TabIndex = 9;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Control)panelExpression).Anchor = (AnchorStyles)13;
		((Control)panelExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelExpression).Controls.Add((Control)(object)buttonExpression);
		((Control)panelExpression).Location = new Point(168, 12);
		((Control)panelExpression).Name = "panelExpression";
		((Control)panelExpression).Size = new Size(472, 26);
		((Control)panelExpression).TabIndex = 0;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(440, 13);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)buttonExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpression).Image = (Image)componentResourceManager.GetObject("buttonExpression.Image");
		((Control)buttonExpression).Location = new Point(447, 1);
		((Control)buttonExpression).Name = "buttonExpression";
		((Control)buttonExpression).Size = new Size(24, 24);
		((Control)buttonExpression).TabIndex = 1;
		((Control)buttonExpression).TabStop = false;
		((ButtonBase)buttonExpression).UseVisualStyleBackColor = true;
		((Control)buttonExpression).Click += ButtonExpression_Click;
		((Control)pageHyperlink).Controls.Add((Control)(object)panel4);
		((Control)pageHyperlink).Controls.Add((Control)(object)panel3);
		((Control)pageHyperlink).Controls.Add((Control)(object)groupHyperlinkHistory);
		pageHyperlink.Dock = (DockStyle)5;
		pageHyperlink.Image = (Image)componentResourceManager.GetObject("pageHyperlink.Image");
		pageHyperlink.InvisibleVisible = false;
		((Control)pageHyperlink).Location = new Point(0, 0);
		((Control)pageHyperlink).Name = "pageHyperlink";
		((Control)pageHyperlink).Padding = new Padding(0, 0, 1, 0);
		((Control)pageHyperlink).Size = new Size(652, 382);
		((Control)pageHyperlink).TabIndex = 1;
		((Control)pageHyperlink).Text = "Hyperlink";
		((Control)pageHyperlink).Visible = false;
		((Control)panel4).Controls.Add((Control)(object)imageHyperlinkPreview);
		((Control)panel4).Dock = (DockStyle)5;
		((Control)panel4).Location = new Point(0, 39);
		((Control)panel4).Name = "panel4";
		((Control)panel4).Size = new Size(651, 210);
		((Control)panel4).TabIndex = 64;
		((Control)imageHyperlinkPreview).AllowDrop = true;
		((Control)imageHyperlinkPreview).Anchor = (AnchorStyles)15;
		((Control)imageHyperlinkPreview).Controls.Add((Control)(object)progressHyperlinkPreview);
		((Control)imageHyperlinkPreview).Location = new Point(12, 12);
		((Control)imageHyperlinkPreview).Name = "imageHyperlinkPreview";
		((Control)imageHyperlinkPreview).Size = new Size(628, 185);
		((Control)imageHyperlinkPreview).TabIndex = 59;
		((Control)imageHyperlinkPreview).Paint += new PaintEventHandler(ImageHyperlinkPreview_Paint);
		((Control)progressHyperlinkPreview).Anchor = (AnchorStyles)14;
		((Control)progressHyperlinkPreview).Location = new Point(4, 172);
		((Control)progressHyperlinkPreview).Name = "progressHyperlinkPreview";
		((Control)progressHyperlinkPreview).Size = new Size(620, 10);
		progressHyperlinkPreview.Style = (ProgressBarStyle)2;
		((Control)progressHyperlinkPreview).TabIndex = 0;
		((Control)progressHyperlinkPreview).Visible = false;
		((Control)panel3).Controls.Add((Control)(object)labelHyperlink);
		((Control)panel3).Controls.Add((Control)(object)panelHyperlink);
		((Control)panel3).Dock = (DockStyle)1;
		((Control)panel3).Location = new Point(0, 0);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(651, 39);
		((Control)panel3).TabIndex = 63;
		((Control)labelHyperlink).Location = new Point(12, 12);
		((Control)labelHyperlink).Name = "labelHyperlink";
		((Control)labelHyperlink).Size = new Size(154, 26);
		((Control)labelHyperlink).TabIndex = 7;
		((Control)labelHyperlink).Text = "Hyperlink";
		((Label)labelHyperlink).TextAlign = (ContentAlignment)16;
		((Control)panelHyperlink).Anchor = (AnchorStyles)13;
		((Control)panelHyperlink).Controls.Add((Control)(object)textBoxHyperlink);
		((Control)panelHyperlink).Controls.Add((Control)(object)buttonHyperlink);
		((Control)panelHyperlink).Location = new Point(168, 12);
		((Control)panelHyperlink).Name = "panelHyperlink";
		((Control)panelHyperlink).Size = new Size(472, 26);
		((Control)panelHyperlink).TabIndex = 0;
		((Control)textBoxHyperlink).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxHyperlink).BorderStyle = (BorderStyle)0;
		((Control)textBoxHyperlink).Location = new Point(4, 6);
		((Control)textBoxHyperlink).Name = "textBoxHyperlink";
		((Control)textBoxHyperlink).Size = new Size(440, 13);
		((Control)textBoxHyperlink).TabIndex = 0;
		((Control)textBoxHyperlink).TextChanged += TextBoxHyperlink_TextChanged;
		((Control)textBoxHyperlink).Leave += TextBoxHyperlink_Leave;
		((Control)buttonHyperlink).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonHyperlink).Image = (Image)componentResourceManager.GetObject("buttonHyperlink.Image");
		((Control)buttonHyperlink).Location = new Point(447, 1);
		((Control)buttonHyperlink).Name = "buttonHyperlink";
		((Control)buttonHyperlink).Size = new Size(24, 24);
		((Control)buttonHyperlink).TabIndex = 1;
		((Control)buttonHyperlink).TabStop = false;
		((ButtonBase)buttonHyperlink).UseVisualStyleBackColor = true;
		((Control)buttonHyperlink).Click += ButtonHyperlink_Click;
		((Control)groupHyperlinkHistory).Controls.Add((Control)(object)listHyperlinkHistory);
		((Control)groupHyperlinkHistory).Dock = (DockStyle)2;
		groupHyperlinkHistory.HeaderHeight = 24;
		((Control)groupHyperlinkHistory).Location = new Point(0, 249);
		((Control)groupHyperlinkHistory).Name = "groupHyperlinkHistory";
		((Control)groupHyperlinkHistory).Size = new Size(651, 133);
		((Control)groupHyperlinkHistory).TabIndex = 62;
		((Control)groupHyperlinkHistory).TabStop = false;
		((Control)groupHyperlinkHistory).Text = "History";
		((Control)groupHyperlinkHistory).Visible = false;
		((Control)listHyperlinkHistory).Anchor = (AnchorStyles)15;
		listHyperlinkHistory.AutoScroll = true;
		((Control)listHyperlinkHistory).BackColor = Color.White;
		((Control)listHyperlinkHistory).Location = new Point(4, 27);
		((Control)listHyperlinkHistory).Name = "listHyperlinkHistory";
		((Control)listHyperlinkHistory).Size = new Size(643, 101);
		((Control)listHyperlinkHistory).TabIndex = 59;
		((Control)pageFile).Controls.Add((Control)(object)panel1);
		((Control)pageFile).Controls.Add((Control)(object)panel2);
		((Control)pageFile).Controls.Add((Control)(object)groupFileHistory);
		pageFile.Dock = (DockStyle)5;
		pageFile.Image = (Image)componentResourceManager.GetObject("pageFile.Image");
		pageFile.InvisibleVisible = false;
		((Control)pageFile).Location = new Point(0, 0);
		((Control)pageFile).Name = "pageFile";
		((Control)pageFile).Padding = new Padding(0, 0, 1, 0);
		((Control)pageFile).Size = new Size(652, 382);
		((Control)pageFile).TabIndex = 4;
		((Control)pageFile).Text = "File";
		((Control)pageFile).Visible = false;
		((Control)panel1).Controls.Add((Control)(object)imageFilePreview);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(0, 39);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(651, 210);
		((Control)panel1).TabIndex = 62;
		((Control)imageFilePreview).AllowDrop = true;
		((Control)imageFilePreview).Anchor = (AnchorStyles)15;
		((Control)imageFilePreview).Location = new Point(12, 12);
		((Control)imageFilePreview).Name = "imageFilePreview";
		((Control)imageFilePreview).Size = new Size(628, 186);
		((Control)imageFilePreview).TabIndex = 1;
		((Control)imageFilePreview).DragDrop += new DragEventHandler(Panel_DragDrop);
		((Control)imageFilePreview).DragOver += new DragEventHandler(Panel_DragOver);
		((Control)imageFilePreview).Paint += new PaintEventHandler(ImageFilePreview_Paint);
		((Control)panel2).Controls.Add((Control)(object)labelFile);
		((Control)panel2).Controls.Add((Control)(object)panelBoxFile);
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(0, 0);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(651, 39);
		((Control)panel2).TabIndex = 63;
		((Control)labelFile).Location = new Point(12, 12);
		((Control)labelFile).Name = "labelFile";
		((Control)labelFile).Size = new Size(150, 26);
		((Control)labelFile).TabIndex = 5;
		((Control)labelFile).Text = "File Path";
		((Label)labelFile).TextAlign = (ContentAlignment)16;
		((Control)panelBoxFile).Anchor = (AnchorStyles)13;
		((Control)panelBoxFile).Controls.Add((Control)(object)textBoxFile);
		((Control)panelBoxFile).Controls.Add((Control)(object)buttonFile);
		((Control)panelBoxFile).Location = new Point(168, 12);
		((Control)panelBoxFile).Name = "panelBoxFile";
		((Control)panelBoxFile).Size = new Size(472, 26);
		((Control)panelBoxFile).TabIndex = 0;
		((Control)textBoxFile).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxFile).BorderStyle = (BorderStyle)0;
		((Control)textBoxFile).Location = new Point(4, 6);
		((Control)textBoxFile).Name = "textBoxFile";
		((Control)textBoxFile).Size = new Size(440, 13);
		((Control)textBoxFile).TabIndex = 0;
		((Control)textBoxFile).Leave += TextBoxFile_Leave;
		((Control)buttonFile).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonFile).Image = (Image)componentResourceManager.GetObject("buttonFile.Image");
		((Control)buttonFile).Location = new Point(447, 1);
		((Control)buttonFile).Name = "buttonFile";
		((Control)buttonFile).Size = new Size(24, 24);
		((Control)buttonFile).TabIndex = 1;
		((Control)buttonFile).TabStop = false;
		((ButtonBase)buttonFile).UseVisualStyleBackColor = true;
		((Control)buttonFile).Click += ButtonFile_Click;
		((Control)groupFileHistory).Controls.Add((Control)(object)listFileHistory);
		((Control)groupFileHistory).Dock = (DockStyle)2;
		groupFileHistory.HeaderHeight = 24;
		((Control)groupFileHistory).Location = new Point(0, 249);
		((Control)groupFileHistory).Name = "groupFileHistory";
		((Control)groupFileHistory).Size = new Size(651, 133);
		((Control)groupFileHistory).TabIndex = 61;
		((Control)groupFileHistory).TabStop = false;
		((Control)groupFileHistory).Text = "History";
		((Control)groupFileHistory).Visible = false;
		((Control)listFileHistory).Anchor = (AnchorStyles)15;
		listFileHistory.AutoScroll = true;
		((Control)listFileHistory).BackColor = Color.White;
		((Control)listFileHistory).Location = new Point(4, 27);
		((Control)listFileHistory).Name = "listFileHistory";
		((Control)listFileHistory).Size = new Size(643, 101);
		((Control)listFileHistory).TabIndex = 59;
		((Control)pageIcon).Controls.Add((Control)(object)panel7);
		((Control)pageIcon).Controls.Add((Control)(object)panelIcon);
		pageIcon.Dock = (DockStyle)5;
		pageIcon.Image = (Image)componentResourceManager.GetObject("pageIcon.Image");
		pageIcon.InvisibleVisible = false;
		((Control)pageIcon).Location = new Point(0, 0);
		((Control)pageIcon).Name = "pageIcon";
		((Control)pageIcon).Padding = new Padding(0, 0, 1, 0);
		((Control)pageIcon).Size = new Size(652, 382);
		((Control)pageIcon).TabIndex = 5;
		((Control)pageIcon).Text = "Icon";
		((Control)pageIcon).Visible = false;
		((Control)panel7).Controls.Add((Control)(object)panelSampleIcon);
		((Control)panel7).Dock = (DockStyle)5;
		((Control)panel7).Location = new Point(0, 39);
		((Control)panel7).Name = "panel7";
		((Control)panel7).Size = new Size(651, 343);
		((Control)panel7).TabIndex = 67;
		((Control)panelSampleIcon).AllowDrop = true;
		((Control)panelSampleIcon).Anchor = (AnchorStyles)15;
		((Control)panelSampleIcon).Location = new Point(12, 12);
		((Control)panelSampleIcon).Name = "panelSampleIcon";
		((Control)panelSampleIcon).Size = new Size(628, 319);
		((Control)panelSampleIcon).TabIndex = 1;
		((Control)panelSampleIcon).Paint += new PaintEventHandler(PanelSampleIcon_Paint);
		((Control)panelIcon).Controls.Add((Control)(object)colorBoxIconColor);
		((Control)panelIcon).Controls.Add((Control)(object)panelBoxIcon);
		((Control)panelIcon).Controls.Add((Control)(object)labelIcon);
		((Control)panelIcon).Dock = (DockStyle)1;
		((Control)panelIcon).Location = new Point(0, 0);
		((Control)panelIcon).Margin = new Padding(2);
		((Control)panelIcon).Name = "panelIcon";
		((Control)panelIcon).Size = new Size(651, 39);
		((Control)panelIcon).TabIndex = 66;
		((Control)colorBoxIconColor).AccessibleRole = (AccessibleRole)43;
		colorBoxIconColor.AutoExpandOnClick = true;
		colorBoxIconColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxIconColor.EnableMarkup = false;
		((Control)colorBoxIconColor).Location = new Point(270, 12);
		colorBoxIconColor.LockClosePopup = false;
		((Control)colorBoxIconColor).Margin = new Padding(2);
		((Control)colorBoxIconColor).Name = "colorBoxIconColor";
		((Control)colorBoxIconColor).Size = new Size(90, 25);
		((Control)colorBoxIconColor).TabIndex = 18;
		colorBoxIconColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxIconColor.SelectedColorChanged += ColorBoxIconColor_SelectedColorChanged;
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIconRemove);
		((Control)panelBoxIcon).Controls.Add((Control)(object)buttonIcon);
		((Control)panelBoxIcon).Location = new Point(168, 12);
		((Control)panelBoxIcon).Margin = new Padding(1);
		((Control)panelBoxIcon).Name = "panelBoxIcon";
		((Control)panelBoxIcon).Size = new Size(90, 25);
		((Control)panelBoxIcon).TabIndex = 17;
		((Control)buttonIconRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonIconRemove).Image = (Image)componentResourceManager.GetObject("buttonIconRemove.Image");
		((Control)buttonIconRemove).Location = new Point(43, 1);
		((Control)buttonIconRemove).Margin = new Padding(1);
		((Control)buttonIconRemove).Name = "buttonIconRemove";
		((Control)buttonIconRemove).Size = new Size(24, 23);
		((Control)buttonIconRemove).TabIndex = 2;
		((Control)buttonIconRemove).TabStop = false;
		((ButtonBase)buttonIconRemove).UseVisualStyleBackColor = true;
		((Control)buttonIconRemove).Click += ButtonIconRemove_Click;
		((Control)buttonIcon).Anchor = (AnchorStyles)9;
		((Control)buttonIcon).Location = new Point(66, 1);
		((Control)buttonIcon).Margin = new Padding(1);
		((Control)buttonIcon).Name = "buttonIcon";
		((Control)buttonIcon).Size = new Size(23, 23);
		((Control)buttonIcon).TabIndex = 1;
		((Control)buttonIcon).TabStop = false;
		((ButtonBase)buttonIcon).UseVisualStyleBackColor = true;
		((Control)buttonIcon).Click += ButtonIcon_Click;
		((Control)labelIcon).Location = new Point(12, 12);
		((Control)labelIcon).Margin = new Padding(1, 0, 1, 0);
		((Control)labelIcon).Name = "labelIcon";
		((Control)labelIcon).Size = new Size(150, 26);
		((Control)labelIcon).TabIndex = 20;
		((Control)labelIcon).Text = "Icon";
		((Label)labelIcon).TextAlign = (ContentAlignment)16;
		((Control)footer).Controls.Add((Control)(object)buttonOk);
		((Control)footer).Controls.Add((Control)(object)buttonCancel);
		((Control)footer).Dock = (DockStyle)2;
		footer.LinePosition = StiLinePosition.Top;
		footer.LineStyle = StiLineStyle.Dot;
		((Control)footer).Location = new Point(0, 382);
		((Control)footer).Name = "footer";
		((Control)footer).Size = new Size(771, 46);
		((Control)footer).TabIndex = 61;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(771, 428);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Controls.Add((Control)(object)footer);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(635, 420);
		((Control)this).Name = "StiImageEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Image Editor";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((Control)this).SizeChanged += This_SizeChanged;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)contextMenuHistory).ResumeLayout(false);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageImage).ResumeLayout(false);
		((Control)panel5).ResumeLayout(false);
		((Control)panelImageContainer).ResumeLayout(false);
		((Control)panelImage).ResumeLayout(false);
		((Control)panelImage).PerformLayout();
		((Control)groupGallery).ResumeLayout(false);
		((Control)pageDataColumn).ResumeLayout(false);
		((Control)panel6).ResumeLayout(false);
		((Control)groupDataColumnGallery).ResumeLayout(false);
		((Control)panelProgressDataColumn).ResumeLayout(false);
		((Control)pageExpression).ResumeLayout(false);
		((Control)panelExpression).ResumeLayout(false);
		((Control)panelExpression).PerformLayout();
		((Control)pageHyperlink).ResumeLayout(false);
		((Control)panel4).ResumeLayout(false);
		((Control)imageHyperlinkPreview).ResumeLayout(false);
		((Control)panel3).ResumeLayout(false);
		((Control)panelHyperlink).ResumeLayout(false);
		((Control)panelHyperlink).PerformLayout();
		((Control)groupHyperlinkHistory).ResumeLayout(false);
		((Control)pageFile).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)panelBoxFile).ResumeLayout(false);
		((Control)panelBoxFile).PerformLayout();
		((Control)groupFileHistory).ResumeLayout(false);
		((Control)pageIcon).ResumeLayout(false);
		((Control)panel7).ResumeLayout(false);
		((Control)panelIcon).ResumeLayout(false);
		((Control)panelBoxIcon).ResumeLayout(false);
		((Control)footer).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
