using System;
using System.ComponentModel;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlExternalStreamParam
{
	public SqlExternalStreamParamType Type { get; }

	public SqlIdentifier Value { get; }

	internal SqlExternalStreamParam(SqlExternalStreamParamType type, SqlIdentifier value)
	{
		Type = type;
		Value = value;
	}

	public static string ToString(SqlExternalStreamParamType param)
	{
		return ((DescriptionAttribute)Attribute.GetCustomAttribute(typeof(SqlExternalStreamParamType).GetField(param.ToString()), typeof(DescriptionAttribute))).Description;
	}
}
