using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerDatabaseFacadeExtensions
{
	public static bool IsSqlServer([JetBrains.Annotations.NotNull] this DatabaseFacade database)
	{
		return database.ProviderName.Equals(typeof(SqlServerOptionsExtension).GetTypeInfo().Assembly.GetName().Name, StringComparison.Ordinal);
	}
}
