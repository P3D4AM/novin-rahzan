using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Dashboard.Components.OnlineMap;

namespace Stimulsoft.Dashboard.Components.HeatmapGradient.Design;

public class StiHeatmapGradientRangeConverter : TypeConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiHeatmapGradientRange stiHeatmapGradientRange = value as StiHeatmapGradientRange;
			Type[] types = new Type[2]
			{
				typeof(Color),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiHeatmapGradientRange).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[2] { stiHeatmapGradientRange.Color, stiHeatmapGradientRange.StartExpression };
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
