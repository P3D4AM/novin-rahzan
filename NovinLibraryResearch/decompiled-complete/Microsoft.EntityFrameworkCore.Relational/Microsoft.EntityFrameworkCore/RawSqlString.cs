using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore;

[Obsolete("For the execution of SQL queries using plain strings, use FromSqlRaw, ExecuteSqlRaw, or ExecuteSqlRawAsync instead. For the execution of SQL queries using interpolated string syntax to create parameters, use FromSqlInterpolated, ExecuteSqlInterpolated, or ExecuteSqlInterpolatedAsync instead.")]
public readonly struct RawSqlString
{
	public string Format { get; }

	public static implicit operator RawSqlString([JetBrains.Annotations.NotNull] string s)
	{
		return new RawSqlString(s);
	}

	public static implicit operator RawSqlString([JetBrains.Annotations.NotNull] FormattableString fs)
	{
		return default(RawSqlString);
	}

	public RawSqlString([JetBrains.Annotations.NotNull] string s)
	{
		Format = s;
	}
}
