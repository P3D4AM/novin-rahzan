using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart.Design;

public class StiXTopChartAxisConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
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

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (value != null)
			{
				return "(" + Loc.Get("PropertyMain", context.PropertyDescriptor.Name) + ")";
			}
			return "(" + Loc.Get("Report", "NotAssigned") + ")";
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiXTopChartAxis stiXTopChartAxis = value as StiXTopChartAxis;
			Type[] types = new Type[4]
			{
				typeof(StiChartAxisLabels),
				typeof(StiXChartAxisTitle),
				typeof(bool),
				typeof(StiAutoBool)
			};
			ConstructorInfo constructor = typeof(StiXTopChartAxis).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiXTopChartAxis.Labels, stiXTopChartAxis.Title, stiXTopChartAxis.Visible, stiXTopChartAxis.ShowEdgeValues };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
