using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class TargetFieldCollection : SfcCollatedDictionaryCollection<TargetField, TargetField.Key, Target>
{
	public TargetField this[string name] => base[new TargetField.Key(name)];

	internal TargetFieldCollection(Target parent)
		: base(parent, parent.Parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new TargetField.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return TargetField.GetObjectFactory();
	}
}
