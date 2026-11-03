using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.TextFormats.Design;

public class StiCurrencyFormatConverter : TypeConverter
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
			StiCurrencyFormatService stiCurrencyFormatService = (StiCurrencyFormatService)value;
			bool flag = stiCurrencyFormatService.UseLocalSetting && stiCurrencyFormatService.State != StiTextFormatState.None;
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
			ConstructorInfo constructor = typeof(StiCurrencyFormatService).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = (flag ? new object[12]
				{
					stiCurrencyFormatService.PositivePattern, stiCurrencyFormatService.NegativePattern, 0, stiCurrencyFormatService.DecimalSeparator, stiCurrencyFormatService.DecimalDigits, stiCurrencyFormatService.GroupSeparator, stiCurrencyFormatService.GroupSize, stiCurrencyFormatService.Symbol, stiCurrencyFormatService.UseGroupSeparator, stiCurrencyFormatService.UseLocalSetting,
					stiCurrencyFormatService.NullDisplay, stiCurrencyFormatService.State
				} : new object[10] { stiCurrencyFormatService.PositivePattern, stiCurrencyFormatService.NegativePattern, stiCurrencyFormatService.DecimalSeparator, stiCurrencyFormatService.DecimalDigits, stiCurrencyFormatService.GroupSeparator, stiCurrencyFormatService.GroupSize, stiCurrencyFormatService.Symbol, stiCurrencyFormatService.UseGroupSeparator, stiCurrencyFormatService.UseLocalSetting, stiCurrencyFormatService.NullDisplay });
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
