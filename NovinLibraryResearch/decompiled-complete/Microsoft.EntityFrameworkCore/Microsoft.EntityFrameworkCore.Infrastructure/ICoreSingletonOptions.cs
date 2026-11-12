namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface ICoreSingletonOptions : ISingletonOptions
{
	bool AreDetailedErrorsEnabled { get; }
}
