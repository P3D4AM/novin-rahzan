using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Design.Controls;

public class StiScaledToolStripRenderer : ToolStripProfessionalRenderer
{
	protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
	{
		if (((object)e.Item).GetType() == typeof(ToolStripDropDownButton))
		{
			Graphics graphics = e.Graphics;
			Rectangle arrowRectangle = e.ArrowRectangle;
			arrowRectangle.X = arrowRectangle.Right - StiScale.I5;
			arrowRectangle.Y += arrowRectangle.Height / 2 - StiScale.I1;
			List<Point> list = new List<Point>();
			list.Add(new Point(arrowRectangle.Left, arrowRectangle.Top));
			list.Add(new Point(arrowRectangle.Left + StiScale.I6, arrowRectangle.Top));
			list.Add(new Point(arrowRectangle.Left + StiScale.I3, arrowRectangle.Top + StiScale.I3));
			graphics.FillPolygon(Brushes.Black, list.ToArray());
		}
		else
		{
			((ToolStripProfessionalRenderer)this).OnRenderArrow(e);
		}
	}

	protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
	{
		Rectangle imageRectangle = e.ImageRectangle;
		((ToolStripItemRenderEventArgs)e).Graphics.DrawRectangle(SystemPens.MenuHighlight, imageRectangle);
		imageRectangle.X += StiScale.I5;
		imageRectangle.Y += StiScale.I5;
		Point point = new Point(imageRectangle.X, imageRectangle.Y + StiScale.I2);
		Point point2 = new Point(imageRectangle.X + StiScale.I2, imageRectangle.Y + StiScale.I4);
		Point point3 = new Point(imageRectangle.X + StiScale.I6, imageRectangle.Y);
		((ToolStripItemRenderEventArgs)e).Graphics.DrawLine(Pens.Black, point, point2);
		((ToolStripItemRenderEventArgs)e).Graphics.DrawLine(Pens.Black, point2, point3);
		point.Y++;
		point2.Y++;
		point3.Y++;
		((ToolStripItemRenderEventArgs)e).Graphics.DrawLine(Pens.Black, point, point2);
		((ToolStripItemRenderEventArgs)e).Graphics.DrawLine(Pens.Black, point2, point3);
	}

	protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
	{
		if (!(((ToolStripItemRenderEventArgs)e).Item is StiScaledToolStripMenuItem))
		{
			((ToolStripProfessionalRenderer)this).OnRenderItemImage(e);
		}
	}
}
