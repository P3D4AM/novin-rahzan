using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Schedule;

public class ColorDefConvertor : ExpandableObjectConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if ((object)destinationType == typeof(string))
		{
			if (value is ColorDef colorDef)
			{
				ColorConverter colorConverter = new ColorConverter();
				if (colorDef.Colors != null)
				{
					if (colorDef.Colors[0] != Color.Empty)
					{
						return colorConverter.ConvertToString(colorDef.Colors[0]);
					}
					if (colorDef.Colors.Length > 1 && colorDef.Colors[1] != Color.Empty)
					{
						return colorConverter.ConvertToString(colorDef.Colors[1]);
					}
				}
				if (colorDef.Angle != 0f)
				{
					return colorDef.Angle.ToString();
				}
			}
			return string.Empty;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
