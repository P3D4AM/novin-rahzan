using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public class DatabaseModelFactoryOptions
{
	public virtual IEnumerable<string> Tables { get; }

	public virtual IEnumerable<string> Schemas { get; }

	public DatabaseModelFactoryOptions([JetBrains.Annotations.CanBeNull] IEnumerable<string> tables = null, [JetBrains.Annotations.CanBeNull] IEnumerable<string> schemas = null)
	{
		Tables = tables ?? Enumerable.Empty<string>();
		Schemas = schemas ?? Enumerable.Empty<string>();
	}
}
