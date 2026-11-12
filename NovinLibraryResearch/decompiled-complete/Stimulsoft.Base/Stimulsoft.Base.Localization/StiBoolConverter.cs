using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Base.Localization;

public class StiBoolConverter : TypeConverter
{
	private static StandardValuesCollection values;

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, destinationType);
	}

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (value is int num && num != 0)
			{
				string text = null;
				if (StiPropertyGridOptions.Localizable)
				{
					text = StiLocalization.Get("PropertyEnum", "boolTrue");
				}
				return text ?? "True";
			}
			string text2 = null;
			if (StiPropertyGridOptions.Localizable)
			{
				text2 = StiLocalization.Get("PropertyEnum", "boolFalse");
			}
			return text2 ?? "False";
		}
		return ConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string text = ((string)value).Trim();
			try
			{
				string text2 = null;
				if (StiPropertyGridOptions.Localizable)
				{
					text2 = StiLocalization.Get("PropertyEnum", "boolTrue");
				}
				if (text2 == null)
				{
					text2 = "True";
				}
				return text == text2;
			}
			catch
			{
				throw new Exception("ConvertInvalidPrimitive");
			}
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
	{
		return values ?? (values = new StandardValuesCollection(new object[2] { true, false }));
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
	{
		return true;
	}

	public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
