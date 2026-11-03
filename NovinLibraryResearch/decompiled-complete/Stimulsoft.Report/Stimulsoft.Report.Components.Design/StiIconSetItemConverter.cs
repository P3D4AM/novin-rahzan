using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace Stimulsoft.Report.Components.Design;

public class StiIconSetItemConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is StiIconSetItem)
			{
				StiIconSetItem stiIconSetItem = (StiIconSetItem)value;
				Type[] types = new Type[4]
				{
					typeof(StiIcon),
					typeof(StiIconSetOperation),
					typeof(StiIconSetValueType),
					typeof(float)
				};
				ConstructorInfo constructor = typeof(StiIconSetItem).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[4] { stiIconSetItem.Icon, stiIconSetItem.Operation, stiIconSetItem.ValueType, stiIconSetItem.Value };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
		}
		else if (destinationType == typeof(string) && value is StiIconSetItem stiIconSetItem2)
		{
			return $"{XmlConvert.EncodeName(stiIconSetItem2.Icon.ToString())},{XmlConvert.EncodeName(stiIconSetItem2.Operation.ToString())},{XmlConvert.EncodeName(stiIconSetItem2.ValueType.ToString())},{XmlConvert.EncodeName(stiIconSetItem2.Value.ToString())}";
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
			return new StiIconSetItem((StiIcon)Enum.Parse(typeof(StiIcon), XmlConvert.DecodeName(array[0])), (StiIconSetOperation)Enum.Parse(typeof(StiIconSetOperation), XmlConvert.DecodeName(array[1])), (StiIconSetValueType)Enum.Parse(typeof(StiIconSetValueType), XmlConvert.DecodeName(array[2])), float.Parse(XmlConvert.DecodeName(array[3])));
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
