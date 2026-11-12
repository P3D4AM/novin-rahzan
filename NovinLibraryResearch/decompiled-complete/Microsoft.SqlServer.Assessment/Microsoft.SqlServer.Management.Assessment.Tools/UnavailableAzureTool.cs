using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class UnavailableAzureTool : IAzureTool, ITool
{
	public ICharacteristicCollection GetDependencies(RunSpace runSpace)
	{
		return CharacteristicCollection.Empty;
	}

	public Task<bool> IsAvailable(RunSpace runSpace)
	{
		return Task.FromResult(result: false);
	}

	public Task<IDataView> Query(string kustoQuery)
	{
		throw new InvalidOperationException("UnavailableAzureTool.Query should not be called.");
	}

	public Task<IDataView> Query(string query, IReadOnlyDictionary<string, object> parameters)
	{
		throw new InvalidOperationException("UnavailableAzureTool.Query should not be called.");
	}

	public static IAzureTool Create(RunSpace arg)
	{
		return new UnavailableAzureTool();
	}
}
