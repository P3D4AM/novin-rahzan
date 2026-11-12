using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class SuperTabPanelItemColorTableConvertor : ExpandableObjectConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string) && value is SuperTabPanelItemColorTable superTabPanelItemColorTable)
		{
			ColorConverter colorConverter = new ColorConverter();
			if (superTabPanelItemColorTable.Background.Colors != null)
			{
				return colorConverter.ConvertToString(superTabPanelItemColorTable.Background.Colors[0]);
			}
			if (!superTabPanelItemColorTable.InnerBorder.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabPanelItemColorTable.InnerBorder);
			}
			if (!superTabPanelItemColorTable.OuterBorder.IsEmpty)
			{
				return colorConverter.ConvertToString(superTabPanelItemColorTable.OuterBorder);
			}
			return string.Empty;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
