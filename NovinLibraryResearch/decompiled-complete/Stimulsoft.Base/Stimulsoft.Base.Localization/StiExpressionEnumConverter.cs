using System;
using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Base.Localization;

public class StiExpressionEnumConverter : StiEnumConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			string expressionFromInstance = StiAppExpressionHelper.GetExpressionFromInstance(context);
			if (!string.IsNullOrWhiteSpace(expressionFromInstance))
			{
				return expressionFromInstance;
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string && StiAppExpressionHelper.IsExpressionSpecified(context.Instance, context.PropertyDescriptor.Name))
		{
			StiAppExpressionHelper.SetExpression(context.Instance, context.PropertyDescriptor.Name, value as string);
			return context.PropertyDescriptor.GetValue(context.Instance);
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return !StiAppExpressionHelper.IsExpressionSpecified(context.Instance, context.PropertyDescriptor.Name);
	}

	public StiExpressionEnumConverter(Type type)
		: base(type)
	{
	}

	public StiExpressionEnumConverter()
	{
	}
}
