using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class TargetInfoCollection : SfcCollatedDictionaryCollection<TargetInfo, TargetInfo.Key, Package>, IXEObjectInfoCollection<TargetInfo>
{
	public TargetInfo this[string name] => base[new TargetInfo.Key(name)];

	internal TargetInfoCollection(Package parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new TargetInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return TargetInfo.GetObjectFactory();
	}
}
