using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Button.Design;

public class StiButtonTypeConverter : TypeConverter
{
	private string LocButton => Loc.Get("Dialogs", "StiButtonControl");

	private string LocRadioButton => Loc.Get("Dialogs", "StiRadioButtonControl");

	private string LocCheckBox => Loc.Get("Dialogs", "StiCheckBoxControl");

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
			switch (stiButtonElement.Type)
			{
			case StiButtonType.Button:
				if (!StiPropertyGridOptions.Localizable)
				{
					return "Button";
				}
				return LocButton;
			case StiButtonType.RadioButton:
				if (!StiPropertyGridOptions.Localizable)
				{
					return "Radio Button";
				}
				return LocRadioButton;
			case StiButtonType.CheckBox:
				if (!StiPropertyGridOptions.Localizable)
				{
					return "Check Box";
				}
				return LocCheckBox;
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
