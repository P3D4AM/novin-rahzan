using System.Globalization;
using Microsoft.SqlServer.Diagnostics.STrace;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Collector;

public sealed class CollectionItemCollection : SfcCollatedDictionaryCollection<CollectionItem, CollectionItem.Key, CollectionSet>
{
	private static TraceContext tc = TraceContext.GetTraceContext("DC_OM", "Microsoft.SqlServer.Management.Collector.CollectionItemCollection");

	public CollectionItem this[string name] => base[new CollectionItem.Key(name)];

	public CollectionItemCollection(CollectionSet parent)
		: base(parent)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("CollectionItemCollection");
		methodTraceContext.TraceParameterIn("parent", (parent != null) ? parent.Name : "(null)");
		base.CultureInfo = CultureInfo.CurrentCulture;
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return CollectionItem.GetObjectFactory();
	}

	public void AddCollectionItem(CollectionItem item)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("AddCollectionItem");
		if (item == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionItemCannotBeNull);
		}
		if (item.Name == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionItemHasNoName);
		}
		if (SfcObjectState.Pending != item.State)
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionItemAlreadyCreatedBefore(item.Name));
		}
		((ISfcCollection)this).Initialized = true;
		methodTraceContext.TraceParameterIn("item", item.Name);
		base.Add(item);
	}

	public void RemoveCollectionItem(CollectionItem item)
	{
		using MethodTraceContext methodTraceContext = tc.GetMethodContext("RemoveCollectionItem");
		if (item == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionItemCannotBeNull);
		}
		if (item.Name == null)
		{
			throw new CollectorException(ExceptionTemplatesSR.CollectionItemHasNoName);
		}
		methodTraceContext.TraceParameterIn("item", item.Name);
		Remove(item);
	}
}
