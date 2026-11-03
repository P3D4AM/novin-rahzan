using System;
using System.ComponentModel;
using System.Globalization;

namespace DevComponents.DotNetBar;

public class PaddingConvertor : ExpandableObjectConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if ((object)destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if ((object)destinationType == typeof(string) && value is Padding padding)
		{
			return $"{padding.Bottom:D}, {padding.Left:D}, {padding.Right:D}, {padding.Top:D}";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if ((object)sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string[] array = ((string)value).Split(new char[1] { ',' });
			if (array.Length != 4)
			{
				throw new ArgumentException("Invalid value to convert.");
			}
			try
			{
				int bottom = int.Parse(array[0]);
				int left = int.Parse(array[1]);
				int right = int.Parse(array[2]);
				int top = int.Parse(array[3]);
				return new Padding(left, right, top, bottom);
			}
			catch (Exception)
			{
				throw new ArgumentException("Invalid value to convert.");
			}
		}
		return base.ConvertFrom(context, culture, value);
	}
}
