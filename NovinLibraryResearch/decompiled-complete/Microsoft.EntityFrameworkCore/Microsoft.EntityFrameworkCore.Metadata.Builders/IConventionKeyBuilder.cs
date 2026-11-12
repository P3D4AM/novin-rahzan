namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionKeyBuilder : IConventionAnnotatableBuilder
{
	new IConventionKey Metadata { get; }
}
