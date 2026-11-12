using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class TargetCollection : SfcCollatedDictionaryCollection<Target, Target.Key, Session>
{
	public Target this[string name] => base[new Target.Key(name)];

	internal TargetCollection(Session parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new Target.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return Target.GetObjectFactory();
	}

	public void AppendAlterScripts(StringBuilder addScript, StringBuilder dropScript)
	{
		XEUtils.AppendAlterScripts(addScript, dropScript, this, base.Parent);
	}
}
