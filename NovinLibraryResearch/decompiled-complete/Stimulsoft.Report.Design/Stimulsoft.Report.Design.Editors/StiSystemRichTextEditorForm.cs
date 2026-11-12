using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Editors;

public class StiSystemRichTextEditorForm : StiFormUX
{
	private StiReport report;

	private StiRichText richText;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private List<string> fileHistoryList;

	private List<string> hyperlinkHistoryList;

	private bool isVariableGalleryActivated;

	private bool isResourceGalleryActivated;

	private bool isColumnGalleryActivated;

	private string dataColumn;

	private StiRichTextEditorStartTab? startTab;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageExpression;

	private StiCloudTabulatorPage pageDataColumn;

	private StiCloudTabulatorPage pageFile;

	private StiCloudTabulatorPage pageHyperlink;

	private StiTreeViewUX treeViewDataColumn;

	private StiRadioButtonUX radioButtonPage;

	private StiRadioButtonUX radioButtonReport;

	private StiRadioButtonUX radioButtonColumn;

	private StiRichTextEditorControl richTextEditorControl;

	private StiButtonPanelUX stiGroupLine1;

	private StiPanelUX panel2;

	private StiLabelUX labelFile;

	private StiPanelBox panelBoxFile;

	private HintTextBox textBoxFile;

	private StiButtonUX buttonFile;

	private StiOffice2013GroupBox groupFileHistory;

	private ScrollableControl listFileHistory;

	private ContextMenuStrip contextMenuHistory;

	private ToolStripMenuItem deleteFileHistoryItem;

	private ToolStripMenuItem deleteAllFileHistoryItem;

	private ToolTip toolTip;

	private ImageList images;

	private StiRichTextBox textFilePreview;

	private StiPanelUX panelFilePreview;

	private StiPanelUX panel4;

	private StiPanelUX panel3;

	private StiLabelUX labelHyperlink;

	private StiPanelBox panelHyperlink;

	private HintTextBox textBoxHyperlink;

	private StiButtonUX buttonHyperlink;

	private StiOffice2013GroupBox groupHyperlinkHistory;

	private ScrollableControl listHyperlinkHistory;

	private Timer timerHyperlinkPreview;

	private StiRichTextBox textHyperlinkPreview;

	private ProgressBar progressHyperlinkPreview;

	private StiPanelUX panelHyperlinkPreview;

	private StiPanelUX panel1;

	private StiOffice2013GroupBox groupDataColumnGallery;

	private ScrollableControl listDataColumnGallery;

	private StiPanelUX panelProgressDataColumn;

	private ProgressBar progressDataColumn;

	private StiOffice2013GroupBox groupGallery;

	private ScrollableControl listGallery;

	private Label labelSpecification;

	private StiButtonUX buttonRemove;

	private Timer timerRichTextFocus;

	private StiPanelUX panel5;

	private StiPanelUX panelTreeViewDataColumnContainer;

	public bool IsVariableFromGallery
	{
		get
		{
			string name = GetVariableNameFromHyperlink();
			return GetVariables().FirstOrDefault((StiVariable v) => v.Name != null && v.Name == name) != null;
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
			((Control)buttonRemove).Visible = value != null;
			((Control)richTextEditorControl.Toolbar).Enabled = value == null;
			((TextBoxBase)richTextEditorControl.rtbCodeEditor).ReadOnly = value != null;
			((Control)richTextEditorControl.rtbCodeEditor).BackColor = Color.White;
		}
	}

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(60.0);

