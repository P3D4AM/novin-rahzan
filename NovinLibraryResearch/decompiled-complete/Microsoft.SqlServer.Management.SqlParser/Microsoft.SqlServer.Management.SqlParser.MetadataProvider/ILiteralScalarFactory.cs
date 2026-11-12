using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ILiteralScalarFactory
{
	IScalarExpression Binary { get; }

	IScalarExpression Default { get; }

	IScalarExpression Identifier { get; }

	IScalarExpression Image { get; }

	IScalarExpression Integer { get; }

	IScalarExpression Money { get; }

	IScalarExpression Null { get; }

	IScalarExpression Numeric { get; }

	IScalarExpression Real { get; }

	IScalarExpression String { get; }

	IScalarExpression UnicodeString { get; }
}
