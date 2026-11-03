using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Components.Design;

public class StiDrillDownParameterConverter : StiUniversalConverter
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
		if (destinationType == typeof(string) && value is StiDrillDownParameter stiDrillDownParameter)
		{
			if (!string.IsNullOrEmpty(stiDrillDownParameter.Name) && (stiDrillDownParameter.Expression == null || string.IsNullOrEmpty(stiDrillDownParameter.Expression.Value)))
			{
				return stiDrillDownParameter.Name;
			}
			if (string.IsNullOrEmpty(stiDrillDownParameter.Name) && stiDrillDownParameter.Expression != null && !string.IsNullOrEmpty(stiDrillDownParameter.Expression.Value))
			{
				return stiDrillDownParameter.Expression.Value;
			}
			if (string.IsNullOrEmpty(stiDrillDownParameter.Name) && (stiDrillDownParameter.Expression == null || string.IsNullOrEmpty(stiDrillDownParameter.Expression.Value)))
			{
				return "{}";
			}
			return stiDrillDownParameter.Name + "," + stiDrillDownParameter.Expression.Value;
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
