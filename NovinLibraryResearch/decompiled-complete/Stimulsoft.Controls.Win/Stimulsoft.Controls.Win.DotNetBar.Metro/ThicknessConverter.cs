using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

public class ThicknessConverter : TypeConverter
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
		double[] array2 = new double[array.Length];
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(double));
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = (double)converter.ConvertFromString(context, culture, array[i]);
		}
		if (array2.Length != 4)
		{
			throw new ArgumentException("Text Parsing Failed");
		}
		return new Thickness(array2[0], array2[1], array2[2], array2[3]);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (value is Thickness)
		{
			if (destinationType == typeof(string))
			{
				Thickness thickness = (Thickness)value;
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string separator = culture.TextInfo.ListSeparator + " ";
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(double));
				string[] array = new string[4];
				int num = 0;
				array[num++] = converter.ConvertToString(context, culture, thickness.Left);
				array[num++] = converter.ConvertToString(context, culture, thickness.Top);
				array[num++] = converter.ConvertToString(context, culture, thickness.Right);
				array[num++] = converter.ConvertToString(context, culture, thickness.Bottom);
				return string.Join(separator, array);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				Thickness thickness2 = (Thickness)value;
				ConstructorInfo constructor = typeof(Thickness).GetConstructor(new Type[4]
				{
					typeof(double),
					typeof(double),
					typeof(double),
					typeof(double)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[4] { thickness2.Left, thickness2.Top, thickness2.Right, thickness2.Bottom });
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
		if (obj == null || obj2 == null || obj3 == null || obj4 == null || !(obj is double) || !(obj2 is double) || !(obj3 is double) || !(obj4 is double))
		{
			throw new ArgumentException($"Property Value Invalid: left={obj}, top={obj2}, right={obj3}, bottom={obj4}");
		}
		return new Thickness((double)obj, (double)obj2, (double)obj3, (double)obj4);
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(typeof(Thickness), attributes).Sort(new string[4] { "Left", "Top", "Right", "Bottom" });
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
