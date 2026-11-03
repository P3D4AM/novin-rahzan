using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiPdf417BarCodeTypeConverter : StiBarCodeTypeServiceConverter
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
			StiPdf417BarCodeType stiPdf417BarCodeType = (StiPdf417BarCodeType)value;
			Type[] types = new Type[10]
			{
				typeof(float),
				typeof(StiPdf417EncodingMode),
				typeof(StiPdf417ErrorsCorrectionLevel),
				typeof(int),
				typeof(int),
				typeof(bool),
				typeof(bool),
				typeof(float),
				typeof(int),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiPdf417BarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[10] { stiPdf417BarCodeType.Module, stiPdf417BarCodeType.EncodingMode, stiPdf417BarCodeType.ErrorsCorrectionLevel, stiPdf417BarCodeType.DataColumns, stiPdf417BarCodeType.DataRows, stiPdf417BarCodeType.AutoDataColumns, stiPdf417BarCodeType.AutoDataRows, stiPdf417BarCodeType.AspectRatio, stiPdf417BarCodeType.RatioY, stiPdf417BarCodeType.ProcessTilde };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
