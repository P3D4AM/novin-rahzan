using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.Design;

public class StiMarginsTypeConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string text = ((string)value).Trim();
			char c = culture?.TextInfo.ListSeparator[0] ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
			if (text.IndexOf(c) != -1)
			{
				string[] array = text.Split(new char[1] { c });
				return new StiMargins(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]), double.Parse(array[3]));
			}
			string[] array2 = text.Split(new char[1] { ',' });
			if (array2.Length == 1)
			{
				return new StiMargins(double.Parse(array2[0]));
			}
			return new StiMargins(double.Parse(array2[0]), double.Parse(array2[1]), double.Parse(array2[2]), double.Parse(array2[3]));
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		StiMargins stiMargins = (StiMargins)value;
		if (destinationType == typeof(string))
		{
			char c = culture?.TextInfo.ListSeparator[0] ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
			return $"{stiMargins.Left}{c}{stiMargins.Right}{c}{stiMargins.Top}{c}{stiMargins.Bottom}";
		}
		if (destinationType == typeof(InstanceDescriptor))
		{
			Type[] types = new Type[4]
			{
				typeof(double),
				typeof(double),
				typeof(double),
				typeof(double)
			};
			ConstructorInfo constructor = typeof(StiMargins).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiMargins.Left, stiMargins.Right, stiMargins.Top, stiMargins.Bottom };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}
}
