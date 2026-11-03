using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DevComponents.DotNetBar;

public class RangeValueConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if ((object)sourceType != typeof(string))
		{
			return base.CanConvertFrom(context, sourceType);
		}
		return true;
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if ((object)destinationType != typeof(InstanceDescriptor))
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
		int[] array2 = new int[array.Length];
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
		}
		if (array2.Length != 2)
		{
			throw new ArgumentException("Text Parsing Failed, only 2 comma separated values are accepted");
		}
		return new RangeValue(array2[0], array2[1]);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if ((object)destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (value is RangeValue)
		{
			if ((object)destinationType == typeof(string))
			{
				RangeValue rangeValue = (RangeValue)value;
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string separator = culture.TextInfo.ListSeparator + " ";
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
				string[] array = new string[2];
				int num = 0;
				array[num++] = converter.ConvertToString(context, culture, rangeValue.Min);
				array[num++] = converter.ConvertToString(context, culture, rangeValue.Max);
				return string.Join(separator, array);
			}
			if ((object)destinationType == typeof(InstanceDescriptor))
			{
				RangeValue rangeValue2 = (RangeValue)value;
				ConstructorInfo constructor = typeof(RangeValue).GetConstructor(new Type[2]
				{
					typeof(int),
					typeof(int)
				});
				if ((object)constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[2] { rangeValue2.Min, rangeValue2.Max });
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
		object obj = propertyValues["Min"];
		object obj2 = propertyValues["Max"];
		if (obj == null || obj2 == null || !(obj is int) || !(obj2 is int))
		{
			throw new ArgumentException("Property Value Invalid Entry");
		}
		return new RangeValue((int)obj, (int)obj2);
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(typeof(RangeValue), attributes).Sort(new string[2] { "Min", "Max" });
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
