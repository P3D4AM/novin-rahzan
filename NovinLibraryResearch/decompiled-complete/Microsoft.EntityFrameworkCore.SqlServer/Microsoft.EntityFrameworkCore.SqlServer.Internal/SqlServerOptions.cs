using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.SqlServer.Internal;

public class SqlServerOptions : ISqlServerOptions, ISingletonOptions
{
	public virtual bool RowNumberPagingEnabled { get; private set; }

	public virtual void Initialize(IDbContextOptions options)
	{
		SqlServerOptionsExtension sqlServerOptionsExtension = options.FindExtension<SqlServerOptionsExtension>() ?? new SqlServerOptionsExtension();
		RowNumberPagingEnabled = sqlServerOptionsExtension.RowNumberPaging.GetValueOrDefault();
	}

	public virtual void Validate(IDbContextOptions options)
	{
		SqlServerOptionsExtension sqlServerOptionsExtension = options.FindExtension<SqlServerOptionsExtension>() ?? new SqlServerOptionsExtension();
		if (RowNumberPagingEnabled != sqlServerOptionsExtension.RowNumberPaging.GetValueOrDefault())
		{
			throw new InvalidOperationException(CoreStrings.SingletonOptionChanged("UseRowNumberForPaging", "UseInternalServiceProvider"));
		}
	}
}
