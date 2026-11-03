using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace Stimulsoft.Report.Components.Design;

public class StiFilterConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is StiFilter)
			{
				StiFilter stiFilter = (StiFilter)value;
				if (stiFilter.Item == StiFilterItem.Expression)
				{
					Type[] types = new Type[1] { typeof(string) };
					ConstructorInfo constructor = typeof(StiFilter).GetConstructor(types);
					if (constructor != null)
					{
						object[] objs = new object[1] { stiFilter.Expression };
						return CreateNewInstanceDescriptor(constructor, objs);
					}
				}
				else
				{
					Type[] types2 = new Type[5]
					{
						typeof(string),
						typeof(StiFilterCondition),
						typeof(string),
						typeof(string),
						typeof(StiFilterDataType)
					};
					ConstructorInfo constructor2 = typeof(StiFilter).GetConstructor(types2);
					if (constructor2 != null)
					{
						object[] objs2 = new object[5] { stiFilter.Column, stiFilter.Condition, stiFilter.Value1, stiFilter.Value2, stiFilter.DataType };
						return CreateNewInstanceDescriptor(constructor2, objs2);
					}
				}
			}
		}
		else if (destinationType == typeof(string) && value is StiFilter stiFilter2)
		{
			if (stiFilter2.Item == StiFilterItem.Expression)
			{
				return XmlConvert.EncodeName(stiFilter2.Expression);
			}
			return $"{XmlConvert.EncodeName(stiFilter2.Column)},{stiFilter2.Condition.ToString()},{XmlConvert.EncodeName(stiFilter2.Value1)},{XmlConvert.EncodeName(stiFilter2.Value2)},{stiFilter2.DataType.ToString()}";
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
		if (value is string text)
		{
			char[] separator = new char[1] { ',' };
			string[] array = text.Split(separator);
			if (array.Length == 1)
			{
				return new StiFilter(XmlConvert.DecodeName(array[0]));
			}
			string text2 = array[1];
			if (text2 == "GreaterThen")
			{
				text2 = "GreaterThan";
			}
			if (text2 == "GreaterThenOrEqualTo")
			{
				text2 = "GreaterThanOrEqualTo";
			}
			if (text2 == "LessThen")
			{
				text2 = "LessThan";
			}
			if (text2 == "LessThenOrEqualTo")
			{
				text2 = "LessThanOrEqualTo";
			}
			return new StiFilter(XmlConvert.DecodeName(array[0]), (StiFilterCondition)Enum.Parse(typeof(StiFilterCondition), text2), XmlConvert.DecodeName(array[2]), XmlConvert.DecodeName(array[3]), (StiFilterDataType)Enum.Parse(typeof(StiFilterDataType), array[4]));
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
