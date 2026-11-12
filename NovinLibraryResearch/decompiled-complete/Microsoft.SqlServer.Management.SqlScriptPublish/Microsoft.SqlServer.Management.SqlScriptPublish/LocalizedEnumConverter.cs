using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

internal class LocalizedEnumConverter : EnumConverter
{
	private const string srTypeFullName = "Microsoft.SqlServer.Management.SqlScriptPublish.SR";

	public LocalizedEnumConverter(Type type)
		: base(type)
	{
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (value != null && destinationType == typeof(string))
		{
			return Type.GetType("Microsoft.SqlServer.Management.SqlScriptPublish.SR").GetProperty(value.ToString()).GetValue(null, null);
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
