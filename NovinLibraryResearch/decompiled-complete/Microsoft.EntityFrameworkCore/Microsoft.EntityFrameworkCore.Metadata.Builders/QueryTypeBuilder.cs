using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

[Obsolete("Use EntityTypeBuilder instead")]
public class QueryTypeBuilder<TQuery> : EntityTypeBuilder<TQuery> where TQuery : class
{
	[EntityFrameworkInternal]
	public QueryTypeBuilder([JetBrains.Annotations.NotNull] IMutableEntityType entityType)
		: base(entityType)
	{
	}
}
