using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class DataEventColumnInfoCollection : SfcCollatedDictionaryCollection<DataEventColumnInfo, DataEventColumnInfo.Key, EventInfo>
{
	public DataEventColumnInfo this[string name] => base[new DataEventColumnInfo.Key(name)];

	internal DataEventColumnInfoCollection(EventInfo parent)
		: base(parent, parent.Parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new DataEventColumnInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return DataEventColumnInfo.GetObjectFactory();
	}
}
