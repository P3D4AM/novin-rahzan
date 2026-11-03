using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Context;

public abstract class StiContextPainter : IDisposable
{
	public abstract StiContextPainter Clone();

	public abstract void Dispose();

	public abstract StiStringFormatGeom GetDefaultStringFormat();

	public abstract StiStringFormatGeom GetGenericStringFormat();

	public abstract StiContext CreateShadowGraphics(bool isPrinting, float zoom);

	public abstract RectangleF GetPathBounds(List<StiSegmentGeom> geoms);

	public abstract SizeF MeasureString(string text, StiFontGeom font);

	public abstract SizeF MeasureString(string text, StiFontGeom font, int width, StiStringFormatGeom sf);

	public abstract RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, float angle);

	public abstract RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode, float angle);

	public abstract RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle, int maximalWidth);

	public abstract RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle);

	public abstract void Render(RectangleF rect, List<StiGeom> geoms);
}
