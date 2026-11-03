using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartConditionsCollectionConverter : TypeConverter
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
			if (!(value is StiChartConditionsCollection { Count: not 0 }))
			{
				return string.Format("[{0}]", StiLocalization.Get("Chart", "NoConditions"));
			}
			return string.Format("({0})", StiLocalization.Get("PropertyMain", "Conditions"));
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
