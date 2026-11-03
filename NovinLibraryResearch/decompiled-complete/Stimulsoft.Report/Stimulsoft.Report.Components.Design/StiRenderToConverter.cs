using System;
using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Components.Design;

public class StiRenderToConverter : TypeConverter
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
			return value as string;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
