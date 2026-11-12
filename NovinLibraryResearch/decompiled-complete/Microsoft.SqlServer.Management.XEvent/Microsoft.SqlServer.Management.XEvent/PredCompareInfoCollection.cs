using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class PredCompareInfoCollection : SfcCollatedDictionaryCollection<PredCompareInfo, PredCompareInfo.Key, Package>, IXEObjectInfoCollection<PredCompareInfo>
{
	public PredCompareInfo this[string name] => base[new PredCompareInfo.Key(name)];

	internal PredCompareInfoCollection(Package parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new PredCompareInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return PredCompareInfo.GetObjectFactory();
	}
}
