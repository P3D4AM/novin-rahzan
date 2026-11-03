using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DevComponents.DotNetBar.Rendering;

public class SuperTabControlBoxStateColorTableConvertor : ExpandableObjectConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if ((object)destinationType == typeof(string) && value is SuperTabControlBoxStateColorTable superTabControlBoxStateColorTable)
		{
			ColorConverter colorConverter = new ColorConverter();
			if (!superTabControlBoxStateColorTable.Background.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabControlBoxStateColorTable.Background);
			}
			if (!superTabControlBoxStateColorTable.Border.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabControlBoxStateColorTable.Border);
			}
			if (!superTabControlBoxStateColorTable.Image.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabControlBoxStateColorTable.Image);
			}
			return string.Empty;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
