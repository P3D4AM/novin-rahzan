using System;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class ActionCollection : SfcCollatedDictionaryCollection<Action, Action.Key, Event>
{
	public Action this[string name] => base[new Action.Key(name)];

	internal ActionCollection(Event parent)
		: base(parent, parent.Parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new Action.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return Action.GetObjectFactory();
	}

	protected override bool AddShadow(Action obj)
	{
		try
		{
			return base.AddShadow(obj);
		}
		catch (ArgumentException innerException)
		{
			throw new XEventException(ExceptionTemplates.ActionNameNotUnique(obj.Name), innerException);
		}
	}
}
