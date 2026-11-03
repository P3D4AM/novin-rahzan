using System.Drawing;

namespace Stimulsoft.Base.Context;

public class StiImageGeom : StiGeom
{
	public RectangleF Rect { get; }

	public byte[] Image { get; }

	public override StiGeomType Type => StiGeomType.Image;

	public StiImageGeom(RectangleF rect, byte[] image)
	{
		Rect = rect;
		Image = image;
	}
}
