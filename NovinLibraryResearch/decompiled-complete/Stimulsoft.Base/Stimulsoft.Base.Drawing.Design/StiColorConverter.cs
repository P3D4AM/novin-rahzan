using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiColorConverter : ColorConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		string text = value as string;
		if (this is StiStyleColorConverter && text == Loc.Get("FormStyleDesigner", "FromStyle"))
		{
			return Color.Transparent;
		}
		if ((text != null) & (text.IndexOfInvariant(",") != -1))
		{
			text = text.Trim();
			string[] array = text.Split(new char[1] { ',' });
			if (array.Length == 4)
			{
				return Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()));
			}
			return Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()));
		}
		if ((text.Length == 6 || text.Length == 3) && !text.StartsWith("#"))
		{
			try
			{
				string pattern = "([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";
				Match match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
				if (match.Success)
				{
					return ColorTranslator.FromHtml("#" + text);
				}
			}
			catch
			{
			}
		}
		return base.ConvertFrom(context, culture, value);
	}

	private Color TryGetDefaultColor(Color color)
	{
		if (color.IsKnownColor)
		{
			return color;
		}
		IEnumerable<Color> source = from f in typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public)
			select (Color)f.GetValue(null, null) into c
			where (c.IsNamedColor || c.IsSystemColor) && c.ToArgb().Equals(color.ToArgb())
			select c;
		if (source.Count() > 0)
		{
			return source.First();
		}
		return color;
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			string expressionFromInstance = StiAppExpressionHelper.GetExpressionFromInstance(context);
			if (!string.IsNullOrWhiteSpace(expressionFromInstance))
			{
				return expressionFromInstance;
			}
			if (value == null)
			{
				return "null";
			}
			Color color = TryGetDefaultColor((Color)value);
			if (color == Color.Transparent && this is StiStyleColorConverter)
			{
				return Loc.Get("FormStyleDesigner", "FromStyle");
			}
			string text;
			if (color.IsSystemColor)
			{
				text = StiLocalization.Get("PropertySystemColors", color.Name, throwError: false);
				if (text == null)
				{
					return color.Name;
				}
			}
			else
			{
				if (!color.IsKnownColor)
				{
					if (color.A == byte.MaxValue)
					{
						return $"{color.R},{color.G},{color.B}";
					}
					return $"{color.A},{color.R},{color.G},{color.B}";
				}
				text = StiLocalization.Get("PropertyColor", color.Name);
			}
			return text;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
