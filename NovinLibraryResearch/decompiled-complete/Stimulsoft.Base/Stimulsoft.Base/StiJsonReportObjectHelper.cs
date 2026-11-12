using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public static class StiJsonReportObjectHelper
{
	public static class Serialize
	{
		public static string FontTahoma12Bold(Font font)
		{
			return Font(font, "Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3);
		}

		public static string FontArial8(Font font)
		{
			return Font(font, "Arial", 8f, (FontStyle)0, (GraphicsUnit)3);
		}

		public static string FontMicrosoftSansSerif8(Font font)
		{
			return Font(font, "Microsoft Sans Serif", 8f, (FontStyle)0, (GraphicsUnit)3);
		}

		public static string FontTahoma8(Font font)
		{
			return Font(font, "Tahoma", 8f, (FontStyle)0, (GraphicsUnit)3);
		}

		public static string FontArial8BoldPixel(Font font)
		{
			return Font(font, "Arial", 8f, (FontStyle)1, (GraphicsUnit)2);
		}

		public static string FontArial7(Font font)
		{
			return Font(font, "Arial", 7f, (FontStyle)0, (GraphicsUnit)3);
		}

		public static string FontArial10(Font font)
		{
			return Font(font, "Arial", 10f, (FontStyle)0, (GraphicsUnit)3);
		}

		public static string FontArial13(Font font)
		{
			return Font(font, "Arial", 13f, (FontStyle)0, (GraphicsUnit)3);
		}

		public static string FontArial11(Font font)
		{
			return Font(font, "Arial", 11f, (FontStyle)0, (GraphicsUnit)3);
		}

		public static string FontArial12Bold(Font font)
		{
			return Font(font, "Arial", 12f, (FontStyle)1);
		}

		public static string FontArial14Bold(Font font)
		{
			return Font(font, "Arial", 14f, (FontStyle)1);
		}

		public static string Font(Font font, string defaultFamily, float defaultEmSize)
		{
			return Font(font, defaultFamily, defaultEmSize, (FontStyle)0, (GraphicsUnit)3);
		}

		public static string Font(Font font, string defaultFamily, float defaultEmSize, FontStyle fontStyle)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			return Font(font, defaultFamily, defaultEmSize, fontStyle, (GraphicsUnit)3);
		}

		public static string Font(Font font, string defaultFamily, float defaultEmSize, FontStyle defaultStyle, GraphicsUnit defaultUnit)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			if (font == null)
			{
				return string.Empty;
			}
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			int num = 0;
			if (font.FontFamily.Name != defaultFamily)
			{
				num++;
				text = font.FontFamily.Name;
			}
			if (font.Size != defaultEmSize)
			{
				num++;
				text2 = font.Size.ToString();
			}
			if (font.Style != defaultStyle)
			{
				num++;
				FontStyle style = font.Style;
				text3 = ((object)(FontStyle)(ref style)).ToString();
			}
			if (font.Unit != defaultUnit)
			{
				num++;
				GraphicsUnit unit = font.Unit;
				text4 = ((object)(GraphicsUnit)(ref unit)).ToString();
			}
			if (num == 0)
			{
				return null;
			}
			return text + ";" + text2 + ";" + text3 + ";" + text4;
		}

		public static string RectangleD(RectangleD rect)
		{
			return rect.X + "," + rect.Y + "," + rect.Width + "," + rect.Height;
		}

		public static string SizeD(SizeD size)
		{
			return size.Width + "," + size.Height;
		}

		public static string JColor(Color color, Color defColor)
		{
			if (color == defColor)
			{
				return null;
			}
			return JColor(color);
		}

		public static string JColor(Color color)
		{
			if (color.IsNamedColor)
			{
				return color.Name;
			}
			if (color.A == byte.MaxValue)
			{
				return color.R + "," + color.G + "," + color.B;
			}
			return color.A + "," + color.R + "," + color.G + "," + color.B;
		}

		public static JObject StringArray(string[] array)
		{
			if (array == null || array.Length == 0)
			{
				return null;
			}
			JObject jObject = new JObject();
			for (int i = 0; i < array.Length; i++)
			{
				jObject.AddPropertyString(i.ToString(), array[i]);
			}
			return jObject;
		}

		public static JObject ListString(List<string> array)
		{
			if (array == null || array.Count == 0)
			{
				return null;
			}
			JObject jObject = new JObject();
			for (int i = 0; i < array.Count; i++)
			{
				jObject.AddPropertyString(i.ToString(), array[i]);
			}
			return jObject;
		}

		public static JObject BoolArray(bool[] array, bool ignoreDefaultValues = false)
		{
			if (array == null || array.Length == 0)
			{
				return null;
			}
			JObject jObject = new JObject();
			for (int i = 0; i < array.Length; i++)
			{
				jObject.AddPropertyBool(i.ToString(), array[i], defaultValue: false, ignoreDefaultValues);
			}
			return jObject;
		}

		public static JObject IntArray(int[] array)
		{
			if (array == null || array.Length == 0)
			{
				return null;
			}
			JObject jObject = new JObject();
			for (int i = 0; i < array.Length; i++)
			{
				jObject.AddPropertyStringNullOrEmpty(i.ToString(), array[i].ToString());
			}
			return jObject;
		}

		public static JObject JObjectCollection(IEnumerable<IStiJsonReportObject> list, StiJsonSaveMode mode)
		{
			if (list == null || list.Count() == 0)
			{
				return null;
			}
			JObject jObject = new JObject();
			int num = 0;
			foreach (IStiJsonReportObject item in list)
			{
				jObject.AddPropertyJObject(num.ToString(), item.SaveToJsonObject(mode));
				num++;
			}
			return jObject;
		}

		public static JObject Size(Size size)
		{
			JObject jObject = new JObject();
			jObject.AddPropertyInt("Width", size.Width);
			jObject.AddPropertyInt("Height", size.Height);
			return jObject;
		}

		public static JObject Point(Point pos)
		{
			JObject jObject = new JObject();
			jObject.AddPropertyStringNullOrEmpty("X", pos.X.ToString());
			jObject.AddPropertyStringNullOrEmpty("Y", pos.Y.ToString());
			return jObject;
		}

		public static JObject PointF(PointF pos)
		{
			JObject jObject = new JObject();
			jObject.AddPropertyStringNullOrEmpty("X", pos.X.ToString());
			jObject.AddPropertyStringNullOrEmpty("Y", pos.Y.ToString());
			return jObject;
		}

		public static string JCap(StiCap cap)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (cap.Width != 10)
			{
				stringBuilder.Append(cap.Width);
			}
			stringBuilder.Append(";");
			if (cap.Height != 10)
			{
				stringBuilder.Append(cap.Height);
			}
			stringBuilder.Append(";");
			if (cap.Style != 0)
			{
				stringBuilder.Append(cap.Style.ToString());
			}
			stringBuilder.Append(";");
			stringBuilder.Append(JColor(cap.Color, Color.Black));
			return stringBuilder.ToString();
		}

		public static string JBrush(StiBrush brush, StiBrush defaultBrush = null)
		{
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Invalid comparison between Unknown and I4
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder stringBuilder = new StringBuilder();
			if (brush is StiStyleBrush)
			{
				stringBuilder.Append("style");
			}
			if (brush is StiDefaultBrush)
			{
				stringBuilder.Append("default");
			}
			else if (brush is StiSolidBrush)
			{
				StiSolidBrush stiSolidBrush = (StiSolidBrush)brush;
				stringBuilder.Append("solid:");
				stringBuilder.Append(JColor(stiSolidBrush.Color, Color.Transparent));
			}
			else if (brush is StiEmptyBrush)
			{
				stringBuilder.Append("empty");
			}
			else if (brush is StiGlareBrush)
			{
				StiGlareBrush stiGlareBrush = (StiGlareBrush)brush;
				stringBuilder.Append("glare:");
				stringBuilder.Append(JColor(stiGlareBrush.StartColor, Color.Black));
				stringBuilder.Append(":");
				stringBuilder.Append(JColor(stiGlareBrush.EndColor, Color.White));
				stringBuilder.Append(":");
				if (stiGlareBrush.Angle != 0.0)
				{
					stringBuilder.Append(stiGlareBrush.Angle);
				}
				stringBuilder.Append(":");
				if (stiGlareBrush.Focus != 0.5f)
				{
					stringBuilder.Append(stiGlareBrush.Focus);
				}
				stringBuilder.Append(":");
				if (stiGlareBrush.Scale != 1f)
				{
					stringBuilder.Append(stiGlareBrush.Scale);
				}
			}
			else if (brush is StiGlassBrush)
			{
				StiGlassBrush stiGlassBrush = (StiGlassBrush)brush;
				stringBuilder.Append("glass:");
				stringBuilder.Append(JColor(stiGlassBrush.Color, Color.Silver));
				stringBuilder.Append(":");
				if (stiGlassBrush.DrawHatch)
				{
					stringBuilder.Append(stiGlassBrush.DrawHatch);
				}
				stringBuilder.Append(":");
				if (stiGlassBrush.Blend != 0.2f)
				{
					stringBuilder.Append(stiGlassBrush.Blend);
				}
			}
			else if (brush is StiGradientBrush)
			{
				StiGradientBrush stiGradientBrush = (StiGradientBrush)brush;
				stringBuilder.Append("gradient:");
				stringBuilder.Append(JColor(stiGradientBrush.StartColor, Color.Black));
				stringBuilder.Append(":");
				stringBuilder.Append(JColor(stiGradientBrush.EndColor, Color.White));
				stringBuilder.Append(":");
				if (stiGradientBrush.Angle != 0.0)
				{
					stringBuilder.Append(stiGradientBrush.Angle);
				}
			}
			else if (brush is StiHatchBrush)
			{
				StiHatchBrush stiHatchBrush = (StiHatchBrush)brush;
				stringBuilder.Append("hatch:");
				stringBuilder.Append(JColor(stiHatchBrush.BackColor, Color.Black));
				stringBuilder.Append(":");
				stringBuilder.Append(JColor(stiHatchBrush.ForeColor, Color.White));
				stringBuilder.Append(":");
				if ((int)stiHatchBrush.Style != 3)
				{
					stringBuilder.Append(stiHatchBrush.Style);
				}
			}
			return stringBuilder.ToString();
		}

		public static string JBorderSide(StiBorderSide side)
		{
			string text = JColor(side.Color, Color.Black);
			string text2 = string.Empty;
			if (side.Size != 1.0)
			{
				text2 = side.Size.ToString();
			}
			string text3 = string.Empty;
			if (side.Style != StiPenStyle.None)
			{
				text3 = side.Style.ToString();
			}
			return text + ":" + text2 + ":" + text3;
		}

		public static string JBorder(StiBorder border)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StiAdvancedBorder stiAdvancedBorder = border as StiAdvancedBorder;
			if (stiAdvancedBorder != null)
			{
				stringBuilder.Append(JBorderSide(stiAdvancedBorder.TopSide));
				stringBuilder.Append(";");
				stringBuilder.Append(JBorderSide(stiAdvancedBorder.BottomSide));
				stringBuilder.Append(";");
				stringBuilder.Append(JBorderSide(stiAdvancedBorder.LeftSide));
				stringBuilder.Append(";");
				stringBuilder.Append(JBorderSide(stiAdvancedBorder.RightSide));
				stringBuilder.Append(";");
				if (border.DropShadow)
				{
					stringBuilder.Append(border.DropShadow);
				}
				stringBuilder.Append(";");
				if (border.ShadowSize != 4.0)
				{
					stringBuilder.Append(border.ShadowSize);
				}
				stringBuilder.Append(";");
				stringBuilder.Append(JBrush(border.ShadowBrush, new StiSolidBrush(Color.Black)));
			}
			else
			{
				if (border.Side != 0)
				{
					stringBuilder.Append(border.Side);
				}
				stringBuilder.Append(";");
				if (border.Color != Color.Black)
				{
					stringBuilder.Append(JColor(border.Color));
				}
				stringBuilder.Append(";");
				if (border.Size != 1.0)
				{
					stringBuilder.Append(border.Size);
				}
				stringBuilder.Append(";");
				if (border.Style != 0)
				{
					stringBuilder.Append(border.Style);
				}
				stringBuilder.Append(";");
				if (border.ShadowSize != 4.0)
				{
					stringBuilder.Append(border.ShadowSize);
				}
				stringBuilder.Append(";");
				if (border.DropShadow)
				{
					stringBuilder.Append(border.DropShadow);
				}
				stringBuilder.Append(";");
				if (border.Topmost)
				{
					stringBuilder.Append(border.Topmost);
				}
				stringBuilder.Append(";");
				stringBuilder.Append(JBrush(border.ShadowBrush, new StiSolidBrush(Color.Black)));
			}
			return stringBuilder.ToString();
		}

		public static string JBorder(StiSimpleBorder border)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (border.Side != 0)
			{
				stringBuilder.Append(border.Side);
			}
			stringBuilder.Append(";");
			if (border.Color != Color.Gray)
			{
				stringBuilder.Append(JColor(border.Color));
			}
			stringBuilder.Append(";");
			if (border.Size != 1.0)
			{
				stringBuilder.Append(border.Size);
			}
			stringBuilder.Append(";");
			if (border.Style != 0)
			{
				stringBuilder.Append(border.Style);
			}
			stringBuilder.Append(";");
			return stringBuilder.ToString();
		}

		public static string JShadow(StiSimpleShadow shadow)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (shadow.Color != StiColor.Get("#44222222"))
			{
				stringBuilder.Append(JColor(shadow.Color));
			}
			stringBuilder.Append(";");
			if (shadow.Location.X != 2 || shadow.Location.Y != 2)
			{
				stringBuilder.Append(shadow.Location.X + "," + shadow.Location.Y);
			}
			stringBuilder.Append(";");
			if (shadow.Size != 5)
			{
				stringBuilder.Append(shadow.Size);
			}
			stringBuilder.Append(";");
			if (shadow.Visible)
			{
				stringBuilder.Append(shadow.Visible);
			}
			return stringBuilder.ToString();
		}
	}

	public static class Deserialize
	{
		public static List<string> ListString(JObject jObject)
		{
			List<string> list = new List<string>();
			int num = 0;
			foreach (JProperty item in jObject.Properties())
			{
				list.Add(item.Value.ToObject<string>());
				num++;
			}
			return list;
		}

		public static string[] StringArray(JObject jObject)
		{
			string[] array = new string[jObject.Count];
			int num = 0;
			foreach (JProperty item in jObject.Properties())
			{
				array[num] = item.Value.ToObject<string>();
				num++;
			}
			return array;
		}

		public static bool[] BoolArray(JObject jObject)
		{
			bool[] array = new bool[jObject.Count];
			int num = 0;
			foreach (JProperty item in jObject.Properties())
			{
				array[num] = item.Value.ToObject<bool>();
				num++;
			}
			return array;
		}

		public static int[] IntArray(JObject jObject)
		{
			int[] array = new int[jObject.Count];
			int num = 0;
			foreach (JProperty item in jObject.Properties())
			{
				array[num] = item.Value.ToObject<int>();
				num++;
			}
			return array;
		}

		public static Font Font(JProperty prop, Font defaultFont)
		{
			if (prop.Value is JValue)
			{
				return Font((string)((JValue)prop.Value).Value, defaultFont);
			}
			return Font((JObject)prop.Value, defaultFont);
		}

		private static Font Font(string text, Font defaultFont)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			string[] array = text.Split(new char[1] { ';' });
			if (array.Length != 4)
			{
				throw new Exception("Parsing Error");
			}
			string text2 = defaultFont.FontFamily.Name;
			float num = defaultFont.Size;
			FontStyle val = defaultFont.Style;
			GraphicsUnit val2 = defaultFont.Unit;
			if (!string.IsNullOrEmpty(array[0]))
			{
				text2 = array[0];
			}
			if (!string.IsNullOrEmpty(array[1]))
			{
				num = float.Parse(array[1]);
			}
			if (!string.IsNullOrEmpty(array[2]))
			{
				val = (FontStyle)Enum.Parse(typeof(FontStyle), array[2]);
			}
			if (!string.IsNullOrEmpty(array[3]))
			{
				val2 = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), array[3]);
			}
			FontFamily fontFamily = StiFontCollection.GetFontFamily(text2, allowNullResult: true);
			if (fontFamily == null)
			{
				return new Font(text2, num, val, val2);
			}
			return new Font(fontFamily, num, val, val2);
		}

		private static Font Font(JObject jObject, Font defaultFont)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			string text = defaultFont.FontFamily.Name;
			float num = defaultFont.Size;
			FontStyle val = defaultFont.Style;
			GraphicsUnit val2 = defaultFont.Unit;
			foreach (JProperty item in jObject.Properties())
			{
				switch (item.Name)
				{
				case "FamilyName":
					text = item.Value.ToObject<string>();
					break;
				case "Size":
					num = item.Value.ToObject<float>();
					break;
				case "Style":
					val = (FontStyle)Enum.Parse(typeof(FontStyle), item.Value.ToObject<string>());
					break;
				case "Unit":
					val2 = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), item.Value.ToObject<string>());
					break;
				}
			}
			return new Font(text, num, val, val2);
		}

		public static StiBorderSide JBorderSide(string text)
		{
			string[] array = text.Split(new char[1] { ':' });
			StiBorderSide stiBorderSide = new StiBorderSide();
			if (!string.IsNullOrEmpty(array[0]))
			{
				stiBorderSide.Color = Color(array[0]);
			}
			if (!string.IsNullOrEmpty(array[1]))
			{
				stiBorderSide.Size = double.Parse(array[1]);
			}
			if (!string.IsNullOrEmpty(array[2]))
			{
				stiBorderSide.Style = (StiPenStyle)Enum.Parse(typeof(StiPenStyle), array[2]);
			}
			return stiBorderSide;
		}

		public static StiCap JCap(string text)
		{
			string[] array = text.Split(new char[1] { ';' });
			StiCap stiCap = new StiCap();
			if (array.Length != 4)
			{
				throw new Exception("Parsing Error");
			}
			if (!string.IsNullOrEmpty(array[0]))
			{
				stiCap.Width = int.Parse(array[0]);
			}
			if (!string.IsNullOrEmpty(array[1]))
			{
				stiCap.Height = int.Parse(array[1]);
			}
			if (!string.IsNullOrEmpty(array[2]))
			{
				stiCap.Style = (StiCapStyle)Enum.Parse(typeof(StiCapStyle), array[2]);
			}
			if (!string.IsNullOrEmpty(array[3]))
			{
				stiCap.Color = Color(array[3]);
			}
			return stiCap;
		}

		public static StiBorder Border(JProperty prop)
		{
			if (prop.Value is JValue)
			{
				return Border((string)((JValue)prop.Value).Value);
			}
			StiBorder stiBorder = new StiBorder();
			stiBorder.LoadFromJson((JObject)prop.Value);
			return stiBorder;
		}

		private static StiBorder Border(string text)
		{
			string[] array = text.Split(new char[1] { ';' });
			if (array.Length == 7)
			{
				bool dropShadow = false;
				double shadowSize = 4.0;
				StiSolidBrush shadowBrush = new StiSolidBrush(System.Drawing.Color.Black);
				return new StiAdvancedBorder(JBorderSide(array[0]), JBorderSide(array[1]), JBorderSide(array[2]), JBorderSide(array[3]), dropShadow, shadowSize, shadowBrush);
			}
			StiBorder stiBorder = new StiBorder();
			if (!string.IsNullOrEmpty(array[0]))
			{
				stiBorder.Side = (StiBorderSides)Enum.Parse(typeof(StiBorderSides), array[0]);
			}
			if (!string.IsNullOrEmpty(array[1]))
			{
				stiBorder.Color = Color(array[1]);
			}
			if (!string.IsNullOrEmpty(array[2]))
			{
				stiBorder.Size = double.Parse(array[2]);
			}
			if (!string.IsNullOrEmpty(array[3]))
			{
				stiBorder.Style = (StiPenStyle)Enum.Parse(typeof(StiPenStyle), array[3]);
			}
			if (array.Length > 4 && !string.IsNullOrEmpty(array[4]))
			{
				stiBorder.ShadowSize = double.Parse(array[4]);
			}
			if (array.Length > 5 && !string.IsNullOrEmpty(array[5]))
			{
				stiBorder.DropShadow = true;
			}
			if (array.Length > 6 && !string.IsNullOrEmpty(array[6]))
			{
				stiBorder.Topmost = true;
			}
			if (array.Length > 7 && !string.IsNullOrEmpty(array[7]))
			{
				stiBorder.ShadowBrush = Brush(array[7]);
			}
			return stiBorder;
		}

		public static StiSimpleBorder SimpleBorder(JProperty prop)
		{
			string text = (string)((JValue)prop.Value).Value;
			string[] array = text.Split(new char[1] { ';' });
			StiSimpleBorder stiSimpleBorder = new StiSimpleBorder();
			if (!string.IsNullOrEmpty(array[0]))
			{
				stiSimpleBorder.Side = (StiBorderSides)Enum.Parse(typeof(StiBorderSides), array[0]);
			}
			if (!string.IsNullOrEmpty(array[1]))
			{
				stiSimpleBorder.Color = Color(array[1]);
			}
			if (!string.IsNullOrEmpty(array[2]))
			{
				stiSimpleBorder.Size = double.Parse(array[2]);
			}
			if (!string.IsNullOrEmpty(array[3]))
			{
				stiSimpleBorder.Style = (StiPenStyle)Enum.Parse(typeof(StiPenStyle), array[3]);
			}
			return stiSimpleBorder;
		}

		public static StiSimpleShadow SimpleShadow(JProperty prop)
		{
			string text = (string)((JValue)prop.Value).Value;
			string[] array = text.Split(new char[1] { ';' });
			StiSimpleShadow stiSimpleShadow = new StiSimpleShadow();
			if (!string.IsNullOrEmpty(array[0]))
			{
				stiSimpleShadow.Color = Color(array[0]);
			}
			if (!string.IsNullOrEmpty(array[1]))
			{
				string[] array2 = array[1].Split(new char[1] { ',' });
				stiSimpleShadow.Location = new Point(int.Parse(array2[0]), int.Parse(array2[1]));
			}
			if (!string.IsNullOrEmpty(array[2]))
			{
				stiSimpleShadow.Size = int.Parse(array[2]);
			}
			if (!string.IsNullOrEmpty(array[3]))
			{
				stiSimpleShadow.Visible = true;
			}
			return stiSimpleShadow;
		}

		public static Color Color(string value)
		{
			if (value.IndexOf(",", StringComparison.InvariantCulture) != -1)
			{
				string[] array = value.Split(new char[1] { ',' });
				if (array.Length == 4)
				{
					return System.Drawing.Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()));
				}
				return System.Drawing.Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()));
			}
			if (value.StartsWith("#"))
			{
				return ColorTranslator.FromHtml(value);
			}
			return System.Drawing.Color.FromName(value);
		}

		public static StiBrush Brush(JProperty prop)
		{
			if (prop.Value is JValue)
			{
				return Brush((string)((JValue)prop.Value).Value);
			}
			return StiBrush.LoadFromJson((JObject)prop.Value);
		}

		private static StiBrush Brush(string text)
		{
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			string[] array = text.Split(new char[1] { ':' });
			switch (array[0])
			{
			case "style":
				return new StiStyleBrush();
			case "default":
				return new StiDefaultBrush();
			case "empty":
				return new StiEmptyBrush();
			case "glass":
			{
				StiGlassBrush stiGlassBrush = new StiGlassBrush();
				if (!string.IsNullOrEmpty(array[1]))
				{
					stiGlassBrush.Color = Color(array[1]);
				}
				if (!string.IsNullOrEmpty(array[2]))
				{
					stiGlassBrush.DrawHatch = true;
				}
				if (!string.IsNullOrEmpty(array[3]))
				{
					stiGlassBrush.Blend = float.Parse(array[3]);
				}
				return stiGlassBrush;
			}
			case "glare":
			{
				StiGlareBrush stiGlareBrush = new StiGlareBrush();
				if (!string.IsNullOrEmpty(array[1]))
				{
					stiGlareBrush.StartColor = Color(array[1]);
				}
				if (!string.IsNullOrEmpty(array[2]))
				{
					stiGlareBrush.EndColor = Color(array[2]);
				}
				if (!string.IsNullOrEmpty(array[3]))
				{
					stiGlareBrush.Angle = double.Parse(array[3]);
				}
				if (!string.IsNullOrEmpty(array[4]))
				{
					stiGlareBrush.Focus = float.Parse(array[4]);
				}
				if (!string.IsNullOrEmpty(array[5]))
				{
					stiGlareBrush.Scale = float.Parse(array[5]);
				}
				return stiGlareBrush;
			}
			case "hatch":
			{
				StiHatchBrush stiHatchBrush = new StiHatchBrush();
				if (!string.IsNullOrEmpty(array[1]))
				{
					stiHatchBrush.BackColor = Color(array[1]);
				}
				if (!string.IsNullOrEmpty(array[2]))
				{
					stiHatchBrush.ForeColor = Color(array[2]);
				}
				if (!string.IsNullOrEmpty(array[3]))
				{
					stiHatchBrush.Style = (HatchStyle)Enum.Parse(typeof(HatchStyle), array[3]);
				}
				return stiHatchBrush;
			}
			case "gradient":
			{
				StiGradientBrush stiGradientBrush = new StiGradientBrush();
				if (!string.IsNullOrEmpty(array[1]))
				{
					stiGradientBrush.StartColor = Color(array[1]);
				}
				if (!string.IsNullOrEmpty(array[2]))
				{
					stiGradientBrush.EndColor = Color(array[2]);
				}
				if (!string.IsNullOrEmpty(array[3]))
				{
					stiGradientBrush.Angle = double.Parse(array[3]);
				}
				return stiGradientBrush;
			}
			case "solid":
			{
				StiSolidBrush stiSolidBrush = new StiSolidBrush();
				if (!string.IsNullOrEmpty(array[1]))
				{
					stiSolidBrush.Color = Color(array[1]);
				}
				return stiSolidBrush;
			}
			default:
				return null;
			}
		}

		public static Color[] ColorArray(JObject jObject)
		{
			Color[] array = new Color[jObject.Count];
			int num = 0;
			foreach (JProperty item in jObject.Properties())
			{
				string text = item.Value.ToObject<string>();
				if (text.IndexOf(",", StringComparison.InvariantCulture) != -1)
				{
					string[] array2 = text.Split(new char[1] { ',' });
					if (array2.Length == 4)
					{
						array[num] = System.Drawing.Color.FromArgb(int.Parse(array2[0].Trim()), int.Parse(array2[1].Trim()), int.Parse(array2[2].Trim()), int.Parse(array2[3].Trim()));
					}
					else
					{
						array[num] = System.Drawing.Color.FromArgb(int.Parse(array2[0].Trim()), int.Parse(array2[1].Trim()), int.Parse(array2[2].Trim()));
					}
				}
				else
				{
					array[num] = System.Drawing.Color.FromName(text);
				}
				num++;
			}
			return array;
		}

		public static Size Size(JObject jObject)
		{
			Size result = default(Size);
			foreach (JProperty item in jObject.Properties())
			{
				string name = item.Name;
				if (!(name == "Width"))
				{
					if (name == "Height")
					{
						result.Height = item.Value.ToObject<int>();
					}
				}
				else
				{
					result.Width = item.Value.ToObject<int>();
				}
			}
			return result;
		}

		public static DateTime DateTime(JProperty prop)
		{
			object value = ((JValue)prop.Value).Value;
			if (value is DateTime)
			{
				return prop.Value.ToObject<DateTime>();
			}
			if (value is long)
			{
				return new DateTime(prop.Value.ToObject<long>());
			}
			return System.DateTime.Now;
		}

		public static RectangleD RectangleD(JProperty prop)
		{
			if (prop.Value is JValue)
			{
				string text = (string)((JValue)prop.Value).Value;
				string[] array = text.Split(new char[1] { ',' });
				if (array.Length != 4)
				{
					throw new Exception("Parsing Error");
				}
				return new RectangleD(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]), double.Parse(array[3]));
			}
			RectangleD result = default(RectangleD);
			result.LoadFromJson((JObject)prop.Value);
			return result;
		}

		public static SizeD SizeD(JProperty prop)
		{
			if (prop.Value is JValue)
			{
				string text = (string)((JValue)prop.Value).Value;
				string[] array = text.Split(new char[1] { ',' });
				if (array.Length != 2)
				{
					throw new Exception("Parsing Error");
				}
				return new SizeD(double.Parse(array[0]), double.Parse(array[1]));
			}
			SizeD result = default(SizeD);
			foreach (JProperty item in ((JObject)prop.Value).Properties())
			{
				string name = prop.Name;
				if (!(name == "Width"))
				{
					if (name == "Height")
					{
						result.Height = item.Value.ToObject<int>();
					}
				}
				else
				{
					result.Width = item.Value.ToObject<int>();
				}
			}
			return result;
		}

		public static Size Size(JProperty prop)
		{
			if (prop.Value is JValue)
			{
				string text = (string)((JValue)prop.Value).Value;
				string[] array = text.Split(new char[1] { ',' });
				if (array.Length != 2)
				{
					throw new Exception("Parsing Error");
				}
				return new Size(int.Parse(array[0]), int.Parse(array[1]));
			}
			Size result = default(Size);
			foreach (JProperty item in ((JObject)prop.Value).Properties())
			{
				string name = prop.Name;
				if (!(name == "Width"))
				{
					if (name == "Height")
					{
						result.Height = item.Value.ToObject<int>();
					}
				}
				else
				{
					result.Width = item.Value.ToObject<int>();
				}
			}
			return result;
		}

		public static Point Point(JObject jObject)
		{
			Point result = default(Point);
			foreach (JProperty item in jObject.Properties())
			{
				string name = item.Name;
				if (!(name == "X"))
				{
					if (name == "Y")
					{
						result.Y = item.Value.ToObject<int>();
					}
				}
				else
				{
					result.X = item.Value.ToObject<int>();
				}
			}
			return result;
		}

		public static PointF PointF(JObject jObject)
		{
			PointF result = default(PointF);
			foreach (JProperty item in jObject.Properties())
			{
				string name = item.Name;
				if (!(name == "X"))
				{
					if (name == "Y")
					{
						result.Y = item.Value.ToObject<float>();
					}
				}
				else
				{
					result.X = item.Value.ToObject<float>();
				}
			}
			return result;
		}
	}

	public static StiSimpleShadow DeserializeSimpleShadow(this JProperty property)
	{
		return Deserialize.SimpleShadow(property);
	}

	public static StiBrush DeserializeBrush(this JProperty property)
	{
		return Deserialize.Brush(property);
	}

	public static StiCornerRadius DeserializeCornerRadius(this JProperty property)
	{
		return StiCornerRadius.TryParse(property.DeserializeString());
	}

	public static StiBorder DeserializeBorder(this JProperty property)
	{
		return Deserialize.Border(property);
	}

	public static StiSimpleBorder DeserializeSimpleBorder(this JProperty property)
	{
		return Deserialize.SimpleBorder(property);
	}

	public static Color DeserializeColor(this JProperty property)
	{
		return Deserialize.Color(property.Value.ToObject<string>());
	}

	public static Color[] DeserializeArrayColor(this JProperty property)
	{
		return Deserialize.ColorArray((JObject)property.Value);
	}

	public static double DeserializeDouble(this JProperty property)
	{
		return property.Value.ToObject<double>();
	}

	public static float DeserializeFloat(this JProperty property)
	{
		return property.Value.ToObject<float>();
	}

	public static decimal DeserializeDecimal(this JProperty property)
	{
		return property.Value.ToObject<decimal>();
	}

	public static float? DeserializeFloatNullable(this JProperty property)
	{
		return property.Value.ToObject<float?>();
	}

	public static DateTime DeserializeDateTime(this JProperty property)
	{
		return Deserialize.DateTime(property);
	}

	public static TimeSpan DeserializeTimeSpan(this JProperty property)
	{
		return new TimeSpan(property.Value.ToObject<long>());
	}

	public static int DeserializeInt(this JProperty property)
	{
		return property.Value.ToObject<int>();
	}

	public static string DeserializeString(this JProperty property)
	{
		return property.Value.ToObject<string>();
	}

	public static string[] DeserializeStringArray(this JProperty property)
	{
		return Deserialize.StringArray((JObject)property.Value);
	}

	public static bool[] DeserializeBoolArray(this JProperty property)
	{
		return Deserialize.BoolArray((JObject)property.Value);
	}

	public static List<string> DeserializeListString(this JProperty property)
	{
		return Deserialize.ListString((JObject)property.Value);
	}

	public static bool DeserializeBool(this JProperty property)
	{
		return property.Value.ToObject<bool>();
	}

	public static Point DeserializePoint(this JProperty property)
	{
		return Deserialize.Point((JObject)property.Value);
	}

	public static PointF DeserializePointF(this JProperty property)
	{
		return Deserialize.PointF((JObject)property.Value);
	}

	public static Font DeserializeFont(this JProperty property, Font defaultFont)
	{
		return Deserialize.Font(property, defaultFont);
	}

	public static StiCap DeserializeCap(this JProperty property)
	{
		return Deserialize.JCap(property.Value.ToObject<string>());
	}

	public static int[] DeserializeIntArray(this JProperty property)
	{
		return Deserialize.IntArray((JObject)property.Value);
	}

	public static byte[] DeserializeByteArray(this JProperty property)
	{
		return StiImageConverter.StringToByteArray(property.Value.ToObject<string>());
	}

	public static Image DeserializeImage(this JProperty property)
	{
		return StiImageConverter.StringToImage(property.Value.ToObject<string>());
	}

	public static SizeD DeserializeSizeD(this JProperty property)
	{
		return Deserialize.SizeD(property);
	}

	public static Size DeserializeSize(this JProperty property)
	{
		return Deserialize.Size(property);
	}

	public static RectangleD DeserializeRectangleD(this JProperty property)
	{
		return Deserialize.RectangleD(property);
	}

	public static T DeserializeEnum<T>(this JProperty property)
	{
		return (T)Enum.Parse(typeof(T), property.Value.ToObject<string>());
	}
}
