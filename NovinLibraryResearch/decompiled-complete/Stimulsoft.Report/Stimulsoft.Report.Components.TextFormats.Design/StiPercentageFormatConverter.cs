using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.TextFormats.Design;

public class StiPercentageFormatConverter : TypeConverter
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
			StiPercentageFormatService stiPercentageFormatService = (StiPercentageFormatService)value;
			bool flag = stiPercentageFormatService.UseLocalSetting && stiPercentageFormatService.State != StiTextFormatState.None;
			Type[] types = (flag ? new Type[12]
			{
				typeof(int),
				typeof(int),
				typeof(int),
				typeof(string),
				typeof(int),
				typeof(string),
				typeof(int),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(string),
				typeof(StiTextFormatState)
			} : new Type[10]
			{
				typeof(int),
				typeof(int),
				typeof(string),
				typeof(int),
				typeof(string),
				typeof(int),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(string)
			});
			ConstructorInfo constructor = typeof(StiPercentageFormatService).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = (flag ? new object[12]
				{
					stiPercentageFormatService.PositivePattern, stiPercentageFormatService.NegativePattern, 0, stiPercentageFormatService.DecimalSeparator, stiPercentageFormatService.DecimalDigits, stiPercentageFormatService.GroupSeparator, stiPercentageFormatService.GroupSize, stiPercentageFormatService.Symbol, stiPercentageFormatService.UseGroupSeparator, stiPercentageFormatService.UseLocalSetting,
					stiPercentageFormatService.NullDisplay, stiPercentageFormatService.State
				} : new object[10] { stiPercentageFormatService.PositivePattern, stiPercentageFormatService.NegativePattern, stiPercentageFormatService.DecimalSeparator, stiPercentageFormatService.DecimalDigits, stiPercentageFormatService.GroupSeparator, stiPercentageFormatService.GroupSize, stiPercentageFormatService.Symbol, stiPercentageFormatService.UseGroupSeparator, stiPercentageFormatService.UseLocalSetting, stiPercentageFormatService.NullDisplay });
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
