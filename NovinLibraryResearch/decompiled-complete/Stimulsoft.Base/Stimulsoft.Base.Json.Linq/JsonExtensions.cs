using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Json.Linq;

public static class JsonExtensions
{
	public static void RemoveProperty(this JObject jObject, string propertyName)
	{
		foreach (JProperty childrenToken in jObject.ChildrenTokens)
		{
			if (childrenToken.Name == propertyName)
			{
				jObject.ChildrenTokens.Remove(childrenToken);
				break;
			}
		}
	}

	public static void AddPropertyInt(this JObject jObject, string propertyName, int value, int defaultValue = 0)
	{
		jObject.RemoveProperty(propertyName);
		if (value != defaultValue)
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyIntNoDefaultValue(this JObject jObject, string propertyName, int value)
	{
		jObject.RemoveProperty(propertyName);
		jObject.Add(new JProperty(propertyName, value));
	}

	public static void AddPropertyFloat(this JObject jObject, string propertyName, float value, float defaultValue = 0f)
	{
		jObject.RemoveProperty(propertyName);
		if (value != defaultValue)
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyFloatNullable(this JObject jObject, string propertyName, float? value, float? defaultValue)
	{
		jObject.RemoveProperty(propertyName);
		if (value != defaultValue)
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyDouble(this JObject jObject, string propertyName, double value, double defaultValue = 0.0)
	{
		jObject.RemoveProperty(propertyName);
		if (value != defaultValue)
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyShort(this JObject jObject, string propertyName, short value, short defaultValue = 0)
	{
		jObject.RemoveProperty(propertyName);
		if (value != defaultValue)
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyDouble(this JObject jObject, string propertyName, double value)
	{
		jObject.RemoveProperty(propertyName);
		jObject.Add(new JProperty(propertyName, value));
	}

	public static void AddPropertyDecimal(this JObject jObject, string propertyName, decimal value, decimal defaultValue)
	{
		jObject.RemoveProperty(propertyName);
		if (!(value == defaultValue))
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyDecimal(this JObject jObject, string propertyName, decimal value)
	{
		jObject.RemoveProperty(propertyName);
		jObject.Add(new JProperty(propertyName, value));
	}

	public static void AddPropertyIntArray(this JObject jObject, string propertyName, int[] array)
	{
		jObject.RemoveProperty(propertyName);
		JObject jObject2 = StiJsonReportObjectHelper.Serialize.IntArray(array);
		if (jObject2 != null && jObject2.Count != 0)
		{
			jObject.Add(new JProperty(propertyName, jObject2));
		}
	}

	public static void AddPropertyJObject(this JObject jObject, string propertyName, JObject value)
	{
		jObject.RemoveProperty(propertyName);
		if (value != null && value.Count != 0)
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyStringArray(this JObject jObject, string propertyName, string[] array)
	{
		jObject.RemoveProperty(propertyName);
		JObject jObject2 = StiJsonReportObjectHelper.Serialize.StringArray(array);
		if (jObject2 != null && jObject2.Count != 0)
		{
			jObject.Add(new JProperty(propertyName, jObject2));
		}
	}

	public static void AddPropertyBoolArray(this JObject jObject, string propertyName, bool[] array, bool ignoreDefaultValues = false)
	{
		jObject.RemoveProperty(propertyName);
		JObject jObject2 = StiJsonReportObjectHelper.Serialize.BoolArray(array, ignoreDefaultValues);
		if (jObject2 != null && jObject2.Count != 0)
		{
			jObject.Add(new JProperty(propertyName, jObject2));
		}
	}

	public static void AddPropertyListString(this JObject jObject, string propertyName, List<string> array)
	{
		jObject.RemoveProperty(propertyName);
		JObject jObject2 = StiJsonReportObjectHelper.Serialize.ListString(array);
		if (jObject2 != null && jObject2.Count != 0)
		{
			jObject.Add(new JProperty(propertyName, jObject2));
		}
	}

	public static void AddPropertyPoint(this JObject jObject, string propertyName, Point point)
	{
		jObject.RemoveProperty(propertyName);
		JObject jObject2 = StiJsonReportObjectHelper.Serialize.Point(point);
		if (jObject2 != null && jObject2.Count != 0)
		{
			jObject.Add(new JProperty(propertyName, jObject2));
		}
	}

	public static void AddPropertyPointF(this JObject jObject, string propertyName, PointF point)
	{
		jObject.RemoveProperty(propertyName);
		JObject jObject2 = StiJsonReportObjectHelper.Serialize.PointF(point);
		if (jObject2 != null && jObject2.Count != 0)
		{
			jObject.Add(new JProperty(propertyName, jObject2));
		}
	}

	public static void AddPropertyIdent(this JObject jObject, string propertyName, string value)
	{
		jObject.Add(new JProperty(propertyName, value));
	}

	public static void AddPropertyBool(this JObject jObject, string propertyName, bool value, bool defaultValue = false, bool ignoreDefaultValues = false)
	{
		jObject.RemoveProperty(propertyName);
		if (value != defaultValue || ignoreDefaultValues)
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyDateTime(this JObject jObject, string propertyName, DateTime value)
	{
		jObject.RemoveProperty(propertyName);
		string content = JsonConvert.SerializeObject(value, new JsonSerializerSettings
		{
			DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
		}).Replace("\"", string.Empty).Replace("\\", string.Empty);
		jObject.Add(new JProperty(propertyName, content));
	}

	public static void AddPropertyTimeSpan(this JObject jObject, string propertyName, TimeSpan value)
	{
		jObject.RemoveProperty(propertyName);
		jObject.Add(new JProperty(propertyName, value.Ticks));
	}

	public static void AddPropertyEnum(this JObject jObject, string propertyName, Enum value, Enum defaultValue)
	{
		jObject.RemoveProperty(propertyName);
		if (!value.Equals(defaultValue))
		{
			jObject.Add(new JProperty(propertyName, value.ToString()));
		}
	}

	public static void AddPropertyEnum(this JObject jObject, string propertyName, Enum value)
	{
		jObject.RemoveProperty(propertyName);
		if (value != null)
		{
			jObject.Add(new JProperty(propertyName, value.ToString()));
		}
	}

	public static void AddPropertyString(this JObject jObject, string propertyName, string value, string defaultValue)
	{
		jObject.RemoveProperty(propertyName);
		if (value != null && !value.Equals(defaultValue))
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyString(this JObject jObject, string propertyName, string value)
	{
		jObject.RemoveProperty(propertyName);
		jObject.Add(new JProperty(propertyName, value));
	}

	public static void AddPropertyRectangleD(this JObject jObject, string propertyName, RectangleD rect)
	{
		jObject.RemoveProperty(propertyName);
		string content = StiJsonReportObjectHelper.Serialize.RectangleD(rect);
		jObject.Add(new JProperty(propertyName, content));
	}

	public static void AddPropertySizeD(this JObject jObject, string propertyName, SizeD size, string defaultValue = "0,0")
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.SizeD(size);
		if (text != null && !text.Equals(defaultValue))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertySize(this JObject jObject, string propertyName, Size size)
	{
		jObject.RemoveProperty(propertyName);
		JObject jObject2 = StiJsonReportObjectHelper.Serialize.Size(size);
		if (jObject2 != null)
		{
			jObject.Add(new JProperty(propertyName, jObject2));
		}
	}

	public static void AddPropertyShadow(this JObject jObject, string propertyName, StiSimpleShadow shadow)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.JShadow(shadow);
		if (text != null)
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyCornerRadius(this JObject jObject, string propertyName, StiCornerRadius cornerRadius)
	{
		jObject.RemoveProperty(propertyName);
		string text = cornerRadius.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyStringNullOrEmpty(this JObject jObject, string propertyName, string value)
	{
		jObject.RemoveProperty(propertyName);
		if (!string.IsNullOrEmpty(value))
		{
			jObject.Add(new JProperty(propertyName, value));
		}
	}

	public static void AddPropertyCap(this JObject jObject, string propertyName, StiCap cap)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.JCap(cap);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyByteArray(this JObject jObject, string propertyName, byte[] buffer)
	{
		jObject.RemoveProperty(propertyName);
		string text = Convert.ToBase64String(buffer);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyImage(this JObject jObject, string propertyName, Image image)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiImageConverter.ImageToString(image);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyBrush(this JObject jObject, string propertyName, StiBrush brush)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.JBrush(brush);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyBorder(this JObject jObject, string propertyName, StiSimpleBorder border)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.JBorder(border);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyBorder(this JObject jObject, string propertyName, StiBorder border)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.JBorder(border);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyColor(this JObject jObject, string propertyName, Color color, string htmlDefaultColor)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.JColor(color, ColorTranslator.FromHtml(htmlDefaultColor));
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyArrayColor(this JObject jObject, string propertyName, Color[] colors)
	{
		JObject jObject2 = new JObject();
		for (int i = 0; i < colors.Length; i++)
		{
			Color color = colors[i];
			string value = StiJsonReportObjectHelper.Serialize.JColor(color);
			jObject2.AddPropertyString(i.ToString(), value);
		}
		jObject.AddPropertyJObject(propertyName, jObject2);
	}

	public static void AddPropertyColor(this JObject jObject, string propertyName, Color color, Color defaultColor)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.JColor(color, defaultColor);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontArial8(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontArial8(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontMicrosoftSansSerif8(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontMicrosoftSansSerif8(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontTahoma8(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontTahoma8(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontTahoma12Bold(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontTahoma12Bold(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontArial8BoldPixel(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontArial8BoldPixel(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontArial7(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontArial7(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontArial10(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontArial10(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontArial13(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontArial13(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontArial11(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontArial11(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontArial12Bold(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontArial12Bold(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFontArial14Bold(this JObject jObject, string propertyName, Font font)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.FontArial14Bold(font);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}

	public static void AddPropertyFont(this JObject jObject, string propertyName, Font font, string defaultFamily, float defaultEmSize)
	{
		jObject.RemoveProperty(propertyName);
		string text = StiJsonReportObjectHelper.Serialize.Font(font, defaultFamily, defaultEmSize);
		if (!string.IsNullOrEmpty(text))
		{
			jObject.Add(new JProperty(propertyName, text));
		}
	}
}
