using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiCsvSourceConverter : TypeConverter
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
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiCsvSource stiCsvSource = (StiCsvSource)value;
			Type[] types = new Type[6]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(int),
				typeof(string),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiCsvSource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[6] { stiCsvSource.Path, stiCsvSource.Name, stiCsvSource.Alias, stiCsvSource.CodePage, stiCsvSource.Separator, stiCsvSource.Key };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
