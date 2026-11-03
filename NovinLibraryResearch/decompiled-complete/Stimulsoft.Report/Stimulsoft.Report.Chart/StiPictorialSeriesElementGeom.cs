using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiPictorialSeriesElementGeom : StiSeriesElementGeom
{
	private StiAnimation animation;

	private List<RectangleF> drawRectangles;

	private List<RectangleF> clipRectangles;

	private StiFontIcons icon;

	public StiAnimation Animation => animation;

	public List<RectangleF> DrawRectangles => drawRectangles;

	public List<RectangleF> ClipRectangles => clipRectangles;

	public StiFontIcons Icon => icon;

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		PointF pt = new PointF(x, y);
		for (int i = 0; i < drawRectangles.Count; i++)
		{
			if (drawRectangles[i].Contains(pt))
			{
				return true;
			}
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		SizeF singleSize = ((StiPictorialSeriesCoreXF)base.Series.Core).GetSingleSize(context);
		base.Draw(context);
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font val = new Font(fontFamilyIcons, 14f * context.Options.Zoom);
		StiFontGeom font = new StiFontGeom(val.FontFamily, val.FontFamily.Name, val.Size, val.Style, val.Unit, val.GdiCharSet, val.GdiVerticalFont);
		float zoom = context.Options.Zoom;
		for (int i = 0; i < drawRectangles.Count; i++)
		{
			RectangleF rectangleF = drawRectangles[i];
			RectangleF clipRect = clipRectangles[i];
			bool flag = true;
			PointF pointF = new PointF((int)rectangleF.X, (int)rectangleF.Y);
			if (base.Series.ShowShadow)
			{
				clipRect.Width += zoom;
				clipRect.Height += zoom;
			}
			context.PushClip(clipRect);
			while (flag)
			{
				RectangleF rectangleF2 = new RectangleF(pointF.X + context.Options.Zoom, pointF.Y + context.Options.Zoom, singleSize.Width, singleSize.Height);
				string content = StiFontIconsHelper.GetContent((StiFontIcons?)icon);
				StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context);
				string toolTip = GetToolTip();
				if (base.Series.ShowShadow)
				{
					StiSolidBrush brush = new StiSolidBrush(Color.FromArgb(50, 0, 0, 0));
					RectangleF rect = rectangleF2;
					rect.Offset(zoom, zoom);
					context.DrawString(content, font, brush, rect, stringFormatGeom, antialiasing: true, toolTip);
				}
				context.DrawString(content, font, base.SeriesBrush, rectangleF2, stringFormatGeom, antialiasing: true, toolTip);
				pointF = new PointF(pointF.X + singleSize.Width, pointF.Y);
				if (pointF.X + singleSize.Width > rectangleF.Right)
				{
					flag = false;
				}
			}
			context.PopClip();
		}
		val.Dispose();
	}

	protected internal StiStringFormatGeom GetStringFormatGeom(StiContext context)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.Trimming = (StringTrimming)0;
		genericStringFormat.FormatFlags = (StringFormatFlags)(genericStringFormat.FormatFlags | 0x1800);
		genericStringFormat.Alignment = (StringAlignment)1;
		genericStringFormat.LineAlignment = (StringAlignment)1;
		return genericStringFormat;
	}

	public StiPictorialSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, StiBrush seriesBrush, IStiSeries series, StiFontIcons icon, List<RectangleF> drawRectangles, List<RectangleF> clipRectangles, RectangleF clientRectangle, StiAnimation animation)
		: base(areaGeom, value, index, series, clientRectangle, seriesBrush)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		this.icon = icon;
		this.drawRectangles = drawRectangles;
		this.clipRectangles = clipRectangles;
		this.animation = animation;
	}
}
