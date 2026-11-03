using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiCode39BarCodeTypeConverter : StiBarCodeTypeServiceConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiCode39BarCodeType stiCode39BarCodeType = (StiCode39BarCodeType)value;
			Type[] types = new Type[4]
			{
				typeof(float),
				typeof(float),
				typeof(float),
				typeof(StiCheckSum)
			};
			ConstructorInfo constructor = typeof(StiCode39BarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiCode39BarCodeType.Module, stiCode39BarCodeType.Height, stiCode39BarCodeType.Ratio, stiCode39BarCodeType.CheckSum };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
