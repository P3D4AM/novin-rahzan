using System;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.DotNetBar.Design;

public class ColumnNamesConverter : TypeConverter
{
	private const string tempCommaReplacement = "```";

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if ((object)sourceType != typeof(string))
		{
			return base.CanConvertFrom(context, sourceType);
		}
		return true;
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value != null && value is string)
		{
			string text = ((string)value).Replace(",,", "```");
			text = text.Replace(",", "\r\n");
			return text.Replace("```", ",");
		}
		return value;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if ((object)destinationType != typeof(string) || !(value is string))
		{
			return base.ConvertTo(context, culture, value, destinationType);
		}
		if (value != null)
		{
			string text = ((string)value).Replace(",", "```");
			text = text.Replace("\r\n", ",");
			return text.Replace("```", ",");
		}
		return "";
	}
}
