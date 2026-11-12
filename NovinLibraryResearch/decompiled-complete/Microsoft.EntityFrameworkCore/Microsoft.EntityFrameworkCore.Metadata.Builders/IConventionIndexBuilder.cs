namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionIndexBuilder : IConventionAnnotatableBuilder
{
	new IConventionIndex Metadata { get; }

	IConventionIndexBuilder IsUnique(bool? unique, bool fromDataAnnotation = false);

	bool CanSetIsUnique(bool? unique, bool fromDataAnnotation = false);
}
