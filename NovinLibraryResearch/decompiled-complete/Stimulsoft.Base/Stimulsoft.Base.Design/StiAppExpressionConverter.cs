using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace Stimulsoft.Base.Design;

public class StiAppExpressionConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiAppExpression stiAppExpression = (StiAppExpression)value;
			Type[] types = new Type[2]
			{
				typeof(string),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiAppExpression).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[2] { stiAppExpression.Name, stiAppExpression.Expression };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		else if (destinationType == typeof(string) && value is StiAppExpression stiAppExpression2)
		{
			string text = XmlConvert.EncodeName(stiAppExpression2.Name);
			string text2 = XmlConvert.EncodeName(stiAppExpression2.Expression);
			return text + "," + text2;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string text = value as string;
			char[] separator = new char[1] { ',' };
			string[] array = text.Split(separator);
			return new StiAppExpression(XmlConvert.DecodeName(array[0]), XmlConvert.DecodeName(array[1]));
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
