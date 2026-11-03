using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiWatermarkConverter : StiUniversalConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (value == null || string.IsNullOrEmpty(((StiWatermark)value).Text))
			{
				return "(" + Loc.Get("PropertyMain", "Watermark") + ")";
			}
			return ((StiWatermark)value).Text;
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiWatermark stiWatermark = (StiWatermark)value;
			Type[] types = new Type[8]
			{
				typeof(StiBrush),
				typeof(string),
				typeof(float),
				typeof(Font),
				typeof(bool),
				typeof(bool),
				typeof(bool),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiWatermark).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8] { stiWatermark.TextBrush, stiWatermark.Text, stiWatermark.Angle, stiWatermark.Font, stiWatermark.ShowBehind, stiWatermark.Enabled, stiWatermark.AspectRatio, stiWatermark.RightToLeft };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return ((StiUniversalConverter)this).ConvertTo(context, culture, value, destinationType);
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
		return ((StiUniversalConverter)this).CanConvertTo(context, destinationType);
	}

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
