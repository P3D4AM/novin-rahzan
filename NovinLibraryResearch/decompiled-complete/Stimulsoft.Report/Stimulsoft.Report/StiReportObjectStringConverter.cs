using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report;

public class StiReportObjectStringConverter : StiObjectStringConverter
{
	private Hashtable hashObjectToString = new Hashtable();

	private Hashtable hashObjects = new Hashtable();

	private Hashtable hashStringToObject = new Hashtable();

	private Hashtable hashStrings = new Hashtable();

	internal bool AllowLoadOptimization { get; set; }

	public void Clear()
	{
		hashObjectToString.Clear();
		hashObjectToString = null;
		hashObjects.Clear();
		hashObjects = null;
		hashStringToObject.Clear();
		hashStringToObject = null;
		hashStrings.Clear();
		hashStrings = null;
	}

	public override void SetProperty(PropertyInfo p, object parentObject, object obj)
	{
		if (!(p.Name != "HighlightCondition"))
		{
			StiHighlightCondition stiHighlightCondition = obj as StiHighlightCondition;
			StiSimpleText stiSimpleText = parentObject as StiSimpleText;
			if (stiHighlightCondition.Condition.Value.Trim().Length != 0)
			{
				stiSimpleText.Conditions.Add(new StiCondition
				{
					BackColor = StiBrush.ToColor(stiHighlightCondition.Brush),
					TextColor = StiBrush.ToColor(stiHighlightCondition.TextBrush),
					Font = stiHighlightCondition.Font,
					Expression = stiHighlightCondition.Condition,
					Item = StiFilterItem.Expression
				});
			}
		}
	}

	public override string ObjectToString(object obj)
	{
		if (!StiOptions.Engine.SerializationOptimization)
		{
			return ObjectToString2(obj);
		}
		if (obj is Font || obj is StiBorder || obj is StiSimpleBorder || obj is StiBrush || obj is StiTextOptions || obj is StiSimpleShadow)
		{
			int hashCode = obj.GetHashCode();
			object obj2 = hashObjectToString[hashCode];
			if (obj2 != null)
			{
				object obj3 = hashObjects[hashCode];
				if (obj.Equals(obj3))
				{
					if (obj2 is bool)
					{
						return null;
					}
					return (string)obj2;
				}
				return ObjectToString2(obj);
			}
			string text = ObjectToString2(obj);
			if (text == null)
			{
				hashObjectToString[hashCode] = false;
			}
			else
			{
				hashObjectToString[hashCode] = text;
			}
			hashObjects[hashCode] = obj;
			return text;
		}
		return ObjectToString2(obj);
	}

