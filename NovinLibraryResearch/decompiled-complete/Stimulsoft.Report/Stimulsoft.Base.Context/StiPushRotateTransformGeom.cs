using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiPushRotateTransformGeom : StiGeom
{
	public float Angle { get; set; }

	public override StiGeomType Type => StiGeomType.PushRotateTransform;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Angle", (object)Angle));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiPushRotateTransformGeom(float angle)
	{
		Angle = angle;
	}
}
