using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiTypeConverter : TypeConverter
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
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (destinationType == typeof(string))
		{
			return ((object)new StiTypeWrapper((Type)value)).ToString();
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
