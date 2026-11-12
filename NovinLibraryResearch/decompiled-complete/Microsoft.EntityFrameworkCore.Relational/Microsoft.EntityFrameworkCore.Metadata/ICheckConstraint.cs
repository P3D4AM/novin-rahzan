namespace Microsoft.EntityFrameworkCore.Metadata;

public interface ICheckConstraint
{
	string Name { get; }

	IEntityType EntityType { get; }

	string Sql { get; }
}
