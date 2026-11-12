using System;
using System.Data.Common;
using Microsoft.SqlServer.Management.Assessment.Configuration;

namespace Microsoft.SqlServer.Management.Assessment;

public interface ISqlObjectLocator : IDisposable
{
	string Platform { get; }

	DbConnection Connection { get; }

	string Name { get; }

	SqlObjectType Type { get; }

	Version Version { get; }

	string Urn { get; }

	SqlEngineEdition EngineEdition { get; }

	string ServerName { get; }

	string MachineType { get; }
}
