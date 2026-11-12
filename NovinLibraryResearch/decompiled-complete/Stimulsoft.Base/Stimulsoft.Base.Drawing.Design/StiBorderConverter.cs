using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiBorderConverter : TypeConverter
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
		if (destinationType == typeof(string) && value is StiBorder)
		{
			StiEnumConverter stiEnumConverter = new StiEnumConverter(typeof(StiBorderSides));
			return stiEnumConverter.ConvertTo(context, culture, ((StiBorder)value).Side, typeof(string)) as string;
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiAdvancedBorder stiAdvancedBorder = value as StiAdvancedBorder;
			if (stiAdvancedBorder != null)
			{
				Type[] types = new Type[16]
				{
					typeof(Color),
					typeof(double),
					typeof(StiPenStyle),
					typeof(Color),
					typeof(double),
					typeof(StiPenStyle),
					typeof(Color),
					typeof(double),
					typeof(StiPenStyle),
					typeof(Color),
					typeof(double),
					typeof(StiPenStyle),
					typeof(bool),
					typeof(double),
					typeof(StiBrush),
					typeof(bool)
				};
				ConstructorInfo constructor = typeof(StiAdvancedBorder).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[16]
					{
						stiAdvancedBorder.TopSide.Color,
						stiAdvancedBorder.TopSide.Size,
						stiAdvancedBorder.TopSide.Style,
						stiAdvancedBorder.BottomSide.Color,
						stiAdvancedBorder.BottomSide.Size,
						stiAdvancedBorder.BottomSide.Style,
						stiAdvancedBorder.LeftSide.Color,
						stiAdvancedBorder.LeftSide.Size,
						stiAdvancedBorder.LeftSide.Style,
						stiAdvancedBorder.RightSide.Color,
						stiAdvancedBorder.RightSide.Size,
						stiAdvancedBorder.RightSide.Style,
						stiAdvancedBorder.DropShadow,
						stiAdvancedBorder.ShadowSize,
						stiAdvancedBorder.ShadowBrush,
						stiAdvancedBorder.Topmost
					};
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (value is StiBorder)
			{
				StiBorder stiBorder = (StiBorder)value;
				Type[] types2 = new Type[8]
				{
					typeof(StiBorderSides),
					typeof(Color),
					typeof(double),
					typeof(StiPenStyle),
					typeof(bool),
					typeof(double),
					typeof(StiBrush),
					typeof(bool)
				};
				ConstructorInfo constructor2 = typeof(StiBorder).GetConstructor(types2);
				if (constructor2 != null)
				{
					object[] objs2 = new object[8] { stiBorder.Side, stiBorder.Color, stiBorder.Size, stiBorder.Style, stiBorder.DropShadow, stiBorder.ShadowSize, stiBorder.ShadowBrush, stiBorder.Topmost };
					return CreateNewInstanceDescriptor(constructor2, objs2);
				}
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
