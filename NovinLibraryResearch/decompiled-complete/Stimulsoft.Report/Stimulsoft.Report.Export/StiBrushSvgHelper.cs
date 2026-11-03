using System;
using System.Drawing;
using System.Text;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public static class StiBrushSvgHelper
{
	private static string[] hatchData = new string[54]
	{
		"000000FF00000000", "1010101010101010", "8040201008040201", "0102040810204080", "101010FF10101010", "8142241818244281", "8000000008000000", "0010000100100001", "2200880022008800", "2288228822882288",
		"2255885522558855", "AA558A55AA55A855", "AA55AA55AA55AA55", "BB55EE55BB55EE55", "DD77DD77DD77DD77", "FFDDFF77FFDDFF77", "FF7FFFF7FF7FFFF7", "FF7FFFFFFFF7FFFF", "8844221188442211", "1122448811224488",
		"CC663399CC663399", "993366CC993366CC", "E070381C0E0783C1", "C183070E1C3870E0", "4040404040404040", "00FF000000FF0000", "AAAAAAAAAAAAAAAA", "FF00FF00FF00FF00", "CCCCCCCCCCCCCCCC", "FFFF0000FFFF0000",
		"8844221100000000", "1122448800000000", "F00000000F000000", "8080808008080808", "0240088004200110", "0C8DB130031BD8C0", "8403304884033048", "00304A8100304A81", "0102040818244281", "202020FF020202FF",
		"1422518854224588", "F0F0F0F0AA55AA55", "0100201020000102", "AA00800080008000", "0020008800020088", "8448300C02010103", "33FFCCFF33FFCCFF", "98F8F877898F8F77", "111111FF111111FF", "3333CCCC3333CCCC",
		"0F0F0F0FF0F0F0F0", "0502058850205088", "10387CFE7C381000", "0000000000000000"
	};

	public static string WriteGlareBrush(XmlTextWriter writer, object brush, RectangleF rect)
	{
		StiGlareBrush val = (StiGlareBrush)((brush is StiGlareBrush) ? brush : null);
		double num = 1.0;
		double num2 = 1.0;
		double num3 = val.Angle;
		if (num3 < 0.0)
		{
			num3 += 360.0;
		}
		if (num3 >= 270.0 && num3 < 360.0)
		{
			num3 = 360.0 - num3;
			num2 = -1.0;
		}
		if (num3 >= 180.0 && num3 < 270.0)
		{
			num3 -= 180.0;
			num2 = -1.0;
			num = -1.0;
		}
		if (num3 >= 90.0 && num3 < 180.0)
		{
			num3 = 180.0 - num3;
			num = -1.0;
		}
		num3 = num3 * Math.PI / 180.0;
		double num4 = rect.X + rect.Width / 2f;
		double num5 = rect.Y + rect.Height / 2f;
		double num6 = Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height) / 2.0;
		double num7 = Math.Atan2(rect.Height, rect.Width);
		double a = Math.PI / 2.0 - num3 + num7;
		double num8 = num6 * Math.Sin(a);
		double num9 = num8 * Math.Cos(num3) * num;
		double num10 = num8 * Math.Sin(num3) * (0.0 - num2);
		double num11 = num4 - num9;
		double num12 = num4 + num9;
		double num13 = num5 + num10;
		double num14 = num5 - num10;
		string text = $"gradient{StiGuidUtils.NewGuid()}";
		writer.WriteStartElement("linearGradient");
		writer.WriteAttributeString("id", text);
		writer.WriteAttributeString("gradientUnits", "userSpaceOnUse");
		writer.WriteAttributeString("x1", num11.ToString().Replace(",", "."));
		writer.WriteAttributeString("y1", num13.ToString().Replace(",", "."));
		writer.WriteAttributeString("x2", num12.ToString().Replace(",", "."));
		writer.WriteAttributeString("y2", num14.ToString().Replace(",", "."));
		writer.WriteStartElement("stop");
		writer.WriteAttributeString("offset", "0%");
		writer.WriteAttributeString("stop-color", $"#{val.StartColor.R:X2}{val.StartColor.G:X2}{val.StartColor.B:X2}");
		if (val.StartColor.A != byte.MaxValue)
		{
			writer.WriteAttributeString("stop-opacity", Math.Round((float)(int)val.StartColor.A / 255f, 3).ToString().Replace(",", "."));
		}
		writer.WriteEndElement();
		writer.WriteStartElement("stop");
		float num15 = val.Focus * 100f;
		writer.WriteAttributeString("offset", $"{num15}%");
		writer.WriteAttributeString("stop-color", $"#{val.EndColor.R:X2}{val.EndColor.G:X2}{val.EndColor.B:X2}");
		if (val.EndColor.A != byte.MaxValue)
		{
			writer.WriteAttributeString("stop-opacity", Math.Round((float)(int)val.EndColor.A / 255f, 3).ToString().Replace(",", "."));
		}
		writer.WriteEndElement();
		writer.WriteStartElement("stop");
		writer.WriteAttributeString("offset", "100%");
		writer.WriteAttributeString("stop-color", $"#{val.StartColor.R:X2}{val.StartColor.G:X2}{val.StartColor.B:X2}");
		if (val.StartColor.A != byte.MaxValue)
		{
			writer.WriteAttributeString("stop-opacity", Math.Round((float)(int)val.StartColor.A / 255f, 3).ToString().Replace(",", "."));
		}
		writer.WriteEndElement();
		writer.WriteFullEndElement();
		return text;
	}

	public static string WriteGradientBrush(XmlTextWriter writer, object brush, RectangleF rect)
	{
		StiGradientBrush val = (StiGradientBrush)((brush is StiGradientBrush) ? brush : null);
		double num = 1.0;
		double num2 = 1.0;
		double num3 = val.Angle;
		if (num3 < 0.0)
		{
			num3 += 360.0;
		}
		if (num3 >= 270.0 && num3 < 360.0)
		{
			num3 = 360.0 - num3;
			num2 = -1.0;
		}
		if (num3 >= 180.0 && num3 < 270.0)
		{
			num3 -= 180.0;
			num2 = -1.0;
			num = -1.0;
		}
		if (num3 >= 90.0 && num3 < 180.0)
		{
			num3 = 180.0 - num3;
			num = -1.0;
		}
		num3 = num3 * Math.PI / 180.0;
		double num4 = rect.X + rect.Width / 2f;
		double num5 = rect.Y + rect.Height / 2f;
		double num6 = Math.Sqrt(rect.Width * rect.Width + rect.Height * rect.Height) / 2.0;
		double num7 = Math.Atan2(rect.Height, rect.Width);
		double a = Math.PI / 2.0 - num3 + num7;
		double num8 = num6 * Math.Sin(a);
		double num9 = num8 * Math.Cos(num3) * num;
		double num10 = num8 * Math.Sin(num3) * (0.0 - num2);
		double num11 = num4 - num9;
		double num12 = num4 + num9;
		double num13 = num5 + num10;
		double num14 = num5 - num10;
		string text = $"gradient{StiGuidUtils.NewGuid()}";
		writer.WriteStartElement("linearGradient");
		writer.WriteAttributeString("id", text);
		writer.WriteAttributeString("gradientUnits", "userSpaceOnUse");
		writer.WriteAttributeString("x1", num11.ToString().Replace(",", "."));
		writer.WriteAttributeString("y1", num13.ToString().Replace(",", "."));
		writer.WriteAttributeString("x2", num12.ToString().Replace(",", "."));
		writer.WriteAttributeString("y2", num14.ToString().Replace(",", "."));
		writer.WriteStartElement("stop");
		writer.WriteAttributeString("offset", "0%");
		writer.WriteAttributeString("stop-color", $"#{val.StartColor.R:X2}{val.StartColor.G:X2}{val.StartColor.B:X2}");
		if (val.StartColor.A != byte.MaxValue)
		{
			writer.WriteAttributeString("stop-opacity", Math.Round((float)(int)val.StartColor.A / 255f, 3).ToString().Replace(",", "."));
		}
		writer.WriteEndElement();
		writer.WriteStartElement("stop");
		writer.WriteAttributeString("offset", "100%");
		writer.WriteAttributeString("stop-color", $"#{val.EndColor.R:X2}{val.EndColor.G:X2}{val.EndColor.B:X2}");
		if (val.EndColor.A != byte.MaxValue)
		{
			writer.WriteAttributeString("stop-opacity", Math.Round((float)(int)val.EndColor.A / 255f, 3).ToString().Replace(",", "."));
		}
		writer.WriteEndElement();
		writer.WriteFullEndElement();
		return text;
	}

	public static string WriteGlassBrush(XmlTextWriter writer, object brush, RectangleF rect)
	{
		StiGlassBrush val = (StiGlassBrush)((brush is StiGlassBrush) ? brush : null);
		Color topColor = val.GetTopColor();
		RectangleF topRectangle = val.GetTopRectangle(rect);
		Color bottomColor = val.GetBottomColor();
		RectangleF bottomRectangle = val.GetBottomRectangle(rect);
		string text = $"glass{StiGuidUtils.NewGuid()}";
		writer.WriteStartElement("pattern");
		writer.WriteAttributeString("id", text);
		writer.WriteAttributeString("x", rect.X.ToString());
		writer.WriteAttributeString("y", rect.Y.ToString());
		writer.WriteAttributeString("width", rect.Width.ToString());
		writer.WriteAttributeString("height", rect.Height.ToString());
		writer.WriteAttributeString("patternUnits", "userSpaceOnUse");
		writer.WriteStartElement("rect");
		writer.WriteAttributeString("x", "0");
		writer.WriteAttributeString("y", "0");
		writer.WriteAttributeString("width", rect.Width.ToString());
		writer.WriteAttributeString("height", rect.Height.ToString());
		writer.WriteAttributeString("style", string.Format("fill:rgb({0},{1},{2});fill-opacity:{3};", bottomColor.R, bottomColor.G, bottomColor.B, Math.Round((float)(int)bottomColor.A / 255f, 3).ToString().Replace(",", ".")));
		writer.WriteEndElement();
		writer.WriteStartElement("rect");
		writer.WriteAttributeString("x", "0");
		writer.WriteAttributeString("y", "0");
		writer.WriteAttributeString("width", topRectangle.Width.ToString());
		writer.WriteAttributeString("height", topRectangle.Height.ToString());
		writer.WriteAttributeString("style", string.Format("fill:rgb({0},{1},{2});fill-opacity:{3};", topColor.R, topColor.G, topColor.B, Math.Round((float)(int)topColor.A / 255f, 3).ToString().Replace(",", ".")));
		writer.WriteEndElement();
		writer.WriteEndElement();
		return text;
	}

	public static string WriteHatchBrush(XmlTextWriter writer, object brush)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected I4, but got Unknown
		StiHatchBrush val = (StiHatchBrush)((brush is StiHatchBrush) ? brush : null);
		Color foreColor = val.ForeColor;
		Color backColor = val.BackColor;
		int num = (int)val.Style;
		if (num > 53)
		{
			num = 53;
		}
		string text = $"hatch{StiGuidUtils.NewGuid()}";
		writer.WriteStartElement("pattern");
		writer.WriteAttributeString("id", text);
		writer.WriteAttributeString("x", "0");
		writer.WriteAttributeString("y", "0");
		writer.WriteAttributeString("width", "8");
		writer.WriteAttributeString("height", "8");
		writer.WriteAttributeString("patternUnits", "userSpaceOnUse");
		StringBuilder stringBuilder = new StringBuilder();
		string text2 = hatchData[num];
		for (int i = 0; i < 16; i++)
		{
			stringBuilder.Append(HexToByteString(text2.Substring(i, 1)));
		}
		writer.WriteStartElement("rect");
		writer.WriteAttributeString("x", "0");
		writer.WriteAttributeString("y", "0");
		writer.WriteAttributeString("width", "8");
		writer.WriteAttributeString("height", "8");
		writer.WriteAttributeString("fill", $"#{backColor.R:X2}{backColor.G:X2}{backColor.B:X2}");
		writer.WriteEndElement();
		for (int j = 0; j < 8; j++)
		{
			for (int k = 0; k < 8; k++)
			{
				if (stringBuilder[j * 8 + k].ToString() == "1")
				{
					writer.WriteStartElement("rect");
					writer.WriteAttributeString("x", k.ToString());
					writer.WriteAttributeString("y", j.ToString());
					writer.WriteAttributeString("width", "1");
					writer.WriteAttributeString("height", "1");
					writer.WriteAttributeString("fill", $"#{foreColor.R:X2}{foreColor.G:X2}{foreColor.B:X2}");
					writer.WriteEndElement();
				}
			}
		}
		writer.WriteEndElement();
		return text;
	}

	private static string HexToByteString(string hex)
	{
		string result = "0000";
		switch (hex)
		{
		case "1":
			result = "0001";
			break;
		case "2":
			result = "0010";
			break;
		case "3":
			result = "0011";
			break;
		case "4":
			result = "0100";
			break;
		case "5":
			result = "0101";
			break;
		case "6":
			result = "0110";
			break;
		case "7":
			result = "0111";
			break;
		case "8":
			result = "1000";
			break;
		case "9":
			result = "1001";
			break;
		case "A":
			result = "1010";
			break;
		case "B":
			result = "1011";
			break;
		case "C":
			result = "1100";
			break;
		case "D":
			result = "1101";
			break;
		case "E":
			result = "1110";
			break;
		case "F":
			result = "1111";
			break;
		}
		return result;
	}

	public static string GetFillBrush(XmlTextWriter writer, object brush, RectangleF sRect)
	{
		if (brush is Color color)
		{
			if (color.A == 0)
			{
				return "transparent";
			}
			return string.Format("rgba({0},{1},{2},{3})", color.R, color.G, color.B, Math.Round((float)(int)color.A / 255f, 3).ToString().Replace(",", "."));
		}
		if (brush is StiGradientBrush)
		{
			string arg = WriteGradientBrush(writer, brush, sRect);
			return $"url(#{arg})";
		}
		if (brush is StiGlareBrush)
		{
			string arg2 = WriteGlareBrush(writer, brush, sRect);
			return $"url(#{arg2})";
		}
		if (brush is StiGlassBrush)
		{
			string arg3 = WriteGlassBrush(writer, brush, sRect);
			return $"url(#{arg3})";
		}
		if (brush is StiHatchBrush)
		{
			string arg4 = WriteHatchBrush(writer, brush);
			return $"url(#{arg4})";
		}
		if (brush is StiBrush || brush is SolidBrush)
		{
			Color color2 = ((brush is StiBrush) ? StiBrush.ToColor((StiBrush)((brush is StiBrush) ? brush : null)) : ((SolidBrush)((brush is SolidBrush) ? brush : null)).Color);
			return string.Format("rgba({0},{1},{2},{3})", color2.R, color2.G, color2.B, Math.Round((float)(int)color2.A / 255f, 3).ToString().Replace(",", "."));
		}
		return "none";
	}
}
