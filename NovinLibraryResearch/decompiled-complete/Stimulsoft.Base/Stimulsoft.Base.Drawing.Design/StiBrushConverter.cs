using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiBrushConverter : StiOrderConverter
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
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		if (destinationType == typeof(string))
		{
			if (value == null)
			{
				return "null";
			}
			if (value is StiDefaultBrush)
			{
				return Loc.GetReport("FromDefaultState");
			}
			if (value is StiStyleBrush)
			{
				return Loc.Get("FormStyleDesigner", "FromStyle");
			}
			if (value is StiEmptyBrush)
			{
				return Loc.Get("Report", "StiEmptyBrush");
			}
			if (value is StiSolidBrush)
			{
				return Loc.Get("Report", "StiSolidBrush");
			}
			if (value is StiGradientBrush)
			{
				return Loc.Get("Report", "StiGradientBrush");
			}
			if (value is StiHatchBrush)
			{
				return Loc.Get("Report", "StiHatchBrush");
			}
			if (value is StiGlareBrush)
			{
				return Loc.Get("Report", "StiGlareBrush");
			}
			if (value is StiGlassBrush)
			{
				return Loc.Get("Report", "StiGlassBrush");
			}
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			if (value is StiEmptyBrush)
			{
				Type[] types = new Type[0];
				ConstructorInfo constructor = typeof(StiEmptyBrush).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[0];
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (value is StiGradientBrush)
			{
				StiGradientBrush stiGradientBrush = (StiGradientBrush)value;
				Type[] types = new Type[3]
				{
					typeof(Color),
					typeof(Color),
					typeof(double)
				};
				ConstructorInfo constructor = typeof(StiGradientBrush).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[3] { stiGradientBrush.StartColor, stiGradientBrush.EndColor, stiGradientBrush.Angle };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (value is StiHatchBrush)
			{
				StiHatchBrush stiHatchBrush = (StiHatchBrush)value;
				Type[] types = new Type[3]
				{
					typeof(HatchStyle),
					typeof(Color),
					typeof(Color)
				};
				ConstructorInfo constructor = typeof(StiHatchBrush).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[3] { stiHatchBrush.Style, stiHatchBrush.ForeColor, stiHatchBrush.BackColor };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (value is StiSolidBrush)
			{
				StiSolidBrush stiSolidBrush = (StiSolidBrush)value;
				Type[] types = new Type[1] { typeof(Color) };
				ConstructorInfo constructor = typeof(StiSolidBrush).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[1] { stiSolidBrush.Color };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (value is StiGlareBrush)
			{
				StiGlareBrush stiGlareBrush = (StiGlareBrush)value;
				Type[] types = new Type[5]
				{
					typeof(Color),
					typeof(Color),
					typeof(double),
					typeof(float),
					typeof(float)
				};
				ConstructorInfo constructor = typeof(StiGlareBrush).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[5] { stiGlareBrush.StartColor, stiGlareBrush.EndColor, stiGlareBrush.Angle, stiGlareBrush.Focus, stiGlareBrush.Scale };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (value is StiGlassBrush)
			{
				StiGlassBrush stiGlassBrush = (StiGlassBrush)value;
				Type[] types = new Type[3]
				{
					typeof(Color),
					typeof(bool),
					typeof(float)
				};
				ConstructorInfo constructor = typeof(StiGlassBrush).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[3] { stiGlassBrush.Color, stiGlassBrush.DrawHatch, stiGlassBrush.Blend };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (value is StiDefaultBrush)
			{
				ConstructorInfo constructor = typeof(StiDefaultBrush).GetConstructor(new Type[0]);
				if (constructor != null)
				{
					return CreateNewInstanceDescriptor(constructor, new object[0]);
				}
			}
			else if (value is StiStyleBrush)
			{
				ConstructorInfo constructor = typeof(StiStyleBrush).GetConstructor(new Type[0]);
				if (constructor != null)
				{
					return CreateNewInstanceDescriptor(constructor, new object[0]);
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
