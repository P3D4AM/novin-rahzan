using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiCachedShadowGeom : StiGeom
{
	public RectangleF Rect { get; set; }

	public RectangleF ClipRect { get; set; }

	public StiShadowSides Sides { get; set; }

	public bool IsPrinting { get; set; }

	public override StiGeomType Type => StiGeomType.CachedShadow;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Rect", (object)StiGeom.SaveRectangleFToJsonObject(Rect)));
		StiShadowSides sides = Sides;
		((JContainer)val).Add((object)new JProperty("Sides", (object)((object)(StiShadowSides)(ref sides)).ToString()));
		((JContainer)val).Add((object)new JProperty("IsPrinting", (object)IsPrinting));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiCachedShadowGeom(RectangleF rect, StiShadowSides sides, bool isPrinting)
		: this(rect, sides, isPrinting, rect)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public StiCachedShadowGeom(RectangleF rect, StiShadowSides sides, bool isPrinting, RectangleF clipRect)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Rect = rect;
		ClipRect = clipRect;
		Sides = sides;
		IsPrinting = isPrinting;
	}
}
