using System.Collections;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal class StiSparklinesCellPainter
{
	internal static void Draw(Graphics g, RectangleD rect, StiSparklinesColumn column, object value, float zoom, StiTableElementStyle style)
	{
		object[] array = CastToArray(value);
		StiGdiContextPainter contextPainter = new StiGdiContextPainter(g);
		StiContext stiContext = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, zoom);
		switch (column.Type)
		{
		case StiSparklinesType.Column:
			StiColumnSparklinesCellPainter.Draw(stiContext, rect, array, column.AllowCustomColors ? column.PositiveColor : style.CellSparkline, column.AllowCustomColors ? column.NegativeColor : style.CellSparkline);
			break;
		case StiSparklinesType.Line:
			StiLineSparklinesCellPainter.Draw(stiContext, rect, array, zoom, column.AllowCustomColors ? column.PositiveColor : style.CellSparkline, showArea: false, column.ShowFirstLastPoints, column.ShowHighLowPoints);
			break;
		case StiSparklinesType.Area:
			StiLineSparklinesCellPainter.Draw(stiContext, rect, array, zoom, column.AllowCustomColors ? column.PositiveColor : style.CellSparkline, showArea: true, column.ShowFirstLastPoints, column.ShowHighLowPoints);
			break;
		case StiSparklinesType.WinLoss:
			StiWinLossSparklinesCellPainter.Draw(stiContext, rect, array, column.AllowCustomColors ? column.PositiveColor : style.CellWinLossPositive, column.AllowCustomColors ? column.NegativeColor : style.CellWinLossNegative);
			break;
		default:
			throw new StiTypeNotRecognizedException(column.Type);
		}
		stiContext.Render(rect.ToRectangleF());
	}

	internal static object[] CastToArray(object value)
	{
		if (!(value is IEnumerable))
		{
			return null;
		}
		return ((IEnumerable)value).Cast<object>().ToArray();
	}
}
