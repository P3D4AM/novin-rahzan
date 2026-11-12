namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IScalarFunction : IFunction, IMetadataObject, IFunctionModuleBase, IScalar
{
	[Abstract]
	bool IsAggregateFunction { get; }
}
