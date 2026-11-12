using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiColumnsFilters : UserControl
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private StiReport report;

	private IContainer components;

	public StiToolBar tbTool;

	public StiRadioButtonUX rbOr;

	public StiRadioButtonUX rbAnd;

	public StiCheckBoxUX cbFilterOn;

	public StiComboBox comboBoxFilterEngine;

	private StiLabelUX labelFilterEngine;

	private Panel panel1;

	private StiDotSeparatorVertical stiDotSeparatorVertical2;

	private StiDotSeparatorVertical stiDotSeparatorVertical1;

	private StiDotSeparator stiDotSeparator1;

	private StiZoneContainerControl zcFilters;

	private StiButtonUX tbAddLevel;

	[Browsable(false)]
	public StiDataSource DataSource { get; set; }

	[Browsable(false)]
	public StiBusinessObject BusinessObject { get; set; }

	internal StiReport Report
	{
		private get
		{
			if (report != null)
			{
				return report;
			}
			if (DataSource != null)
			{
				return DataSource.Dictionary.Report;
			}
			if (BusinessObject != null)
			{
				return BusinessObject.Dictionary.Report;
			}
			return null;
		}
		set
		{
			report = value;
		}
	}

	private string NoFilter => "[" + Loc.Get("Report", "No") + "]";

	internal StiZoneContainerControl ZoneContainerControl => zcFilters;

	public StiColumnsFilters()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			DoTheming();
			((ListControl)comboBoxFilterEngine).SelectedIndex = 0;
			CheckState();
			StiLabelUX stiLabelUX = labelFilterEngine;
			bool visible = (((Control)comboBoxFilterEngine).Visible = StiOptions.Designer.IsDeveloperDesigner);
			((Control)stiLabelUX).Visible = visible;
			MeasureButtonSize();
		}
	}

	private void Localize()
	{
		((Control)tbAddLevel).Text = StiLocalization.Get("FormBand", "AddFilter");
		((Control)rbAnd).Text = StiLocalization.Get("Report", "FilterAnd");
		((Control)rbOr).Text = StiLocalization.Get("Report", "FilterOr");
		((Control)cbFilterOn).Text = StiLocalization.Get("PropertyMain", "FilterOn");
		((Control)labelFilterEngine).Text = Loc.GetCleaned("PropertyMain", "FilterEngine");
		((ComboBox)comboBoxFilterEngine).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterEngineReportEngine"));
		((ComboBox)comboBoxFilterEngine).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterEngineSQLQuery"));
	}

	private void DoTheming()
	{
		((Control)panel1).BackColor = StiUX.Background;
		((Control)stiDotSeparator1).BackColor = StiUX.Background;
	}

	private void MeasureButtonSize()
	{
		Size size = TextRenderer.MeasureText(((Control)tbAddLevel).Text, ((Control)tbAddLevel).Font);
		((Control)tbAddLevel).Width = size.Width + StiScale.I20;
	}

	private void CheckState()
	{
		StiRadioButtonUX stiRadioButtonUX = rbAnd;
		bool enabled = (((Control)rbOr).Enabled = ((ArrangedElementCollection)((Control)zcFilters).Controls).Count > 1);
		((Control)stiRadioButtonUX).Enabled = enabled;
		((Control)cbFilterOn).Enabled = ((ArrangedElementCollection)((Control)zcFilters).Controls).Count > 0;
	}

	public void FillFilters(StiFiltersCollection filters)
	{
		filters.Clear();
		foreach (StiFilterZoneControl item2 in (ArrangedElementCollection)((Control)zcFilters).Controls)
		{
			StiFilterItem item = StiFilterItem.Value;
			StiFilterCondition stiFilterCondition = StiFilterCondition.EqualTo;
			string value = ((Control)item2.tbValue1).Text;
			string value2 = ((Control)item2.tbValue2).Text;
			if (((Control)item2.comboBoxBoolean).Visible)
			{
				value = ((Control)item2.comboBoxBoolean).Text;
			}
			string expression = "{" + ((Control)item2.tbExpression).Text + "}";
			StiFilterDataType stiFilterDataType = StiFilterDataType.String;
			if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemValue"))
			{
				item = StiFilterItem.Value;
				expression = string.Empty;
			}
			else if (((Control)item2.cbItem).Text == StiLocalization.Get("PropertyEnum", "StiFilterItemExpression"))
			{
				item = StiFilterItem.Expression;
				value = (value2 = string.Empty);
			}
			stiFilterCondition = StiFilterHelper.ConvertStringToCondition(((Control)item2.cbCondition).Text);
			if (((Control)item2.cbCondition).Text != StiLocalization.Get("PropertyEnum", "StiFilterConditionBetween") && ((Control)item2.cbCondition).Text != StiLocalization.Get("PropertyEnum", "StiFilterConditionNotBetween"))
			{
				value2 = string.Empty;
			}
			stiFilterDataType = StiFilterHelper.ConvertStringToDataType(((Control)item2.cbDataType).Text);
			if (stiFilterDataType == StiFilterDataType.DateTime)
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				try
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
					value = item2.dtpValue1.Value.ToShortDateString();
					value2 = item2.dtpValue2.Value.ToShortDateString();
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = currentCulture;
				}
			}
			else
			{
				value = ((Control)item2.tbValue1).Text;
				if (((Control)item2.comboBoxBoolean).Visible)
				{
					value = ((Control)item2.comboBoxBoolean).Text;
				}
				value2 = ((Control)item2.tbValue2).Text;
			}
			string text = ((Control)item2.cbColumn).Text;
			if (text == NoFilter)
			{
				text = string.Empty;
			}
			filters.Insert(0, new StiFilter(item, text, stiFilterCondition, value, value2, stiFilterDataType, expression));
		}
	}

	public void BuildFilters(StiFiltersCollection filters)
	{
		((Control)zcFilters).Controls.Clear();
		foreach (StiFilter filter in filters)
		{
			string item = null;
			string column = filter.Column;
			string text = filter.Expression;
			string value = filter.Value1;
			string value2 = filter.Value2;
			text = text.Trim();
			if (text.StartsWithInvariant("{"))
			{
				text = text.Substring(1);
			}
			if (text.EndsWithInvariant("}"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			switch (filter.Item)
			{
			case StiFilterItem.Value:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemValue");
				break;
			case StiFilterItem.Expression:
				item = StiLocalization.Get("PropertyEnum", "StiFilterItemExpression");
				break;
			}
			string condition = StiFilterHelper.ConvertConditionToString(filter.Condition);
			string dataType = StiFilterHelper.ConvertDataTypeToString(filter.DataType);
			AddLevel(item, condition, column, value, value2, dataType, text);
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
			string str = ((val.Tag is StiDataRelation stiDataRelation) ? stiDataRelation.Name : val.Text);
			str = StiNameValidator.CorrectName(str, Report);
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

	private void AddLevel()
	{
		AddLevel(StiLocalization.Get("PropertyEnum", "StiFilterItemValue"), StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"), string.Empty, string.Empty, string.Empty, StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"), string.Empty);
	}

	private void AddLevel(string item, string condition, string column, string value1, string value2, string dataType, string expression)
	{
		StiFilterZoneControl stiFilterZoneControl = new StiFilterZoneControl(Report);
		((ComboBox)stiFilterZoneControl.cbItem).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemValue"));
		((ComboBox)stiFilterZoneControl.cbItem).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterItemExpression"));
		((Control)stiFilterZoneControl.cbItem).Text = item;
		if (string.IsNullOrEmpty(column))
		{
			column = NoFilter;
		}
		((Control)stiFilterZoneControl.cbColumn).Text = column;
		((Control)stiFilterZoneControl.tbExpression).Text = expression;
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBetween"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotBetween"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNull"));
		((ComboBox)stiFilterZoneControl.cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionIsNotNull"));
		((Control)stiFilterZoneControl.cbCondition).Text = condition;
		((ComboBox)stiFilterZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"));
		((ComboBox)stiFilterZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeNumeric"));
		((ComboBox)stiFilterZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"));
		((ComboBox)stiFilterZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"));
		((ComboBox)stiFilterZoneControl.cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeExpression"));
		((Control)stiFilterZoneControl.cbDataType).Text = dataType;
		if (dataType == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"))
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				stiFilterZoneControl.dtpValue1.Value = DateTime.Parse(value1);
				stiFilterZoneControl.dtpValue2.Value = DateTime.Parse(value2);
			}
			catch
			{
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		else
		{
			((Control)stiFilterZoneControl.tbValue1).Text = value1;
			((Control)stiFilterZoneControl.comboBoxBoolean).Text = value1;
			((Control)stiFilterZoneControl.tbValue2).Text = value2;
		}
		stiFilterZoneControl.cbColumn.ValueChanged += ComboBoxColumn_ValueChanged;
		stiFilterZoneControl.cbColumn.TreeNodesFill += ComboBoxColumn_TreeNodesFill;
		((Control)zcFilters).Controls.Add((Control)(object)stiFilterZoneControl);
		CheckState();
	}

	private void ExpandTreeNode(TreeNode node)
	{
		if (node is TreeNodeEx node2 && node.Tag != null)
		{
			dataBuilder.ExpandedNode(node2, includeCalcColumn: true);
		}
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		ExpandTreeNode(e.Node);
	}

	private void ButtonAddLevel_Click(object sender, EventArgs e)
	{
		AddLevel();
	}

	private void ComboBoxColumn_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		((TreeView)e.TreeView).ImageList = StiDataBuilder.Images;
		((TreeView)e.TreeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		TreeNode val = new TreeNode(NoFilter);
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		e.Nodes.Add(val);
		TreeNodeEx dataNode = new TreeNodeEx();
		if (DataSource != null)
		{
			dataBuilder.BuildDataSource(DataSource, ref dataNode, includeCalcColumn: false);
		}
		if (BusinessObject != null)
		{
			dataBuilder.BuildBusinessObjects(BusinessObject.Dictionary.BusinessObjects, (TreeNode)(object)dataNode, includeColumns: true, includeCalcColumns: false);
			for (int i = 0; i < ((TreeNode)dataNode).Nodes.Count; i++)
			{
				e.Nodes.Add(((TreeNode)dataNode).Nodes[i]);
			}
			dataNode = null;
		}
		if (dataNode == null || ((TreeNode)dataNode).Nodes.Count <= 0)
		{
			return;
		}
		e.Nodes.Add((TreeNode)(object)dataNode);
		StiTreeViewBox stiTreeViewBox = sender as StiTreeViewBox;
		if (((Control)stiTreeViewBox).Text == NoFilter)
		{
			((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
			return;
		}
		string[] strs = ((Control)stiTreeViewBox).Text.Split(new char[1] { '.' });
		TreeNode val2 = e.Nodes[1];
		if (val2.Nodes.Count == 1 && val2.Nodes[0].Text == "Loading")
		{
			ExpandTreeNode(val2);
		}
		if (!SelectNode(strs, 0, val2.Nodes, (TreeView)(object)e.TreeView))
		{
			((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
		}
	}

	private void ComboBoxColumn_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		if (!(e.Text != NoFilter))
		{
			return;
		}
		TreeNode selectedNode = ((TreeView)e.TreeView).SelectedNode;
		((TreeView)e.TreeView).ImageList = null;
		if (selectedNode != null)
		{
			if (!(selectedNode.Tag is StiDataColumn))
			{
				e.Text = NoFilter;
				return;
			}
			string columnPathFromNode = StiDataBuilder.GetColumnPathFromNode(selectedNode);
			int num = columnPathFromNode.IndexOfInvariant(".");
			e.Text = ((BusinessObject != null) ? columnPathFromNode : columnPathFromNode.Substring(num + 1));
		}
		else
		{
			e.Text = NoFilter;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		panel1 = new Panel();
		zcFilters = new StiZoneContainerControl();
		stiDotSeparator1 = new StiDotSeparator();
		stiDotSeparatorVertical2 = new StiDotSeparatorVertical();
		comboBoxFilterEngine = new StiComboBox();
		stiDotSeparatorVertical1 = new StiDotSeparatorVertical();
		labelFilterEngine = new StiLabelUX();
		cbFilterOn = new StiCheckBoxUX();
		rbAnd = new StiRadioButtonUX();
		rbOr = new StiRadioButtonUX();
		tbTool = new StiToolBar();
		tbAddLevel = new StiButtonUX();
		((Control)panel1).SuspendLayout();
		((Control)tbTool).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).Controls.Add((Control)(object)stiDotSeparatorVertical2);
		((Control)panel1).Controls.Add((Control)(object)comboBoxFilterEngine);
		((Control)panel1).Controls.Add((Control)(object)stiDotSeparatorVertical1);
		((Control)panel1).Controls.Add((Control)(object)labelFilterEngine);
		((Control)panel1).Controls.Add((Control)(object)cbFilterOn);
		((Control)panel1).Controls.Add((Control)(object)rbAnd);
		((Control)panel1).Controls.Add((Control)(object)rbOr);
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(0, 47);
		((Control)panel1).Margin = new Padding(2);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(544, 44);
		((Control)panel1).TabIndex = 35;
		((Control)zcFilters).AllowDrop = true;
		((ScrollableControl)zcFilters).AutoScroll = true;
		((Control)zcFilters).Dock = (DockStyle)5;
		zcFilters.EmptyImage = null;
		zcFilters.EmptyText = null;
		((Control)zcFilters).Location = new Point(0, 96);
		((Control)zcFilters).Margin = new Padding(1);
		((Control)zcFilters).Name = "zcFilters";
		((Control)zcFilters).Padding = new Padding(12, 12, 12, 0);
		zcFilters.ShowBorder = false;
		((Control)zcFilters).Size = new Size(544, 253);
		((Control)zcFilters).TabIndex = 37;
		((Control)stiDotSeparator1).Dock = (DockStyle)1;
		((Control)stiDotSeparator1).Location = new Point(0, 91);
		((Control)stiDotSeparator1).Margin = new Padding(2);
		((Control)stiDotSeparator1).MaximumSize = new Size(0, 5);
		((Control)stiDotSeparator1).MinimumSize = new Size(0, 5);
		((Control)stiDotSeparator1).Name = "stiDotSeparator1";
		((Control)stiDotSeparator1).Size = new Size(544, 5);
		((Control)stiDotSeparator1).TabIndex = 36;
		((Control)stiDotSeparator1).Text = "stiDotSeparator1";
		((Control)stiDotSeparatorVertical2).Location = new Point(267, 0);
		((Control)stiDotSeparatorVertical2).Margin = new Padding(2);
		((Control)stiDotSeparatorVertical2).MaximumSize = new Size(1, 0);
		((Control)stiDotSeparatorVertical2).MinimumSize = new Size(1, 0);
		((Control)stiDotSeparatorVertical2).Name = "stiDotSeparatorVertical2";
		((Control)stiDotSeparatorVertical2).Size = new Size(1, 0);
		((Control)stiDotSeparatorVertical2).TabIndex = 8;
		((Control)stiDotSeparatorVertical2).Text = "stiDotSeparatorVertical2";
		((Control)comboBoxFilterEngine).Anchor = (AnchorStyles)9;
		comboBoxFilterEngine.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxFilterEngine).FormattingEnabled = true;
		((Control)comboBoxFilterEngine).Location = new Point(390, 12);
		((Control)comboBoxFilterEngine).Margin = new Padding(2);
		((Control)comboBoxFilterEngine).Name = "comboBoxFilterEngine";
		((Control)comboBoxFilterEngine).Size = new Size(142, 21);
		((Control)comboBoxFilterEngine).TabIndex = 33;
		((Control)stiDotSeparatorVertical1).Location = new Point(134, 0);
		((Control)stiDotSeparatorVertical1).Margin = new Padding(2);
		((Control)stiDotSeparatorVertical1).MaximumSize = new Size(1, 0);
		((Control)stiDotSeparatorVertical1).MinimumSize = new Size(1, 0);
		((Control)stiDotSeparatorVertical1).Name = "stiDotSeparatorVertical1";
		((Control)stiDotSeparatorVertical1).Size = new Size(1, 0);
		((Control)stiDotSeparatorVertical1).TabIndex = 7;
		((Control)stiDotSeparatorVertical1).Text = "stiDotSeparatorVertical1";
		((Control)labelFilterEngine).Anchor = (AnchorStyles)9;
		((Control)labelFilterEngine).BackColor = Color.Transparent;
		((Control)labelFilterEngine).Location = new Point(254, 12);
		((Control)labelFilterEngine).Margin = new Padding(2, 0, 2, 0);
		((Control)labelFilterEngine).Name = "labelFilterEngine";
		((Control)labelFilterEngine).Size = new Size(130, 21);
		((Control)labelFilterEngine).TabIndex = 34;
		((Control)labelFilterEngine).Text = "Filter Engine";
		((Label)labelFilterEngine).TextAlign = (ContentAlignment)64;
		((Label)labelFilterEngine).UseCompatibleTextRendering = true;
		((Control)cbFilterOn).BackColor = Color.Transparent;
		((CheckBox)cbFilterOn).Checked = true;
		((CheckBox)cbFilterOn).CheckState = (CheckState)1;
		((Control)cbFilterOn).Location = new Point(141, 12);
		((Control)cbFilterOn).Margin = new Padding(2);
		((Control)cbFilterOn).Name = "cbFilterOn";
		((Control)cbFilterOn).Size = new Size(129, 21);
		((Control)cbFilterOn).TabIndex = 0;
		((Control)cbFilterOn).Text = "Filter On";
		((ButtonBase)cbFilterOn).UseVisualStyleBackColor = false;
		((Control)rbAnd).BackColor = Color.Transparent;
		((RadioButton)rbAnd).Checked = true;
		((Control)rbAnd).Location = new Point(12, 12);
		((Control)rbAnd).Margin = new Padding(2);
		((Control)rbAnd).Name = "rbAnd";
		((Control)rbAnd).Size = new Size(60, 21);
		((Control)rbAnd).TabIndex = 0;
		((RadioButton)rbAnd).TabStop = true;
		((Control)rbAnd).Text = "And";
		((ButtonBase)rbAnd).UseVisualStyleBackColor = false;
		((Control)rbOr).BackColor = Color.Transparent;
		((Control)rbOr).Location = new Point(76, 12);
		((Control)rbOr).Margin = new Padding(2);
		((Control)rbOr).Name = "rbOr";
		((Control)rbOr).Size = new Size(61, 21);
		((Control)rbOr).TabIndex = 1;
		((Control)rbOr).Text = "Or";
		((ButtonBase)rbOr).UseVisualStyleBackColor = false;
		((Control)tbTool).Controls.Add((Control)(object)tbAddLevel);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(0, 0);
		((Control)tbTool).Margin = new Padding(0);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(3);
		((Control)tbTool).Size = new Size(544, 47);
		((Control)tbTool).TabIndex = 0;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)tbAddLevel).Location = new Point(11, 11);
		((Control)tbAddLevel).Name = "tbAddLevel";
		((Control)tbAddLevel).Size = new Size(113, 24);
		((Control)tbAddLevel).TabIndex = 77;
		((Control)tbAddLevel).Text = "Add Filter";
		((ButtonBase)tbAddLevel).UseVisualStyleBackColor = true;
		((Control)tbAddLevel).Click += ButtonAddLevel_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)zcFilters);
		((Control)this).Controls.Add((Control)(object)stiDotSeparator1);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Control)this).Margin = new Padding(0);
		((Control)this).Name = "StiColumnsFilters";
		((Control)this).Size = new Size(544, 349);
		((Control)panel1).ResumeLayout(false);
		((Control)tbTool).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
