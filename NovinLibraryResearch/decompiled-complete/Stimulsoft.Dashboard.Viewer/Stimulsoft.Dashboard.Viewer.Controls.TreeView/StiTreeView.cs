using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Controls.TreeView;

[ToolboxItem(false)]
public class StiTreeView : TreeView
{
	private StiControlColorCollection colors;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
			}
		}
	}

	protected override void OnDrawNode(DrawTreeNodeEventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		Graphics graphics = e.Graphics;
		Rectangle rect = new Rectangle(0, e.Bounds.Y, ((Control)this).Width, e.Bounds.Height);
		bool isSelected = (e.State & 1) > 0;
		bool isHot = (e.State & 0x40) > 0;
		Color foreColor = Colors.GetForeColor(isSelected, isHot);
		Color backColor = Colors.GetBackColor(isSelected, isHot);
		Color glyphColor = Colors.GetGlyphColor(isSelected, isHot);
		StiDrawing.FillRectangle(graphics, backColor, rect);
		DrawExpandedState(graphics, e.Node, e.Bounds, glyphColor);
		DrawCheck(graphics, e.Node, e.Bounds, glyphColor);
		DrawText(graphics, e.Node, e.Bounds, foreColor);
		if (e.Node.Index == 0 && e.Node.Text == StiLocHelper.LocAll)
		{
			StiDrawing.DrawLine(graphics, Colors.SeparatorColor, rect.X, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
		}
	}

	protected override void OnAfterCheck(TreeViewEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((TreeView)this).OnAfterCheck(e);
		TreeViewAction action = e.Action;
		if (action - 1 <= 1)
		{
			(e.Node as StiTreeNode)?.Toggle();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		TreeNode nodeAt = ((TreeView)this).GetNodeAt(e.Location);
		if (nodeAt != null)
		{
			((TreeView)this).SelectedNode = nodeAt;
		}
	}

	private void DrawText(Graphics g, TreeNode node, Rectangle rect, Color foreColor)
	{
		Rectangle textRect = GetTextRect(rect);
		TextRenderer.DrawText((IDeviceContext)(object)g, node.Text, ((Control)this).Font, textRect, foreColor);
	}

	private void DrawCheck(Graphics g, TreeNode node, Rectangle rect, Color glyphColor)
	{
		if (((TreeView)this).CheckBoxes)
		{
			Rectangle checkedRect = GetCheckedRect(rect);
			if (node is StiTreeNode)
			{
				StiCheckPainter.PaintCheckBox(g, checkedRect, (node as StiTreeNode).State, glyphColor);
			}
		}
	}

	private void DrawExpandedState(Graphics g, TreeNode node, Rectangle rect, Color glyphColor)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		if (node.Nodes.Count == 0)
		{
			return;
		}
		Rectangle checkedRect = GetCheckedRect(rect);
		int checkSize = GetCheckSize();
		Rectangle rectangle = new Rectangle(checkedRect.X - checkedRect.Width - StiScale.I2, checkedRect.Y, checkSize, checkSize);
		int num = rectangle.X + rectangle.Width / 2;
		int num2 = rectangle.Y + rectangle.Height / 2;
		GraphicsPath val = new GraphicsPath();
		try
		{
			SolidBrush val2 = new SolidBrush(glyphColor);
			try
			{
				int i = StiScale.I2;
				int i2 = StiScale.I4;
				if (node.IsExpanded)
				{
					val.AddPolygon(new Point[3]
					{
						new Point(num - i2, num2 - i),
						new Point(num, num2 + i),
						new Point(num + i2, num2 - i)
					});
				}
				else
				{
					val.AddPolygon(new Point[3]
					{
						new Point(num - i, num2 - i2),
						new Point(num + i, num2),
						new Point(num - i, num2 + i2)
					});
				}
				SmoothingMode smoothingMode = g.SmoothingMode;
				g.SmoothingMode = (SmoothingMode)4;
				g.FillPath((Brush)(object)val2, val);
				g.SmoothingMode = smoothingMode;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private int GetCheckSize(float scale)
	{
		int val = (int)(16f * scale);
		return Math.Min(26, val);
	}

	private int GetCheckSize()
	{
		return GetCheckSize((float)StiScale.Factor);
	}

	private Rectangle GetTextRect(Rectangle rect)
	{
		rect.X += StiScale.XXI(9.0);
		return rect;
	}

	private Rectangle GetCheckedRect(Rectangle rect)
	{
		int checkSize = GetCheckSize();
		Rectangle result = new Rectangle(rect.X - checkSize + StiScale.I2, rect.Y + (rect.Height - checkSize) / 2 - (int)StiScale.Y, checkSize, checkSize);
		if (!((TreeView)this).CheckBoxes)
		{
			result.Width = 0;
		}
		return result;
	}

	public StiTreeView()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		if (Environment.OSVersion.Version.Major < 6)
		{
			((Control)this).SetStyle((ControlStyles)2, true);
		}
		Colors = new StiControlColorCollection((Control)(object)this);
		((TreeView)this).BorderStyle = (BorderStyle)0;
		((TreeView)this).CheckBoxes = true;
		((TreeView)this).DrawMode = (TreeViewDrawMode)1;
		((TreeView)this).FullRowSelect = true;
		((TreeView)this).HotTracking = true;
		((TreeView)this).HideSelection = false;
		((TreeView)this).ItemHeight = StiScale.YYI(StiElementConsts.TreeView.ItemHeight);
		((TreeView)this).ShowLines = false;
		((TreeView)this).ShowNodeToolTips = true;
	}
}
