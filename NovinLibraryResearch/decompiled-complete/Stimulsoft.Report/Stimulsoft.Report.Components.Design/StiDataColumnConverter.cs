using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiDataColumnConverter : TypeConverter
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
			if (value != null && ((string)value).Trim().Length != 0)
			{
				return value as string;
			}
			return "[" + StiLocalization.Get("Report", "NotAssigned") + "]";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
