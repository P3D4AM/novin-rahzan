using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Interaction;

[ToolboxItem(false)]
public class StiInteractionSortControl : UserControl
{
	private StiComponent comp;

	private StiInteraction interaction;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private IContainer components;

	private Panel panelTop;

	private CheckBox checkBoxSortingEnabled;

	private StiOffice2013GroupBox groupBoxSortingColumn;

	internal StiTreeView treeViewDataColumn;

	internal bool IsNotEmpty
	{
		get
		{
			if (checkBoxSortingEnabled.Checked)
			{
				TreeNode selectedNode = ((TreeView)treeViewDataColumn).SelectedNode;
				return ((selectedNode != null) ? selectedNode.Tag : null) is StiDataColumn;
			}
			return false;
		}
	}

	public StiInteractionSortControl(StiComponent comp, StiInteraction interaction)
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.comp = comp;
			this.interaction = interaction;
			checkBoxSortingEnabled.Checked = interaction.SortingEnabled;
			Localize();
			Build();
		}
	}

	public void Localize()
	{
		((Control)checkBoxSortingEnabled).Text = StiLocalization.Get("PropertyMain", "SortingEnabled");
		((Control)groupBoxSortingColumn).Text = StiLocalization.Get("PropertyMain", "SortingColumn");
	}

	public void Save()
	{
		interaction.SortingEnabled = checkBoxSortingEnabled.Checked;
		string text = string.Empty;
		if (((TreeView)treeViewDataColumn).SelectedNode != null)
		{
			if (((TreeView)treeViewDataColumn).SelectedNode.Text == StiLocalization.Get("Report", "No"))
			{
				text = string.Empty;
			}
			else if (((TreeView)treeViewDataColumn).SelectedNode.Tag is StiDataColumn)
			{
				text = StiDataBuilder.GetDataBandColumnPathFromNode(((TreeView)treeViewDataColumn).SelectedNode, useRelationName: true);
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			interaction.SortingColumn = string.Empty;
		}
		else
		{
			interaction.SortingColumn = text;
		}
	}

	private void Build()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		((TreeView)treeViewDataColumn).ImageList = StiDataBuilder.Images;
		TreeNode val = new TreeNode(StiLocalization.Get("Report", "No"))
		{
			ImageKey = "Close",
			SelectedImageKey = "Close"
		};
		((TreeView)treeViewDataColumn).Nodes.Add(val);
		StiComponentsCollection stiComponentsCollection = comp.Report.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			StiDataBand stiDataBand = item as StiDataBand;
			if (stiDataBand != null && stiDataBand.DataSource != null)
			{
				TreeNodeEx dataNode = new TreeNodeEx();
				dataBuilder.BuildDataSource(stiDataBand.DataSource, ref dataNode, includeCalcColumn: true);
				((TreeNode)dataNode).Text = stiDataBand.ToString();
				((TreeNode)dataNode).Tag = stiDataBand;
				((TreeView)treeViewDataColumn).Nodes.Add((TreeNode)(object)dataNode);
			}
			if (stiDataBand != null && stiDataBand.BusinessObject != null)
			{
				TreeNodeEx dataNode2 = new TreeNodeEx();
				dataBuilder.BuildBusinessObject(stiDataBand.BusinessObject, ref dataNode2, includeCalcColumn: true);
				((TreeNode)dataNode2).Text = stiDataBand.ToString();
				((TreeNode)dataNode2).Tag = stiDataBand;
				((TreeView)treeViewDataColumn).Nodes.Add((TreeNode)(object)dataNode2);
			}
		}
		string text = interaction.SortingColumn;
		if (text.StartsWith("{"))
		{
			text = text.Remove(0, 1);
		}
		if (text.EndsWith("}"))
		{
			text = text.Remove(text.Length - 1, 1);
		}
		string[] strs = text.Split(new char[1] { '.' });
		SelectNode(strs, 0, ((TreeView)treeViewDataColumn).Nodes, (TreeView)(object)treeViewDataColumn);
		if (((TreeView)treeViewDataColumn).SelectedNode == null)
		{
			((TreeView)treeViewDataColumn).SelectedNode = ((TreeView)treeViewDataColumn).Nodes[0];
		}
		((Control)treeViewDataColumn).Focus();
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
			string str = ((val.Tag is StiDataRelation) ? ((StiDataRelation)val.Tag).Name : val.Text);
			str = StiNameValidator.CorrectName(str, comp?.Report);
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
			if (node.Tag is StiDataBand stiDataBand)
			{
				object tag = ((stiDataBand.DataSource == null) ? ((object)stiDataBand.BusinessObject) : ((object)stiDataBand.DataSource));
				dataBuilder.ExpandedNode(node2, includeCalcColumn: true, tag);
			}
			else
			{
				dataBuilder.ExpandedNode(node2, includeCalcColumn: true);
			}
		}
	}

	private void TreeViewDataColumn_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		ExpandTreeNode(e.Node);
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		panelTop = new Panel();
		checkBoxSortingEnabled = new CheckBox();
		groupBoxSortingColumn = new StiOffice2013GroupBox();
		treeViewDataColumn = new StiTreeView();
		((Control)panelTop).SuspendLayout();
		((Control)groupBoxSortingColumn).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelTop).Controls.Add((Control)(object)checkBoxSortingEnabled);
		((Control)panelTop).Dock = (DockStyle)1;
		((Control)panelTop).Location = new Point(0, 0);
		((Control)panelTop).Name = "panelTop";
		((Control)panelTop).Size = new Size(400, 46);
		((Control)panelTop).TabIndex = 6;
		((Control)checkBoxSortingEnabled).Location = new Point(12, 12);
		((Control)checkBoxSortingEnabled).Name = "checkBoxSortingEnabled";
		((Control)checkBoxSortingEnabled).Size = new Size(293, 24);
		((Control)checkBoxSortingEnabled).TabIndex = 0;
		((Control)checkBoxSortingEnabled).Text = "Sorting Enabled";
		((ButtonBase)checkBoxSortingEnabled).UseVisualStyleBackColor = true;
		((Control)groupBoxSortingColumn).Controls.Add((Control)(object)treeViewDataColumn);
		((Control)groupBoxSortingColumn).Dock = (DockStyle)5;
		groupBoxSortingColumn.HeaderHeight = 24;
		((Control)groupBoxSortingColumn).Location = new Point(0, 46);
		((Control)groupBoxSortingColumn).Name = "groupBoxSortingColumn";
		((Control)groupBoxSortingColumn).Size = new Size(400, 354);
		((Control)groupBoxSortingColumn).TabIndex = 7;
		((Control)groupBoxSortingColumn).TabStop = false;
		((Control)groupBoxSortingColumn).Text = "Sorting Column";
		((Control)treeViewDataColumn).Anchor = (AnchorStyles)15;
		((Control)treeViewDataColumn).BackColor = Color.White;
		((TreeView)treeViewDataColumn).BorderStyle = (BorderStyle)0;
		((Control)treeViewDataColumn).Location = new Point(12, 39);
		((Control)treeViewDataColumn).Name = "treeViewDataColumn";
		((Control)treeViewDataColumn).Size = new Size(371, 298);
		((Control)treeViewDataColumn).TabIndex = 0;
		((TreeView)treeViewDataColumn).BeforeExpand += new TreeViewCancelEventHandler(TreeViewDataColumn_BeforeExpand);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)groupBoxSortingColumn);
		((Control)this).Controls.Add((Control)(object)panelTop);
		((Control)this).Name = "StiInteractionSortControl";
		((Control)this).Size = new Size(400, 400);
		((Control)panelTop).ResumeLayout(false);
		((Control)groupBoxSortingColumn).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
