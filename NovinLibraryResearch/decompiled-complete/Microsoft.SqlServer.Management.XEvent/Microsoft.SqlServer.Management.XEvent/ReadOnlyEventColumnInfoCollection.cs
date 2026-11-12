using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class ReadOnlyEventColumnInfoCollection : SfcCollatedDictionaryCollection<ReadOnlyEventColumnInfo, ReadOnlyEventColumnInfo.Key, EventInfo>
{
	public ReadOnlyEventColumnInfo this[string name] => base[new ReadOnlyEventColumnInfo.Key(name)];

	internal ReadOnlyEventColumnInfoCollection(EventInfo parent)
		: base(parent, parent.Parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new ReadOnlyEventColumnInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return ReadOnlyEventColumnInfo.GetObjectFactory();
	}
}
