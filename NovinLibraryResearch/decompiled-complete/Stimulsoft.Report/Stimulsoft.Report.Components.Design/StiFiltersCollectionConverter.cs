using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiFiltersCollectionConverter : TypeConverter
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
		if (destinationType == typeof(string) && value != null)
		{
			if (value is StiFiltersCollection { Count: not 0 })
			{
				return "(" + Loc.Get("PropertyMain", "Filters") + ")";
			}
			return "[" + Loc.Get("FormBand", "NoFilters") + "]";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
