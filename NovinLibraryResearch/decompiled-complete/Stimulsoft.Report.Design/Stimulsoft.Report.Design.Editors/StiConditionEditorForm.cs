using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Editor;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Editors;

public class StiConditionEditorForm : StiFormUX
{
	public enum StartPage
	{
		Expression,
		DataColumn,
		Summary
	}

	private StiGroupHeaderBand group;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageExpression;

	private StiCloudTabulatorPage pageDataColumn;

	private StiCloudTabulatorPage pageSummary;

	private ImageList ilTool;

	private ImageList ilDataSet;

	private StiTreeViewUX treeViewDataColumn;

	private StiCodeEditor CodeEditor;

	private StiCodeEditor CodeEditorSummary;

	private SyntaxEdit syntaxEdit1;

	private SyntaxEdit syntaxEditSummary;

	private StiLabelUX labelSummarySortDirection;

	private StiComboBox comboBoxSummarySortDirection;

	private StiComboBox comboBoxSortDirection;

	private StiComboBox comboBoxSummaryType;

	private StiButtonPanelUX panelButtons;

	private StiGroupPanelUX stiGroupLine2;

	private StiGroupPanelUX stiGroupLine3;

	private Timer timerMarker;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX labelSortDirection;

	private StiLabelUX labelSummaryType;

	private string CodeEditorText
	{
		get
		{
			return StiExpressionPacker.UnPackExpression(((Control)CodeEditor).Text, group.Report.Designer, useBraces: false);
		}
		set
		{
			((Control)CodeEditor).Text = StiExpressionPacker.PackExpression(group.Condition.Value, group.Report.Designer, useBraces: false);
		}
	}

	private string CodeEditorTextSummary
	{
		get
		{
			return StiExpressionPacker.UnPackExpression(((Control)CodeEditorSummary).Text, group.Report.Designer, useBraces: false);
		}
		set
		{
			((Control)CodeEditorSummary).Text = StiExpressionPacker.PackExpression(group.SummaryExpression.Value, group.Report.Designer, useBraces: false);
		}
	}

