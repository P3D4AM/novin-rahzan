using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public abstract class StiAtomElementGdiPainter : StiElementGdiPainter
{
	public override void PaintContent(Graphics g, RectangleD rect, IStiElement element)
	{
		StiErrorStack.SetOk(element.GetKey());
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(rect.ToRectangleF());
			rect = StiElementGdiPainter.ApplyCornerRadiusAndSetClip(g, rect, element);
			PaintAtom(g, rect, element);
		}
		catch (Exception ex)
		{
			StiErrorStack.SetError(element.GetKey(), ex.Message);
			PaintError(g, rect.ToRectangleF(), element);
		}
		finally
		{
			g.Restore(val);
		}
	}

	public virtual void PaintAtom(Graphics g, RectangleD rect, IStiElement element)
	{
		StiDataTable dataTable = GetDataTable(element);
		if (IsNoResult(element, dataTable))
		{
			PaintNoResult(g, rect, element);
		}
		else if (IsEmptyData(element, dataTable))
		{
			PaintEmptyData(g, rect, element);
		}
		else
		{
			StiElementAtom.PaintElement(g, rect, element, dataTable);
		}
	}

	private StiDataTable GetDataTable(IStiElement element)
	{
		StiDataTable stiDataTable = (element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
		if (stiDataTable != null)
		{
			return stiDataTable;
		}
		if (StiDbsThumbnaiModelHelper.AllowPage(element.Page))
		{
			return StiThumbnailDataHelper.Get(element);
		}
		if (element.IsQuerable)
		{
			return StiElementDataCache.GetOrCreateWithProgress(element);
		}
		return null;
	}
}
