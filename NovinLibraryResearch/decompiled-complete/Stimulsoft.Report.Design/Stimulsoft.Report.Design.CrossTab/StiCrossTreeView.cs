using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.CrossTab;

[ToolboxItem(false)]
public class StiCrossTreeView : StiTreeView
{
	private TreeNode lastMouseOverNode;

	private TreeNode lastMouseOverCloseButtonNode;

	private const int WM_HSCROLL = 276;

	private const int WM_VSCROLL = 277;

	private const int WM_MOUSEWHEEL = 522;

	private const int TVS_NOHSCROLL = 32768;

	private bool isDragOverFromDictionary;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((TreeView)this).CreateParams;
			createParams.Style |= 0x8000;
			return createParams;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDragOverFromDictionary
	{
		get
		{
			return isDragOverFromDictionary;
		}
		set
		{
			if (isDragOverFromDictionary != value)
			{
				isDragOverFromDictionary = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BorderStyle BorderStyle
	{
		get
		{
			return (BorderStyle)0;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsVerticalScrollBarVisible => StiListBoxPainter.IsVerticalScrollbarVisible((Control)(object)this);

	public Rectangle GetNodeRect(TreeNode node)
	{
		int num = ((TreeView)(object)this).ScrollPosition().Y * ((TreeView)this).ItemHeight;
		Rectangle result = new Rectangle(2, node.Index * ((TreeView)this).ItemHeight - num, ((Control)this).Width - 4, ((TreeView)this).ItemHeight);
		if (IsVerticalScrollBarVisible)
		{
			result.Width -= SystemInformation.VerticalScrollBarWidth;
			if (node.Index == ((TreeView)this).Nodes.Count - 1)
			{
				result.Height -= 2;
			}
		}
		if (node.Index == 0)
		{
			result.Y += 2;
			result.Height -= 2;
		}
		return result;
	}

	public Rectangle GetCloseButtonRect(Rectangle rect)
	{
		return new Rectangle(rect.Right - StiListBoxPainter.CloseButtonWidth - 1, rect.Y, StiListBoxPainter.CloseButtonWidth, rect.Height);
	}

	public bool IsCloseButtonClicked(TreeNodeMouseClickEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		if ((((MouseEventArgs)e).Button & 0x100000) == 0)
		{
			return false;
		}
		Rectangle nodeRect = GetNodeRect(e.Node);
		Rectangle closeButtonRect = GetCloseButtonRect(nodeRect);
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		return closeButtonRect.Contains(pt);
	}

	protected override void WndProc(ref Message m)
	{
		base.WndProc(ref m);
		if (((Message)(ref m)).Msg == 277 || ((Message)(ref m)).Msg == 276 || ((Message)(ref m)).Msg == 522)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (((TreeView)this).Nodes.Count == 0)
		{
			StiListBoxPainter.PaintLabel(e.Graphics, ((Control)this).ClientRectangle, Loc.Get("Dashboard", "DragDropData"));
		}
		OnPaintItems(e.Graphics);
	}

	private void OnPaintItems(Graphics g)
	{
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(new Rectangle(2, 2, ((Control)this).Width - 4, ((Control)this).Height - 4));
			int num = ((TreeView)(object)this).ScrollPosition().Y * ((TreeView)this).ItemHeight;
			Rectangle bounds = new Rectangle(2, -num, ((Control)this).Width - 4, ((TreeView)this).ItemHeight);
			if (IsVerticalScrollBarVisible)
			{
				bounds.Width -= SystemInformation.VerticalScrollBarWidth;
			}
			for (int i = 0; i < ((TreeView)this).Nodes.Count; i++)
			{
				OnDrawNode(new StiDrawTreeNodeEventArgs(g, ((TreeView)this).Nodes[i], bounds));
				bounds.Y += ((TreeView)this).ItemHeight;
			}
		}
		finally
		{
			g.Restore(val);
		}
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		StiListBoxPainter.PaintBorder(e.Graphics, ((Control)this).ClientRectangle, ((TreeView)this).Nodes.Count, IsDragOverFromDictionary);
	}

	protected override void OnDrawNode(StiDrawTreeNodeEventArgs e)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		bool isSelected = e.Node.IsSelected;
		bool isHotLight = false;
		Color backgroundColor = StiListBoxPainter.GetBackgroundColor(isSelected, isHotLight, ((Control)this).Focused);
		Color foregroundColor = StiListBoxPainter.GetForegroundColor(isSelected, isHotLight, ((Control)this).Focused);
		Rectangle nodeRect = GetNodeRect(e.Node);
		StiDrawing.FillRectangle(graphics, backgroundColor, nodeRect);
		if (isSelected && ((Control)this).Focused && StiUX.IsDark)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), nodeRect);
		}
		TextFormatFlags val = (TextFormatFlags)32804;
		int i = StiScale.I4;
		nodeRect.X += i;
		nodeRect.Width -= i;
		TextRenderer.DrawText((IDeviceContext)(object)graphics, e.Node.Text, ((Control)this).Font, nodeRect, foregroundColor, val);
		OnPaintCloseButton(e.Graphics, nodeRect);
	}

	public void OnPaintCloseButton(Graphics g, Rectangle rect)
	{
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		if (!rect.Contains(pt))
		{
			return;
		}
		Image val = StiReportDesignImages.SystemParts.Remove(StiImageSize.Normal, allowCache: false);
		try
		{
			Rectangle closeButtonRect = GetCloseButtonRect(rect);
			Rectangle rectangle = new Rectangle(closeButtonRect.X + (closeButtonRect.Width - val.Width) / 2, closeButtonRect.Y + (closeButtonRect.Height - val.Height) / 2, val.Width, val.Height);
			bool flag = closeButtonRect.Contains(pt);
			closeButtonRect.Inflate(-StiScale.I3, -StiScale.I3);
			if (flag)
			{
				g.FillRectangle(Brushes.LightGray, closeButtonRect);
			}
			else
			{
				g.FillRectangle(Brushes.White, closeButtonRect);
			}
			g.DrawImage(val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((TreeView)this).OnMouseLeave(e);
		lastMouseOverNode = null;
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		TreeNode val = null;
		Point point = ((Control)this).PointToClient(Cursor.Position);
		TreeNode nodeAt = ((TreeView)this).GetNodeAt(point);
		if (nodeAt != null)
		{
			Rectangle nodeRect = GetNodeRect(nodeAt);
			if (GetCloseButtonRect(nodeRect).Contains(point))
			{
				val = nodeAt;
			}
		}
		if (nodeAt != lastMouseOverNode || val != lastMouseOverCloseButtonNode)
		{
			((Control)this).Invalidate();
		}
		lastMouseOverNode = nodeAt;
		lastMouseOverCloseButtonNode = val;
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		IsDragOverFromDictionary = false;
	}

	public StiCrossTreeView()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((TreeView)this).BorderStyle = (BorderStyle)0;
		((Control)this).AllowDrop = true;
	}
}
