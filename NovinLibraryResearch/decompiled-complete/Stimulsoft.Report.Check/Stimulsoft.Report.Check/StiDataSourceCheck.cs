using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Check;

public abstract class StiDataSourceCheck : StiCheck
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	public override StiCheckObjectType ObjectType => StiCheckObjectType.DataSource;

	public override string ElementName
	{
		get
		{
			if (base.Element == null)
			{
				return null;
			}
			return ((StiDataSource)base.Element).Name;
		}
	}

	private TreeNode GetTreeNode(TreeNodeCollection nodes)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag == base.Element)
			{
				return val;
			}
			if (!val.IsExpanded)
			{
				val.Expand();
			}
			TreeNode treeNode = GetTreeNode(val.Nodes);
			if (treeNode != null)
			{
				return treeNode;
			}
			val.Collapse();
		}
		return null;
	}

	public override void CreatePreviewImage(out Image elementImage, out Image highlightedElementImage, bool useScale = false)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		int num = 300;
		int num2 = 300;
		if (useScale)
		{
			num *= 2;
			num2 *= 2;
		}
		StiDataSource stiDataSource = base.Element as StiDataSource;
		TreeView val = new TreeView();
		val.BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		val.BeforeCollapse += new TreeViewCancelEventHandler(TreeNode_Collapsed);
		val.Scrollable = false;
		val.ImageList = StiDataBuilder.Images;
		((Control)val).Width = num - 8;
		((Control)val).Height = num2 - 8;
		val.BorderStyle = (BorderStyle)0;
		dataBuilder.Build(val.Nodes, stiDataSource.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		Rectangle rectangle = Rectangle.Empty;
		TreeNode treeNode = GetTreeNode(val.Nodes);
		if (treeNode != null)
		{
			treeNode.EnsureVisible();
			rectangle = treeNode.Bounds;
			rectangle.X += 4;
			rectangle.Y += 4;
		}
		Bitmap val2 = new Bitmap(num, num2);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			val3.Clear(Color.White);
			((Control)val).DrawToBitmap(val2, new Rectangle(4, 4, ((Control)val).Width, ((Control)val).Height));
			val3.DrawRectangle(Pens.Gray, new Rectangle(0, 0, ((Image)val2).Width - 1, ((Image)val2).Height - 1));
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		elementImage = (Image)(object)val2;
		highlightedElementImage = null;
		if (rectangle.IsEmpty)
		{
			return;
		}
		object obj = ((Image)val2).Clone();
		Image val4 = (Image)((obj is Image) ? obj : null);
		Graphics val5 = Graphics.FromImage(val4);
		try
		{
			Pen val6 = new Pen(Color.FromArgb(2013200384));
			try
			{
				val6.Width = 3f;
				val5.DrawRectangle(val6, rectangle);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		highlightedElementImage = val4;
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void TreeNode_Collapsed(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx && e.Node.Tag != null)
		{
			dataBuilder.CollapsedNode(e.Node);
		}
	}
}
