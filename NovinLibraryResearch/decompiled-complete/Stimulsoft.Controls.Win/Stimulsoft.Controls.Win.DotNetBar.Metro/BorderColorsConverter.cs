using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

public class BorderColorsConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (!(sourceType == typeof(string)))
		{
			return base.CanConvertFrom(context, sourceType);
		}
		return true;
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (!(destinationType == typeof(InstanceDescriptor)))
		{
			return base.CanConvertTo(context, destinationType);
		}
		return true;
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		string text = value as string;
		if (string.IsNullOrEmpty(text))
		{
			return default(BorderColors);
		}
		string text2 = text.Trim();
		if (text2.Length == 0)
		{
			return null;
		}
		if (culture == null)
		{
			culture = CultureInfo.CurrentCulture;
		}
		char c = culture.TextInfo.ListSeparator[0];
		string[] array = text2.Split(new char[1] { c });
		Color[] array2 = new Color[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = ColorScheme.GetColor(array[i]);
		}
		if (array2.Length == 1)
		{
			return new BorderColors(array2[0]);
		}
		if (array2.Length != 4)
		{
			throw new ArgumentException("Text Parsing Failed");
		}
		return new BorderColors(array2[0], array2[1], array2[2], array2[3]);
	}

	private string ToArgbString(Color color)
	{
		if (color.IsEmpty)
		{
			return "";
		}
		if (color.A == byte.MaxValue)
		{
			return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}
		return color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (value is BorderColors)
		{
			if (destinationType == typeof(string))
			{
				BorderColors borderColors = (BorderColors)value;
				if (borderColors.IsEmpty)
				{
					return "";
				}
				if (borderColors.IsUniform)
				{
					return ToArgbString(borderColors.Left);
				}
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string separator = culture.TextInfo.ListSeparator + " ";
				string[] array = new string[4];
				int num = 0;
				array[num++] = ToArgbString(borderColors.Left);
				array[num++] = ToArgbString(borderColors.Top);
				array[num++] = ToArgbString(borderColors.Right);
				array[num++] = ToArgbString(borderColors.Bottom);
				return string.Join(separator, array);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				BorderColors borderColors2 = (BorderColors)value;
				ConstructorInfo constructor = typeof(BorderColors).GetConstructor(new Type[4]
				{
					typeof(Color),
					typeof(Color),
					typeof(Color),
					typeof(Color)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[4] { borderColors2.Left, borderColors2.Top, borderColors2.Right, borderColors2.Bottom });
				}
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
	{
		if (propertyValues == null)
		{
			throw new ArgumentNullException("propertyValues");
		}
		object obj = propertyValues["Left"];
		object obj2 = propertyValues["Top"];
		object obj3 = propertyValues["Right"];
		object obj4 = propertyValues["Bottom"];
		if (obj == null || obj2 == null || obj3 == null || obj4 == null || !(obj is Color) || !(obj2 is Color) || !(obj3 is Color) || !(obj4 is Color))
		{
			throw new ArgumentException($"Property Value Invalid: left={obj}, top={obj2}, right={obj3}, bottom={obj4}");
		}
		return new BorderColors((Color)obj, (Color)obj2, (Color)obj3, (Color)obj4);
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(typeof(BorderColors), attributes).Sort(new string[4] { "Left", "Top", "Right", "Bottom" });
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
