using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Data.Extensions;

namespace Stimulsoft.Dashboard.Components.Design;

public class StiMeterListConverter : TypeConverter
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
				return "(" + Loc.Get("Report", "NotAssigned") + ")";
			}
			if (ListExt.IsList(value) && !ListExt.ToList(value).Any())
			{
				return "(" + Loc.Get("Report", "NotAssigned") + ")";
			}
			return "(" + context.PropertyDescriptor.DisplayName + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
