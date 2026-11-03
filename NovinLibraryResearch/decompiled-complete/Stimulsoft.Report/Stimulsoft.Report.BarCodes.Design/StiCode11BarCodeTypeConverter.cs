using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiCode11BarCodeTypeConverter : StiBarCodeTypeServiceConverter
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
			StiCode11BarCodeType stiCode11BarCodeType = (StiCode11BarCodeType)value;
			Type[] types = new Type[3]
			{
				typeof(float),
				typeof(float),
				typeof(StiCode11CheckSum)
			};
			ConstructorInfo constructor = typeof(StiCode11BarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[3] { stiCode11BarCodeType.Module, stiCode11BarCodeType.Height, stiCode11BarCodeType.Checksum };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
