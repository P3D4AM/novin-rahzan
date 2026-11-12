using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IDbContextOptionsExtension
{
	DbContextOptionsExtensionInfo Info { get; }

	void ApplyServices([JetBrains.Annotations.NotNull] IServiceCollection services);

	void Validate([JetBrains.Annotations.NotNull] IDbContextOptions options);
}
