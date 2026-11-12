using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

public class MetroColorGeneratorParametersConverter : TypeConverter
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
		if (!(value is string text))
		{
			return base.ConvertFrom(context, culture, value);
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
		if (array.Length != 2)
		{
			throw new ArgumentException("String parsing failed due to incorrect format.");
		}
		MetroColorGeneratorParameters metroColorGeneratorParameters = default(MetroColorGeneratorParameters);
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
		metroColorGeneratorParameters.CanvasColor = (Color)converter.ConvertFromString(context, culture, array[0]);
		metroColorGeneratorParameters.BaseColor = (Color)converter.ConvertFromString(context, culture, array[1]);
		return metroColorGeneratorParameters;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (value is MetroColorGeneratorParameters)
		{
			if (destinationType == typeof(string))
			{
				MetroColorGeneratorParameters metroColorGeneratorParameters = (MetroColorGeneratorParameters)value;
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string separator = culture.TextInfo.ListSeparator + " ";
				string[] array = new string[2];
				int num = 0;
				array[num++] = ColorToString(culture, metroColorGeneratorParameters.CanvasColor);
				array[num++] = ColorToString(culture, metroColorGeneratorParameters.BaseColor);
				return string.Join(separator, array);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				MetroColorGeneratorParameters metroColorGeneratorParameters2 = (MetroColorGeneratorParameters)value;
				ConstructorInfo constructor = typeof(MetroColorGeneratorParameters).GetConstructor(new Type[2]
				{
					typeof(Color),
					typeof(Color)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[2] { metroColorGeneratorParameters2.CanvasColor, metroColorGeneratorParameters2.BaseColor });
				}
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private static string ColorToString(CultureInfo culture, Color color)
	{
		if (color == Color.Empty)
		{
			return string.Empty;
		}
		if (color.IsKnownColor)
		{
			return color.Name;
		}
		if (color.IsNamedColor)
		{
			return "'" + color.Name + "'";
		}
		if (culture == null)
		{
			culture = CultureInfo.CurrentCulture;
		}
		string separator = "";
		int num = 0;
		string[] array;
		if (color.A < byte.MaxValue)
		{
			array = new string[4];
			array[num++] = color.A.ToString("X");
		}
		else
		{
			array = new string[3];
		}
		array[num++] = color.R.ToString("X");
		array[num++] = color.G.ToString("X");
		array[num++] = color.B.ToString("X");
		return "#" + string.Join(separator, array);
	}

	public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
	{
		if (propertyValues == null)
		{
			throw new ArgumentNullException("propertyValues");
		}
		object obj = propertyValues["CanvasColor"];
		object obj2 = propertyValues["BaseColor"];
		if (obj == null || !(obj is Color) || obj2 == null || !(obj2 is Color))
		{
			throw new ArgumentException("Invalid property value entry");
		}
		return new MetroColorGeneratorParameters((Color)obj, (Color)obj2);
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(typeof(MetroColorGeneratorParameters), attributes).Sort(new string[2] { "CanvasColor", "BaseColor" });
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
