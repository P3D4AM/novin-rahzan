using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiSimpleShadowConverter : TypeConverter
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
		if (destinationType == typeof(string) && value is StiSimpleShadow)
		{
			return "(" + Loc.GetMain("Shadow") + ")";
		}
		if (destinationType == typeof(InstanceDescriptor) && value is StiSimpleShadow)
		{
			StiSimpleShadow stiSimpleShadow = (StiSimpleShadow)value;
			Type[] types = new Type[4]
			{
				typeof(Color),
				typeof(Point),
				typeof(int),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiSimpleShadow).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiSimpleShadow.Color, stiSimpleShadow.Location, stiSimpleShadow.Size, stiSimpleShadow.Visible };
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