	public string ObjectToString2(object obj)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (obj is Font)
		{
			return ConvertFontToString((Font)obj);
		}
		if (obj is PointF)
		{
			return ConvertPointFToString((PointF)obj);
		}
		if (obj is RectangleD)
		{
			return ConvertRectangleDToString((RectangleD)obj);
		}
		if (obj is StiTextOptions)
		{
			return ConvertTextOptionsToString((StiTextOptions)((obj is StiTextOptions) ? obj : null));
		}
		if (obj is StiBrush)
		{
			return ConvertBrushToString((StiBrush)((obj is StiBrush) ? obj : null));
		}
		if (obj is StiBorder)
		{
			return ConvertBorderToString((StiBorder)((obj is StiBorder) ? obj : null));
		}
		if (obj is StiSimpleBorder)
		{
			return ConvertSimpleBorderToString((StiSimpleBorder)((obj is StiSimpleBorder) ? obj : null));
		}
		if (obj is StiSimpleShadow)
		{
			return ConvertSimpleShadowToString((StiSimpleShadow)((obj is StiSimpleShadow) ? obj : null));
		}
		if (obj is StiVariable)
		{
			return ConvertVariableToString(obj as StiVariable);
		}
		if (obj is StiExpression)
		{
			return ConvertExpressionToString(obj as StiExpression);
		}
		if (obj is StiEvent)
		{
			return ConvertEventToString(obj as StiEvent);
		}
		if (obj is DateTime dateTime)
		{
			return dateTime.ToString(new CultureInfo("en-US", useUserOverride: false));
		}
		if (obj is Color[])
		{
			return ConvertColorArrayToString(obj as Color[]);
		}
		return ((StiObjectStringConverter)this).ObjectToString(obj);
	}

	public override object StringToObject(string str, Type type)
	{
		if (!StiOptions.Engine.SerializationOptimization || !AllowLoadOptimization)
		{
			return StringToObject2(str, type);
		}
		if (type == typeof(Font) || type == typeof(StiBorder) || type == typeof(StiSimpleBorder) || type == typeof(StiBrush) || type == typeof(StiTextOptions) || type == typeof(StiSimpleShadow))
		{
			int hashCode = str.GetHashCode();
			object obj = hashStringToObject[hashCode];
			if (obj != null)
			{
				string value = (string)hashStrings[hashCode];
				if (str.Equals(value))
				{
					return obj;
				}
				return StringToObject2(str, type);
			}
			obj = StringToObject2(str, type);
			hashStringToObject[hashCode] = obj;
			hashStrings[hashCode] = str;
			return obj;
		}
		return StringToObject2(str, type);
	}

	public object StringToObject2(string str, Type type)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (type == typeof(Font))
		{
			return ConvertStringToFont(str);
		}
		if (type == typeof(RectangleD))
		{
			return ConvertStringToRectangleD(str);
		}
		if (type == typeof(PointF))
		{
			return ConvertStringToPointF(str);
		}
		if (type == typeof(StiTextOptions))
		{
			return ConvertStringToTextOptions(str);
		}
		if (type == typeof(StiBrush))
		{
			return ConvertStringToBrush(str);
		}
		if (type == typeof(StiBorder))
		{
			return ConvertStringToBorder(str);
		}
		if (type == typeof(StiSimpleBorder))
		{
			return ConvertStringToSimpleBorder(str);
		}
		if (type == typeof(StiSimpleShadow))
		{
			return ConvertStringToSimpleShadow(str);
		}
		if (type == typeof(StiVariable))
		{
			return ConvertStringToVariable(type, str);
		}
		if (StiTypeFinder.FindType(type, typeof(StiDataParameter)))
		{
			return ((StiObjectStringConverter)this).StringToObject(str, type);
		}
		if (type == typeof(StiExpression))
		{
			return ConvertStringToExpression(type, str);
		}
		if (type == typeof(Image))
		{
			return StiImageConverter.StringToImage(str);
		}
		if (type == typeof(Metafile))
		{
			return StiMetafileConverter.StringToMetafile(str);
		}
		if (type == typeof(DateTime))
		{
			if (str == null || str.Trim().Length == 0)
			{
				return DateTime.Now;
			}
			if (!DateTime.TryParse(str, new CultureInfo("en-US", useUserOverride: false), DateTimeStyles.None, out var result))
			{
				return DateTime.Today;
			}
			return result;
		}
		if (type == typeof(StiUnit))
		{
			switch (str)
			{
			case "Centimeters":
				return StiUnit.Centimeters;
			case "Millimeters":
				return StiUnit.Millimeters;
			case "HundredthsOfInch":
				return StiUnit.HundredthsOfInch;
			case "Inches":
				return StiUnit.Inches;
			}
		}
		if (type == typeof(Color[]))
		{
			return ConvertStringToColorArray(str);
		}
		return ((StiObjectStringConverter)this).StringToObject(str, type);
	}

	public string ConvertEventToString(StiEvent ev)
	{
		if (string.IsNullOrEmpty(ev.Script))
		{
			return null;
		}
		return ev.Script;
	}

	public string ConvertExpressionToString(StiExpression expr)
	{
		if (!(expr is StiDataParameter) && expr.Value == string.Empty)
		{
			return null;
		}
		return TypeDescriptor.GetConverter(expr.GetType()).ConvertToString(expr);
	}

	public StiExpression ConvertStringToExpression(Type type, string str)
	{
		return TypeDescriptor.GetConverter(type).ConvertFromString(str) as StiExpression;
	}

	public string ConvertVariableToString(StiVariable variable)
	{
		return TypeDescriptor.GetConverter(variable.GetType()).ConvertToString(variable);
	}

	public StiVariable ConvertStringToVariable(Type type, string str)
	{
		return TypeDescriptor.GetConverter(type).ConvertFromString(str) as StiVariable;
	}

	private static string GetOriginalFontStr(Font font)
	{
		if (!string.IsNullOrEmpty(font.OriginalFontName) && !(font.OriginalFontName == font.Name))
		{
			return $"{font.OriginalFontName},{font.Size}";
		}
		return $"{font.FontFamily.Name},{font.Size}";
	}

	public static string ConvertFontToString(Font font)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder = (StiOptions.Engine.AllowUseOriginalFontName ? stringBuilder.Append(GetOriginalFontStr(font)) : stringBuilder.AppendFormat("{0},{1}", font.FontFamily.Name, font.Size));
		int num = 2;
		if (font.GdiCharSet != 1)
		{
			num = 6;
		}
		else if (font.GdiVerticalFont)
		{
			num = 5;
		}
		else if ((int)font.Unit != 3)
		{
			num = 4;
		}
		else if ((int)font.Style != 0)
		{
			num = 3;
		}
		if (num > 2)
		{
			EnumConverter enumConverter = new EnumConverter(typeof(FontStyle));
			stringBuilder = stringBuilder.AppendFormat(",{0}", enumConverter.ConvertToString(font.Style).Replace(',', '|'));
			if (num > 3)
			{
				EnumConverter enumConverter2 = new EnumConverter(typeof(GraphicsUnit));
				stringBuilder = stringBuilder.AppendFormat(",{0}", enumConverter2.ConvertToString(font.Unit));
				if (num > 4)
				{
					stringBuilder = stringBuilder.AppendFormat(",{0}", font.GdiVerticalFont);
					if (num > 5)
					{
						stringBuilder = stringBuilder.AppendFormat(",{0}", font.GdiCharSet);
					}
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static Font ConvertStringToFont(string str)
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_025b: Expected O, but got Unknown
		string[] array = str.Split(new char[1] { ',' });
		string text = array[0];
		FontFamily fontFamily = StiFontCollection.GetFontFamily(text, true);
		float num = float.Parse(array[1]);
		if (array.Length == 2)
		{
			Font val = ((fontFamily != null) ? new Font(fontFamily, num) : new Font(text, num));
			if (StiOptions.Engine.ThrowErrorIfFontIsNotInstalledInSystem && val.FontFamily.Name != text)
			{
				throw new Exception($"Font '{fontFamily}' is not installed on system!");
			}
			return val;
		}
		if (array.Length > 2)
		{
			EnumConverter enumConverter = new EnumConverter(typeof(FontStyle));
			FontStyle val2 = (FontStyle)enumConverter.ConvertFromString(array[2].Replace('|', ','));
			if (array.Length == 3)
			{
				try
				{
					Font val3 = ((fontFamily != null) ? new Font(fontFamily, num, val2) : new Font(text, num, val2));
					if (StiOptions.Engine.ThrowErrorIfFontIsNotInstalledInSystem && val3.FontFamily.Name != text)
					{
						throw new Exception($"Font '{fontFamily}' is not installed on system!");
					}
					return val3;
				}
				catch (ArgumentException)
				{
					return new Font("Arial", num, val2);
				}
			}
			EnumConverter enumConverter2 = new EnumConverter(typeof(GraphicsUnit));
			GraphicsUnit val4 = (GraphicsUnit)enumConverter2.ConvertFromString(array[3]);
			if (array.Length == 4)
			{
				try
				{
					Font val5 = ((fontFamily != null) ? new Font(fontFamily, num, val2, val4) : new Font(text, num, val2, val4));
					if (StiOptions.Engine.ThrowErrorIfFontIsNotInstalledInSystem && val5.FontFamily.Name != text)
					{
						throw new Exception($"Font '{fontFamily}' is not installed on system!");
					}
					return val5;
				}
				catch (ArgumentException)
				{
					return new Font("Arial", num, val2, val4);
				}
			}
			bool flag = array[4] == "True";
			if (array.Length == 5)
			{
				try
				{
					Font val6 = ((fontFamily != null) ? new Font(fontFamily, num, val2, val4, (byte)1, flag) : new Font(text, num, val2, val4, (byte)1, flag));
					if (StiOptions.Engine.ThrowErrorIfFontIsNotInstalledInSystem && val6.FontFamily.Name != text)
					{
						throw new Exception($"Font '{fontFamily}' is not installed on system!");
					}
					return val6;
				}
				catch (ArgumentException)
				{
					return new Font("Arial", num, val2, val4, (byte)1, flag);
				}
			}
			byte b = byte.Parse(array[5]);
			if (array.Length == 6)
			{
				try
				{
					Font val7 = ((fontFamily != null) ? new Font(fontFamily, num, val2, val4, b, flag) : new Font(text, num, val2, val4, b, flag));
					if (StiOptions.Engine.ThrowErrorIfFontIsNotInstalledInSystem && val7.FontFamily.Name != text)
					{
						throw new Exception($"Font '{fontFamily}' is not installed on system!");
					}
					return val7;
				}
				catch (ArgumentException)
				{
					return new Font("Arial", num, val2, val4, b, flag);
				}
			}
		}
		return null;
	}

	public string ConvertTextOptionsToString(StiTextOptions textOptions)
	{
		if (!textOptions.IsDefault)
		{
			return StiTextOptionsHelper.ConvertTextOptionsToString(textOptions, ',');
		}
		return null;
	}

	public StiTextOptions ConvertStringToTextOptions(string str)
	{
		return StiTextOptionsHelper.ConvertStringToTextOptions(str, ',');
	}

	public string ConvertRectangleDToString(RectangleD rectangle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return RectangleDHelper.ConvertRectangleDToString(rectangle);
	}

	public RectangleD ConvertStringToRectangleD(string str)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return RectangleDHelper.ConvertStringToRectangleD(str, ',');
	}

	public static string ConvertPointFToString(PointF point)
	{
		return $"{point.X},{point.Y}";
	}

	public static PointF ConvertStringToPointF(string str)
	{
		string[] array = str.Split(new char[1] { ',' });
		float x = float.Parse(array[0]);
		float y = float.Parse(array[1]);
		return new PointF(x, y);
	}

	public string ConvertBrushToString(StiBrush brush)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if (brush is StiEmptyBrush)
		{
			return "EmptyBrush";
		}
		if (brush is StiSolidBrush)
		{
			StiSolidBrush val = (StiSolidBrush)(object)((brush is StiSolidBrush) ? brush : null);
			return ConvertColorToString(val.Color);
		}
		if (brush is StiHatchBrush)
		{
			StiHatchBrush val2 = (StiHatchBrush)(object)((brush is StiHatchBrush) ? brush : null);
			return $"HatchBrush,{val2.Style},{ConvertColorToString(val2.ForeColor)},{ConvertColorToString(val2.BackColor)}";
		}
		if (brush is StiGradientBrush)
		{
			StiGradientBrush val3 = (StiGradientBrush)(object)((brush is StiGradientBrush) ? brush : null);
			return $"GradientBrush,{ConvertColorToString(val3.StartColor)},{ConvertColorToString(val3.EndColor)},{val3.Angle}";
		}
		if (brush is StiGlassBrush)
		{
			StiGlassBrush val4 = (StiGlassBrush)(object)((brush is StiGlassBrush) ? brush : null);
			return $"GlassBrush,{ConvertColorToString(val4.Color)},{val4.DrawHatch},{val4.Blend}";
		}
		if (brush is StiGlareBrush)
		{
			StiGlareBrush val5 = (StiGlareBrush)(object)((brush is StiGlareBrush) ? brush : null);
			return $"GlareBrush,{ConvertColorToString(val5.StartColor)},{ConvertColorToString(val5.EndColor)},{val5.Angle},{val5.Focus},{val5.Scale}";
		}
		if (brush is StiDefaultBrush)
		{
			return "DefaultBrush";
		}
		if (brush is StiStyleBrush)
		{
			return "StyleBrush";
		}
		return string.Empty;
	}

	public StiBrush ConvertStringToBrush(string str)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		string[] array = str.Split(new char[1] { ',' });
		if (!(array[0] == "EmptyBrush"))
		{
			if (array[0] == "HatchBrush")
			{
				HatchStyle val = (HatchStyle)Enum.Parse(typeof(HatchStyle), array[1], ignoreCase: false);
				return (StiBrush)new StiHatchBrush(val, ConvertStringToColor(array[2]), ConvertStringToColor(array[3]));
			}
			if (!(array[0] == "GradientBrush"))
			{
				if (!(array[0] == "GlareBrush"))
				{
					if (!(array[0] == "GlassBrush"))
					{
						if (!(array[0] == "DefaultBrush"))
						{
							if (!(array[0] == "StyleBrush"))
							{
								return (StiBrush)new StiSolidBrush(ConvertStringToColor(array[0]));
							}
							return (StiBrush)new StiStyleBrush();
						}
						return (StiBrush)new StiDefaultBrush();
					}
					return (StiBrush)new StiGlassBrush(ConvertStringToColor(array[1]), array[2].ToLowerInvariant() == "true", float.Parse(array[3]));
				}
				return (StiBrush)new StiGlareBrush(ConvertStringToColor(array[1]), ConvertStringToColor(array[2]), double.Parse(array[3]), float.Parse(array[4]), float.Parse(array[5]));
			}
			return (StiBrush)new StiGradientBrush(ConvertStringToColor(array[1]), ConvertStringToColor(array[2]), double.Parse(array[3]));
		}
		return (StiBrush)new StiEmptyBrush();
	}

	public static string ConvertColorToString(Color color)
	{
		if (!color.IsNamedColor && !color.IsSystemColor)
		{
			if (color.A != byte.MaxValue)
			{
				return $"[{color.A}:{color.R}:{color.G}:{color.B}]";
			}
			return $"[{color.R}:{color.G}:{color.B}]";
		}
		ColorConverter colorConverter = new ColorConverter();
		return colorConverter.ConvertToString(color);
	}

	public static Color ConvertStringToColor(string str)
	{
		if (str.IndexOf(';') != -1)
		{
			str = str.Trim();
			string[] array = str.Split(new char[1] { ';' });
			try
			{
				if (array.Length == 4)
				{
					return Color.FromArgb(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]), int.Parse(array[3]));
				}
				if (array.Length == 3)
				{
					return Color.FromArgb(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
				}
				return Color.Empty;
			}
			catch
			{
				return Color.Empty;
			}
		}
		if (str.IndexOf('[') != -1)
		{
			str = str.Trim().Substring(1, str.Length - 2);
			string[] array2 = str.Split(new char[1] { ':' });
			try
			{
				if (array2.Length == 4)
				{
					return Color.FromArgb(int.Parse(array2[0]), int.Parse(array2[1]), int.Parse(array2[2]), int.Parse(array2[3]));
				}
				if (array2.Length == 3)
				{
					return Color.FromArgb(int.Parse(array2[0]), int.Parse(array2[1]), int.Parse(array2[2]));
				}
				return Color.Empty;
			}
			catch
			{
				return Color.Empty;
			}
		}
		ColorConverter colorConverter = new ColorConverter();
		return (Color)colorConverter.ConvertFromString(str);
	}

	public static string ConvertColorArrayToString(Color[] colors)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		foreach (Color color in colors)
		{
			if (num != 0)
			{
				stringBuilder = stringBuilder.Append(", ");
			}
			stringBuilder = stringBuilder.Append(ConvertColorToString(color));
			num++;
		}
		return stringBuilder.ToString();
	}

	public static Color[] ConvertStringToColorArray(string str)
	{
		if (StringExt.IndexOfInvariant(str, ",") == -1)
		{
			return new Color[1] { ConvertStringToColor(str) };
		}
		string[] array = str.Split(new char[1] { ',' });
		Color[] array2 = new Color[array.Length];
		int num = 0;
		string[] array3 = array;
		foreach (string text in array3)
		{
			array2[num] = ConvertStringToColor(text.Trim());
			num++;
		}
		return array2;
	}

	public string ConvertBorderToString(StiBorder border)
	{
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		if (border.IsDefault)
		{
			return null;
		}
		if (border is StiAdvancedBorder)
		{
			StiAdvancedBorder val = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder = stringBuilder.AppendFormat("Adv{0};", ConvertColorToString(val.TopSide.Color));
			stringBuilder = stringBuilder.AppendFormat("{0};", val.TopSide.Size);
			stringBuilder = stringBuilder.AppendFormat("{0};", val.TopSide.Style);
			stringBuilder = stringBuilder.AppendFormat("{0};", ConvertColorToString(val.BottomSide.Color));
			stringBuilder = stringBuilder.AppendFormat("{0};", val.BottomSide.Size);
			stringBuilder = stringBuilder.AppendFormat("{0};", val.BottomSide.Style);
			stringBuilder = stringBuilder.AppendFormat("{0};", ConvertColorToString(val.LeftSide.Color));
			stringBuilder = stringBuilder.AppendFormat("{0};", val.LeftSide.Size);
			stringBuilder = stringBuilder.AppendFormat("{0};", val.LeftSide.Style);
			stringBuilder = stringBuilder.AppendFormat("{0};", ConvertColorToString(val.RightSide.Color));
			stringBuilder = stringBuilder.AppendFormat("{0};", val.RightSide.Size);
			stringBuilder = stringBuilder.AppendFormat("{0};", val.RightSide.Style);
			stringBuilder = stringBuilder.AppendFormat("{0};", border.DropShadow);
			stringBuilder = stringBuilder.AppendFormat("{0};", border.ShadowSize);
			stringBuilder = stringBuilder.Append(ConvertBrushToString(border.ShadowBrush));
			if (border.Topmost)
			{
				stringBuilder = stringBuilder.AppendFormat(";{0}", border.Topmost);
			}
			return stringBuilder.ToString();
		}
		EnumConverter enumConverter = new EnumConverter(typeof(StiBorderSides));
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2 = stringBuilder2.AppendFormat("{0};", enumConverter.ConvertToString(border.Side));
		stringBuilder2 = stringBuilder2.AppendFormat("{0};", ConvertColorToString(border.Color));
		stringBuilder2 = stringBuilder2.AppendFormat("{0};", border.Size);
		stringBuilder2 = stringBuilder2.AppendFormat("{0};", border.Style);
		stringBuilder2 = stringBuilder2.AppendFormat("{0};", border.DropShadow);
		stringBuilder2 = stringBuilder2.AppendFormat("{0};", border.ShadowSize);
		stringBuilder2 = stringBuilder2.Append(ConvertBrushToString(border.ShadowBrush));
		if (border.Topmost)
		{
			stringBuilder2 = stringBuilder2.AppendFormat(";{0}", border.Topmost);
		}
		return stringBuilder2.ToString();
	}

	public StiBorder ConvertStringToBorder(string str)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		if (StringExt.StartsWithInvariant(str, "Adv"))
		{
			str = str.Substring(3);
			string[] array = str.Split(new char[1] { ';' });
			EnumConverter enumConverter = new EnumConverter(typeof(StiPenStyle));
			StiBorderSide val = new StiBorderSide();
			StiBorderSide val2 = new StiBorderSide();
			StiBorderSide val3 = new StiBorderSide();
			StiBorderSide val4 = new StiBorderSide();
			val.Color = ConvertStringToColor(array[0]);
			val.Size = double.Parse(array[1]);
			val.Style = (StiPenStyle)enumConverter.ConvertFromString(array[2]);
			val2.Color = ConvertStringToColor(array[3]);
			val2.Size = double.Parse(array[4]);
			val2.Style = (StiPenStyle)enumConverter.ConvertFromString(array[5]);
			val3.Color = ConvertStringToColor(array[6]);
			val3.Size = double.Parse(array[7]);
			val3.Style = (StiPenStyle)enumConverter.ConvertFromString(array[8]);
			val4.Color = ConvertStringToColor(array[9]);
			val4.Size = double.Parse(array[10]);
			val4.Style = (StiPenStyle)enumConverter.ConvertFromString(array[11]);
			if (array.Length != 15)
			{
				return (StiBorder)new StiAdvancedBorder(val, val2, val3, val4, array[12].ToLowerInvariant() == "true", (double)float.Parse(array[13]), ConvertStringToBrush(array[14]), array[15].ToLowerInvariant() == "true");
			}
			return (StiBorder)new StiAdvancedBorder(val, val2, val3, val4, array[12].ToLowerInvariant() == "true", (double)float.Parse(array[13]), ConvertStringToBrush(array[14]));
		}
		string[] array2 = str.Split(new char[1] { ';' });
		EnumConverter enumConverter2 = new EnumConverter(typeof(StiBorderSides));
		EnumConverter enumConverter3 = new EnumConverter(typeof(StiPenStyle));
		int num;
		Color color;
		if (array2.Length == 10)
		{
			num = 5;
			color = Color.FromArgb(int.Parse(array2[1]), int.Parse(array2[2]), int.Parse(array2[3]), int.Parse(array2[4]));
		}
		else if (array2.Length == 9)
		{
			num = 4;
			color = Color.FromArgb(int.Parse(array2[1]), int.Parse(array2[2]), int.Parse(array2[3]));
		}
		else
		{
			num = 2;
			color = ConvertStringToColor(array2[1]);
		}
		if (array2.Length - 1 != num + 4)
		{
			return new StiBorder((StiBorderSides)enumConverter2.ConvertFromString(array2[0]), color, (double)float.Parse(array2[num]), (StiPenStyle)enumConverter3.ConvertFromString(array2[num + 1]), array2[num + 2].ToLowerInvariant() == "true", (double)float.Parse(array2[num + 3]), ConvertStringToBrush(array2[num + 4]), array2[num + 5].ToLowerInvariant() == "true");
		}
		return new StiBorder((StiBorderSides)enumConverter2.ConvertFromString(array2[0]), color, (double)float.Parse(array2[num]), (StiPenStyle)enumConverter3.ConvertFromString(array2[num + 1]), array2[num + 2].ToLowerInvariant() == "true", (double)float.Parse(array2[num + 3]), ConvertStringToBrush(array2[num + 4]));
	}

	public string ConvertSimpleBorderToString(StiSimpleBorder border, bool ignoreDefault = false)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		if (border.IsDefault && !ignoreDefault)
		{
			return null;
		}
		EnumConverter enumConverter = new EnumConverter(typeof(StiBorderSides));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder = stringBuilder.AppendFormat("{0};", enumConverter.ConvertToString(border.Side));
		stringBuilder = stringBuilder.AppendFormat("{0};", ConvertColorToString(border.Color));
		stringBuilder = stringBuilder.AppendFormat("{0};", border.Size);
		stringBuilder = stringBuilder.AppendFormat("{0}", border.Style);
		return stringBuilder.ToString();
	}

	public StiSimpleBorder ConvertStringToSimpleBorder(string str)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		string[] array = str.Split(new char[1] { ';' });
		EnumConverter enumConverter = new EnumConverter(typeof(StiBorderSides));
		EnumConverter enumConverter2 = new EnumConverter(typeof(StiPenStyle));
		return new StiSimpleBorder((StiBorderSides)enumConverter.ConvertFromString(array[0]), ConvertStringToColor(array[1]), (double)float.Parse(array[2]), (StiPenStyle)enumConverter2.ConvertFromString(array[3]));
	}

	public string ConvertSimpleShadowToString(StiSimpleShadow shadow, bool ignoreDefault = false)
	{
		if (shadow.IsDefault && !ignoreDefault)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder = stringBuilder.AppendFormat(ConvertColorToString(shadow.Color) + ";");
		stringBuilder = stringBuilder.AppendFormat($"{shadow.Location.X}, {shadow.Location.Y};");
		stringBuilder = stringBuilder.AppendFormat($"{shadow.Size};");
		stringBuilder = stringBuilder.AppendFormat($"{shadow.Visible}");
		return stringBuilder.ToString();
	}

	public StiSimpleShadow ConvertStringToSimpleShadow(string str)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		string[] array = str.Split(new char[1] { ';' });
		string[] array2 = array[1].Split(new char[1] { ',' });
		return new StiSimpleShadow(ConvertStringToColor(array[0]), new Point(int.Parse(array2[0]), int.Parse(array2[1])), int.Parse(array[2]), array[3].ToLowerInvariant() == "true");
	}

	static StiReportObjectStringConverter()
	{
		StiSerializing.AddStringType(typeof(StiPage), "Page");
		StiSerializing.AddStringType(typeof(StiReportTitleBand), "ReportTitleBand");
		StiSerializing.AddStringType(typeof(StiReportSummaryBand), "ReportSummaryBand");
		StiSerializing.AddStringType(typeof(StiPageHeaderBand), "PageHeaderBand");
		StiSerializing.AddStringType(typeof(StiPageFooterBand), "PageFooterBand");
		StiSerializing.AddStringType(typeof(StiGroupHeaderBand), "GroupHeaderBand");
		StiSerializing.AddStringType(typeof(StiGroupFooterBand), "GroupFooterBand");
		StiSerializing.AddStringType(typeof(StiHeaderBand), "HeaderBand");
		StiSerializing.AddStringType(typeof(StiFooterBand), "FooterBand");
		StiSerializing.AddStringType(typeof(StiDataBand), "DataBand");
		StiSerializing.AddStringType(typeof(StiChildBand), "ChildBand");
		StiSerializing.AddStringType(typeof(StiCrossGroupHeaderBand), "CrossGroupHeaderBand");
		StiSerializing.AddStringType(typeof(StiCrossGroupFooterBand), "CrossGroupFooterBand");
		StiSerializing.AddStringType(typeof(StiCrossDataBand), "CrossDataBand");
		StiSerializing.AddStringType(typeof(StiCheckBox), "CheckBox");
		StiSerializing.AddStringType(typeof(StiContourText), "ContourText");
		StiSerializing.AddStringType(typeof(StiSubReport), "SubReport");
		StiSerializing.AddStringType(typeof(StiShape), "Shape");
		StiSerializing.AddStringType(typeof(StiSystemText), "SystemText");
		StiSerializing.AddStringType(typeof(StiImage), "Image");
		StiSerializing.AddStringType(typeof(StiContainer), "Container");
		StiSerializing.AddStringType(typeof(StiText), "Text");
		StiSerializing.AddStringType(typeof(StiClone), "Clone");
		StiSerializing.AddStringType(typeof(StiCap), "Cap");
		StiSerializing.AddStringType(typeof(StiVerticalLinePrimitive), "VerticalLinePrimitive");
		StiSerializing.AddStringType(typeof(StiHorizontalLinePrimitive), "HorizontalLinePrimitive");
		StiSerializing.AddStringType(typeof(StiRectanglePrimitive), "RectanglePrimitive");
		StiSerializing.AddStringType(typeof(StiCrossTitle), "CrossTitle");
		StiSerializing.AddStringType(typeof(StiCrossColumn), "CrossColumn");
		StiSerializing.AddStringType(typeof(StiCrossRow), "CrossRow");
		StiSerializing.AddStringType(typeof(StiCrossColumnTotal), "CrossColumnTotal");
		StiSerializing.AddStringType(typeof(StiCrossRowTotal), "CrossRowTotal");
		StiSerializing.AddStringType(typeof(StiCrossSummary), "CrossSummary");
		StiSerializing.AddStringType(typeof(StiGeneralFormatService), "GeneralFormat");
		StiSerializing.AddStringType(typeof(StiNumberFormatService), "NumberFormat");
		StiSerializing.AddStringType(typeof(StiCurrencyFormatService), "CurrencyFormat");
		StiSerializing.AddStringType(typeof(StiPercentageFormatService), "PercentageFormat");
		StiSerializing.AddStringType(typeof(StiDateFormatService), "DateFormat");
		StiSerializing.AddStringType(typeof(StiTimeFormatService), "TimeFormat");
		StiSerializing.AddStringType(typeof(StiBooleanFormatService), "BooleanFormat");
		StiSerializing.AddStringType(typeof(StiCustomFormatService), "CustomFormat");
		StiSerializing.AddStringType(typeof(StiCentimetersUnit), "cm");
		StiSerializing.AddStringType(typeof(StiHundredthsOfInchUnit), "hi");
		StiSerializing.AddStringType(typeof(StiInchesUnit), "in");
		StiSerializing.AddStringType(typeof(StiMillimetersUnit), "mm");
		StiSerializing.AddStringType(typeof(StiDictionary), "Dictionary");
		StiSerializing.AddStringType(typeof(StiDataTableSource), "DataTableSource");
		StiSerializing.AddStringType(typeof(StiDataRelation), "DataRelation");
		StiSerializing.AddStringType(typeof(StiReport), "Report");
		StiSerializing.AddStringType(typeof(StiCSharpLanguage), "CSharp");
		StiSerializing.AddStringType(typeof(StiVBLanguage), "VB");
		StiSerializing.AddStringType(typeof(StiRenderProviderV1), "RenderProvider");
		StiSerializing.AddStringType(typeof(StiHighlightCondition), "HighlightCondition");
		StiSerializing.AddStringType(typeof(StiConditionsCollection), "Conditions");
		StiSerializing.AddStringType(typeof(StiBookmark), "Bookmark");
		StiSerializing.AddStringType(typeof(StiPanel), "Panel");
		StiSerializing.AddStringType(typeof(StiTableCell), "TableCell");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiForm", "Form");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiButtonControl", "Stimulsoft.Report.ReportControls.StiButtonControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiCheckBoxControl", "Stimulsoft.Report.ReportControls.StiCheckBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiCheckedListBoxControl", "Stimulsoft.Report.ReportControls.StiCheckedListBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiComboBoxControl", "Stimulsoft.Report.ReportControls.StiComboBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiDateTimePickerControl", "Stimulsoft.Report.ReportControls.StiDateTimePickerControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiForm", "Stimulsoft.Report.ReportControls.StiForm");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiGridControl", "Stimulsoft.Report.ReportControls.StiGridControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiGroupBoxControl", "Stimulsoft.Report.ReportControls.StiGroupBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiLabelControl", "Stimulsoft.Report.ReportControls.StiLabelControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiListBoxControl", "Stimulsoft.Report.ReportControls.StiListBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiLookUpBoxControl", "Stimulsoft.Report.ReportControls.StiLookUpBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiNumericUpDownControl", "Stimulsoft.Report.ReportControls.StiNumericUpDownControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiPanelControl", "Stimulsoft.Report.ReportControls.StiPanelControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiPictureBoxControl", "Stimulsoft.Report.ReportControls.StiPictureBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiRadioButtonControl", "Stimulsoft.Report.ReportControls.StiRadioButtonControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiTextBoxControl", "Stimulsoft.Report.ReportControls.StiTextBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiRichTextBoxControl", "Stimulsoft.Report.ReportControls.StiRichTextBoxControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiTreeViewControl", "Stimulsoft.Report.ReportControls.StiTreeViewControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.Dialogs.StiListViewControl", "Stimulsoft.Report.ReportControls.StiListViewControl");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiBarCode", "Stimulsoft.Report.Components.StiBarCode");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiCodabarBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiCodabarBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiCode128aBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiCode128aBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiCode128bBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiCode128bBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiCode128cBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiCode128cBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiCode39BarCodeType", "Stimulsoft.Report.Components.BarCodes.StiCode39BarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiCode39ExtBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiCode39ExtBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiCode93BarCodeType", "Stimulsoft.Report.Components.BarCodes.StiCode93BarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiCode93ExtBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiCode93ExtBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiEAN128aBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiEAN128aBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiEAN128bBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiEAN128bBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiEAN128cBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiEAN128cBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiEAN13BarCodeType", "Stimulsoft.Report.Components.BarCodes.StiEAN13BarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiEAN8BarCodeType", "Stimulsoft.Report.Components.BarCodes.StiEAN8BarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiInterleaved2of5BarCodeType", "Stimulsoft.Report.Components.BarCodes.StiInterleaved2of5BarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiMsiBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiMsiBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiPlesseyBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiPlesseyBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiPostnetBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiPostnetBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiStandard2of5BarCodeType", "Stimulsoft.Report.Components.BarCodes.StiStandard2of5BarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiUpcABarCodeType", "Stimulsoft.Report.Components.BarCodes.StiUpcABarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiUpcEBarCodeType", "Stimulsoft.Report.Components.BarCodes.StiUpcEBarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiUpcSup2BarCodeType", "Stimulsoft.Report.Components.BarCodes.StiUpcSup2BarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Report.BarCodes.StiUpcSup5BarCodeType", "Stimulsoft.Report.Components.BarCodes.StiUpcSup5BarCodeType");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Viewer.StiPageViewMode", "Stimulsoft.Report.Render.StiPageViewMode");
		StiSerializing.AddSourceTypeToDestinationType("Stimulsoft.Viewer.StiPreviewSettings", "Stimulsoft.Report.Render.StiPreviewSettings");
	}

	public StiReportObjectStringConverter()
		: this(allowLoadOptimization: false)
	{
	}

	public StiReportObjectStringConverter(bool allowLoadOptimization)
	{
		AllowLoadOptimization = allowLoadOptimization;
	}
}
