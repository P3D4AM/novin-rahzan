using System;
using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Gauge.Design;

public class StiGaugeStyleConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

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
			if (value is StiCustomGaugeStyle)
			{
				StiGauge stiGauge = context.Instance as StiGauge;
				if (!string.IsNullOrEmpty(stiGauge.CustomStyleName))
				{
					return stiGauge.CustomStyleName;
				}
			}
			if (value == null)
			{
				return string.Empty;
			}
			return value.ToString();
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
