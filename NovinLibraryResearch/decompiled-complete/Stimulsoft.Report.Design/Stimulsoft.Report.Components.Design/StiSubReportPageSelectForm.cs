using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components.Design;

public class StiSubReportPageSelectForm : StiFormUX
{
	private bool firstRun;

	private StiSubReport component;

	private List<StiPage> datas;

	private StiPage nestedPage;

	private Image imageParameter;

	private StiParametersCollection parameters;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageSelectPage;

	private StiCloudTabulatorPage pageParameters;

	private StiCloudTabulatorPage pageFile;

	private StiCloudTabulatorPage pageUrl;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiListBoxV2 lbPanels;

	private StiButtonPanelUX stiGroupLine1;

	public StiToolBar tbTool;

	private StiToolButton buttonRemove;

	private StiToolButton buttonAdd;

	private StiListBoxV2 listBoxParameters;

	private StiToolButton buttonMoveDown;

	private StiToolButton buttonMoveUp;

	private StiPanelUX panelProperties;

	public StiButtonEdit textBoxExpression;

	private StiLabelUX labelExpression;

	public StiTextBox textBoxName;

	private StiLabelUX labelName;

	private StiDotSeparatorVertical stiDotSeparatorVertical1;

	private Timer timerMarker;

	private StiLabelUX labelFile;

	private StiPanelBox panelBoxFile;

	private HintTextBox textBoxFile;

	private StiButtonUX buttonEditFilePath;

	private StiLabelUX labelHyperlink;

	private StiPanelBox panelHyperlink;

	private HintTextBox textBoxHyperlink;

	private StiButtonUX buttonHyperlink;

	private StiOffice2013GroupBox groupGallery;

	private ScrollableControl listGallery;

	private StiCheckBoxUX checkBoxCreateNestedPage;

	private int ItemGalleryStep => StiScale.I4;

	private int ItemGallerySize => StiScale.XXI(75.0);

	private string LocNoPage => "[" + Loc.Get("Report", "NotAssigned") + "]";

	internal StiParametersCollection Parameters
	{
		get
		{
			return parameters;
		}
		set
		{
			parameters = value;
			BuildParameters();
		}
	}

