using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public static class DbContextTransactionExtensions
{
	public static DbTransaction GetDbTransaction([JetBrains.Annotations.NotNull] this IDbContextTransaction dbContextTransaction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dbContextTransaction, "dbContextTransaction");
		return ((dbContextTransaction as IInfrastructure<DbTransaction>) ?? throw new InvalidOperationException(RelationalStrings.RelationalNotInUse)).GetInfrastructure();
	}
}
