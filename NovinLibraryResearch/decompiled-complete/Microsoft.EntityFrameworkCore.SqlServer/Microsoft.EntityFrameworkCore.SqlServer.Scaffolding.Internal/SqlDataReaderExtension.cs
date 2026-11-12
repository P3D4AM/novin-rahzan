using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;

public static class SqlDataReaderExtension
{
	public static T GetValueOrDefault<T>([JetBrains.Annotations.NotNull] this DbDataReader reader, [JetBrains.Annotations.NotNull] string name)
	{
		int ordinal = reader.GetOrdinal(name);
		if (!reader.IsDBNull(ordinal))
		{
			return reader.GetFieldValue<T>(ordinal);
		}
		return default(T);
	}

	public static T GetValueOrDefault<T>([JetBrains.Annotations.NotNull] this DbDataRecord record, [JetBrains.Annotations.NotNull] string name)
	{
		int ordinal = record.GetOrdinal(name);
		if (!record.IsDBNull(ordinal))
		{
			return (T)record.GetValue(ordinal);
		}
		return default(T);
	}
}
