using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore;

public interface IEntityTypeConfiguration<TEntity> where TEntity : class
{
	void Configure(EntityTypeBuilder<TEntity> builder);
}
