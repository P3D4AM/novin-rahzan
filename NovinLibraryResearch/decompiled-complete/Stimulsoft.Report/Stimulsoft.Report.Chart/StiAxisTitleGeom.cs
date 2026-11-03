using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiAxisTitleGeom : StiCellGeom
{
	public IStiAxis Axis { get; }

	public float Angle { get; }

	private StiFontGeom Font { get; }

	public override void Draw(StiContext context)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		StiSolidBrush brush = new StiSolidBrush(Axis.Title.Color);
		StiStringFormatGeom defaultStringFormat = context.GetDefaultStringFormat();
		context.DrawRotatedString(Axis.Title.Text, Font, brush, base.ClientRectangle, defaultStringFormat, (StiRotationMode)4, Angle, Axis.Title.Antialiasing);
	}

	public StiAxisTitleGeom(IStiAxis axis, RectangleF clientRectangle, float angle, StiFontGeom font)
		: base(clientRectangle)
	{
		Axis = axis;
		Angle = angle;
		Font = font;
	}
}
