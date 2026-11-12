using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Images.Office2013;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Editors;

public class StiSystemTextEditorForm : StiFormUX
{
	private const int TabIndexExpression = 0;

	private const int TabIndexDataColumn = 1;

	private const int TabIndexSystemVariables = 2;

	private const int TabIndexTotals = 3;

	private string lastCodeText;

	private bool locked;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private StiReport report;

	private Timer timerUpdate;

	private StiParser.StiParserException lastCheckerException;

	private Balloon balloonChecker;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageSystemVariables;

	private StiCloudTabulatorPage pageExpression;

	private StiCloudTabulatorPage pageSummary;

	private StiCloudTabulatorPage pageDataColumn;

	private StiOffice2013GroupBox groupBoxSummaryRunning;

	private StiGroupPanelUX separatorSystemVariable;

	private StiRadioButtonUX radioButtonPage;

	private StiRadioButtonUX radioButtonReport;

	private StiRadioButtonUX radioButtonColumn;

	private StiLabelUX labelDataColumn;

	private StiComboBox comboBoxDataBand;

	private StiTreeViewBox treeViewBoxDataColumn;

	private StiComboBox comboBoxSummaryFunction;

	private StiLabelUX labelDataBand;

	private StiLabelUX labelSummaryFunction;

	private CheckBox checkBoxCondition;

	private StiCodeEditor codeEditorExpression;

	private StiCodeEditor codeEditorSummary;

	private ImageList imageListTool;

	public StiButtonEdit buttonEditCondition;

	private StiDropDownBox comboBoxShowInsteadNullValues;

	private StiTreeViewUX treeViewSystemVariables;

	private StiRichTextBox richTextBoxInformation;

	private StiButtonPanelUX panelButtons;

	private StiDotSeparator dotSeparator;

	private StiText systemText;

	private ToolTip toolTip1;

	private StiGroupPanelUX stiGroupLine2;

	private StiButtonUX buttonCheck;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiTreeViewUX treeViewDataColumn;

	private StiLabelUX labelShowInsteadNullValues;

	private StiCheckBoxUX checkBoxRunningTotal;

