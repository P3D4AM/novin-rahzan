using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Data.AggregateFuncs;

internal class StringJoinFunc : IAggregateFunc
{
	public const string DefaultSeparator = ", ";

	[JsonProperty("trim", DefaultValueHandling = DefaultValueHandling.Populate)]
	[DefaultValue(true)]
	public bool Trim { get; set; } = true;


	[JsonProperty("separator", NullValueHandling = NullValueHandling.Ignore)]
	public string Separator { get; set; } = ", ";


	public object GetAggregate(IEnumerable<object> sequence)
	{
		IEnumerable<string> enumerable = from s in sequence.Select(Utils.Normalize<string>)
			where s != null
			select s;
		if (Trim)
		{
			enumerable = enumerable.Select((string s) => s.Trim());
		}
		return string.Join(Separator, enumerable);
	}
}
