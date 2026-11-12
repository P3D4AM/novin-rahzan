using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

internal class SqlLocatorAccessPointComparer : IEqualityComparer<ISqlObjectLocator>
{
	public bool Equals(ISqlObjectLocator x, ISqlObjectLocator y)
	{
		return x?.Connection?.DataSource == y?.Connection?.DataSource;
	}

	public int GetHashCode(ISqlObjectLocator obj)
	{
		return (obj.Connection?.DataSource?.GetHashCode()).GetValueOrDefault();
	}
}
