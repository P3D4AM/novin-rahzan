using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartConditionConverter : StiChartFilterConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is StiChartCondition)
			{
				StiChartCondition stiChartCondition = (StiChartCondition)value;
				Type[] types = new Type[7]
				{
					typeof(Color),
					typeof(StiFilterItem),
					typeof(StiFilterDataType),
					typeof(StiFilterCondition),
					typeof(string),
					typeof(StiMarkerType),
					typeof(float)
				};
				ConstructorInfo constructor = typeof(StiChartCondition).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[7] { stiChartCondition.Color, stiChartCondition.Item, stiChartCondition.DataType, stiChartCondition.Condition, stiChartCondition.Value, stiChartCondition.MarkerType, stiChartCondition.MarkerAngle };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
		}
		else if (destinationType == typeof(string) && value is StiChartCondition stiChartCondition2)
		{
			return $"{StiReportObjectStringConverter.ConvertColorToString(stiChartCondition2.Color)},{stiChartCondition2.Item.ToString()},{stiChartCondition2.DataType.ToString()},{stiChartCondition2.Condition.ToString()},{XmlConvert.EncodeName(stiChartCondition2.Value)},{stiChartCondition2.MarkerType.ToString()},{stiChartCondition2.MarkerAngle.ToString()}";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string text = value as string;
			char[] separator = new char[1] { ',' };
			string[] array = text.Split(separator);
			string condition = array[3];
			condition = FixOldErrors(condition);
			if (array.Length == 5)
			{
				return new StiChartCondition(StiReportObjectStringConverter.ConvertStringToColor(array[0]), (StiFilterItem)Enum.Parse(typeof(StiFilterItem), array[1]), (StiFilterDataType)Enum.Parse(typeof(StiFilterDataType), array[2]), (StiFilterCondition)Enum.Parse(typeof(StiFilterCondition), condition), XmlConvert.DecodeName(array[4]));
			}
			if (array.Length == 7)
			{
				return new StiChartCondition(StiReportObjectStringConverter.ConvertStringToColor(array[0]), (StiFilterItem)Enum.Parse(typeof(StiFilterItem), array[1]), (StiFilterDataType)Enum.Parse(typeof(StiFilterDataType), array[2]), (StiFilterCondition)Enum.Parse(typeof(StiFilterCondition), condition), XmlConvert.DecodeName(array[4]), (StiMarkerType)Enum.Parse(typeof(StiMarkerType), array[5]), float.Parse(array[6]));
			}
		}
		return base.ConvertFrom(context, culture, value);
	}

	private static string FixOldErrors(string condition)
	{
		if (condition == "GreaterThen")
		{
			condition = "GreaterThan";
		}
		else
		{
			switch (condition)
			{
			case "GreaterThenOrEqualTo":
				condition = "GreaterThanOrEqualTo";
				break;
			case "LessThen":
				condition = "LessThan";
				break;
			case "LessThenOrEqualTo":
				condition = "LessThanOrEqualTo";
				break;
			case "GreaterThen":
				condition = "GreaterThan";
				break;
			}
		}
		return condition;
	}
}
