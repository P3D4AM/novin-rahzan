namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

public interface IReadableConventionContext : IConventionContext
{
	bool ShouldStopProcessing();
}