	public StiSubReportPageSelectForm(StiSubReport component, bool firstRun = false)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.firstRun = firstRun;
			this.component = component;
			Parameters = component.Parameters.Clone() as StiParametersCollection;
			Localize();
			DoTheming();
			DoScaling();
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			TabControl.SelectedIndex = 0;
			((Form)this).ShowIcon = false;
			((Control)checkBoxCreateNestedPage).Visible = firstRun;
			if (firstRun)
			{
				nestedPage = component.SubReportPage;
			}
			else
			{
				((Control)lbPanels).Height = ((Control)checkBoxCreateNestedPage).Bottom - ((Control)lbPanels).Top;
			}
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiSubReport");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)pageParameters).Text = StiLocalization.Get("Components", "StiPage");
		((Control)pageFile).Text = StiLocalization.Get("PropertyMain", "File");
		((Control)pageParameters).Text = StiLocalization.Get("PropertyMain", "Parameters");
		((Control)pageUrl).Text = StiLocalization.Get("PropertyMain", "Hyperlink");
		((Control)buttonAdd).Text = StiLocalization.Get("Buttons", "Add");
		((Control)labelFile).Text = Loc.GetCleaned("Cloud", "labelFileName");
		((Control)labelHyperlink).Text = Loc.GetCleaned("PropertyMain", "Hyperlink");
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelExpression).Text = Loc.GetCleaned("Report", "LabelExpression");
		((Control)checkBoxCreateNestedPage).Text = Loc.Get("HelpDesigner", "PageNew").Replace(".", "");
		((Control)groupGallery).Text = StiLocalization.Get("Report", "Gallery");
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)listGallery).BackColor = StiUX.Background;
		}
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageSelectPage.Image = StiReportDesignImages.Tasks.SubReportPage(StiImageSize.Double);
		pageFile.Image = StiReportDesignImages.Tasks.SubReportFile(StiImageSize.Double);
		pageUrl.Image = StiReportDesignImages.Tasks.SubReportHyperlink(StiImageSize.Double);
		pageParameters.Image = StiReportDesignImages.Tasks.SubReportParameters(StiImageSize.Double);
		((ButtonBase)buttonAdd).Image = StiReportDesignImages.Actions.NewDataParameter();
		((ButtonBase)buttonRemove).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonMoveUp).Image = StiReportDesignImages.Actions.MoveUp();
		((ButtonBase)buttonMoveDown).Image = StiReportDesignImages.Actions.MoveDown();
		((ButtonBase)buttonEditFilePath).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)buttonHyperlink).Image = StiReportDesignImages.Actions.Edit();
		textBoxExpression.Image = StiReportDesignImages.Actions.EditSmall();
		((Control)textBoxName).Width = ((Control)textBoxExpression).Width;
	}

	private void BuildParameters()
	{
		((ListBox)listBoxParameters).Items.Clear();
		foreach (StiParameter parameter in parameters)
		{
			AddParameter(parameter);
		}
		if (((ListBox)listBoxParameters).Items.Count > 0)
		{
			((ListControl)listBoxParameters).SelectedIndex = 0;
		}
	}

	private void AddParameter(StiParameter parameter)
	{
		((ListBox)listBoxParameters).Items.Add((object)parameter);
	}

	private void BuildNestedPages()
	{
		((ListBox)lbPanels).BeginUpdate();
		((ListBox)lbPanels).Items.Clear();
		((ListBox)lbPanels).Items.Add((object)LocNoPage);
		((ListControl)lbPanels).SelectedIndex = 0;
		datas = new List<StiPage>();
		StiReport report = component.Report;
		foreach (StiPage item in from p in report.Pages.ToList()
			where p.IsPage && p != component.Page
			select p)
		{
			datas.Add(item);
			((ListBox)lbPanels).Items.Add((object)item.Name);
			if (item == component.SubReportPage)
			{
				((ListControl)lbPanels).SelectedIndex = ((ListBox)lbPanels).Items.Count - 1;
			}
		}
		((ListBox)lbPanels).EndUpdate();
	}

	private void BuildNestedReport()
	{
		string subReportUrl = component.SubReportUrl;
		string fileNameFromHyperlink = StiHyperlinkProcessor.GetFileNameFromHyperlink(subReportUrl);
		if (fileNameFromHyperlink != null)
		{
			((Control)textBoxFile).Text = fileNameFromHyperlink;
			TabControl.SelectedTab = pageFile;
		}
		else if (!string.IsNullOrEmpty(subReportUrl))
		{
			((Control)textBoxHyperlink).Text = StiExpressionPacker.PackExpression(subReportUrl, component.Report.Designer, useBraces: true);
			TabControl.SelectedTab = pageUrl;
		}
	}

	private void BuildGallery()
	{
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		List<StiResource> list = (from v in component.Report.Dictionary.Resources.ToList()
			where (v.Type == StiResourceType.Report || v.Type == StiResourceType.ReportSnapshot) && v.Content != null
			select v).ToList();
		((Control)groupGallery).Visible = list.Any();
		((Control)listGallery).Controls.Clear();
		int num = ItemGalleryStep;
		int itemGalleryStep = ItemGalleryStep;
		string resourceNameFromHyperlink = StiHyperlinkProcessor.GetResourceNameFromHyperlink(((Control)textBoxHyperlink).Text);
		foreach (StiResource item in list)
		{
			try
			{
				StiImageButton button = CreateGalleryButton(item.Name, item, num, itemGalleryStep, ItemGallerySize);
				if (resourceNameFromHyperlink == item.Name)
				{
					((CheckBox)button).Checked = true;
					ImageGallery_Click(button, EventArgs.Empty);
				}
				((CheckBox)button).CheckedChanged += ImageGallery_Click;
				((CheckBox)button).DoubleClick += This_OkClick;
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					if (!IsFormDestructed())
					{
						((Control)listGallery).Controls.Add((Control)(object)button);
					}
				});
				num += ItemGallerySize + ItemGalleryStep;
			}
			catch
			{
			}
		}
	}

	private StiImageButton CreateGalleryButton(string name, object tag, int x, int y, int thumbWidth)
	{
		StiImageButton stiImageButton = new StiImageButton();
		((Control)stiImageButton).Left = x;
		((Control)stiImageButton).Top = y;
		((Control)stiImageButton).Width = thumbWidth;
		((Control)stiImageButton).Height = thumbWidth;
		((Control)stiImageButton).Text = name;
		((Control)stiImageButton).Tag = tag;
		StiImageButton stiImageButton2 = stiImageButton;
		try
		{
			StiResource stiResource = tag as StiResource;
			StiReport stiReport = new StiReport();
			StiPage stiPage;
			if (stiResource.Type == StiResourceType.ReportSnapshot)
			{
				stiReport.LoadDocument(stiResource.Content);
				stiPage = stiReport.RenderedPages[0];
			}
			else
			{
				stiReport.Load(stiResource.Content);
				stiPage = stiReport.Pages[0];
			}
			((ButtonBase)stiImageButton2).Image = (Image)(object)stiPage.GetThumbnail(thumbWidth, thumbWidth);
		}
		catch
		{
		}
		return stiImageButton2;
	}

	private void CheckStates()
	{
		int selectedIndex = ((ListControl)listBoxParameters).SelectedIndex;
		if (selectedIndex == -1)
		{
			((Control)buttonRemove).Enabled = false;
			((Control)buttonMoveUp).Enabled = false;
			((Control)buttonMoveDown).Enabled = false;
		}
		else
		{
			((Control)buttonRemove).Enabled = true;
			if (((ListBox)listBoxParameters).Items.Count > 1)
			{
				((Control)buttonMoveUp).Enabled = selectedIndex > 0;
				((Control)buttonMoveDown).Enabled = selectedIndex < ((ListBox)listBoxParameters).Items.Count - 1;
			}
			else
			{
				((Control)buttonMoveUp).Enabled = false;
				((Control)buttonMoveDown).Enabled = false;
			}
		}
		if (selectedIndex == -1)
		{
			((Control)panelProperties).Enabled = false;
		}
		else
		{
			((Control)panelProperties).Enabled = true;
		}
	}

	private bool IsFormDestructed()
	{
		if (!((Control)this).IsDisposed && !((Control)this).Disposing)
		{
			return !((Control)this).IsHandleCreated;
		}
		return true;
	}

	private void CheckBoxCreateNestedPage_CheckedChanged(object sender, EventArgs e)
	{
		object obj = ((ListBox)lbPanels).SelectedItem;
		if (((CheckBox)checkBoxCreateNestedPage).Checked)
		{
			component.Report.Pages.Add(nestedPage);
			object obj2 = (((ListBox)lbPanels).SelectedItem = nestedPage);
			obj = obj2;
		}
		else
		{
			component.Report.Pages.Remove(nestedPage);
		}
		BuildNestedPages();
		(component.Report.Designer.DesignerControl as StiDesignerControl).Builder.BuildReport();
		if (obj != nestedPage)
		{
			((ListBox)lbPanels).SelectedItem = obj;
		}
	}

	private void TimerMarker_Tick(object sender, EventArgs e)
	{
		try
		{
			pageParameters.ShowMarker = ((ListBox)listBoxParameters).Items.Count > 0;
			pageSelectPage.ShowMarker = ((ListControl)lbPanels).SelectedIndex > 0;
			pageUrl.ShowMarker = !string.IsNullOrEmpty(((Control)textBoxHyperlink).Text);
			pageFile.ShowMarker = !string.IsNullOrEmpty(((Control)textBoxFile).Text);
		}
		catch
		{
		}
	}

	private void ListBoxParameters_SizeChanged(object sender, EventArgs eventArgs)
	{
		((Control)listBoxParameters).Invalidate();
	}

	private void ListBoxParameters_SelectedIndexChanged(object sender, EventArgs eventArgs)
	{
		int selectedIndex = ((ListControl)listBoxParameters).SelectedIndex;
		if (selectedIndex >= 0)
		{
			StiParameter stiParameter = parameters[selectedIndex];
			((Control)textBoxName).Text = stiParameter.Name;
			((Control)textBoxExpression).Text = stiParameter.Expression;
		}
		CheckStates();
		((Control)listBoxParameters).Invalidate();
	}

	private void ListBoxParameters_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		if (e.Index == -1)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		Brush val = ((e.Index == ((ListControl)listBoxParameters).SelectedIndex) ? SystemBrushes.Highlight : SystemBrushes.Window);
		Brush val2 = ((e.Index == ((ListControl)listBoxParameters).SelectedIndex) ? SystemBrushes.HighlightText : SystemBrushes.WindowText);
		Rectangle rectangle = new Rectangle(e.Bounds.Left + StiScale.I3, e.Bounds.Top + StiScale.I1, e.Bounds.Width - StiScale.I3, e.Bounds.Height - StiScale.I1);
		graphics.FillRectangle(val, rectangle);
		if (imageParameter == null)
		{
			imageParameter = (Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.Parameter.png");
		}
		Rectangle rectangle2 = new Rectangle(rectangle.Left + StiScale.I2, rectangle.Top + (rectangle.Height - StiScale.YYI(16.0)) / 2, StiScale.XXI(16.0), StiScale.YYI(16.0));
		graphics.DrawImage(imageParameter, rectangle2);
		StiParameter stiParameter = parameters[e.Index];
		Rectangle rectangle3 = new Rectangle(rectangle.Left + StiScale.XXI(22.0), rectangle.Top, rectangle.Width - StiScale.XXI(22.0), rectangle.Height - StiScale.I2);
		StringFormat val3 = new StringFormat();
		try
		{
			val3.LineAlignment = (StringAlignment)1;
			val3.Alignment = (StringAlignment)0;
			Font val4 = new Font("Arial", 8f);
			try
			{
				graphics.DrawString(stiParameter.Name, val4, val2, (RectangleF)rectangle3, val3);
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

	private void ButtonEditFilePathOn_ButtonClick(object sender, EventArgs eventArgs)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = StiLocalization.Get("FileFilters", "ReportFiles") + "|" + StiLocalization.Get("FileFilters", "DocumentFiles") + "|" + StiLocalization.Get("FileFilters", "PackedDocumentFiles") + "|" + StiLocalization.Get("FileFilters", "PackedReportFiles");
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
			{
				((Control)textBoxFile).Text = ((FileDialog)val).FileName;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void TextBoxAssignExpression_ButtonClick(object sender, EventArgs e)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		StiButtonEdit stiButtonEdit = sender as StiButtonEdit;
		string text = ((Control)stiButtonEdit).Text;
		DialogResult val = StiTextEditor.EditText(component.Report, ref text, StiCodeEditorMode.EditorCode);
		if ((int)val == 1)
		{
			((Control)stiButtonEdit).Text = text;
		}
	}

	private void ButtonAdd_Click(object sender, EventArgs e)
	{
		if (parameters == null)
		{
			parameters = new StiParametersCollection();
		}
		int num = 1;
		string name = string.Format("{0}{1}", Loc.GetMain("Parameter"), num);
		while (!parameters.Cast<StiParameter>().All((StiParameter p) => p.Name != name))
		{
			name = string.Format("{0}{1}", Loc.GetMain("Parameter"), num++);
		}
		StiParameter parameter = new StiParameter
		{
			Name = name
		};
		parameters.Add(parameter);
		AddParameter(parameter);
		((ListControl)listBoxParameters).SelectedIndex = ((ListBox)listBoxParameters).Items.Count - 1;
	}

	private void ButtonRemove_Click(object sender, EventArgs e)
	{
		int num = ((ListControl)listBoxParameters).SelectedIndex;
		StiParameter parameter = parameters[num];
		parameters.Remove(parameter);
		((ListBox)listBoxParameters).Items.RemoveAt(num);
		if (((ListBox)listBoxParameters).Items.Count > 0)
		{
			if (num > ((ListBox)listBoxParameters).Items.Count - 1)
			{
				num = ((ListBox)listBoxParameters).Items.Count - 1;
			}
		}
		else
		{
			num = -1;
		}
		if (num >= 0)
		{
			((ListControl)listBoxParameters).SelectedIndex = num;
		}
	}

	private void ButtonMoveUp_Click(object sender, EventArgs e)
	{
		int selectedIndex = ((ListControl)listBoxParameters).SelectedIndex;
		if (selectedIndex > 0)
		{
			object obj = ((ListBox)listBoxParameters).Items[selectedIndex];
			StiParameter parameter = parameters[selectedIndex];
			((ListBox)listBoxParameters).Items.RemoveAt(selectedIndex);
			parameters.RemoveAt(selectedIndex);
			selectedIndex--;
			((ListBox)listBoxParameters).Items.Insert(selectedIndex, obj);
			parameters.Insert(selectedIndex, parameter);
			((ListControl)listBoxParameters).SelectedIndex = selectedIndex;
		}
	}

	private void ButtonMoveDown_Click(object sender, EventArgs e)
	{
		int selectedIndex = ((ListControl)listBoxParameters).SelectedIndex;
		if (selectedIndex >= 0)
		{
			object obj = ((ListBox)listBoxParameters).Items[selectedIndex];
			StiParameter parameter = parameters[selectedIndex];
			((ListBox)listBoxParameters).Items.RemoveAt(selectedIndex);
			parameters.RemoveAt(selectedIndex);
			selectedIndex++;
			((ListBox)listBoxParameters).Items.Insert(selectedIndex, obj);
			parameters.Insert(selectedIndex, parameter);
			((ListControl)listBoxParameters).SelectedIndex = selectedIndex;
		}
	}

	private void TextBoxName_TextChanged(object sender, EventArgs e)
	{
		int selectedIndex = ((ListControl)listBoxParameters).SelectedIndex;
		StiParameter stiParameter = parameters[selectedIndex];
		stiParameter.Name = ((Control)textBoxName).Text;
		((ListBox)listBoxParameters).SelectedItem = stiParameter;
		((Control)listBoxParameters).Invalidate();
	}

	private void TextBoxExpression_TextChanged(object sender, EventArgs e)
	{
		int selectedIndex = ((ListControl)listBoxParameters).SelectedIndex;
		StiParameter stiParameter = parameters[selectedIndex];
		stiParameter.Expression = ((Control)textBoxExpression).Text;
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (TabControl.SelectedTab == pageSelectPage)
		{
			((Control)lbPanels).Focus();
		}
		else if (TabControl.SelectedTab == pageFile)
		{
			((Control)textBoxFile).Focus();
		}
		else if (TabControl.SelectedTab == pageUrl)
		{
			((Control)textBoxHyperlink).Focus();
		}
	}

	private void ButtonHyperlink_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		string text = ((Control)textBoxHyperlink).Text;
		text = StiExpressionPacker.PackExpression(text, component.Report.Designer, useBraces: false);
		DialogResult val = StiTextEditor.EditText(component.Report, ref text, StiCodeEditorMode.EditorExpression);
		if ((int)val == 1)
		{
			((Control)textBoxHyperlink).Text = StiExpressionPacker.UnPackExpression(text, component.Report.Designer, useBraces: false);
		}
		((Control)textBoxHyperlink).Focus();
		((TextBoxBase)textBoxHyperlink).Select(0, 0);
	}

	private void ImageGallery_Click(object sender, EventArgs e)
	{
		(from Control c in (IEnumerable)((Control)listGallery).Controls
			where c is StiImageButton && c != sender
			select c).Cast<StiImageButton>().ToList().ForEach(delegate(StiImageButton c)
		{
			((CheckBox)c).Checked = false;
		});
		if (sender is StiImageButton stiImageButton && ((CheckBox)stiImageButton).Checked && ((Control)stiImageButton).Tag is StiResource stiResource)
		{
			((Control)textBoxHyperlink).Text = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
		}
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		timerMarker.Stop();
	}

	private void This_Load(object sender, EventArgs e)
	{
		BuildNestedPages();
		BuildNestedReport();
		BuildGallery();
		CheckStates();
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		if (imageParameter != null)
		{
			imageParameter.Dispose();
			imageParameter = null;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?report_internals_sub-reports.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		int num = TabControl.SelectedIndex;
		if (num >= 3)
		{
			int num2 = 0;
			if (((ListControl)lbPanels).SelectedIndex > 0)
			{
				num2++;
			}
			if (!string.IsNullOrEmpty(((Control)textBoxFile).Text))
			{
				num2++;
			}
			if (!string.IsNullOrEmpty(((Control)textBoxHyperlink).Text))
			{
				num2++;
			}
			if (num2 == 1)
			{
				if (((ListControl)lbPanels).SelectedIndex > 0)
				{
					num = 0;
				}
				else if (!string.IsNullOrEmpty(((Control)textBoxFile).Text))
				{
					num = 1;
				}
				else if (!string.IsNullOrEmpty(((Control)textBoxHyperlink).Text))
				{
					num = 2;
				}
			}
		}
		switch (num)
		{
		case 0:
			component.SubReportPage = ((((ListControl)lbPanels).SelectedIndex > 0) ? datas[((ListControl)lbPanels).SelectedIndex - 1] : null);
			component.SubReportUrl = string.Empty;
			break;
		case 1:
			component.SubReportUrl = ((!string.IsNullOrEmpty(((Control)textBoxFile).Text)) ? StiHyperlinkProcessor.CreateFileName(((Control)textBoxFile).Text) : string.Empty);
			if (firstRun && component.Report.Pages.Contains(nestedPage))
			{
				component.Report.Pages.Remove(nestedPage);
			}
			if (!string.IsNullOrEmpty(component.SubReportUrl))
			{
				component.SubReportPage = null;
			}
			break;
		case 2:
			component.SubReportUrl = ((!string.IsNullOrEmpty(((Control)textBoxHyperlink).Text)) ? StiExpressionPacker.UnPackExpression(((Control)textBoxHyperlink).Text, component.Report.Designer, useBraces: true) : string.Empty);
			if (firstRun && component.Report.Pages.Contains(nestedPage))
			{
				component.Report.Pages.Remove(nestedPage);
			}
			if (!string.IsNullOrEmpty(component.SubReportUrl))
			{
				component.SubReportPage = null;
			}
			break;
		}
		component.Parameters = parameters;
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Expected O, but got Unknown
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Expected O, but got Unknown
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_0933: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2a: Expected O, but got Unknown
		//IL_0cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Expected O, but got Unknown
		//IL_0d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_106e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1078: Expected O, but got Unknown
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109f: Expected O, but got Unknown
		//IL_113b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1189: Unknown result type (might be due to invalid IL or missing references)
		//IL_1193: Expected O, but got Unknown
		//IL_1266: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ef: Expected O, but got Unknown
		//IL_1396: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a0: Expected O, but got Unknown
		//IL_1447: Unknown result type (might be due to invalid IL or missing references)
		//IL_1451: Expected O, but got Unknown
		//IL_14f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1502: Expected O, but got Unknown
		//IL_170f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1719: Expected O, but got Unknown
		//IL_1721: Unknown result type (might be due to invalid IL or missing references)
		//IL_172b: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSubReportPageSelectForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		TabControl = new StiCloudTabulator();
		pageSelectPage = new StiCloudTabulatorPage();
		checkBoxCreateNestedPage = new StiCheckBoxUX();
		lbPanels = new StiListBoxV2();
		pageFile = new StiCloudTabulatorPage();
		panelBoxFile = new StiPanelBox();
		textBoxFile = new HintTextBox();
		buttonEditFilePath = new StiButtonUX();
		labelFile = new StiLabelUX();
		pageUrl = new StiCloudTabulatorPage();
		groupGallery = new StiOffice2013GroupBox();
		listGallery = new ScrollableControl();
		labelHyperlink = new StiLabelUX();
		panelHyperlink = new StiPanelBox();
		textBoxHyperlink = new HintTextBox();
		buttonHyperlink = new StiButtonUX();
		pageParameters = new StiCloudTabulatorPage();
		panelProperties = new StiPanelUX();
		stiDotSeparatorVertical1 = new StiDotSeparatorVertical();
		labelExpression = new StiLabelUX();
		textBoxName = new StiTextBox();
		labelName = new StiLabelUX();
		textBoxExpression = new StiButtonEdit();
		listBoxParameters = new StiListBoxV2();
		tbTool = new StiToolBar();
		buttonMoveDown = new StiToolButton();
		buttonMoveUp = new StiToolButton();
		buttonRemove = new StiToolButton();
		buttonAdd = new StiToolButton();
		stiGroupLine1 = new StiButtonPanelUX();
		timerMarker = new Timer(components);
		((Control)TabControl).SuspendLayout();
		((Control)pageSelectPage).SuspendLayout();
		((Control)pageFile).SuspendLayout();
		((Control)panelBoxFile).SuspendLayout();
		((Control)pageUrl).SuspendLayout();
		((Control)groupGallery).SuspendLayout();
		((Control)panelHyperlink).SuspendLayout();
		((Control)pageParameters).SuspendLayout();
		((Control)panelProperties).SuspendLayout();
		((Control)tbTool).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(492, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(573, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)TabControl).Anchor = (AnchorStyles)15;
		((Control)TabControl).Controls.Add((Control)(object)pageSelectPage);
		((Control)TabControl).Controls.Add((Control)(object)pageFile);
		((Control)TabControl).Controls.Add((Control)(object)pageUrl);
		((Control)TabControl).Controls.Add((Control)(object)pageParameters);
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(660, 317);
		((Control)TabControl).TabIndex = 5;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageSelectPage).Controls.Add((Control)(object)checkBoxCreateNestedPage);
		((Control)pageSelectPage).Controls.Add((Control)(object)lbPanels);
		pageSelectPage.Dock = (DockStyle)5;
		pageSelectPage.Image = (Image)componentResourceManager.GetObject("pageSelectPage.Image");
		pageSelectPage.InvisibleVisible = true;
		((Control)pageSelectPage).Location = new Point(0, 0);
		((Control)pageSelectPage).Name = "pageSelectPage";
		((Control)pageSelectPage).Padding = new Padding(3);
		((Control)pageSelectPage).Size = new Size(541, 317);
		((Control)pageSelectPage).TabIndex = 2;
		((Control)pageSelectPage).Text = "Page";
		((Control)pageSelectPage).Visible = false;
		((Control)checkBoxCreateNestedPage).Anchor = (AnchorStyles)6;
		((Control)checkBoxCreateNestedPage).AutoSize = true;
		((CheckBox)checkBoxCreateNestedPage).Checked = true;
		((CheckBox)checkBoxCreateNestedPage).CheckState = (CheckState)1;
		((Control)checkBoxCreateNestedPage).Location = new Point(12, 288);
		((Control)checkBoxCreateNestedPage).Name = "checkBoxCreateNestedPage";
		((Control)checkBoxCreateNestedPage).Size = new Size(122, 17);
		((Control)checkBoxCreateNestedPage).TabIndex = 1;
		((Control)checkBoxCreateNestedPage).Text = "Create Nested Page";
		((ButtonBase)checkBoxCreateNestedPage).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxCreateNestedPage).CheckedChanged += CheckBoxCreateNestedPage_CheckedChanged;
		((Control)lbPanels).Anchor = (AnchorStyles)15;
		((Control)lbPanels).Location = new Point(12, 12);
		((Control)lbPanels).Name = "lbPanels";
		((Control)lbPanels).Size = new Size(518, 262);
		((Control)lbPanels).TabIndex = 0;
		((Control)lbPanels).DoubleClick += This_OkClick;
		((Control)pageFile).Controls.Add((Control)(object)panelBoxFile);
		((Control)pageFile).Controls.Add((Control)(object)labelFile);
		pageFile.Dock = (DockStyle)5;
		pageFile.Image = (Image)componentResourceManager.GetObject("pageFile.Image");
		pageFile.InvisibleVisible = false;
		((Control)pageFile).Location = new Point(0, 0);
		((Control)pageFile).Name = "pageFile";
		((Control)pageFile).Padding = new Padding(4);
		((Control)pageFile).Size = new Size(541, 317);
		((Control)pageFile).TabIndex = 4;
		((Control)pageFile).Text = "File";
		((Control)pageFile).Visible = false;
		((Control)panelBoxFile).Anchor = (AnchorStyles)13;
		((Control)panelBoxFile).Controls.Add((Control)(object)textBoxFile);
		((Control)panelBoxFile).Controls.Add((Control)(object)buttonEditFilePath);
		((Control)panelBoxFile).Location = new Point(150, 12);
		((Control)panelBoxFile).Name = "panelBoxFile";
		((Control)panelBoxFile).Size = new Size(380, 26);
		((Control)panelBoxFile).TabIndex = 9;
		((Control)textBoxFile).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxFile).BorderStyle = (BorderStyle)0;
		((Control)textBoxFile).Location = new Point(4, 6);
		((Control)textBoxFile).Name = "textBoxFile";
		((Control)textBoxFile).Size = new Size(348, 13);
		((Control)textBoxFile).TabIndex = 0;
		((Control)buttonEditFilePath).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditFilePath).Image = (Image)componentResourceManager.GetObject("buttonEditFilePath.Image");
		((Control)buttonEditFilePath).Location = new Point(355, 1);
		((Control)buttonEditFilePath).Name = "buttonEditFilePath";
		((Control)buttonEditFilePath).Size = new Size(24, 24);
		((Control)buttonEditFilePath).TabIndex = 1;
		((Control)buttonEditFilePath).TabStop = false;
		((ButtonBase)buttonEditFilePath).UseVisualStyleBackColor = true;
		((Control)buttonEditFilePath).Click += ButtonEditFilePathOn_ButtonClick;
		((Control)labelFile).Location = new Point(12, 12);
		((Control)labelFile).Name = "labelFile";
		((Control)labelFile).Size = new Size(135, 25);
		((Control)labelFile).TabIndex = 8;
		((Control)labelFile).Text = "File Path";
		((Label)labelFile).TextAlign = (ContentAlignment)16;
		((Control)pageUrl).Controls.Add((Control)(object)groupGallery);
		((Control)pageUrl).Controls.Add((Control)(object)labelHyperlink);
		((Control)pageUrl).Controls.Add((Control)(object)panelHyperlink);
		pageUrl.Dock = (DockStyle)5;
		pageUrl.Image = (Image)componentResourceManager.GetObject("pageUrl.Image");
		pageUrl.InvisibleVisible = false;
		((Control)pageUrl).Location = new Point(0, 0);
		((Control)pageUrl).Name = "pageUrl";
		((Control)pageUrl).Padding = new Padding(4);
		((Control)pageUrl).Size = new Size(541, 317);
		((Control)pageUrl).TabIndex = 1;
		((Control)pageUrl).Text = "Url";
		((Control)pageUrl).Visible = false;
		((Control)groupGallery).Controls.Add((Control)(object)listGallery);
		((Control)groupGallery).Dock = (DockStyle)2;
		groupGallery.HeaderHeight = 24;
		((Control)groupGallery).Location = new Point(4, 180);
		((Control)groupGallery).Name = "groupGallery";
		((Control)groupGallery).Size = new Size(533, 133);
		((Control)groupGallery).TabIndex = 61;
		((Control)groupGallery).TabStop = false;
		((Control)groupGallery).Text = "Gallery";
		((Control)listGallery).Anchor = (AnchorStyles)15;
		listGallery.AutoScroll = true;
		((Control)listGallery).BackColor = Color.White;
		((Control)listGallery).Location = new Point(4, 27);
		((Control)listGallery).Name = "listGallery";
		((Control)listGallery).Size = new Size(524, 101);
		((Control)listGallery).TabIndex = 59;
		((Control)labelHyperlink).Location = new Point(12, 12);
		((Control)labelHyperlink).Name = "labelHyperlink";
		((Control)labelHyperlink).Size = new Size(135, 25);
		((Control)labelHyperlink).TabIndex = 9;
		((Control)labelHyperlink).Text = "Hyperlink";
		((Label)labelHyperlink).TextAlign = (ContentAlignment)16;
		((Control)panelHyperlink).Anchor = (AnchorStyles)13;
		((Control)panelHyperlink).Controls.Add((Control)(object)textBoxHyperlink);
		((Control)panelHyperlink).Controls.Add((Control)(object)buttonHyperlink);
		((Control)panelHyperlink).Location = new Point(150, 12);
		((Control)panelHyperlink).Name = "panelHyperlink";
		((Control)panelHyperlink).Size = new Size(380, 26);
		((Control)panelHyperlink).TabIndex = 8;
		((Control)textBoxHyperlink).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxHyperlink).BorderStyle = (BorderStyle)0;
		((Control)textBoxHyperlink).Location = new Point(4, 6);
		((Control)textBoxHyperlink).Name = "textBoxHyperlink";
		((Control)textBoxHyperlink).Size = new Size(349, 13);
		((Control)textBoxHyperlink).TabIndex = 0;
		((Control)buttonHyperlink).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonHyperlink).Image = (Image)componentResourceManager.GetObject("buttonHyperlink.Image");
		((Control)buttonHyperlink).Location = new Point(355, 1);
		((Control)buttonHyperlink).Name = "buttonHyperlink";
		((Control)buttonHyperlink).Size = new Size(24, 24);
		((Control)buttonHyperlink).TabIndex = 1;
		((Control)buttonHyperlink).TabStop = false;
		((ButtonBase)buttonHyperlink).UseVisualStyleBackColor = true;
		((Control)buttonHyperlink).Click += ButtonHyperlink_Click;
		((Control)pageParameters).Controls.Add((Control)(object)panelProperties);
		((Control)pageParameters).Controls.Add((Control)(object)listBoxParameters);
		((Control)pageParameters).Controls.Add((Control)(object)tbTool);
		pageParameters.Dock = (DockStyle)5;
		pageParameters.Image = (Image)componentResourceManager.GetObject("pageParameters.Image");
		pageParameters.InvisibleVisible = false;
		((Control)pageParameters).Location = new Point(0, 0);
		((Control)pageParameters).Name = "pageParameters";
		((Control)pageParameters).Padding = new Padding(0, 0, 1, 0);
		((Control)pageParameters).Size = new Size(541, 317);
		((Control)pageParameters).TabIndex = 3;
		((Control)pageParameters).Text = "Parameters";
		((Control)pageParameters).Visible = false;
		((Control)panelProperties).Anchor = (AnchorStyles)15;
		((Control)panelProperties).Controls.Add((Control)(object)stiDotSeparatorVertical1);
		((Control)panelProperties).Controls.Add((Control)(object)labelExpression);
		((Control)panelProperties).Controls.Add((Control)(object)textBoxName);
		((Control)panelProperties).Controls.Add((Control)(object)labelName);
		((Control)panelProperties).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelProperties).Location = new Point(222, 32);
		((Control)panelProperties).Margin = new Padding(0);
		((Control)panelProperties).Name = "panelProperties";
		((Control)panelProperties).Size = new Size(318, 285);
		((Control)panelProperties).TabIndex = 61;
		((Control)stiDotSeparatorVertical1).Dock = (DockStyle)3;
		((Control)stiDotSeparatorVertical1).Location = new Point(0, 0);
		((Control)stiDotSeparatorVertical1).Margin = new Padding(0);
		((Control)stiDotSeparatorVertical1).MaximumSize = new Size(5, 0);
		((Control)stiDotSeparatorVertical1).MinimumSize = new Size(5, 0);
		((Control)stiDotSeparatorVertical1).Name = "stiDotSeparatorVertical1";
		((Control)stiDotSeparatorVertical1).Size = new Size(5, 285);
		((Control)stiDotSeparatorVertical1).TabIndex = 38;
		((Control)stiDotSeparatorVertical1).Text = "stiDotSeparatorVertical1";
		((Control)labelExpression).Location = new Point(12, 44);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(103, 22);
		((Control)labelExpression).TabIndex = 36;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Label)labelExpression).UseCompatibleTextRendering = true;
		((Control)textBoxName).Anchor = (AnchorStyles)13;
		((Control)textBoxName).Location = new Point(121, 12);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(185, 21);
		((Control)textBoxName).TabIndex = 35;
		((Control)textBoxName).TextChanged += TextBoxName_TextChanged;
		((Control)labelName).Location = new Point(12, 14);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(103, 18);
		((Control)labelName).TabIndex = 37;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Label)labelName).UseCompatibleTextRendering = true;
		textBoxExpression.AllowEscape = false;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		textBoxExpression.ButtonBitmap = (Image)componentResourceManager.GetObject("textBoxExpression.ButtonBitmap");
		textBoxExpression.Flat = false;
		textBoxExpression.Image = (Image)componentResourceManager.GetObject("textBoxExpression.Image");
		((Control)textBoxExpression).Location = new Point(121, 44);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(185, 21);
		((Control)textBoxExpression).TabIndex = 3;
		textBoxExpression.ButtonClick += TextBoxAssignExpression_ButtonClick;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)listBoxParameters).Anchor = (AnchorStyles)7;
		((Control)listBoxParameters).Location = new Point(12, 44);
		((Control)listBoxParameters).Margin = new Padding(0);
		((Control)listBoxParameters).Name = "listBoxParameters";
		((Control)listBoxParameters).Size = new Size(196, 260);
		((Control)listBoxParameters).TabIndex = 59;
		((ListBox)listBoxParameters).DrawItem += new DrawItemEventHandler(ListBoxParameters_DrawItem);
		((ListBox)listBoxParameters).SelectedIndexChanged += ListBoxParameters_SelectedIndexChanged;
		((Control)listBoxParameters).SizeChanged += ListBoxParameters_SizeChanged;
		((Control)tbTool).Controls.Add((Control)(object)buttonMoveDown);
		((Control)tbTool).Controls.Add((Control)(object)buttonMoveUp);
		((Control)tbTool).Controls.Add((Control)(object)buttonRemove);
		((Control)tbTool).Controls.Add((Control)(object)buttonAdd);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(0, 0);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(3);
		((Control)tbTool).Size = new Size(540, 32);
		((Control)tbTool).TabIndex = 58;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)buttonMoveDown).AutoSize = true;
		((Control)buttonMoveDown).Dock = (DockStyle)3;
		buttonMoveDown.Draw3DButton = false;
		buttonMoveDown.HotDragDrop = false;
		((ButtonBase)buttonMoveDown).Image = (Image)componentResourceManager.GetObject("buttonMoveDown.Image");
		((Control)buttonMoveDown).Location = new Point(111, 3);
		((Control)buttonMoveDown).Name = "buttonMoveDown";
		((Control)buttonMoveDown).Size = new Size(30, 26);
		((Control)buttonMoveDown).TabIndex = 3;
		((ButtonBase)buttonMoveDown).UseVisualStyleBackColor = false;
		((Control)buttonMoveDown).Click += ButtonMoveDown_Click;
		((Control)buttonMoveUp).AutoSize = true;
		((Control)buttonMoveUp).Dock = (DockStyle)3;
		buttonMoveUp.Draw3DButton = false;
		buttonMoveUp.HotDragDrop = false;
		((ButtonBase)buttonMoveUp).Image = (Image)componentResourceManager.GetObject("buttonMoveUp.Image");
		((Control)buttonMoveUp).Location = new Point(81, 3);
		((Control)buttonMoveUp).Name = "buttonMoveUp";
		((Control)buttonMoveUp).Size = new Size(30, 26);
		((Control)buttonMoveUp).TabIndex = 2;
		((ButtonBase)buttonMoveUp).UseVisualStyleBackColor = false;
		((Control)buttonMoveUp).Click += ButtonMoveUp_Click;
		((Control)buttonRemove).AutoSize = true;
		((Control)buttonRemove).Dock = (DockStyle)3;
		buttonRemove.Draw3DButton = false;
		buttonRemove.HotDragDrop = false;
		((ButtonBase)buttonRemove).Image = (Image)componentResourceManager.GetObject("buttonRemove.Image");
		((Control)buttonRemove).Location = new Point(51, 3);
		((Control)buttonRemove).Name = "buttonRemove";
		((Control)buttonRemove).Size = new Size(30, 26);
		((Control)buttonRemove).TabIndex = 1;
		((ButtonBase)buttonRemove).UseVisualStyleBackColor = false;
		((Control)buttonRemove).Click += ButtonRemove_Click;
		((Control)buttonAdd).AutoSize = true;
		((Control)buttonAdd).Dock = (DockStyle)3;
		buttonAdd.Draw3DButton = false;
		buttonAdd.HotDragDrop = false;
		((ButtonBase)buttonAdd).Image = (Image)componentResourceManager.GetObject("buttonAdd.Image");
		((Control)buttonAdd).Location = new Point(3, 3);
		((Control)buttonAdd).Name = "buttonAdd";
		((Control)buttonAdd).Size = new Size(48, 26);
		((Control)buttonAdd).TabIndex = 0;
		((Control)buttonAdd).Text = "Add";
		((ButtonBase)buttonAdd).UseVisualStyleBackColor = false;
		((Control)buttonAdd).Click += ButtonAdd_Click;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 320);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(660, 46);
		((Control)stiGroupLine1).TabIndex = 4;
		timerMarker.Enabled = true;
		timerMarker.Interval = 500;
		timerMarker.Tick += TimerMarker_Tick;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(660, 366);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(645, 385);
		((Control)this).Name = "StiSubReportPageSelectForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "SubReport Page";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Form)this).Load += This_Load;
		((Control)TabControl).ResumeLayout(false);
		((Control)pageSelectPage).ResumeLayout(false);
		((Control)pageSelectPage).PerformLayout();
		((Control)pageFile).ResumeLayout(false);
		((Control)panelBoxFile).ResumeLayout(false);
		((Control)panelBoxFile).PerformLayout();
		((Control)pageUrl).ResumeLayout(false);
		((Control)groupGallery).ResumeLayout(false);
		((Control)panelHyperlink).ResumeLayout(false);
		((Control)panelHyperlink).PerformLayout();
		((Control)pageParameters).ResumeLayout(false);
		((Control)panelProperties).ResumeLayout(false);
		((Control)tbTool).ResumeLayout(false);
		((Control)tbTool).PerformLayout();
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
