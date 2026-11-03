using System.Collections.Generic;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiPathAnimationGeom : StiAnimationGeom
{
	public static object GetBoundsState { get; set; } = new object();


	public StiBrush WpfBackColor { get; set; }

	public object Background { get; set; }

	public StiPenGeom Pen { get; set; }

	public object Rect { get; set; }

	public List<StiSegmentGeom> Geoms { get; set; }

	public object Tag { get; set; }

	public override StiGeomType Type => StiGeomType.AnimationPath;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Rect", (object)SaveRectToJsonObject(Rect)));
		if (Background != null)
		{
			((JContainer)val).Add((object)new JProperty("Background", (object)SaveBrushToJsonObject(Background, mode)));
		}
		if (Pen != null)
		{
			((JContainer)val).Add((object)new JProperty("Pen", (object)Pen.SaveToJsonObject(mode)));
		}
		if (Geoms != null)
		{
			object[] array = SaveGeomListToJsonObject(Geoms, mode);
			((JContainer)val).Add((object)new JProperty("Geoms", array));
		}
		if (Tag is string)
		{
			((JContainer)val).Add((object)new JProperty("Tag", (object)(Tag as string)));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiPathAnimationGeom(StiBrush wpfBackColor, object background, StiPenGeom pen, List<StiSegmentGeom> geoms, object rect, object tag, StiAnimation animation, StiInteractionDataGeom interaction)
		: base(animation, interaction)
	{
		WpfBackColor = wpfBackColor;
		Background = background;
		Pen = pen;
		Geoms = geoms;
		Rect = rect;
		Tag = tag;
	}

	public StiPathAnimationGeom(object background, StiPenGeom pen, List<StiSegmentGeom> geoms, object rect, object tag, StiAnimation animation, StiInteractionDataGeom interaction)
		: base(animation, interaction)
	{
		Background = background;
		Pen = pen;
		Geoms = geoms;
		Rect = rect;
		Tag = tag;
	}
}
