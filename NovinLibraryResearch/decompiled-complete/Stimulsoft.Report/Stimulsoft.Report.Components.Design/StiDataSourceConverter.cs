using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components.Design;

public class StiDataSourceConverter : TypeConverter
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
				return ((StiDataSource)value).Alias;
			}
			return "[" + StiLocalization.Get("Report", "NotAssigned") + "]";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
