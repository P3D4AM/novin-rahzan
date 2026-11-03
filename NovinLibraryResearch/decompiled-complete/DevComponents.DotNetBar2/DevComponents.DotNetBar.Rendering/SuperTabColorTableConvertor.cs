using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering;

public class SuperTabColorTableConvertor : ExpandableObjectConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if ((object)destinationType == typeof(string) && value is SuperTabColorTable superTabColorTable)
		{
			ColorConverter colorConverter = new ColorConverter();
			if (superTabColorTable.Background.Colors != null)
			{
				return colorConverter.ConvertToString(superTabColorTable.Background.Colors);
			}
			if (!superTabColorTable.InnerBorder.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabColorTable.InnerBorder);
			}
			if (!superTabColorTable.OuterBorder.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabColorTable.OuterBorder);
			}
			return string.Empty;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
