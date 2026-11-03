using System.Collections.Generic;

namespace Stimulsoft.Base.Context;

public class StiPushClipPathGeom : StiGeom
{
	public List<StiSegmentGeom> Geoms { get; set; }

	public override StiGeomType Type => StiGeomType.PushClipPath;

	public StiPushClipPathGeom(List<StiSegmentGeom> geoms)
	{
		Geoms = geoms;
	}
}
