using System.Drawing;

namespace Stimulsoft.Base.Context;

public class StiPushClipGeom : StiGeom
{
	public RectangleF ClipRectangle { get; set; }

	public override StiGeomType Type => StiGeomType.PushClip;

	public StiPushClipGeom(RectangleF clipRectangle)
	{
		ClipRectangle = clipRectangle;
	}
}
