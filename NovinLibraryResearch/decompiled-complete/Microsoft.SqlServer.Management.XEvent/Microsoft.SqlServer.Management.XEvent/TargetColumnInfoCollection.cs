using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class TargetColumnInfoCollection : SfcCollatedDictionaryCollection<TargetColumnInfo, TargetColumnInfo.Key, TargetInfo>
{
	public TargetColumnInfo this[string name] => base[new TargetColumnInfo.Key(name)];

	internal TargetColumnInfoCollection(TargetInfo parent)
		: base(parent, parent.Parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new TargetColumnInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return TargetColumnInfo.GetObjectFactory();
	}
}
