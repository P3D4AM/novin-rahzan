namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Constant("IsAggregateFunction", false)]
public interface IUdtMethod : IUdtMember, IScalar, IMetadataObject, IScalarFunction, IFunction, IFunctionModuleBase
{
}
