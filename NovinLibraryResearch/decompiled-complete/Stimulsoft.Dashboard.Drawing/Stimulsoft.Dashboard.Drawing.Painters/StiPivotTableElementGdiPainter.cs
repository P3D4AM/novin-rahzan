using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Drawing.Painters.PivotTable;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiPivotTableElementGdiPainter : StiElementGdiPainter
{
	public override void PaintContent(Graphics g, RectangleD rect, IStiElement element)
	{
		StiErrorStack.SetOk(element.GetKey());
		GraphicsState val = g.Save();
		StiPivotTableElement stiPivotTableElement = element as StiPivotTableElement;
		try
		{
			if (StiElementDataCache.GetOrCreatePivotWithProgress(stiPivotTableElement, new StiPivotTableCreator()) is StiPivotTableContainer stiPivotTableContainer)
			{
				g.SetClip(rect.ToRectangleF());
				rect = StiElementGdiPainter.ApplyCornerRadiusAndSetClip(g, rect, stiPivotTableElement);
				Rectangle crossRect = new Rectangle(0, 0, (int)rect.Width, (int)rect.Height);
				StiPivotElementStyle pivotTableStyle = StiDashboardStyleHelper.GetPivotTableStyle(stiPivotTableElement);
				double zoom = stiPivotTableContainer.PivotTable?.Page.Zoom ?? 1.0;
				StiDataTable dataTable = StiElementDataCache.Get(element);
				if (IsNoResult(element, dataTable))
				{
					PaintNoResult(g, rect, element);
				}
				else
				{
					PaintPivotTable(g, rect, stiPivotTableElement, stiPivotTableContainer, crossRect, pivotTableStyle, zoom);
				}
			}
		}
		catch (Exception ex)
		{
			StiErrorStack.SetError(element.GetKey(), ex.Message);
			PaintError(g, rect.ToRectangleF(), element);
		}
		finally
		{
			StiPivotTableElementClickHelper.ResetClick(stiPivotTableElement);
			StiPivotTableElementMouseOverHelper.ResetMouseOverPoint(stiPivotTableElement);
			g.Restore(val);
		}
	}

	private static void PaintPivotTable(Graphics g, RectangleD rect, StiPivotTableElement pivot, StiPivotTableContainer container, Rectangle crossRect, StiPivotElementStyle style, double zoom)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		GraphicsState val = g.Save();
		try
		{
			g.TranslateTransform((float)rect.X, (float)rect.Y);
			container.GridPaint(new PaintEventArgs(g, crossRect), style, isDesigner: true, new Size((int)rect.X, (int)rect.Y));
			PaintMoreArrows(g, rect, container, zoom);
		}
		finally
		{
			g.Restore(val);
		}
	}

	private static void PaintMoreArrows(Graphics g, RectangleD rect, StiPivotTableContainer container, double zoom)
	{
		if (container.Width * zoom > rect.Width)
		{
			PaintMoreColumnsArrow(g, new RectangleD(0.0, 0.0, rect.Width, container.Rows[0].Height * zoom), (float)(StiScale.Factor * 1.7000000476837158 * zoom));
		}
		if (container.Height * zoom > rect.Height)
		{
			PaintMoreRowsArrow(g, new RectangleD(0.0, 0.0, container.Columns[0].Width * zoom, rect.Height), (float)(StiScale.Factor * 1.7000000476837158 * zoom));
		}
	}

	private static void PaintMoreColumnsArrow(Graphics g, RectangleD rect, float step)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			RectangleF rectangleF = rect.ToRectangleF();
			PointF[] array = new PointF[3]
			{
				new PointF(rectangleF.Right - step * 5f, rectangleF.Y + step * 3f),
				new PointF(rectangleF.Right - step * 2f, rectangleF.Y + step * 5f),
				new PointF(rectangleF.Right - step * 5f, rectangleF.Y + step * 7f)
			};
			SolidBrush val = new SolidBrush(Color.FromArgb(140, 140, 140));
			try
			{
				g.FillPolygon((Brush)(object)val, array);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	private static void PaintMoreRowsArrow(Graphics g, RectangleD rect, float step)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			RectangleF rectangleF = rect.ToRectangleF();
			float num = (float)rect.Width / 2f - step * 4f;
			PointF[] array = new PointF[3]
			{
				new PointF(rectangleF.X + step * 3f + num, rectangleF.Bottom - step * 5f),
				new PointF(rectangleF.X + step * 5f + num, rectangleF.Bottom - step * 2f),
				new PointF(rectangleF.X + step * 7f + num, rectangleF.Bottom - step * 5f)
			};
			SolidBrush val = new SolidBrush(Color.FromArgb(140, 140, 140));
			try
			{
				g.FillPolygon((Brush)(object)val, array);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}
}
