using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Base.Localization;

public class StiPointFConverter : TypeConverter
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
		float[] array2 = new float[array.Length];
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = (float)converter.ConvertFromString(context, culture, array[i]);
		}
		if (array2.Length != 2)
		{
			throw new Exception("ConvertInvalidPrimitive");
		}
		return new PointF(array2[0], array2[1]);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (value is PointF)
		{
			if (destinationType == typeof(string))
			{
				PointF pointF = (PointF)value;
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string separator = culture.TextInfo.ListSeparator + " ";
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
				string[] value2 = new string[2]
				{
					converter.ConvertToString(context, culture, pointF.X),
					converter.ConvertToString(context, culture, pointF.Y)
				};
				return string.Join(separator, value2);
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				PointF pointF2 = (PointF)value;
				ConstructorInfo constructor = typeof(PointF).GetConstructor(new Type[2]
				{
					typeof(float),
					typeof(float)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[2] { pointF2.X, pointF2.Y });
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
		object obj = propertyValues["X"];
		object obj2 = propertyValues["Y"];
		if (obj == null || obj2 == null || !(obj is float) || !(obj2 is float))
		{
			throw new Exception("ConvertInvalidPrimitive");
		}
		return new PointF((float)obj, (float)obj2);
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(typeof(PointF), attributes).Sort(new string[2] { "X", "Y" });
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
