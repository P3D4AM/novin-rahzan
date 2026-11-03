using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree.Display;

public abstract class NodeExpandDisplay
{
	private TreeColorTable __137C;

	public TreeColorTable ColorTable
	{
		get
		{
			return __137C;
		}
		set
		{
			__137C = value;
		}
	}

	public NodeExpandDisplay()
	{
	}

	public abstract void DrawExpandButton(NodeExpandPartRendererEventArgs e);

	protected Pen GetBorderPen(NodeExpandPartRendererEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (!e.BorderColor.IsEmpty)
		{
			return new Pen(e.BorderColor, 1f);
		}
		if (__137C != null)
		{
			bool expanded = e.Node.Expanded;
			TreeExpandColorTable treeExpandColorTable = _2770(e);
			if (treeExpandColorTable == null)
			{
				return null;
			}
			if (expanded)
			{
				if (!e.IsMouseOver && treeExpandColorTable.CollapseBorder != null)
				{
					return treeExpandColorTable.CollapseBorder.CreatePen();
				}
				if (e.IsMouseOver && treeExpandColorTable.CollapseMouseOverBorder != null)
				{
					return treeExpandColorTable.CollapseMouseOverBorder.CreatePen();
				}
			}
			else
			{
				if (!e.IsMouseOver && treeExpandColorTable.ExpandBorder != null)
				{
					return treeExpandColorTable.ExpandBorder.CreatePen();
				}
				if (e.IsMouseOver && treeExpandColorTable.ExpandMouseOverBorder != null)
				{
					return treeExpandColorTable.ExpandMouseOverBorder.CreatePen();
				}
			}
		}
		return null;
	}

	private TreeExpandColorTable _2770(NodeExpandPartRendererEventArgs ؿ)
	{
		TreeExpandColorTable result = null;
		if (ؿ._276F == eExpandButtonType.Rectangle)
		{
			result = __137C.ExpandRectangle;
		}
		else if (ؿ._276F == eExpandButtonType.Triangle)
		{
			result = __137C.ExpandTriangle;
		}
		else if (ؿ._276F == eExpandButtonType.Ellipse)
		{
			result = __137C.ExpandEllipse;
		}
		return result;
	}

	protected Pen GetExpandPen(NodeExpandPartRendererEventArgs e)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		if (e.ExpandLineColor.IsEmpty)
		{
			TreeExpandColorTable treeExpandColorTable = _2770(e);
			if (treeExpandColorTable != null)
			{
				if (e.Node.Expanded)
				{
					if (!e.IsMouseOver && treeExpandColorTable.CollapseForeground != null)
					{
						return treeExpandColorTable.CollapseForeground.CreatePen(Dpi.Width1);
					}
					if (e.IsMouseOver && treeExpandColorTable.CollapseMouseOverForeground != null)
					{
						return treeExpandColorTable.CollapseMouseOverForeground.CreatePen(Dpi.Width1);
					}
				}
				else
				{
					if (!e.IsMouseOver && treeExpandColorTable.ExpandForeground != null)
					{
						return treeExpandColorTable.ExpandForeground.CreatePen(Dpi.Width1);
					}
					if (e.IsMouseOver && treeExpandColorTable.ExpandMouseOverForeground != null)
					{
						return treeExpandColorTable.ExpandMouseOverForeground.CreatePen(Dpi.Width1);
					}
				}
			}
			return GetBorderPen(e);
		}
		return new Pen(e.ExpandLineColor, 1f);
	}

	protected Brush GetBackgroundBrush(NodeExpandPartRendererEventArgs e)
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		if (e.BackColor.IsEmpty && e.BackColor2.IsEmpty)
		{
			bool expanded = e.Node.Expanded;
			TreeExpandColorTable treeExpandColorTable = _2770(e);
			if (treeExpandColorTable == null)
			{
				return null;
			}
			if (expanded)
			{
				if (!e.IsMouseOver && treeExpandColorTable.CollapseFill != null)
				{
					return treeExpandColorTable.CollapseFill.CreateBrush(e.ExpandPartBounds);
				}
				if (e.IsMouseOver && treeExpandColorTable.CollapseMouseOverFill != null)
				{
					return treeExpandColorTable.CollapseMouseOverFill.CreateBrush(e.ExpandPartBounds);
				}
			}
			else
			{
				if (!e.IsMouseOver && treeExpandColorTable.ExpandFill != null)
				{
					return treeExpandColorTable.ExpandFill.CreateBrush(e.ExpandPartBounds);
				}
				if (e.IsMouseOver && treeExpandColorTable.ExpandMouseOverFill != null)
				{
					return treeExpandColorTable.ExpandMouseOverFill.CreateBrush(e.ExpandPartBounds);
				}
			}
			return null;
		}
		if (e.BackColor2.IsEmpty)
		{
			return (Brush)new SolidBrush(e.BackColor);
		}
		return (Brush)(object)DisplayHelp.CreateLinearGradientBrush(e.ExpandPartBounds, e.BackColor, e.BackColor2, e.BackColorGradientAngle);
	}
}
