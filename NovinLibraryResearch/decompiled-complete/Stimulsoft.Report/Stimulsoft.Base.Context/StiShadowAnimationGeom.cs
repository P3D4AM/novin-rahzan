using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiShadowAnimationGeom : StiAnimationGeom
{
	public RectangleF Rect { get; set; }

	public double RadiusX { get; set; }

	public double RadiusY { get; set; }

	public int ShadowWidth { get; set; }

	public override StiGeomType Type => StiGeomType.AnimationShadow;

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
		((JContainer)val).Add((object)new JProperty("Rect", (object)StiGeom.SaveRectangleFToJsonObject(Rect)));
		((JContainer)val).Add((object)new JProperty("RadiusX", (object)RadiusX));
		((JContainer)val).Add((object)new JProperty("RadiusY", (object)RadiusY));
		((JContainer)val).Add((object)new JProperty("ShadowWidth", (object)ShadowWidth));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiShadowAnimationGeom(RectangleF rect, int shadowWidth, StiAnimation animation)
		: base(animation)
	{
		Rect = rect;
		RadiusX = 0.0;
		RadiusY = 0.0;
		ShadowWidth = shadowWidth;
	}

	public StiShadowAnimationGeom(RectangleF rect, double radiusX, double radiusY, int shadowWidth, StiAnimation animation)
		: base(animation)
	{
		Rect = rect;
		RadiusX = radiusX;
		RadiusY = radiusY;
		ShadowWidth = shadowWidth;
	}
}
