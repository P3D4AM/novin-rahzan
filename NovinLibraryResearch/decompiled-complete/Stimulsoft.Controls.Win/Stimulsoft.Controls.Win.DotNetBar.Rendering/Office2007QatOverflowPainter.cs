using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Presentation;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007QatOverflowPainter : QatOverflowPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	public override void Paint(QatOverflowItemRendererEventArgs e)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		QatOverflowItem overflowItem = e.OverflowItem;
		System.Drawing.Rectangle displayRectangle = overflowItem.DisplayRectangle;
		Region val = null;
		if (graphics.Clip != null)
		{
			val = graphics.Clip.Clone();
		}
		graphics.SetClip(overflowItem.DisplayRectangle, (CombineMode)1);
		Office2007ButtonItemColorTable colorTable = GetColorTable();
		Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = colorTable.Default;
		if (overflowItem.Expanded)
		{
			office2007ButtonItemStateColorTable = colorTable.Expanded;
		}
		else if (overflowItem.IsMouseOver)
		{
			office2007ButtonItemStateColorTable = colorTable.MouseOver;
		}
		Office2007ButtonItemPainter.PaintBackground(graphics, office2007ButtonItemStateColorTable, displayRectangle, RoundRectangleShapeDescriptor.RoundCorner2);
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		Color expandBackground = office2007ButtonItemStateColorTable.ExpandBackground;
		Color expandLight = office2007ButtonItemStateColorTable.ExpandLight;
		Shape shape = new Shape();
		ShapeBorder border = new ShapeBorder(expandLight, 1);
		ShapeBorder border2 = new ShapeBorder(expandBackground, 1);
		shape.Children.Add(new Line(new Location(0, 0), new Location(2, 2), border));
		shape.Children.Add(new Line(new Location(2, 2), new Location(0, 4), border));
		shape.Children.Add(new Line(new Location(0, 1), new Location(1, 2), border2));
		shape.Children.Add(new Line(new Location(1, 2), new Location(0, 3), border2));
		shape.Children.Add(new Line(new Location(0, 3), new Location(0, 1), border2));
		System.Drawing.Rectangle bounds = new System.Drawing.Rectangle(displayRectangle.X + (displayRectangle.Width - 5) / 2, displayRectangle.Y + (displayRectangle.Height - 4) / 2, 3, 6);
		ShapePaintInfo shapePaintInfo = new ShapePaintInfo(graphics, bounds);
		shape.Paint(shapePaintInfo);
		bounds.Offset(4, 0);
		shapePaintInfo.Bounds = bounds;
		shape.Paint(shapePaintInfo);
		graphics.SmoothingMode = smoothingMode;
		if (val != null)
		{
			graphics.Clip = val;
		}
		else
		{
			graphics.ResetClip();
		}
	}

	protected virtual Office2007ButtonItemColorTable GetColorTable()
	{
		Office2007ColorTable colorTable = ColorTable;
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = null;
		eButtonColor eButtonColor = eButtonColor.Orange;
		office2007ButtonItemColorTable = colorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor)];
		if (office2007ButtonItemColorTable == null)
		{
			return colorTable.ButtonItemColors[0];
		}
		return office2007ButtonItemColorTable;
	}
}
