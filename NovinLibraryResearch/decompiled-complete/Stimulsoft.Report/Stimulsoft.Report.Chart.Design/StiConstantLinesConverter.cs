using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart.Design;

public class StiConstantLinesConverter : StiUniversalConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (!(value is StiConstantLinesCollection { Count: not 0 }))
			{
				return string.Format("[{0}]", Loc.GetMain("NoElements"));
			}
			return string.Format("({0})", Loc.GetMain("ConstantLines"));
		}
		return ((StiUniversalConverter)this).ConvertTo(context, culture, value, destinationType);
	}
}
