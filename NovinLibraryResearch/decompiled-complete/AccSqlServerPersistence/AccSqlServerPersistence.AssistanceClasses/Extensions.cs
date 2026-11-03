using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;

namespace AccSqlServerPersistence.AssistanceClasses;

public static class Extensions
{
	public static string GetSqlTableName<T>(this BaseModelContext context) where T : class, new()
	{
		return RelationalEntityTypeExtensions.GetTableName(ModelExtensions.FindEntityType(((DbContext)context).Model, typeof(T)));
	}
}
