using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal class Duplicator : IDuplicator<ITable, IMutableTable>, IDuplicator<IView, IMutableView>
{
	private readonly IMetadataFactory metadataFactory;

	public Duplicator(IMetadataFactory metadataFactory)
	{
		this.metadataFactory = metadataFactory;
	}

	IMutableTable IDuplicator<ITable, IMutableTable>.Duplicate(ITable obj)
	{
		return metadataFactory.Tabular.CreateTable(obj);
	}

	IMutableView IDuplicator<IView, IMutableView>.Duplicate(IView obj)
	{
		return metadataFactory.Tabular.CreateView(obj);
	}
}
