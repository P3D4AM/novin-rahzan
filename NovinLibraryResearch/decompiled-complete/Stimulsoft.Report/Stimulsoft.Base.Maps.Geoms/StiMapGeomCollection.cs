using System.Collections.Generic;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms;

public sealed class StiMapGeomCollection : List<StiMapGeom>
{
	public PointD GetLastPoint()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (base.Count == 0)
		{
			return default(PointD);
		}
		StiMapGeom stiMapGeom = base[base.Count - 1];
		if (stiMapGeom.GeomType == StiMapGeomType.Close && base.Count > 0)
		{
			stiMapGeom = base[base.Count - 2];
			return stiMapGeom.GetLastPoint();
		}
		return stiMapGeom.GetLastPoint();
	}
}
