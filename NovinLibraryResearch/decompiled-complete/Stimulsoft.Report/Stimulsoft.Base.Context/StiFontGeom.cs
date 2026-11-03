using System.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiFontGeom : StiGeom
{
	public FontFamily FontFamily { get; set; }

	public string FontName { get; set; }

	public float FontSize { get; set; }

	public FontStyle FontStyle { get; set; }

	public GraphicsUnit Unit { get; set; }

	public byte GdiCharSet { get; set; }

	public bool GdiVerticalFont { get; set; }

	public override StiGeomType Type => StiGeomType.Font;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("FontName", (object)FontName));
		((JContainer)val).Add((object)new JProperty("FontSize", (object)FontSize));
		FontStyle fontStyle = FontStyle;
		((JContainer)val).Add((object)new JProperty("FontStyle", (object)((object)(FontStyle)(ref fontStyle)).ToString()));
		GraphicsUnit unit = Unit;
		((JContainer)val).Add((object)new JProperty("Unit", (object)((object)(GraphicsUnit)(ref unit)).ToString()));
		((JContainer)val).Add((object)new JProperty("GdiCharSet", (object)GdiCharSet));
		((JContainer)val).Add((object)new JProperty("GdiVerticalFont", (object)GdiVerticalFont));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public static StiFontGeom ChangeFontSize(Font font, float newFontSize)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (newFontSize < 1f)
		{
			newFontSize = 1f;
		}
		return new StiFontGeom(font.FontFamily.Name, newFontSize, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
	}

	public StiFontGeom(Font font)
		: this(font.FontFamily.Name, font.Size, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont)
	{
	}//IL_0013: Unknown result type (might be due to invalid IL or missing references)
	//IL_0019: Unknown result type (might be due to invalid IL or missing references)


	public StiFontGeom(string fontName, float fontSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
		: this(null, fontName, fontSize, style, unit, gdiCharSet, gdiVerticalFont)
	{
	}//IL_0004: Unknown result type (might be due to invalid IL or missing references)
	//IL_0005: Unknown result type (might be due to invalid IL or missing references)


	public StiFontGeom(FontFamily fontFamily, string fontName, float fontSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		FontFamily = fontFamily;
		FontName = fontName;
		FontSize = fontSize;
		FontStyle = style;
		Unit = unit;
		GdiCharSet = gdiCharSet;
		GdiVerticalFont = gdiVerticalFont;
	}
}
