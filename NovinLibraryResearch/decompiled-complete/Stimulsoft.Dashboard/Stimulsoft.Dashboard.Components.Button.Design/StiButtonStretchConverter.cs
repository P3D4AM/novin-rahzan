using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Button.Design;

public class StiButtonStretchConverter : TypeConverter
{
	private string LocStretchXY => Loc.GetEnum("StiDashboardContentAlignmentStretchXY");

	private string LocStretchX => Loc.GetEnum("StiDashboardContentAlignmentStretchX");

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
					return "Stretch XY";
				}
				return LocStretchXY;
			}
			switch (stiButtonElement.Stretch)
			{
			case StiButtonStretch.StretchXY:
				if (!StiPropertyGridOptions.Localizable)
				{
					return "Stretch XY";
				}
				return LocStretchXY;
			case StiButtonStretch.StretchX:
				if (!StiPropertyGridOptions.Localizable)
				{
					return "Stretch X";
				}
				return LocStretchX;
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
