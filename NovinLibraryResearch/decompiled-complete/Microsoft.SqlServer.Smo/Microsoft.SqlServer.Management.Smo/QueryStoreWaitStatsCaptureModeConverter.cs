using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.SqlServer.Management.Smo;

public class QueryStoreWaitStatsCaptureModeConverter : EnumToDisplayNameConverter
{
	public QueryStoreWaitStatsCaptureModeConverter()
		: base(typeof(QueryStoreWaitStatsCaptureMode))
	{
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string) && value == null)
		{
			return string.Empty;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
