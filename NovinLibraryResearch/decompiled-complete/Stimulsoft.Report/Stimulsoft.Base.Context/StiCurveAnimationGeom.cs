using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiCurveAnimationGeom : StiAnimationGeom
{
	public StiPenGeom Pen { get; set; }

	public PointF[] Points { get; set; }

	public float Tension { get; set; }

	public override StiGeomType Type => StiGeomType.AnimationCurve;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		if (Pen != null)
		{
			((JContainer)val).Add((object)new JProperty("Pen", (object)Pen.SaveToJsonObject(mode)));
		}
		((JContainer)val).Add((object)new JProperty("Tension", (object)Tension));
		object[] array = SavePointFArrayToJsonObject(Points);
		((JContainer)val).Add((object)new JProperty("Points", array));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiCurveAnimationGeom(StiPenGeom pen, PointF[] points, float tension, StiAnimation animation)
		: base(animation)
	{
		Pen = pen;
		Points = points;
		Tension = tension;
	}
}
