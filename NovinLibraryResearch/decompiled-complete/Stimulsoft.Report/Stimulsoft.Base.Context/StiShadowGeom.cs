using System.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiShadowGeom : StiGeom
{
	public RectangleF Rect { get; set; }

	public float Radius { get; set; }

	public StiContext ShadowContext { get; set; }

	public override StiGeomType Type => StiGeomType.Shadow;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Rect", (object)StiGeom.SaveRectangleFToJsonObject(Rect)));
		((JContainer)val).Add((object)new JProperty("Radius", (object)Radius));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiShadowGeom(StiContext shadowContext, RectangleF rect, float radius)
	{
		ShadowContext = shadowContext;
		Rect = rect;
		Radius = radius;
	}
}
