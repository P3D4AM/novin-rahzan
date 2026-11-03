using System.Drawing;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiLabelAnimationGeom : StiAnimationGeom
{
	public string Text { get; set; }

	public StiFontGeom Font { get; set; }

	public object TextBrush { get; set; }

	public object LabelBrush { get; set; }

	public StiPenGeom PenBorder { get; set; }

	public Rectangle Rectangle { get; set; }

	public StiStringFormatGeom StringFormat { get; set; }

	public float Angle { get; set; }

	public StiRotationMode? RotationMode { get; set; }

	public bool DrawBorder { get; set; }

	public override StiGeomType Type => StiGeomType.AnimationLabel;

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
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Text", (object)Text));
		((JContainer)val).Add((object)new JProperty("Font", (object)Font.SaveToJsonObject(mode)));
		((JContainer)val).Add((object)new JProperty("Rectangle", (object)StiGeom.SaveRectangleToJsonObject(Rectangle)));
		((JContainer)val).Add((object)new JProperty("Angle", (object)Angle));
		((JContainer)val).Add((object)new JProperty("DrawBorder", (object)DrawBorder));
		if (TextBrush != null)
		{
			((JContainer)val).Add((object)new JProperty("TextBrush", (object)SaveBrushToJsonObject(TextBrush, mode)));
		}
		if (LabelBrush != null)
		{
			((JContainer)val).Add((object)new JProperty("LabelBrush", (object)SaveBrushToJsonObject(LabelBrush, mode)));
		}
		if (PenBorder != null)
		{
			((JContainer)val).Add((object)new JProperty("PenBorder", (object)PenBorder.SaveToJsonObject(mode)));
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
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiLabelAnimationGeom(string text, StiFontGeom font, object textBrush, object LabelBrush, StiPenGeom penBorder, Rectangle rect, StiStringFormatGeom sf, StiRotationMode rotationMode, float angle, bool drawBorder, StiAnimation animation)
		: base(animation)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		Font = font;
		TextBrush = textBrush;
		this.LabelBrush = LabelBrush;
		PenBorder = penBorder;
		Rectangle = rect;
		StringFormat = sf;
		RotationMode = rotationMode;
		Angle = angle;
		DrawBorder = drawBorder;
	}
}
