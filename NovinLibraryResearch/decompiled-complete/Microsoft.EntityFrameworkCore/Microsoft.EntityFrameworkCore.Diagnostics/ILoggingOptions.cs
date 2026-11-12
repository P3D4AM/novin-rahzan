using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public interface ILoggingOptions : ISingletonOptions
{
	bool IsSensitiveDataLoggingEnabled { get; }

	bool IsSensitiveDataLoggingWarned { get; set; }

	WarningsConfiguration WarningsConfiguration { get; }
}
