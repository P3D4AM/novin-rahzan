using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Data.AggregateFuncs;

internal class CountFunc : IAggregateFunc
{
	[JsonProperty("distinct", DefaultValueHandling = DefaultValueHandling.Populate)]
	[DefaultValue(true)]
	public bool Distinct { get; set; } = true;


	[JsonProperty("notNull", DefaultValueHandling = DefaultValueHandling.Populate)]
	[DefaultValue(true)]
	public bool NotNull { get; set; } = true;


	public object GetAggregate(IEnumerable<object> sequence)
	{
		IEnumerable<object> source = (NotNull ? sequence.Where((object obj) => obj != null) : sequence);
		if (Distinct)
		{
			source = source.Distinct();
		}
		return source.Count();
	}
}
