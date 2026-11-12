using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Dictionary.Design;

[ToolboxItem(false)]
public class StiColumnsResults : UserControl
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private IContainer components;

	public StiToolBar tbTool;

	private StiZoneContainerControl zcResults;

	private StiButtonUX tbAddLevel;

	public StiDataSource DataSource { get; set; }

	private string No => "[" + StiLocalization.Get("Report", "No") + "]";

	public StiColumnsResults()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			MeasureButtonSize();
		}
	}

	private void Localize()
	{
		((Control)tbAddLevel).Text = StiLocalization.Get("FormBand", "AddResult");
	}

	private void MeasureButtonSize()
	{
		Size size = TextRenderer.MeasureText(((Control)tbAddLevel).Text, ((Control)tbAddLevel).Font);
		((Control)tbAddLevel).Width = size.Width + StiScale.I20;
	}

	public string[] GetResults(Hashtable names)
	{
		List<string> list = new List<string>();
		for (int num = ((ArrangedElementCollection)((Control)zcResults).Controls).Count - 1; num >= 0; num--)
		{
			StiTotalsZoneControl stiTotalsZoneControl = ((Control)zcResults).Controls[num] as StiTotalsZoneControl;
			string text = ((Control)stiTotalsZoneControl.comboBoxColumn).Text;
			string text2 = ((Control)stiTotalsZoneControl.comboBoxFunction).Text;
			string text3 = ((Control)stiTotalsZoneControl.textBoxName).Text;
			int num2 = 2;
			if (text3 == "Count")
			{
				text3 += "1";
			}
			while (names[text3] != null)
			{
				text3 = ((Control)stiTotalsZoneControl.textBoxName).Text + num2++;
			}
			names[text3] = text3;
			list.Add((text == No) ? string.Empty : text);
			list.Add((text2 == No) ? string.Empty : text2);
			list.Add(text3);
		}
		return list.ToArray();
	}

	public void SetResults(string[] totals)
	{
		int num = 0;
		while (num < totals.Length)
		{
			AddLevel(totals[num++], totals[num++], totals[num++]);
		}
	}

	private TreeNode SelectNode(TreeNode node, string[] strs, int index)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		TreeNode val = null;
		foreach (TreeNode node2 in node.Nodes)
		{
			TreeNode val2 = node2;
			if (StiNameValidator.CorrectName(val2.Text, DataSource?.Dictionary?.Report) == strs[index])
			{
				val = val2;
				break;
			}
		}
		if (val == null)
		{
			return node;
		}
		if (!node.IsExpanded)
		{
			node.Expand();
		}
		return SelectNode(val, strs, ++index);
	}

	private void AddLevel()
	{
		AddLevel(No, No, string.Empty);
	}

	private void AddLevel(string column, string function, string name)
	{
		if (string.IsNullOrEmpty(function))
		{
			function = No;
		}
		if (string.IsNullOrEmpty(column))
		{
			column = No;
		}
		StiTotalsZoneControl stiTotalsZoneControl = new StiTotalsZoneControl();
		((Control)stiTotalsZoneControl.comboBoxColumn).Text = column;
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)No);
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"Sum");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"SumDistinct");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"Count");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"CountDistinct");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"Min");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"Max");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"MinDate");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"MaxDate");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"MinTime");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"MaxTime");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"MinStr");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"MaxStr");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"Median");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"Mode");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"Avg");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"First");
		((ComboBox)stiTotalsZoneControl.comboBoxFunction).Items.Add((object)"Last");
		((Control)stiTotalsZoneControl.comboBoxFunction).Text = function;
		((Control)stiTotalsZoneControl.textBoxName).Text = name;
		stiTotalsZoneControl.comboBoxColumn.ValueChanged += CbColumn_ValueChanged;
		stiTotalsZoneControl.comboBoxColumn.TreeNodesFill += CbColumn_TreeNodesFill;
		((Control)zcResults).Controls.Add((Control)(object)stiTotalsZoneControl);
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void TbAddLevel_Click(object sender, EventArgs e)
	{
		AddLevel();
	}

	private void CbColumn_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		TreeNode val = new TreeNode(No);
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		((TreeView)e.TreeView).ImageList = StiDataBuilder.Images;
		((TreeView)e.TreeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		e.Nodes.Add(val);
		TreeNodeEx dataNode = new TreeNodeEx();
		dataBuilder.BuildDataSource(DataSource, ref dataNode, includeCalcColumn: true);
		if (dataNode != null && ((TreeNode)dataNode).Nodes.Count > 0)
		{
			e.Nodes.Add((TreeNode)(object)dataNode);
			StiTreeViewBox stiTreeViewBox = sender as StiTreeViewBox;
			if (((Control)stiTreeViewBox).Text == No)
			{
				((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
				return;
			}
			string[] strs = ((Control)stiTreeViewBox).Text.Split(new char[1] { '.' });
			((TreeView)e.TreeView).SelectedNode = SelectNode(e.Nodes[1], strs, 0);
		}
	}

	private void CbColumn_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		if (!(e.Text != No))
		{
			return;
		}
		TreeNode selectedNode = ((TreeView)e.TreeView).SelectedNode;
		if (selectedNode != null)
		{
			if (!(selectedNode.Tag is StiDataColumn))
			{
				e.Text = No;
				return;
			}
			string columnPathFromNode = StiDataBuilder.GetColumnPathFromNode(selectedNode);
			int num = columnPathFromNode.IndexOf(".", StringComparison.InvariantCulture);
			e.Text = columnPathFromNode.Substring(num + 1);
		}
		else
		{
			e.Text = No;
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
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		zcResults = new StiZoneContainerControl();
		tbTool = new StiToolBar();
		tbAddLevel = new StiButtonUX();
		((Control)tbTool).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)zcResults).AllowDrop = true;
		((ScrollableControl)zcResults).AutoScroll = true;
		((Control)zcResults).BackColor = Color.White;
		((Control)zcResults).Dock = (DockStyle)5;
		zcResults.EmptyImage = null;
		zcResults.EmptyText = null;
		((Control)zcResults).Location = new Point(0, 47);
		((Control)zcResults).Margin = new Padding(1);
		((Control)zcResults).Name = "zcResults";
		((Control)zcResults).Padding = new Padding(2);
		zcResults.ShowBorder = false;
		((Control)zcResults).Size = new Size(512, 302);
		((Control)zcResults).TabIndex = 0;
		((Control)tbTool).Controls.Add((Control)(object)tbAddLevel);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(0, 0);
		((Control)tbTool).Margin = new Padding(2);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(3);
		((Control)tbTool).Size = new Size(512, 47);
		((Control)tbTool).TabIndex = 57;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)tbAddLevel).Location = new Point(11, 11);
		((Control)tbAddLevel).Name = "tbAddLevel";
		((Control)tbAddLevel).Size = new Size(113, 24);
		((Control)tbAddLevel).TabIndex = 79;
		((Control)tbAddLevel).Text = "Add";
		((ButtonBase)tbAddLevel).UseVisualStyleBackColor = true;
		((Control)tbAddLevel).Click += TbAddLevel_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)zcResults);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Control)this).Margin = new Padding(2);
		((Control)this).Name = "StiColumnsResults";
		((Control)this).Size = new Size(512, 349);
		((Control)tbTool).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
