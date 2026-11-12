using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class TypeInfoCollection : SfcCollatedDictionaryCollection<TypeInfo, TypeInfo.Key, Package>, IXEObjectInfoCollection<TypeInfo>
{
	public TypeInfo this[string name] => base[new TypeInfo.Key(name)];

	internal TypeInfoCollection(Package parent)
		: base(parent, parent.Parent.GetComparer())
	{
	}

	public bool Contains(string name)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Contains");
		methodTraceContext.TraceParameterIn("name", name);
		return Contains(new TypeInfo.Key(name));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return TypeInfo.GetObjectFactory();
	}
}
