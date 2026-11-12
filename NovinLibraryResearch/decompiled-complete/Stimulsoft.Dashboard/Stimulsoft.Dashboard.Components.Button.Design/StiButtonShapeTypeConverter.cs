using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Button.Design;

public class StiButtonShapeTypeConverter : TypeConverter
{
	private string LocRectangle => Loc.Get("Components", "StiRectanglePrimitive");

	private string LocCircle => Loc.GetEnum("StiArrowStyleCircle");

	private string LocButton => Loc.Get("Components", "StiButton");

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			StiButtonElement stiButtonElement = context.Instance as StiButtonElement;
			if (context.Instance is Array)
			{
				stiButtonElement = (context.Instance as Array)?.GetValue(0) as StiButtonElement;
			}
			if (stiButtonElement == null)
			{
				if (!StiPropertyGridOptions.Localizable)
				{
					return "Button";
				}
				return LocButton;
			}
			switch (stiButtonElement.ShapeType)
			{
			case StiButtonShapeType.Circle:
				if (!StiPropertyGridOptions.Localizable)
				{
					return "Circle";
				}
				return LocCircle;
			case StiButtonShapeType.Rectangle:
				if (!StiPropertyGridOptions.Localizable)
				{
					return "Rectangle";
				}
				return LocRectangle;
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
