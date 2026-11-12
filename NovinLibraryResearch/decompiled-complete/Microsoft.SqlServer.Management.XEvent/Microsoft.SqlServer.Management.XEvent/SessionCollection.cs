using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class SessionCollection : SfcCollatedDictionaryCollection<Session, Session.Key, BaseXEStore>
{
	public Session this[string name] => base[new Session.Key(name)];

	internal SessionCollection(BaseXEStore parent)
		: base(parent, parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new Session.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return Session.GetObjectFactory();
	}
}
