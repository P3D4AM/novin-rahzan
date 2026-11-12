using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IVariableFactory
{
	IScalarVariable CreateScalarVariable(string name, IScalarDataType dataType);

	ITableVariable CreateTableVariable(string name, ITableDataType dataType);

	ICursorVariable CreateCursorVariable(string name);
}
