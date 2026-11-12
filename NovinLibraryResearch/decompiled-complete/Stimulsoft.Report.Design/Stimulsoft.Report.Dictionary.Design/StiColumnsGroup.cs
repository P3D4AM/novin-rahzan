using System;
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
public class StiColumnsGroup : UserControl
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private IContainer components;

	public StiToolBar tbTool;

	private StiZoneContainerControl zsGroups;

	private StiButtonUX tbAddLevel;

	private string No => "[" + StiLocalization.Get("Report", "No") + "]";

	public StiDataSource DataSource { get; set; }

	public StiColumnsGroup()
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
		((Control)tbAddLevel).Text = StiLocalization.Get("FormBand", "AddGroup");
	}

	private void MeasureButtonSize()
	{
		Size size = TextRenderer.MeasureText(((Control)tbAddLevel).Text, ((Control)tbAddLevel).Font);
		((Control)tbAddLevel).Width = size.Width + StiScale.I20;
	}

	public string[] GetResults()
	{
		List<string> list = new List<string>();
		for (int num = ((ArrangedElementCollection)((Control)zsGroups).Controls).Count - 1; num >= 0; num--)
		{
			StiGroupZoneControl stiGroupZoneControl = ((Control)zsGroups).Controls[num] as StiGroupZoneControl;
			string text = ((Control)stiGroupZoneControl.comboBoxColumn).Text;
			if (text == No)
			{
				list.Add(string.Empty);
			}
			else
			{
				if (((Control)stiGroupZoneControl.cbOrder).Text == StiLocalization.Get("PropertyEnum", "StiSortDirectionDesc"))
				{
					text = "DESC" + text;
				}
				else if (((Control)stiGroupZoneControl.cbOrder).Text == StiLocalization.Get("PropertyEnum", "StiSortDirectionNone"))
				{
					text = "NONE" + text;
				}
				list.Add(text);
			}
		}
		return list.ToArray();
	}

	public void SetResults(string[] groups)
	{
		int num = 0;
		while (num < groups.Length)
		{
			AddLevel(groups[num++]);
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
		if (val != null)
		{
			return SelectNode(val, strs, ++index);
		}
		return node;
	}

	private void AddLevel()
	{
		AddLevel(No);
	}

	private void AddLevel(string column)
	{
		if (string.IsNullOrEmpty(column))
		{
			column = No;
		}
		StiGroupZoneControl stiGroupZoneControl = new StiGroupZoneControl();
		((ComboBox)stiGroupZoneControl.cbOrder).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSortDirectionAsc"));
		((ComboBox)stiGroupZoneControl.cbOrder).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSortDirectionDesc"));
		((ComboBox)stiGroupZoneControl.cbOrder).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSortDirectionNone"));
		((ListControl)stiGroupZoneControl.cbOrder).SelectedIndex = 0;
		if (column.StartsWith("DESC", StringComparison.InvariantCulture))
		{
			if (DataSource == null || DataSource.Columns[column] == null || DataSource.Columns[column.Substring(4)] != null)
			{
				((ListControl)stiGroupZoneControl.cbOrder).SelectedIndex = 1;
				column = column.Substring(4);
			}
		}
		else if (column.StartsWith("NONE", StringComparison.InvariantCulture))
		{
			((ListControl)stiGroupZoneControl.cbOrder).SelectedIndex = 2;
			column = column.Substring(4);
		}
		((Control)stiGroupZoneControl.comboBoxColumn).Text = column;
		stiGroupZoneControl.comboBoxColumn.ValueChanged += cbColumn_ValueChanged;
		stiGroupZoneControl.comboBoxColumn.TreeNodesFill += cbColumn_TreeNodesFill;
		((Control)zsGroups).Controls.Add((Control)(object)stiGroupZoneControl);
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void tbAddLevel_Click(object sender, EventArgs e)
	{
		AddLevel();
	}

	private void cbColumn_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
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
		dataBuilder.BuildDataSource(DataSource, ref dataNode, includeCalcColumn: false);
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

	private void cbColumn_ValueChanged(object sender, StiValueChangedEventArgs e)
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		tbTool = new StiToolBar();
		zsGroups = new StiZoneContainerControl();
		tbAddLevel = new StiButtonUX();
		((Control)tbTool).SuspendLayout();
		((Control)this).SuspendLayout();
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
		((Control)zsGroups).AllowDrop = true;
		((ScrollableControl)zsGroups).AutoScroll = true;
		((Control)zsGroups).BackColor = Color.White;
		((Control)zsGroups).Dock = (DockStyle)5;
		zsGroups.EmptyImage = null;
		zsGroups.EmptyText = null;
		((Control)zsGroups).Location = new Point(0, 47);
		((Control)zsGroups).Margin = new Padding(1);
		((Control)zsGroups).Name = "zsGroups";
		((Control)zsGroups).Padding = new Padding(2);
		zsGroups.ShowBorder = false;
		((Control)zsGroups).Size = new Size(512, 302);
		((Control)zsGroups).TabIndex = 0;
		((Control)tbAddLevel).Location = new Point(11, 11);
		((Control)tbAddLevel).Name = "tbAddLevel";
		((Control)tbAddLevel).Size = new Size(113, 24);
		((Control)tbAddLevel).TabIndex = 79;
		((Control)tbAddLevel).Text = "Add";
		((ButtonBase)tbAddLevel).UseVisualStyleBackColor = true;
		((Control)tbAddLevel).Click += tbAddLevel_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)zsGroups);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Control)this).Margin = new Padding(2);
		((Control)this).Name = "StiColumnsGroup";
		((Control)this).Size = new Size(512, 349);
		((Control)tbTool).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
