using System.Collections.Generic;

namespace Stimulsoft.Base.Maps.Geoms;

public class StiMapGeomsContainer
{
	public List<StiMapGeomsObject> Geoms = new List<StiMapGeomsObject>();

	public string Name { get; set; }

	public double Width { get; set; }

	public double Height { get; set; }

	public override string ToString()
	{
		return $"Name={Width}x{Height}, geoms={Geoms.Count}";
	}
}
