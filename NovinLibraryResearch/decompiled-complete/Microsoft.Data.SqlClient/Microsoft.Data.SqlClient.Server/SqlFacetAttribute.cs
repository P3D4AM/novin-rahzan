using System;

namespace Microsoft.Data.SqlClient.Server;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = false)]
public class SqlFacetAttribute : Attribute
{
	public bool IsFixedLength { get; set; }

	public int MaxSize { get; set; }

	public int Precision { get; set; }

	public int Scale { get; set; }

	public bool IsNullable { get; set; }
}
