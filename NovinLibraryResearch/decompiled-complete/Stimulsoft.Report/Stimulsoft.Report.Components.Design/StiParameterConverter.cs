using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Components.Design;

public class StiParameterConverter : StiUniversalConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string) && value is StiParameter stiParameter)
		{
			if (!string.IsNullOrEmpty(stiParameter.Name) && (stiParameter.Expression == null || string.IsNullOrEmpty(stiParameter.Expression.Value)))
			{
				return stiParameter.Name;
			}
			if (string.IsNullOrEmpty(stiParameter.Name) && stiParameter.Expression != null && !string.IsNullOrEmpty(stiParameter.Expression.Value))
			{
				return stiParameter.Expression.Value;
			}
			if (string.IsNullOrEmpty(stiParameter.Name) && (stiParameter.Expression == null || string.IsNullOrEmpty(stiParameter.Expression.Value)))
			{
				return "{}";
			}
			return stiParameter.Name + "," + stiParameter.Expression.Value;
		}
		return ((StiUniversalConverter)this).ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return ((StiUniversalConverter)this).CanConvertTo(context, destinationType);
	}
}
