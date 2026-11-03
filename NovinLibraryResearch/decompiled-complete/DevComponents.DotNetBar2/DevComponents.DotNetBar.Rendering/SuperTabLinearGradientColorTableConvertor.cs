using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering;

public class SuperTabLinearGradientColorTableConvertor : ExpandableObjectConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if ((object)destinationType == typeof(string) && value is SuperTabLinearGradientColorTable superTabLinearGradientColorTable)
		{
			ColorConverter colorConverter = new ColorConverter();
			if (superTabLinearGradientColorTable.Colors != null)
			{
				if (superTabLinearGradientColorTable.Colors[0] != Color.Empty)
				{
					return colorConverter.ConvertToString(superTabLinearGradientColorTable.Colors[0]);
				}
				if (superTabLinearGradientColorTable.Colors.Length > 1 && superTabLinearGradientColorTable.Colors[1] != Color.Empty)
				{
					return colorConverter.ConvertToString(superTabLinearGradientColorTable.Colors[1]);
				}
			}
			if (superTabLinearGradientColorTable.GradientAngle != 90)
			{
				return superTabLinearGradientColorTable.GradientAngle.ToString();
			}
			return string.Empty;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
