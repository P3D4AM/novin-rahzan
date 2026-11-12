using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IScalarFactory
{
	ILiteralScalarFactory Literal { get; }

	IScalar Null { get; }

	IScalarExpression CreateScalarExpression(IScalarDataType dataType, bool nullable);
}
