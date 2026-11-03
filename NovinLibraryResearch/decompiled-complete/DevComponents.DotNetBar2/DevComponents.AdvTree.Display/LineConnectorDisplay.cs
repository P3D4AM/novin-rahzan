using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.AdvTree.Display;

public class LineConnectorDisplay : NodeConnectorDisplay
{
	public override void DrawConnector(ConnectorRendererEventArgs info)
	{
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		if (info.NodeConnector.LineColor.IsEmpty || info.NodeConnector.LineWidth <= 0)
		{
			return;
		}
		Point point;
		Point point2;
		if (info.FromNode == null)
		{
			Rectangle nodeRectangle = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, info.ToNode, info.Offset);
			Rectangle nodeRectangle2 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds, info.ToNode, info.Offset);
			point = new Point(nodeRectangle.X - 4, nodeRectangle.Y + nodeRectangle.Height / 2);
			point2 = new Point(nodeRectangle2.X + nodeRectangle2.Width / 2, point.Y);
		}
		else
		{
			Rectangle nodeRectangle3 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.NodeContentBounds, info.FromNode, info.Offset);
			Rectangle nodeRectangle4 = NodeDisplay.GetNodeRectangle(eNodeRectanglePart.ExpandBounds, info.ToNode, info.Offset);
			point = new Point(nodeRectangle4.X + nodeRectangle4.Width / 2, nodeRectangle3.Bottom);
			point2 = new Point(point.X, nodeRectangle4.Y + nodeRectangle4.Height / 2);
		}
		Graphics graphics = info.Graphics;
		Pen linePen = GetLinePen(info);
		try
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			if ((int)linePen.DashStyle != 0)
			{
				graphics.SmoothingMode = (SmoothingMode)0;
			}
			graphics.DrawLine(linePen, point, point2);
			graphics.SmoothingMode = smoothingMode;
		}
		finally
		{
			((IDisposable)linePen)?.Dispose();
		}
	}
}
