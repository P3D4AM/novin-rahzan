using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace Stimulsoft.Report.Design;

public class StiStyleConditionConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is StiStyleCondition)
			{
				StiStyleCondition stiStyleCondition = value as StiStyleCondition;
				Type[] types = new Type[11]
				{
					typeof(StiStyleConditionType),
					typeof(StiStyleConditionOperation),
					typeof(StiStyleConditionOperation),
					typeof(StiStyleConditionOperation),
					typeof(StiStyleConditionOperation),
					typeof(StiStyleConditionOperation),
					typeof(StiStyleComponentPlacement),
					typeof(int),
					typeof(StiStyleComponentType),
					typeof(StiStyleLocation),
					typeof(string)
				};
				ConstructorInfo constructor = typeof(StiStyleCondition).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[11]
					{
						stiStyleCondition.Type, stiStyleCondition.OperationPlacement, stiStyleCondition.OperationPlacementNestedLevel, stiStyleCondition.OperationComponentType, stiStyleCondition.OperationLocation, stiStyleCondition.OperationComponentName, stiStyleCondition.Placement, stiStyleCondition.PlacementNestedLevel, stiStyleCondition.ComponentType, stiStyleCondition.Location,
						stiStyleCondition.ComponentName
					};
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
		}
		else if (destinationType == typeof(string) && value is StiStyleCondition stiStyleCondition2)
		{
			return $"{XmlConvert.EncodeName(stiStyleCondition2.Type.ToString())},{XmlConvert.EncodeName(stiStyleCondition2.OperationPlacement.ToString())},{XmlConvert.EncodeName(stiStyleCondition2.OperationPlacementNestedLevel.ToString())},{XmlConvert.EncodeName(stiStyleCondition2.OperationComponentType.ToString())},{XmlConvert.EncodeName(stiStyleCondition2.OperationLocation.ToString())},{XmlConvert.EncodeName(stiStyleCondition2.OperationComponentName.ToString())},{XmlConvert.EncodeName(stiStyleCondition2.Placement.ToString())},{stiStyleCondition2.PlacementNestedLevel},{XmlConvert.EncodeName(stiStyleCondition2.ComponentType.ToString())},{XmlConvert.EncodeName(stiStyleCondition2.Location.ToString())},{XmlConvert.EncodeName(stiStyleCondition2.ComponentName)}";
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
			StiStyleCondition stiStyleCondition = new StiStyleCondition();
			stiStyleCondition.Type = (StiStyleConditionType)Enum.Parse(typeof(StiStyleConditionType), XmlConvert.DecodeName(array[0]));
			stiStyleCondition.OperationPlacement = (StiStyleConditionOperation)Enum.Parse(typeof(StiStyleConditionOperation), XmlConvert.DecodeName(array[1]));
			stiStyleCondition.OperationPlacementNestedLevel = (StiStyleConditionOperation)Enum.Parse(typeof(StiStyleConditionOperation), XmlConvert.DecodeName(array[2]));
			stiStyleCondition.OperationComponentType = (StiStyleConditionOperation)Enum.Parse(typeof(StiStyleConditionOperation), XmlConvert.DecodeName(array[3]));
			stiStyleCondition.OperationLocation = (StiStyleConditionOperation)Enum.Parse(typeof(StiStyleConditionOperation), XmlConvert.DecodeName(array[4]));
			stiStyleCondition.OperationComponentName = (StiStyleConditionOperation)Enum.Parse(typeof(StiStyleConditionOperation), XmlConvert.DecodeName(array[5]));
			stiStyleCondition.Placement = (StiStyleComponentPlacement)Enum.Parse(typeof(StiStyleComponentPlacement), XmlConvert.DecodeName(array[6]));
			stiStyleCondition.PlacementNestedLevel = int.Parse(array[7]);
			stiStyleCondition.ComponentType = (StiStyleComponentType)Enum.Parse(typeof(StiStyleComponentType), XmlConvert.DecodeName(array[8]));
			stiStyleCondition.Location = (StiStyleLocation)Enum.Parse(typeof(StiStyleLocation), XmlConvert.DecodeName(array[9]));
			stiStyleCondition.ComponentName = XmlConvert.DecodeName(array[10]);
			return stiStyleCondition;
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
