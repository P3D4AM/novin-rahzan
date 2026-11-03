using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiAztecBarCodeTypeConverter : StiBarCodeTypeServiceConverter
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
			StiAztecBarCodeType stiAztecBarCodeType = (StiAztecBarCodeType)value;
			Type[] types = new Type[4]
			{
				typeof(float),
				typeof(int),
				typeof(StiAztecSize),
				typeof(int)
			};
			ConstructorInfo constructor = typeof(StiAztecBarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiAztecBarCodeType.Module, stiAztecBarCodeType.ErrorCorrectionLevel, stiAztecBarCodeType.MatrixSize, stiAztecBarCodeType.CodePage };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
