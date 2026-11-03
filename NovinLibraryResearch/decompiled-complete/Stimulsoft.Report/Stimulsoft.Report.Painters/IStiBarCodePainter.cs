using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Painters;

public interface IStiBarCodePainter
{
	void BaseTransform(object context, float x, float y, float angle, float dx, float dy);

	void BaseRollbackTransform(object context);

	void BaseFillRectangle(object context, StiBrush brush, float x, float y, float width, float height);

	void BaseFillRectangle2D(object context, StiBrush brush, float x, float y, float width, float height);

	void BaseFillPolygon(object context, StiBrush brush, PointF[] points);

	void BaseFillEllipse(object context, StiBrush brush, float x, float y, float width, float height);

	void BaseDrawRectangle(object context, Color penColor, float penSize, float x, float y, float width, float height);

	void BaseDrawImage(object context, Image image, StiReport report, float x, float y, float width, float height);

	void BaseDrawString(object context, string st, Font font, StiBrush brush, RectangleF rect, StringFormat sf);

	SizeF BaseMeasureString(object context, string st, Font font);
}
