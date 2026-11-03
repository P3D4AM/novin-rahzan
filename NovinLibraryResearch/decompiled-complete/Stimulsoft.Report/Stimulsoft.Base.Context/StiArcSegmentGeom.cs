using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiArcSegmentGeom : StiSegmentGeom
{
	public RectangleF Rect { get; set; }

	public float StartAngle { get; set; }

	public float SweepAngle { get; set; }

	public float? RealStartAngle { get; set; }

	public float? RealSweepAngle { get; set; }

	public StiAnimation Animation { get; set; }

	public bool CrossElipseDraw { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Rect", (object)StiGeom.SaveRectangleFToJsonObject(Rect)));
		((JContainer)val).Add((object)new JProperty("StartAngle", (object)StartAngle));
		((JContainer)val).Add((object)new JProperty("SweepAngle", (object)SweepAngle));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiArcSegmentGeom(RectangleF rect, float startAngle, float sweepAngle)
	{
		Rect = rect;
		StartAngle = startAngle;
		SweepAngle = sweepAngle;
	}

	public StiArcSegmentGeom(RectangleF rect, float startAngle, float sweepAngle, float realStartAngle, float realSweepAngle)
	{
		Rect = rect;
		StartAngle = startAngle;
		SweepAngle = sweepAngle;
		RealStartAngle = realStartAngle;
		RealSweepAngle = realSweepAngle;
		CrossElipseDraw = true;
	}
}
