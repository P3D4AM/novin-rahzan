using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace AccSqlServerPersistence.ModelClass;

public static class ContextExtention
{
	public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		return (DbContext)((object)dbSet).GetType().GetTypeInfo().GetField("_context", BindingFlags.Instance | BindingFlags.NonPublic)
			.GetValue(dbSet);
	}
}
