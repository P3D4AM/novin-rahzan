using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class SuperTabItemStateColorTableConvertor : ExpandableObjectConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string) && value is SuperTabItemStateColorTable superTabItemStateColorTable)
		{
			ColorConverter colorConverter = new ColorConverter();
			if (superTabItemStateColorTable.Background.Colors != null)
			{
				return colorConverter.ConvertToString(superTabItemStateColorTable.Background.Colors[0]);
			}
			if (!superTabItemStateColorTable.InnerBorder.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabItemStateColorTable.InnerBorder);
			}
			if (!superTabItemStateColorTable.OuterBorder.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabItemStateColorTable.OuterBorder);
			}
			if (!superTabItemStateColorTable.Text.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabItemStateColorTable.Text);
			}
			return string.Empty;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
