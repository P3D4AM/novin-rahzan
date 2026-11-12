using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiColumnsGroup : UserControl
{
	private TreeView selectedColumns;

	private IContainer components;

	public TreeView tvColumnsGroup;

	private ImageList ilData;

	public bool IsMasterDetailWizard { get; set; }

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

	public StiColumnsGroup()
	{
		InitializeComponent();
		if (StiOptions.Designer.AllowUseDragDrop)
		{
			((Control)tvColumnsGroup).AllowDrop = true;
		}
	}

	private void Build()
	{
		tvColumnsGroup.ShowRootLines = IsMasterDetailWizard;
		Hashtable strs = new Hashtable();
		BuildStrs(strs, tvColumnsGroup.Nodes);
		tvColumnsGroup.Nodes.Clear();
		BuildNodes(strs, SelectedColumns.Nodes, tvColumnsGroup.Nodes);
		tvColumnsGroup.ExpandAll();
		if (tvColumnsGroup.Nodes.Count > 0)
		{
			tvColumnsGroup.Nodes[0].EnsureVisible();
		}
	}

	private void BuildStrs(Hashtable strs, TreeNodeCollection nodes)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Checked)
			{
				strs[val.Tag] = val.Tag;
			}
			if (val.Nodes.Count > 0)
			{
				BuildStrs(strs, val.Nodes);
			}
		}
	}

	private void BuildNodes(Hashtable strs, TreeNodeCollection nodes, TreeNodeCollection destNodes)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			int num = ((val.Nodes.Count > 0) ? 1 : 0);
			TreeNode val2 = new TreeNode(val.Text, num, num)
			{
				Tag = val.Tag
			};
			destNodes.Add(val2);
			if (strs[val.Tag] != null)
			{
				val2.Checked = true;
			}
			if (val.Nodes.Count > 0)
			{
				BuildNodes(strs, val.Nodes, val2.Nodes);
			}
		}
	}

	private void TreeViewColumnsOrder_ItemDrag(object sender, ItemDragEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (e.Item is TreeNode && ((TreeNode)e.Item).Checked)
		{
			((Control)this).DoDragDrop(e.Item, (DragDropEffects)2);
		}
	}

	private void tvColumnsOrder_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		TreeNode selectedNode = tvColumnsGroup.SelectedNode;
		TreeNodeCollection nodes = tvColumnsGroup.Nodes;
		if (val.Parent != null)
		{
			nodes = val.Parent.Nodes;
		}
		if (val != selectedNode)
		{
			tvColumnsGroup.BeginUpdate();
			int num = nodes.IndexOf(selectedNode);
			nodes.Remove(val);
			nodes.Insert(num, val);
			tvColumnsGroup.SelectedNode = val;
			tvColumnsGroup.EndUpdate();
		}
	}

	private void TreeViewColumnsOrder_DragEnter(object sender, DragEventArgs e)
	{
		e.Effect = (DragDropEffects)2;
	}

	private void TreeViewColumnsOrder_DragOver(object sender, DragEventArgs e)
	{
		TreeNode val = tvColumnsGroup.GetNodeAt(((Control)tvColumnsGroup).PointToClient(Cursor.Position));
		if (val != null)
		{
			object data = e.Data.GetData(typeof(TreeNode));
			TreeNode val2 = (TreeNode)((data is TreeNode) ? data : null);
			if (val2.Parent != val.Parent || !val.Checked)
			{
				val = null;
			}
		}
		tvColumnsGroup.SelectedNode = val;
	}

	private void TreeViewColumnsGroup_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		if (e.Node.Nodes.Count > 0)
		{
			return;
		}
		TreeNodeCollection nodes = tvColumnsGroup.Nodes;
		if (e.Node.Parent != null)
		{
			nodes = e.Node.Parent.Nodes;
		}
		nodes.Remove(e.Node);
		int num = 0;
		foreach (TreeNode item in nodes)
		{
			TreeNode val = item;
			if (!val.Checked)
			{
				break;
			}
			num++;
		}
		nodes.Insert(num, e.Node);
		tvColumnsGroup.SelectedNode = e.Node;
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiColumnsGroup));
		tvColumnsGroup = new TreeView();
		ilData = new ImageList(components);
		((Control)this).SuspendLayout();
		tvColumnsGroup.BorderStyle = (BorderStyle)0;
		tvColumnsGroup.CheckBoxes = true;
		((Control)tvColumnsGroup).Dock = (DockStyle)5;
		tvColumnsGroup.HideSelection = false;
		tvColumnsGroup.ImageIndex = 0;
		tvColumnsGroup.ImageList = ilData;
		((Control)tvColumnsGroup).Location = new Point(0, 0);
		((Control)tvColumnsGroup).Name = "tvColumnsGroup";
		tvColumnsGroup.SelectedImageIndex = 0;
		tvColumnsGroup.ShowRootLines = false;
		((Control)tvColumnsGroup).Size = new Size(320, 336);
		((Control)tvColumnsGroup).TabIndex = 0;
		tvColumnsGroup.AfterCheck += new TreeViewEventHandler(TreeViewColumnsGroup_AfterCheck);
		tvColumnsGroup.ItemDrag += new ItemDragEventHandler(TreeViewColumnsOrder_ItemDrag);
		((Control)tvColumnsGroup).DragDrop += new DragEventHandler(tvColumnsOrder_DragDrop);
		((Control)tvColumnsGroup).DragEnter += new DragEventHandler(TreeViewColumnsOrder_DragEnter);
		((Control)tvColumnsGroup).DragOver += new DragEventHandler(TreeViewColumnsOrder_DragOver);
		ilData.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilData.ImageStream");
		ilData.TransparentColor = Color.Magenta;
		ilData.Images.SetKeyName(0, "");
		ilData.Images.SetKeyName(1, "");
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)tvColumnsGroup);
		((Control)this).Name = "StiColumnsGroup";
		((Control)this).Size = new Size(320, 336);
		((Control)this).ResumeLayout(false);
	}
}
