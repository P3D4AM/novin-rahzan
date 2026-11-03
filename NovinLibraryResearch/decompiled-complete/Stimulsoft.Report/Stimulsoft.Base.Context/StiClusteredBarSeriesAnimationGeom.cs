using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiClusteredBarSeriesAnimationGeom : StiAnimationGeom
{
	public StiBrush WpfBackColor { get; set; }

	public object Background { get; set; }

	public StiPenGeom BorderPen { get; set; }

	public object ColumnRect { get; set; }

	public string ToolTip { get; set; }

	public object Tag { get; set; }

	public double? Value { get; set; }

	public override StiGeomType Type => StiGeomType.AnimationBar;

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
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		if (Background != null)
		{
			((JContainer)val).Add((object)new JProperty("Background", (object)SaveBrushToJsonObject(Background, mode)));
		}
		if (BorderPen != null)
		{
			((JContainer)val).Add((object)new JProperty("BorderPen", (object)BorderPen.SaveToJsonObject(mode)));
		}
		((JContainer)val).Add((object)new JProperty("ColumnRect", (object)SaveRectToJsonObject(ColumnRect)));
		if (Tag is string)
		{
			((JContainer)val).Add((object)new JProperty("Tag", (object)(Tag as string)));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiClusteredBarSeriesAnimationGeom(StiBrush wpfBackColor, object background, StiPenGeom borderPen, object columnRect, double? value, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction)
		: base(animation, interaction)
	{
		WpfBackColor = wpfBackColor;
		Value = value;
		Background = background;
		BorderPen = borderPen;
		ColumnRect = columnRect;
		ToolTip = toolTip;
		Tag = tag;
	}

	public StiClusteredBarSeriesAnimationGeom(object background, StiPenGeom borderPen, object columnRect, double? value, string toolTip, object tag, StiAnimation animation, StiInteractionDataGeom interaction)
		: base(animation, interaction)
	{
		Value = value;
		Background = background;
		BorderPen = borderPen;
		ColumnRect = columnRect;
		ToolTip = toolTip;
		Tag = tag;
	}
}
