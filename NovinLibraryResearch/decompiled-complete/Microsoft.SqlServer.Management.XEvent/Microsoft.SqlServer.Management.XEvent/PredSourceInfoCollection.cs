using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class PredSourceInfoCollection : SfcCollatedDictionaryCollection<PredSourceInfo, PredSourceInfo.Key, Package>, IXEObjectInfoCollection<PredSourceInfo>
{
	public PredSourceInfo this[string name] => base[new PredSourceInfo.Key(name)];

	internal PredSourceInfoCollection(Package parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new PredSourceInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return PredSourceInfo.GetObjectFactory();
	}
}
