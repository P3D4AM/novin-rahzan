using System.Globalization;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Collector;

public sealed class CollectionSetCollection : SfcCollatedDictionaryCollection<CollectionSet, CollectionSet.Key, CollectorConfigStore>
{
	private static TraceContext tc = TraceContext.GetTraceContext("DC_OM", "Microsoft.SqlServer.Management.Collector.CollectionSetCollection");

	public CollectionSet this[string name] => base[new CollectionSet.Key(name)];

	public CollectionSetCollection(CollectorConfigStore parent)
		: base(parent)
	{
		using (tc.GetMethodContext("CollectionSetCollection"))
		{
			base.CultureInfo = CultureInfo.CurrentCulture;
		}
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return CollectionSet.GetObjectFactory();
	}
}