	public StiConditionEditorForm(StiGroupHeaderBand group, StartPage? startPage)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		this.group = group;
		Localize();
		DoScaling();
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		if (CodeEditor.DictionaryControl != null)
		{
			((Control)CodeEditor.DictionaryControl).Width = StiSettings.GetInt("StiGroupEditorForm", "Spl", ((Control)CodeEditor.DictionaryControl).Width);
		}
		if (CodeEditorSummary.DictionaryControl != null)
		{
			((Control)CodeEditorSummary.DictionaryControl).Width = StiSettings.GetInt("StiGroupEditorForm", "Summary", ((Control)CodeEditorSummary.DictionaryControl).Width);
		}
		CodeEditor.Init(group.Report, StiCodeEditorMode.EditorExpression);
		CodeEditorSummary.Init(group.Report, StiCodeEditorMode.EditorExpression);
		((Control)treeViewDataColumn).Handle.ToString();
		if (startPage.HasValue)
		{
			switch (startPage)
			{
			case StartPage.DataColumn:
				TabControl.SelectedTab = pageDataColumn;
				break;
			case StartPage.Expression:
				TabControl.SelectedTab = pageExpression;
				((Control)CodeEditor).Focus();
				break;
			case StartPage.Summary:
				TabControl.SelectedTab = pageSummary;
				break;
			}
		}
		else
		{
			TabControl.SelectedTab = ((BuildDataColumn() || this.group.Condition.Value.Length == 0) ? pageDataColumn : pageExpression);
		}
		TabControl_SelectedIndexChanged(null, null);
		CodeEditorText = this.group.Condition.Value;
		CodeEditorTextSummary = this.group.SummaryExpression.Value;
		switch (group.SummarySortDirection)
		{
		case StiGroupSortDirection.Ascending:
			((Control)comboBoxSummarySortDirection).Text = StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionAscending");
			break;
		case StiGroupSortDirection.Descending:
			((Control)comboBoxSummarySortDirection).Text = StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionDescending");
			break;
		case StiGroupSortDirection.None:
			((Control)comboBoxSummarySortDirection).Text = StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionNone");
			break;
		}
		switch (group.SortDirection)
		{
		case StiGroupSortDirection.Ascending:
			((Control)comboBoxSortDirection).Text = StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionAscending");
			break;
		case StiGroupSortDirection.Descending:
			((Control)comboBoxSortDirection).Text = StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionDescending");
			break;
		case StiGroupSortDirection.None:
			((Control)comboBoxSortDirection).Text = StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionNone");
			break;
		}
		((ListControl)comboBoxSummaryType).SelectedIndex = (int)group.SummaryType;
		((Form)this).ShowIcon = false;
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("FormTitles", "GroupConditionForm");
		((Control)pageDataColumn).Text = StiLocalization.Get("PropertyMain", "DataColumn");
		((Control)pageExpression).Text = StiLocalization.Get("PropertyMain", "Expression");
		((Control)pageSummary).Text = StiLocalization.Get("PropertyMain", "Summary");
		((ComboBox)comboBoxSummarySortDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionNone"));
		((ComboBox)comboBoxSummarySortDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionAscending"));
		((ComboBox)comboBoxSummarySortDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionDescending"));
		((ComboBox)comboBoxSortDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionNone"));
		((ComboBox)comboBoxSortDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionAscending"));
		((ComboBox)comboBoxSortDirection).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionDescending"));
		((Control)labelSummarySortDirection).Text = Loc.GetCleaned("PropertyMain", "SummarySortDirection");
		((Control)labelSortDirection).Text = Loc.GetCleaned("PropertyMain", "SortDirection");
		((Control)labelSummaryType).Text = Loc.GetCleaned("PropertyMain", "SummaryType");
		Size size = TextRenderer.MeasureText(((Control)labelSummarySortDirection).Text, ((Control)labelSummarySortDirection).Font);
		int num = (size.Width += 10);
		size = TextRenderer.MeasureText(((Control)labelSummaryType).Text, ((Control)labelSummaryType).Font);
		if (size.Width + 10 > num)
		{
			num = size.Width + 10;
		}
		size = TextRenderer.MeasureText(((Control)labelSortDirection).Text, ((Control)labelSummaryType).Font);
		if (size.Width + 10 > num)
		{
			num = size.Width + 10;
		}
		((Control)labelSummarySortDirection).Width = num;
		((Control)labelSummaryType).Width = num;
		((Control)labelSortDirection).Width = num;
		((Control)comboBoxSummarySortDirection).Left = ((Control)labelSummaryType).Right + 10;
		((Control)comboBoxSummaryType).Left = ((Control)labelSummaryType).Right + 10;
		((Control)comboBoxSortDirection).Left = ((Control)labelSummaryType).Right + 10;
		string[] names = Enum.GetNames(typeof(StiGroupSummaryType));
		foreach (string text in names)
		{
			((ComboBox)comboBoxSummaryType).Items.Add((object)text);
		}
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageDataColumn.Image = StiReportDesignImages.Tasks.GroupDataColumn(StiImageSize.Double);
		pageExpression.Image = StiReportDesignImages.Tasks.GroupExpression(StiImageSize.Double);
		pageSummary.Image = StiReportDesignImages.Tasks.GroupSummary(StiImageSize.Double);
	}

	private bool BuildDataColumn()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		string text = group.Condition.Value.Replace(" ", "");
		text = text.Replace("{", "");
		text = text.Replace("}", "");
		((TreeView)treeViewDataColumn).ImageList = StiDataBuilder.Images;
		TreeNode val = new TreeNode(StiLocalization.Get("Report", "No"));
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		((TreeView)treeViewDataColumn).Nodes.Add(val);
		dataBuilder.Build(((TreeView)treeViewDataColumn).Nodes, group.Report.Dictionary, dictionaryDesign: true, includeCalcColumn: false);
		bool result = false;
		if (text.Length > 0)
		{
			string[] strs = text.Split(new char[1] { '.' });
			result = SelectNode(strs, 0, ((TreeView)treeViewDataColumn).Nodes, (TreeView)(object)treeViewDataColumn);
		}
		if (((TreeView)treeViewDataColumn).SelectedNode == null)
		{
			((TreeView)treeViewDataColumn).SelectedNode = ((TreeView)treeViewDataColumn).Nodes[0];
		}
		return result;
	}

	private bool SelectNode(string[] strs, int index, TreeNodeCollection nodes, TreeView view)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		string text = strs[index];
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			string text2 = val.Text;
			if (val.Tag is StiDataRelation stiDataRelation)
			{
				text2 = stiDataRelation.Name;
			}
			int num = text2.IndexOfInvariant("[") - 1;
			int num2 = text2.IndexOfInvariant("]");
			if (num != -1 && num2 != -1 && num < num2)
			{
				text2 = text2.Remove(num, num2 - num + 1);
			}
			text2 = StiNameValidator.CorrectName(text2, group?.Report);
			if (!(text2 == text))
			{
				continue;
			}
			if (val.Nodes.Count == 0 || index >= strs.Length - 1)
			{
				if (strs.Length - 1 != index)
				{
					return false;
				}
				view.SelectedNode = val;
				return true;
			}
			if (!val.IsExpanded)
			{
				val.Expand();
			}
			return SelectNode(strs, index + 1, val.Nodes, view);
		}
		return false;
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		switch (TabControl.SelectedIndex)
		{
		case 0:
		case 1:
			((Control)stiGroupLine2).Visible = false;
			((Control)stiGroupLine3).Visible = true;
			break;
		case 2:
			((Control)stiGroupLine2).Visible = true;
			((Control)stiGroupLine3).Visible = false;
			break;
		}
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: false);
		}
	}

	private void TimerMarker_Tick(object sender, EventArgs e)
	{
		try
		{
			pageSummary.ShowMarker = !string.IsNullOrEmpty(((Control)CodeEditorSummary).Text);
		}
		catch
		{
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		group.Report.Designer.UndoRedoSave(StiLocalization.Get("Buttons", "Duplicate"));
		group.Report.IsModified = true;
		if (((Control)comboBoxSummarySortDirection).Text == StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionAscending"))
		{
			group.SummarySortDirection = StiGroupSortDirection.Ascending;
		}
		else if (((Control)comboBoxSummarySortDirection).Text == StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionDescending"))
		{
			group.SummarySortDirection = StiGroupSortDirection.Descending;
		}
		else if (((Control)comboBoxSummarySortDirection).Text == StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionNone"))
		{
			group.SummarySortDirection = StiGroupSortDirection.None;
		}
		if (((Control)comboBoxSortDirection).Text == StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionAscending"))
		{
			group.SortDirection = StiGroupSortDirection.Ascending;
		}
		else if (((Control)comboBoxSortDirection).Text == StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionDescending"))
		{
			group.SortDirection = StiGroupSortDirection.Descending;
		}
		else if (((Control)comboBoxSortDirection).Text == StiLocalization.Get("PropertyEnum", "StiGroupSortDirectionNone"))
		{
			group.SortDirection = StiGroupSortDirection.None;
		}
		if (TabControl.SelectedTab == pageDataColumn)
		{
			string text = string.Empty;
			if (((TreeView)treeViewDataColumn).SelectedNode != null)
			{
				if (((TreeView)treeViewDataColumn).SelectedNode.Text == StiLocalization.Get("Report", "No"))
				{
					text = string.Empty;
				}
				if (((TreeView)treeViewDataColumn).SelectedNode.Tag is StiDataColumn)
				{
					text = StiDataBuilder.GetColumnPathFromNode(((TreeView)treeViewDataColumn).SelectedNode, useRelationName: false);
				}
			}
			group.Condition.Value = ((text.Length == 0) ? string.Empty : ("{" + text + "}"));
		}
		else
		{
			group.Condition.Value = CodeEditorText;
		}
		group.SummaryType = (StiGroupSummaryType)((ListControl)comboBoxSummaryType).SelectedIndex;
		group.SummaryExpression.Value = CodeEditorTextSummary;
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set("StiGroupEditorForm", "Spl", ((Control)CodeEditor.DictionaryControl).Width);
		StiSettings.Set("StiGroupEditorForm", "Summary", ((Control)CodeEditorSummary.DictionaryControl).Width);
		CodeEditor.NodesMemoryForEditor.SaveExpNodes((TreeView)(object)CodeEditor.DictionaryControl.tvData);
		CodeEditor.DesignerLoadNodes();
		StiSettings.Save();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_groups_grouping_conditions.htm");
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		timerMarker.Stop();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			((TreeView)treeViewDataColumn).ImageList = null;
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Expected O, but got Unknown
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Expected O, but got Unknown
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07be: Expected O, but got Unknown
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_099f: Expected O, but got Unknown
		//IL_0a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cf: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiConditionEditorForm));
		ilTool = new ImageList(components);
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		ilDataSet = new ImageList(components);
		pageExpression = new StiCloudTabulatorPage();
		CodeEditor = new StiCodeEditor();
		syntaxEdit1 = new SyntaxEdit(components);
		pageDataColumn = new StiCloudTabulatorPage();
		treeViewDataColumn = new StiTreeViewUX();
		pageSummary = new StiCloudTabulatorPage();
		CodeEditorSummary = new StiCodeEditor();
		syntaxEditSummary = new SyntaxEdit(components);
		TabControl = new StiCloudTabulator();
		comboBoxSummarySortDirection = new StiComboBox();
		comboBoxSortDirection = new StiComboBox();
		labelSummarySortDirection = new StiLabelUX();
		labelSortDirection = new StiLabelUX();
		panelButtons = new StiButtonPanelUX();
		stiGroupLine2 = new StiGroupPanelUX();
		comboBoxSummaryType = new StiComboBox();
		labelSummaryType = new StiLabelUX();
		stiGroupLine3 = new StiGroupPanelUX();
		timerMarker = new Timer(components);
		((Control)pageExpression).SuspendLayout();
		((Control)CodeEditor).SuspendLayout();
		((Control)pageDataColumn).SuspendLayout();
		((Control)pageSummary).SuspendLayout();
		((Control)CodeEditorSummary).SuspendLayout();
		((Control)TabControl).SuspendLayout();
		((Control)panelButtons).SuspendLayout();
		((Control)stiGroupLine2).SuspendLayout();
		((Control)stiGroupLine3).SuspendLayout();
		((Control)this).SuspendLayout();
		ilTool.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilTool.ImageStream");
		ilTool.TransparentColor = Color.Magenta;
		ilTool.Images.SetKeyName(0, "");
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(459, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 2;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(540, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 3;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		ilDataSet.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilDataSet.ImageStream");
		ilDataSet.TransparentColor = Color.Magenta;
		ilDataSet.Images.SetKeyName(0, "");
		ilDataSet.Images.SetKeyName(1, "");
		ilDataSet.Images.SetKeyName(2, "");
		ilDataSet.Images.SetKeyName(3, "");
		ilDataSet.Images.SetKeyName(4, "");
		((Control)pageExpression).Controls.Add((Control)(object)CodeEditor);
		pageExpression.Dock = (DockStyle)5;
		pageExpression.Image = (Image)componentResourceManager.GetObject("pageExpression.Image");
		pageExpression.InvisibleVisible = false;
		((Control)pageExpression).Location = new Point(0, 0);
		((Control)pageExpression).Name = "pageExpression";
		((Control)pageExpression).Padding = new Padding(4);
		((Control)pageExpression).Size = new Size(508, 189);
		((Control)pageExpression).TabIndex = 0;
		((Control)pageExpression).Text = "Expression";
		((Control)pageExpression).Visible = false;
		CodeEditor.AllowOutlining = false;
		CodeEditor.AllowSQLFormatting = false;
		((Control)CodeEditor).Anchor = (AnchorStyles)15;
		((Control)CodeEditor).Controls.Add((Control)(object)syntaxEdit1);
		CodeEditor.ForceShowAutoGeneratedCode = false;
		((Control)CodeEditor).ForeColor = SystemColors.ControlText;
		((Control)CodeEditor).Location = new Point(12, 12);
		((Control)CodeEditor).Name = "CodeEditor";
		((Control)CodeEditor).Padding = new Padding(1);
		CodeEditor.ReplaceMode = false;
		CodeEditor.ShowAutoGeneratedCode = false;
		CodeEditor.ShowInsertSymbol = false;
		CodeEditor.ShowLineNumbersInEditor = false;
		((Control)CodeEditor).Size = new Size(484, 164);
		((Control)CodeEditor).TabIndex = 4;
		CodeEditor.TextFont = null;
		((Control)syntaxEdit1).BackColor = SystemColors.Window;
		syntaxEdit1.BorderStyle = EditBorderStyle.FixedSingle;
		((Control)syntaxEdit1).Cursor = Cursors.IBeam;
		syntaxEdit1.DisabledBackColor = SystemColors.Control;
		syntaxEdit1.DisabledForeColor = SystemColors.GrayText;
		((Control)syntaxEdit1).Dock = (DockStyle)5;
		((Control)syntaxEdit1).Font = new Font("Courier New", 10f);
		syntaxEdit1.Gutter.Visible = false;
		((Control)syntaxEdit1).Location = new Point(1, 1);
		((Control)syntaxEdit1).Name = "syntaxEdit1";
		((Control)syntaxEdit1).Size = new Size(482, 162);
		((Control)syntaxEdit1).TabIndex = 0;
		((Control)syntaxEdit1).Text = "";
		syntaxEdit1.UseDefaultMenu = false;
		((Control)pageDataColumn).Controls.Add((Control)(object)treeViewDataColumn);
		pageDataColumn.Dock = (DockStyle)5;
		pageDataColumn.Image = (Image)componentResourceManager.GetObject("pageDataColumn.Image");
		pageDataColumn.InvisibleVisible = true;
		((Control)pageDataColumn).Location = new Point(0, 0);
		((Control)pageDataColumn).Name = "pageDataColumn";
		((Control)pageDataColumn).Padding = new Padding(4);
		((Control)pageDataColumn).Size = new Size(508, 189);
		((Control)pageDataColumn).TabIndex = 3;
		((Control)pageDataColumn).Text = "Data Column";
		((Control)pageDataColumn).Visible = false;
		((Control)treeViewDataColumn).Anchor = (AnchorStyles)15;
		((TreeView)treeViewDataColumn).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewDataColumn).HideSelection = false;
		((Control)treeViewDataColumn).Location = new Point(12, 12);
		((Control)treeViewDataColumn).Name = "treeViewDataColumn";
		((Control)treeViewDataColumn).Size = new Size(484, 164);
		((Control)treeViewDataColumn).TabIndex = 0;
		((TreeView)treeViewDataColumn).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		((Control)treeViewDataColumn).DoubleClick += This_OkClick;
		((Control)pageSummary).Controls.Add((Control)(object)CodeEditorSummary);
		pageSummary.Dock = (DockStyle)5;
		pageSummary.Image = (Image)componentResourceManager.GetObject("pageSummary.Image");
		pageSummary.InvisibleVisible = false;
		((Control)pageSummary).Location = new Point(0, 0);
		((Control)pageSummary).Name = "pageSummary";
		((Control)pageSummary).Padding = new Padding(4);
		((Control)pageSummary).Size = new Size(508, 189);
		((Control)pageSummary).TabIndex = 3;
		((Control)pageSummary).Text = "Summary";
		((Control)pageSummary).Visible = false;
		CodeEditorSummary.AllowOutlining = false;
		CodeEditorSummary.AllowSQLFormatting = false;
		((Control)CodeEditorSummary).Anchor = (AnchorStyles)15;
		((Control)CodeEditorSummary).Controls.Add((Control)(object)syntaxEditSummary);
		CodeEditorSummary.ForceShowAutoGeneratedCode = false;
		((Control)CodeEditorSummary).ForeColor = SystemColors.ControlText;
		((Control)CodeEditorSummary).Location = new Point(12, 12);
		((Control)CodeEditorSummary).Name = "CodeEditorSummary";
		((Control)CodeEditorSummary).Padding = new Padding(1);
		CodeEditorSummary.ReplaceMode = false;
		CodeEditorSummary.ShowAutoGeneratedCode = false;
		CodeEditorSummary.ShowInsertSymbol = false;
		CodeEditorSummary.ShowLineNumbersInEditor = false;
		((Control)CodeEditorSummary).Size = new Size(485, 165);
		((Control)CodeEditorSummary).TabIndex = 4;
		CodeEditorSummary.TextFont = null;
		((Control)syntaxEditSummary).BackColor = SystemColors.Window;
		syntaxEditSummary.BorderStyle = EditBorderStyle.None;
		((Control)syntaxEditSummary).Cursor = Cursors.IBeam;
		syntaxEditSummary.DisabledBackColor = SystemColors.Control;
		syntaxEditSummary.DisabledForeColor = SystemColors.GrayText;
		((Control)syntaxEditSummary).Dock = (DockStyle)5;
		((Control)syntaxEditSummary).Font = new Font("Courier New", 10f);
		syntaxEditSummary.Gutter.Visible = false;
		((Control)syntaxEditSummary).Location = new Point(1, 1);
		((Control)syntaxEditSummary).Name = "syntaxEditSummary";
		((Control)syntaxEditSummary).Size = new Size(483, 163);
		((Control)syntaxEditSummary).TabIndex = 0;
		((Control)syntaxEditSummary).Text = "";
		syntaxEditSummary.UseDefaultMenu = false;
		((Control)TabControl).Controls.Add((Control)(object)pageExpression);
		((Control)TabControl).Controls.Add((Control)(object)pageDataColumn);
		((Control)TabControl).Controls.Add((Control)(object)pageSummary);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(627, 189);
		((Control)TabControl).TabIndex = 0;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		comboBoxSummarySortDirection.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxSummarySortDirection).Location = new Point(197, 12);
		((Control)comboBoxSummarySortDirection).Name = "comboBoxSummarySortDirection";
		((Control)comboBoxSummarySortDirection).Size = new Size(192, 21);
		((Control)comboBoxSummarySortDirection).TabIndex = 1;
		comboBoxSortDirection.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxSortDirection).Location = new Point(197, 12);
		((Control)comboBoxSortDirection).Name = "comboBoxSortDirection";
		((Control)comboBoxSortDirection).Size = new Size(192, 21);
		((Control)comboBoxSortDirection).TabIndex = 1;
		((Control)labelSummarySortDirection).ForeColor = Color.DimGray;
		((Control)labelSummarySortDirection).Location = new Point(12, 12);
		((Control)labelSummarySortDirection).Name = "labelSummarySortDirection";
		((Control)labelSummarySortDirection).Size = new Size(179, 21);
		((Control)labelSummarySortDirection).TabIndex = 61;
		((Control)labelSummarySortDirection).Text = "Summary Sort Direction";
		((Label)labelSummarySortDirection).TextAlign = (ContentAlignment)16;
		((Control)labelSortDirection).ForeColor = Color.DimGray;
		((Control)labelSortDirection).Location = new Point(12, 12);
		((Control)labelSortDirection).Name = "labelSortDirection";
		((Control)labelSortDirection).Size = new Size(179, 23);
		((Control)labelSortDirection).TabIndex = 61;
		((Control)labelSortDirection).Text = "Sort Direction";
		((Label)labelSortDirection).TextAlign = (ContentAlignment)16;
		((Control)panelButtons).Controls.Add((Control)(object)btCancel);
		((Control)panelButtons).Controls.Add((Control)(object)btOk);
		((Control)panelButtons).Dock = (DockStyle)2;
		panelButtons.LinePosition = StiLinePosition.Top;
		panelButtons.LineStyle = StiLineStyle.Dot;
		((Control)panelButtons).Location = new Point(0, 312);
		((Control)panelButtons).Name = "panelButtons";
		((Control)panelButtons).Size = new Size(627, 46);
		((Control)panelButtons).TabIndex = 62;
		((Control)stiGroupLine2).Controls.Add((Control)(object)comboBoxSummaryType);
		((Control)stiGroupLine2).Controls.Add((Control)(object)labelSummaryType);
		((Control)stiGroupLine2).Controls.Add((Control)(object)comboBoxSummarySortDirection);
		((Control)stiGroupLine2).Controls.Add((Control)(object)labelSummarySortDirection);
		((Control)stiGroupLine2).Dock = (DockStyle)2;
		stiGroupLine2.LinePosition = StiLinePosition.Top;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(0, 189);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(627, 78);
		((Control)stiGroupLine2).TabIndex = 63;
		comboBoxSummaryType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxSummaryType).Location = new Point(197, 45);
		((Control)comboBoxSummaryType).Name = "comboBoxSummaryType";
		((Control)comboBoxSummaryType).Size = new Size(192, 21);
		((Control)comboBoxSummaryType).TabIndex = 63;
		((Control)labelSummaryType).ForeColor = Color.DimGray;
		((Control)labelSummaryType).Location = new Point(12, 45);
		((Control)labelSummaryType).Name = "labelSummaryType";
		((Control)labelSummaryType).Size = new Size(179, 21);
		((Control)labelSummaryType).TabIndex = 62;
		((Control)labelSummaryType).Text = "Summary Type";
		((Label)labelSummaryType).TextAlign = (ContentAlignment)16;
		((Control)stiGroupLine3).Controls.Add((Control)(object)comboBoxSortDirection);
		((Control)stiGroupLine3).Controls.Add((Control)(object)labelSortDirection);
		((Control)stiGroupLine3).Dock = (DockStyle)2;
		stiGroupLine3.LinePosition = StiLinePosition.Top;
		stiGroupLine3.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine3).Location = new Point(0, 267);
		((Control)stiGroupLine3).Name = "stiGroupLine3";
		((Control)stiGroupLine3).Size = new Size(627, 45);
		((Control)stiGroupLine3).TabIndex = 63;
		timerMarker.Enabled = true;
		timerMarker.Interval = 500;
		timerMarker.Tick += TimerMarker_Tick;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(627, 358);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Controls.Add((Control)(object)stiGroupLine2);
		((Control)this).Controls.Add((Control)(object)stiGroupLine3);
		((Control)this).Controls.Add((Control)(object)panelButtons);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(195, 185);
		((Control)this).Name = "StiConditionEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Condition";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Control)pageExpression).ResumeLayout(false);
		((Control)CodeEditor).ResumeLayout(false);
		((Control)CodeEditor).PerformLayout();
		((Control)pageDataColumn).ResumeLayout(false);
		((Control)pageSummary).ResumeLayout(false);
		((Control)CodeEditorSummary).ResumeLayout(false);
		((Control)CodeEditorSummary).PerformLayout();
		((Control)TabControl).ResumeLayout(false);
		((Control)panelButtons).ResumeLayout(false);
		((Control)stiGroupLine2).ResumeLayout(false);
		((Control)stiGroupLine3).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
