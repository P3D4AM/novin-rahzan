using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class ActionInfoCollection : SfcCollatedDictionaryCollection<ActionInfo, ActionInfo.Key, Package>, IXEObjectInfoCollection<ActionInfo>
{
	public ActionInfo this[string name] => base[new ActionInfo.Key(name)];

	internal ActionInfoCollection(Package parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new ActionInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return ActionInfo.GetObjectFactory();
	}
}
