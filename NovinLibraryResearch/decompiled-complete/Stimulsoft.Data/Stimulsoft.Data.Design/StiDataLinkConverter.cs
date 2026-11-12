using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Data.Design;

public class StiDataLinkConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string)
		{
			string key = value as string;
			return new StiDataLink(key);
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (value is StiDataLink stiDataLink)
		{
			if (destinationType == typeof(InstanceDescriptor) && value != null)
			{
				Type[] types = new Type[1] { typeof(string) };
				ConstructorInfo constructor = typeof(StiDataSortRule).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[1] { stiDataLink.Key };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (destinationType == typeof(string))
			{
				return stiDataLink.Key;
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

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
