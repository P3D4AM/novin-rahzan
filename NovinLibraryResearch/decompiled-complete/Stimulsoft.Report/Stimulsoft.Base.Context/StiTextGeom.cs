using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiTextGeom : StiAnimationGeom
{
	public string Text { get; set; }

	public StiFontGeom Font { get; set; }

	public bool IsRounded { get; set; }

	public bool IsRotatedText { get; set; }

	public object Brush { get; set; }

	public object Location { get; set; }

	public StiStringFormatGeom StringFormat { get; set; }

	public float Angle { get; set; }

	public bool Antialiasing { get; set; }

	public int? MaximalWidth { get; set; }

	public StiRotationMode? RotationMode { get; set; }

	public string ToolTip { get; set; }

	public int ElementIndex { get; set; } = -1;


	public override StiGeomType Type => StiGeomType.Text;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Text", (object)Text));
		((JContainer)val).Add((object)new JProperty("Font", (object)Font.SaveToJsonObject(mode)));
		((JContainer)val).Add((object)new JProperty("IsRounded", (object)IsRounded));
		((JContainer)val).Add((object)new JProperty("IsRotatedText", (object)IsRotatedText));
		((JContainer)val).Add((object)new JProperty("Angle", (object)Angle));
		((JContainer)val).Add((object)new JProperty("Antialiasing", (object)Antialiasing));
		((JContainer)val).Add((object)new JProperty("MaximalWidth", (object)MaximalWidth));
		if (Brush != null)
		{
			((JContainer)val).Add((object)new JProperty("Brush", (object)SaveBrushToJsonObject(Brush, mode)));
		}
		if (StringFormat != null)
		{
			((JContainer)val).Add((object)new JProperty("StringFormat", (object)StringFormat.SaveToJsonObject(mode)));
		}
		if (RotationMode.HasValue)
		{
			StiRotationMode value = RotationMode.Value;
			((JContainer)val).Add((object)new JProperty("RotationMode", (object)((object)(StiRotationMode)(ref value)).ToString()));
		}
		if (Location is PointF)
		{
			((JContainer)val).Add((object)new JProperty("Location", (object)StiGeom.SavePointFToJsonObject((PointF)Location)));
		}
		if (Location is Rectangle)
		{
			((JContainer)val).Add((object)new JProperty("Location", (object)StiGeom.SaveRectangleToJsonObject((Rectangle)Location)));
		}
		if (Location is RectangleF)
		{
			((JContainer)val).Add((object)new JProperty("Location", (object)StiGeom.SaveRectangleFToJsonObject((RectangleF)Location)));
		}
		if (Location is RectangleD)
		{
			((JContainer)val).Add((object)new JProperty("Location", (object)StiGeom.SaveRectangleDToJsonObject((RectangleD)Location)));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, bool isRotatedText, int elementIndex = -1)
		: this(text, font, brush, location, stringFormat, 0f, antialiasing: false, null, null, isRotatedText)
	{
		ElementIndex = elementIndex;
	}

	public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, float angle, bool antialiasing, bool isRotatedText)
		: this(text, font, brush, location, stringFormat, angle, antialiasing, null, null, isRotatedText)
	{
	}

	public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, float angle, bool antialiasing, StiRotationMode? rotationMode, bool isRotatedText)
		: this(text, font, brush, location, stringFormat, angle, antialiasing, null, rotationMode, isRotatedText)
	{
	}

	public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, float angle, bool antialiasing, int? maximalWidth, StiRotationMode? rotationMode, bool isRotatedText)
		: this(text, font, brush, location, stringFormat, angle, antialiasing, maximalWidth, rotationMode, isRotatedText, null)
	{
	}

	public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, float angle, bool antialiasing, int? maximalWidth, StiRotationMode? rotationMode, bool isRotatedText, string toolTip, StiAnimation animation = null)
		: base(animation)
	{
		IsRotatedText = isRotatedText;
		Text = text;
		Font = font;
		Brush = brush;
		Location = location;
		StringFormat = stringFormat;
		Angle = angle;
		Antialiasing = antialiasing;
		MaximalWidth = maximalWidth;
		RotationMode = rotationMode;
		ToolTip = toolTip;
	}
}
