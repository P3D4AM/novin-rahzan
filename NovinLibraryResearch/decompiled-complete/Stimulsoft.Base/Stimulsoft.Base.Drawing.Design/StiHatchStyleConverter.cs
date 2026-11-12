using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design;

public class StiHatchStyleConverter : ColorConverter
{
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

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (destinationType == typeof(string))
		{
			HatchStyle val = (HatchStyle)value;
			return StiLocalization.Get("PropertyHatchStyle", ((object)(HatchStyle)(ref val)).ToString());
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
