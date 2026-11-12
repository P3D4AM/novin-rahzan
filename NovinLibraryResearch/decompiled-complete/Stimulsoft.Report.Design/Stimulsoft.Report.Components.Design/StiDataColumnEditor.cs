using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components.Design;

public class StiDataColumnEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private StiTreeView treeView;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private bool includeCalcColumn;

	public override void PaintValue(PaintValueEventArgs e)
	{
		if (e.Value != null)
		{
			Rectangle bounds = e.Bounds;
			Graphics graphics = e.Graphics;
			graphics.FillRectangle(Brushes.WhiteSmoke, bounds);
			if (StiScale.Id == SystemScaleID.x1)
			{
				int width = StiDataBuilder.Images.ImageSize.Width;
				int height = StiDataBuilder.Images.ImageSize.Height;
				int x = bounds.X + (bounds.Width - width) / 2;
				int y = bounds.Y + (bounds.Height - height) / 2;
				Rectangle rectangle = new Rectangle(x, y, width, height);
				graphics.DrawImage(StiDataBuilder.Images.Images["DataColumn"], rectangle);
			}
			else
			{
				bounds = new Rectangle(bounds.X + 2, bounds.Y + (bounds.Height - StiScale.I5) / 2, bounds.Width - 4, StiScale.I5);
				graphics.FillRectangle(Brushes.White, bounds);
				bounds.Width--;
				bounds.Height--;
				graphics.DrawRectangle(Pens.Gray, bounds);
				graphics.DrawLine(Pens.Gray, bounds.X + StiScale.I2, bounds.Y + bounds.Height / 2, bounds.Right - StiScale.I2, bounds.Y + bounds.Height / 2);
			}
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		string text = strs[index];
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			string str = val.Text;
			if (val.Tag is StiDataRelation)
			{
				str = ((StiDataRelation)val.Tag).NameInSource;
			}
			str = StiNameValidator.CorrectName(str);
			if (str != text)
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
				if (val is TreeNodeEx && val.Tag != null)
				{
					dataBuilder.ExpandedNode((TreeNodeEx)(object)val, includeCalcColumn);
				}
			}
			SelectNode(strs, index + 1, val.Nodes, view);
			break;
		}
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		treeView = new StiTreeView();
		int height = StiScale.YYI(500.0);
		if (edSvc is Control)
		{
			Control topLevelControl = ((Control)edSvc).TopLevelControl;
			Form val = (Form)(object)((topLevelControl is Form) ? topLevelControl : null);
			if (val != null)
			{
				Screen val2 = Screen.FromControl((Control)(object)val);
				int num = val2.WorkingArea.Height / 2;
				if (num < StiScale.YYI(500.0))
				{
					height = num;
				}
			}
		}
		((Control)treeView).Width = StiScale.YYI(300.0);
		((Control)treeView).Height = height;
		TreeNode val3 = new TreeNode(StiLocalization.Get("Report", "No"));
		string imageKey = (val3.SelectedImageKey = "Close");
		val3.ImageKey = imageKey;
		((TreeView)treeView).Nodes.Add(val3);
		StiComponent stiComponent = context.Instance as StiComponent;
		StiDataBindingsCollection stiDataBindingsCollection = context.Instance as StiDataBindingsCollection;
		IStiSeriesParent stiSeriesParent = context.Instance as IStiSeriesParent;
		StiSeriesInteraction stiSeriesInteraction = context.Instance as StiSeriesInteraction;
		StiHierarchicalBand stiHierarchicalBand = context.Instance as StiHierarchicalBand;
		if (stiHierarchicalBand != null && !stiHierarchicalBand.IsDataSourceEmpty)
		{
			foreach (StiDataColumn column in stiHierarchicalBand.DataSource.Columns)
			{
				if (!(column is StiCalcDataColumn))
				{
					TreeNode val4 = new TreeNode(column.Name);
					imageKey = (val4.SelectedImageKey = "DataColumn");
					val4.ImageKey = imageKey;
					val4.Tag = column;
					((TreeView)treeView).Nodes.Add(val4);
				}
			}
		}
		else if (stiHierarchicalBand != null && !stiHierarchicalBand.IsBusinessObjectEmpty)
		{
			foreach (StiDataColumn column2 in stiHierarchicalBand.BusinessObject.Columns)
			{
				if (!(column2 is StiCalcDataColumn))
				{
					TreeNode val5 = new TreeNode(column2.Name);
					imageKey = (val5.SelectedImageKey = "DataColumn");
					val5.ImageKey = imageKey;
					val5.Tag = column2;
					((TreeView)treeView).Nodes.Add(val5);
				}
			}
		}
		else if (stiSeriesInteraction != null)
		{
			dataBuilder.Build(((TreeView)treeView).Nodes, (stiSeriesInteraction.GetReport() as StiReport).Dictionary, dictionaryDesign: true, includeCalcColumn: true);
			includeCalcColumn = true;
		}
		else if (stiComponent != null)
		{
			dataBuilder.Build(((TreeView)treeView).Nodes, stiComponent.Report.Dictionary, dictionaryDesign: true, includeCalcColumn: false);
			includeCalcColumn = false;
		}
		else if (stiDataBindingsCollection != null)
		{
			dataBuilder.Build(((TreeView)treeView).Nodes, ((StiComponent)stiDataBindingsCollection.Control).Report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
			includeCalcColumn = true;
		}
		else if (stiSeriesParent != null)
		{
			dataBuilder.Build(((TreeView)treeView).Nodes, stiSeriesParent.Parent.Report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
			includeCalcColumn = true;
		}
		string[] strs = ((string)value).Split(new char[1] { '.' });
		SelectNode(strs, 0, ((TreeView)treeView).Nodes, (TreeView)(object)treeView);
		if (((TreeView)treeView).SelectedNode == null)
		{
			((TreeView)treeView).SelectedNode = ((TreeView)treeView).Nodes[0];
		}
		((TreeView)treeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		((Control)treeView).DoubleClick += OnDoubleClick;
		((TreeView)treeView).ImageList = StiDataBuilder.Images;
		((TreeView)treeView).BorderStyle = (BorderStyle)0;
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
		if (!(((TreeView)treeView).SelectedNode.Tag is StiDataColumn stiDataColumn3))
		{
			return value;
		}
		if (stiHierarchicalBand != null)
		{
			return stiDataColumn3.Name;
		}
		return StiDataBuilder.GetColumnPathFromNode(((TreeView)treeView).SelectedNode, useRelationName: true);
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn);
		}
	}
}
