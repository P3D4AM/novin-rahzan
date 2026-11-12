using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalParameter
{
	string InvariantName { get; }

	void AddDbParameter([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] object value);

	void AddDbParameter([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] IReadOnlyDictionary<string, object> parameterValues);
}
