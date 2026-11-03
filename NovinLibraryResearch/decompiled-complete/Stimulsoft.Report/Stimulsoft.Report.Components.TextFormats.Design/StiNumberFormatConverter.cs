using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.TextFormats.Design;

public class StiNumberFormatConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiNumberFormatService stiNumberFormatService = (StiNumberFormatService)value;
			bool flag = stiNumberFormatService.UseLocalSetting && stiNumberFormatService.State != StiTextFormatState.None;
			Type[] types = (flag ? new Type[10]
			{
				typeof(int),
				typeof(int),
				typeof(string),
				typeof(int),
				typeof(string),
				typeof(int),
				typeof(bool),
				typeof(bool),
				typeof(string),
				typeof(StiTextFormatState)
			} : new Type[8]
			{
				typeof(int),
				typeof(string),
				typeof(int),
				typeof(string),
				typeof(int),
				typeof(bool),
				typeof(bool),
				typeof(string)
			});
			ConstructorInfo constructor = typeof(StiNumberFormatService).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = (flag ? new object[10] { stiNumberFormatService.NegativePattern, 0, stiNumberFormatService.DecimalSeparator, stiNumberFormatService.DecimalDigits, stiNumberFormatService.GroupSeparator, stiNumberFormatService.GroupSize, stiNumberFormatService.UseGroupSeparator, stiNumberFormatService.UseLocalSetting, stiNumberFormatService.NullDisplay, stiNumberFormatService.State } : new object[8] { stiNumberFormatService.NegativePattern, stiNumberFormatService.DecimalSeparator, stiNumberFormatService.DecimalDigits, stiNumberFormatService.GroupSeparator, stiNumberFormatService.GroupSize, stiNumberFormatService.UseGroupSeparator, stiNumberFormatService.UseLocalSetting, stiNumberFormatService.NullDisplay });
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
