using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRawSqlCommandBuilder
{
	IRelationalCommand Build([JetBrains.Annotations.NotNull] string sql);

	RawSqlCommand Build([JetBrains.Annotations.NotNull] string sql, [JetBrains.Annotations.NotNull] IEnumerable<object> parameters);
}
