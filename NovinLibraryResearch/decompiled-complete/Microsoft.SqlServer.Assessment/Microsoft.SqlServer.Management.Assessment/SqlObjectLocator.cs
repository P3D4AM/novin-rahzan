using System;
using System.Data.Common;
using Microsoft.SqlServer.Management.Assessment.Configuration;

namespace Microsoft.SqlServer.Management.Assessment;

public sealed class SqlObjectLocator : ISqlObjectLocator, IDisposable
{
	public string Name { get; set; }

	public SqlObjectType Type { get; set; }

	public Version Version { get; set; }

	public string Urn { get; set; }

	public SqlEngineEdition EngineEdition { get; set; }

	public string ServerName { get; set; }

	public DbConnection Connection { get; set; }

	public string Platform { get; set; }

	public string MachineType { get; set; }

	public void Dispose()
	{
		Connection?.Dispose();
	}

	public override string ToString()
	{
		return Urn;
	}
}
