using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiITF14BarCodeTypeConverter : StiBarCodeTypeServiceConverter
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
			StiITF14BarCodeType stiITF14BarCodeType = (StiITF14BarCodeType)value;
			Type[] types = new Type[4]
			{
				typeof(float),
				typeof(float),
				typeof(float),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiITF14BarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiITF14BarCodeType.Module, stiITF14BarCodeType.Height, stiITF14BarCodeType.Ratio, stiITF14BarCodeType.PrintVerticalBars };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