	public StiSystemRichTextEditorForm(StiReport report, StiRichText richText, StiRichTextEditorStartTab? startTab = null)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			DoTheming();
			DoScaling();
			Localize();
			((Form)this).ShowIcon = false;
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			this.richText = richText;
			this.report = report;
			this.startTab = startTab;
			richTextEditorControl.Report = report;
			richTextEditorControl.TextValue = StiRichText.UnpackRtf(XmlConvert.DecodeName(richText.Text.Value));
			StiSettings.Load();
			fileHistoryList = StringToList(StiSettings.GetStr(((Control)this).Name, "RtfFileHistory", null));
			hyperlinkHistoryList = StringToList(StiSettings.GetStr(((Control)this).Name, "RtfHyperlinkHistory", null));
			fileHistoryList = CheckPathFiles(fileHistoryList);
			hyperlinkHistoryList = CheckPathFiles(hyperlinkHistoryList);
			StiFormSettings.Load((Form)(object)this);
			((Control)buttonRemove).Left = ((Control)richTextEditorControl).Width - SystemInformation.VerticalScrollBarWidth - 2 - ((Control)buttonRemove).Width;
			((RichTextBox)richTextEditorControl.rtbCodeEditor).ScrollBars = (RichTextBoxScrollBars)18;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiRichText");
		((Control)pageExpression).Text = StiLocalization.Get("PropertyMain", "Text");
		((Control)pageDataColumn).Text = StiLocalization.Get("PropertyMain", "DataColumn");
		((Control)pageFile).Text = StiLocalization.Get("PropertyMain", "File");
		((Control)pageHyperlink).Text = StiLocalization.Get("PropertyMain", "Hyperlink");
		((Control)groupGallery).Text = StiLocalization.Get("Report", "Gallery");
		((Control)groupDataColumnGallery).Text = StiLocalization.Get("Report", "Gallery");
		((Control)groupFileHistory).Text = StiLocalization.Get("Report", "History");
		((Control)groupHyperlinkHistory).Text = StiLocalization.Get("Report", "History");
		((ToolStripItem)deleteFileHistoryItem).Text = StiLocalization.Get("Buttons", "Delete");
		((ToolStripItem)deleteAllFileHistoryItem).Text = StiLocalization.Get("Cloud", "ButtonDeleteAll");
		((Control)labelHyperlink).Text = Loc.GetCleaned("PropertyMain", "Hyperlink");
		((Control)labelFile).Text = Loc.GetCleaned("Cloud", "labelFileName");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		textBoxHyperlink.Hint = "http://site.com/text.rtf";
		textBoxFile.Hint = "c:\\folder\\text.rtf";
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		ResetGallery(sender);
		Specification = null;
		isResourceGalleryActivated = false;
		isVariableGalleryActivated = false;
		isColumnGalleryActivated = false;
		richTextEditorControl.TextValue = string.Empty;
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (TabControl.SelectedTab == pageFile)
		{
			((Control)textBoxFile).Focus();
		}
		else if (TabControl.SelectedTab == pageHyperlink)
		{
			((Control)textBoxHyperlink).Focus();
		}
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

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		ExpandTreeNode(e.Node);
	}

	private void TimerRichTextFocus_Tick(object sender, EventArgs e)
	{
		timerRichTextFocus.Stop();
		((Control)richTextEditorControl.rtbCodeEditor).Focus();
	}

	private void PanelFilePreview_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelFilePreview, e.Graphics, drawDragDrop: true, drawBorder: true, isImage: false);
	}

	private void TextBoxFile_Leave(object sender, EventArgs e)
	{
		TryLoadRtfFromFile();
	}

	private void ButtonEditFilePathOnButton_Click(object sender, EventArgs eventArgs)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "RtfFiles") + "|" + StiLocalization.Get("FileFilters", "TxtFiles");
			((FileDialog)val).RestoreDirectory = true;
			((FileDialog)val).FileName = ((Control)textBoxFile).Text;
			if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
			{
				((Control)textBoxFile).Text = ((FileDialog)val).FileName;
				TryLoadRtfFromFile();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PanelHyperlinkPreview_Paint(object sender, PaintEventArgs e)
	{
		StiImageDialogPainter.Paint((Control)(object)panelHyperlinkPreview, e.Graphics, drawDragDrop: false, drawBorder: true, isImage: false);
	}

	private void ButtonHyperlink_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxHyperlink).Text;
		text = StiExpressionPacker.PackExpression(text, report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxHyperlink).Text = StiExpressionPacker.UnPackExpression(text, report.Designer, useBraces: false);
			TryLoadRtfFromHyperlink();
		}
		((Control)textBoxHyperlink).Focus();
		((TextBoxBase)textBoxHyperlink).Select(0, 0);
	}

	private void TextBoxHyperlink_Leave(object sender, EventArgs e)
	{
		TryLoadRtfFromHyperlink();
	}

	private void TextBoxHyperlink_TextChanged(object sender, EventArgs e)
	{
		timerHyperlinkPreview.Stop();
		timerHyperlinkPreview.Start();
	}

	private void TimerHyperlinkPreview_Tick(object sender, EventArgs e)
	{
		timerHyperlinkPreview.Stop();
		TryLoadRtfFromHyperlink();
	}

	private void This_OkClicked(object sender, EventArgs e)
	{
		richText.Report.Designer.UndoRedoSave("Text Editor");
		richText.Report.IsModified = true;
		richText.ResetSourceProperties();
		string value = StiExpressionPacker.UnPackExpression(((Control)textBoxHyperlink).Text, report.Designer, useBraces: true);
		if (TabControl.SelectedTab == pageExpression && isResourceGalleryActivated)
		{
			richText.DataUrl.Value = value;
		}
		else if (TabControl.SelectedTab == pageExpression && isVariableGalleryActivated)
		{
			richText.DataUrl.Value = value;
		}
		else if (TabControl.SelectedTab == pageExpression && isColumnGalleryActivated)
		{
			richText.DataColumn = dataColumn;
		}
		else if (TabControl.SelectedTab == pageExpression)
		{
			richText.Text.Value = XmlConvert.EncodeName(StiRichText.PackRtf(richTextEditorControl.TextValue));
		}
		else if (TabControl.SelectedTab == pageDataColumn)
		{
			if (((TreeView)treeViewDataColumn).SelectedNode != null)
			{
				if (((TreeView)treeViewDataColumn).SelectedNode.Text == StiLocalization.Get("Report", "No"))
				{
					richText.DataColumn = string.Empty;
				}
				if (((TreeView)treeViewDataColumn).SelectedNode.Tag is StiDataColumn)
				{
					richText.DataColumn = StiDataBuilder.GetColumnPathFromNode(((TreeView)treeViewDataColumn).SelectedNode, useRelationName: true);
				}
			}
		}
		else if (TabControl.SelectedTab == pageFile)
		{
			richText.DataUrl.Value = StiHyperlinkProcessor.CreateFileName(((Control)textBoxFile).Text);
			AddToFileHistory(((Control)textBoxFile).Text);
		}
		else if (TabControl.SelectedTab == pageHyperlink)
		{
			richText.DataUrl.Value = StiExpressionPacker.UnPackExpression(((Control)textBoxHyperlink).Text, richText.Report.Designer, useBraces: true);
			AddToHyperlinkHistory(((Control)textBoxHyperlink).Text);
		}
		richText.ResetImage();
		if (richText.Report.Designer.DesignerControl is StiDesignerControl)
		{
			((StiDesignerControl)richText.Report.Designer.DesignerControl).Builder.Redraw();
		}
		((Form)this).Close();
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
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_rich_text_output_rich_text_editor.htm");
	}

	private void This_Load(object sender, EventArgs e)
	{
		BuildDataColumn();
		BuildComponentOptions();
		BuildGallery();
		BuildDataColumnGallery();
		BuildFileHistory();
		BuildHyperlinkHistory();
		((TextBoxBase)textFilePreview).ReadOnly = true;
		((Control)textFilePreview).BackColor = Color.White;
	}

	private void This_Closed(object sender, FormClosedEventArgs e)
	{
		SaveSettings();
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)listGallery).BackColor = StiUX.Background;
			((Control)labelSpecification).ForeColor = StiUX.ButtonForeground;
			((Control)labelSpecification).BackColor = StiUX.Accent;
		}
	}

	private void DoScaling()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		double step = StiScale.Step;
		if (step >= 1.5)
		{
			string bigStepName = StiScale.BigStepName;
			images = new ImageList();
			images.TransparentColor = Color.Transparent;
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceRtf" + bigStepName + ".png");
			images.ImageSize = ((Image)image).Size;
			images.Images.Add("Rtf", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceRtf" + bigStepName + ".png"));
			images.Images.Add("Txt", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.ResourceTxt" + bigStepName + ".png"));
		}
		pageDataColumn.Image = StiReportDesignImages.Tasks.RichTextDataColumn(StiImageSize.Double);
		pageExpression.Image = StiReportDesignImages.Tasks.RichTextExpression(StiImageSize.Double);
		pageFile.Image = StiReportDesignImages.Tasks.RichTextFile(StiImageSize.Double);
		pageHyperlink.Image = StiReportDesignImages.Tasks.RichTextHyperlink(StiImageSize.Double);
		richTextEditorControl.DoScaling();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonHyperlink).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonFile).Image = StiReportDesignImages.Actions.OpenFile();
	}

	private void BuildComponentOptions()
	{
		((Control)textBoxHyperlink).Text = StiExpressionPacker.PackExpression(richText.DataUrl.Value, richText.Report.Designer, useBraces: true);
		if (startTab.HasValue)
		{
			if (startTab == StiRichTextEditorStartTab.Text)
			{
				TabControl.SelectedTab = pageExpression;
			}
			else if (startTab == StiRichTextEditorStartTab.DataColumn)
			{
				TabControl.SelectedTab = pageDataColumn;
			}
			else if (startTab == StiRichTextEditorStartTab.DataUrl && StiHyperlinkProcessor.GetFileNameFromHyperlink(richText.DataUrl.Value) != null)
			{
				TabControl.SelectedTab = pageFile;
			}
			else if (startTab == StiRichTextEditorStartTab.DataUrl)
			{
				TabControl.SelectedTab = pageHyperlink;
			}
		}
		if (IsVariableFromGallery)
		{
			if (!startTab.HasValue)
			{
				TabControl.SelectedTab = pageExpression;
			}
			Specification = GetVariableNameFromHyperlink();
			richTextEditorControl.TextValue = GetVariableString();
			isVariableGalleryActivated = true;
		}
		else if (IsResourceFromGallery)
		{
			if (!startTab.HasValue)
			{
				TabControl.SelectedTab = pageExpression;
			}
			Specification = GetResourceNameFromHyperlink();
			richTextEditorControl.TextValue = GetResourceString();
			isResourceGalleryActivated = true;
		}
		else
		{
			string fileNameFromHyperlink = StiHyperlinkProcessor.GetFileNameFromHyperlink(richText.DataUrl.Value);
			if (fileNameFromHyperlink != null)
			{
				((Control)textBoxFile).Text = fileNameFromHyperlink;
				if (!startTab.HasValue)
				{
					TabControl.SelectedTab = pageFile;
				}
				((Control)textBoxFile).Select();
				((TextBoxBase)textBoxFile).Select(0, 0);
			}
			else if (!string.IsNullOrWhiteSpace(richText.DataUrl.Value))
			{
				if (!startTab.HasValue)
				{
					TabControl.SelectedTab = pageHyperlink;
				}
				((Control)textBoxHyperlink).Select();
				((TextBoxBase)textBoxHyperlink).Select(0, 0);
			}
			else if (!string.IsNullOrEmpty(richText.DataColumn))
			{
				if (!startTab.HasValue)
				{
					TabControl.SelectedTab = pageDataColumn;
				}
				((Control)treeViewDataColumn).Select();
			}
			else
			{
				((Control)richTextEditorControl.rtbCodeEditor).Select();
				((TextBoxBase)richTextEditorControl.rtbCodeEditor).Select(0, 0);
			}
		}
		pageDataColumn.Invisible = !StiOptions.Designer.Editors.SystemRichTextEditor.ShowDataColumnTabPage;
		pageExpression.Invisible = !StiOptions.Designer.Editors.SystemRichTextEditor.ShowDataTabPage;
		pageHyperlink.Invisible = !StiOptions.Designer.Editors.SystemRichTextEditor.ShowURLTabPage;
		pageFile.Invisible = !StiOptions.Designer.Editors.SystemRichTextEditor.ShowFileTabPage;
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
		StiSettings.Set(((Control)this).Name, "RtfFileHistory", ListToString(fileHistoryList));
		StiSettings.Set(((Control)this).Name, "RtfHyperlinkHistory", ListToString(hyperlinkHistoryList));
		StiSettings.Save();
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
		return JsonConvert.SerializeObject(list, Stimulsoft.Base.Json.Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	private List<string> CheckPathFiles(List<string> list)
	{
		List<string> list2 = new List<string>();
		foreach (string item in list)
		{
			if (File.Exists(item))
			{
				list2.Add(item);
			}
		}
		return list2;
	}

	private List<string> StringToList(string str)
	{
		List<string> list = new List<string>();
		if (str != null)
		{
			JsonConvert.PopulateObject(str, list);
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

	private void BuildGallery()
	{
		List<StiVariable> variables = GetVariables();
		List<StiResource> resources = GetResources();
		((Control)groupGallery).Visible = variables.Any() || resources.Any();
		((Control)listGallery).Controls.Clear();
		Task.Factory.StartNew(delegate
		{
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Expected O, but got Unknown
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Expected O, but got Unknown
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Expected O, but got Unknown
			int num = ItemGalleryStep;
			int itemGalleryStep = ItemGalleryStep;
			string variableNameFromHyperlink = GetVariableNameFromHyperlink();
			string resourceNameFromHyperlink = GetResourceNameFromHyperlink();
			foreach (StiVariable variable in variables)
			{
				try
				{
					string text = ((variable.ValueObject is string && StiRtfHelper.IsRtfText(variable.ValueObject as string)) ? "Rtf" : "Txt");
					StiImageButton button3 = CreateGalleryButton(variable.Name, images.Images[text], variable, num, itemGalleryStep, ItemGallerySize);
					((CheckBox)button3).Checked = variableNameFromHyperlink == variable.Name;
					((CheckBox)button3).CheckedChanged += GalleryClick;
					((CheckBox)button3).DoubleClick += This_OkClicked;
					StiImageButton stiImageButton = button3;
					((Control)stiImageButton).Width = ((Control)stiImageButton).Width * 2;
					((Control)this).Invoke((Delegate)(MethodInvoker)delegate
					{
						if (!IsFormDestructed())
						{
							toolTip.SetToolTip((Control)(object)button3, string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "Variable"), variable.Name));
							((Control)listGallery).Controls.Add((Control)(object)button3);
						}
					});
					num += ((Control)button3).Width + ItemGalleryStep;
				}
				catch
				{
				}
			}
			foreach (StiResource resource in resources)
			{
				try
				{
					StiImageButton button2 = CreateGalleryButton(resource.Name, images.Images[resource.Type.ToString()], resource, num, itemGalleryStep, ItemGallerySize);
					((CheckBox)button2).Checked = resourceNameFromHyperlink == resource.Name;
					((CheckBox)button2).CheckedChanged += GalleryClick;
					((CheckBox)button2).DoubleClick += This_OkClicked;
					StiImageButton stiImageButton2 = button2;
					((Control)stiImageButton2).Width = ((Control)stiImageButton2).Width * 2;
					((Control)this).Invoke((Delegate)(MethodInvoker)delegate
					{
						if (!IsFormDestructed())
						{
							toolTip.SetToolTip((Control)(object)button2, string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "Resource"), resource.Name));
							((Control)listGallery).Controls.Add((Control)(object)button2);
						}
					});
					num += ((Control)button2).Width + ItemGalleryStep;
				}
				catch
				{
				}
			}
			int counter = 0;
			try
			{
				List<StiDataColumn> columns = GetColumns();
				List<StiDataSource> dataSources = columns.Select((StiDataColumn c) => c.DataSource).Distinct().ToList();
				if (StiOptions.Designer.Editors.AllowConnectToDataInGallery)
				{
					report.Dictionary.TryConnect(loadData: true, dataSources);
				}
				foreach (StiDataColumn item in columns)
				{
					try
					{
						if (IsRtfColumn(item))
						{
							string columnPath = item.GetColumnPath();
							StiImageButton button = CreateGalleryButton(columnPath, images.Images["Rtf"], item, num, itemGalleryStep, ItemGallerySize);
							((CheckBox)button).CheckedChanged += GalleryClick;
							((CheckBox)button).DoubleClick += This_OkClicked;
							StiImageButton stiImageButton3 = button;
							((Control)stiImageButton3).Width = ((Control)stiImageButton3).Width * 2;
							((Control)this).Invoke((Delegate)(MethodInvoker)delegate
							{
								if (!IsFormDestructed())
								{
									toolTip.SetToolTip((Control)(object)button, string.Format("{0}: {1}", StiLocalization.Get("PropertyMain", "DataColumn"), columnPath));
									((Control)listGallery).Controls.Add((Control)(object)button);
								}
							});
							num += ((Control)button).Width + ItemGalleryStep;
							int num2 = counter;
							counter = num2 + 1;
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
					report.Dictionary.TryDisconnect();
				}
			}
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				if (!IsFormDestructed())
				{
					ScrollToActiveButton(listGallery);
					((Control)groupGallery).Visible = variables.Any() || resources.Any() || counter > 0;
				}
			});
		});
	}

	private void GalleryClick(object sender, EventArgs e)
	{
		Specification = null;
		isResourceGalleryActivated = false;
		isVariableGalleryActivated = false;
		isColumnGalleryActivated = false;
		ResetGallery(sender);
		if (sender is StiImageButton stiImageButton && ((CheckBox)stiImageButton).Checked)
		{
			if (((Control)stiImageButton).Tag is StiResource stiResource)
			{
				((Control)textBoxHyperlink).Text = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
				Specification = stiResource.Name;
				isResourceGalleryActivated = true;
				richTextEditorControl.TextValue = GetResourceString();
			}
			if (((Control)stiImageButton).Tag is StiVariable stiVariable)
			{
				((Control)textBoxHyperlink).Text = StiHyperlinkProcessor.CreateVariableName(stiVariable.Name);
				Specification = stiVariable.Name;
				isVariableGalleryActivated = true;
				richTextEditorControl.TextValue = GetVariableString();
			}
			if (((Control)stiImageButton).Tag is StiDataColumn stiDataColumn)
			{
				dataColumn = stiDataColumn.GetColumnPath();
				SelectDataColumn(dataColumn);
				Specification = stiDataColumn.GetColumnPath();
				isColumnGalleryActivated = true;
				richTextEditorControl.TextValue = GetColumnString(stiDataColumn);
			}
		}
	}

	private void ResetGallery(object sender = null)
	{
		(from Control c in (IEnumerable)((Control)listGallery).Controls
			where c is StiImageButton && c != sender
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
	}

	private string GetVariableNameFromHyperlink()
	{
		return StiHyperlinkProcessor.GetVariableNameFromHyperlink(((Control)textBoxHyperlink).Text);
	}

	private string GetResourceNameFromHyperlink()
	{
		return StiHyperlinkProcessor.GetResourceNameFromHyperlink(((Control)textBoxHyperlink).Text);
	}

	private List<StiResource> GetResources()
	{
		return (from v in report.Dictionary.Resources.ToList()
			where (v.Type == StiResourceType.Rtf || v.Type == StiResourceType.Txt) && v.Content != null
			select v).ToList();
	}

	private List<StiVariable> GetVariables()
	{
		return (from v in report.Dictionary.Variables.ToList()
			where v.ValueObject is string
			select v).ToList();
	}

	private string GetVariableString()
	{
		return StiHyperlinkProcessor.GetString(report, ((Control)textBoxHyperlink).Text);
	}

	private string GetResourceString()
	{
		return StiHyperlinkProcessor.GetString(report, ((Control)textBoxHyperlink).Text);
	}

	private string GetColumnString(StiDataColumn column)
	{
		string columnPath = column.GetColumnPath();
		List<object> dataListFromDataColumn = StiDataColumn.GetDataListFromDataColumn(report.Dictionary, columnPath, 3);
		foreach (object item in dataListFromDataColumn)
		{
			if (item is string)
			{
				return item as string;
			}
			if (item is byte[] && StiRtfHelper.IsRtfBytes(item as byte[]))
			{
				return StiBytesToStringConverter.ConvertBytesToString(item as byte[]);
			}
		}
		return null;
	}

	private void BuildDataColumnGallery()
	{
		List<StiDataColumn> columns = GetColumns();
		List<StiDataSource> dataSources = columns.Select((StiDataColumn c) => c.DataSource).Distinct().ToList();
		if (StiOptions.Designer.Editors.AllowConnectToDataInGallery)
		{
			report.Dictionary.TryConnect(loadData: true, dataSources);
		}
		((Control)listDataColumnGallery).Controls.Clear();
		string currentColumn = GetCurrentDataColumn();
		Task.Factory.StartNew(delegate
		{
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Expected O, but got Unknown
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Expected O, but got Unknown
			try
			{
				int counter = 0;
				int num = ItemGalleryStep;
				int itemGalleryStep = ItemGalleryStep;
				SetProgressVisible((Control)(object)panelProgressDataColumn, visible: true);
				foreach (StiDataColumn item in columns)
				{
					if (IsRtfColumn(item))
					{
						string columnPath = item.GetColumnPath();
						StiImageButton button = CreateGalleryButton(columnPath, images.Images["Rtf"], columnPath, num, itemGalleryStep, ItemGallerySize);
						((CheckBox)button).Checked = columnPath == currentColumn;
						((CheckBox)button).CheckedChanged += delegate(object sender, EventArgs e)
						{
							DataColumnGalleryClick(sender);
						};
						((CheckBox)button).DoubleClick += This_OkClicked;
						StiImageButton stiImageButton = button;
						((Control)stiImageButton).Width = ((Control)stiImageButton).Width * 2;
						num += ((Control)button).Width + ItemGalleryStep;
						int num2 = counter;
						counter = num2 + 1;
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
						((Control)groupDataColumnGallery).Visible = counter > 0;
					}
				});
			}
			finally
			{
				if (StiOptions.Designer.Editors.AllowConnectToDataInGallery)
				{
					report.Dictionary.TryDisconnect();
				}
				SetProgressVisible((Control)(object)panelProgressDataColumn, visible: false);
			}
		});
	}

	private bool IsRtfColumn(StiDataColumn column)
	{
		string columnPath = column.GetColumnPath();
		List<object> dataListFromDataColumn = StiDataColumn.GetDataListFromDataColumn(report.Dictionary, columnPath, 3);
		foreach (object item in dataListFromDataColumn)
		{
			if (item != null)
			{
				if (item is byte[])
				{
					return StiRtfHelper.IsRtfBytes(item as byte[]);
				}
				if (item is string)
				{
					return StiRtfHelper.IsRtfText(item as string);
				}
			}
		}
		return false;
	}

	private List<StiDataColumn> GetColumns()
	{
		IEnumerable<StiDataColumn> source = report.Dictionary.DataSources.ToList().SelectMany((StiDataSource d) => d.Columns.ToList());
		return source.Where((StiDataColumn c) => c.Type == typeof(byte[]) || c.Type == typeof(string)).ToList();
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
		if (sender is StiImageButton stiImageButton && ((Control)stiImageButton).Tag is string columnPath)
		{
			SelectDataColumn(columnPath);
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
		dataBuilder.Build(((TreeView)treeViewDataColumn).Nodes, richText.Report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		bool flag = false;
		if (!string.IsNullOrEmpty(richText.DataColumn))
		{
			string[] strs = richText.DataColumn.Split(new char[1] { '.' });
			flag = SelectNode(strs, 0, ((TreeView)treeViewDataColumn).Nodes, (TreeView)(object)treeViewDataColumn);
		}
		if (!flag)
		{
			((TreeView)treeViewDataColumn).SelectedNode = ((TreeView)treeViewDataColumn).Nodes[0];
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
			string str = ((!(val.Tag is StiDataRelation)) ? val.Text : ((StiDataRelation)val.Tag).Name);
			str = StiNameValidator.CorrectName(str, report);
			if (!(str == text))
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

	private void ExpandTreeNode(TreeNode node)
	{
		if (node is TreeNodeEx node2 && node.Tag != null)
		{
			dataBuilder.ExpandedNode(node2, includeCalcColumn: true);
		}
	}

	private void BuildFileHistory()
	{
		((Control)groupFileHistory).Visible = fileHistoryList.Count > 0;
		((Control)listFileHistory).Controls.Clear();
		Task.Factory.StartNew(delegate
		{
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			int num = ItemGalleryStep;
			int itemGalleryStep = ItemGalleryStep;
			foreach (string file in fileHistoryList)
			{
				if (IsFormDestructed())
				{
					return;
				}
				string fileName = Path.GetFileName(file);
				string text = (IsRtfFile(file) ? "Rtf" : "Txt");
				StiImageButton button = CreateGalleryButton(fileName, images.Images[text], file, num, itemGalleryStep, ItemGallerySize);
				((CheckBox)button).Checked = ((Control)textBoxFile).Text == fileName;
				((Control)button).Click += FileHistoryClick;
				((CheckBox)button).DoubleClick += This_OkClicked;
				StiImageButton stiImageButton = button;
				((Control)stiImageButton).Width = ((Control)stiImageButton).Width * 2;
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

	private bool IsRtfFile(string fileName)
	{
		byte[] array = new byte[6];
		using (BinaryReader binaryReader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
		{
			binaryReader.Read(array, 0, 6);
		}
		return StiRtfHelper.IsRtfBytes(array);
	}

	private void FileHistoryClick(object sender, EventArgs e)
	{
		(from Control c in (IEnumerable)((Control)listFileHistory).Controls
			where c is StiImageButton
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton && ((Control)stiImageButton).Tag is string text)
		{
			((Control)textBoxFile).Text = text;
			TryLoadRtfFromFile();
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

	private void TryLoadRtfFromFile()
	{
		((Control)textFilePreview).Visible = false;
		try
		{
			if (!string.IsNullOrWhiteSpace(((Control)textBoxFile).Text) && File.Exists(((Control)textBoxFile).Text))
			{
				string text = File.ReadAllText(((Control)textBoxFile).Text);
				if (StiRtfHelper.IsRtfText(text))
				{
					((RichTextBox)textFilePreview).Rtf = text;
				}
				else
				{
					((Control)textFilePreview).Text = text;
				}
				((Control)textFilePreview).Visible = true;
			}
		}
		catch
		{
			((Control)textFilePreview).Visible = false;
		}
	}

	private void BuildHyperlinkHistory()
	{
		((Control)groupHyperlinkHistory).Visible = hyperlinkHistoryList.Count > 0;
		((Control)listHyperlinkHistory).Controls.Clear();
		Task.Factory.StartNew(delegate
		{
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected O, but got Unknown
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Expected O, but got Unknown
			int num = ItemGalleryStep;
			int itemGalleryStep = ItemGalleryStep;
			foreach (string hyperlink in hyperlinkHistoryList)
			{
				if (IsFormDestructed())
				{
					return;
				}
				string fileName = Path.GetFileName(hyperlink);
				StiImageButton button = CreateGalleryButton(fileName, images.Images["Rtf"], hyperlink, num, itemGalleryStep, ItemGallerySize);
				((CheckBox)button).Checked = ((Control)textBoxHyperlink).Text == fileName;
				((Control)button).Click += HyperlinkHistoryClick;
				((CheckBox)button).DoubleClick += This_OkClicked;
				StiImageButton stiImageButton = button;
				((Control)stiImageButton).Width = ((Control)stiImageButton).Width * 2;
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

	private void TryLoadRtfFromHyperlink()
	{
		if (string.IsNullOrWhiteSpace(((Control)textBoxHyperlink).Text))
		{
			return;
		}
		Task.Factory.StartNew(delegate
		{
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			try
			{
				Thread.Sleep(1000);
				if (TabControl.SelectedTab == pageHyperlink)
				{
					SetProgressVisible((Control)(object)progressHyperlinkPreview, visible: true);
					if (string.IsNullOrWhiteSpace(((Control)textBoxHyperlink).Text))
					{
						((Control)textHyperlinkPreview).Visible = false;
					}
					else
					{
						string text = StiHyperlinkProcessor.GetString(report, ((Control)textBoxHyperlink).Text);
						if (string.IsNullOrWhiteSpace(text))
						{
							((Control)textHyperlinkPreview).Visible = false;
						}
						else if (!IsFormDestructed())
						{
							((Control)this).Invoke((Delegate)(MethodInvoker)delegate
							{
								if (StiRtfHelper.IsRtfText(text))
								{
									((RichTextBox)textHyperlinkPreview).Rtf = text;
								}
								else
								{
									((Control)textHyperlinkPreview).Text = text;
								}
								((Control)textHyperlinkPreview).Visible = true;
							});
						}
					}
				}
			}
			catch
			{
				((Control)textHyperlinkPreview).Visible = false;
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

	private void HyperlinkHistoryClick(object sender, EventArgs e)
	{
		(from Control c in (IEnumerable)((Control)listHyperlinkHistory).Controls
			where c is StiImageButton
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton && ((Control)stiImageButton).Tag is string text)
		{
			((Control)textBoxHyperlink).Text = text;
			TryLoadRtfFromHyperlink();
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
			if (StiResourceTypeHelper.IsTextType(ext))
			{
				if (sender == panelFilePreview)
				{
					((Control)textBoxFile).Text = item;
					TryLoadRtfFromFile();
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
				if (StiResourceTypeHelper.IsTextType(ext))
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
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Expected O, but got Unknown
		//IL_098d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Expected O, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d65: Expected O, but got Unknown
		//IL_0d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0f: Expected O, but got Unknown
		//IL_1268: Unknown result type (might be due to invalid IL or missing references)
		//IL_1272: Expected O, but got Unknown
		//IL_12a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_133f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1401: Expected O, but got Unknown
		//IL_140e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1418: Expected O, but got Unknown
		//IL_1425: Unknown result type (might be due to invalid IL or missing references)
		//IL_142f: Expected O, but got Unknown
		//IL_19c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cb: Expected O, but got Unknown
		//IL_1a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2c: Expected O, but got Unknown
		//IL_1db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc1: Expected O, but got Unknown
		//IL_1f02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0c: Expected O, but got Unknown
		//IL_1f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f30: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSystemRichTextEditorForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		TabControl = new StiCloudTabulator();
		pageExpression = new StiCloudTabulatorPage();
		panel5 = new StiPanelUX();
		labelSpecification = new Label();
		buttonRemove = new StiButtonUX();
		groupGallery = new StiOffice2013GroupBox();
		listGallery = new ScrollableControl();
		pageDataColumn = new StiCloudTabulatorPage();
		panelTreeViewDataColumnContainer = new StiPanelUX();
		treeViewDataColumn = new StiTreeViewUX();
		groupDataColumnGallery = new StiOffice2013GroupBox();
		listDataColumnGallery = new ScrollableControl();
		panelProgressDataColumn = new StiPanelUX();
		progressDataColumn = new ProgressBar();
		pageHyperlink = new StiCloudTabulatorPage();
		panel4 = new StiPanelUX();
		panelHyperlinkPreview = new StiPanelUX();
		textHyperlinkPreview = new StiRichTextBox();
		progressHyperlinkPreview = new ProgressBar();
		panel3 = new StiPanelUX();
		labelHyperlink = new StiLabelUX();
		groupHyperlinkHistory = new StiOffice2013GroupBox();
		listHyperlinkHistory = new ScrollableControl();
		pageFile = new StiCloudTabulatorPage();
		panel1 = new StiPanelUX();
		panelFilePreview = new StiPanelUX();
		textFilePreview = new StiRichTextBox();
		groupFileHistory = new StiOffice2013GroupBox();
		listFileHistory = new ScrollableControl();
		panel2 = new StiPanelUX();
		labelFile = new StiLabelUX();
		radioButtonPage = new StiRadioButtonUX();
		radioButtonReport = new StiRadioButtonUX();
		radioButtonColumn = new StiRadioButtonUX();
		stiGroupLine1 = new StiButtonPanelUX();
		contextMenuHistory = new ContextMenuStrip(components);
		deleteFileHistoryItem = new ToolStripMenuItem();
		deleteAllFileHistoryItem = new ToolStripMenuItem();
		toolTip = new ToolTip(components);
		images = new ImageList(components);
		timerHyperlinkPreview = new Timer(components);
		timerRichTextFocus = new Timer(components);
		richTextEditorControl = new StiRichTextEditorControl();
		panelHyperlink = new StiPanelBox();
		textBoxHyperlink = new HintTextBox();
		buttonHyperlink = new StiButtonUX();
		panelBoxFile = new StiPanelBox();
		textBoxFile = new HintTextBox();
		buttonFile = new StiButtonUX();
		((Control)TabControl).SuspendLayout();
		((Control)pageExpression).SuspendLayout();
		((Control)panel5).SuspendLayout();
		((Control)groupGallery).SuspendLayout();
		((Control)pageDataColumn).SuspendLayout();
		((Control)panelTreeViewDataColumnContainer).SuspendLayout();
		((Control)groupDataColumnGallery).SuspendLayout();
		((Control)panelProgressDataColumn).SuspendLayout();
		((Control)pageHyperlink).SuspendLayout();
		((Control)panel4).SuspendLayout();
		((Control)panelHyperlinkPreview).SuspendLayout();
		((Control)panel3).SuspendLayout();
		((Control)groupHyperlinkHistory).SuspendLayout();
		((Control)pageFile).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)panelFilePreview).SuspendLayout();
		((Control)groupFileHistory).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)contextMenuHistory).SuspendLayout();
		((Control)panelHyperlink).SuspendLayout();
		((Control)panelBoxFile).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(805, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClicked;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(886, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)TabControl).Controls.Add((Control)(object)pageExpression);
		((Control)TabControl).Controls.Add((Control)(object)pageDataColumn);
		((Control)TabControl).Controls.Add((Control)(object)pageHyperlink);
		((Control)TabControl).Controls.Add((Control)(object)pageFile);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(973, 492);
		((Control)TabControl).TabIndex = 0;
		TabControl.TitleBackColor = SystemColors.ControlDark;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageExpression).Controls.Add((Control)(object)panel5);
		((Control)pageExpression).Controls.Add((Control)(object)groupGallery);
		pageExpression.Dock = (DockStyle)5;
		pageExpression.Image = (Image)componentResourceManager.GetObject("pageExpression.Image");
		pageExpression.InvisibleVisible = false;
		((Control)pageExpression).Location = new Point(0, 0);
		((Control)pageExpression).Name = "pageExpression";
		((Control)pageExpression).Padding = new Padding(4);
		((Control)pageExpression).Size = new Size(854, 492);
		((Control)pageExpression).TabIndex = 2;
		((Control)pageExpression).Text = "Expression";
		((Control)pageExpression).Visible = false;
		((Control)panel5).Controls.Add((Control)(object)labelSpecification);
		((Control)panel5).Controls.Add((Control)(object)buttonRemove);
		((Control)panel5).Controls.Add((Control)(object)richTextEditorControl);
		((Control)panel5).Dock = (DockStyle)5;
		((Control)panel5).Location = new Point(4, 4);
		((Control)panel5).Name = "panel5";
		((Control)panel5).Size = new Size(846, 372);
		((Control)panel5).TabIndex = 65;
		((Control)labelSpecification).Anchor = (AnchorStyles)6;
		((Control)labelSpecification).AutoSize = true;
		((Control)labelSpecification).BackColor = SystemColors.ControlDark;
		((Control)labelSpecification).ForeColor = SystemColors.ControlLightLight;
		((Control)labelSpecification).Location = new Point(14, 347);
		((Control)labelSpecification).Name = "labelSpecification";
		((Control)labelSpecification).Padding = new Padding(3);
		((Control)labelSpecification).Size = new Size(74, 19);
		((Control)labelSpecification).TabIndex = 62;
		((Control)labelSpecification).Text = "Specification";
		((Control)labelSpecification).Visible = false;
		((Control)buttonRemove).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(801, 54);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(24, 24);
		((Control)buttonRemove).TabIndex = 64;
		((Control)buttonRemove).TabStop = false;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = true;
		((Control)buttonRemove).Visible = false;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)groupGallery).Controls.Add((Control)(object)listGallery);
		((Control)groupGallery).Dock = (DockStyle)2;
		groupGallery.HeaderHeight = 24;
		((Control)groupGallery).Location = new Point(4, 376);
		((Control)groupGallery).Name = "groupGallery";
		((Control)groupGallery).Size = new Size(846, 112);
		((Control)groupGallery).TabIndex = 61;
		((Control)groupGallery).TabStop = false;
		((Control)groupGallery).Text = "Gallery";
		((Control)listGallery).Anchor = (AnchorStyles)15;
		listGallery.AutoScroll = true;
		((Control)listGallery).BackColor = Color.White;
		((Control)listGallery).Location = new Point(4, 27);
		((Control)listGallery).Name = "listGallery";
		((Control)listGallery).Size = new Size(837, 80);
		((Control)listGallery).TabIndex = 59;
		((Control)pageDataColumn).Controls.Add((Control)(object)panelTreeViewDataColumnContainer);
		((Control)pageDataColumn).Controls.Add((Control)(object)groupDataColumnGallery);
		((Control)pageDataColumn).Controls.Add((Control)(object)panelProgressDataColumn);
		pageDataColumn.Dock = (DockStyle)5;
		pageDataColumn.Image = (Image)componentResourceManager.GetObject("pageDataColumn.Image");
		pageDataColumn.InvisibleVisible = false;
		((Control)pageDataColumn).Location = new Point(0, 0);
		((Control)pageDataColumn).Name = "pageDataColumn";
		((Control)pageDataColumn).Padding = new Padding(4);
		((Control)pageDataColumn).Size = new Size(716, 439);
		((Control)pageDataColumn).TabIndex = 3;
		((Control)pageDataColumn).Text = "Data Column";
		((Control)pageDataColumn).Visible = false;
		((Control)panelTreeViewDataColumnContainer).Controls.Add((Control)(object)treeViewDataColumn);
		((Control)panelTreeViewDataColumnContainer).Dock = (DockStyle)5;
		((Control)panelTreeViewDataColumnContainer).Location = new Point(4, 4);
		((Control)panelTreeViewDataColumnContainer).Name = "panelTreeViewDataColumnContainer";
		((Control)panelTreeViewDataColumnContainer).Size = new Size(708, 302);
		((Control)panelTreeViewDataColumnContainer).TabIndex = 66;
		((Control)treeViewDataColumn).Anchor = (AnchorStyles)15;
		((TreeView)treeViewDataColumn).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewDataColumn).HideSelection = false;
		((Control)treeViewDataColumn).Location = new Point(12, 12);
		((Control)treeViewDataColumn).Name = "treeViewDataColumn";
		((Control)treeViewDataColumn).Size = new Size(681, 275);
		((Control)treeViewDataColumn).TabIndex = 0;
		((TreeView)treeViewDataColumn).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		((Control)treeViewDataColumn).DoubleClick += This_OkClicked;
		((Control)groupDataColumnGallery).Controls.Add((Control)(object)listDataColumnGallery);
		((Control)groupDataColumnGallery).Dock = (DockStyle)2;
		groupDataColumnGallery.HeaderHeight = 24;
		((Control)groupDataColumnGallery).Location = new Point(4, 306);
		((Control)groupDataColumnGallery).Name = "groupDataColumnGallery";
		((Control)groupDataColumnGallery).Size = new Size(708, 112);
		((Control)groupDataColumnGallery).TabIndex = 64;
		((Control)groupDataColumnGallery).TabStop = false;
		((Control)groupDataColumnGallery).Text = "Gallery";
		((Control)listDataColumnGallery).Anchor = (AnchorStyles)15;
		listDataColumnGallery.AutoScroll = true;
		((Control)listDataColumnGallery).BackColor = Color.White;
		((Control)listDataColumnGallery).Location = new Point(4, 27);
		((Control)listDataColumnGallery).Name = "listDataColumnGallery";
		((Control)listDataColumnGallery).Size = new Size(699, 80);
		((Control)listDataColumnGallery).TabIndex = 59;
		((Control)panelProgressDataColumn).Controls.Add((Control)(object)progressDataColumn);
		((Control)panelProgressDataColumn).Dock = (DockStyle)2;
		((Control)panelProgressDataColumn).Location = new Point(4, 418);
		((Control)panelProgressDataColumn).Name = "panelProgressDataColumn";
		((Control)panelProgressDataColumn).Size = new Size(708, 17);
		((Control)panelProgressDataColumn).TabIndex = 65;
		((Control)panelProgressDataColumn).Visible = false;
		((Control)progressDataColumn).Anchor = (AnchorStyles)13;
		((Control)progressDataColumn).Location = new Point(3, 3);
		((Control)progressDataColumn).Name = "progressDataColumn";
		((Control)progressDataColumn).Size = new Size(700, 10);
		progressDataColumn.Style = (ProgressBarStyle)2;
		((Control)progressDataColumn).TabIndex = 62;
		((Control)pageHyperlink).Controls.Add((Control)(object)panel4);
		((Control)pageHyperlink).Controls.Add((Control)(object)panel3);
		((Control)pageHyperlink).Controls.Add((Control)(object)groupHyperlinkHistory);
		pageHyperlink.Dock = (DockStyle)5;
		pageHyperlink.Image = (Image)componentResourceManager.GetObject("pageHyperlink.Image");
		pageHyperlink.InvisibleVisible = false;
		((Control)pageHyperlink).Location = new Point(0, 0);
		((Control)pageHyperlink).Name = "pageHyperlink";
		((Control)pageHyperlink).Padding = new Padding(4);
		((Control)pageHyperlink).Size = new Size(716, 439);
		((Control)pageHyperlink).TabIndex = 1;
		((Control)pageHyperlink).Text = "Hyperlink";
		((Control)pageHyperlink).Visible = false;
		((Control)panel4).Controls.Add((Control)(object)panelHyperlinkPreview);
		((Control)panel4).Dock = (DockStyle)5;
		((Control)panel4).Location = new Point(4, 43);
		((Control)panel4).Name = "panel4";
		((Control)panel4).Padding = new Padding(6);
		((Control)panel4).Size = new Size(708, 280);
		((Control)panel4).TabIndex = 67;
		((Control)panelHyperlinkPreview).Anchor = (AnchorStyles)15;
		((Control)panelHyperlinkPreview).Controls.Add((Control)(object)textHyperlinkPreview);
		((Control)panelHyperlinkPreview).Controls.Add((Control)(object)progressHyperlinkPreview);
		((Control)panelHyperlinkPreview).Location = new Point(12, 12);
		((Control)panelHyperlinkPreview).Name = "panelHyperlinkPreview";
		((Control)panelHyperlinkPreview).Padding = new Padding(4);
		((Control)panelHyperlinkPreview).Size = new Size(685, 262);
		((Control)panelHyperlinkPreview).TabIndex = 69;
		((Control)panelHyperlinkPreview).Paint += new PaintEventHandler(PanelHyperlinkPreview_Paint);
		((TextBoxBase)textHyperlinkPreview).BorderStyle = (BorderStyle)0;
		((Control)textHyperlinkPreview).Dock = (DockStyle)5;
		((Control)textHyperlinkPreview).Location = new Point(4, 4);
		((Control)textHyperlinkPreview).Name = "textHyperlinkPreview";
		textHyperlinkPreview.SelectionAlignment = StiRtfSelectionAlign.Left;
		((Control)textHyperlinkPreview).Size = new Size(677, 244);
		((Control)textHyperlinkPreview).TabIndex = 67;
		((Control)textHyperlinkPreview).Text = "";
		((Control)textHyperlinkPreview).Visible = false;
		((Control)progressHyperlinkPreview).Dock = (DockStyle)2;
		((Control)progressHyperlinkPreview).Location = new Point(4, 248);
		((Control)progressHyperlinkPreview).Name = "progressHyperlinkPreview";
		((Control)progressHyperlinkPreview).Size = new Size(677, 10);
		progressHyperlinkPreview.Style = (ProgressBarStyle)2;
		((Control)progressHyperlinkPreview).TabIndex = 68;
		((Control)progressHyperlinkPreview).Visible = false;
		((Control)panel3).Controls.Add((Control)(object)labelHyperlink);
		((Control)panel3).Controls.Add((Control)(object)panelHyperlink);
		((Control)panel3).Dock = (DockStyle)1;
		((Control)panel3).Location = new Point(4, 4);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(708, 39);
		((Control)panel3).TabIndex = 66;
		((Control)labelHyperlink).BackColor = Color.Transparent;
		((Control)labelHyperlink).Location = new Point(12, 12);
		((Control)labelHyperlink).Name = "labelHyperlink";
		((Control)labelHyperlink).Size = new Size(154, 26);
		((Control)labelHyperlink).TabIndex = 7;
		((Control)labelHyperlink).Text = "Hyperlink";
		((Label)labelHyperlink).TextAlign = (ContentAlignment)16;
		((Control)groupHyperlinkHistory).Controls.Add((Control)(object)listHyperlinkHistory);
		((Control)groupHyperlinkHistory).Dock = (DockStyle)2;
		groupHyperlinkHistory.HeaderHeight = 24;
		((Control)groupHyperlinkHistory).Location = new Point(4, 323);
		((Control)groupHyperlinkHistory).Name = "groupHyperlinkHistory";
		((Control)groupHyperlinkHistory).Size = new Size(708, 112);
		((Control)groupHyperlinkHistory).TabIndex = 65;
		((Control)groupHyperlinkHistory).TabStop = false;
		((Control)groupHyperlinkHistory).Text = "History";
		((Control)groupHyperlinkHistory).Visible = false;
		((Control)listHyperlinkHistory).Anchor = (AnchorStyles)15;
		listHyperlinkHistory.AutoScroll = true;
		((Control)listHyperlinkHistory).BackColor = Color.White;
		((Control)listHyperlinkHistory).Location = new Point(4, 27);
		((Control)listHyperlinkHistory).Name = "listHyperlinkHistory";
		((Control)listHyperlinkHistory).Size = new Size(699, 80);
		((Control)listHyperlinkHistory).TabIndex = 59;
		((Control)pageFile).Controls.Add((Control)(object)panel1);
		((Control)pageFile).Controls.Add((Control)(object)groupFileHistory);
		((Control)pageFile).Controls.Add((Control)(object)panel2);
		pageFile.Dock = (DockStyle)5;
		pageFile.Image = (Image)componentResourceManager.GetObject("pageFile.Image");
		pageFile.InvisibleVisible = true;
		((Control)pageFile).Location = new Point(0, 0);
		((Control)pageFile).Name = "pageFile";
		((Control)pageFile).Padding = new Padding(4);
		((Control)pageFile).Size = new Size(854, 492);
		((Control)pageFile).TabIndex = 4;
		((Control)pageFile).Text = "File";
		((Control)pageFile).Visible = false;
		((Control)panel1).Controls.Add((Control)(object)panelFilePreview);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(4, 43);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Padding = new Padding(6);
		((Control)panel1).Size = new Size(846, 333);
		((Control)panel1).TabIndex = 68;
		((Control)panelFilePreview).AllowDrop = true;
		((Control)panelFilePreview).Anchor = (AnchorStyles)15;
		((Control)panelFilePreview).Controls.Add((Control)(object)textFilePreview);
		((Control)panelFilePreview).Location = new Point(12, 12);
		((Control)panelFilePreview).Name = "panelFilePreview";
		((Control)panelFilePreview).Size = new Size(823, 315);
		((Control)panelFilePreview).TabIndex = 67;
		((Control)panelFilePreview).DragDrop += new DragEventHandler(Panel_DragDrop);
		((Control)panelFilePreview).DragOver += new DragEventHandler(Panel_DragOver);
		((Control)panelFilePreview).Paint += new PaintEventHandler(PanelFilePreview_Paint);
		((Control)textFilePreview).Anchor = (AnchorStyles)15;
		((TextBoxBase)textFilePreview).BorderStyle = (BorderStyle)0;
		((Control)textFilePreview).Location = new Point(3, 3);
		((Control)textFilePreview).Name = "textFilePreview";
		textFilePreview.SelectionAlignment = StiRtfSelectionAlign.Left;
		((Control)textFilePreview).Size = new Size(817, 309);
		((Control)textFilePreview).TabIndex = 66;
		((Control)textFilePreview).Text = "";
		((Control)textFilePreview).Visible = false;
		((Control)groupFileHistory).Controls.Add((Control)(object)listFileHistory);
		((Control)groupFileHistory).Dock = (DockStyle)2;
		groupFileHistory.HeaderHeight = 24;
		((Control)groupFileHistory).Location = new Point(4, 376);
		((Control)groupFileHistory).Name = "groupFileHistory";
		((Control)groupFileHistory).Size = new Size(846, 112);
		((Control)groupFileHistory).TabIndex = 65;
		((Control)groupFileHistory).TabStop = false;
		((Control)groupFileHistory).Text = "History";
		((Control)groupFileHistory).Visible = false;
		((Control)listFileHistory).Anchor = (AnchorStyles)15;
		listFileHistory.AutoScroll = true;
		((Control)listFileHistory).BackColor = Color.White;
		((Control)listFileHistory).Location = new Point(4, 27);
		((Control)listFileHistory).Name = "listFileHistory";
		((Control)listFileHistory).Size = new Size(839, 80);
		((Control)listFileHistory).TabIndex = 59;
		((Control)panel2).Controls.Add((Control)(object)labelFile);
		((Control)panel2).Controls.Add((Control)(object)panelBoxFile);
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(4, 4);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(846, 39);
		((Control)panel2).TabIndex = 64;
		((Control)labelFile).BackColor = Color.Transparent;
		((Control)labelFile).Location = new Point(12, 12);
		((Control)labelFile).Name = "labelFile";
		((Control)labelFile).Size = new Size(153, 26);
		((Control)labelFile).TabIndex = 5;
		((Control)labelFile).Text = "File Path";
		((Label)labelFile).TextAlign = (ContentAlignment)16;
		((Control)radioButtonPage).BackColor = SystemColors.Control;
		((Control)radioButtonPage).Location = new Point(8, 64);
		((Control)radioButtonPage).Name = "radioButtonPage";
		((Control)radioButtonPage).Size = new Size(120, 24);
		((Control)radioButtonPage).TabIndex = 2;
		((Control)radioButtonPage).Text = "Page";
		((ButtonBase)radioButtonPage).UseVisualStyleBackColor = false;
		((Control)radioButtonReport).BackColor = SystemColors.Control;
		((RadioButton)radioButtonReport).Checked = true;
		((Control)radioButtonReport).Location = new Point(8, 16);
		((Control)radioButtonReport).Name = "radioButtonReport";
		((Control)radioButtonReport).Size = new Size(120, 24);
		((Control)radioButtonReport).TabIndex = 0;
		((RadioButton)radioButtonReport).TabStop = true;
		((Control)radioButtonReport).Text = "Report";
		((ButtonBase)radioButtonReport).UseVisualStyleBackColor = false;
		((Control)radioButtonColumn).BackColor = SystemColors.Control;
		((Control)radioButtonColumn).Location = new Point(8, 40);
		((Control)radioButtonColumn).Name = "radioButtonColumn";
		((Control)radioButtonColumn).Size = new Size(120, 24);
		((Control)radioButtonColumn).TabIndex = 1;
		((Control)radioButtonColumn).Text = "Column";
		((ButtonBase)radioButtonColumn).UseVisualStyleBackColor = false;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 492);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(973, 46);
		((Control)stiGroupLine1).TabIndex = 3;
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
		images.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("images.ImageStream");
		images.TransparentColor = Color.Transparent;
		images.Images.SetKeyName(0, "Rtf");
		images.Images.SetKeyName(1, "Txt");
		timerHyperlinkPreview.Interval = 2000;
		timerHyperlinkPreview.Tick += TimerHyperlinkPreview_Tick;
		timerRichTextFocus.Enabled = true;
		timerRichTextFocus.Interval = 500;
		timerRichTextFocus.Tick += TimerRichTextFocus_Tick;
		((Control)richTextEditorControl).Anchor = (AnchorStyles)15;
		((Control)richTextEditorControl).BackColor = Color.White;
		((Control)richTextEditorControl).Location = new Point(12, 4);
		((Control)richTextEditorControl).Margin = new Padding(4);
		((Control)richTextEditorControl).Name = "richTextEditorControl";
		richTextEditorControl.Report = null;
		((Control)richTextEditorControl).Size = new Size(822, 355);
		((Control)richTextEditorControl).TabIndex = 17;
		richTextEditorControl.TextValue = "{\\rtf1\\ansi\\ansicpg1251\\deff0\\nouicompat\\deflang1049{\\fonttbl{\\f0\\fnil\\fcharset204 Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\pard\\f0\\fs17\\par\r\n}\r\n";
		((Control)panelHyperlink).Anchor = (AnchorStyles)13;
		((Control)panelHyperlink).Controls.Add((Control)(object)textBoxHyperlink);
		((Control)panelHyperlink).Controls.Add((Control)(object)buttonHyperlink);
		((Control)panelHyperlink).Location = new Point(168, 12);
		((Control)panelHyperlink).Name = "panelHyperlink";
		((Control)panelHyperlink).Size = new Size(529, 26);
		((Control)panelHyperlink).TabIndex = 0;
		((Control)textBoxHyperlink).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxHyperlink).BorderStyle = (BorderStyle)0;
		((Control)textBoxHyperlink).Location = new Point(4, 6);
		((Control)textBoxHyperlink).Name = "textBoxHyperlink";
		((Control)textBoxHyperlink).Size = new Size(498, 13);
		((Control)textBoxHyperlink).TabIndex = 0;
		((Control)textBoxHyperlink).TextChanged += TextBoxHyperlink_TextChanged;
		((Control)textBoxHyperlink).Leave += TextBoxHyperlink_Leave;
		((Control)buttonHyperlink).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonHyperlink).Image = (Image)componentResourceManager.GetObject("buttonHyperlink.Image");
		((Control)buttonHyperlink).Location = new Point(504, 1);
		((Control)buttonHyperlink).Name = "buttonHyperlink";
		((Control)buttonHyperlink).Size = new Size(24, 24);
		((Control)buttonHyperlink).TabIndex = 1;
		((Control)buttonHyperlink).TabStop = false;
		((ButtonBase)buttonHyperlink).UseVisualStyleBackColor = true;
		((Control)buttonHyperlink).Click += ButtonHyperlink_Click;
		((Control)panelBoxFile).Anchor = (AnchorStyles)13;
		((Control)panelBoxFile).Controls.Add((Control)(object)textBoxFile);
		((Control)panelBoxFile).Controls.Add((Control)(object)buttonFile);
		((Control)panelBoxFile).Location = new Point(168, 12);
		((Control)panelBoxFile).Name = "panelBoxFile";
		((Control)panelBoxFile).Size = new Size(667, 26);
		((Control)panelBoxFile).TabIndex = 0;
		((Control)textBoxFile).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxFile).BorderStyle = (BorderStyle)0;
		((Control)textBoxFile).Location = new Point(4, 6);
		((Control)textBoxFile).Name = "textBoxFile";
		((Control)textBoxFile).Size = new Size(635, 13);
		((Control)textBoxFile).TabIndex = 0;
		((Control)textBoxFile).Leave += TextBoxFile_Leave;
		((Control)buttonFile).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonFile).Image = (Image)componentResourceManager.GetObject("buttonFile.Image");
		((Control)buttonFile).Location = new Point(642, 1);
		((Control)buttonFile).Name = "buttonFile";
		((Control)buttonFile).Size = new Size(24, 24);
		((Control)buttonFile).TabIndex = 1;
		((Control)buttonFile).TabStop = false;
		((ButtonBase)buttonFile).UseVisualStyleBackColor = true;
		((Control)buttonFile).Click += ButtonEditFilePathOnButton_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(973, 538);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(651, 389);
		((Control)this).Name = "StiSystemRichTextEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiSystemRichTextEditorForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosed += new FormClosedEventHandler(This_Closed);
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageExpression).ResumeLayout(false);
		((Control)panel5).ResumeLayout(false);
		((Control)panel5).PerformLayout();
		((Control)groupGallery).ResumeLayout(false);
		((Control)pageDataColumn).ResumeLayout(false);
		((Control)panelTreeViewDataColumnContainer).ResumeLayout(false);
		((Control)groupDataColumnGallery).ResumeLayout(false);
		((Control)panelProgressDataColumn).ResumeLayout(false);
		((Control)pageHyperlink).ResumeLayout(false);
		((Control)panel4).ResumeLayout(false);
		((Control)panelHyperlinkPreview).ResumeLayout(false);
		((Control)panel3).ResumeLayout(false);
		((Control)groupHyperlinkHistory).ResumeLayout(false);
		((Control)pageFile).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)panelFilePreview).ResumeLayout(false);
		((Control)groupFileHistory).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)contextMenuHistory).ResumeLayout(false);
		((Control)panelHyperlink).ResumeLayout(false);
		((Control)panelHyperlink).PerformLayout();
		((Control)panelBoxFile).ResumeLayout(false);
		((Control)panelBoxFile).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
