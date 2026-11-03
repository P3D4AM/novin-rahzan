using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiCurveSegmentGeom : StiSegmentGeom
{
	public float Tension { get; set; } = 0.55f;


	public PointF[] Points { get; set; }

	public StiAnimation Animation { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Tension", (object)Tension));
		object[] array = SavePointFArrayToJsonObject(Points);
		((JContainer)val).Add((object)new JProperty("Points", array));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiCurveSegmentGeom(PointF[] points, float tension, StiAnimation animation)
		: this(points, tension)
	{
		Animation = animation;
	}

	public StiCurveSegmentGeom(PointF[] points, float tension)
	{
		Points = points;
		Tension = tension;
	}
}
