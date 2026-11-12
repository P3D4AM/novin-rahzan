using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IColumnFactory
{
	IColumn Null { get; }

	IMutableColumn Create(ITabular parent, string name);

	IColumn CreateSimpleColumn(ITabular parent, string name, IScalarDataType dataType, bool nullable);

	IColumn CreateAnonymousColumn(IScalarDataType dataType, bool nullable);

	IColumn CreateScalarAlias(IScalar aliasedScalar, string alias);

	IColumn CreateScalarAlias(ITabular parent, IScalar aliasedScalar, string alias);

	IColumn CreateColumnAlias(IColumn aliasedColumn, string alias);

	IColumn CreateColumnAlias(ITabular parent, IColumn aliasedColumn, string alias);
}
