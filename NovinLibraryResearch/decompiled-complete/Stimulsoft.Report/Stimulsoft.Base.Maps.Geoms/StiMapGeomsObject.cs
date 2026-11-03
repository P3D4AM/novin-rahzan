using System.Collections.Generic;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Base.Maps.Geoms;

public class StiMapGeomsObject
{
	public string Key { get; set; }

	public List<StiMapGeom> Geoms { get; set; } = new List<StiMapGeom>();


	public StiOpacityAnimation Animation { get; set; }

	public override string ToString()
	{
		return $"{Key}({((Geoms != null) ? Geoms.Count : 0)})";
	}
}
