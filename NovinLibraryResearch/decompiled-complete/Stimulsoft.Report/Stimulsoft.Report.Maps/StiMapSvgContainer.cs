using System.Collections.Generic;
using Stimulsoft.Base.Maps.Geoms;

namespace Stimulsoft.Report.Maps;

public class StiMapSvgContainer
{
	private Dictionary<string, object> hashGeometryWpf;

	internal bool IsNotCorrect { get; set; }

	internal bool IsCustom { get; set; }

	public string Name { get; set; }

	public int Width { get; set; }

	public int Height { get; set; }

	public double? TextScale { get; set; }

	public string Icon { get; set; }

	public List<StiMapSvg> Paths { get; set; } = new List<StiMapSvg>();


	public List<StiMapGeomsObject> Geoms { get; set; }

	public Dictionary<string, StiMapSvg> HashPaths { get; set; }

	public object GetGeometryWpf(string key)
	{
		if (hashGeometryWpf == null)
		{
			hashGeometryWpf = new Dictionary<string, object>();
		}
		if (hashGeometryWpf.ContainsKey(key))
		{
			return hashGeometryWpf[key];
		}
		return null;
	}

	public void SetGeometryWpf(string key, object geom)
	{
		if (hashGeometryWpf == null)
		{
			hashGeometryWpf = new Dictionary<string, object>();
		}
		hashGeometryWpf[key] = geom;
	}

	public void Prepare()
	{
		if (Paths == null)
		{
			Paths = new List<StiMapSvg>();
		}
		if (HashPaths != null)
		{
			return;
		}
		HashPaths = new Dictionary<string, StiMapSvg>();
		foreach (StiMapSvg path in Paths)
		{
			HashPaths.Add(path.Key, path);
		}
		Paths.Clear();
		Paths = null;
	}
}
