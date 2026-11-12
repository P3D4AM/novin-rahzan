using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiBorderSideConverter : TypeConverter
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
		if (destinationType == typeof(string) && value is StiBorderSide)
		{
			StiBorderSide stiBorderSide = value as StiBorderSide;
			if (stiBorderSide.side == StiBorderSides.Top)
			{
				return "(" + Loc.Get("PropertyMain", "TopSide") + ")";
			}
			if (stiBorderSide.side == StiBorderSides.Bottom)
			{
				return "(" + Loc.Get("PropertyMain", "BottomSide") + ")";
			}
			if (stiBorderSide.side == StiBorderSides.Left)
			{
				return "(" + Loc.Get("PropertyMain", "LeftSide") + ")";
			}
			if (stiBorderSide.side == StiBorderSides.Right)
			{
				return "(" + Loc.Get("PropertyMain", "RightSide") + ")";
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
