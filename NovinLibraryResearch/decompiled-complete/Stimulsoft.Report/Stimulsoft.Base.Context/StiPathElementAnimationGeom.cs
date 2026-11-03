using System.Collections.Generic;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiPathElementAnimationGeom : StiAnimationGeom
{
	public StiBrush WpfBackColor { get; set; }

	public object Background { get; set; }

	public StiPenGeom BorderPen { get; set; }

	public object Rect { get; set; }

	public List<StiSegmentGeom> PathGeoms { get; set; }

	public string ToolTip { get; set; }

	public object Tag { get; set; }

	public override StiGeomType Type => StiGeomType.AnimationPathElement;

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
		if (BorderPen != null)
		{
			((JContainer)val).Add((object)new JProperty("BorderPen", (object)BorderPen.SaveToJsonObject(mode)));
		}
		if (PathGeoms != null)
		{
			object[] array = SaveGeomListToJsonObject(PathGeoms, mode);
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

	public StiPathElementAnimationGeom(StiBrush wpfBackColor, object background, StiPenGeom borderPen, List<StiSegmentGeom> pathGeoms, object rect, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction)
		: base(animation, interaction)
	{
		WpfBackColor = wpfBackColor;
		Background = background;
		BorderPen = borderPen;
		Rect = rect;
		PathGeoms = pathGeoms;
		ToolTip = toolTip;
		Tag = tag;
	}

	public StiPathElementAnimationGeom(object background, StiPenGeom borderPen, List<StiSegmentGeom> pathGeoms, object rect, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction)
		: base(animation, interaction)
	{
		Background = background;
		BorderPen = borderPen;
		Rect = rect;
		PathGeoms = pathGeoms;
		ToolTip = toolTip;
		Tag = tag;
	}
}
