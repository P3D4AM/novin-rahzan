using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiSimpleBorderConverter : TypeConverter
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
		if (destinationType == typeof(string) && value == null && StiVisualStatePermissionAttribute.IsFromDefaultStateAllowed(context))
		{
			return Loc.GetReport("FromDefaultState");
		}
		if (destinationType == typeof(string) && value is StiSimpleBorder)
		{
			StiEnumConverter stiEnumConverter = new StiEnumConverter(typeof(StiBorderSides));
			return stiEnumConverter.ConvertTo(context, culture, ((StiSimpleBorder)value).Side, typeof(string)) as string;
		}
		if (destinationType == typeof(InstanceDescriptor) && value is StiSimpleBorder)
		{
			StiSimpleBorder stiSimpleBorder = (StiSimpleBorder)value;
			Type[] types = new Type[4]
			{
				typeof(StiBorderSides),
				typeof(Color),
				typeof(double),
				typeof(StiPenStyle)
			};
			ConstructorInfo constructor = typeof(StiSimpleBorder).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiSimpleBorder.Side, stiSimpleBorder.Color, stiSimpleBorder.Size, stiSimpleBorder.Style };
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
