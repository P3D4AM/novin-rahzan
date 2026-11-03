using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiBorderAnimationGeom : StiAnimationGeom
{
	public StiBrush WpfBackColor { get; set; }

	public object Background { get; set; }

	public StiPenGeom BorderPen { get; set; }

	public object Rect { get; set; }

	public object Tag { get; set; }

	public string ToolTip { get; set; }

	public override StiGeomType Type => StiGeomType.AnimationBorder;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		if (Background != null)
		{
			((JContainer)val).Add((object)new JProperty("Background", (object)SaveBrushToJsonObject(Background, mode)));
		}
		if (BorderPen != null)
		{
			((JContainer)val).Add((object)new JProperty("BorderPen", (object)BorderPen.SaveToJsonObject(mode)));
		}
		((JContainer)val).Add((object)new JProperty("Rect", (object)SaveRectToJsonObject(Rect)));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiBorderAnimationGeom(StiBrush wpfBackColor, object background, StiPenGeom borderPen, object rect, object tag, StiAnimation animation, StiInteractionDataGeom interaction, string toolTip)
		: base(animation, interaction)
	{
		WpfBackColor = wpfBackColor;
		Background = background;
		BorderPen = borderPen;
		Rect = rect;
		Tag = tag;
		ToolTip = toolTip;
	}

	public StiBorderAnimationGeom(object background, StiPenGeom borderPen, object rect, object tag, StiAnimation animation, StiInteractionDataGeom interaction, string toolTip)
		: base(animation, interaction)
	{
		Background = background;
		BorderPen = borderPen;
		Rect = rect;
		Tag = tag;
		ToolTip = toolTip;
	}
}
