using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace Stimulsoft.Report.Design;

public class StiGlobalizationItemConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is StiGlobalizationItem)
			{
				StiGlobalizationItem stiGlobalizationItem = (StiGlobalizationItem)value;
				Type[] types = new Type[2]
				{
					typeof(string),
					typeof(string)
				};
				ConstructorInfo constructor = typeof(StiGlobalizationItem).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[2] { stiGlobalizationItem.PropertyName, stiGlobalizationItem.Text };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
		}
		else if (destinationType == typeof(string))
		{
			StiGlobalizationItem stiGlobalizationItem2 = (StiGlobalizationItem)value;
			if (stiGlobalizationItem2 != null)
			{
				char c = culture?.TextInfo.ListSeparator[0] ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
				return $"{stiGlobalizationItem2.PropertyName}{c}{XmlConvert.EncodeName(stiGlobalizationItem2.Text)}";
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
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string text = value as string;
			char c = culture?.TextInfo.ListSeparator[0] ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
			char[] separator = new char[1] { c };
			string[] array = text.Split(separator);
			return new StiGlobalizationItem(array[0], XmlConvert.DecodeName(array[1]));
		}
		return base.ConvertFrom(context, culture, value);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
