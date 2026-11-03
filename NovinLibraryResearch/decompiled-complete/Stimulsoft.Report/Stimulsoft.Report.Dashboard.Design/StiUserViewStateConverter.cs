using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dashboard.Design;

public class StiUserViewStateConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null && value is StiUserViewState stiUserViewState)
		{
			Type[] types = new Type[4]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(StiChartSeriesType)
			};
			ConstructorInfo constructor = typeof(StiUserViewState).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiUserViewState.Key, stiUserViewState.Name, stiUserViewState.State, stiUserViewState.SeriesType };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
