using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart.Design;

public class StiChartLegendVisibleConverter : StiBoolConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string) && context?.Instance is StiChartLegend stiChartLegend)
		{
			switch (stiChartLegend.Visibility)
			{
			case StiLegendVisibility.Always:
				return Loc.GetMain("Always");
			case StiLegendVisibility.Auto:
				return Loc.GetMain("Auto");
			case StiLegendVisibility.False:
				return Loc.Get("PropertyEnum", "boolFalse");
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
