namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IBuiltInFunction : IScalarFunction, IFunction, IMetadataObject, IFunctionModuleBase, IScalar
{
	[ReadOnly]
	int MinNumberOfArgs { get; }

	[ReadOnly]
	int MaxNumberOfArgs { get; }

	[ReadOnly]
	bool IsVarArg { get; }

	[ReadOnly]
	bool IsDatePartFunction { get; }

	[ReadOnly]
	bool IsGlobalVariable { get; }
}
