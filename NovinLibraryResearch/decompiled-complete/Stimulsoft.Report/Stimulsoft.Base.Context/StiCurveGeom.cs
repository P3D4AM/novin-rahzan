using System.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiCurveGeom : StiGeom
{
	public StiPenGeom Pen { get; set; }

	public float Tension { get; set; }

	public PointF[] Points { get; set; }

	public override StiGeomType Type => StiGeomType.Curve;

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

	public StiCurveGeom(StiPenGeom pen, PointF[] points, float tension)
	{
		Pen = pen;
		Tension = tension;
		Points = points;
	}
}
