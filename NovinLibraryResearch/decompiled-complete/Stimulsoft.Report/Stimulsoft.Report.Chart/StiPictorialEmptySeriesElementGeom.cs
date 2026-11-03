using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiPictorialEmptySeriesElementGeom : StiSeriesElementGeom
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
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		SizeF singleSize = ((StiPictorialSeriesCoreXF)base.Series.Core).GetSingleSize(context);
		base.Draw(context);
		FontFamily fontFamilyIcons = StiFontIconsHelper.GetFontFamilyIcons();
		Font val = new Font(fontFamilyIcons, 14f * context.Options.Zoom);
		StiFontGeom font = new StiFontGeom(val.FontFamily, val.FontFamily.Name, val.Size, val.Style, val.Unit, val.GdiCharSet, val.GdiVerticalFont);
		for (int i = 0; i < drawRectangles.Count; i++)
		{
			RectangleF rectangleF = drawRectangles[i];
			RectangleF clipRect = clipRectangles[i];
			bool flag = true;
			PointF pointF = new PointF((int)rectangleF.X, (int)rectangleF.Y);
			context.PushClip(clipRect);
			while (flag)
			{
				context.DrawString(rect: new RectangleF(pointF.X + context.Options.Zoom, pointF.Y + context.Options.Zoom, singleSize.Width, singleSize.Height), text: StiFontIconsHelper.GetContent((StiFontIcons?)icon), font: font, brush: base.SeriesBrush, sf: GetStringFormatGeom(context), antialiasing: true, toolTip: GetToolTip());
				pointF = new PointF(pointF.X + singleSize.Width, pointF.Y);
				if (pointF.X + singleSize.Width > rectangleF.Right && pointF.Y + singleSize.Height > rectangleF.Bottom)
				{
					flag = false;
				}
				else if (pointF.X + singleSize.Width > rectangleF.Right)
				{
					pointF = new PointF((int)rectangleF.X, (float)(int)pointF.Y + singleSize.Height);
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

	public StiPictorialEmptySeriesElementGeom(StiAreaGeom areaGeom, double value, int index, StiBrush seriesBrush, IStiSeries series, StiFontIcons icon, List<RectangleF> drawRectangles, List<RectangleF> clipRectangles, RectangleF clientRectangle, StiAnimation animation)
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
