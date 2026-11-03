using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiPushTranslateTransformGeom : StiGeom
{
	public float X { get; set; }

	public float Y { get; set; }

	public override StiGeomType Type => StiGeomType.PushTranslateTransform;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("X", (object)X));
		((JContainer)val).Add((object)new JProperty("Y", (object)Y));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiPushTranslateTransformGeom(float x, float y)
	{
		X = x;
		Y = y;
	}
}
