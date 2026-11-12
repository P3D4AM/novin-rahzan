using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

public interface ISqlServerOptions : ISingletonOptions
{
	bool RowNumberPagingEnabled { get; }
}
