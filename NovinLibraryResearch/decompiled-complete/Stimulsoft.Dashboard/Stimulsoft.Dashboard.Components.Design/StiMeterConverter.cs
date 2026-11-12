using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Design;

public class StiMeterConverter : TypeConverter
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
		if (destinationType == typeof(string))
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
		if (destinationType == typeof(string))
		{
			if (value != null)
			{
				return "(" + Loc.Get("PropertyMain", "Value") + ")";
			}
			return "(" + Loc.Get("Report", "NotAssigned") + ")";
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiMeter stiMeter = value as StiMeter;
			Type[] types = new Type[3]
			{
				typeof(string),
				typeof(string),
				typeof(string)
			};
			object[] objs = new object[3] { stiMeter.Key, stiMeter.Expression, stiMeter.Label };
			ConstructorInfo constructor = typeof(StiMeter).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
