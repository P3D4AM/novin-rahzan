using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Chart.Design;

public class Sti3dOptionsConverter : StiUniversalConverter
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
		if (destinationType == typeof(string))
		{
			return "(3D)";
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
