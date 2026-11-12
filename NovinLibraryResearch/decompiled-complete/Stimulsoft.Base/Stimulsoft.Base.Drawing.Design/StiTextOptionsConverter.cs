using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Base.Drawing.Design;

public class StiTextOptionsConverter : TypeConverter
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
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		if (destinationType == typeof(string))
		{
			if (value == null)
			{
				return "null";
			}
			char separator = culture?.TextInfo.ListSeparator[0] ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
			StiTextOptions op = (StiTextOptions)value;
			return StiTextOptionsHelper.ConvertTextOptionsToLocalizedString(op, separator, wordWrap: false);
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiTextOptions stiTextOptions = (StiTextOptions)value;
			if (stiTextOptions.FirstTabOffset == 40f && stiTextOptions.DistanceBetweenTabs == 20f)
			{
				Type[] types = new Type[6]
				{
					typeof(bool),
					typeof(bool),
					typeof(bool),
					typeof(float),
					typeof(HotkeyPrefix),
					typeof(StringTrimming)
				};
				ConstructorInfo constructor = typeof(StiTextOptions).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[6] { stiTextOptions.RightToLeft, stiTextOptions.LineLimit, stiTextOptions.WordWrap, stiTextOptions.Angle, stiTextOptions.HotkeyPrefix, stiTextOptions.Trimming };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else
			{
				Type[] types2 = new Type[8]
				{
					typeof(bool),
					typeof(bool),
					typeof(bool),
					typeof(float),
					typeof(HotkeyPrefix),
					typeof(StringTrimming),
					typeof(float),
					typeof(float)
				};
				ConstructorInfo constructor2 = typeof(StiTextOptions).GetConstructor(types2);
				if (constructor2 != null)
				{
					object[] objs2 = new object[8] { stiTextOptions.RightToLeft, stiTextOptions.LineLimit, stiTextOptions.WordWrap, stiTextOptions.Angle, stiTextOptions.HotkeyPrefix, stiTextOptions.Trimming, stiTextOptions.FirstTabOffset, stiTextOptions.DistanceBetweenTabs };
					return CreateNewInstanceDescriptor(constructor2, objs2);
				}
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

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
		if (destinationType == typeof(string))
		{
			return true;
		}
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string str)
		{
			char separator = culture?.TextInfo.ListSeparator[0] ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
			return StiTextOptionsHelper.ConvertStringToTextOptions(str, separator);
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
