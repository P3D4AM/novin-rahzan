using System;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms;

public abstract class StiMapGeom
{
	public abstract StiMapGeomType GeomType { get; }

	public virtual PointD GetLastPoint()
	{
		throw new NotSupportedException();
	}
}
