using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class RelationalPropertyExtensions
{
	public static string FormatColumns([JetBrains.Annotations.NotNull] this IEnumerable<IProperty> properties)
	{
		return "{" + string.Join(", ", properties.Select((IProperty p) => "'" + p.GetColumnName() + "'")) + "}";
	}
}
