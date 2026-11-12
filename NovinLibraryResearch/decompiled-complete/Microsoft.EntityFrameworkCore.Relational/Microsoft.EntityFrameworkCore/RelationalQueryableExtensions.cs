using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalQueryableExtensions
{
	internal static readonly MethodInfo FromSqlOnQueryableMethodInfo = typeof(RelationalQueryableExtensions).GetTypeInfo().GetDeclaredMethods("FromSqlOnQueryable").Single();

	[JetBrains.Annotations.StringFormatMethod("sql")]
	[Obsolete("For returning objects from SQL queries using plain strings, use FromSqlRaw instead. For returning objects from SQL queries using interpolated string syntax to create parameters, use FromSqlInterpolated instead. Call either new method directly on the DbSet at the root of the query.", true)]
	public static IQueryable<TEntity> FromSql<TEntity>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source, [NotParameterized] RawSqlString sql, [JetBrains.Annotations.NotNull] params object[] parameters) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql.Format, "sql");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameters, "parameters");
		return source.Provider.CreateQuery<TEntity>(Expression.Call(null, FromSqlOnQueryableMethodInfo.MakeGenericMethod(typeof(TEntity)), source.Expression, Expression.Constant(sql.Format), Expression.Constant(parameters)));
	}

	[Obsolete("For returning objects from SQL queries using plain strings, use FromSqlRaw instead. For returning objects from SQL queries using interpolated string syntax to create parameters, use FromSqlInterpolated instead. Call either new method directly on the DbSet at the root of the query.", true)]
	public static IQueryable<TEntity> FromSql<TEntity>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source, [JetBrains.Annotations.NotNull][NotParameterized] FormattableString sql) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sql, "sql");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql.Format, "source");
		return source.Provider.CreateQuery<TEntity>(Expression.Call(null, FromSqlOnQueryableMethodInfo.MakeGenericMethod(typeof(TEntity)), source.Expression, Expression.Constant(sql.Format), Expression.Constant(sql.GetArguments())));
	}

	[JetBrains.Annotations.StringFormatMethod("sql")]
	public static IQueryable<TEntity> FromSqlRaw<TEntity>([JetBrains.Annotations.NotNull] this DbSet<TEntity> source, [JetBrains.Annotations.NotNull][NotParameterized] string sql, [JetBrains.Annotations.NotNull] params object[] parameters) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql, "sql");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parameters, "parameters");
		return ((IQueryable)source).Provider.CreateQuery<TEntity>(Expression.Call(null, FromSqlOnQueryableMethodInfo.MakeGenericMethod(typeof(TEntity)), ((IQueryable)source).Expression, Expression.Constant(sql), Expression.Constant(parameters)));
	}

	public static IQueryable<TEntity> FromSqlInterpolated<TEntity>([JetBrains.Annotations.NotNull] this DbSet<TEntity> source, [JetBrains.Annotations.NotNull][NotParameterized] FormattableString sql) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(source, "source");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sql, "sql");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql.Format, "source");
		return ((IQueryable)source).Provider.CreateQuery<TEntity>(Expression.Call(null, FromSqlOnQueryableMethodInfo.MakeGenericMethod(typeof(TEntity)), ((IQueryable)source).Expression, Expression.Constant(sql.Format), Expression.Constant(sql.GetArguments())));
	}

	internal static IQueryable<TEntity> FromSqlOnQueryable<TEntity>([JetBrains.Annotations.NotNull] this IQueryable<TEntity> source, [NotParameterized] string sql, [JetBrains.Annotations.NotNull] params object[] parameters) where TEntity : class
	{
		throw new NotImplementedException();
	}
}
