using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components.Indicator.Design;

public class StiIndicatorIconRangeConverter : TypeConverter
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
			StiIndicatorIconRange stiIndicatorIconRange = value as StiIndicatorIconRange;
			Type[] types = new Type[3]
			{
				typeof(StiFontIcons),
				typeof(string),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiIndicatorIconRange).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[3] { stiIndicatorIconRange.Icon, stiIndicatorIconRange.StartExpression, stiIndicatorIconRange.EndExpression };
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
