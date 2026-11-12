using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Dashboard.Components.Chart.Design;

public class StiChartSeriesLabelsValueTypeConverter : TypeConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (value != null)
			{
				return Loc.GetEnum("StiSeriesLabelsValueType" + Enum.GetName(typeof(StiSeriesLabelsValueType), value));
			}
			return Loc.GetMain("Auto");
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
