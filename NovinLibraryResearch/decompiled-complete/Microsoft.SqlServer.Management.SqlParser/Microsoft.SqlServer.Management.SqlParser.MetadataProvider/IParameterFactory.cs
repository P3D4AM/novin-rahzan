using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IParameterFactory
{
	IScalarParameter CreateScalarParameter(string name, IScalarDataType dataType);

	IScalarParameter CreateScalarParameter(string name, IScalarDataType dataType, bool isOutput, string defaultValue);

	ITableParameter CreateTableParameter(string name, ITableDataType dataType);

	ICursorParameter CreateCursorParameter(string name);
}
