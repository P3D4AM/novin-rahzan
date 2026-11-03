using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiLineGeom : StiGeom
{
	public StiPenGeom Pen { get; set; }

	public float X1 { get; set; }

	public float Y1 { get; set; }

	public float X2 { get; set; }

	public float Y2 { get; set; }

	public override StiGeomType Type => StiGeomType.Line;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Pen", (object)Pen.SaveToJsonObject(mode)));
		((JContainer)val).Add((object)new JProperty("X1", (object)X1));
		((JContainer)val).Add((object)new JProperty("Y1", (object)Y1));
		((JContainer)val).Add((object)new JProperty("X2", (object)X2));
		((JContainer)val).Add((object)new JProperty("Y2", (object)Y2));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiLineGeom(StiPenGeom pen, float x1, float y1, float x2, float y2)
	{
		Pen = pen;
		X1 = x1;
		Y1 = y1;
		X2 = x2;
		Y2 = y2;
	}
}
