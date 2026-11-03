using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiLinesSegmentGeom : StiSegmentGeom
{
	public PointF[] Points { get; set; }

	public StiAnimation Animation { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		object[] array = SavePointFArrayToJsonObject(Points);
		((JContainer)val).Add((object)new JProperty("Points", array));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiLinesSegmentGeom(PointF[] points, StiAnimation animation)
		: this(points)
	{
		Animation = animation;
	}

	public StiLinesSegmentGeom(PointF[] points)
	{
		Points = points;
	}
}
