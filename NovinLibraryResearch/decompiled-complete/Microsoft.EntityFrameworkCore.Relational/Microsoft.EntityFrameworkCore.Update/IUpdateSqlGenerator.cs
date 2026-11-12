using System.Text;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Update;

public interface IUpdateSqlGenerator
{
	string GenerateNextSequenceValueOperation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema);

	void AppendNextSequenceValueOperation([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema);

	void AppendBatchHeader([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder);

	ResultSetMapping AppendDeleteOperation([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] ModificationCommand command, int commandPosition);

	ResultSetMapping AppendInsertOperation([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] ModificationCommand command, int commandPosition);

	ResultSetMapping AppendUpdateOperation([JetBrains.Annotations.NotNull] StringBuilder commandStringBuilder, [JetBrains.Annotations.NotNull] ModificationCommand command, int commandPosition);
}
