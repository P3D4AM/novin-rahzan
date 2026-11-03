using System;
using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Chart.Design;

public class StiAreaConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

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
			return value.ToString();
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
