using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.BarCodes.Design;

public class StiMaxicodeBarCodeTypeConverter : StiBarCodeTypeServiceConverter
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
			StiMaxicodeBarCodeType stiMaxicodeBarCodeType = (StiMaxicodeBarCodeType)value;
			Type[] types = new Type[5]
			{
				typeof(StiMaxicodeMode),
				typeof(int),
				typeof(int),
				typeof(bool),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiMaxicodeBarCodeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiMaxicodeBarCodeType.Mode, stiMaxicodeBarCodeType.StructuredAppendPosition, stiMaxicodeBarCodeType.StructuredAppendTotal, stiMaxicodeBarCodeType.ProcessTilde, stiMaxicodeBarCodeType.TrimExcessData };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
