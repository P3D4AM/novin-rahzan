using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalCommandBuilder
{
	IReadOnlyList<IRelationalParameter> Parameters { get; }

	IRelationalTypeMappingSource TypeMappingSource { get; }

	int CommandTextLength { get; }

	IRelationalCommandBuilder AddParameter([JetBrains.Annotations.NotNull] IRelationalParameter parameter);

	IRelationalCommand Build();

	IRelationalCommandBuilder Append([JetBrains.Annotations.NotNull] object value);

	IRelationalCommandBuilder AppendLine();

	IRelationalCommandBuilder IncrementIndent();

	IRelationalCommandBuilder DecrementIndent();
}
