namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionCheckConstraint : ICheckConstraint
{
	new IConventionEntityType EntityType { get; }

	ConfigurationSource GetConfigurationSource();
}
