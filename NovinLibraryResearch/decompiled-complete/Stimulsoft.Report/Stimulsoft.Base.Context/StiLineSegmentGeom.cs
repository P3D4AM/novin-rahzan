using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiLineSegmentGeom : StiSegmentGeom
{
	public float X1 { get; set; }

	public float Y1 { get; set; }

	public float X2 { get; set; }

	public float Y2 { get; set; }

	public StiAnimation Animation { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("X1", (object)X1));
		((JContainer)val).Add((object)new JProperty("Y1", (object)Y1));
		((JContainer)val).Add((object)new JProperty("X2", (object)X2));
		((JContainer)val).Add((object)new JProperty("Y2", (object)Y2));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiLineSegmentGeom(PointF start, PointF end, StiAnimation animation)
		: this(start, end)
	{
		Animation = animation;
	}

	public StiLineSegmentGeom(PointF start, PointF end)
	{
		X1 = start.X;
		Y1 = start.Y;
		X2 = end.X;
		Y2 = end.Y;
	}

	public StiLineSegmentGeom(float x1, float y1, float x2, float y2)
	{
		X1 = x1;
		Y1 = y1;
		X2 = x2;
		Y2 = y2;
	}
}
