using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Painters;

internal class StiColumnSparklinesCellPainter
{
	internal static void Draw(StiContext context, RectangleD rect, object[] array, Color positiveColor, Color negativeColor)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (array == null || array.Length == 0)
		{
			return;
		}
		((RectangleD)(ref rect)).Inflate(-2.0, -2.0);
		double[] array2 = ((IEnumerable<object>)array).Select((Func<object, double>)StiValueHelper.TryToDouble).ToArray();
		double min = array2.Min((double v) => v);
		double max = array2.Max((double v) => v);
		double rMin = min;
		if (min > 0.0)
		{
			array2 = array2.Select((double v) => v - min).ToArray();
			max -= min;
			min = 0.0;
		}
		if (max < 0.0)
		{
			array2 = array2.Select((double v) => v + (0.0 - max)).ToArray();
			min += 0.0 - max;
			max = 0.0;
		}
		double stepWidth = ((RectangleD)(ref rect)).Width / (double)array2.Length;
		double totalHeight = max + Math.Abs(min);
		double maxPart = ((RectangleD)(ref rect)).Height * max / totalHeight;
		double x = ((RectangleD)(ref rect)).X;
		double barWidth = stepWidth;
		if (barWidth < 1.0)
		{
			barWidth = 1.0;
		}
		array2.ToList().ForEach(delegate(double value)
		{
			Color color = positiveColor;
			if (value < 0.0)
			{
				color = negativeColor;
			}
			double num = ((RectangleD)(ref rect)).Height * Math.Abs(value) / totalHeight;
			RectangleD val = default(RectangleD);
			((RectangleD)(ref val))._002Ector(x, ((RectangleD)(ref rect)).Y + ((value > 0.0) ? (maxPart - num) : maxPart), barWidth, num);
			if (value == 0.0)
			{
				if (rMin > 0.0)
				{
					((RectangleD)(ref val)).Y = ((RectangleD)(ref val)).Y - 1.0;
				}
				((RectangleD)(ref val)).Height = 1.0;
			}
			if (((RectangleD)(ref val)).Width > 5.0)
			{
				((RectangleD)(ref val)).Inflate(-1.0, 0.0);
			}
			x += stepWidth;
			context.FillRectangle(color, (float)((RectangleD)(ref val)).X, (float)((RectangleD)(ref val)).Y, (float)((RectangleD)(ref val)).Width, (float)((RectangleD)(ref val)).Height);
		});
	}
}
