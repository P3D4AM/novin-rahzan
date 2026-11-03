using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiBorderGeom : StiGeom
{
	public object Background { get; set; }

	public object BackgroundMouseOver { get; set; }

	public StiPenGeom BorderPen { get; set; }

	public object Rect { get; set; }

	public StiInteractionDataGeom Interaction { get; set; }

	public int ElementIndex { get; set; }

	public float Angle { get; set; }

	public StiRotationMode RotationMode { get; set; }

	public override StiGeomType Type => StiGeomType.Border;

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

	public StiBorderGeom(object background, object backgroundMouseOver, StiPenGeom borderPen, object rect, StiInteractionDataGeom interaction, int elementIndex, float angle, StiRotationMode rotationMode)
		: this(background, backgroundMouseOver, borderPen, rect, interaction, elementIndex)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Angle = angle;
		RotationMode = rotationMode;
	}

	public StiBorderGeom(object background, object backgroundMouseOver, StiPenGeom borderPen, object rect, StiInteractionDataGeom interaction, int elementIndex)
	{
		Background = background;
		BackgroundMouseOver = backgroundMouseOver;
		BorderPen = borderPen;
		Rect = rect;
		Interaction = interaction;
		ElementIndex = elementIndex;
	}
}
