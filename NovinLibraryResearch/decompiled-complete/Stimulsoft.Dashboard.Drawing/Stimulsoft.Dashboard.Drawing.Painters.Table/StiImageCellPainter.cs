using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal static class StiImageCellPainter
{
	internal static void Draw(Graphics g, RectangleD rect, object imageObj, StiTableColumn column)
	{
		Image imageFromObject = StiImageHelper.GetImageFromObject(imageObj);
		try
		{
			if (imageFromObject == null)
			{
				return;
			}
			double num = rect.Width / (double)imageFromObject.Width;
			double num2 = rect.Height / (double)imageFromObject.Height;
			double num3 = ((num > num2) ? num2 : num);
			float num4 = (float)((double)imageFromObject.Width * num3);
			float num5 = (float)((double)imageFromObject.Height * num3);
			RectangleF rectangleF = rect.ToRectangleF();
			if (column != null)
			{
				if (column.HorAlignment == StiHorAlignment.Right)
				{
					rectangleF.X = rectangleF.Right - num4;
				}
				else if (column.HorAlignment == StiHorAlignment.Center)
				{
					rectangleF.X += (rectangleF.Width - num4) / 2f;
				}
			}
			g.DrawImage(imageFromObject, rectangleF.X, rectangleF.Y, num4, num5);
		}
		finally
		{
			((IDisposable)imageFromObject)?.Dispose();
		}
	}
}
