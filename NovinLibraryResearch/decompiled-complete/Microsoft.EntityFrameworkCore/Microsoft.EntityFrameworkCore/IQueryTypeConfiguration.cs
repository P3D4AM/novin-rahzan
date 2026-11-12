using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore;

[Obsolete("Use IEntityTypeConfiguration<TEntity> instead")]
public interface IQueryTypeConfiguration<TQuery> where TQuery : class
{
	void Configure([JetBrains.Annotations.NotNull] QueryTypeBuilder<TQuery> builder);
}
