using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiQRCodeBarCodeTypeConverter : StiBarCodeTypeServiceConverter
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
			StiQRCodeBarCodeType stiQRCodeBarCodeType = (StiQRCodeBarCodeType)value;
			Type[] types = new Type[5]
			{
				typeof(float),
				typeof(StiQRCodeErrorCorrectionLevel),
				typeof(StiQRCodeSize),
				typeof(Image),
				typeof(double)
			};
			ConstructorInfo constructor = typeof(StiQRCodeBarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiQRCodeBarCodeType.Module, stiQRCodeBarCodeType.ErrorCorrectionLevel, stiQRCodeBarCodeType.MatrixSize, stiQRCodeBarCodeType.Image, stiQRCodeBarCodeType.ImageMultipleFactor };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
