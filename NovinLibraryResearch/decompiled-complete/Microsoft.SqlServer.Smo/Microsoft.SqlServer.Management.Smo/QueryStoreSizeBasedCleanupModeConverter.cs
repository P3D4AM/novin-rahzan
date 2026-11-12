using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.SqlServer.Management.Smo;

public class QueryStoreSizeBasedCleanupModeConverter : EnumToDisplayNameConverter
{
	public QueryStoreSizeBasedCleanupModeConverter()
		: base(typeof(QueryStoreSizeBasedCleanupMode))
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
