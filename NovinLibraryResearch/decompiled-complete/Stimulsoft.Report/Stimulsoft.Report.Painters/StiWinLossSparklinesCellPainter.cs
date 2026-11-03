using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Painters;

internal class StiWinLossSparklinesCellPainter
{
	internal static void Draw(StiContext context, RectangleD rect, object[] array, Color positiveColor, Color negativeColor)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (array == null || array.Length == 0)
		{
			return;
		}
		((RectangleD)(ref rect)).Inflate(-2.0, -2.0);
		double[] values = ((IEnumerable<object>)array).Select((Func<object, double>)StiValueHelper.TryToDouble).ToArray();
		values = values.Select(delegate(double v)
		{
			if (v == 0.0)
			{
				return 0.0;
			}
			return (!(v > 0.0)) ? (-1.0) : 1.0;
		}).ToArray();
		double stepWidth = ((RectangleD)(ref rect)).Width / (double)values.Length;
		double maxPart = ((RectangleD)(ref rect)).Height / 2.0;
		double x = ((RectangleD)(ref rect)).X;
		List<RectangleD> list = values.Select(delegate(double value)
		{
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			double num = ((RectangleD)(ref rect)).Height * Math.Abs(value) / 2.0;
			RectangleD result = default(RectangleD);
			((RectangleD)(ref result))._002Ector(x, ((RectangleD)(ref rect)).Y + ((value > 0.0) ? (maxPart - num) : maxPart), stepWidth, num);
			if (((RectangleD)(ref result)).Width > 4.0)
			{
				((RectangleD)(ref result)).Inflate(-1.0, 0.0);
			}
			x += stepWidth;
			return result;
		}).ToList();
		int valueIndex = 0;
		list.ForEach(delegate(RectangleD r)
		{
			Color color = ((values[valueIndex] > 0.0) ? positiveColor : negativeColor);
			context.FillRectangle(color, (float)((RectangleD)(ref r)).X, (float)((RectangleD)(ref r)).Y, (float)((RectangleD)(ref r)).Width, (float)((RectangleD)(ref r)).Height);
			valueIndex++;
		});
	}
}
