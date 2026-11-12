using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Dialogs.Design;

public class StiGridDataColumnEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private StiTreeView treeView;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)3;
	}

	internal void OnDoubleClick(object sender, EventArgs e)
	{
		if (((TreeView)treeView).SelectedNode == null)
		{
			edSvc.CloseDropDown();
		}
		if (((TreeView)treeView).SelectedNode.Text == StiLocalization.Get("Report", "No"))
		{
			edSvc.CloseDropDown();
		}
		if (((TreeView)treeView).SelectedNode.Tag is StiDataColumn)
		{
			edSvc.CloseDropDown();
		}
	}

	private void SelectNode(string[] strs, int index, TreeNodeCollection nodes, TreeView view)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		string text = strs[index];
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			string text2 = val.Text;
			text2 = StiNameValidator.CorrectName(text2);
			if (!(text2 == text))
			{
				continue;
			}
			if (val.Nodes.Count == 0)
			{
				view.SelectedNode = val;
				break;
			}
			if (!val.IsExpanded)
			{
				val.Expand();
			}
			SelectNode(strs, index + 1, val.Nodes, view);
			break;
		}
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		treeView = new StiTreeView();
		((TreeView)treeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		((Control)treeView).Width = 200;
		((Control)treeView).Height = 200;
		TreeNode val = new TreeNode(StiLocalization.Get("Report", "No"));
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		((TreeView)treeView).Nodes.Add(val);
		if (context.Instance is StiGridColumn stiGridColumn)
		{
			TreeNodeEx dataNode = new TreeNodeEx();
			dataBuilder.BuildDataSource(stiGridColumn.GridColumnCollection.GridControl.DataSource, ref dataNode, includeCalcColumn: true);
			((TreeView)treeView).Nodes.Add((TreeNode)(object)dataNode);
		}
		string[] strs = ((string)value).Split(new char[1] { '.' });
		SelectNode(strs, 0, ((TreeView)treeView).Nodes, (TreeView)(object)treeView);
		if (((TreeView)treeView).SelectedNode == null)
		{
			((TreeView)treeView).SelectedNode = ((TreeView)treeView).Nodes[0];
		}
		((TreeView)treeView).ImageList = StiDataBuilder.Images;
		((TreeView)treeView).BorderStyle = (BorderStyle)0;
		((Control)treeView).DoubleClick += OnDoubleClick;
		edSvc.DropDownControl((Control)(object)treeView);
		((TreeView)treeView).ImageList = null;
		if (((TreeView)treeView).SelectedNode == null)
		{
			return value;
		}
		if (((TreeView)treeView).SelectedNode.Text == StiLocalization.Get("Report", "No"))
		{
			return string.Empty;
		}
		if (((TreeView)treeView).SelectedNode.Tag is StiDataColumn)
		{
			return StiDataBuilder.GetColumnPathFromNode(((TreeView)treeView).SelectedNode, useRelationName: true);
		}
		return value;
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}
}
