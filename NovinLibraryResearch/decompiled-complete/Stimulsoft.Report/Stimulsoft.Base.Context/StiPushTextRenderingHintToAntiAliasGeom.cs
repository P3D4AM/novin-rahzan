using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiPushTextRenderingHintToAntiAliasGeom : StiGeom
{
	public override StiGeomType Type => StiGeomType.PushTextRenderingHintToAntiAlias;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return base.SaveToJsonObject(mode);
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}
}
