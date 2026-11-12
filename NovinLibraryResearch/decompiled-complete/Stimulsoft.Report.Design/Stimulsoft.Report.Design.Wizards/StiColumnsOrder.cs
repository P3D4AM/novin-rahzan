using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiColumnsOrder : UserControl
{
	private List<string> dataSources = new List<string>();

	private TreeView selectedColumns;

	private IContainer components;

	private StiTreeView tvColumnsOrder;

	private ImageList ilData;

	private Button btUp;

	private Button btDown;

	private Panel panel1;

	public TreeView SelectedColumns
	{
		get
		{
			return selectedColumns;
		}
		set
		{
			selectedColumns = value;
			if (value != null)
			{
				Build();
			}
		}
	}

	public TreeView OrderedColumns => (TreeView)(object)tvColumnsOrder;

	public StiColumnsOrder()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((TreeView)tvColumnsOrder).SelectedNode = null;
			CheckState();
			DoScaling();
			if (StiOptions.Designer.AllowUseDragDrop)
			{
				((Control)tvColumnsOrder).AllowDrop = true;
			}
		}
	}

	internal void DoScaling()
	{
		((ButtonBase)btUp).Image = StiReportDesignImages.Actions.MoveUp();
		((ButtonBase)btDown).Image = StiReportDesignImages.Actions.MoveDown();
		ilData.ImageSize = new Size(StiScale.XXI(16.0), StiScale.YYI(16.0));
		ilData.Images.Clear();
		ilData.Images.Add(StiReportImages.Dictionary.DataColumn());
		ilData.Images.Add(StiReportImages.Dictionary.DataSource());
	}

	private void Build()
	{
		dataSources.Clear();
		List<TreeNode> newNodes = new List<TreeNode>();
		BuildNode(newNodes, selectedColumns.Nodes, string.Empty);
		if (dataSources.Count > 1)
		{
			BuildMultipleDataSources(newNodes);
		}
		else
		{
			BuildOneDataSource(newNodes);
		}
	}

	private void BuildMultipleDataSources(List<TreeNode> newNodes)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		((TreeView)tvColumnsOrder).ShowRootLines = dataSources.Count > 1;
		Hashtable hashtable = new Hashtable();
		foreach (TreeNode node in ((TreeView)tvColumnsOrder).Nodes)
		{
			TreeNode val = node;
			hashtable[val.Text] = val;
		}
		((TreeView)tvColumnsOrder).Nodes.Clear();
		foreach (string dataSource in dataSources)
		{
			TreeNodeCollection val2 = null;
			if (dataSources.Count > 1)
			{
				object? obj = hashtable[dataSource];
				TreeNode val3 = (TreeNode)((obj is TreeNode) ? obj : null);
				if (val3 == null)
				{
					TreeNode val4 = new TreeNode(dataSource, 1, 1)
					{
						Tag = dataSource
					};
					((TreeView)tvColumnsOrder).Nodes.Add(val4);
					foreach (TreeNode newNode in newNodes)
					{
						if (GetDataSourceName(newNode.Text) == dataSource)
						{
							if (dataSources.Count > 1)
							{
								newNode.Text = GetColumnName(newNode.Text);
							}
							_003F val5 = val4.Nodes;
							object obj2 = newNode.Clone();
							((TreeNodeCollection)val5).Add((TreeNode)((obj2 is TreeNode) ? obj2 : null));
						}
					}
					continue;
				}
				((TreeView)tvColumnsOrder).Nodes.Add(val3);
				val3.Expand();
				val2 = val3.Nodes;
			}
			else
			{
				val2 = ((TreeView)tvColumnsOrder).Nodes;
			}
			int num = 0;
			while (num < val2.Count)
			{
				TreeNode val6 = val2[num];
				bool flag = false;
				foreach (TreeNode newNode2 in newNodes)
				{
					if (dataSource + "." + val6.Text == newNode2.Text)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					((TreeView)tvColumnsOrder).Nodes.Remove(val6);
				}
				else
				{
					num++;
				}
			}
			foreach (TreeNode newNode3 in newNodes)
			{
				if (GetDataSourceName(newNode3.Text) != dataSource)
				{
					continue;
				}
				string text = newNode3.Text;
				if (dataSources.Count > 1)
				{
					text = GetColumnName(text);
				}
				bool flag2 = false;
				foreach (TreeNode item in val2)
				{
					TreeNode val7 = item;
					if (val7.Text == text)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					if (dataSources.Count > 1)
					{
						newNode3.Text = text;
					}
					val2.Add(newNode3);
				}
			}
		}
		((TreeView)tvColumnsOrder).ExpandAll();
		if (((TreeView)tvColumnsOrder).Nodes.Count > 0)
		{
			((TreeView)tvColumnsOrder).Nodes[0].EnsureVisible();
		}
	}

	private void BuildOneDataSource(List<TreeNode> newNodes)
	{
		int num = 0;
		while (num < ((TreeView)tvColumnsOrder).Nodes.Count)
		{
			TreeNode node2 = ((TreeView)tvColumnsOrder).Nodes[num];
			if (!newNodes.Any((TreeNode newNode) => node2.Text == newNode.Text))
			{
				((TreeView)tvColumnsOrder).Nodes.Remove(node2);
			}
			else
			{
				num++;
			}
		}
		foreach (TreeNode newNode2 in newNodes)
		{
			if (!((IEnumerable)((TreeView)tvColumnsOrder).Nodes).Cast<TreeNode>().Any((TreeNode node) => node.Text == newNode2.Text))
			{
				((TreeView)tvColumnsOrder).Nodes.Add(newNode2);
			}
		}
	}

	private string GetDataSourceName(string text)
	{
		int num = text.IndexOfInvariant(".");
		if (num != -1)
		{
			text = text.Substring(0, num);
		}
		num = text.IndexOfInvariant("[");
		if (text.IndexOfInvariant("]") != -1 && num > 0)
		{
			text = text.Substring(0, num - 1);
		}
		return text;
	}

	private string GetColumnName(string text)
	{
		int num = text.IndexOfInvariant(".");
		if (num == -1)
		{
			return text;
		}
		return text.Substring(num + 1);
	}

	private void BuildNode(List<TreeNode> newNodes, TreeNodeCollection nodes, string parentName)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Nodes.Count == 0 && val.Checked && val.Parent != null)
			{
				TreeNode val2 = ((parentName.Length <= 0) ? new TreeNode(val.Text, 0, 0) : new TreeNode(parentName + "." + val.Text, 0, 0));
				val2.Tag = StiDataBuilder.GetColumnPathFromNode(val);
				if (((string)val2.Tag).EndsWithInvariant("."))
				{
					val2.Tag = val2.Text;
				}
				newNodes.Add(val2);
				string dataSourceName = GetDataSourceName(val2.Text);
				if (dataSources.IndexOf(dataSourceName) == -1)
				{
					dataSources.Add(dataSourceName);
				}
			}
			else
			{
				string text = ((val.Tag is StiDataRelation) ? ((StiDataRelation)val.Tag).NameInSource : val.Text);
				if (parentName.Length > 0)
				{
					BuildNode(newNodes, val.Nodes, parentName + "." + text);
				}
				else
				{
					BuildNode(newNodes, val.Nodes, text);
				}
			}
		}
	}

	private void CheckState()
	{
		TreeNode selectedNode = ((TreeView)tvColumnsOrder).SelectedNode;
		if (selectedNode != null)
		{
			((Control)btUp).Enabled = selectedNode.Nodes.Count == 0 && selectedNode.PrevNode != null && selectedNode.PrevNode.Parent == selectedNode.Parent;
			((Control)btDown).Enabled = selectedNode.Nodes.Count == 0 && selectedNode.NextNode != null && selectedNode.NextNode.Parent == selectedNode.Parent;
		}
		else
		{
			((Control)btUp).Enabled = false;
			((Control)btDown).Enabled = false;
		}
	}

	private void TreeViewColumnsOrder_ItemDrag(object sender, ItemDragEventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (e.Item is TreeNode)
		{
			((Control)this).DoDragDrop(e.Item, (DragDropEffects)2);
		}
	}

	private void TreeViewColumnsOrder_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		TreeNode selectedNode = ((TreeView)tvColumnsOrder).SelectedNode;
		TreeNodeCollection nodes = ((TreeView)tvColumnsOrder).Nodes;
		if (val.Parent != null)
		{
			nodes = val.Parent.Nodes;
		}
		if (val != selectedNode)
		{
			((TreeView)tvColumnsOrder).BeginUpdate();
			int num = nodes.IndexOf(selectedNode);
			nodes.Remove(val);
			nodes.Insert(num, val);
			((TreeView)tvColumnsOrder).SelectedNode = val;
			((TreeView)tvColumnsOrder).EndUpdate();
		}
	}

	private void TreeViewColumnsOrder_DragEnter(object sender, DragEventArgs e)
	{
		e.Effect = (DragDropEffects)2;
	}

	private void TreeViewColumnsOrder_DragOver(object sender, DragEventArgs e)
	{
		TreeNode val = ((TreeView)tvColumnsOrder).GetNodeAt(((Control)tvColumnsOrder).PointToClient(Cursor.Position));
		if (val != null)
		{
			object data = e.Data.GetData(typeof(TreeNode));
			TreeNode val2 = (TreeNode)((data is TreeNode) ? data : null);
			if (val2.Parent != val.Parent)
			{
				val = null;
			}
		}
		((TreeView)tvColumnsOrder).SelectedNode = val;
	}

	private void ButtonUp_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)tvColumnsOrder).SelectedNode;
		if (selectedNode != null)
		{
			TreeNode parent = selectedNode.Parent;
			if (parent != null)
			{
				int num = parent.Nodes.IndexOf(selectedNode);
				parent.Nodes.Remove(selectedNode);
				parent.Nodes.Insert(num - 1, selectedNode);
			}
			else
			{
				int num2 = ((TreeView)tvColumnsOrder).Nodes.IndexOf(selectedNode);
				((TreeView)tvColumnsOrder).Nodes.Remove(selectedNode);
				((TreeView)tvColumnsOrder).Nodes.Insert(num2 - 1, selectedNode);
			}
			((TreeView)tvColumnsOrder).SelectedNode = selectedNode;
		}
	}

	private void ButtonDown_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)tvColumnsOrder).SelectedNode;
		if (selectedNode != null)
		{
			TreeNode parent = selectedNode.Parent;
			if (parent != null)
			{
				int num = parent.Nodes.IndexOf(selectedNode);
				parent.Nodes.Remove(selectedNode);
				parent.Nodes.Insert(num + 1, selectedNode);
			}
			else
			{
				int num2 = ((TreeView)tvColumnsOrder).Nodes.IndexOf(selectedNode);
				((TreeView)tvColumnsOrder).Nodes.Remove(selectedNode);
				((TreeView)tvColumnsOrder).Nodes.Insert(num2 + 1, selectedNode);
				((TreeView)tvColumnsOrder).SelectedNode = selectedNode;
			}
			((TreeView)tvColumnsOrder).SelectedNode = selectedNode;
		}
	}

	private void TreeViewColumnsOrder_AfterSelect(object sender, TreeViewEventArgs e)
	{
		CheckState();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiColumnsOrder));
		ilData = new ImageList(components);
		btUp = new Button();
		btDown = new Button();
		tvColumnsOrder = new StiTreeView();
		panel1 = new Panel();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		ilData.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilData.ImageStream");
		ilData.TransparentColor = Color.Magenta;
		ilData.Images.SetKeyName(0, "ColumnsOrder.png");
		ilData.Images.SetKeyName(1, "DataSource.png");
		((Control)btUp).Enabled = false;
		((ButtonBase)btUp).Image = (Image)componentResourceManager.GetObject("btUp.Image");
		((Control)btUp).Location = new Point(4, 4);
		((Control)btUp).Name = "btUp";
		((Control)btUp).Size = new Size(24, 24);
		((Control)btUp).TabIndex = 1;
		((Control)btUp).TabStop = false;
		((ButtonBase)btUp).UseVisualStyleBackColor = true;
		((Control)btUp).Click += ButtonUp_Click;
		((Control)btDown).Enabled = false;
		((ButtonBase)btDown).Image = (Image)componentResourceManager.GetObject("btDown.Image");
		((Control)btDown).Location = new Point(4, 28);
		((Control)btDown).Name = "btDown";
		((Control)btDown).Size = new Size(24, 24);
		((Control)btDown).TabIndex = 1;
		((Control)btDown).TabStop = false;
		((ButtonBase)btDown).UseVisualStyleBackColor = true;
		((Control)btDown).Click += ButtonDown_Click;
		((TreeView)tvColumnsOrder).BorderStyle = (BorderStyle)0;
		((Control)tvColumnsOrder).Dock = (DockStyle)5;
		((TreeView)tvColumnsOrder).HideSelection = false;
		((TreeView)tvColumnsOrder).ImageIndex = 0;
		((TreeView)tvColumnsOrder).ImageList = ilData;
		((Control)tvColumnsOrder).Location = new Point(0, 0);
		((Control)tvColumnsOrder).Name = "tvColumnsOrder";
		((TreeView)tvColumnsOrder).SelectedImageIndex = 0;
		((TreeView)tvColumnsOrder).ShowRootLines = false;
		((Control)tvColumnsOrder).Size = new Size(287, 336);
		((Control)tvColumnsOrder).TabIndex = 0;
		((TreeView)tvColumnsOrder).ItemDrag += new ItemDragEventHandler(TreeViewColumnsOrder_ItemDrag);
		((TreeView)tvColumnsOrder).AfterSelect += new TreeViewEventHandler(TreeViewColumnsOrder_AfterSelect);
		((Control)tvColumnsOrder).DragDrop += new DragEventHandler(TreeViewColumnsOrder_DragDrop);
		((Control)tvColumnsOrder).DragEnter += new DragEventHandler(TreeViewColumnsOrder_DragEnter);
		((Control)tvColumnsOrder).DragOver += new DragEventHandler(TreeViewColumnsOrder_DragOver);
		((Control)panel1).Controls.Add((Control)(object)btUp);
		((Control)panel1).Controls.Add((Control)(object)btDown);
		((Control)panel1).Dock = (DockStyle)4;
		((Control)panel1).Location = new Point(287, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(33, 336);
		((Control)panel1).TabIndex = 2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)tvColumnsOrder);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Name = "StiColumnsOrder";
		((Control)this).Size = new Size(320, 336);
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
