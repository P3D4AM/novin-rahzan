using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Dashboard.Components.Gauge.Design;

public class StiGaugeRangeConverter : TypeConverter
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
			StiGaugeRange stiGaugeRange = value as StiGaugeRange;
			Type[] types = new Type[3]
			{
				typeof(Color),
				typeof(string),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiGaugeRange).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[3] { stiGaugeRange.Color, stiGaugeRange.StartExpression, stiGaugeRange.EndExpression };
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
