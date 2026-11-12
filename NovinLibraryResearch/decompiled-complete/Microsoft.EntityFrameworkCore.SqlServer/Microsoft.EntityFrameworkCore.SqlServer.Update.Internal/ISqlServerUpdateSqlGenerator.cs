using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;

public interface ISqlServerUpdateSqlGenerator : IUpdateSqlGenerator
{
	ResultSetMapping AppendBulkInsertOperation([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition);
}
