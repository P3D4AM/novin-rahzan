using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Configuration;
using Microsoft.SqlServer.Management.Assessment.Configuration.Json;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal abstract class Probe : IProbe
{
	public SqlObjectLocatorPattern Target { get; set; }

	public string FamilyName { get; set; }

	[JsonProperty("transform")]
	public IDataMorph Morph { get; set; }

	[JsonProperty("runFor")]
	[JsonConverter(typeof(ConcreteConverter<CharacteristicCollection>))]
	public ICharacteristicCollection Subject { get; set; }

	[JsonProperty("requires")]
	[JsonConverter(typeof(ConcreteConverter<CharacteristicCollection>))]
	public ICharacteristicCollection Requirements { get; set; }

	public abstract IEnumerable<Type> ToolTypes { get; }

	public async Task<IDataView> GetDataAsync(RunSpace runSpace, IReadOnlyDictionary<string, object> parameters)
	{
		IDataView dataView = await GetDataImpl(runSpace, parameters).ConfigureAwait(continueOnCapturedContext: false);
		if (Morph != null)
		{
			dataView = Morph.ApplyTo(dataView);
		}
		return dataView;
	}

	protected abstract Task<IDataView> GetDataImpl(RunSpace runSpace, IReadOnlyDictionary<string, object> parameters);
}
