using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiCodabarBarCodeTypeConverter : StiBarCodeTypeServiceConverter
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
			StiCodabarBarCodeType stiCodabarBarCodeType = (StiCodabarBarCodeType)value;
			Type[] types = new Type[3]
			{
				typeof(float),
				typeof(float),
				typeof(float)
			};
			ConstructorInfo constructor = typeof(StiCodabarBarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[3] { stiCodabarBarCodeType.Module, stiCodabarBarCodeType.Height, stiCodabarBarCodeType.Ratio };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
