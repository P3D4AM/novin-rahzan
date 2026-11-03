using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiPenGeom : StiGeom
{
	public object Brush { get; set; }

	public float Thickness { get; set; } = 1f;


	public StiPenStyle PenStyle { get; set; }

	public StiPenAlignment Alignment { get; set; }

	public StiPenLineCap StartCap { get; set; }

	public StiPenLineCap EndCap { get; set; }

	public override StiGeomType Type => StiGeomType.Pen;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		if (Brush != null)
		{
			((JContainer)val).Add((object)new JProperty("Brush", (object)SaveBrushToJsonObject(Brush, mode)));
		}
		((JContainer)val).Add((object)new JProperty("Thickness", (object)Thickness));
		StiPenStyle penStyle = PenStyle;
		((JContainer)val).Add((object)new JProperty("PenStyle", (object)((object)(StiPenStyle)(ref penStyle)).ToString()));
		((JContainer)val).Add((object)new JProperty("Alignment", (object)Alignment.ToString()));
		StiPenLineCap val2 = StartCap;
		((JContainer)val).Add((object)new JProperty("StartCap", (object)((object)(StiPenLineCap)(ref val2)).ToString()));
		val2 = EndCap;
		((JContainer)val).Add((object)new JProperty("EndCap", (object)((object)(StiPenLineCap)(ref val2)).ToString()));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiPenGeom(object brush)
		: this(brush, 1f)
	{
	}

	public StiPenGeom(object brush, float thickness)
	{
		Brush = brush;
		Thickness = thickness;
	}
}
