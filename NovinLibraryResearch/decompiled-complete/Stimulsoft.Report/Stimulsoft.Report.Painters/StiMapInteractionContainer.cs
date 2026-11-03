using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Painters;

public sealed class StiMapInteractionContainer : IDisposable
{
	private Dictionary<string, List<GraphicsPath>> items = new Dictionary<string, List<GraphicsPath>>();

	private double mapGeomWidth;

	private double mapGeomHeight;

	public void Dispose()
	{
		if (items == null)
		{
			return;
		}
		foreach (KeyValuePair<string, List<GraphicsPath>> item in items)
		{
			foreach (GraphicsPath item2 in item.Value)
			{
				item2.Dispose();
			}
			item.Value.Clear();
		}
		items.Clear();
		items = null;
	}

	public void Add(string key, List<GraphicsPath> path)
	{
		items.Add(key, path);
	}

	public string GetKeyByPosition(Point pos, float zoom, Point globalMovePoint, RectangleD rect)
	{
		float num = (float)((((RectangleD)(ref rect)).Width / (double)zoom - mapGeomWidth) / 2.0);
		float num2 = (float)((((RectangleD)(ref rect)).Height / (double)zoom - mapGeomHeight) / 2.0);
		float num3 = (float)pos.X / zoom - num - (float)globalMovePoint.X;
		float num4 = (float)pos.Y / zoom - num2 - (float)globalMovePoint.Y;
		foreach (KeyValuePair<string, List<GraphicsPath>> item in items)
		{
			foreach (GraphicsPath item2 in item.Value)
			{
				if (item2.IsVisible(num3, num4))
				{
					return item.Key;
				}
			}
		}
		return null;
	}

	public StiMapInteractionContainer(double mapGeomWidth, double mapGeomHeight)
	{
		this.mapGeomWidth = mapGeomWidth;
		this.mapGeomHeight = mapGeomHeight;
	}
}
