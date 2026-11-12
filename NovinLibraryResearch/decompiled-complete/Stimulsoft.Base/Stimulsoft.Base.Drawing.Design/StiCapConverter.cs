using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiCapConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
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

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (value == null)
			{
				return "null";
			}
			StiCap stiCap = value as StiCap;
			return StiLocalization.Get("PropertyEnum", typeof(StiCapStyle).Name + Enum.GetName(typeof(StiCapStyle), stiCap.Style), throwError: false);
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiCap stiCap2 = (StiCap)value;
			Type[] types = new Type[5]
			{
				typeof(int),
				typeof(StiCapStyle),
				typeof(int),
				typeof(bool),
				typeof(Color)
			};
			ConstructorInfo constructor = typeof(StiCap).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiCap2.Width, stiCap2.Style, stiCap2.Height, stiCap2.Fill, stiCap2.Color };
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
