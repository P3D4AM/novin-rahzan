using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Helpers;

internal static class StiRegionMapZoomHelper
{
	public class StiMapState
	{
		public float Zoom { get; set; } = 1f;


		public Point MoveOffset { get; set; } = new Point(0, 0);


		public float LastDrawingZoom { get; set; } = 1f;

	}

	private static Dictionary<IStiElement, StiMapState> cache = new Dictionary<IStiElement, StiMapState>();

	public static void Reset()
	{
		cache.Clear();
	}

	public static StiMapState Get(IStiElement map)
	{
		StiMapState value = null;
		cache.TryGetValue(map, out value);
		if (value == null)
		{
			value = new StiMapState();
			cache.Add(map, value);
		}
		return value;
	}
}
