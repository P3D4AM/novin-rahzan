using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IParameterValues
{
	IReadOnlyDictionary<string, object> ParameterValues { get; }

	void AddParameter([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value);
}
