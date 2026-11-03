using System;
using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataParameterTypeConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (!(((StiDataParameter)context.Instance).DataSource is StiSqlSource stiSqlSource))
			{
				return value.ToString();
			}
			Type parameterTypesEnum = stiSqlSource.GetParameterTypesEnum();
			return Enum.ToObject(parameterTypesEnum, (int)value).ToString();
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}
}
