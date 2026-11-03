using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiEAN13BarCodeTypeConverter : StiBarCodeTypeServiceConverter
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
			StiEAN13BarCodeType stiEAN13BarCodeType = (StiEAN13BarCodeType)value;
			Type[] types = new Type[5]
			{
				typeof(float),
				typeof(float),
				typeof(StiEanSupplementType),
				typeof(string),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiEAN13BarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiEAN13BarCodeType.Module, stiEAN13BarCodeType.Height, stiEAN13BarCodeType.SupplementType, stiEAN13BarCodeType.SupplementCode, stiEAN13BarCodeType.ShowQuietZoneIndicator };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
