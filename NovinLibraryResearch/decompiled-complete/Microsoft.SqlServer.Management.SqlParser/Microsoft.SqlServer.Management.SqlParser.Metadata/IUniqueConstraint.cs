namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("Type", ConstraintType.Unique)]
public interface IUniqueConstraint : IUniqueConstraintBase, IConstraint, IMetadataObject
{
}
