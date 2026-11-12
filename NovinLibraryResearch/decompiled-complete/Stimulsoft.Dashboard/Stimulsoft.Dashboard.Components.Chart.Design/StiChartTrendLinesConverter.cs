using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Dashboard.Components.Chart.Design;

public class StiChartTrendLinesConverter : TypeConverter
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
			if (value == null)
			{
				return "(" + Loc.GetMain("NoElements") + ")";
			}
			if (ListExt.IsList(value) && !ListExt.ToList(value).Any())
			{
				return "(" + Loc.GetMain("NoElements") + ")";
			}
			return "(" + context.PropertyDescriptor.DisplayName + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
