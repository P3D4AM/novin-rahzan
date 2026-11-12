namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("Type", ConstraintType.PrimaryKey)]
public interface IPrimaryKeyConstraint : IUniqueConstraintBase, IConstraint, IMetadataObject
{
}
