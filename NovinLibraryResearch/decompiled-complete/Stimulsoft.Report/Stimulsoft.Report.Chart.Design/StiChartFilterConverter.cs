using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartFilterConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is StiChartFilter)
			{
				StiChartFilter stiChartFilter = (StiChartFilter)value;
				Type[] types = new Type[4]
				{
					typeof(StiFilterItem),
					typeof(StiFilterDataType),
					typeof(StiFilterCondition),
					typeof(string)
				};
				ConstructorInfo constructor = typeof(StiChartFilter).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[4] { stiChartFilter.Item, stiChartFilter.DataType, stiChartFilter.Condition, stiChartFilter.Value };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
		}
		else if (destinationType == typeof(string) && value is StiChartFilter stiChartFilter2)
		{
			return $"{stiChartFilter2.Item.ToString()},{stiChartFilter2.DataType.ToString()},{stiChartFilter2.Condition.ToString()},{XmlConvert.EncodeName(stiChartFilter2.Value)}";
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
			string filterCondition = array[2];
			filterCondition = FixOldErrors(filterCondition);
			return new StiChartFilter((StiFilterItem)Enum.Parse(typeof(StiFilterItem), array[0]), (StiFilterDataType)Enum.Parse(typeof(StiFilterDataType), array[1]), (StiFilterCondition)Enum.Parse(typeof(StiFilterCondition), filterCondition), XmlConvert.DecodeName(array[3]));
		}
		return base.ConvertFrom(context, culture, value);
	}

	private static string FixOldErrors(string filterCondition)
	{
		if (filterCondition == "GreaterThen")
		{
			filterCondition = "GreaterThan";
		}
		if (filterCondition == "GreaterThenOrEqualTo")
		{
			filterCondition = "GreaterThanOrEqualTo";
		}
		if (filterCondition == "LessThen")
		{
			filterCondition = "LessThan";
		}
		if (filterCondition == "LessThenOrEqualTo")
		{
			filterCondition = "LessThanOrEqualTo";
		}
		return filterCondition;
	}

	protected object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
