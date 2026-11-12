using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Collector;

public sealed class CollectionSetExtender : SfcObjectExtender<CollectionSet>
{
	private static TraceContext tc = TraceContext.GetTraceContext("DC_OM", "Microsoft.SqlServer.Management.Collector.CollectionSetExtender");

	public CollectionSetExtender()
	{
		using (tc.GetMethodContext("CollectionSetExtender"))
		{
		}
	}

	public CollectionSetExtender(CollectionSet collectionSet)
		: base(collectionSet)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("CollectionSetExtender");
		methodTraceContext.TraceParameterIn("collectionSet", (collectionSet != null) ? collectionSet.Name : "(null)");
	}

	public CollectionSetExtender(CollectorConfigStore collectorConfigStore, string name)
		: base(new CollectionSet(collectorConfigStore, name))
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("CollectionSetExtender");
		methodTraceContext.TraceParameterIn("name", name);
	}
}