	public StiSystemTextEditorForm(StiReport report, StiText systemText)
	{
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		locked = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		((Control)pageSummary).SizeChanged += delegate
		{
			((Control)groupBoxSummaryRunning).Width = ((Control)pageSummary).Width;
		};
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		((Control)treeViewSystemVariables).Height = ((Control)richTextBoxInformation).Bottom - ((Control)treeViewSystemVariables).Top;
		codeEditorExpression.DictionaryControl.CanBuildHtmlTags = true;
		codeEditorExpression.Init(report, StiCodeEditorMode.EditorExpression);
		codeEditorSummary.DictionaryControl.CanBuildHtmlTags = true;
		codeEditorSummary.Init(report, StiCodeEditorMode.Expression);
		this.report = report;
		this.systemText = systemText;
		((Control)comboBoxShowInsteadNullValues).Text = systemText.NullValue;
		Localize();
		DoTheming();
		DoScaling();
		((TreeView)treeViewSystemVariables).ImageList = StiDataBuilder.Images;
		Build(report, systemText);
		((Form)this).ShowIcon = false;
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		if (codeEditorExpression.DictionaryControl != null)
		{
			((Control)codeEditorExpression.DictionaryControl).Width = StiSettings.GetInt("StiSystemTextEditorForm", "Spl1", ((Control)codeEditorExpression.DictionaryControl).Width);
		}
		if (((Control)comboBoxSummaryFunction).Text.Length == 0)
		{
			((Control)comboBoxSummaryFunction).Text = "Sum";
		}
		if (string.IsNullOrEmpty(systemText.Text.Value))
		{
			TabControl.SelectedIndex = 0;
		}
		if (StiOptions.Designer.Editors.SystemTextEditor.HighlightTextInSystemTextEditor)
		{
			codeEditorExpression.Editor.Selection.SelectAll();
		}
		codeEditorSummary.TextFont = systemText.Font;
		codeEditorSummary.ShowInsertSymbol = true;
		((Control)codeEditorSummary.Editor).GotFocus += delegate
		{
			lastCodeText = ((Control)codeEditorSummary).Text;
		};
		((Control)codeEditorSummary.Editor).LostFocus += delegate
		{
			if (lastCodeText != ((Control)codeEditorSummary).Text)
			{
				lastCodeText = ((Control)codeEditorSummary).Text;
				ParseSummaryFunction(lastCodeText);
			}
		};
		codeEditorExpression.ShowInsertSymbol = true;
		codeEditorExpression.TextFont = systemText.Font;
		((Control)codeEditorExpression.Editor).Focus();
		locked = false;
		((TreeView)treeViewDataColumn).NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(TreeView_NodeMouseDoubleClick);
		((TreeView)treeViewSystemVariables).NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(TreeView_NodeMouseDoubleClick);
		lastCodeText = ((Control)codeEditorSummary).Text;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiText");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)pageDataColumn).Text = StiLocalization.Get("PropertyMain", "DataColumn");
		((Control)labelSummaryFunction).Text = Loc.GetCleaned("FormSystemTextEditor", "LabelSummaryFunction");
		((Control)labelDataBand).Text = Loc.GetCleaned("FormSystemTextEditor", "LabelDataBand");
		((Control)groupBoxSummaryRunning).Text = StiLocalization.Get("FormSystemTextEditor", "SummaryRunning");
		((Control)radioButtonReport).Text = StiLocalization.Get("FormSystemTextEditor", "SummaryRunningByReport");
		((Control)radioButtonPage).Text = StiLocalization.Get("FormSystemTextEditor", "SummaryRunningByPage");
		((Control)radioButtonColumn).Text = StiLocalization.Get("FormSystemTextEditor", "SummaryRunningByColumn");
		((Control)checkBoxCondition).Text = StiLocalization.Get("FormSystemTextEditor", "Condition");
		((Control)checkBoxRunningTotal).Text = StiLocalization.Get("FormSystemTextEditor", "RunningTotal");
		((Control)labelDataColumn).Text = Loc.GetCleaned("FormSystemTextEditor", "LabelDataColumn");
		((Control)pageSystemVariables).Text = StiLocalization.Get("FormSystemTextEditor", "pageSystemVariable");
		((Control)pageSummary).Text = StiLocalization.Get("FormSystemTextEditor", "pageSummary");
		((Control)pageExpression).Text = StiLocalization.Get("FormSystemTextEditor", "pageExpression");
		((Control)labelShowInsteadNullValues).Text = Loc.GetCleaned("FormSystemTextEditor", "LabelShowInsteadNullValues");
		((Control)buttonCheck).Text = StiLocalization.Get("Buttons", "Check");
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)richTextBoxInformation).ForeColor = StiUX.InputForegroundHint;
			((Control)richTextBoxInformation).BackColor = StiUX.Background;
		}
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageDataColumn.Image = StiReportDesignImages.Tasks.TextDataColumn(StiImageSize.Double);
		pageExpression.Image = StiReportDesignImages.Tasks.TextExpression(StiImageSize.Double);
		pageSummary.Image = StiReportDesignImages.Tasks.TextSummary(StiImageSize.Double);
		pageSystemVariables.Image = StiReportDesignImages.Tasks.TextSystemVariable(StiImageSize.Double);
		buttonEditCondition.Image = StiReportDesignImages.Actions.EditSmall();
		StiComboBox stiComboBox = comboBoxDataBand;
		right = (((Control)comboBoxSummaryFunction).Width = ((Control)treeViewBoxDataColumn).Width);
		((Control)stiComboBox).Width = right;
	}

	private bool BuildDataColumn(StiText systemText)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		((TreeView)treeViewDataColumn).ImageList = StiDataBuilder.Images;
		string text = systemText.Text.Value.Replace(" ", "");
		text = text.Replace("{", "");
		text = text.Replace("}", "");
		TreeNode val = new TreeNode(StiLocalization.Get("Report", "No"))
		{
			ImageKey = "Close",
			SelectedImageKey = "Close"
		};
		((TreeView)treeViewDataColumn).Nodes.Add(val);
		dataBuilder.Build(((TreeView)treeViewDataColumn).Nodes, systemText.Report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		bool flag = false;
		if (!string.IsNullOrEmpty(text))
		{
			string[] strs = text.Split(new char[1] { '.' });
			flag = SelectNode(strs, 0, ((TreeView)treeViewDataColumn).Nodes, (TreeView)(object)treeViewDataColumn);
		}
		if (!flag)
		{
			((TreeView)treeViewDataColumn).SelectedNode = ((TreeView)treeViewDataColumn).Nodes[0];
		}
		return flag;
	}

	private void Build(StiReport report, StiText systemText)
	{
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Expected O, but got Unknown
		string value = systemText.Text.Value;
		StiSystemTextType stiSystemTextType = systemText.Type;
		string text = StiExpressionPacker.PackExpression(value, report.Designer, useBraces: true);
		((Control)codeEditorExpression).Text = text;
		((Control)codeEditorSummary).Text = text;
		((ComboBox)comboBoxDataBand).Items.Clear();
		((Control)comboBoxDataBand).Text = "";
		((ComboBox)comboBoxDataBand).Items.Add((object)string.Empty);
		StiComponentsCollection stiComponentsCollection = report.GetCurrentPage().GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item is StiDataBand || item is StiGroupHeaderBand)
			{
				((ComboBox)comboBoxDataBand).Items.Add((object)item.Name);
			}
		}
		((Control)buttonEditCondition).Enabled = false;
		((Control)comboBoxSummaryFunction).Text = "";
		((ComboBox)comboBoxSummaryFunction).Items.Clear();
		foreach (StiAggregateFunctionService item2 in StiOptions.Services.AggregateFunctions.Where((StiAggregateFunctionService s) => s.ServiceEnabled))
		{
			((ComboBox)comboBoxSummaryFunction).Items.Add((object)item2.ServiceName);
		}
		if ((stiSystemTextType == StiSystemTextType.None || stiSystemTextType == StiSystemTextType.Totals) && value.Trim().StartsWith("{", StringComparison.InvariantCulture) && value.Trim().EndsWith("}", StringComparison.InvariantCulture))
		{
			int num = value.IndexOf('(');
			int num2 = value.LastIndexOf(')');
			if (num2 - num > 0)
			{
				string inputString = value.Substring(num + 1, num2 - num - 1);
				string[] array = StiExportUtils.SplitString(inputString, ',');
				int num3 = value.IndexOf("{", StringComparison.InvariantCulture);
				if (num3 != -1)
				{
					int num4 = value.IndexOf("(", num3, StringComparison.InvariantCulture);
					if (num4 - num3 - 1 > 0)
					{
						string text2 = value.Substring(num3 + 1, num4 - num3 - 1);
						foreach (StiAggregateFunctionService item3 in StiOptions.Services.AggregateFunctions.Where((StiAggregateFunctionService s) => s.ServiceEnabled))
						{
							string text3 = text2;
							bool flag = false;
							bool @checked = false;
							bool flag2 = false;
							if (text3.EndsWith("If", StringComparison.InvariantCulture))
							{
								text3 = text3.Substring(0, text3.Length - 2);
								flag2 = true;
							}
							if (text3.EndsWith("Running", StringComparison.InvariantCulture))
							{
								text3 = text3.Substring(0, text3.Length - 7);
								@checked = true;
							}
							if (text3.EndsWith("If", StringComparison.InvariantCulture))
							{
								text3 = text3.Substring(0, text3.Length - 2);
								flag2 = true;
							}
							if (item3.ServiceName == text3)
							{
								flag = true;
							}
							if ("c" + item3.ServiceName == text3)
							{
								flag = true;
							}
							if ("col" + item3.ServiceName == text3)
							{
								flag = true;
							}
							if (!flag)
							{
								continue;
							}
							((Control)comboBoxSummaryFunction).Text = item3.ServiceName;
							TabControl.SelectedIndex = 3;
							((RadioButton)radioButtonColumn).Checked = value.IndexOfInvariant("col" + item3.ServiceName) != -1;
							((RadioButton)radioButtonPage).Checked = value.IndexOfInvariant("c" + item3.ServiceName) != -1;
							checkBoxCondition.Checked = flag2;
							((CheckBox)checkBoxRunningTotal).Checked = @checked;
							if (item3.RecureParam)
							{
								int num5 = array.Length;
								if (flag2 && num5 > 0)
								{
									((Control)buttonEditCondition).Text = StiExpressionPacker.PackExpression(array[num5 - 1], report.Designer, useBraces: false);
									((Control)buttonEditCondition).Enabled = true;
									num5--;
								}
								if (text3 == "Count")
								{
									if (num5 == 1)
									{
										FillDataBandName(array[0]);
									}
									continue;
								}
								switch (num5)
								{
								case 1:
									FillDataBandName(array[0]);
									if (((ListControl)comboBoxDataBand).SelectedIndex == -1)
									{
										((Control)treeViewBoxDataColumn).Text = StiExpressionPacker.PackExpression(array[0], report.Designer, useBraces: false);
									}
									break;
								case 2:
									FillDataBandName(array[0]);
									((Control)treeViewBoxDataColumn).Text = StiExpressionPacker.PackExpression(array[1], report.Designer, useBraces: false);
									break;
								}
							}
							else if (array.Length == 1)
							{
								if (flag2)
								{
									((Control)buttonEditCondition).Text = StiExpressionPacker.PackExpression(array[0], report.Designer, useBraces: false);
								}
								else
								{
									((Control)comboBoxDataBand).Text = array[0];
								}
							}
							else if (array.Length == 2)
							{
								((Control)comboBoxDataBand).Text = array[0];
								((Control)buttonEditCondition).Text = StiExpressionPacker.PackExpression(array[1], report.Designer, useBraces: false);
							}
						}
					}
				}
			}
		}
		((TreeView)treeViewSystemVariables).Nodes.Clear();
		List<string> systemVariables = StiSystemVariablesHelper.GetSystemVariables(report);
		foreach (string item4 in systemVariables)
		{
			TreeNode val = new TreeNode(item4)
			{
				Tag = item4
			};
			string imageKey = (val.SelectedImageKey = "SystemVariable" + item4);
			val.ImageKey = imageKey;
			((TreeView)treeViewSystemVariables).Nodes.Add(val);
			if ("{" + item4 + "}" == value)
			{
				((TreeView)treeViewSystemVariables).SelectedNode = val;
				TabControl.SelectedIndex = 2;
				stiSystemTextType = StiSystemTextType.SystemVariables;
			}
		}
		if (BuildDataColumn(systemText))
		{
			TabControl.SelectedIndex = 1;
			return;
		}
		switch (stiSystemTextType)
		{
		case StiSystemTextType.SystemVariables:
			TabControl.SelectedIndex = 2;
			return;
		case StiSystemTextType.Totals:
			TabControl.SelectedIndex = 3;
			return;
		}
		TabControl.SelectedIndex = 0;
		text = StiExpressionPacker.PackExpression(value, report.Designer, useBraces: true);
		((Control)codeEditorExpression).Text = text;
		((Control)codeEditorSummary).Text = text;
	}

	private void ParseSummaryFunction(string text)
	{
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Expected O, but got Unknown
		StiSystemTextType stiSystemTextType = systemText.Type;
		((Control)buttonEditCondition).Enabled = false;
		((Control)comboBoxSummaryFunction).Text = "";
		((ListControl)comboBoxDataBand).SelectedIndex = -1;
		((Control)comboBoxDataBand).Text = string.Empty;
		if ((stiSystemTextType == StiSystemTextType.None || stiSystemTextType == StiSystemTextType.Totals) && text.Trim().StartsWithInvariant("{") && text.Trim().EndsWithInvariant("}"))
		{
			int num = text.IndexOf('(');
			int num2 = text.LastIndexOf(')');
			if (num2 - num > 0)
			{
				string inputString = text.Substring(num + 1, num2 - num - 1);
				string[] array = StiExportUtils.SplitString(inputString, ',');
				int num3 = text.IndexOf("{", StringComparison.InvariantCulture);
				if (num3 != -1)
				{
					int num4 = text.IndexOf("(", num3, StringComparison.InvariantCulture);
					if (num4 - num3 - 1 > 0)
					{
						string text2 = text.Substring(num3 + 1, num4 - num3 - 1);
						foreach (StiAggregateFunctionService item in StiOptions.Services.AggregateFunctions.Where((StiAggregateFunctionService s) => s.ServiceEnabled))
						{
							string text3 = text2;
							bool flag = false;
							bool @checked = false;
							bool flag2 = false;
							if (text3.EndsWith("If", StringComparison.InvariantCulture))
							{
								text3 = text3.Substring(0, text3.Length - 2);
								flag2 = true;
							}
							if (text3.EndsWith("Running", StringComparison.InvariantCulture))
							{
								text3 = text3.Substring(0, text3.Length - 7);
								@checked = true;
							}
							if (text3.EndsWith("If", StringComparison.InvariantCulture))
							{
								text3 = text3.Substring(0, text3.Length - 2);
								flag2 = true;
							}
							if (item.ServiceName == text3)
							{
								flag = true;
							}
							if ("c" + item.ServiceName == text3)
							{
								flag = true;
							}
							if ("col" + item.ServiceName == text3)
							{
								flag = true;
							}
							if (!flag)
							{
								continue;
							}
							((Control)comboBoxSummaryFunction).Text = item.ServiceName;
							((RadioButton)radioButtonColumn).Checked = text.IndexOf("col" + item.ServiceName, StringComparison.InvariantCulture) != -1;
							((RadioButton)radioButtonPage).Checked = text.IndexOf("c" + item.ServiceName, StringComparison.InvariantCulture) != -1;
							checkBoxCondition.Checked = flag2;
							((CheckBox)checkBoxRunningTotal).Checked = @checked;
							if (item.RecureParam)
							{
								int num5 = array.Length;
								if (flag2 && num5 > 0)
								{
									((Control)buttonEditCondition).Text = StiExpressionPacker.PackExpression(array[num5 - 1], report.Designer, useBraces: false);
									((Control)buttonEditCondition).Enabled = true;
									num5--;
								}
								if (text3 == "Count")
								{
									if (num5 == 1)
									{
										FillDataBandName(array[0]);
									}
									continue;
								}
								switch (num5)
								{
								case 1:
									FillDataBandName(array[0]);
									if (((ListControl)comboBoxDataBand).SelectedIndex == -1)
									{
										((Control)treeViewBoxDataColumn).Text = StiExpressionPacker.PackExpression(array[0], report.Designer, useBraces: false);
									}
									break;
								case 2:
									FillDataBandName(array[0]);
									((Control)treeViewBoxDataColumn).Text = StiExpressionPacker.PackExpression(array[1], report.Designer, useBraces: false);
									break;
								}
							}
							else if (array.Length == 1)
							{
								if (flag2)
								{
									((Control)buttonEditCondition).Text = StiExpressionPacker.PackExpression(array[0], report.Designer, useBraces: false);
								}
								else
								{
									((Control)comboBoxDataBand).Text = array[0];
								}
							}
							else if (array.Length == 2)
							{
								((Control)comboBoxDataBand).Text = array[0];
								((Control)buttonEditCondition).Text = StiExpressionPacker.PackExpression(array[1], report.Designer, useBraces: false);
							}
						}
					}
				}
			}
		}
		((TreeView)treeViewSystemVariables).SelectedNode = null;
		foreach (TreeNode node in ((TreeView)treeViewSystemVariables).Nodes)
		{
			TreeNode val = node;
			string text4 = (string)val.Tag;
			if ("{" + text4 + "}" == text)
			{
				((TreeView)treeViewSystemVariables).SelectedNode = val;
				stiSystemTextType = StiSystemTextType.SystemVariables;
				break;
			}
		}
		if (!BuildDataColumn(systemText) && stiSystemTextType != StiSystemTextType.SystemVariables && stiSystemTextType != 0)
		{
			((Control)codeEditorExpression).Text = text;
		}
	}

	private void FillDataBandName(string str)
	{
		int num = 0;
		foreach (string item in ((ComboBox)comboBoxDataBand).Items)
		{
			if (item == str || StiNameValidator.CorrectName(item, report) == str)
			{
				((ListControl)comboBoxDataBand).SelectedIndex = num;
			}
			num++;
		}
	}

	private string GetSummaryText()
	{
		string result = string.Empty;
		if (((Control)comboBoxSummaryFunction).Text != "")
		{
			string text = ((Control)comboBoxSummaryFunction).Text;
			if (((RadioButton)radioButtonPage).Checked)
			{
				text = "c" + text;
			}
			else if (((RadioButton)radioButtonColumn).Checked)
			{
				text = "col" + text;
			}
			if (checkBoxCondition.Checked)
			{
				text += "If";
			}
			if (((CheckBox)checkBoxRunningTotal).Checked)
			{
				text += "Running";
			}
			text += "(";
			if (((Control)comboBoxSummaryFunction).Text != "Count")
			{
				bool flag = false;
				if (((Control)comboBoxDataBand).Text.Length > 0)
				{
					text += StiNameValidator.CorrectName(((Control)comboBoxDataBand).Text, report);
					flag = true;
				}
				if (((Control)treeViewBoxDataColumn).Text.Length > 0)
				{
					text = text + (flag ? "," : "") + ((Control)treeViewBoxDataColumn).Text;
					flag = true;
				}
				if (checkBoxCondition.Checked)
				{
					text = text + (flag ? "," : "") + ((Control)buttonEditCondition).Text;
				}
			}
			else
			{
				bool flag2 = false;
				if (((Control)comboBoxDataBand).Text.Length > 0)
				{
					text += StiNameValidator.CorrectName(((Control)comboBoxDataBand).Text, report);
					flag2 = true;
				}
				if (checkBoxCondition.Checked)
				{
					text = text + (flag2 ? "," : "") + ((Control)buttonEditCondition).Text;
				}
			}
			text += ")";
			result = "{" + text + "}";
		}
		return result;
	}

	private TreeNode SelectNode(TreeNodeCollection nodes, string[] strs, int index)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		TreeNode val = null;
		foreach (TreeNode node in nodes)
		{
			TreeNode val2 = node;
			if ((val2.Tag is StiDataColumn || val2.Tag is StiDataSource) && StiDataBuilder.GetColumnPathFromNode(val2) == strs[index])
			{
				val = val2;
				break;
			}
		}
		if (val == null)
		{
			return null;
		}
		TreeNode val3 = SelectNode(val.Nodes, strs, ++index);
		return val3 ?? val;
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

	private void ExpandTreeNode(TreeNode node)
	{
		if (node is TreeNodeEx node2 && node.Tag != null)
		{
			dataBuilder.ExpandedNode(node2, includeCalcColumn: true);
		}
	}

	private string GetCurrentExpression()
	{
		if (TabControl.SelectedTab != pageExpression)
		{
			return ((Control)codeEditorSummary.Editor).Text;
		}
		return ((Control)codeEditorExpression.Editor).Text;
	}

	private Image GetCheckerImage()
	{
		bool flag = lastCheckerException != null;
		if (StiScale.Step == 1.5)
		{
			if (!flag)
			{
				return (Image)(object)DesignerOffice2013Resource.Message_x1_5;
			}
			return (Image)(object)DesignerOffice2013Resource.Error_x1_5;
		}
		if (StiScale.Step > 1.5)
		{
			if (!flag)
			{
				return (Image)(object)DesignerOffice2013Resource.Message_x2;
			}
			return (Image)(object)DesignerOffice2013Resource.Error_x2;
		}
		if (!flag)
		{
			return (Image)(object)DesignerOffice2013Resource.Message;
		}
		return (Image)(object)DesignerOffice2013Resource.Error;
	}

	private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (e.Node != null && e.Node.Bounds.Contains(((MouseEventArgs)e).Location))
		{
			This_OkClick(sender, null);
		}
	}

	private void TreeViewSystemVariables_AfterSelect(object sender, TreeViewEventArgs e)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		((Control)richTextBoxInformation).Visible = true;
		((Control)separatorSystemVariable).Visible = true;
		((Control)richTextBoxInformation).Text = string.Empty;
		((Control)treeViewSystemVariables).Height = ((Control)separatorSystemVariable).Top - ((Control)treeViewSystemVariables).Top - StiScale.XXI(12.0);
		string text = e.Node.Text;
		string systemVariableInfo = StiSystemVariablesHelper.GetSystemVariableInfo(text);
		if (systemVariableInfo != null)
		{
			((Control)richTextBoxInformation).Text = e.Node.Text + "\n";
			int length = ((Control)richTextBoxInformation).Text.Length;
			StiRichTextBox stiRichTextBox = richTextBoxInformation;
			((Control)stiRichTextBox).Text = ((Control)stiRichTextBox).Text + systemVariableInfo;
			((TextBoxBase)richTextBoxInformation).Select(0, length);
			((RichTextBox)richTextBoxInformation).SelectionFont = new Font("Arial", 8f, (FontStyle)1);
		}
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void TreeViewBoxDataColumn_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		((TreeView)e.TreeView).ImageList = StiDataBuilder.Images;
		((TreeView)e.TreeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		TreeNode val = new TreeNode(StiLocalization.Get("MainMenu", "menuViewQuickInfoNone"));
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		e.Nodes.Add(val);
		dataBuilder.Build(e.Nodes, report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		if (e.Nodes.Count <= 1)
		{
			return;
		}
		StiTreeViewBox stiTreeViewBox = sender as StiTreeViewBox;
		if (((Control)stiTreeViewBox).Text == StiLocalization.Get("MainMenu", "menuViewQuickInfoNone"))
		{
			((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
			return;
		}
		string[] strs = ((Control)stiTreeViewBox).Text.Split(new char[1] { '.' });
		if (!SelectNode(strs, 0, e.Nodes, (TreeView)(object)e.TreeView))
		{
			((TreeView)e.TreeView).SelectedNode = ((TreeView)e.TreeView).Nodes[0];
		}
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (TabControl.SelectedTab == pageExpression)
		{
			((Control)codeEditorExpression.Editor).Focus();
		}
		else if (TabControl.SelectedTab == pageSummary)
		{
			((Control)codeEditorSummary.Editor).Focus();
		}
	}

	private void This_Activated(object sender, EventArgs e)
	{
		((Control)codeEditorExpression.Editor).Focus();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_output_text_parameters_text_editor.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		systemText.Report.Designer.UndoRedoSave("Text Editor");
		systemText.Report.IsModified = true;
		if (TabControl.SelectedIndex == 3)
		{
			systemText.Text = StiExpressionPacker.UnPackExpression(GetSummaryText(), systemText.Report.Designer, useBraces: false);
			systemText.Type = StiSystemTextType.Totals;
		}
		else if (TabControl.SelectedIndex == 1)
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
			if (text.Length == 0)
			{
				systemText.Text = string.Empty;
			}
			else
			{
				systemText.Text = "{" + text + "}";
			}
			systemText.Type = StiSystemTextType.DataColumn;
		}
		else if (TabControl.SelectedIndex == 2)
		{
			List<string> systemVariables = StiSystemVariablesHelper.GetSystemVariables(report);
			string text2 = ((((TreeView)treeViewSystemVariables).SelectedNode != null) ? (((TreeView)treeViewSystemVariables).SelectedNode.Tag as string) : string.Empty);
			foreach (string item in systemVariables)
			{
				if (item == text2)
				{
					systemText.Text.Value = "{" + item + "}";
					break;
				}
			}
			systemText.Type = StiSystemTextType.SystemVariables;
		}
		else if (TabControl.SelectedIndex == 0)
		{
			systemText.Text.Value = StiExpressionPacker.UnPackExpression(((Control)codeEditorExpression).Text, systemText.Report.Designer, useBraces: true);
			systemText.Type = StiSystemTextType.Expression;
		}
		report.Designer.TextAliasesHash.Clear();
		systemText.NullValue = ((Control)comboBoxShowInsteadNullValues).Text;
		if (report.Designer.DesignerControl is StiDesignerControl)
		{
			((StiDesignerControl)report.Designer.DesignerControl).Builder.Redraw();
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set("StiSystemTextEditorForm", "Spl1", ((Control)codeEditorExpression.DictionaryControl).Width);
		StiSettings.Save();
		codeEditorExpression.NodesMemoryForEditor.SaveExpNodes((TreeView)(object)codeEditorExpression.DictionaryControl.tvData);
		codeEditorExpression.DesignerLoadNodes();
		if (codeEditorExpression.Editor.SearchDialog.Visible)
		{
			codeEditorExpression.Editor.SearchDialog.Close();
		}
	}

	private void ComboBoxSummaryFunction_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			((Control)codeEditorSummary).Text = GetSummaryText();
		}
	}

	private void ComboBoxDataBand_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			((Control)codeEditorSummary).Text = GetSummaryText();
		}
	}

	private void TreeViewBoxDataColumn_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		TreeNode selectedNode = ((TreeView)e.TreeView).SelectedNode;
		if (selectedNode != null && selectedNode == ((TreeView)e.TreeView).Nodes[0])
		{
			e.Text = string.Empty;
		}
		else if (selectedNode != null && selectedNode.Tag is StiDataColumn)
		{
			string columnPathFromNode = StiDataBuilder.GetColumnPathFromNode(selectedNode);
			e.Text = StiExpressionPacker.PackExpression(columnPathFromNode, report.Designer, useBraces: false);
		}
		if (!locked)
		{
			if (timerUpdate == null)
			{
				timerUpdate = new Timer
				{
					Interval = 50
				};
				timerUpdate.Tick += TimerUpdateEditorSummaryText_Tick;
			}
			timerUpdate.Start();
		}
	}

	private void TimerUpdateEditorSummaryText_Tick(object sender, EventArgs e)
	{
		timerUpdate.Stop();
		((Control)codeEditorSummary).Text = GetSummaryText();
	}

	private void RadioButtons_CheckedChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			((Control)codeEditorSummary).Text = GetSummaryText();
		}
	}

	private void CheckBoxRunningTotal_CheckedChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			((Control)codeEditorSummary).Text = GetSummaryText();
		}
	}

	private void CheckBoxCondition_CheckedChanged(object sender, EventArgs e)
	{
		((Control)buttonEditCondition).Enabled = checkBoxCondition.Checked;
		if (!locked)
		{
			((Control)codeEditorSummary).Text = GetSummaryText();
		}
	}

	private void ButtonExpression_ButtonClick(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		StiButtonEdit stiButtonEdit = sender as StiButtonEdit;
		string text = ((Control)stiButtonEdit).Text;
		DialogResult val = StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorCode);
		if ((int)val == 1)
		{
			((Control)stiButtonEdit).Text = text;
			if (!locked)
			{
				((Control)codeEditorSummary).Text = GetSummaryText();
			}
		}
	}

	private void ButtonCheck_Click(object sender, EventArgs e)
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		string currentExpression = GetCurrentExpression();
		bool useAliases = report != null && report.Designer != null && report.Designer.UseAliases;
		lastCheckerException = StiParser.CheckExpression(currentExpression, systemText, useAliases);
		bool flag = lastCheckerException != null;
		Balloon balloon = new Balloon();
		((Form)balloon).Owner = (Form)(object)this;
		((Control)balloon).BackColor = Color.White;
		balloon.BorderColor = Color.FromArgb(153, 16, 16, 16);
		balloon.AntiAlias = true;
		balloon.AutoClose = true;
		balloon.AutoCloseTimeOut = 10;
		balloon.ShowCloseButton = true;
		((Form)balloon).ShowInTaskbar = false;
		balloon.TipPosition = eTipPosition.Bottom;
		((Control)balloon).Width = (flag ? 320 : 150);
		((Control)balloon).Height = 100;
		balloonChecker = balloon;
		PictureBox val = new PictureBox();
		((Control)val).MouseDown += (MouseEventHandler)delegate
		{
			((Form)balloonChecker).Close();
		};
		val.Image = GetCheckerImage();
		((Control)val).Left = 10;
		((Control)val).Top = 30;
		((Control)val).Width = 16;
		((Control)val).Height = 16;
		((Control)balloonChecker).Controls.Add((Control)(object)val);
		Label val2 = new Label();
		((Control)val2).Click += delegate
		{
			((Form)balloonChecker).Close();
		};
		((Control)val2).Left = 32;
		((Control)val2).Top = 30;
		((Control)val2).Width = (flag ? 264 : 94);
		((Control)val2).Height = 32;
		val2.TextAlign = (ContentAlignment)1;
		((Control)val2).Text = ((lastCheckerException != null) ? lastCheckerException.BaseMessage : "OK!");
		((Control)balloonChecker).Controls.Add((Control)(object)val2);
		Rectangle rectangle = ((Control)buttonCheck).RectangleToScreen(((Control)buttonCheck).ClientRectangle);
		((Form)balloonChecker).Location = new Point(rectangle.X - 4, rectangle.Y - StiScale.I(((Control)balloonChecker).Height) + StiScale.I9);
		balloonChecker.Show(balloonFocus: false);
		((Control)balloonChecker).Click += delegate
		{
			((Form)balloonChecker).Close();
		};
		if (lastCheckerException != null && lastCheckerException.Position > -1 && lastCheckerException.Position + lastCheckerException.Length < currentExpression.Length)
		{
			((Control)codeEditorExpression).Focus();
			codeEditorExpression.Editor.Selection.SelectionStart = lastCheckerException.Position;
			codeEditorExpression.Editor.Selection.SelectionLength = lastCheckerException.Length;
		}
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b23: Expected O, but got Unknown
		//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Expected O, but got Unknown
		//IL_0bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Expected O, but got Unknown
		//IL_0f27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ea: Expected O, but got Unknown
		//IL_1107: Unknown result type (might be due to invalid IL or missing references)
		//IL_1111: Expected O, but got Unknown
		//IL_139e: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSystemTextEditorForm));
		imageListTool = new ImageList(components);
		toolTip1 = new ToolTip(components);
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		panelButtons = new StiButtonPanelUX();
		buttonCheck = new StiButtonUX();
		TabControl = new StiCloudTabulator();
		pageExpression = new StiCloudTabulatorPage();
		codeEditorExpression = new StiCodeEditor();
		pageDataColumn = new StiCloudTabulatorPage();
		stiGroupLine2 = new StiGroupPanelUX();
		comboBoxShowInsteadNullValues = new StiDropDownBox();
		labelShowInsteadNullValues = new StiLabelUX();
		dotSeparator = new StiDotSeparator();
		treeViewDataColumn = new StiTreeViewUX();
		pageSystemVariables = new StiCloudTabulatorPage();
		treeViewSystemVariables = new StiTreeViewUX();
		separatorSystemVariable = new StiGroupPanelUX();
		richTextBoxInformation = new StiRichTextBox();
		pageSummary = new StiCloudTabulatorPage();
		codeEditorSummary = new StiCodeEditor();
		checkBoxRunningTotal = new StiCheckBoxUX();
		buttonEditCondition = new StiButtonEdit();
		comboBoxDataBand = new StiComboBox();
		treeViewBoxDataColumn = new StiTreeViewBox();
		comboBoxSummaryFunction = new StiComboBox();
		groupBoxSummaryRunning = new StiOffice2013GroupBox();
		radioButtonPage = new StiRadioButtonUX();
		radioButtonReport = new StiRadioButtonUX();
		radioButtonColumn = new StiRadioButtonUX();
		labelDataColumn = new StiLabelUX();
		labelDataBand = new StiLabelUX();
		labelSummaryFunction = new StiLabelUX();
		checkBoxCondition = new CheckBox();
		((Control)panelButtons).SuspendLayout();
		((Control)TabControl).SuspendLayout();
		((Control)pageExpression).SuspendLayout();
		((Control)pageDataColumn).SuspendLayout();
		((Control)stiGroupLine2).SuspendLayout();
		((Control)pageSystemVariables).SuspendLayout();
		((Control)pageSummary).SuspendLayout();
		((Control)groupBoxSummaryRunning).SuspendLayout();
		((Control)this).SuspendLayout();
		imageListTool.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListTool.ImageStream");
		imageListTool.TransparentColor = Color.Magenta;
		imageListTool.Images.SetKeyName(0, "");
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(475, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(556, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)panelButtons).Controls.Add((Control)(object)buttonOk);
		((Control)panelButtons).Controls.Add((Control)(object)buttonCheck);
		((Control)panelButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)panelButtons).Dock = (DockStyle)2;
		panelButtons.LinePosition = StiLinePosition.Top;
		panelButtons.LineStyle = StiLineStyle.Dot;
		((Control)panelButtons).Location = new Point(0, 417);
		((Control)panelButtons).Name = "panelButtons";
		((Control)panelButtons).Size = new Size(643, 46);
		((Control)panelButtons).TabIndex = 1;
		((Control)buttonCheck).Anchor = (AnchorStyles)7;
		((Control)buttonCheck).Location = new Point(12, 12);
		((Control)buttonCheck).Name = "buttonCheck";
		((Control)buttonCheck).Size = new Size(75, 24);
		((Control)buttonCheck).TabIndex = 2;
		((Control)buttonCheck).Text = "Check";
		((ButtonBase)buttonCheck).UseVisualStyleBackColor = true;
		((Control)buttonCheck).Click += ButtonCheck_Click;
		((Control)TabControl).Anchor = (AnchorStyles)15;
		((Control)TabControl).Controls.Add((Control)(object)pageExpression);
		((Control)TabControl).Controls.Add((Control)(object)pageDataColumn);
		((Control)TabControl).Controls.Add((Control)(object)pageSystemVariables);
		((Control)TabControl).Controls.Add((Control)(object)pageSummary);
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Margin = new Padding(0, 3, 3, 3);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(638, 414);
		((Control)TabControl).TabIndex = 0;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageExpression).Controls.Add((Control)(object)codeEditorExpression);
		pageExpression.Dock = (DockStyle)5;
		pageExpression.Image = (Image)componentResourceManager.GetObject("pageExpression.Image");
		pageExpression.InvisibleVisible = false;
		((Control)pageExpression).Location = new Point(0, 0);
		((Control)pageExpression).Name = "pageExpression";
		((Control)pageExpression).Padding = new Padding(4);
		((Control)pageExpression).Size = new Size(519, 414);
		((Control)pageExpression).TabIndex = 2;
		((Control)pageExpression).Text = "Expression";
		((Control)pageExpression).Visible = false;
		codeEditorExpression.AllowOutlining = false;
		codeEditorExpression.AllowSQLFormatting = false;
		((Control)codeEditorExpression).Anchor = (AnchorStyles)15;
		codeEditorExpression.ForceShowAutoGeneratedCode = false;
		((Control)codeEditorExpression).Location = new Point(12, 12);
		((Control)codeEditorExpression).Name = "codeEditorExpression";
		((Control)codeEditorExpression).Padding = new Padding(1);
		codeEditorExpression.ReplaceMode = false;
		codeEditorExpression.ShowAutoGeneratedCode = false;
		codeEditorExpression.ShowInsertSymbol = false;
		codeEditorExpression.ShowLineNumbersInEditor = false;
		((Control)codeEditorExpression).Size = new Size(493, 389);
		((Control)codeEditorExpression).TabIndex = 0;
		codeEditorExpression.TextFont = null;
		((Control)pageDataColumn).Controls.Add((Control)(object)stiGroupLine2);
		((Control)pageDataColumn).Controls.Add((Control)(object)dotSeparator);
		((Control)pageDataColumn).Controls.Add((Control)(object)treeViewDataColumn);
		pageDataColumn.Dock = (DockStyle)5;
		pageDataColumn.Image = (Image)componentResourceManager.GetObject("pageDataColumn.Image");
		pageDataColumn.InvisibleVisible = false;
		((Control)pageDataColumn).Location = new Point(0, 0);
		((Control)pageDataColumn).Name = "pageDataColumn";
		((Control)pageDataColumn).Padding = new Padding(4);
		((Control)pageDataColumn).Size = new Size(519, 414);
		((Control)pageDataColumn).TabIndex = 3;
		((Control)pageDataColumn).Text = "Data Column";
		((Control)pageDataColumn).Visible = false;
		((Control)stiGroupLine2).Controls.Add((Control)(object)comboBoxShowInsteadNullValues);
		((Control)stiGroupLine2).Controls.Add((Control)(object)labelShowInsteadNullValues);
		((Control)stiGroupLine2).Dock = (DockStyle)2;
		stiGroupLine2.LinePosition = StiLinePosition.Top;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(4, 368);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(511, 42);
		((Control)stiGroupLine2).TabIndex = 4;
		comboBoxShowInsteadNullValues.DrawMode = (DrawMode)2;
		comboBoxShowInsteadNullValues.IsEditable = true;
		comboBoxShowInsteadNullValues.Items.AddRange(new object[6] { "", "-", "None", "Absent", "Empty", "N/A" });
		((Control)comboBoxShowInsteadNullValues).Location = new Point(225, 12);
		comboBoxShowInsteadNullValues.MaxDropDownItems = 20;
		((Control)comboBoxShowInsteadNullValues).Name = "comboBoxShowInsteadNullValues";
		comboBoxShowInsteadNullValues.SelectedIndex = -1;
		comboBoxShowInsteadNullValues.SelectedItem = null;
		comboBoxShowInsteadNullValues.SelectedValue = null;
		((Control)comboBoxShowInsteadNullValues).Size = new Size(193, 21);
		((Control)comboBoxShowInsteadNullValues).TabIndex = 2;
		((Control)labelShowInsteadNullValues).Location = new Point(12, 12);
		((Control)labelShowInsteadNullValues).Name = "labelShowInsteadNullValues";
		((Control)labelShowInsteadNullValues).Size = new Size(208, 21);
		((Control)labelShowInsteadNullValues).TabIndex = 1;
		((Control)labelShowInsteadNullValues).Text = "Show Instead Null Values";
		((Label)labelShowInsteadNullValues).TextAlign = (ContentAlignment)16;
		((Control)dotSeparator).Anchor = (AnchorStyles)14;
		((Control)dotSeparator).BackColor = Color.White;
		((Control)dotSeparator).Location = new Point(0, 376);
		((Control)dotSeparator).MaximumSize = new Size(0, 5);
		((Control)dotSeparator).MinimumSize = new Size(0, 5);
		((Control)dotSeparator).Name = "dotSeparator";
		((Control)dotSeparator).Size = new Size(0, 5);
		((Control)dotSeparator).TabIndex = 3;
		((Control)dotSeparator).Text = "stiDotSeparator1";
		((Control)treeViewDataColumn).Anchor = (AnchorStyles)15;
		((TreeView)treeViewDataColumn).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewDataColumn).HideSelection = false;
		((TreeView)treeViewDataColumn).LineColor = Color.FromArgb(74, 74, 74);
		((Control)treeViewDataColumn).Location = new Point(12, 12);
		((Control)treeViewDataColumn).Name = "treeViewDataColumn";
		((Control)treeViewDataColumn).Size = new Size(494, 339);
		((Control)treeViewDataColumn).TabIndex = 0;
		((TreeView)treeViewDataColumn).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		((Control)pageSystemVariables).Controls.Add((Control)(object)treeViewSystemVariables);
		((Control)pageSystemVariables).Controls.Add((Control)(object)separatorSystemVariable);
		((Control)pageSystemVariables).Controls.Add((Control)(object)richTextBoxInformation);
		pageSystemVariables.Dock = (DockStyle)5;
		pageSystemVariables.Image = (Image)componentResourceManager.GetObject("pageSystemVariables.Image");
		pageSystemVariables.InvisibleVisible = true;
		((Control)pageSystemVariables).Location = new Point(0, 0);
		((Control)pageSystemVariables).Name = "pageSystemVariables";
		((Control)pageSystemVariables).Padding = new Padding(4);
		((Control)pageSystemVariables).Size = new Size(519, 414);
		((Control)pageSystemVariables).TabIndex = 0;
		((Control)pageSystemVariables).Text = "SystemVariable";
		((Control)pageSystemVariables).Visible = false;
		((Control)treeViewSystemVariables).Anchor = (AnchorStyles)15;
		((TreeView)treeViewSystemVariables).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewSystemVariables).HideSelection = false;
		((TreeView)treeViewSystemVariables).LineColor = Color.FromArgb(74, 74, 74);
		((Control)treeViewSystemVariables).Location = new Point(12, 12);
		((Control)treeViewSystemVariables).Name = "treeViewSystemVariables";
		((Control)treeViewSystemVariables).Size = new Size(495, 291);
		((Control)treeViewSystemVariables).TabIndex = 0;
		((TreeView)treeViewSystemVariables).AfterSelect += new TreeViewEventHandler(TreeViewSystemVariables_AfterSelect);
		((Control)separatorSystemVariable).Anchor = (AnchorStyles)14;
		separatorSystemVariable.LinePosition = StiLinePosition.Bottom;
		separatorSystemVariable.LineStyle = StiLineStyle.Dot;
		((Control)separatorSystemVariable).Location = new Point(4, 316);
		((Control)separatorSystemVariable).Name = "separatorSystemVariable";
		((Control)separatorSystemVariable).Size = new Size(511, 3);
		((Control)separatorSystemVariable).TabIndex = 2;
		((Control)separatorSystemVariable).Visible = false;
		((Control)richTextBoxInformation).Anchor = (AnchorStyles)14;
		((Control)richTextBoxInformation).BackColor = SystemColors.Window;
		((TextBoxBase)richTextBoxInformation).BorderStyle = (BorderStyle)0;
		((Control)richTextBoxInformation).ForeColor = SystemColors.InfoText;
		((Control)richTextBoxInformation).Location = new Point(12, 333);
		((Control)richTextBoxInformation).Name = "richTextBoxInformation";
		((TextBoxBase)richTextBoxInformation).ReadOnly = true;
		richTextBoxInformation.SelectionAlignment = StiRtfSelectionAlign.Left;
		((Control)richTextBoxInformation).Size = new Size(495, 68);
		((Control)richTextBoxInformation).TabIndex = 1;
		((Control)richTextBoxInformation).Text = "";
		((Control)richTextBoxInformation).Visible = false;
		((Control)pageSummary).Controls.Add((Control)(object)codeEditorSummary);
		((Control)pageSummary).Controls.Add((Control)(object)checkBoxRunningTotal);
		((Control)pageSummary).Controls.Add((Control)(object)buttonEditCondition);
		((Control)pageSummary).Controls.Add((Control)(object)comboBoxDataBand);
		((Control)pageSummary).Controls.Add((Control)(object)treeViewBoxDataColumn);
		((Control)pageSummary).Controls.Add((Control)(object)comboBoxSummaryFunction);
		((Control)pageSummary).Controls.Add((Control)(object)groupBoxSummaryRunning);
		((Control)pageSummary).Controls.Add((Control)(object)labelDataColumn);
		((Control)pageSummary).Controls.Add((Control)(object)labelDataBand);
		((Control)pageSummary).Controls.Add((Control)(object)labelSummaryFunction);
		((Control)pageSummary).Controls.Add((Control)(object)checkBoxCondition);
		pageSummary.Dock = (DockStyle)5;
		pageSummary.Image = (Image)componentResourceManager.GetObject("pageSummary.Image");
		pageSummary.InvisibleVisible = true;
		((Control)pageSummary).Location = new Point(0, 0);
		((Control)pageSummary).Name = "pageSummary";
		((Control)pageSummary).Padding = new Padding(4);
		((Control)pageSummary).Size = new Size(519, 414);
		((Control)pageSummary).TabIndex = 1;
		((Control)pageSummary).Text = "Summary";
		((Control)pageSummary).Visible = false;
		codeEditorSummary.AllowOutlining = false;
		codeEditorSummary.AllowSQLFormatting = false;
		((Control)codeEditorSummary).Anchor = (AnchorStyles)13;
		codeEditorSummary.ForceShowAutoGeneratedCode = false;
		((Control)codeEditorSummary).Location = new Point(12, 12);
		((Control)codeEditorSummary).Name = "codeEditorSummary";
		((Control)codeEditorSummary).Padding = new Padding(1);
		codeEditorSummary.ReplaceMode = false;
		codeEditorSummary.ShowAutoGeneratedCode = false;
		codeEditorSummary.ShowInsertSymbol = false;
		codeEditorSummary.ShowLineNumbersInEditor = false;
		((Control)codeEditorSummary).Size = new Size(495, 66);
		((Control)codeEditorSummary).TabIndex = 0;
		codeEditorSummary.TextFont = null;
		((Control)checkBoxRunningTotal).Anchor = (AnchorStyles)13;
		((Control)checkBoxRunningTotal).Location = new Point(12, 322);
		((Control)checkBoxRunningTotal).Name = "checkBoxRunningTotal";
		((Control)checkBoxRunningTotal).Size = new Size(497, 18);
		((Control)checkBoxRunningTotal).TabIndex = 4;
		((Control)checkBoxRunningTotal).Text = "Running Total";
		((ButtonBase)checkBoxRunningTotal).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxRunningTotal).CheckedChanged += CheckBoxRunningTotal_CheckedChanged;
		((Control)buttonEditCondition).Anchor = (AnchorStyles)13;
		buttonEditCondition.ButtonBitmap = (Image)componentResourceManager.GetObject("buttonEditCondition.ButtonBitmap");
		buttonEditCondition.Flat = false;
		buttonEditCondition.Image = (Image)componentResourceManager.GetObject("buttonEditCondition.Image");
		((Control)buttonEditCondition).Location = new Point(12, 378);
		((Control)buttonEditCondition).Name = "buttonEditCondition";
		((Control)buttonEditCondition).Size = new Size(495, 21);
		((Control)buttonEditCondition).TabIndex = 6;
		buttonEditCondition.ButtonClick += ButtonExpression_ButtonClick;
		((Control)comboBoxDataBand).Anchor = (AnchorStyles)13;
		comboBoxDataBand.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxDataBand).Location = new Point(167, 125);
		((Control)comboBoxDataBand).Name = "comboBoxDataBand";
		((Control)comboBoxDataBand).Size = new Size(123, 21);
		((Control)comboBoxDataBand).TabIndex = 1;
		((ComboBox)comboBoxDataBand).SelectedIndexChanged += ComboBoxDataBand_SelectedIndexChanged;
		((Control)treeViewBoxDataColumn).Anchor = (AnchorStyles)13;
		treeViewBoxDataColumn.DropDownWidth = 400;
		treeViewBoxDataColumn.Flat = false;
		((Control)treeViewBoxDataColumn).Location = new Point(167, 158);
		treeViewBoxDataColumn.MaxDropDownHeight = 320;
		((Control)treeViewBoxDataColumn).Name = "treeViewBoxDataColumn";
		((Control)treeViewBoxDataColumn).Size = new Size(124, 21);
		((Control)treeViewBoxDataColumn).TabIndex = 2;
		treeViewBoxDataColumn.ValueChanged += TreeViewBoxDataColumn_ValueChanged;
		treeViewBoxDataColumn.TreeNodesFill += TreeViewBoxDataColumn_TreeNodesFill;
		((Control)comboBoxSummaryFunction).Anchor = (AnchorStyles)13;
		comboBoxSummaryFunction.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxSummaryFunction).Location = new Point(167, 92);
		((ComboBox)comboBoxSummaryFunction).MaxDropDownItems = 20;
		((Control)comboBoxSummaryFunction).Name = "comboBoxSummaryFunction";
		((Control)comboBoxSummaryFunction).Size = new Size(123, 21);
		((Control)comboBoxSummaryFunction).TabIndex = 0;
		((ComboBox)comboBoxSummaryFunction).SelectedIndexChanged += ComboBoxSummaryFunction_SelectedIndexChanged;
		((Control)groupBoxSummaryRunning).Controls.Add((Control)(object)radioButtonPage);
		((Control)groupBoxSummaryRunning).Controls.Add((Control)(object)radioButtonReport);
		((Control)groupBoxSummaryRunning).Controls.Add((Control)(object)radioButtonColumn);
		groupBoxSummaryRunning.HeaderHeight = 24;
		((Control)groupBoxSummaryRunning).Location = new Point(0, 194);
		((Control)groupBoxSummaryRunning).Name = "groupBoxSummaryRunning";
		((Control)groupBoxSummaryRunning).Padding = new Padding(0, 25, 0, 0);
		((Control)groupBoxSummaryRunning).Size = new Size(517, 122);
		((Control)groupBoxSummaryRunning).TabIndex = 3;
		((Control)groupBoxSummaryRunning).TabStop = false;
		((Control)groupBoxSummaryRunning).Text = "SummaryRunning";
		((Control)radioButtonPage).Location = new Point(12, 96);
		((Control)radioButtonPage).Name = "radioButtonPage";
		((Control)radioButtonPage).Size = new Size(120, 24);
		((Control)radioButtonPage).TabIndex = 2;
		((Control)radioButtonPage).Text = "Page";
		((ButtonBase)radioButtonPage).UseVisualStyleBackColor = false;
		((RadioButton)radioButtonPage).CheckedChanged += RadioButtons_CheckedChanged;
		((RadioButton)radioButtonReport).Checked = true;
		((Control)radioButtonReport).Location = new Point(12, 36);
		((Control)radioButtonReport).Name = "radioButtonReport";
		((Control)radioButtonReport).Size = new Size(120, 24);
		((Control)radioButtonReport).TabIndex = 0;
		((RadioButton)radioButtonReport).TabStop = true;
		((Control)radioButtonReport).Text = "Report";
		((ButtonBase)radioButtonReport).UseVisualStyleBackColor = false;
		((RadioButton)radioButtonReport).CheckedChanged += RadioButtons_CheckedChanged;
		((Control)radioButtonColumn).Location = new Point(12, 66);
		((Control)radioButtonColumn).Name = "radioButtonColumn";
		((Control)radioButtonColumn).Size = new Size(120, 24);
		((Control)radioButtonColumn).TabIndex = 1;
		((Control)radioButtonColumn).Text = "Column";
		((ButtonBase)radioButtonColumn).UseVisualStyleBackColor = false;
		((RadioButton)radioButtonColumn).CheckedChanged += RadioButtons_CheckedChanged;
		((Control)labelDataColumn).Location = new Point(12, 161);
		((Control)labelDataColumn).Name = "labelDataColumn";
		((Control)labelDataColumn).Size = new Size(144, 19);
		((Control)labelDataColumn).TabIndex = 2;
		((Control)labelDataColumn).Text = "Data Column";
		((Label)labelDataColumn).TextAlign = (ContentAlignment)16;
		((Control)labelDataBand).Location = new Point(12, 125);
		((Control)labelDataBand).Name = "labelDataBand";
		((Control)labelDataBand).Size = new Size(147, 21);
		((Control)labelDataBand).TabIndex = 2;
		((Control)labelDataBand).Text = "DataBand";
		((Label)labelDataBand).TextAlign = (ContentAlignment)16;
		((Control)labelSummaryFunction).Location = new Point(12, 92);
		((Control)labelSummaryFunction).Name = "labelSummaryFunction";
		((Control)labelSummaryFunction).Size = new Size(147, 21);
		((Control)labelSummaryFunction).TabIndex = 0;
		((Control)labelSummaryFunction).Text = "SummaryFunction";
		((Label)labelSummaryFunction).TextAlign = (ContentAlignment)16;
		((Control)checkBoxCondition).Anchor = (AnchorStyles)13;
		((Control)checkBoxCondition).Location = new Point(12, 347);
		((Control)checkBoxCondition).Name = "checkBoxCondition";
		((Control)checkBoxCondition).Size = new Size(495, 18);
		((Control)checkBoxCondition).TabIndex = 5;
		((Control)checkBoxCondition).Text = "Condition";
		((ButtonBase)checkBoxCondition).UseVisualStyleBackColor = false;
		checkBoxCondition.CheckedChanged += CheckBoxCondition_CheckedChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(643, 463);
		((Control)this).Controls.Add((Control)(object)panelButtons);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(659, 502);
		((Control)this).Name = "StiSystemTextEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Editor";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Activated += This_Activated;
		((Form)this).Closed += This_Closed;
		((Control)panelButtons).ResumeLayout(false);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageExpression).ResumeLayout(false);
		((Control)pageDataColumn).ResumeLayout(false);
		((Control)stiGroupLine2).ResumeLayout(false);
		((Control)pageSystemVariables).ResumeLayout(false);
		((Control)pageSummary).ResumeLayout(false);
		((Control)groupBoxSummaryRunning).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
