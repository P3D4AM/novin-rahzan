using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Report.Design.Ribbon;

namespace Stimulsoft.Report.Design.Styles;

[ToolboxItem(false)]
internal class StiStylesTreeView : TreeView
{
	public Image FolderIcon { get; private set; }

	public Image FolderIconOpen { get; private set; }

	public Image CollapsedIcon { get; private set; }

	public Image ExpandedIcon { get; private set; }

	protected override void OnDrawNode(DrawTreeNodeEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		e.DrawDefault = true;
		Graphics graphics = e.Graphics;
		graphics.SetClip(e.Bounds);
		CompositingQuality compositingQuality = graphics.CompositingQuality;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		graphics.CompositingQuality = (CompositingQuality)2;
		graphics.SmoothingMode = (SmoothingMode)2;
		graphics.TextRenderingHint = (TextRenderingHint)3;
		if (e.Node.IsSelected)
		{
			graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
		}
		else
		{
			graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
		}
		if (!e.Node.IsEditing)
		{
			((TreeView)this).OnDrawNode(e);
		}
		graphics.CompositingQuality = compositingQuality;
		graphics.SmoothingMode = smoothingMode;
		graphics.TextRenderingHint = textRenderingHint;
		e.Graphics.ResetClip();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		TreeNode nodeAt = ((TreeView)this).GetNodeAt(e.X, e.Y);
		if (nodeAt != null)
		{
			((TreeView)this).SelectedNode = nodeAt;
		}
	}

	public StiStylesTreeView()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((TreeView)this).DrawMode = (TreeViewDrawMode)2;
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonBackStageUserControl));
		FolderIcon = (Image)componentResourceManager.GetObject("pageOpen1.Image");
		FolderIconOpen = (Image)componentResourceManager.GetObject("pageOpen.Image");
		CollapsedIcon = (Image)componentResourceManager.GetObject("Collapsed");
		ExpandedIcon = (Image)componentResourceManager.GetObject("Expanded");
	}
}
